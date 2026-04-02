'use client';

import Link from 'next/link';
import { Container, Row, Col, Button, Table } from 'react-bootstrap';
import { PageHeader } from '@/components/shared/PageHeader';
import { QuantitySelector } from '@/components/shared/QuantitySelector';
import { EmptyState } from '@/components/shared/EmptyState';
import { Trash2, ShoppingBag, ArrowRight } from 'lucide-react';
import { formatPrice } from '@/utils/formatters';
import { useAppSelector, useAppDispatch } from '@/store/hooks';
import { updateItemQuantity, removeItem } from '@/store/slices/cartSlice';
import type { CartItem } from '@/types/cart';

export default function CartPage() {
    const dispatch = useAppDispatch();
    const { items, totalAmount, itemCount } = useAppSelector((state) => state.cart);

    if (items.length === 0) {
        return (
            <Container className="py-5">
                <EmptyState icon={ShoppingBag} title="Your cart is empty" description="Looks like you haven't added any products yet." actionLabel="Start Shopping" actionHref="/products" />
            </Container>
        );
    }

    return (
        <Container className="py-4">
            <PageHeader title="Shopping Cart" subtitle={`${itemCount} item${itemCount !== 1 ? 's' : ''} in your cart`} breadcrumbs={[{ label: 'Cart' }]} />

            <Row className="g-4">
                <Col lg={8}>
                    <div className="tj-card overflow-hidden">
                        <Table responsive className="mb-0" style={{ borderColor: 'var(--tj-border)' }}>
                            <thead>
                                <tr style={{ background: 'var(--tj-bg-secondary)' }}>
                                    <th className="fw-semibold py-3 px-4" style={{ fontSize: '0.8rem', color: 'var(--tj-text-muted)' }}>Product</th>
                                    <th className="fw-semibold py-3" style={{ fontSize: '0.8rem', color: 'var(--tj-text-muted)' }}>Price</th>
                                    <th className="fw-semibold py-3" style={{ fontSize: '0.8rem', color: 'var(--tj-text-muted)' }}>Quantity</th>
                                    <th className="fw-semibold py-3" style={{ fontSize: '0.8rem', color: 'var(--tj-text-muted)' }}>Subtotal</th>
                                    <th className="py-3" style={{ width: 50 }}></th>
                                </tr>
                            </thead>
                            <tbody>
                                {items.map((item: CartItem) => (
                                    <tr key={item.productId}>
                                        <td className="py-3 px-4">
                                            <div className="d-flex align-items-center gap-3">
                                                <img src={item.thumbnailUrl || 'https://images.unsplash.com/photo-1523275335684-37898b6baf30?q=80&w=80'} alt={item.productName} className="rounded-3" style={{ width: 64, height: 64, objectFit: 'cover' }} />
                                                <Link href={`/products/${item.productSlug}`} className="text-decoration-none fw-semibold" style={{ color: 'var(--tj-text-primary)', fontSize: '0.9rem' }}>
                                                    {item.productName}
                                                </Link>
                                            </div>
                                        </td>
                                        <td className="py-3 align-middle" style={{ color: 'var(--tj-text-secondary)', fontSize: '0.9rem' }}>{formatPrice(item.price)}</td>
                                        <td className="py-3 align-middle">
                                            <QuantitySelector value={item.quantity} max={item.stock} onChange={(qty) => dispatch(updateItemQuantity({ productId: item.productId, quantity: qty }))} />
                                        </td>
                                        <td className="py-3 align-middle fw-bold" style={{ color: 'var(--tj-text-primary)' }}>{formatPrice(item.subtotal)}</td>
                                        <td className="py-3 align-middle">
                                            <Button variant="link" className="p-1" style={{ color: 'var(--tj-danger)' }} onClick={() => dispatch(removeItem(item.productId))}>
                                                <Trash2 size={16} />
                                            </Button>
                                        </td>
                                    </tr>
                                ))}
                            </tbody>
                        </Table>
                    </div>
                </Col>

                {/* Summary */}
                <Col lg={4}>
                    <div className="tj-card p-4" style={{ position: 'sticky', top: 100 }}>
                        <h5 className="fw-bold mb-4" style={{ color: 'var(--tj-text-primary)' }}>Order Summary</h5>
                        <div className="d-flex justify-content-between mb-2">
                            <span style={{ color: 'var(--tj-text-muted)', fontSize: '0.9rem' }}>Subtotal</span>
                            <span className="fw-semibold" style={{ color: 'var(--tj-text-primary)' }}>{formatPrice(totalAmount)}</span>
                        </div>
                        <div className="d-flex justify-content-between mb-2">
                            <span style={{ color: 'var(--tj-text-muted)', fontSize: '0.9rem' }}>Shipping</span>
                            <span className="fw-semibold" style={{ color: 'var(--tj-success)' }}>Free</span>
                        </div>
                        <hr style={{ borderColor: 'var(--tj-border)' }} />
                        <div className="d-flex justify-content-between mb-4">
                            <span className="fw-bold" style={{ color: 'var(--tj-text-primary)' }}>Total</span>
                            <span className="fw-bold" style={{ fontSize: '1.25rem', color: 'var(--tj-accent)' }}>{formatPrice(totalAmount)}</span>
                        </div>
                        <Link href="/checkout" className="btn btn-primary w-100 d-flex align-items-center justify-content-center gap-2 py-3">
                            Proceed to Checkout <ArrowRight size={18} />
                        </Link>
                    </div>
                </Col>
            </Row>
        </Container>
    );
}
