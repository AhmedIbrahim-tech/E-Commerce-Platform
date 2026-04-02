'use client';

import { AuthGuard } from '@/components/layout/AuthGuard';
import { AdminSidebar } from '@/components/layout/AdminSidebar';
import { ThemeToggle } from '@/components/layout/ThemeToggle';

export default function AdminLayout({ children }: { children: React.ReactNode }) {
    return (
        <AuthGuard requiredRole="Admin">
            <AdminSidebar />
            <div style={{ marginLeft: 260, minHeight: '100vh', background: 'var(--tj-bg)', transition: 'margin-left 0.3s' }}>
                {/* Top Bar */}
                <div className="d-flex justify-content-end align-items-center px-4 py-3" style={{ borderBottom: '1px solid var(--tj-border)', background: 'var(--tj-card)' }}>
                    <ThemeToggle />
                </div>
                <div className="p-4">
                    {children}
                </div>
            </div>
        </AuthGuard>
    );
}
