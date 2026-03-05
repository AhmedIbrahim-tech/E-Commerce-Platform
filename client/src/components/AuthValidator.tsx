'use client';

import { useEffect } from 'react';
import { useAppDispatch } from '@/hooks/useRedux';
import { validateToken, logout } from '@/store/slices/authSlice';

export default function AuthValidator({ children }: { children: React.ReactNode }) {
    const dispatch = useAppDispatch();

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
