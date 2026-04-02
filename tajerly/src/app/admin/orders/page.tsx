'use client';

import { Table, Badge, Button, Form } from 'react-bootstrap';
import { PageHeader } from '@/components/shared/PageHeader';
import { Eye, Search, Filter, Download } from 'lucide-react';
import { formatPrice, formatDate } from '@/utils/formatters';
import { getOrderStatusColor, ORDER_STATUSES } from '@/constants/orderStatuses';
import { useTranslation } from 'react-i18next';

const mockOrders = [
    { id: 1, orderNumber: 'TJ-2025-001', customerName: 'Ahmed Ibrahim', status: 'Delivered', totalAmount: 3799.98, itemCount: 3, createdAt: '2025-03-15T10:30:00' },
    { id: 2, orderNumber: 'TJ-2025-002', customerName: 'Mohamed Hassan', status: 'Shipped', totalAmount: 1299.99, itemCount: 1, createdAt: '2025-03-28T14:00:00' },
    { id: 3, orderNumber: 'TJ-2025-003', customerName: 'Sara Ali', status: 'Pending', totalAmount: 849.99, itemCount: 2, createdAt: '2025-04-01T09:15:00' },
    { id: 4, orderNumber: 'TJ-2025-004', customerName: 'Nour El-Din', status: 'Processing', totalAmount: 2099.00, itemCount: 4, createdAt: '2025-04-02T08:00:00' },
];

export default function AdminOrdersPage() {
    const { t } = useTranslation();

    return (
        <div className="animate-fade-in">
            <PageHeader 
                title={t('admin.orders')} 
                subtitle={t('admin.stats.orders')} 
                action={
                    <Button variant="outline-primary" className="d-flex align-items-center gap-2 px-4 shadow-sm">
                        <Download size={18} /> Export CSV
                    </Button>
                }
            />

            <div className="tj-card mb-4 p-3 d-flex flex-wrap gap-3 align-items-center justify-content-between border-0 shadow-sm">
                <div className="position-relative" style={{ maxWidth: 300, flex: 1 }}>
                    <Search size={16} className="position-absolute translate-middle-y top-50 text-muted" style={{ left: 12 }} />
                    <input 
                        type="text" 
                        placeholder="Search orders..." 
                        className="form-control ps-5 border-light-subtle rounded-3" 
                        style={{ height: 42, background: 'var(--tj-bg-secondary)' }}
                    />
                </div>
                <div className="d-flex gap-2">
                    <Form.Select size="sm" className="border-light-subtle rounded-3" style={{ height: 42, width: 180, background: 'var(--tj-bg-secondary)' }}>
                        <option value="">{t('admin.stats.orders')} Status</option>
                        {ORDER_STATUSES.map((s) => <option key={s.value} value={s.value}>{s.label}</option>)}
                    </Form.Select>
                </div>
            </div>

            <div className="tj-card overflow-hidden border-0 shadow-sm">
                <Table responsive hover className="mb-0 align-middle">
                    <thead>
                        <tr style={{ background: 'var(--tj-bg-secondary)' }}>
                            <th className="fw-bold py-3 px-4 border-0 text-uppercase" style={{ fontSize: '0.7rem', color: 'var(--tj-text-muted)', letterSpacing: '0.05em' }}>Order</th>
                            <th className="fw-bold py-3 border-0 text-uppercase" style={{ fontSize: '0.7rem', color: 'var(--tj-text-muted)', letterSpacing: '0.05em' }}>Customer</th>
                            <th className="fw-bold py-3 border-0 text-uppercase" style={{ fontSize: '0.7rem', color: 'var(--tj-text-muted)', letterSpacing: '0.05em' }}>Date</th>
                            <th className="fw-bold py-3 border-0 text-uppercase" style={{ fontSize: '0.7rem', color: 'var(--tj-text-muted)', letterSpacing: '0.05em' }}>Items</th>
                            <th className="fw-bold py-3 border-0 text-uppercase" style={{ fontSize: '0.7rem', color: 'var(--tj-text-muted)', letterSpacing: '0.05em' }}>Total</th>
                            <th className="fw-bold py-3 border-0 text-uppercase" style={{ fontSize: '0.7rem', color: 'var(--tj-text-muted)', letterSpacing: '0.05em' }}>Status</th>
                            <th className="fw-bold py-3 px-4 border-0 text-uppercase text-end" style={{ fontSize: '0.7rem', color: 'var(--tj-text-muted)', letterSpacing: '0.05em' }}>Actions</th>
                        </tr>
                    </thead>
                    <tbody>
                        {mockOrders.map((order) => (
                            <tr key={order.id} className="transition-all hover-bg-light">
                                <td className="py-3 px-4 fw-bold text-primary-emphasis">{order.orderNumber}</td>
                                <td className="py-3 text-secondary">{order.customerName}</td>
                                <td className="py-3 text-muted" style={{ fontSize: '0.85rem' }}>{formatDate(order.createdAt)}</td>
                                <td className="py-3"><span className="badge-light-subtle rounded-pill px-2 py-1 small">{order.itemCount} items</span></td>
                                <td className="py-3 fw-bold text-accent">{formatPrice(order.totalAmount)}</td>
                                <td className="py-3">
                                    <Badge bg={getOrderStatusColor(order.status) + '-soft'} className={`text-${getOrderStatusColor(order.status)} px-3 py-2 rounded-pill`} style={{ fontSize: '0.7rem' }}>
                                        {order.status}
                                    </Badge>
                                </td>
                                <td className="py-3 px-4 text-end">
                                    <Button variant="soft-primary" size="sm" className="p-2 shadow-none border-0"><Eye size={18} /></Button>
                                </td>
                            </tr>
                        ))}
                    </tbody>
                </Table>
            </div>
        </div>
    );
}
