'use client';

import { useEffect, useState } from 'react';
import { useRouter } from 'next/navigation';
import { useAppSelector } from '@/store/hooks';
import type { RootState } from '@/store';
import { Spinner } from 'react-bootstrap';

/** Persisted root state includes _persist added by redux-persist. */
type PersistedRootState = RootState & { _persist?: { rehydrated?: boolean } };

/**
 * Guards protected routes. Waits for redux-persist rehydration
 * before deciding auth, so a reload does not falsely redirect to /login.
 */
export function AuthGuard({ children, requiredRole }: { children: React.ReactNode; requiredRole?: string }) {
    const router = useRouter();
    const accessToken = useAppSelector((state) => state.auth.accessToken);
    const userRole = useAppSelector((state) => state.auth.user?.role);
    const roles = useAppSelector((state) => state.auth.roles);
    const rehydrated = useAppSelector((state: PersistedRootState) => state._persist?.rehydrated === true);
    const [allowed, setAllowed] = useState<boolean | null>(null);

    useEffect(() => {
        if (!rehydrated) return;

        if (!accessToken) {
            router.replace('/login');
            setAllowed(false);
            return;
        }

        if (requiredRole) {
            const required = requiredRole.toLowerCase();
            const primary = userRole?.toLowerCase();
            const fromList = roles.some((r) => r.toLowerCase() === required);
            if (primary !== required && !fromList) {
                router.replace('/');
                setAllowed(false);
                return;
            }
        }

        setAllowed(true);
    }, [rehydrated, accessToken, userRole, roles, requiredRole, router]);

    if (!rehydrated || allowed !== true) {
        return (
            <div className="d-flex min-vh-100 align-items-center justify-content-center" style={{ background: 'var(--tj-bg)' }}>
                <Spinner animation="border" style={{ color: 'var(--tj-accent)' }} />
            </div>
        );
    }

    return <>{children}</>;
}
