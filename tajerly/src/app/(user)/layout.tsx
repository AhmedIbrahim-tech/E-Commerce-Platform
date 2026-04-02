'use client';

import { AuthGuard } from '@/components/layout/AuthGuard';
import { StorefrontHeader } from '@/components/layout/StorefrontHeader';
import { StorefrontFooter } from '@/components/layout/StorefrontFooter';

export default function UserLayout({ children }: { children: React.ReactNode }) {
    return (
        <AuthGuard>
            <StorefrontHeader />
            <main style={{ paddingTop: 80, minHeight: '100vh' }}>{children}</main>
            <StorefrontFooter />
        </AuthGuard>
    );
}
