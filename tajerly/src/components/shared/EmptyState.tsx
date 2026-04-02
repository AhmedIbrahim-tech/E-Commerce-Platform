import React from 'react';
import { LucideIcon } from 'lucide-react';
import { Button } from 'react-bootstrap';

interface EmptyStateProps {
    icon: LucideIcon;
    title: string;
    description?: string;
    actionLabel?: string;
    actionHref?: string;
    onAction?: () => void;
}

export function EmptyState({ icon: Icon, title, description, actionLabel, actionHref, onAction }: EmptyStateProps) {
    return (
        <div className="text-center py-5">
            <div className="d-flex justify-content-center mb-4">
                <div
                    className="rounded-circle d-flex align-items-center justify-content-center"
                    style={{ width: 80, height: 80, background: 'var(--tj-accent-soft)' }}
                >
                    <Icon size={36} style={{ color: 'var(--tj-accent)' }} />
                </div>
            </div>
            <h5 className="fw-bold mb-2" style={{ color: 'var(--tj-text-primary)' }}>{title}</h5>
            {description && <p style={{ color: 'var(--tj-text-muted)', maxWidth: 400, margin: '0 auto' }}>{description}</p>}
            {actionLabel && (
                <div className="mt-4">
                    {actionHref ? (
                        <a href={actionHref} className="btn btn-primary px-4">{actionLabel}</a>
                    ) : (
                        <Button variant="primary" className="px-4" onClick={onAction}>{actionLabel}</Button>
                    )}
                </div>
            )}
        </div>
    );
}
