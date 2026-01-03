import React, { useState, useEffect } from 'react';
import {
  Table,
  Button,
  Modal,
  Form,
  Input,
  Space,
  message,
  Card,
  Row,
  Col,
  Popconfirm,
} from 'antd';
import {
  PlusOutlined,
  EditOutlined,
  DeleteOutlined,
  SearchOutlined,
} from '@ant-design/icons';
import './CategoriesPage.css';

export default function CategoriesPage() {
  const [categories, setCategories] = useState([]);
  const [loading, setLoading] = useState(false);
  const [isModalVisible, setIsModalVisible] = useState(false);
  const [editingCategory, setEditingCategory] = useState(null);
  const [form] = Form.useForm();
  const [searchText, setSearchText] = useState('');

  const API_BASE_URL = process.env.REACT_APP_API_BASE_URL || 'http://localhost:5000/api';
  const token = localStorage.getItem('token');

  // Lấy danh sách danh mục
  useEffect(() => {
    fetchCategories();
  }, [searchText]);

  const fetchCategories = async () => {
    setLoading(true);
    try {
      const url = searchText
        ? `${API_BASE_URL}/medicinegroups?search=${searchText}`
        : `${API_BASE_URL}/medicinegroups`;

      const response = await fetch(url, {
        headers: {
          Authorization: `Bearer ${token}`,
        },
      });

      if (!response.ok) throw new Error('Lỗi khi lấy danh mục');

      const result = await response.json();
      setCategories(result.data || []);
    } catch (error) {
      message.error('Lỗi khi lấy danh mục');
      console.error(error);
    } finally {
      setLoading(false);
    }
  };

  // Mở modal tạo/sửa
  const handleOpenModal = (category = null) => {
    setEditingCategory(category);
    if (category) {
      form.setFieldsValue({
        name: category.name,
        description: category.description,
      });
    } else {
      form.resetFields();
    }
    setIsModalVisible(true);
  };

  // Đóng modal
  const handleCloseModal = () => {
    setIsModalVisible(false);
    setEditingCategory(null);
    form.resetFields();
  };

  // Lưu danh mục (thêm hoặc sửa)
  const handleSaveCategory = async (values) => {
    try {
      const url = editingCategory
        ? `${API_BASE_URL}/medicinegroups/${editingCategory.id}`
        : `${API_BASE_URL}/medicinegroups`;

      const method = editingCategory ? 'PUT' : 'POST';

      const response = await fetch(url, {
        method,
        headers: {
          'Content-Type': 'application/json',
          Authorization: `Bearer ${token}`,
        },
        body: JSON.stringify({
          id: editingCategory?.id,
          name: values.name,
          description: values.description,
        }),
      });

      if (!response.ok) {
        const errorData = await response.json();
        throw new Error(errorData.message || 'Lỗi');
      }

      const result = await response.json();
      message.success(result.message);
      handleCloseModal();
      fetchCategories();
    } catch (error) {
      message.error(error.message);
      console.error(error);
    }
  };

  // Xóa danh mục
  const handleDeleteCategory = async (id) => {
    try {
      const response = await fetch(`${API_BASE_URL}/medicinegroups/${id}`, {
        method: 'DELETE',
        headers: {
          Authorization: `Bearer ${token}`,
        },
      });

      if (!response.ok) {
        const errorData = await response.json();
        throw new Error(errorData.message || 'Lỗi xóa danh mục');
      }

      const result = await response.json();
      message.success(result.message);
      fetchCategories();
    } catch (error) {
      message.error(error.message);
      console.error(error);
    }
  };

  // Cột bảng
  const columns = [
    {
      title: 'ID',
      dataIndex: 'id',
      key: 'id',
      width: 80,
    },
    {
      title: 'Tên danh mục',
      dataIndex: 'name',
      key: 'name',
      render: (text) => <strong>{text}</strong>,
    },
    {
      title: 'Mô tả',
      dataIndex: 'description',
      key: 'description',
      ellipsis: true,
    },
    {
      title: 'Thao tác',
      key: 'action',
      width: 150,
      render: (_, record) => (
        <Space>
          <Button
            type="primary"
            size="small"
            icon={<EditOutlined />}
            onClick={() => handleOpenModal(record)}
          >
            Sửa
          </Button>
          <Popconfirm
            title="Xóa danh mục"
            description="Bạn có chắc chắn muốn xóa danh mục này không?"
            onConfirm={() => handleDeleteCategory(record.id)}
            okText="Có"
            cancelText="Không"
          >
            <Button type="primary" danger size="small" icon={<DeleteOutlined />}>
              Xóa
            </Button>
          </Popconfirm>
        </Space>
      ),
    },
  ];

  return (
    <div className="categories-page">
      <Card
        title={
          <Row justify="space-between" align="middle">
            <Col>
              <h2>Quản lý danh mục sản phẩm</h2>
            </Col>
            <Col>
              <Button
                type="primary"
                icon={<PlusOutlined />}
                onClick={() => handleOpenModal()}
              >
                Thêm danh mục mới
              </Button>
            </Col>
          </Row>
        }
        style={{ marginBottom: '20px' }}
      >
        <Row gutter={[16, 16]} style={{ marginBottom: '20px' }}>
          <Col xs={24} sm={12}>
            <Input
              placeholder="Tìm kiếm danh mục..."
              prefix={<SearchOutlined />}
              value={searchText}
              onChange={(e) => setSearchText(e.target.value)}
            />
          </Col>
        </Row>

        <Table
          columns={columns}
          dataSource={categories}
          loading={loading}
          rowKey="id"
          pagination={{ pageSize: 10 }}
        />
      </Card>

      {/* Modal thêm/sửa danh mục */}
      <Modal
        title={editingCategory ? 'Sửa danh mục' : 'Thêm danh mục mới'}
        open={isModalVisible}
        onOk={() => form.submit()}
        onCancel={handleCloseModal}
        okText={editingCategory ? 'Cập nhật' : 'Thêm'}
        cancelText="Hủy"
      >
        <Form form={form} layout="vertical" onFinish={handleSaveCategory}>
          <Form.Item
            label="Tên danh mục"
            name="name"
            rules={[{ required: true, message: 'Vui lòng nhập tên danh mục' }]}
          >
            <Input placeholder="Ví dụ: Vitamin, Kháng sinh, v.v..." />
          </Form.Item>

          <Form.Item
            label="Mô tả"
            name="description"
          >
            <Input.TextArea
              placeholder="Nhập mô tả danh mục..."
              rows={4}
            />
          </Form.Item>
        </Form>
      </Modal>
    </div>
  );
}
