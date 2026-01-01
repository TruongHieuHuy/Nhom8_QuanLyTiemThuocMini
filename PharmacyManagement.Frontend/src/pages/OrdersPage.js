import React, { useState, useEffect } from 'react';
import { Row, Col, Card, Table, Button, Input, List, Typography, message, Tag, Divider, Select, Empty, Space } from 'antd'; // Đã thêm Space vào đây
import { ShoppingCartOutlined, DeleteOutlined, SearchOutlined, UserOutlined, PlusOutlined } from '@ant-design/icons';
import axios from 'axios';

const { Title, Text } = Typography;
const { Option } = Select;

// Đảm bảo cổng này đúng với Backend của bạn
const API_MEDICINES = 'http://localhost:5000/api/Medicines';
const API_ORDERS = 'http://localhost:5000/api/Orders';

export default function OrdersPage() {
  const [medicines, setMedicines] = useState([]);
  const [cart, setCart] = useState([]); // Giỏ hàng
  const [loading, setLoading] = useState(false);
  const [searchText, setSearchText] = useState('');
  const [selectedPayment, setSelectedPayment] = useState('Cash');

  // Lấy User từ LocalStorage để biết ai đang bán (nếu có tính năng login)
  const user = JSON.parse(localStorage.getItem('user'));

  useEffect(() => {
    fetchMedicines();
  }, []);

  const fetchMedicines = async () => {
    try {
      setLoading(true);
      const res = await axios.get(API_MEDICINES);
      setMedicines(res.data);
    } catch (err) {
      message.error('Lỗi tải danh sách thuốc');
    } finally {
      setLoading(false);
    }
  };

  // Thêm vào giỏ hàng
  const addToCart = (medicine) => {
    if (medicine.currentStock <= 0) {
        message.warning('Thuốc này đã hết hàng!');
        return;
    }

    const existingItem = cart.find(item => item.id === medicine.id);
    if (existingItem) {
        if(existingItem.quantity >= medicine.currentStock) {
            message.warning('Không đủ tồn kho!');
            return;
        }
        setCart(cart.map(item => 
            item.id === medicine.id ? { ...item, quantity: item.quantity + 1 } : item
        ));
    } else {
        setCart([...cart, { ...medicine, quantity: 1 }]);
    }
    message.success(`Đã thêm ${medicine.name}`);
  };

  // Xóa khỏi giỏ
  const removeFromCart = (id) => {
    setCart(cart.filter(item => item.id !== id));
  };

  // Tính tổng tiền
  const totalAmount = cart.reduce((sum, item) => sum + (item.price * item.quantity), 0);

  // THANH TOÁN
  const handleCheckout = async () => {
    if (cart.length === 0) {
        message.error('Giỏ hàng đang trống!');
        return;
    }

    try {
        const orderPayload = {
            customerId: null, // Khách lẻ (null)
            employeeId: user?.id || null, // ID nhân viên bán
            paymentMethod: selectedPayment,
            discount: 0,
            tax: 0,
            notes: "Bán lẻ tại quầy",
            // Mapping đúng cấu trúc Backend yêu cầu
            orderDetails: cart.map(item => ({
                medicineId: item.id,
                quantity: item.quantity
            }))
        };

        await axios.post(API_ORDERS, orderPayload);
        
        message.success('Thanh toán thành công! Đã trừ kho.');
        setCart([]); // Xóa giỏ
        fetchMedicines(); // Load lại thuốc để cập nhật tồn kho mới
    } catch (err) {
        console.error(err);
        const errorMsg = err.response?.data?.message || err.message;
        message.error('Lỗi thanh toán: ' + errorMsg);
    }
  };

  return (
    <div style={{ padding: '0 12px' }}>
      <Row gutter={16} style={{ height: 'calc(100vh - 80px)' }}>
        
        {/* CỘT TRÁI: DANH SÁCH THUỐC */}
        <Col span={14} style={{ height: '100%', overflowY: 'auto' }}>
            <Card title="Danh mục thuốc" bordered={false} style={{ height: '100%' }}>
                <Input 
                    prefix={<SearchOutlined />} 
                    placeholder="Tìm tên thuốc, mã vạch..." 
                    size="large"
                    style={{ marginBottom: 16 }}
                    onChange={e => setSearchText(e.target.value.toLowerCase())} 
                    autoFocus
                />
                <Table 
                    dataSource={medicines.filter(m => 
                        m.name.toLowerCase().includes(searchText) || 
                        (m.barcode && m.barcode.includes(searchText))
                    )} 
                    columns={[
                        { title: 'Tên thuốc', dataIndex: 'name', key: 'name', render: t => <b>{t}</b> },
                        { title: 'Đơn giá', dataIndex: 'price', render: v => <span style={{color:'#2ecc71', fontWeight:'bold'}}>{v?.toLocaleString()}</span> },
                        { title: 'Tồn kho', dataIndex: 'currentStock', render: v => <Tag color={v > 0 ? 'blue' : 'red'}>{v}</Tag> },
                        {
                            render: (_, record) => (
                                <Button 
                                    type="primary" 
                                    shape="circle"
                                    icon={<PlusOutlined />}
                                    disabled={record.currentStock <= 0}
                                    onClick={() => addToCart(record)}
                                />
                            )
                        }
                    ]} 
                    rowKey="id" 
                    pagination={{ pageSize: 8 }}
                    size="small"
                />
            </Card>
        </Col>

        {/* CỘT PHẢI: GIỎ HÀNG */}
        <Col span={10} style={{ height: '100%' }}>
            <Card 
                title={<span><ShoppingCartOutlined /> Giỏ hàng ({cart.length})</span>} 
                style={{ height: '100%', display: 'flex', flexDirection: 'column' }}
                headStyle={{ backgroundColor: '#fafafa' }}
                bodyStyle={{ flex: 1, display: 'flex', flexDirection: 'column', padding: 12 }}
            >
                {/* List sản phẩm trong giỏ */}
                <div style={{ flex: 1, overflowY: 'auto', marginBottom: 16 }}>
                    {cart.length === 0 ? (
                        <Empty description="Chưa có sản phẩm nào" style={{ marginTop: 50 }} />
                    ) : (
                        <List
                            itemLayout="horizontal"
                            dataSource={cart}
                            renderItem={item => (
                                <List.Item actions={[
                                    <Button danger type="text" icon={<DeleteOutlined />} onClick={() => removeFromCart(item.id)} />
                                ]}>
                                    <List.Item.Meta
                                        title={item.name}
                                        description={
                                            <Space>
                                                <Tag color="blue">{item.quantity} {item.unit}</Tag>
                                                <span>x {item.price.toLocaleString()}</span>
                                            </Space>
                                        }
                                    />
                                    <div style={{ fontWeight: 'bold' }}>{(item.price * item.quantity).toLocaleString()} đ</div>
                                </List.Item>
                            )}
                        />
                    )}
                </div>

                <Divider style={{ margin: '12px 0' }} />
                
                {/* Phần tổng tiền & Nút thanh toán */}
                <div style={{ backgroundColor: '#f6ffed', padding: 16, borderRadius: 8, border: '1px solid #b7eb8f' }}>
                    <div style={{ display: 'flex', justifyContent: 'space-between', marginBottom: 8 }}>
                        <Text>Khách hàng:</Text> 
                        <Tag icon={<UserOutlined />}>Khách lẻ</Tag>
                    </div>
                    <div style={{ display: 'flex', justifyContent: 'space-between', marginBottom: 16 }}>
                        <Text>Hình thức:</Text>
                        <Select defaultValue="Cash" size="small" style={{ width: 120 }} onChange={setSelectedPayment}>
                            <Option value="Cash">Tiền mặt</Option>
                            <Option value="Banking">Chuyển khoản</Option>
                        </Select>
                    </div>
                    
                    <div style={{ display: 'flex', justifyContent: 'space-between', marginBottom: 16 }}>
                        <Title level={4} style={{ margin: 0 }}>TỔNG CỘNG:</Title>
                        <Title level={3} type="danger" style={{ margin: 0 }}>{totalAmount.toLocaleString()} VNĐ</Title>
                    </div>

                    <Button type="primary" block size="large" onClick={handleCheckout} disabled={cart.length === 0} style={{ height: 50, fontSize: 18 }}>
                        THANH TOÁN
                    </Button>
                </div>
            </Card>
        </Col>
      </Row>
    </div>
  );
}