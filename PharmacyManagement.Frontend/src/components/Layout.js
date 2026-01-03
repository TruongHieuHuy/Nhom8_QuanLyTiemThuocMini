import React, { useState, useEffect } from 'react';
import { Layout, Menu, Badge, Avatar, Dropdown, Button, List, Empty, Spin } from 'antd';
import {
  MenuFoldOutlined,
  MenuUnfoldOutlined,
  LogoutOutlined,
  DashboardOutlined,
  MedicineBoxOutlined,
  ShoppingCartOutlined,
  UserOutlined,
  BarChartOutlined,
  BellOutlined,
  ShopOutlined,
  FileTextOutlined,
  CheckOutlined,
} from '@ant-design/icons';
import { useLocation } from 'react-router-dom';
import useStore from '../store';
import axios from '../utils/axiosConfig';
import './Layout.css';

const { Sider, Content, Header } = Layout;

export default function MainLayout({ children }) {
  const [collapsed, setCollapsed] = useState(false);
  const [avatarUrl, setAvatarUrl] = useState(null);
  const [displayName, setDisplayName] = useState('Người dùng');
  const [notifications, setNotifications] = useState([]);
  const [unreadCount, setUnreadCount] = useState(0);
  const [loadingNotifications, setLoadingNotifications] = useState(false);
  const [notificationVisible, setNotificationVisible] = useState(false);
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

    // Load unread notification count
    fetchUnreadCount();

    // Poll notifications every 5 minutes
    const interval = setInterval(fetchUnreadCount, 5 * 60 * 1000);
    return () => clearInterval(interval);
  }, [user]);

  const fetchUnreadCount = async () => {
    try {
      const response = await axios.get('/Notifications/unread-count');
      setUnreadCount(response.data.count);
    } catch (error) {
      console.error('Error fetching unread count:', error);
    }
  };

  const fetchNotifications = async () => {
    setLoadingNotifications(true);
    try {
      const response = await axios.get('/Notifications');
      setNotifications(response.data);
    } catch (error) {
      console.error('Error fetching notifications:', error);
    } finally {
      setLoadingNotifications(false);
    }
  };

  const handleMarkAsRead = async (id) => {
    try {
      await axios.put(`/Notifications/${id}/mark-read`);
      setNotifications(prev => 
        prev.map(n => n.id === id ? { ...n, isRead: true } : n)
      );
      fetchUnreadCount();
    } catch (error) {
      console.error('Error marking notification as read:', error);
    }
  };

  const handleMarkAllAsRead = async () => {
    try {
      await axios.put('/Notifications/mark-all-read');
      setNotifications(prev => 
        prev.map(n => ({ ...n, isRead: true }))
      );
      fetchUnreadCount();
    } catch (error) {
      console.error('Error marking all as read:', error);
    }
  };

  const handleResetAllUnread = async () => {
    try {
      console.log('Calling reset API...');
      const response = await axios.put('/Notifications/reset-all-unread');
      console.log('Reset response:', response.data);
      alert(response.data.message + ` (${response.data.count} thông báo)`);
      
      // Đóng dropdown trước khi reload
      setNotificationVisible(false);
      
      // Reload notifications và count
      await fetchNotifications();
      await fetchUnreadCount();
    } catch (error) {
      console.error('Error resetting notifications:', error);
      alert('Lỗi khi reset thông báo: ' + (error.response?.data?.message || error.message));
    }
  };

  const handleNotificationDropdownVisibleChange = (visible) => {
    setNotificationVisible(visible);
    if (visible) {
      fetchNotifications();
    }
  };

  const formatNotificationTime = (date) => {
    const now = new Date();
    const notifDate = new Date(date);
    const diffMs = now - notifDate;
    const diffMins = Math.floor(diffMs / 60000);
    const diffHours = Math.floor(diffMins / 60);
    const diffDays = Math.floor(diffHours / 24);

    if (diffMins < 1) return 'Vừa xong';
    if (diffMins < 60) return `${diffMins} phút trước`;
    if (diffHours < 24) return `${diffHours} giờ trước`;
    if (diffDays < 7) return `${diffDays} ngày trước`;
    return notifDate.toLocaleDateString('vi-VN');
  };

  const notificationMenu = (
    <div style={{ width: 400, maxHeight: 500, overflow: 'auto', backgroundColor: 'white', borderRadius: 8, boxShadow: '0 2px 8px rgba(0,0,0,0.15)' }}>
      <div style={{ padding: '12px 16px', borderBottom: '1px solid #f0f0f0', display: 'flex', justifyContent: 'space-between', alignItems: 'center' }}>
        <h3 style={{ margin: 0, fontSize: 16 }}>Thông báo</h3>
        <div style={{ display: 'flex', gap: 8 }}>
          {/* Button tạm để reset - XÓA SAU KHI QUAY VIDEO */}
          <Button 
            type="default" 
            size="small" 
            onClick={handleResetAllUnread}
            danger
          >
            🔄 Reset (Demo)
          </Button>
          {unreadCount > 0 && (
            <Button 
              type="link" 
              size="small" 
              onClick={handleMarkAllAsRead}
              icon={<CheckOutlined />}
            >
              Đánh dấu tất cả đã đọc
            </Button>
          )}
        </div>
      </div>
      {loadingNotifications ? (
        <div style={{ padding: 40, textAlign: 'center' }}>
          <Spin />
        </div>
      ) : notifications.length === 0 ? (
        <Empty 
          description="Không có thông báo" 
          style={{ padding: 40 }}
        />
      ) : (
        <List
          dataSource={notifications}
          renderItem={item => (
            <List.Item
              onClick={() => {
                // Không cho phép đánh dấu đã đọc thông báo Stock (chỉ tự động xóa khi nhập kho)
                if (!item.isRead && item.notificationType !== 'Stock') {
                  handleMarkAsRead(item.id);
                }
              }}
              style={{
                padding: '12px 16px',
                cursor: (item.isRead || item.notificationType === 'Stock') ? 'default' : 'pointer',
                backgroundColor: item.isRead ? 'white' : '#fff7e6',
                borderLeft: item.isRead ? 'none' : (item.notificationType === 'Stock' ? '3px solid #ff4d4f' : '3px solid #faad14'),
              }}
            >
              <List.Item.Meta
                title={
                  <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center' }}>
                    <span style={{ fontWeight: item.isRead ? 'normal' : 'bold' }}>
                      {item.title}
                    </span>
                    {!item.isRead && (
                      <Badge 
                        color={item.notificationType === 'Stock' ? 'red' : 'orange'} 
                        text={item.notificationType === 'Stock' ? 'Cần xử lý' : ''}
                        style={{ fontSize: 11 }}
                      />
                    )}
                  </div>
                }
                description={
                  <>
                    <div>{item.message}</div>
                    <div style={{ fontSize: 12, color: '#999', marginTop: 4 }}>
                      {formatNotificationTime(item.createdDate)}
                      {item.notificationType === 'Stock' && (
                        <span style={{ color: '#ff4d4f', marginLeft: 8 }}>
                          • Tự động xóa khi nhập kho
                        </span>
                      )}
                    </div>
                  </>
                }
              />
            </List.Item>
          )}
        />
      )}
    </div>
  );

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
      key: '/payments',
      icon: <FileTextOutlined />,
      label: 'Lịch sử thanh toán',
    },
    {
      key: '/suppliers',
      icon: <ShopOutlined />,
      label: 'Quản lý nhà cung cấp',
    },
    {
      key: '/reports',
      icon: <BarChartOutlined />,
      label: 'Báo cáo & thống kê',
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
            <Dropdown
              dropdownRender={() => notificationMenu}
              trigger={['click']}
              open={notificationVisible}
              onOpenChange={handleNotificationDropdownVisibleChange}
              placement="bottomRight"
            >
              <div>
                <Badge
                  count={unreadCount}
                  style={{ cursor: 'pointer' }}
                >
                  <BellOutlined style={{ fontSize: '18px' }} />
                </Badge>
              </div>
            </Dropdown>

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
