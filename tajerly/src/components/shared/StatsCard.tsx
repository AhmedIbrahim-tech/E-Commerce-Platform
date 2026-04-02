import React from 'react';
import { LucideIcon, TrendingUp, TrendingDown } from 'lucide-react';

interface StatsCardProps {
    title: string;
    value: string | number;
    icon: LucideIcon;
    color: 'primary' | 'success' | 'warning' | 'danger' | 'info';
    trend?: { value: string; isUp: boolean };
}

const colorMap = {
    primary: { bg: 'var(--tj-accent-soft)', text: 'var(--tj-accent)' },
    success: { bg: 'var(--tj-success-soft)', text: 'var(--tj-success)' },
    warning: { bg: 'var(--tj-warning-soft)', text: 'var(--tj-warning)' },
    danger: { bg: 'var(--tj-danger-soft)', text: 'var(--tj-danger)' },
    info: { bg: 'var(--tj-info-soft)', text: 'var(--tj-info)' },
};

export function StatsCard({ title, value, icon: Icon, color, trend }: StatsCardProps) {
    const colors = colorMap[color];
    return (
        <div className="tj-card p-4">
            <div className="d-flex align-items-center justify-content-between mb-3">
                <div>
                    <p className="mb-1" style={{ fontSize: '0.75rem', fontWeight: 600, color: 'var(--tj-text-muted)' }}>{title}</p>
                    <h3 className="mb-0 fw-bold" style={{ fontSize: '1.375rem', color: 'var(--tj-text-primary)' }}>{value}</h3>
                </div>
                <div className="d-flex align-items-center justify-content-center rounded-3 p-3" style={{ background: colors.bg }}>
                    <Icon size={22} style={{ color: colors.text }} />
                </div>
            </div>
            {trend && (
                <div className="d-flex align-items-center gap-2">
                    {trend.isUp ? (
                        <TrendingUp size={14} style={{ color: 'var(--tj-success)' }} />
                    ) : (
                        <TrendingDown size={14} style={{ color: 'var(--tj-danger)' }} />
                    )}
                    <span style={{ fontSize: '0.75rem', fontWeight: 600, color: trend.isUp ? 'var(--tj-success)' : 'var(--tj-danger)' }}>
                        {trend.value}
                    </span>
                    <span style={{ fontSize: '0.75rem', color: 'var(--tj-text-light)' }}>vs last month</span>
                </div>
            )}
        </div>
    );
}
