'use client';

import Link from 'next/link';
import { usePathname } from 'next/navigation';
import { Nav } from 'react-bootstrap';
import {
    LayoutDashboard, Package, FolderTree, ClipboardList, Users,
    ShoppingBag, ChevronLeft, ChevronRight, LogOut, Store
} from 'lucide-react';
import { siteConfig } from '@/config/site';
import { useAppDispatch } from '@/store/hooks';
import { logoutUser } from '@/store/slices/authSlice';
import { useRouter } from 'next/navigation';
import { useState } from 'react';
import { useTranslation } from 'react-i18next';

const getAdminLinks = (t: (key: string) => string) => [
    { href: '/admin', label: t('admin.dashboard'), icon: LayoutDashboard },
    { href: '/admin/products', label: t('admin.products'), icon: Package },
    { href: '/admin/categories', label: t('admin.categories'), icon: FolderTree },
    { href: '/admin/orders', label: t('admin.orders'), icon: ClipboardList },
    { href: '/admin/users', label: t('admin.users'), icon: Users },
];

export function AdminSidebar() {
    const pathname = usePathname();
    const dispatch = useAppDispatch();
    const router = useRouter();
    const [collapsed, setCollapsed] = useState(false);

    const { t, i18n } = useTranslation();
    const isRtl = i18n.language === 'ar';
    const adminLinks = getAdminLinks(t);

    const handleLogout = async () => {
        await dispatch(logoutUser());
        router.push('/login');
    };

    return (
        <div
            className={`d-flex flex-column vh-100 position-fixed top-0 ${isRtl ? 'end-0 border-start' : 'start-0 border-end'}`}
            style={{
                width: collapsed ? 80 : 280,
                background: 'var(--tj-card)',
                borderColor: 'var(--tj-border) !important',
                transition: 'all 0.4s cubic-bezier(0.4, 0, 0.2, 1)',
                zIndex: 1040,
                overflowX: 'hidden',
                boxShadow: '0 0 20px rgba(0,0,0,0.05)'
            }}
            dir={isRtl ? 'rtl' : 'ltr'}
        >
            {/* Header */}
            <div className="d-flex align-items-center justify-content-between p-3" style={{ borderBottom: '1px solid var(--tj-border)' }}>
                {!collapsed && (
                    <Link href="/admin" className="d-flex align-items-center gap-2 text-decoration-none">
                        <div
                            className="d-flex align-items-center justify-content-center rounded-3"
                            style={{ width: 36, height: 36, background: 'var(--tj-accent)', color: 'var(--tj-accent-foreground)', fontWeight: 900 }}
                        >
                            <ShoppingBag size={18} />
                        </div>
                        <span style={{ fontWeight: 800, color: 'var(--tj-text-primary)', fontSize: '1rem' }}>{siteConfig.name}</span>
                    </Link>
                )}
                <button
                    onClick={() => setCollapsed(!collapsed)}
                    className="btn btn-link p-2 rounded-circle hover-bg-light"
                    style={{ color: 'var(--tj-text-muted)', transition: 'transform 0.3s' }}
                >
                    {collapsed ? (isRtl ? <ChevronLeft size={18} /> : <ChevronRight size={18} />) : (isRtl ? <ChevronRight size={18} /> : <ChevronLeft size={18} />)}
                </button>
            </div>

            <Nav className="flex-column flex-grow-1 py-3 px-2 gap-1">
                <Link
                    href="/"
                    className="d-flex align-items-center gap-3 px-3 py-2 rounded-3 text-decoration-none mb-1"
                    style={{
                        color: 'var(--tj-text-secondary)',
                        background: 'var(--tj-bg-secondary)',
                        fontWeight: 600,
                        fontSize: '0.875rem',
                        border: '1px solid var(--tj-border)',
                        whiteSpace: 'nowrap',
                    }}
                    title={t('admin.visitSite')}
                >
                    <Store size={20} strokeWidth={2.25} />
                    {!collapsed && <span>{t('admin.visitSite')}</span>}
                </Link>
                {adminLinks.map((link) => {
                    const isActive = pathname === link.href || (link.href !== '/admin' && pathname.startsWith(link.href));
                    return (
                        <Link
                            key={link.href}
                            href={link.href}
                            className={`d-flex align-items-center gap-3 px-3 py-2 rounded-3 text-decoration-none ${isActive ? 'active' : ''}`}
                            style={{
                                color: isActive ? 'var(--tj-accent)' : 'var(--tj-text-secondary)',
                                background: isActive ? 'var(--tj-accent-soft)' : 'transparent',
                                fontWeight: isActive ? 700 : 500,
                                fontSize: '0.875rem',
                                transition: 'all 0.2s',
                                whiteSpace: 'nowrap',
                            }}
                            title={link.label}
                        >
                            <link.icon size={20} />
                            {!collapsed && <span>{link.label}</span>}
                        </Link>
                    );
                })}
            </Nav>

            {/* Logout */}
            <div className="p-3" style={{ borderTop: '1px solid var(--tj-border)' }}>
                <button
                    onClick={handleLogout}
                    className="btn d-flex align-items-center gap-3 w-100 px-3 py-2 rounded-3 hover-danger-bg transition-all"
                    style={{
                        color: 'var(--tj-danger)',
                        background: 'transparent',
                        fontWeight: 600,
                        fontSize: '0.875rem',
                        border: 'none',
                    }}
                >
                    <LogOut size={20} />
                    {!collapsed && <span>{t('header.logout') || 'Logout'}</span>}
                </button>
            </div>
        </div>
    );
}
