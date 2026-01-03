import React, { useEffect, useMemo, useState } from 'react';
import { Card, Table, Tag, Space, Input, Select, DatePicker, Button, Drawer, Descriptions, message, Typography } from 'antd';
import { paymentService } from '../services';

const { RangePicker } = DatePicker;
const { Option } = Select;
const { Text } = Typography;

const formatVnd = (value) => {
  try {
    return new Intl.NumberFormat('vi-VN', { style: 'currency', currency: 'VND' }).format(value || 0);
  } catch {
    return `${value || 0} VND`;
  }
};

const statusTag = (status) => {
  if (status === 'Success') return <Tag color="green">Thành công</Tag>;
  if (status === 'Failed') return <Tag color="red">Thất bại</Tag>;
  return <Tag color="gold">Đang chờ</Tag>;
};

const PaymentHistoryPage = () => {
  const [loading, setLoading] = useState(false);
  const [data, setData] = useState({ items: [], total: 0, page: 1, pageSize: 20 });

  const [orderCode, setOrderCode] = useState('');
  const [provider, setProvider] = useState('');
  const [status, setStatus] = useState('');
  const [range, setRange] = useState(null);

  const [drawerOpen, setDrawerOpen] = useState(false);
  const [selected, setSelected] = useState(null);

  const queryParams = useMemo(() => {
    const params = {};
    if (orderCode) params.orderCode = orderCode;
    if (provider) params.provider = provider;
    if (status) params.status = status;

    if (range && range.length === 2 && range[0] && range[1]) {
      // send ISO for backend DateTime?
      params.from = range[0].startOf('day').toISOString();
      params.to = range[1].endOf('day').toISOString();
    }

    return params;
  }, [orderCode, provider, status, range]);

  const load = async (page = 1, pageSize = data.pageSize) => {
    setLoading(true);
    try {
      const res = await paymentService.getHistory({ ...queryParams, page, pageSize });
      setData({
        items: res.items || [],
        total: res.total || 0,
        page: res.page || page,
        pageSize: res.pageSize || pageSize,
      });
    } catch (err) {
      message.error(err?.message || 'Không tải được lịch sử thanh toán');
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    load(1, data.pageSize);
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, []);

  const columns = [
    {
      title: 'Thời gian',
      dataIndex: 'createdAt',
      key: 'createdAt',
      width: 180,
      render: (v) => (v ? new Date(v).toLocaleString('vi-VN') : ''),
    },
    {
      title: 'Mã đơn',
      dataIndex: 'orderCode',
      key: 'orderCode',
      width: 140,
      render: (v) => <Text strong>{v}</Text>,
    },
    {
      title: 'Nhà cung cấp',
      dataIndex: 'provider',
      key: 'provider',
      width: 120,
      render: (v) => v || '',
    },
    {
      title: 'Hình thức',
      dataIndex: 'paymentMethod',
      key: 'paymentMethod',
      width: 120,
      render: (v) => v || '',
    },
    {
      title: 'Số tiền',
      dataIndex: 'amount',
      key: 'amount',
      align: 'right',
      width: 140,
      render: (v) => <Text>{formatVnd(v)}</Text>,
    },
    {
      title: 'Trạng thái',
      dataIndex: 'status',
      key: 'status',
      width: 120,
      render: (v) => statusTag(v),
    },
    {
      title: 'Mã phản hồi',
      dataIndex: 'responseCode',
      key: 'responseCode',
      width: 110,
    },
    {
      title: 'Mã GD',
      dataIndex: 'transactionNo',
      key: 'transactionNo',
      width: 150,
      ellipsis: true,
    },
    {
      title: 'Ngân hàng',
      dataIndex: 'bankCode',
      key: 'bankCode',
      width: 110,
    },
    {
      title: '',
      key: 'action',
      fixed: 'right',
      width: 120,
      render: (_, record) => (
        <Space>
          <Button
            size="small"
            onClick={() => {
              setSelected(record);
              setDrawerOpen(true);
            }}
          >
            Chi tiết
          </Button>
        </Space>
      ),
    },
  ];

  return (
    <div style={{ padding: 24 }}>
      <Card
        title="Lịch sử thanh toán"
        extra={<Text type="secondary">Tổng: {data.total}</Text>}
      >
        <Space wrap style={{ marginBottom: 16 }}>
          <Input
            placeholder="Tìm mã đơn (OrderCode)"
            value={orderCode}
            onChange={(e) => setOrderCode(e.target.value)}
            style={{ width: 220 }}
            allowClear
          />
          <Select
            placeholder="Nhà cung cấp"
            value={provider || undefined}
            onChange={(v) => setProvider(v || '')}
            style={{ width: 170 }}
            allowClear
          >
            <Option value="VNPay">VNPay</Option>
            <Option value="Cash">Cash</Option>
            <Option value="Banking">Banking</Option>
          </Select>
          <Select
            placeholder="Trạng thái"
            value={status || undefined}
            onChange={(v) => setStatus(v || '')}
            style={{ width: 170 }}
            allowClear
          >
            <Option value="Pending">Đang chờ</Option>
            <Option value="Success">Thành công</Option>
            <Option value="Failed">Thất bại</Option>
          </Select>

          <RangePicker value={range} onChange={(v) => setRange(v)} />
          <Button type="primary" onClick={() => load(1, data.pageSize)}>
            Lọc
          </Button>
          <Button
            onClick={() => {
              setOrderCode('');
              setProvider('');
              setStatus('');
              setRange(null);
              setTimeout(() => load(1, data.pageSize), 0);
            }}
          >
            Reset
          </Button>
        </Space>

        <Table
          rowKey="id"
          columns={columns}
          dataSource={data.items}
          loading={loading}
          scroll={{ x: 1200 }}
          pagination={{
            current: data.page,
            pageSize: data.pageSize,
            total: data.total,
            showSizeChanger: true,
          }}
          onChange={(pagination) => load(pagination.current, pagination.pageSize)}
        />
      </Card>

      <Drawer
        title="Chi tiết giao dịch"
        open={drawerOpen}
        onClose={() => setDrawerOpen(false)}
        width={720}
      >
        {selected ? (
          <>
            <Descriptions bordered column={2} size="small">
              <Descriptions.Item label="Mã đơn">{selected.orderCode}</Descriptions.Item>
              <Descriptions.Item label="Trạng thái">{statusTag(selected.status)}</Descriptions.Item>
              <Descriptions.Item label="Nhà cung cấp">{selected.provider}</Descriptions.Item>
              <Descriptions.Item label="Hình thức">{selected.paymentMethod}</Descriptions.Item>
              <Descriptions.Item label="Số tiền">{formatVnd(selected.amount)}</Descriptions.Item>
              <Descriptions.Item label="Currency">{selected.currency}</Descriptions.Item>
              <Descriptions.Item label="ResponseCode">{selected.responseCode || ''}</Descriptions.Item>
              <Descriptions.Item label="TransactionNo">{selected.transactionNo || ''}</Descriptions.Item>
              <Descriptions.Item label="BankCode">{selected.bankCode || ''}</Descriptions.Item>
              <Descriptions.Item label="PayDate">{selected.payDate || ''}</Descriptions.Item>
              <Descriptions.Item label="CreatedAt">
                {selected.createdAt ? new Date(selected.createdAt).toLocaleString('vi-VN') : ''}
              </Descriptions.Item>
              <Descriptions.Item label="UpdatedAt">
                {selected.updatedAt ? new Date(selected.updatedAt).toLocaleString('vi-VN') : ''}
              </Descriptions.Item>
            </Descriptions>

            <div style={{ marginTop: 16 }}>
              <Text strong>Raw Data</Text>
              <pre style={{ marginTop: 8, padding: 12, background: '#f5f5f5', borderRadius: 6, whiteSpace: 'pre-wrap' }}>
{selected.rawData || ''}
              </pre>
            </div>
          </>
        ) : null}
      </Drawer>
    </div>
  );
};

export default PaymentHistoryPage;
