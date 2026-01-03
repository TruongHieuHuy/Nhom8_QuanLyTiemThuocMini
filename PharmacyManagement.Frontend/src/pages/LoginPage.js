import React, { useState } from 'react';
import { Form, Input, Button, Card, Row, Col, message, Checkbox } from 'antd';
import { LockOutlined, UserOutlined } from '@ant-design/icons';
import useStore from '../store';
import axios from 'axios'; 
import './LoginPage.css';

// ⚠️ LƯU Ý: Thay 7198 bằng cổng Backend đang chạy của bạn
const API_URL = 'http://localhost:5000/api/Auth/login'; 

export default function LoginPage() {
  const [loading, setLoading] = useState(false);
  const { setUser } = useStore();
  const [form] = Form.useForm();

  const handleSubmit = async (values) => {
    try {
      setLoading(true);

      // 1. GỌI API THẬT
      const response = await axios.post(API_URL, {
        username: values.username,
        password: values.password
      });

      // 2. Xử lý dữ liệu trả về
      const { token, user } = response.data;

      const userData = {
        id: user.id,
        username: user.username,
        fullName: user.fullName || user.username,
        email: user.email,
        phoneNumber: user.phoneNumber,
        avatarUrl: user.avatarUrl,
        role: user.role
      };

      // 3. Lưu vào Local Storage
      localStorage.setItem('token', token);
      localStorage.setItem('role', user.role);
      localStorage.setItem('user', JSON.stringify(userData));

      // 4. Cập nhật State
      setUser(userData);
      
      message.success(`Xin chào ${userData.fullName}`);
      
      // 5. Chuyển trang
      window.location.href = '/dashboard';

    } catch (err) {
      // --- ĐOẠN CODE SỬA LỖI AN TOÀN ---
      console.error(err);
      
      if (err.response && err.response.data) {
         // Lỗi do Backend trả về (ví dụ: Sai pass, tài khoản khóa...)
         message.error(err.response.data.message || 'Sai tài khoản hoặc mật khẩu!');
      } else if (err.request) {
         // Lỗi do không kết nối được (Backend chưa chạy hoặc chưa mở CORS)
         message.error('Không thể kết nối đến Server. Hãy kiểm tra Backend đã chạy chưa?');
      } else {
         message.error('Lỗi đăng nhập không xác định');
      }
      // ----------------------------------

    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="login-container">
      <Row justify="center" align="middle" style={{ minHeight: '100vh' }}>
        <Col xs={20} sm={16} md={12} lg={8} xl={6}>
          <Card className="login-card">
            <div className="login-header">
              <h1>Pharmacy Management</h1>
              <p>Hệ thống quản lý tiệm thuốc</p>
            </div>

            <Form
              form={form}
              layout="vertical"
              onFinish={handleSubmit}
              autoComplete="off"
            >
              <Form.Item
                name="username"
                rules={[{ required: true, message: 'Vui lòng nhập tên đăng nhập' }]}
              >
                <Input prefix={<UserOutlined />} placeholder="Tên đăng nhập" size="large" />
              </Form.Item>

              <Form.Item
                name="password"
                rules={[{ required: true, message: 'Vui lòng nhập mật khẩu' }]}
              >
                <Input.Password prefix={<LockOutlined />} placeholder="Mật khẩu" size="large" />
              </Form.Item>

              <Form.Item name="remember" valuePropName="checked" initialValue={true}>
                <Checkbox>Nhớ tôi</Checkbox>
              </Form.Item>

              <Form.Item>
                <Button type="primary" htmlType="submit" block size="large" loading={loading}>
                  Đăng nhập
                </Button>
              </Form.Item>
            </Form>
          </Card>
        </Col>
      </Row>
    </div>
  );
}