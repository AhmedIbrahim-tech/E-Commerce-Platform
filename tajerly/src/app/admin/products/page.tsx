'use client';

import { Table, Badge, Button } from 'react-bootstrap';
import { PageHeader } from '@/components/shared/PageHeader';
import { Plus, Edit, Trash2, ToggleLeft, ToggleRight, Search, Filter } from 'lucide-react';
import { formatPrice, formatDate } from '@/utils/formatters';
import { useTranslation } from 'react-i18next';

const mockProducts = [
    { id: 1, name: 'Premium Wireless Headphones', sku: 'PWH-001', price: 1299.99, stock: 15, categoryName: 'Electronics', isActive: true, isFeatured: true, createdAt: '2025-01-15' },
    { id: 2, name: 'Leather Crossbody Bag', sku: 'LCB-002', price: 849.99, stock: 8, categoryName: 'Fashion', isActive: true, isFeatured: false, createdAt: '2025-02-10' },
    { id: 3, name: 'Smart Fitness Watch Pro', sku: 'SFW-003', price: 2499.99, stock: 22, categoryName: 'Electronics', isActive: true, isFeatured: true, createdAt: '2025-03-01' },
    { id: 4, name: 'Organic Skincare Set', sku: 'OSS-004', price: 599.99, stock: 0, categoryName: 'Beauty', isActive: false, isFeatured: false, createdAt: '2025-03-10' },
];

export default function AdminProductsPage() {
    const { t } = useTranslation();

    return (
        <div className="animate-fade-in">
            <PageHeader
                title={t('admin.products')}
                subtitle={t('admin.stats.products')}
                action={
                    <Button variant="primary" className="d-flex align-items-center gap-2 px-4 shadow-sm">
                        <Plus size={18} /> {t('admin.addProduct')}
                    </Button>
                }
            />

            <div className="tj-card mb-4 p-3 d-flex flex-wrap gap-3 align-items-center justify-content-between border-0 shadow-sm">
                <div className="position-relative" style={{ maxWidth: 300, flex: 1 }}>
                    <Search size={16} className="position-absolute translate-middle-y top-50 text-muted" style={{ left: 12 }} />
                    <input 
                        type="text" 
                        placeholder="Search products..." 
                        className="form-control ps-5 border-light-subtle rounded-3" 
                        style={{ height: 42, background: 'var(--tj-bg-secondary)' }}
                    />
                </div>
                <div className="d-flex gap-2">
                    <Button variant="outline-secondary" className="d-flex align-items-center gap-2 border-light-subtle" size="sm">
                        <Filter size={16} /> Filters
                    </Button>
                </div>
            </div>

            <div className="tj-card overflow-hidden border-0 shadow-sm">
                <Table responsive hover className="mb-0 align-middle">
                    <thead>
                        <tr style={{ background: 'var(--tj-bg-secondary)' }}>
                            <th className="fw-bold py-3 px-4 border-0 text-uppercase" style={{ fontSize: '0.7rem', color: 'var(--tj-text-muted)', letterSpacing: '0.05em' }}>Product</th>
                            <th className="fw-bold py-3 border-0 text-uppercase" style={{ fontSize: '0.7rem', color: 'var(--tj-text-muted)', letterSpacing: '0.05em' }}>SKU</th>
                            <th className="fw-bold py-3 border-0 text-uppercase" style={{ fontSize: '0.7rem', color: 'var(--tj-text-muted)', letterSpacing: '0.05em' }}>Price</th>
                            <th className="fw-bold py-3 border-0 text-uppercase" style={{ fontSize: '0.7rem', color: 'var(--tj-text-muted)', letterSpacing: '0.05em' }}>Stock</th>
                            <th className="fw-bold py-3 border-0 text-uppercase" style={{ fontSize: '0.7rem', color: 'var(--tj-text-muted)', letterSpacing: '0.05em' }}>Category</th>
                            <th className="fw-bold py-3 border-0 text-uppercase" style={{ fontSize: '0.7rem', color: 'var(--tj-text-muted)', letterSpacing: '0.05em' }}>Status</th>
                            <th className="fw-bold py-3 px-4 border-0 text-uppercase text-end" style={{ fontSize: '0.7rem', color: 'var(--tj-text-muted)', letterSpacing: '0.05em' }}>Actions</th>
                        </tr>
                    </thead>
                    <tbody>
                        {mockProducts.map((product) => (
                            <tr key={product.id} className="transition-all hover-bg-light">
                                <td className="py-3 px-4">
                                    <div className="d-flex align-items-center gap-3">
                                        <div className="rounded-3 bg-light" style={{ width: 44, height: 44, background: 'linear-gradient(45deg, var(--tj-bg-secondary), var(--tj-border))' }} />
                                        <div>
                                            <span className="fw-bold d-block text-primary-emphasis" style={{ fontSize: '0.9rem' }}>{product.name}</span>
                                            {product.isFeatured && <Badge bg="primary-soft" className="text-primary px-2" style={{ fontSize: '0.6rem' }}>Featured</Badge>}
                                        </div>
                                    </div>
                                </td>
                                <td className="py-3 text-secondary" style={{ fontSize: '0.85rem' }}><code>{product.sku}</code></td>
                                <td className="py-3 fw-bold text-accent">{formatPrice(product.price)}</td>
                                <td className="py-3">
                                    <div className="d-flex align-items-center gap-2">
                                        <div className={`rounded-circle`} style={{ width: 8, height: 8, background: product.stock > 10 ? 'var(--tj-success)' : product.stock > 0 ? 'var(--tj-warning)' : 'var(--tj-danger)' }} />
                                        <span style={{ fontWeight: 600, fontSize: '0.9rem' }}>{product.stock}</span>
                                    </div>
                                </td>
                                <td className="py-3"><span className="badge-light-subtle rounded-pill px-2 py-1 small">{product.categoryName}</span></td>
                                <td className="py-3">
                                    <Badge bg={product.isActive ? 'success-soft' : 'secondary-soft'} className={product.isActive ? 'text-success' : 'text-secondary'}>
                                        {product.isActive ? 'Active' : 'Inactive'}
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
