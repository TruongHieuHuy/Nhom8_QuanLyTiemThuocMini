import React, { useState, useEffect } from 'react';
import { Row, Col, Card, Select, Table, Button, DatePicker, Space } from 'antd';
import { reportService } from '../services';
import { BarChart, Bar, XAxis, YAxis, CartesianGrid, Tooltip, Legend, ResponsiveContainer } from 'recharts';
import dayjs from 'dayjs';

export default function ReportsPage() {
  const [reports, setReports] = useState({
    topMedicines: [],
    topCustomers: [],
    outOfStock: [],
  });
  const [loading, setLoading] = useState(false);
  const [dateRange, setDateRange] = useState([dayjs().startOf('month'), dayjs()]);

  useEffect(() => {
    loadReports();
  }, []);

  const loadReports = async () => {
    try {
      setLoading(true);
      const [medicines, customers, outOfStock] = await Promise.all([
        reportService.getTopMedicines(10),
        reportService.getTopCustomers(10),
        reportService.getOutOfStock(),
      ]);

      setReports({
        topMedicines: medicines,
        topCustomers: customers,
        outOfStock: outOfStock,
      });
    } catch (err) {
      console.error('Lỗi khi tải báo cáo:', err);
    } finally {
      setLoading(false);
    }
  };

  const medicineColumns = [
    {
      title: 'Tên thuốc',
      dataIndex: 'name',
      key: 'name',
    },
    {
      title: 'Giá',
      dataIndex: 'price',
      key: 'price',
      render: (price) => `${price.toLocaleString('vi-VN')} đ`,
    },
    {
      title: 'Tồn kho',
      dataIndex: 'currentStock',
      key: 'currentStock',
    },
  ];

  const customerColumns = [
    {
      title: 'Tên khách hàng',
      dataIndex: 'name',
      key: 'name',
    },
    {
      title: 'Tổng chi tiêu',
      dataIndex: 'totalSpending',
      key: 'totalSpending',
      render: (amount) => `${amount.toLocaleString('vi-VN')} đ`,
    },
  ];

  const outOfStockColumns = [
    {
      title: 'Tên thuốc',
      dataIndex: 'name',
      key: 'name',
    },
    {
      title: 'Mức tối thiểu',
      dataIndex: 'minStockLevel',
      key: 'minStockLevel',
    },
  ];

  return (
    <div>
      <h1>Báo cáo & thống kê</h1>

      <Card style={{ marginBottom: '24px' }}>
        <Space>
          <span>Thời gian:</span>
          <DatePicker.RangePicker
            value={dateRange}
            onChange={(dates) => setDateRange(dates)}
            format="DD/MM/YYYY"
          />
          <Button type="primary" onClick={loadReports}>
            Cập nhật
          </Button>
        </Space>
      </Card>

      <Row gutter={16}>
        <Col xs={24} lg={12}>
          <Card title="Sản phẩm bán chạy nhất" loading={loading}>
            <Table
              columns={medicineColumns}
              dataSource={reports.topMedicines}
              pagination={false}
              rowKey="id"
            />
          </Card>
        </Col>
        <Col xs={24} lg={12}>
          <Card title="Khách hàng hàng đầu" loading={loading}>
            <Table
              columns={customerColumns}
              dataSource={reports.topCustomers}
              pagination={false}
              rowKey="id"
            />
          </Card>
        </Col>
      </Row>

      <Card title="Thuốc hết hàng" style={{ marginTop: '24px' }} loading={loading}>
        <Table
          columns={outOfStockColumns}
          dataSource={reports.outOfStock}
          pagination={false}
          rowKey="id"
        />
      </Card>
    </div>
  );
}
