import React, { useState, useEffect } from 'react';
import { Table, Button, Space, Modal, Form, Input, InputNumber, message, Card } from 'antd';
import { PlusOutlined, EditOutlined, DeleteOutlined, ReloadOutlined } from '@ant-design/icons';
import axios from '../utils/axiosConfig';

const API_URL = '/Suppliers';

export default function SuppliersPage() {
  const [suppliers, setSuppliers] = useState([]);
  const [loading, setLoading] = useState(false);
  const [isModalVisible, setIsModalVisible] = useState(false);
  const [editingSupplier, setEditingSupplier] = useState(null);
  const [form] = Form.useForm();

  useEffect(() => {
    fetchSuppliers();
  }, []);

  const fetchSuppliers = async () => {
    try {
      setLoading(true);
      const res = await axios.get(API_URL);
      setSuppliers(res.data);
    } catch (err) {
      message.error('Không thể tải danh sách nhà cung cấp!');
    } finally {
      setLoading(false);
    }
  };

  const handleOk = async () => {
    try {
      const values = await form.validateFields();
      
      const payload = {
        ...values,
        isActive: true,
        ...(editingSupplier && { id: editingSupplier.id })
      };

      if (editingSupplier) {
        await axios.put(`${API_URL}/${editingSupplier.id}`, payload);
        message.success('Cập nhật nhà cung cấp thành công!');
      } else {
        await axios.post(API_URL, payload);
        message.success('Thêm nhà cung cấp mới thành công!');
      }

      setIsModalVisible(false);
      form.resetFields();
      fetchSuppliers();
    } catch (err) {
      console.error(err);
      message.error('Có lỗi xảy ra!');
    }
  };

  const handleDelete = async (id) => {
    Modal.confirm({
      title: 'Xác nhận xóa',
      content: 'Bạn có chắc muốn xóa nhà cung cấp này không?',
      okText: 'Xóa',
      okType: 'danger',
      cancelText: 'Hủy',
      onOk: async () => {
        try {
          await axios.delete(`${API_URL}/${id}`);
          message.success('Đã xóa nhà cung cấp!');
          fetchSuppliers();
        } catch (err) {
          message.error('Xóa thất bại!');
        }
      },
    });
  };

  const openModal = (supplier = null) => {
    setEditingSupplier(supplier);
    if (supplier) {
      form.setFieldsValue(supplier);
    } else {
      form.resetFields();
    }
    setIsModalVisible(true);
  };

  const columns = [
    { 
      title: 'Tên nhà cung cấp', 
      dataIndex: 'name', 
      key: 'name',
      render: text => <b>{text}</b>
    },
    { 
      title: 'Số điện thoại', 
      dataIndex: 'phoneNumber', 
      key: 'phoneNumber'
    },
    { 
      title: 'Email', 
      dataIndex: 'email', 
      key: 'email',
      responsive: ['lg']
    },
    { 
      title: 'Địa chỉ', 
      dataIndex: 'address', 
      key: 'address',
      responsive: ['lg']
    },
    { 
      title: 'Công nợ', 
      dataIndex: 'debt', 
      key: 'debt',
      align: 'right',
      render: val => <span style={{color: val > 0 ? '#e74c3c' : '#27ae60', fontWeight: 'bold'}}>
        {val?.toLocaleString('vi-VN')} đ
      </span>
    },
    {
      title: 'Hành động', 
      key: 'action',
      align: 'center',
      render: (_, record) => (
        <Space>
          <Button 
            icon={<EditOutlined />} 
            type="primary" 
            ghost 
            size="small" 
            onClick={() => openModal(record)} 
          />
          <Button 
            icon={<DeleteOutlined />} 
            danger 
            size="small" 
            onClick={() => handleDelete(record.id)} 
          />
        </Space>
      ),
    },
  ];

  return (
    <div>
      <Card 
        title="Danh sách nhà cung cấp"
        extra={
          <Space>
            <Button icon={<ReloadOutlined />} onClick={fetchSuppliers}>Làm mới</Button>
            <Button type="primary" icon={<PlusOutlined />} onClick={() => openModal(null)}>
              Thêm nhà cung cấp
            </Button>
          </Space>
        }
      >
        <Table 
          columns={columns} 
          dataSource={suppliers} 
          rowKey="id" 
          loading={loading}
          pagination={{ pageSize: 10 }}
        />
      </Card>

      <Modal
        title={editingSupplier ? "Sửa nhà cung cấp" : "Thêm nhà cung cấp mới"}
        open={isModalVisible}
        onOk={handleOk}
        onCancel={() => setIsModalVisible(false)}
        width={600}
      >
        <Form form={form} layout="vertical">
          <Form.Item 
            name="name" 
            label="Tên nhà cung cấp" 
            rules={[{ required: true, message: 'Vui lòng nhập tên!' }]}
          >
            <Input placeholder="Ví dụ: Công ty TNHH Dược Phẩm ABC" />
          </Form.Item>

          <Form.Item 
            name="phoneNumber" 
            label="Số điện thoại" 
            rules={[{ required: true, message: 'Vui lòng nhập SĐT!' }]}
          >
            <Input placeholder="0123456789" />
          </Form.Item>

          <Form.Item name="email" label="Email">
            <Input placeholder="supplier@example.com" />
          </Form.Item>

          <Form.Item name="address" label="Địa chỉ">
            <Input.TextArea rows={2} placeholder="Địa chỉ liên hệ..." />
          </Form.Item>

          <Form.Item 
            name="debt" 
            label="Công nợ hiện tại" 
            initialValue={0}
          >
            <InputNumber 
              style={{ width: '100%' }} 
              formatter={value => `${value}`.replace(/\B(?=(\d{3})+(?!\d))/g, ',')}
              min={0}
              addonAfter="VNĐ"
            />
          </Form.Item>
        </Form>
      </Modal>
    </div>
  );
}
