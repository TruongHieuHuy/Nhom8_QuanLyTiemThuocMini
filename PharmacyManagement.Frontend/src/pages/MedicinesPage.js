import React, { useState, useEffect } from 'react';
import { Table, Button, Space, Modal, Form, Input, InputNumber, Select, message, Tag, Card, Row, Col, DatePicker } from 'antd';
import { PlusOutlined, EditOutlined, DeleteOutlined, SearchOutlined, ReloadOutlined } from '@ant-design/icons';
import axios from 'axios';
import dayjs from 'dayjs';

const API_URL = 'http://localhost:5000/api/Medicines';
const GROUP_API_URL = 'http://localhost:5000/api/MedicineGroups'; 

export default function MedicinesPage() {
  const [medicines, setMedicines] = useState([]);
  const [groups, setGroups] = useState([]);
  const [loading, setLoading] = useState(false);
  const [isModalVisible, setIsModalVisible] = useState(false);
  const [editingMedicine, setEditingMedicine] = useState(null); 
  const [form] = Form.useForm();
  const [searchText, setSearchText] = useState('');

  useEffect(() => {
    fetchMedicines();
    fetchGroups();
  }, []);

  const fetchMedicines = async () => {
    try {
      setLoading(true);
      const res = await axios.get(API_URL);
      setMedicines(res.data);
    } catch (err) {
      message.error('Không thể tải danh sách thuốc!');
    } finally {
      setLoading(false);
    }
  };

  const fetchGroups = async () => {
    try {
      const res = await axios.get(GROUP_API_URL);
      setGroups(res.data);
    } catch (err) {
      console.error('Lỗi tải nhóm thuốc:', err);
    }
  };

  const handleOk = async () => {
    try {
      const values = await form.validateFields();
      
      const payload = {
          ...values,
          isActive: true,
          expiryDate: values.expiryDate ? values.expiryDate.toISOString() : new Date().toISOString(),
          ...(editingMedicine && { id: editingMedicine.id })
      };

      if (editingMedicine) {
        await axios.put(API_URL, payload);
        message.success('Cập nhật thành công!');
      } else {
        await axios.post(API_URL, { ...payload, createdDate: new Date() });
        message.success('Thêm thuốc mới thành công!');
      }

      setIsModalVisible(false);
      form.resetFields();
      fetchMedicines(); 
    } catch (err) {
      console.error(err);
      if(err.response && err.response.data && err.response.data.errors) {
          const errorMsg = Object.values(err.response.data.errors).flat().join(', ');
          message.error('Thiếu thông tin: ' + errorMsg);
      } else {
          message.error('Có lỗi xảy ra, vui lòng kiểm tra lại!');
      }
    }
  };

  const handleDelete = async (id) => {
    Modal.confirm({
      title: 'Xác nhận xóa',
      content: 'Bạn có chắc muốn xóa thuốc này không?',
      okText: 'Xóa',
      okType: 'danger',
      cancelText: 'Hủy',
      onOk: async () => {
        try {
          await axios.delete(`${API_URL}/${id}`);
          message.success('Đã xóa thuốc!');
          fetchMedicines();
        } catch (err) {
          message.error('Xóa thất bại!');
        }
      },
    });
  };

  const openModal = (medicine = null) => {
    setEditingMedicine(medicine);
    if (medicine) {
      form.setFieldsValue({
          ...medicine,
          expiryDate: medicine.expiryDate ? dayjs(medicine.expiryDate) : null
      }); 
    } else {
      form.resetFields();
    }
    setIsModalVisible(true);
  };

  const columns = [
    { title: 'Tên thuốc', dataIndex: 'name', key: 'name', render: text => <b>{text}</b> },
    { title: 'Mã vạch', dataIndex: 'barcode', key: 'barcode', responsive: ['lg'] },
    { 
      title: 'Giá bán', dataIndex: 'price', key: 'price', align: 'right',
      render: val => <span style={{color: '#2ecc71', fontWeight: 'bold'}}>{val?.toLocaleString('vi-VN')} đ</span>
    },
    { 
      title: 'Tồn', dataIndex: 'currentStock', key: 'currentStock', align: 'center',
      render: val => <Tag color={val < 10 ? 'red' : 'blue'}>{val}</Tag>
    },
    { title: 'Đơn vị', dataIndex: 'unit', key: 'unit', align: 'center' },
    {
      title: 'Hành động', key: 'action', align: 'center',
      render: (_, record) => (
        <Space>
          <Button icon={<EditOutlined />} type="primary" ghost size="small" onClick={() => openModal(record)} />
          <Button icon={<DeleteOutlined />} danger size="small" onClick={() => handleDelete(record.id)} />
        </Space>
      ),
    },
  ];

  return (
    <div>
      <Card 
        title="Danh sách thuốc" 
        extra={
            <Space>
                <Button icon={<ReloadOutlined />} onClick={fetchMedicines}>Làm mới</Button>
                <Button type="primary" icon={<PlusOutlined />} onClick={() => openModal(null)}>Thêm thuốc</Button>
            </Space>
        }
      >
        <Input 
            placeholder="Tìm thuốc..." 
            prefix={<SearchOutlined />} 
            style={{ marginBottom: 16, maxWidth: 300 }} 
            onChange={e => setSearchText(e.target.value.toLowerCase())}
        />
        
        <Table 
            columns={columns} 
            dataSource={medicines.filter(m => m.name?.toLowerCase().includes(searchText))} 
            rowKey="id" 
            loading={loading} 
            pagination={{ pageSize: 6 }}
        />
      </Card>

      <Modal
        title={editingMedicine ? "Sửa thuốc" : "Thêm thuốc mới"}
        open={isModalVisible}
        onOk={handleOk}
        onCancel={() => setIsModalVisible(false)}
        width={800}
      >
        <Form form={form} layout="vertical">
            <Row gutter={16}>
                <Col span={12}>
                    <Form.Item name="name" label="Tên thuốc" rules={[{ required: true }]}>
                        <Input placeholder="Ví dụ: Panadol Extra" />
                    </Form.Item>
                </Col>
                <Col span={12}>
                    <Form.Item name="medicineGroupId" label="Nhóm thuốc" rules={[{ required: true }]}>
                        <Select placeholder="Chọn nhóm">
                            {groups.map(g => <Select.Option key={g.id} value={g.id}>{g.name}</Select.Option>)}
                        </Select>
                    </Form.Item>
                </Col>
            </Row>

            <Row gutter={16}>
                <Col span={12}>
                    <Form.Item name="barcode" label="Mã vạch" rules={[{ required: true, message: 'Cần nhập mã vạch' }]}>
                        <Input placeholder="Quét hoặc nhập mã..." />
                    </Form.Item>
                </Col>
                <Col span={12}>
                    <Form.Item name="expiryDate" label="Hạn sử dụng" rules={[{ required: true, message: 'Cần nhập hạn dùng' }]}>
                        <DatePicker style={{ width: '100%' }} format="DD/MM/YYYY" placeholder="Chọn ngày hết hạn" />
                    </Form.Item>
                </Col>
            </Row>

            <Row gutter={16}>
                <Col span={8}>
                    <Form.Item name="price" label="Giá bán" rules={[{ required: true }]}>
                        <InputNumber style={{ width: '100%' }} formatter={value => `${value}`.replace(/\B(?=(\d{3})+(?!\d))/g, ',')} min={0} />
                    </Form.Item>
                </Col>
                <Col span={8}>
                    <Form.Item name="currentStock" label="Tồn kho">
                        <InputNumber style={{ width: '100%' }} min={0} />
                    </Form.Item>
                </Col>
                <Col span={8}>
                    <Form.Item name="unit" label="Đơn vị" rules={[{ required: true }]}>
                        <Input placeholder="Viên, Hộp..." />
                    </Form.Item>
                </Col>
            </Row>

            <Row gutter={16}>
                <Col span={12}>
                    <Form.Item name="usage" label="Cách dùng" rules={[{ required: true, message: 'Nhập cách dùng' }]}>
                        <Input placeholder="Uống sau ăn..." />
                    </Form.Item>
                </Col>
                <Col span={12}>
                    <Form.Item name="dosage" label="Liều lượng" rules={[{ required: true, message: 'Nhập liều lượng' }]}>
                        <Input placeholder="Ngày 2 viên..." />
                    </Form.Item>
                </Col>
            </Row>

            {/* --- BỔ SUNG Ô NHÀ SẢN XUẤT --- */}
            <Form.Item name="manufacturer" label="Nhà sản xuất" rules={[{ required: true, message: 'Vui lòng nhập Nhà sản xuất' }]}>
                <Input placeholder="Ví dụ: Dược Hậu Giang, Sanofi..." />
            </Form.Item>

            {/* --- BẮT BUỘC NHẬP MÔ TẢ --- */}
            <Form.Item name="description" label="Mô tả / Công dụng" rules={[{ required: true, message: 'Vui lòng nhập mô tả' }]}>
                <Input.TextArea rows={2} placeholder="Nhập công dụng thuốc..." />
            </Form.Item>
        </Form>
      </Modal>
    </div>
  );
}