import React from 'react';
import { BrowserRouter as Router, Routes, Route, Navigate } from 'react-router-dom';
import { ConfigProvider } from 'antd';
import viVN from 'antd/locale/vi_VN';
import Layout from './components/Layout';
import Dashboard from './pages/Dashboard';
import MedicinesPage from './pages/MedicinesPage';
import CustomersPage from './pages/CustomersPage';
import OrdersPage from './pages/OrdersPage';
import ReportsPage from './pages/ReportsPage';
import PaymentHistoryPage from './pages/PaymentHistoryPage';
import LoginPage from './pages/LoginPage';
import ProfilePage from './pages/ProfilePage';
import CategoriesPage from './pages/CategoriesPage';
import useStore from './store';
import './App.css';

function App() {
  const { user } = useStore();

  return (
    <ConfigProvider locale={viVN}>
      <Router>
        <Routes>
          <Route
            path="/login"
            element={!user ? <LoginPage /> : <Navigate to="/dashboard" />}
          />

          <Route
            path="/dashboard"
            element={
              user ? (
                <Layout>
                  <Dashboard />
                </Layout>
              ) : (
                <Navigate to="/login" />
              )
            }
          />

          <Route
            path="/medicines"
            element={
              user ? (
                <Layout>
                  <MedicinesPage />
                </Layout>
              ) : (
                <Navigate to="/login" />
              )
            }
          />

          <Route
            path="/customers"
            element={
              user ? (
                <Layout>
                  <CustomersPage />
                </Layout>
              ) : (
                <Navigate to="/login" />
              )
            }
          />

          <Route
            path="/orders"
            element={
              user ? (
                <Layout>
                  <OrdersPage />
                </Layout>
              ) : (
                <Navigate to="/login" />
              )
            }
          />

          

          <Route
            path="/payments"
            element={
              user ? (
                <Layout>
                  <PaymentHistoryPage />
                </Layout>
              ) : (
                <Navigate to="/login" />
              )
            }
          />
<Route
            path="/reports"
            element={
              user ? (
                <Layout>
                  <ReportsPage />
                </Layout>
              ) : (
                <Navigate to="/login" />
              )
            }
          />

          <Route
            path="/profile"
            element={
              user ? (
                <Layout>
                  <ProfilePage />
                </Layout>
              ) : (
                <Navigate to="/login" />
              )
            }
          />

          <Route
            path="/categories"
            element={
              user ? (
                <Layout>
                  <CategoriesPage />
                </Layout>
              ) : (
                <Navigate to="/login" />
              )
            }
          />

          <Route path="/" element={<Navigate to="/dashboard" />} />
          <Route path="*" element={<Navigate to="/dashboard" />} />
        </Routes>
      </Router>
    </ConfigProvider>
  );
}

export default App;
