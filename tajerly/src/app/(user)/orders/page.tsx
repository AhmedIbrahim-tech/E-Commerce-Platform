'use client';

import { Container, Table, Badge } from 'react-bootstrap';
import { PageHeader } from '@/components/shared/PageHeader';
import { EmptyState } from '@/components/shared/EmptyState';
import { ClipboardList } from 'lucide-react';
import { formatPrice, formatDate } from '@/utils/formatters';
import { getOrderStatusColor } from '@/constants/orderStatuses';
import Link from 'next/link';

const mockOrders = [
    { id: 1, orderNumber: 'TJ-2025-001', status: 'Delivered', totalAmount: 3799.98, createdAt: '2025-03-15T10:30:00', itemCount: 3 },
    { id: 2, orderNumber: 'TJ-2025-002', status: 'Shipped', totalAmount: 1299.99, createdAt: '2025-03-28T14:00:00', itemCount: 1 },
    { id: 3, orderNumber: 'TJ-2025-003', status: 'Pending', totalAmount: 849.99, createdAt: '2025-04-01T09:15:00', itemCount: 2 },
];

export default function OrdersPage() {
    return (
        <Container className="py-4">
            <PageHeader title="My Orders" subtitle="Track your purchases" breadcrumbs={[{ label: 'Orders' }]} />

            {mockOrders.length === 0 ? (
                <EmptyState icon={ClipboardList} title="No orders yet" description="Once you place an order, it will appear here." actionLabel="Shop Now" actionHref="/products" />
            ) : (
                <div className="tj-card overflow-hidden">
                    <Table responsive className="mb-0">
                        <thead>
                            <tr style={{ background: 'var(--tj-bg-secondary)' }}>
                                <th className="fw-semibold py-3 px-4" style={{ fontSize: '0.8rem', color: 'var(--tj-text-muted)' }}>Order</th>
                                <th className="fw-semibold py-3" style={{ fontSize: '0.8rem', color: 'var(--tj-text-muted)' }}>Date</th>
                                <th className="fw-semibold py-3" style={{ fontSize: '0.8rem', color: 'var(--tj-text-muted)' }}>Status</th>
                                <th className="fw-semibold py-3" style={{ fontSize: '0.8rem', color: 'var(--tj-text-muted)' }}>Items</th>
                                <th className="fw-semibold py-3" style={{ fontSize: '0.8rem', color: 'var(--tj-text-muted)' }}>Total</th>
                                <th className="py-3"></th>
                            </tr>
                        </thead>
                        <tbody>
                            {mockOrders.map((order) => (
                                <tr key={order.id}>
                                    <td className="py-3 px-4 fw-semibold" style={{ color: 'var(--tj-text-primary)' }}>{order.orderNumber}</td>
                                    <td className="py-3" style={{ color: 'var(--tj-text-secondary)', fontSize: '0.9rem' }}>{formatDate(order.createdAt)}</td>
                                    <td className="py-3"><Badge bg={getOrderStatusColor(order.status)}>{order.status}</Badge></td>
                                    <td className="py-3" style={{ color: 'var(--tj-text-secondary)' }}>{order.itemCount}</td>
                                    <td className="py-3 fw-bold" style={{ color: 'var(--tj-text-primary)' }}>{formatPrice(order.totalAmount)}</td>
                                    <td className="py-3">
                                        <Link href={`/orders/${order.id}`} className="btn btn-outline-primary btn-sm">View</Link>
                                    </td>
                                </tr>
                            ))}
                        </tbody>
                    </Table>
                </div>
            )}
        </Container>
    );
}
