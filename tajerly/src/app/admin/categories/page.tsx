'use client';

import { Table, Badge, Button } from 'react-bootstrap';
import { PageHeader } from '@/components/shared/PageHeader';
import { Plus, Edit, Trash2, Search } from 'lucide-react';
import { useTranslation } from 'react-i18next';

const mockCategories = [
    { id: 1, name: 'Electronics', productCount: 128, isActive: true, displayOrder: 1 },
    { id: 2, name: 'Fashion', productCount: 256, isActive: true, displayOrder: 2 },
    { id: 3, name: 'Beauty', productCount: 94, isActive: true, displayOrder: 3 },
    { id: 4, name: 'Home & Living', productCount: 182, isActive: true, displayOrder: 4 },
    { id: 5, name: 'Sports', productCount: 75, isActive: false, displayOrder: 5 },
];

export default function AdminCategoriesPage() {
    const { t } = useTranslation();

    return (
        <div className="animate-fade-in">
            <PageHeader
                title={t('admin.categories')}
                subtitle={t('admin.stats.products') + ' ' + t('admin.categories')}
                action={
                    <Button variant="primary" className="d-flex align-items-center gap-2 px-4 shadow-sm">
                        <Plus size={18} /> {t('admin.addCategory')}
                    </Button>
                }
            />

            <div className="tj-card mb-4 p-3 d-flex flex-wrap gap-3 align-items-center justify-content-between border-0 shadow-sm">
                <div className="position-relative" style={{ maxWidth: 300, flex: 1 }}>
                    <Search size={16} className="position-absolute translate-middle-y top-50 text-muted" style={{ left: 12 }} />
                    <input 
                        type="text" 
                        placeholder="Search categories..." 
                        className="form-control ps-5 border-light-subtle rounded-3" 
                        style={{ height: 42, background: 'var(--tj-bg-secondary)' }}
                    />
                </div>
            </div>

            <div className="tj-card overflow-hidden border-0 shadow-sm">
                <Table responsive hover className="mb-0 align-middle">
                    <thead>
                        <tr style={{ background: 'var(--tj-bg-secondary)' }}>
                            <th className="fw-bold py-3 px-4 border-0 text-uppercase" style={{ fontSize: '0.7rem', color: 'var(--tj-text-muted)', letterSpacing: '0.05em' }}>Name</th>
                            <th className="fw-bold py-3 border-0 text-uppercase" style={{ fontSize: '0.7rem', color: 'var(--tj-text-muted)', letterSpacing: '0.05em' }}>Products</th>
                            <th className="fw-bold py-3 border-0 text-uppercase" style={{ fontSize: '0.7rem', color: 'var(--tj-text-muted)', letterSpacing: '0.05em' }}>Order</th>
                            <th className="fw-bold py-3 border-0 text-uppercase" style={{ fontSize: '0.7rem', color: 'var(--tj-text-muted)', letterSpacing: '0.05em' }}>Status</th>
                            <th className="fw-bold py-3 px-4 border-0 text-uppercase text-end" style={{ fontSize: '0.7rem', color: 'var(--tj-text-muted)', letterSpacing: '0.05em' }}>Actions</th>
                        </tr>
                    </thead>
                    <tbody>
                        {mockCategories.map((cat) => (
                            <tr key={cat.id} className="transition-all hover-bg-light">
                                <td className="py-3 px-4 fw-bold text-primary-emphasis">{cat.name}</td>
                                <td className="py-3 text-secondary"><span className="badge-light-subtle rounded-pill px-2 py-1 small">{cat.productCount} products</span></td>
                                <td className="py-3 text-muted">{cat.displayOrder}</td>
                                <td className="py-3">
                                    <Badge bg={cat.isActive ? 'success-soft' : 'secondary-soft'} className={cat.isActive ? 'text-success' : 'text-secondary'}>
                                        {cat.isActive ? 'Active' : 'Inactive'}
                                    </Badge>
                                </td>
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
