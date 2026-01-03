import React, { useState, useEffect } from 'react';
import { Form, Input, Button, Card, Avatar, Upload, message, Spin, Row, Col } from 'antd';
import { UserOutlined, CameraOutlined } from '@ant-design/icons';
import useStore from '../store';
import axios from '../utils/axiosConfig';
import './ProfilePage.css';

export default function ProfilePage() {
  const [form] = Form.useForm();
  const [loading, setLoading] = useState(false);
  const [submitting, setSubmitting] = useState(false);
  const [profile, setProfile] = useState(null);
  const [avatarPreview, setAvatarPreview] = useState(null);
  const { user, setUser } = useStore();

  // Lấy thông tin profile khi component mount
  useEffect(() => {
    fetchProfile();
  }, []);

  const fetchProfile = async () => {
    setLoading(true);
    try {
      const response = await axios.get('/profile');
      setProfile(response.data.data);
      setAvatarPreview(response.data.data.avatarUrl);

      // Set form values
      form.setFieldsValue({
        username: response.data.data.username,
        email: response.data.data.email,
        fullName: response.data.data.fullName,
        phoneNumber: response.data.data.phoneNumber,
        address: response.data.data.address,
      });
    } catch (error) {
      message.error('Lỗi khi lấy thông tin tài khoản: ' + (error.response?.data?.message || error.message));
      console.error('Fetch error:', error);
    } finally {
      setLoading(false);
    }
  };

  const handleUpdateProfile = async (values) => {
    setSubmitting(true);
    try {
      const response = await axios.put('/profile', {
        fullName: values.fullName,
        email: values.email,
        phoneNumber: values.phoneNumber,
        address: values.address,
      });

      message.success(response.data.message || 'Cập nhật thông tin thành công');

      // Cập nhật store
      const updatedUser = {
        ...user,
        fullName: values.fullName,
        email: values.email,
      };
      localStorage.setItem('user', JSON.stringify(updatedUser));
      setUser(updatedUser);

      fetchProfile();
    } catch (error) {
      message.error('Lỗi khi cập nhật thông tin: ' + (error.response?.data?.message || error.message));
      console.error('Update error:', error);
    } finally {
      setSubmitting(false);
    }
  };

  const handleAvatarUpload = async (file) => {
    // Kiểm tra định dạng
    const allowedTypes = ['image/jpeg', 'image/png', 'image/gif', 'image/jpg'];
    if (!allowedTypes.includes(file.type)) {
      message.error('Chỉ hỗ trợ định dạng: jpg, jpeg, png, gif');
      return false;
    }

    // Kiểm tra kích thước (5MB)
    if (file.size > 5 * 1024 * 1024) {
      message.error('Kích thước file không được vượt quá 5MB');
      return false;
    }

    // Preview ảnh
    const reader = new FileReader();
    reader.onload = (e) => {
      setAvatarPreview(e.target.result);
    };
    reader.readAsDataURL(file);

    // Upload ảnh
    const formData = new FormData();
    formData.append('file', file);

    setSubmitting(true);
    try {
      const response = await axios.post('/profile/upload-avatar', formData, {
        headers: {
          'Content-Type': 'multipart/form-data',
        },
      });

      message.success(response.data.message || 'Cập nhật ảnh đại diện thành công');

      // Cập nhật preview với URL từ server
      const fullAvatarUrl = response.data.data.avatarUrl;
      setAvatarPreview(fullAvatarUrl);

      // Cập nhật store
      const updatedUser = {
        ...user,
        avatarUrl: fullAvatarUrl,
      };
      localStorage.setItem('user', JSON.stringify(updatedUser));
      setUser(updatedUser);

      fetchProfile();
    } catch (error) {
      message.error('Lỗi khi upload ảnh: ' + (error.response?.data?.message || error.message));
      console.error('Upload error:', error);
    } finally {
      setSubmitting(false);
    }

    return false; // Prevent default upload behavior
  };

  if (loading) {
    return (
      <div style={{ display: 'flex', justifyContent: 'center', alignItems: 'center', height: '80vh' }}>
        <Spin size="large" tip="Đang tải thông tin..." />
      </div>
    );
  }

  return (
    <div style={{ maxWidth: '800px', margin: '20px auto' }}>
      <Card title="Tài khoản của tôi" style={{ marginBottom: '20px' }}>
        <Row gutter={[16, 16]}>
          <Col xs={24} sm={8} style={{ textAlign: 'center' }}>
            <div style={{ marginBottom: '20px' }}>
              <Avatar
                size={120}
                src={avatarPreview}
                icon={!avatarPreview && <UserOutlined />}
                style={{ backgroundColor: '#1890ff' }}
              />
            </div>

            <Upload
              beforeUpload={handleAvatarUpload}
              accept="image/jpeg,image/png,image/gif,image/jpg"
              maxCount={1}
              showUploadList={false}
            >
              <Button
                type="primary"
                icon={<CameraOutlined />}
                loading={submitting}
                block
              >
                Cập nhật ảnh
              </Button>
            </Upload>
          </Col>

          <Col xs={24} sm={16}>
            <Form
              form={form}
              layout="vertical"
              onFinish={handleUpdateProfile}
            >
              <Form.Item
                label="Tên đăng nhập"
                name="username"
              >
                <Input disabled />
              </Form.Item>

              <Form.Item
                label="Họ và tên"
                name="fullName"
                rules={[{ required: true, message: 'Vui lòng nhập họ và tên' }]}
              >
                <Input placeholder="Nhập họ và tên" />
              </Form.Item>

              <Form.Item
                label="Email"
                name="email"
                rules={[
                  { type: 'email', message: 'Email không hợp lệ' }
                ]}
              >
                <Input placeholder="Nhập email" type="email" />
              </Form.Item>

              <Form.Item
                label="Số điện thoại"
                name="phoneNumber"
              >
                <Input placeholder="Nhập số điện thoại" />
              </Form.Item>

              <Form.Item
                label="Địa chỉ"
                name="address"
              >
                <Input.TextArea placeholder="Nhập địa chỉ" rows={3} />
              </Form.Item>

              <Form.Item>
                <Button
                  type="primary"
                  htmlType="submit"
                  loading={submitting}
                  size="large"
                  block
                >
                  Cập nhật thông tin
                </Button>
              </Form.Item>
            </Form>
          </Col>
        </Row>
      </Card>
    </div>
  );
}
