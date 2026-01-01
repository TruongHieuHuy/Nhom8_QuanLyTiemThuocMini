using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace PharmacyManagement.Services
{
    public class VnPaySettings
    {
        public string TmnCode { get; set; }
        public string HashSecret { get; set; }
        public string BaseUrl { get; set; }
        public string ReturnUrl { get; set; }
        public string FrontendReturnUrl { get; set; }
        public string Version { get; set; } = "2.1.0";
    }

    public interface IVnPayService
    {
        string CreatePaymentUrl(string ipAddress, string txnRef, long amountVndTimes100, string orderInfo);
        bool ValidateSignature(IDictionary<string, string> vnpParams, string secureHash);
        VnPaySettings GetSettings();
    }

    public class VnPayService : IVnPayService
    {
        private readonly VnPaySettings _settings;

        public VnPayService(IConfiguration configuration)
        {
            _settings = new VnPaySettings();
            configuration.GetSection("VnPay").Bind(_settings);
        }

        public VnPaySettings GetSettings() => _settings;

        public string CreatePaymentUrl(string ipAddress, string txnRef, long amountVndTimes100, string orderInfo)
        {
            if (string.IsNullOrWhiteSpace(_settings.BaseUrl) || string.IsNullOrWhiteSpace(_settings.TmnCode) || string.IsNullOrWhiteSpace(_settings.HashSecret))
                throw new InvalidOperationException("Chưa cấu hình VNPay trong appsettings.json (VnPay:TmnCode/HashSecret/BaseUrl)");

            var vnpParams = new SortedDictionary<string, string>();

            vnpParams["vnp_Version"] = _settings.Version ?? "2.1.0";
            vnpParams["vnp_Command"] = "pay";
            vnpParams["vnp_TmnCode"] = _settings.TmnCode;
            vnpParams["vnp_Amount"] = amountVndTimes100.ToString();
            vnpParams["vnp_CurrCode"] = "VND";
            vnpParams["vnp_TxnRef"] = txnRef;
            vnpParams["vnp_OrderInfo"] = orderInfo;
            vnpParams["vnp_OrderType"] = "other";
            vnpParams["vnp_Locale"] = "vn";
            vnpParams["vnp_ReturnUrl"] = _settings.ReturnUrl;
            vnpParams["vnp_IpAddr"] = string.IsNullOrWhiteSpace(ipAddress) ? "127.0.0.1" : ipAddress;
            vnpParams["vnp_CreateDate"] = DateTime.Now.ToString("yyyyMMddHHmmss");
            vnpParams["vnp_ExpireDate"] = DateTime.Now.AddMinutes(15).ToString("yyyyMMddHHmmss");

            var queryString = BuildQueryString(vnpParams, urlEncode: true);
            var hashData = BuildQueryString(vnpParams, urlEncode: true);
            var secureHash = HmacSha512(_settings.HashSecret, hashData);

            return _settings.BaseUrl + "?" + queryString + "&vnp_SecureHash=" + secureHash;
        }

        public bool ValidateSignature(IDictionary<string, string> vnpParams, string secureHash)
        {
            if (vnpParams == null || vnpParams.Count == 0 || string.IsNullOrWhiteSpace(secureHash))
                return false;

            var filtered = new SortedDictionary<string, string>(StringComparer.Ordinal);
            foreach (var kv in vnpParams)
            {
                if (string.IsNullOrWhiteSpace(kv.Value)) continue;
                if (kv.Key == "vnp_SecureHash" || kv.Key == "vnp_SecureHashType") continue;
                filtered[kv.Key] = kv.Value;
            }

            var hashData = BuildQueryString(filtered, urlEncode: true);
            var calc = HmacSha512(_settings.HashSecret, hashData);
            return string.Equals(calc, secureHash, StringComparison.OrdinalIgnoreCase);
        }

        private static string BuildQueryString(IEnumerable<KeyValuePair<string, string>> data, bool urlEncode)
        {
            return string.Join("&", data
                .Where(kv => !string.IsNullOrWhiteSpace(kv.Value))
                .Select(kv =>
                {
                    var k = urlEncode ? WebUtility.UrlEncode(kv.Key) : kv.Key;
                    var v = urlEncode ? WebUtility.UrlEncode(kv.Value) : kv.Value;
                    return $"{k}={v}";
                }));
        }

        private static string HmacSha512(string key, string inputData)
        {
            using var hmac = new HMACSHA512(Encoding.UTF8.GetBytes(key));
            var hashBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(inputData));
            var sb = new StringBuilder(hashBytes.Length * 2);
            foreach (var b in hashBytes)
                sb.Append(b.ToString("x2"));
            return sb.ToString();
        }
    }
}
