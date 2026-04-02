import React from 'react';
import Link from 'next/link';
import { ChevronRight, ChevronLeft, Home } from 'lucide-react';
import { useTranslation } from 'react-i18next';

interface PageHeaderProps {
    title: string;
    subtitle?: string;
    action?: React.ReactNode;
    breadcrumbs?: { label: string; href?: string }[];
}

export function PageHeader({ title, subtitle, action, breadcrumbs }: PageHeaderProps) {
    const { i18n } = useTranslation();
    const isRtl = i18n.language === 'ar';
    return (
        <div className="mb-4">
            {/* Breadcrumbs */}
            <nav className="d-flex align-items-center gap-2 mb-3" style={{ fontSize: '0.75rem', color: 'var(--tj-text-muted)', fontWeight: 500 }}>
                <Home size={14} style={{ color: 'var(--tj-text-light)' }} />
                {isRtl ? <ChevronLeft size={14} style={{ color: 'var(--tj-text-light)', opacity: 0.6 }} /> : <ChevronRight size={14} style={{ color: 'var(--tj-text-light)', opacity: 0.6 }} />}
                {breadcrumbs ? (
                    breadcrumbs.map((crumb, idx) => (
                        <React.Fragment key={idx}>
                            {crumb.href ? (
                                <Link href={crumb.href} className="text-decoration-none" style={{ color: 'var(--tj-text-muted)', transition: 'color 0.2s' }}>
                                    {crumb.label}
                                </Link>
                            ) : (
                                <span>{crumb.label}</span>
                            )}
                            {idx < breadcrumbs.length - 1 && (isRtl ? <ChevronLeft size={14} style={{ color: 'var(--tj-text-light)', opacity: 0.6 }} /> : <ChevronRight size={14} style={{ color: 'var(--tj-text-light)', opacity: 0.6 }} />)}
                        </React.Fragment>
                    ))
                ) : (
                    <span>{title}</span>
                )}
            </nav>

            <div className="d-flex flex-column flex-md-row align-items-md-end justify-content-between gap-3">
                <div>
                    <h1 className="fw-bold mb-0" style={{ fontSize: '1.5rem', color: 'var(--tj-text-primary)', letterSpacing: '-0.025em' }}>
                        {title}
                    </h1>
                    {subtitle && (
                        <p className="mb-0 mt-1" style={{ fontSize: '0.875rem', color: 'var(--tj-text-secondary)', fontWeight: 500 }}>
                            {subtitle}
                        </p>
                    )}
                </div>
                {action && <div className="flex-shrink-0 animate-fade-in">{action}</div>}
            </div>
        </div>
    );
}
