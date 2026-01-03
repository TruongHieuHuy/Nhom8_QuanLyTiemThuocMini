
import React, { useState } from 'react';
import { Form, Input, Button, Card, Row, Col, message, Checkbox } from 'antd';
import { LockOutlined, UserOutlined } from '@ant-design/icons';
import useStore from '../store';
import './LoginPage.css';

export default function LoginPage() {
  const [loading, setLoading] = useState(false);
  const { setUser } = useStore();
  const [form] = Form.useForm();

  const handleSubmit = async (values) => {
    try {
      setLoading(true);
      // Mock login - trong thực tế cần call API
      const mockUser = {
        id: 1,
        fullName: values.username,
        email: values.username + '@pharmacy.com',
        role: 'Admin',
      };

      // Lưu user và token
      localStorage.setItem('user', JSON.stringify(mockUser));
      localStorage.setItem('token', 'mock-jwt-token');

      setUser(mockUser);
      message.success('Đăng nhập thành công');
      window.location.href = '/dashboard';
    } catch (err) {
      message.error('Lỗi khi đăng nhập');
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
                rules={[
                  {
                    required: true,
                    message: 'Vui lòng nhập tên đăng nhập',
                  },
                ]}
              >
                <Input
                  prefix={<UserOutlined />}
                  placeholder="Tên đăng nhập"
                  size="large"
                />
              </Form.Item>

              <Form.Item
                name="password"
                rules={[
                  {
                    required: true,
                    message: 'Vui lòng nhập mật khẩu',
                  },
                ]}
              >
                <Input.Password
                  prefix={<LockOutlined />}
                  placeholder="Mật khẩu"
                  size="large"
                />
              </Form.Item>

              <Form.Item name="remember" valuePropName="checked" initialValue={true}>
                <Checkbox>Nhớ tôi</Checkbox>
              </Form.Item>

              <Form.Item>
                <Button
                  type="primary"
                  htmlType="submit"
                  block
                  size="large"
                  loading={loading}
                >
                  Đăng nhập
                </Button>
              </Form.Item>
            </Form>

            <div className="login-footer">
              <p>Demo: Để trống để đăng nhập</p>
            </div>
          </Card>
        </Col>
      </Row>
    </div>
  );
}
