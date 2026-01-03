import React, { useState, useEffect } from 'react';
import { Layout, Menu, Badge, Avatar, Dropdown, Button } from 'antd';
import {
  MenuFoldOutlined,
  MenuUnfoldOutlined,
  LogoutOutlined,
  DashboardOutlined,
  MedicineBoxOutlined,
  ShoppingCartOutlined,
  UserOutlined,
  FileTextOutlined,
  BarChartOutlined,
  BellOutlined,
} from '@ant-design/icons';
import { useLocation } from 'react-router-dom';
import useStore from '../store';
import './Layout.css';

const { Sider, Content, Header } = Layout;

export default function MainLayout({ children }) {
  const [collapsed, setCollapsed] = useState(false);
  const [avatarUrl, setAvatarUrl] = useState(null);
  const [displayName, setDisplayName] = useState('Người dùng');
  const { user, logout } = useStore();
  const location = useLocation();

  useEffect(() => {
    // Update display name and avatar from user store
    if (user?.fullName) {
      setDisplayName(user.fullName);
    } else if (user?.username) {
      setDisplayName(user.username);
    }
    
    // Load avatar từ user store hoặc localStorage
    if (user?.avatarUrl) {
      setAvatarUrl(user.avatarUrl);
    } else if (user?.id) {
      const savedAvatar = localStorage.getItem(`avatar_${user.id}`);
      if (savedAvatar) {
        setAvatarUrl(savedAvatar);
      }
    }
  }, [user]);

  const handleLogout = () => {
    logout();
    window.location.href = '/login';
  };

  const menuItems = [
    {
      key: '/dashboard',
      icon: <DashboardOutlined />,
      label: 'Bảng điều khiển',
    },
    {
      key: '/medicines',
      icon: <MedicineBoxOutlined />,
      label: 'Quản lý thuốc',
    },
    {
      key: '/categories',
      icon: <FileTextOutlined />,
      label: 'Quản lý danh mục',
    },
    {
      key: '/customers',
      icon: <UserOutlined />,
      label: 'Quản lý khách hàng',
    },
    {
      key: '/orders',
      icon: <ShoppingCartOutlined />,
      label: 'Quản lý đơn hàng',
    },
    {
      key: '/reports',
      icon: <BarChartOutlined />,
      label: 'Báo cáo & thống kê',
    },
    {
      key: '/inventory',
      icon: <FileTextOutlined />,
      label: 'Quản lý tồn kho',
    },
  ];

  const userMenuItems = [
    {
      key: 'profile',
      label: 'Tài khoản của tôi',
      onClick: () => (window.location.href = '/profile'),
    },
    {
      type: 'divider',
    },
    {
      key: 'logout',
      label: 'Đăng xuất',
      icon: <LogoutOutlined />,
      onClick: handleLogout,
    },
  ];

  return (
    <Layout style={{ minHeight: '100vh' }}>
      <Sider
        trigger={null}
        collapsible
        collapsed={collapsed}
        width={200}
        className="sider"
      >
        <div className="logo">
          <h2>NHÀ THUỐC MINI</h2>
        </div>
        <Menu
          theme="dark"
          mode="inline"
          defaultSelectedKeys={[location.pathname]}
          items={menuItems.map(item => ({
            ...item,
            onClick: () => (window.location.href = item.key),
          }))}
        />
      </Sider>

      <Layout>
        <Header className="header">
          <Button
            type="text"
            icon={collapsed ? <MenuUnfoldOutlined /> : <MenuFoldOutlined />}
            onClick={() => setCollapsed(!collapsed)}
            style={{ fontSize: '16px', width: 64, height: 64 }}
          />

          <div className="header-right">
            <Badge
              count={3}
              showZero
              style={{ cursor: 'pointer' }}
            >
              <BellOutlined style={{ fontSize: '18px' }} />
            </Badge>

            <Dropdown menu={{ items: userMenuItems }}>
              <div style={{ marginLeft: '20px', cursor: 'pointer', display: 'flex', alignItems: 'center' }}>
                <Avatar 
                  size={32}
                  src={avatarUrl}
                  icon={<UserOutlined />}
                  style={{ backgroundColor: '#667eea' }}
                />
                <span style={{ marginLeft: '8px', fontWeight: 500 }}>{displayName}</span>
              </div>
            </Dropdown>
          </div>
        </Header>

        <Content className="content">
          {children}
        </Content>
      </Layout>
    </Layout>
  );
}
