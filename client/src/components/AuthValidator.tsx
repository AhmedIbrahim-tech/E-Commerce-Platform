'use client';

import { useEffect } from 'react';
import { useDispatch } from 'react-redux';
import { AppDispatch } from '@/store/store';
import { validateToken, logout } from '@/store/slices/authSlice';

export default function AuthValidator({ children }: { children: React.ReactNode }) {
    const dispatch = useDispatch<AppDispatch>();

    useEffect(() => {
        const accessToken = typeof window !== 'undefined' ? localStorage.getItem('accessToken') : null;
        if (!accessToken) return;

        dispatch(validateToken()).then((result) => {
            if (validateToken.rejected.match(result)) {
                dispatch(logout());
                window.location.href = '/login';
            }
        });
    }, [dispatch]);

    return <>{children}</>;
}
