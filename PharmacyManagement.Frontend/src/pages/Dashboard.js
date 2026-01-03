import React, { useState, useEffect } from 'react';
import { Card, Row, Col, Statistic, Table, Tag, message, Select, DatePicker } from 'antd';
import { 
  ShoppingCartOutlined, 
  UserOutlined, 
  MedicineBoxOutlined, 
  DollarOutlined, 
  WarningOutlined,
  ArrowUpOutlined,
  ArrowDownOutlined
} from '@ant-design/icons';
import { LineChart, Line, XAxis, YAxis, CartesianGrid, Tooltip, Legend, ResponsiveContainer } from 'recharts';
import axios from '../utils/axiosConfig';

const { Option } = Select;
const { RangePicker } = DatePicker;

export default function Dashboard() {
  const [period, setPeriod] = useState('day'); // Chu kỳ: day, week, month, year, custom
  const [dateRange, setDateRange] = useState(null); // [startDate, endDate]
  const [stats, setStats] = useState({
    totalRevenue: 0,
    revenueGrowth: 0,
    totalOrders: 0,
    ordersGrowth: 0,
    totalCustomers: 0,
    customersGrowth: 0,
    lowStock: 0
  });

  const [medicines, setMedicines] = useState([]); // Để hiện bảng thuốc sắp hết
  const [revenueChart, setRevenueChart] = useState([]); // Doanh thu theo period

  useEffect(() => {
    fetchDashboardData();
    // Kiểm tra và tạo thông báo cho thuốc sắp hết hàng
    checkLowStockNotifications();
    
    // Kiểm tra định kỳ mỗi 2 phút
    const interval = setInterval(checkLowStockNotifications, 2 * 60 * 1000);
    return () => clearInterval(interval);
  }, [period, dateRange]);

  const checkLowStockNotifications = async () => {
    try {
      await axios.post('/Notifications/check-low-stock');
      console.log('Đã kiểm tra thông báo tồn kho');
    } catch (error) {
      console.error('Error checking low stock:', error);
    }
  };

  const fetchDashboardData = async () => {
    try {
      // Nếu chọn custom mà chưa chọn dateRange thì không gọi API
      if (period === 'custom' && (!dateRange || dateRange.length !== 2)) {
        return;
      }

      // 1. Gọi API Thống kê với period hoặc custom date range
      let statsUrl = `/Dashboard/stats?period=${period}`;
      if (period === 'custom' && dateRange && dateRange.length === 2) {
        const startDate = dateRange[0].format('YYYY-MM-DD');
        const endDate = dateRange[1].format('YYYY-MM-DD');
        statsUrl = `/Dashboard/stats?period=custom&startDateStr=${startDate}&endDateStr=${endDate}`;
      }
      const statsRes = await axios.get(statsUrl);
      setStats(statsRes.data);

      // 2. Gọi API lấy danh sách thuốc để lọc thuốc sắp hết
      const medRes = await axios.get('/Medicines');
      setMedicines(medRes.data.filter(m => m.currentStock <= m.minStockLevel));

      // 3. Lấy dữ liệu biểu đồ theo period
      await fetchChartData();

    } catch (error) {
      console.error("Lỗi tải dữ liệu dashboard:", error);
      message.error("Không tải được dữ liệu bảng điều khiển");
    }
  };

  const fetchChartData = async () => {
    try {
      let dates = [];
      const now = new Date();

      switch (period) {
        case 'custom':
          if (dateRange && dateRange.length === 2) {
            const start = dateRange[0].toDate();
            const end = dateRange[1].toDate();
            const diffDays = Math.ceil((end - start) / (1000 * 60 * 60 * 24));
            
            if (diffDays <= 14) {
              // Hiển thị từng ngày cho khoảng < 14 ngày
              dates = Array.from({ length: diffDays + 1 }, (_, i) => {
                const date = new Date(start);
                date.setDate(start.getDate() + i);
                return date.toISOString().split('T')[0];
              });
            } else if (diffDays <= 90) {
              // Hiển thị theo tuần cho 14-90 ngày
              const weeks = Math.ceil(diffDays / 7);
              dates = Array.from({ length: weeks }, (_, i) => {
                const date = new Date(start);
                date.setDate(start.getDate() + (i * 7));
                return date.toISOString().split('T')[0];
              });
            } else {
              // Hiển thị theo tháng cho > 90 ngày
              const months = Math.ceil(diffDays / 30);
              dates = Array.from({ length: months }, (_, i) => {
                const date = new Date(start);
                date.setMonth(start.getMonth() + i);
                return date.toISOString().split('T')[0];
              });
            }
          }
          break;

        case 'week':
          // 7 ngày gần nhất
          dates = Array.from({ length: 7 }, (_, i) => {
            const date = new Date();
            date.setDate(date.getDate() - (6 - i));
            return date.toISOString().split('T')[0];
          });
          break;

        case 'month':
          // 30 ngày gần nhất
          dates = Array.from({ length: 30 }, (_, i) => {
            const date = new Date();
            date.setDate(date.getDate() - (29 - i));
            return date.toISOString().split('T')[0];
          });
          break;

        case 'year':
          // 12 tháng trong năm hiện tại (từ tháng 1 đến tháng 12)
          dates = Array.from({ length: 12 }, (_, i) => {
            const date = new Date(now.getFullYear(), i, 1); // Tháng i của năm hiện tại
            return date.toISOString().split('T')[0];
          });
          break;

        default: // day
          // 7 ngày gần nhất
          dates = Array.from({ length: 7 }, (_, i) => {
            const date = new Date();
            date.setDate(date.getDate() - (6 - i));
            return date.toISOString().split('T')[0];
          });
          break;
      }

      const revenuePromises = dates.map(date => 
        axios.get(`/reports/revenue/date/${date}`)
          .catch(() => ({ data: { date, revenue: 0 } }))
      );

      const revenueResults = await Promise.all(revenuePromises);
      const chartData = revenueResults.map(res => {
        const dateObj = new Date(res.data.date);
        let label = '';
        
        if (period === 'year') {
          label = dateObj.toLocaleDateString('vi-VN', { month: '2-digit', year: 'numeric' });
        } else {
          label = dateObj.toLocaleDateString('vi-VN', { day: '2-digit', month: '2-digit' });
        }

        return {
          date: label,
          revenue: res.data.revenue || 0
        };
      });
      setRevenueChart(chartData);

    } catch (error) {
      console.error("Lỗi tải dữ liệu biểu đồ:", error);
    }
  };

  // Cấu hình bảng thuốc sắp hết
  const columns = [
    { title: 'Tên thuốc', dataIndex: 'name', key: 'name' },
    { title: 'Tồn kho', dataIndex: 'currentStock', key: 'currentStock', render: t => <Tag color="red">{t}</Tag> },
    { title: 'Định mức', dataIndex: 'minStockLevel', key: 'minStockLevel' },
    { title: 'Đơn giá', dataIndex: 'price', key: 'price', render: p => p?.toLocaleString() },
  ];

  return (
    <div style={{ padding: 20 }}>
      <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', marginBottom: 24 }}>
        <h2>Bảng điều khiển</h2>
        <div style={{ display: 'flex', gap: 16 }}>
          <Select 
            value={period} 
            onChange={(val) => {
              setPeriod(val);
              if (val !== 'custom') setDateRange(null);
            }} 
            style={{ width: 150 }}
          >
            <Option value="day">Theo ngày</Option>
            <Option value="week">Theo tuần</Option>
            <Option value="month">Theo tháng</Option>
            <Option value="year">Theo năm</Option>
            <Option value="custom">Tùy chọn</Option>
          </Select>
          {period === 'custom' && (
            <RangePicker 
              onChange={setDateRange}
              format="DD/MM/YYYY"
              placeholder={['Từ ngày', 'Đến ngày']}
            />
          )}
        </div>
      </div>
      
      <Row gutter={16} style={{ marginBottom: 24 }}>
        {/* Doanh thu */}
        <Col span={6}>
          <Card bordered={false}>
            <Statistic
              title={period === 'day' ? 'Doanh thu hôm nay' : 
                     period === 'week' ? 'Doanh thu tuần này' :
                     period === 'month' ? 'Doanh thu tháng này' :
                     period === 'year' ? 'Doanh thu năm nay' :
                     'Doanh thu'}
              value={stats.totalRevenue}
              precision={0}
              valueStyle={{ color: '#cf1322' }}
              prefix={<DollarOutlined />}
              suffix="VNĐ"
            />
            <div style={{ marginTop: 8, fontSize: 12 }}>
              {stats.revenueGrowth >= 0 ? (
                <span style={{ color: '#3f8600' }}>
                  <ArrowUpOutlined /> +{stats.revenueGrowth}% so với kỳ trước
                </span>
              ) : (
                <span style={{ color: '#cf1322' }}>
                  <ArrowDownOutlined /> {stats.revenueGrowth}% so với kỳ trước
                </span>
              )}
            </div>
          </Card>
        </Col>

        {/* Đơn hàng */}
        <Col span={6}>
          <Card bordered={false}>
            <Statistic
              title={period === 'day' ? 'Đơn hàng hôm nay' : 
                     period === 'week' ? 'Đơn hàng tuần này' :
                     period === 'month' ? 'Đơn hàng tháng này' :
                     'Đơn hàng năm nay'}
              value={stats.totalOrders}
              valueStyle={{ color: '#3f8600' }}
              prefix={<ShoppingCartOutlined />}
            />
            <div style={{ marginTop: 8, fontSize: 12 }}>
              {stats.ordersGrowth >= 0 ? (
                <span style={{ color: '#3f8600' }}>
                  <ArrowUpOutlined /> +{stats.ordersGrowth}% so với kỳ trước
                </span>
              ) : (
                <span style={{ color: '#cf1322' }}>
                  <ArrowDownOutlined /> {stats.ordersGrowth}% so với kỳ trước
                </span>
              )}
            </div>
          </Card>
        </Col>

        {/* Khách hàng */}
        <Col span={6}>
          <Card bordered={false}>
            <Statistic
              title={period === 'day' ? 'Khách hàng hôm nay' : 
                     period === 'week' ? 'Khách hàng tuần này' :
                     period === 'month' ? 'Khách hàng tháng này' :
                     'Khách hàng năm nay'}
              value={stats.totalCustomers}
              valueStyle={{ color: '#1890ff' }}
              prefix={<UserOutlined />}
            />
            <div style={{ marginTop: 8, fontSize: 12 }}>
              {stats.customersGrowth >= 0 ? (
                <span style={{ color: '#3f8600' }}>
                  <ArrowUpOutlined /> +{stats.customersGrowth}% so với kỳ trước
                </span>
              ) : (
                <span style={{ color: '#cf1322' }}>
                  <ArrowDownOutlined /> {stats.customersGrowth}% so với kỳ trước
                </span>
              )}
            </div>
          </Card>
        </Col>

        {/* Thuốc sắp hết */}
        <Col span={6}>
          <Card bordered={false}>
            <Statistic
              title="Thuốc sắp hết"
              value={stats.lowStock}
              valueStyle={{ color: '#faad14' }}
              prefix={<WarningOutlined />}
            />
          </Card>
        </Col>
      </Row>

      {/* Biểu đồ doanh thu */}
      <Row gutter={16} style={{ marginBottom: 24 }}>
        <Col span={24}>
          <Card title={
            period === 'day' ? 'Doanh thu 7 ngày gần nhất' : 
            period === 'week' ? 'Doanh thu 7 ngày gần nhất' :
            period === 'month' ? 'Doanh thu 30 ngày gần nhất' :
            'Doanh thu 12 tháng gần nhất'
          } bordered={false}>
            <ResponsiveContainer width="100%" height={300}>
              <LineChart data={revenueChart}>
                <CartesianGrid strokeDasharray="3 3" />
                <XAxis dataKey="date" />
                <YAxis />
                <Tooltip 
                  formatter={(value) => `${value.toLocaleString('vi-VN')} đ`}
                />
                <Legend />
                <Line 
                  type="monotone" 
                  dataKey="revenue" 
                  stroke="#8884d8" 
                  strokeWidth={2}
                  name="Doanh thu"
                  activeDot={{ r: 8 }}
                />
              </LineChart>
            </ResponsiveContainer>
          </Card>
        </Col>
      </Row>

      <Row gutter={16}>
        <Col span={24}>
          <Card title="Danh sách thuốc cần nhập thêm (Sắp hết)" bordered={false}>
            <Table 
                dataSource={medicines} 
                columns={columns} 
                rowKey="id"
                pagination={{ pageSize: 5 }}
            />
          </Card>
        </Col>
      </Row>
    </div>
  );
}