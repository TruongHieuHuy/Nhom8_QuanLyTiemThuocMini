import React, { useState, useEffect } from 'react';
import { Table, Button, Modal, Form, Input, Select, Space, message } from 'antd';
import { PlusOutlined, EditOutlined, DeleteOutlined } from '@ant-design/icons';
import { customerService } from '../services';
import useStore from '../store';

export default function CustomerList() {
  const [customers, setCustomers] = useState([]);
  const [loading, setLoading] = useState(false);
  const [isModalVisible, setIsModalVisible] = useState(false);
  const [editingCustomer, setEditingCustomer] = useState(null);
  const [form] = Form.useForm();
  const { setError } = useStore();

  useEffect(() => {
    fetchCustomers();
  }, []);

  const fetchCustomers = async () => {
    try {
      setLoading(true);
      const data = await customerService.getAll();
      setCustomers(data);
    } catch (err) {
      message.error('Lỗi khi tải danh sách khách hàng');
      setError(err.message);
    } finally {
      setLoading(false);
    }
  };

  const handleAdd = () => {
    form.resetFields();
    setEditingCustomer(null);
    setIsModalVisible(true);
  };

  const handleEdit = (customer) => {
    setEditingCustomer(customer);
    form.setFieldsValue(customer);
    setIsModalVisible(true);
  };

  const handleDelete = async (id) => {
    Modal.confirm({
      title: 'Xóa khách hàng',
      content: 'Bạn có chắc chắn muốn xóa khách hàng này?',
      okText: 'Có',
      cancelText: 'Không',
      onOk: async () => {
        try {
          await customerService.delete(id);
          message.success('Xóa khách hàng thành công');
          fetchCustomers();
        } catch (err) {
          message.error('Lỗi khi xóa khách hàng');
        }
      },
    });
  };

  const handleSubmit = async (values) => {
    try {
      if (editingCustomer) {
        await customerService.update({
          ...editingCustomer,
          ...values,
        });
        message.success('Cập nhật khách hàng thành công');
      } else {
        await customerService.create(values);
        message.success('Thêm khách hàng thành công');
      }
      setIsModalVisible(false);
      fetchCustomers();
    } catch (err) {
      message.error('Lỗi khi lưu khách hàng');
    }
  };

  const columns = [
    {
      title: 'Tên khách hàng',
      dataIndex: 'name',
      key: 'name',
    },
    {
      title: 'Số điện thoại',
      dataIndex: 'phoneNumber',
      key: 'phoneNumber',
    },
    {
      title: 'Email',
      dataIndex: 'email',
      key: 'email',
    },
    {
      title: 'Địa chỉ',
      dataIndex: 'address',
      key: 'address',
    },
    {
      title: 'Tổng chi tiêu',
      dataIndex: 'totalSpending',
      key: 'totalSpending',
      render: (amount) => `${amount.toLocaleString('vi-VN')} đ`,
    },
    {
      title: 'Hành động',
      key: 'action',
      render: (_, record) => (
        <Space>
          <Button
            type="primary"
            size="small"
            icon={<EditOutlined />}
            onClick={() => handleEdit(record)}
          >
            Sửa
          </Button>
          <Button
            danger
            size="small"
            icon={<DeleteOutlined />}
            onClick={() => handleDelete(record.id)}
          >
            Xóa
          </Button>
        </Space>
      ),
    },
  ];

  return (
    <>
      <Space style={{ marginBottom: '16px' }}>
        <Button type="primary" icon={<PlusOutlined />} onClick={handleAdd}>
          Thêm khách hàng mới
        </Button>
      </Space>

      <Table
        columns={columns}
        dataSource={customers}
        loading={loading}
        rowKey="id"
        pagination={{ pageSize: 10 }}
      />

      <Modal
        title={editingCustomer ? 'Sửa thông tin khách hàng' : 'Thêm khách hàng mới'}
        visible={isModalVisible}
        onOk={() => form.submit()}
        onCancel={() => setIsModalVisible(false)}
      >
        <Form form={form} layout="vertical" onFinish={handleSubmit}>
          <Form.Item
            name="name"
            label="Tên khách hàng"
            rules={[{ required: true, message: 'Vui lòng nhập tên' }]}
          >
            <Input />
          </Form.Item>

          <Form.Item
            name="phoneNumber"
            label="Số điện thoại"
            rules={[{ required: true, message: 'Vui lòng nhập số điện thoại' }]}
          >
            <Input />
          </Form.Item>

          <Form.Item name="email" label="Email">
            <Input type="email" />
          </Form.Item>

          <Form.Item name="address" label="Địa chỉ">
            <Input />
          </Form.Item>

          <Form.Item name="city" label="Tỉnh/Thành phố">
            <Input />
          </Form.Item>

          <Form.Item name="district" label="Quận/Huyện">
            <Input />
          </Form.Item>

          <Form.Item name="ward" label="Phường/Xã">
            <Input />
          </Form.Item>

          <Form.Item
            name="gender"
            label="Giới tính"
            rules={[{ required: true }]}
          >
            <Select
              options={[
                { label: 'Nam', value: 'Nam' },
                { label: 'Nữ', value: 'Nữ' },
                { label: 'Khác', value: 'Khác' },
              ]}
            />
          </Form.Item>
        </Form>
      </Modal>
    </>
  );
}
