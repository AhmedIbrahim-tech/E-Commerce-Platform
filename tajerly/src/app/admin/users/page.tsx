'use client';

import { Table, Badge, Button } from 'react-bootstrap';
import { PageHeader } from '@/components/shared/PageHeader';
import { Plus, Edit, Trash2, Search, Shield, User as UserIcon } from 'lucide-react';
import { formatDate } from '@/utils/formatters';
import { useTranslation } from 'react-i18next';

const mockUsers = [
    { id: 1, fullName: 'Ahmed Ibrahim', email: 'ahmed@test.com', role: 'Admin', status: 'Active', createdAt: '2025-01-01' },
    { id: 2, fullName: 'Sara Ali', email: 'sara@test.com', role: 'Customer', status: 'Active', createdAt: '2025-02-15' },
    { id: 3, fullName: 'Mohamed Hassan', email: 'mohamed@test.com', role: 'Customer', status: 'Active', createdAt: '2025-03-01' },
    { id: 4, fullName: 'Nour El-Din', email: 'nour@test.com', role: 'Customer', status: 'Suspended', createdAt: '2025-03-20' },
];

export default function AdminUsersPage() {
    const { t } = useTranslation();

    return (
        <div className="animate-fade-in">
            <PageHeader
                title={t('admin.users')}
                subtitle={t('admin.stats.users')}
                action={
                    <Button variant="primary" className="d-flex align-items-center gap-2 px-4 shadow-sm">
                        <Plus size={18} /> {t('admin.addUser')}
                    </Button>
                }
            />

            <div className="tj-card mb-4 p-3 d-flex flex-wrap gap-3 align-items-center justify-content-between border-0 shadow-sm">
                <div className="position-relative" style={{ maxWidth: 300, flex: 1 }}>
                    <Search size={16} className="position-absolute translate-middle-y top-50 text-muted" style={{ left: 12 }} />
                    <input 
                        type="text" 
                        placeholder="Search users..." 
                        className="form-control ps-5 border-light-subtle rounded-3" 
                        style={{ height: 42, background: 'var(--tj-bg-secondary)' }}
                    />
                </div>
            </div>

            <div className="tj-card overflow-hidden border-0 shadow-sm">
                <Table responsive hover className="mb-0 align-middle">
                    <thead>
                        <tr style={{ background: 'var(--tj-bg-secondary)' }}>
                            <th className="fw-bold py-3 px-4 border-0 text-uppercase" style={{ fontSize: '0.7rem', color: 'var(--tj-text-muted)', letterSpacing: '0.05em' }}>User</th>
                            <th className="fw-bold py-3 border-0 text-uppercase" style={{ fontSize: '0.7rem', color: 'var(--tj-text-muted)', letterSpacing: '0.05em' }}>Email</th>
                            <th className="fw-bold py-3 border-0 text-uppercase" style={{ fontSize: '0.7rem', color: 'var(--tj-text-muted)', letterSpacing: '0.05em' }}>Role</th>
                            <th className="fw-bold py-3 border-0 text-uppercase" style={{ fontSize: '0.7rem', color: 'var(--tj-text-muted)', letterSpacing: '0.05em' }}>Status</th>
                            <th className="fw-bold py-3 border-0 text-uppercase" style={{ fontSize: '0.7rem', color: 'var(--tj-text-muted)', letterSpacing: '0.05em' }}>Joined</th>
                            <th className="fw-bold py-3 px-4 border-0 text-uppercase text-end" style={{ fontSize: '0.7rem', color: 'var(--tj-text-muted)', letterSpacing: '0.05em' }}>Actions</th>
                        </tr>
                    </thead>
                    <tbody>
                        {mockUsers.map((user) => (
                            <tr key={user.id} className="transition-all hover-bg-light">
                                <td className="py-3 px-4">
                                    <div className="d-flex align-items-center gap-3">
                                        <div className="rounded-circle d-flex align-items-center justify-content-center" style={{ width: 40, height: 40, background: 'var(--tj-bg-secondary)' }}>
                                            <UserIcon size={18} className="text-muted" />
                                        </div>
                                        <span className="fw-bold text-primary-emphasis">{user.fullName}</span>
                                    </div>
                                </td>
                                <td className="py-3 text-secondary" style={{ fontSize: '0.85rem' }}>{user.email}</td>
                                <td className="py-3">
                                    <div className="d-flex align-items-center gap-1">
                                        {user.role === 'Admin' && <Shield size={14} className="text-primary" />}
                                        <Badge bg={user.role === 'Admin' ? 'primary-soft' : 'info-soft'} className={user.role === 'Admin' ? 'text-primary' : 'text-info'}>
                                            {user.role}
                                        </Badge>
                                    </div>
                                </td>
                                <td className="py-3">
                                    <Badge bg={user.status === 'Active' ? 'success-soft' : 'secondary-soft'} className={user.status === 'Active' ? 'text-success' : 'text-secondary'}>
                                        {user.status}
                                    </Badge>
                                </td>
                                <td className="py-3 text-muted" style={{ fontSize: '0.85rem' }}>{formatDate(user.createdAt)}</td>
                                <td className="py-3 px-4 text-end">
                                    <div className="d-flex gap-2 justify-content-end">
                                        <Button variant="soft-primary" size="sm" className="p-2"><Edit size={16} /></Button>
                                        <Button variant="soft-danger" size="sm" className="p-2"><Trash2 size={16} /></Button>
                                    </div>
                                </td>
                            </tr>
                        ))}
                    </tbody>
                </Table>
            </div>
        </div>
    );
}
