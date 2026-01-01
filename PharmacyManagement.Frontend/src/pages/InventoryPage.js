import React from 'react';
import { Table, Card, Button, Space, DatePicker, Tag, message } from 'antd';
import { medicineService } from '../services';

export default function InventoryPage() {
  const [inventories, setInventories] = React.useState([]);
  const [loading, setLoading] = React.useState(false);

  React.useEffect(() => {
    fetchInventories();
  }, []);

  const fetchInventories = async () => {
    try {
      setLoading(true);
      // Get low stock medicines
      const medicines = await medicineService.getLowStock();
      setInventories(medicines);
    } catch (err) {
      message.error('Lỗi khi tải dữ liệu tồn kho');
    } finally {
      setLoading(false);
    }
  };

  const columns = [
    {
      title: 'Tên thuốc',
      dataIndex: 'name',
      key: 'name',
    },
    {
      title: 'Tồn kho hiện tại',
      dataIndex: 'currentStock',
      key: 'currentStock',
      render: (stock) => (
        <Tag color={stock <= 5 ? 'red' : stock <= 10 ? 'orange' : 'green'}>
          {stock}
        </Tag>
      ),
    },
    {
      title: 'Mức tối thiểu',
      dataIndex: 'minStockLevel',
      key: 'minStockLevel',
    },
    {
      title: 'Hạn sử dụng',
      dataIndex: 'expiryDate',
      key: 'expiryDate',
      render: (date) => new Date(date).toLocaleDateString('vi-VN'),
    },
    {
      title: 'Trạng thái',
      dataIndex: 'isLowStock',
      key: 'status',
      render: (isLowStock) => (
        <Tag color={isLowStock ? 'red' : 'green'}>
          {isLowStock ? 'Cảnh báo' : 'Bình thường'}
        </Tag>
      ),
    },
    {
      title: 'Hành động',
      key: 'action',
      render: (_, record) => (
        <Space>
          <Button
            type="primary"
            size="small"
            onClick={() => window.location.href = `/medicines`}
          >
            Xem chi tiết
          </Button>
        </Space>
      ),
    },
  ];

  return (
    <div>
      <h1>Quản Lý Tồn Kho</h1>

      <Card
        title="Danh Sách Thuốc Sắp Hết Hàng"
        loading={loading}
        extra={
          <Button type="primary" onClick={fetchInventories}>
            Làm mới
          </Button>
        }
      >
        <Table
          columns={columns}
          dataSource={inventories}
          rowKey="id"
          pagination={{ pageSize: 10 }}
        />
      </Card>

      {inventories.length === 0 && !loading && (
        <Card style={{ marginTop: '16px' }}>
          <p style={{ textAlign: 'center', color: '#999' }}>
            ✓ Tất cả thuốc đều có đủ tồn kho
          </p>
        </Card>
      )}
    </div>
  );
}
