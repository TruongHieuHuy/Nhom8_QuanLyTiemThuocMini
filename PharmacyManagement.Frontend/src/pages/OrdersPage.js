import React, { useState, useEffect } from 'react';
import { Table, Button, Modal, Form, Input, InputNumber, Select, Space, message, DatePicker } from 'antd';
import { PlusOutlined, EditOutlined, DeleteOutlined, PrinterOutlined } from '@ant-design/icons';
import { orderService, medicineService, customerService } from '../services';
import useStore from '../store';
import dayjs from 'dayjs';

export default function OrdersPage() {
  const [orders, setOrders] = useState([]);
  const [medicines, setMedicines] = useState([]);
  const [customers, setCustomers] = useState([]);
  const [loading, setLoading] = useState(false);
  const [isModalVisible, setIsModalVisible] = useState(false);
  const [form] = Form.useForm();
  const { setError } = useStore();

  useEffect(() => {
    fetchData();
  }, []);

  const fetchData = async () => {
    try {
      setLoading(true);
      const [ordersData, medicinesData, customersData] = await Promise.all([
        orderService.getAll(),
        medicineService.getAll(),
        customerService.getAll(),
      ]);
      setOrders(ordersData);
      setMedicines(medicinesData);
      setCustomers(customersData);
    } catch (err) {
      message.error('Lỗi khi tải dữ liệu');
      setError(err.message);
    } finally {
      setLoading(false);
    }
  };

  const handleAdd = () => {
    form.resetFields();
    setIsModalVisible(true);
  };

  const handleSubmit = async (values) => {
    try {
      await orderService.create(values);
      message.success('Tạo đơn hàng thành công');
      setIsModalVisible(false);
      fetchData();
    } catch (err) {
      message.error('Lỗi khi tạo đơn hàng');
    }
  };

  const handlePrint = (order) => {
    window.print();
  };

  const columns = [
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
      title: 'Ngày tạo',
      dataIndex: 'orderDate',
      key: 'orderDate',
      render: (date) => new Date(date).toLocaleDateString('vi-VN'),
    },
    {
      title: 'Tổng tiền',
      dataIndex: 'total',
      key: 'total',
      render: (total) => `${total.toLocaleString('vi-VN')} đ`,
    },
    {
      title: 'Phương thức thanh toán',
      dataIndex: 'paymentMethod',
      key: 'paymentMethod',
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
    {
      title: 'Hành động',
      key: 'action',
      render: (_, record) => (
        <Space>
          <Button
            type="primary"
            size="small"
            icon={<PrinterOutlined />}
            onClick={() => handlePrint(record)}
          >
            In
          </Button>
        </Space>
      ),
    },
  ];

  return (
    <div>
      <h1>Quản lý đơn hàng</h1>

      <Space style={{ marginBottom: '16px' }}>
        <Button type="primary" icon={<PlusOutlined />} onClick={handleAdd}>
          Tạo đơn hàng mới
        </Button>
      </Space>

      <Table
        columns={columns}
        dataSource={orders}
        loading={loading}
        rowKey="id"
        pagination={{ pageSize: 10 }}
      />

      <Modal
        title="Tạo đơn hàng mới"
        visible={isModalVisible}
        onOk={() => form.submit()}
        onCancel={() => setIsModalVisible(false)}
        width={900}
      >
        <Form form={form} layout="vertical" onFinish={handleSubmit}>
          <Form.Item
            name="customerId"
            label="Khách hàng"
            rules={[{ required: true, message: 'Vui lòng chọn khách hàng' }]}
          >
            <Select
              placeholder="Chọn khách hàng"
              options={customers.map((c) => ({
                label: c.name,
                value: c.id,
              }))}
            />
          </Form.Item>

          <Form.Item
            name="paymentMethod"
            label="Phương thức thanh toán"
            rules={[{ required: true }]}
          >
            <Select
              options={[
                { label: 'Tiền mặt', value: 'Cash' },
                { label: 'Thẻ ngân hàng', value: 'Card' },
                { label: 'Chuyển khoản', value: 'Transfer' },
              ]}
            />
          </Form.Item>

          <Form.Item
            name="orderDetails"
            label="Chi tiết đơn hàng"
            rules={[{ required: true, message: 'Vui lòng thêm ít nhất một sản phẩm' }]}
          >
            <Select mode="multiple" placeholder="Chọn thuốc" />
          </Form.Item>

          <Form.Item name="discount" label="Giảm giá">
            <InputNumber min={0} />
          </Form.Item>

          <Form.Item name="tax" label="Thuế">
            <InputNumber min={0} />
          </Form.Item>

          <Form.Item name="notes" label="Ghi chú">
            <Input.TextArea rows={3} />
          </Form.Item>
        </Form>
      </Modal>
    </div>
  );
}
