import React, { useState, useEffect } from 'react';
import { Card, Row, Col, Statistic, Table, Tag, message } from 'antd';
import { 
  ShoppingCartOutlined, 
  UserOutlined, 
  MedicineBoxOutlined, 
  DollarOutlined, 
  WarningOutlined 
} from '@ant-design/icons';
import axios from 'axios';

const API_BASE_URL = 'http://localhost:5000/api';

export default function Dashboard() {
  const [stats, setStats] = useState({
    totalMedicines: 0,
    totalCustomers: 0,
    lowStock: 0,
    totalOrders: 0,
    totalRevenue: 0
  });

  const [medicines, setMedicines] = useState([]); // Để hiện bảng thuốc sắp hết

  useEffect(() => {
    fetchDashboardData();
  }, []);

  const fetchDashboardData = async () => {
    try {
      const token = localStorage.getItem('token');
      const headers = {
        'Content-Type': 'application/json',
        ...(token && { Authorization: `Bearer ${token}` })
      };

      // 1. Gọi API Thống kê tổng hợp (Cái file Controller mới tạo)
      const statsRes = await axios.get(`${API_BASE_URL}/Dashboard/stats`, { headers });
      setStats(statsRes.data);

      // 2. Gọi API lấy danh sách thuốc để lọc thuốc sắp hết hiện ra bảng
      const medRes = await axios.get(`${API_BASE_URL}/Medicines`, { headers });
      setMedicines(medRes.data.filter(m => m.currentStock <= m.minStockLevel));

    } catch (error) {
      console.error("Lỗi tải dữ liệu dashboard:", error);
      message.error("Không tải được dữ liệu bảng điều khiển");
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
      <h2>Bảng điều khiển</h2>
      
      <Row gutter={16} style={{ marginBottom: 24 }}>
        {/* Doanh thu */}
        <Col span={6}>
          <Card bordered={false}>
            <Statistic
              title="Doanh thu"
              value={stats.totalRevenue}
              precision={0}
              valueStyle={{ color: '#cf1322' }}
              prefix={<DollarOutlined />}
              suffix="VNĐ"
            />
          </Card>
        </Col>

        {/* Đơn hàng */}
        <Col span={6}>
          <Card bordered={false}>
            <Statistic
              title="Đơn hàng"
              value={stats.totalOrders}
              valueStyle={{ color: '#3f8600' }}
              prefix={<ShoppingCartOutlined />}
            />
          </Card>
        </Col>

        {/* Khách hàng */}
        <Col span={6}>
          <Card bordered={false}>
            <Statistic
              title="Khách hàng"
              value={stats.totalCustomers}
              valueStyle={{ color: '#1890ff' }}
              prefix={<UserOutlined />}
            />
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