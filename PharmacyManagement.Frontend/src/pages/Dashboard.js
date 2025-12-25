import React, { useState, useEffect } from 'react';
import { Row, Col, Card, Statistic, Table, Space, Button, message } from 'antd';
import {
  ShoppingCartOutlined,
  DollarOutlined,
  UserOutlined,
  MedicineBoxOutlined,
  ArrowUpOutlined,
  ArrowDownOutlined,
} from '@ant-design/icons';
import { orderService, reportService, medicineService, customerService } from '../services';

export default function Dashboard() {
  const [stats, setStats] = useState({
    totalOrders: 0,
    totalRevenue: 0,
    totalCustomers: 0,
    lowStockMedicines: 0,
  });
  const [topMedicines, setTopMedicines] = useState([]);
  const [topCustomers, setTopCustomers] = useState([]);
  const [recentOrders, setRecentOrders] = useState([]);
  const [loading, setLoading] = useState(false);

  useEffect(() => {
    loadDashboardData();
  }, []);

  const loadDashboardData = async () => {
    try {
      setLoading(true);

      // Get statistics
      const orders = await orderService.getAll();
      const customers = await customerService.getAll();
      const medicines = await medicineService.getAll();
      const lowStock = medicines.filter((m) => m.isLowStock).length;

      const today = new Date().toISOString().split('T')[0];
      const revenueData = await reportService.getRevenueByDate(today);

      setStats({
        totalOrders: orders.length,
        totalRevenue: revenueData.revenue || 0,
        totalCustomers: customers.length,
        lowStockMedicines: lowStock,
      });

      // Get top medicines
      const topMeds = await reportService.getTopMedicines(5);
      setTopMedicines(topMeds);

      // Get top customers
      const topCusts = await reportService.getTopCustomers(5);
      setTopCustomers(topCusts);

      // Get recent orders
      const recentOrds = orders.slice(-5).reverse();
      setRecentOrders(recentOrds);
    } catch (err) {
      message.error('Lỗi khi tải dữ liệu bảng điều khiển');
    } finally {
      setLoading(false);
    }
  };

  const orderColumns = [
    {
      title: 'Mã đơn hàng',
      dataIndex: 'orderCode',
      key: 'orderCode',
    },
    {
      title: 'Khách hàng',
      dataIndex: 'customerName',
      key: 'customerName',
    },
    {
      title: 'Tổng tiền',
      dataIndex: 'total',
      key: 'total',
      render: (total) => `${total.toLocaleString('vi-VN')} đ`,
    },
    {
      title: 'Trạng thái',
      dataIndex: 'orderStatus',
      key: 'orderStatus',
      render: (status) => (
        <span
          style={{
            color: status === 'Completed' ? 'green' : 'orange',
          }}
        >
          {status}
        </span>
      ),
    },
  ];

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

  return (
    <div>
      <h1>Bảng điều khiển</h1>

      {/* Statistics */}
      <Row gutter={16} style={{ marginBottom: '24px' }}>
        <Col xs={24} sm={12} lg={6}>
          <Card loading={loading}>
            <Statistic
              title="Tổng đơn hàng"
              value={stats.totalOrders}
              prefix={<ShoppingCartOutlined />}
              valueStyle={{ color: '#1890ff' }}
            />
          </Card>
        </Col>
        <Col xs={24} sm={12} lg={6}>
          <Card loading={loading}>
            <Statistic
              title="Doanh thu hôm nay"
              value={stats.totalRevenue}
              suffix="đ"
              prefix={<DollarOutlined />}
              valueStyle={{ color: '#52c41a' }}
            />
          </Card>
        </Col>
        <Col xs={24} sm={12} lg={6}>
          <Card loading={loading}>
            <Statistic
              title="Tổng khách hàng"
              value={stats.totalCustomers}
              prefix={<UserOutlined />}
              valueStyle={{ color: '#722ed1' }}
            />
          </Card>
        </Col>
        <Col xs={24} sm={12} lg={6}>
          <Card loading={loading}>
            <Statistic
              title="Thuốc sắp hết"
              value={stats.lowStockMedicines}
              prefix={<MedicineBoxOutlined />}
              valueStyle={{ color: '#eb2f96' }}
            />
          </Card>
        </Col>
      </Row>

      {/* Charts and Tables */}
      <Row gutter={16}>
        <Col xs={24} lg={12}>
          <Card title="Sản phẩm bán chạy nhất" loading={loading}>
            <Table
              columns={medicineColumns}
              dataSource={topMedicines}
              pagination={false}
              rowKey="id"
            />
          </Card>
        </Col>
        <Col xs={24} lg={12}>
          <Card title="Khách hàng hàng đầu" loading={loading}>
            <Table
              columns={customerColumns}
              dataSource={topCustomers}
              pagination={false}
              rowKey="id"
            />
          </Card>
        </Col>
      </Row>

      {/* Recent Orders */}
      <Card title="Đơn hàng gần đây" style={{ marginTop: '24px' }} loading={loading}>
        <Table
          columns={orderColumns}
          dataSource={recentOrders}
          pagination={false}
          rowKey="id"
        />
      </Card>
    </div>
  );
}
