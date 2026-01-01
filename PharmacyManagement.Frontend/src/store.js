import create from 'zustand';

const useStore = create((set) => ({
  user: JSON.parse(localStorage.getItem('user')) || null,
  setUser: (user) => {
    localStorage.setItem('user', JSON.stringify(user));
    set({ user });
  },
  logout: () => {
    localStorage.removeItem('user');
    localStorage.removeItem('token');
    set({ user: null });
  },

  medicines: [],
  setMedicines: (medicines) => set({ medicines }),

  customers: [],
  setCustomers: (customers) => set({ customers }),

  orders: [],
  setOrders: (orders) => set({ orders }),

  loading: false,
  setLoading: (loading) => set({ loading }),

  error: null,
  setError: (error) => set({ error }),
}));

export default useStore;
