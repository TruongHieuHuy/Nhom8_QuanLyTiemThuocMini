import React, { useState, useEffect } from 'react';
import { Table, Button, Modal, Form, Input, InputNumber, Select, Space, message } from 'antd';
import { PlusOutlined, EditOutlined, DeleteOutlined } from '@ant-design/icons';
import { medicineService } from '../services';
import useStore from '../store';

export default function MedicineList() {
  const [medicines, setMedicines] = useState([]);
  const [loading, setLoading] = useState(false);
  const [isModalVisible, setIsModalVisible] = useState(false);
  const [editingMedicine, setEditingMedicine] = useState(null);
  const [form] = Form.useForm();
  const { setError } = useStore();

  useEffect(() => {
    fetchMedicines();
  }, []);

  const fetchMedicines = async () => {
    try {
      setLoading(true);
      const data = await medicineService.getAll();
      setMedicines(data);
    } catch (err) {
      message.error('Lỗi khi tải danh sách thuốc');
      setError(err.message);
    } finally {
      setLoading(false);
    }
  };

  const handleAdd = () => {
    form.resetFields();
    setEditingMedicine(null);
    setIsModalVisible(true);
  };

  const handleEdit = (medicine) => {
    setEditingMedicine(medicine);
    form.setFieldsValue(medicine);
    setIsModalVisible(true);
  };

  const handleDelete = async (id) => {
    Modal.confirm({
      title: 'Xóa thuốc',
      content: 'Bạn có chắc chắn muốn xóa thuốc này?',
      okText: 'Có',
      cancelText: 'Không',
      onOk: async () => {
        try {
          await medicineService.delete(id);
          message.success('Xóa thuốc thành công');
          fetchMedicines();
        } catch (err) {
          message.error('Lỗi khi xóa thuốc');
        }
      },
    });
  };

  const handleSubmit = async (values) => {
    try {
      if (editingMedicine) {
        await medicineService.update({
          ...editingMedicine,
          ...values,
        });
        message.success('Cập nhật thuốc thành công');
      } else {
        await medicineService.create(values);
        message.success('Thêm thuốc thành công');
      }
      setIsModalVisible(false);
      fetchMedicines();
    } catch (err) {
      message.error('Lỗi khi lưu thuốc');
    }
  };

  const columns = [
    {
      title: 'Tên thuốc',
      dataIndex: 'name',
      key: 'name',
    },
    {
      title: 'Nhà sản xuất',
      dataIndex: 'manufacturer',
      key: 'manufacturer',
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
      render: (stock, record) => (
        <span style={{ color: stock <= record.minStockLevel ? 'red' : 'green' }}>
          {stock}
        </span>
      ),
    },
    {
      title: 'Hạn sử dụng',
      dataIndex: 'expiryDate',
      key: 'expiryDate',
      render: (date) => new Date(date).toLocaleDateString('vi-VN'),
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
          Thêm thuốc mới
        </Button>
      </Space>

      <Table
        columns={columns}
        dataSource={medicines}
        loading={loading}
        rowKey="id"
        pagination={{ pageSize: 10 }}
      />

      <Modal
        title={editingMedicine ? 'Sửa thông tin thuốc' : 'Thêm thuốc mới'}
        open={isModalVisible}
        onOk={() => form.submit()}
        onCancel={() => setIsModalVisible(false)}
      >
        <Form form={form} layout="vertical" onFinish={handleSubmit}>
          <Form.Item
            name="name"
            label="Tên thuốc"
            rules={[{ required: true, message: 'Vui lòng nhập tên thuốc' }]}
          >
            <Input />
          </Form.Item>

          <Form.Item
            name="manufacturer"
            label="Nhà sản xuất"
            rules={[{ required: true, message: 'Vui lòng nhập nhà sản xuất' }]}
          >
            <Input />
          </Form.Item>

          <Form.Item
            name="price"
            label="Giá"
            rules={[{ required: true, message: 'Vui lòng nhập giá' }]}
          >
            <InputNumber min={0} step={1000} />
          </Form.Item>

          <Form.Item
            name="currentStock"
            label="Tồn kho"
            rules={[{ required: true, message: 'Vui lòng nhập tồn kho' }]}
          >
            <InputNumber min={0} />
          </Form.Item>

          <Form.Item
            name="minStockLevel"
            label="Mức tồn kho tối thiểu"
            rules={[{ required: true }]}
          >
            <InputNumber min={0} />
          </Form.Item>

          <Form.Item
            name="expiryDate"
            label="Hạn sử dụng"
            rules={[{ required: true, message: 'Vui lòng chọn hạn sử dụng' }]}
          >
            <Input type="date" />
          </Form.Item>

          <Form.Item name="description" label="Mô tả">
            <Input.TextArea rows={3} />
          </Form.Item>
        </Form>
      </Modal>
    </>
  );
}
