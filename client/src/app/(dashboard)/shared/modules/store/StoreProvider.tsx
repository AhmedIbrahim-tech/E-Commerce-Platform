"use client";

import { useEffect } from 'react';
import { Provider } from 'react-redux';
import { store } from './index';
import { initializeAuthAsync } from './slices/authSlice';

export default function StoreProvider({ children }: { children: React.ReactNode }) {
  useEffect(() => {
    store.dispatch(initializeAuthAsync());
  }, []);

  return <Provider store={store}>{children}</Provider>;
}

