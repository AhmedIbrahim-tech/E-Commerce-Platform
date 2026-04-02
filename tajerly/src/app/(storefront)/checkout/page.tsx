'use client';

import { Container, Row, Col, Button, Form } from 'react-bootstrap';
import { PageHeader } from '@/components/shared/PageHeader';
import { useFormik } from 'formik';
import { checkoutSchema, checkoutInitialValues } from '@/validators/checkoutValidator';
import { formatPrice } from '@/utils/formatters';
import { useAppSelector } from '@/store/hooks';
import type { CartItem } from '@/types/cart';
import { notify } from '@/lib/notify';
import { CreditCard, Wallet, Banknote, Lock } from 'lucide-react';

const paymentMethods = [
    { value: 'cod', label: 'Cash on Delivery', icon: Banknote },
    { value: 'card', label: 'Credit / Debit Card', icon: CreditCard },
    { value: 'wallet', label: 'Digital Wallet', icon: Wallet },
];

export default function CheckoutPage() {
    const { items, totalAmount } = useAppSelector((state) => state.cart);

    const formik = useFormik({
        initialValues: checkoutInitialValues,
        validationSchema: checkoutSchema,
        onSubmit: async (values) => {
            // This will call orderService.createOrder when backend is ready
            notify.success('Order placed successfully!');
        },
    });

    return (
        <Container className="py-4">
            <PageHeader title="Checkout" breadcrumbs={[{ label: 'Cart', href: '/cart' }, { label: 'Checkout' }]} />

            <form onSubmit={formik.handleSubmit}>
                <Row className="g-4">
                    <Col lg={8}>
                        {/* Shipping Address */}
                        <div className="tj-card p-4 mb-4">
                            <h5 className="fw-bold mb-3" style={{ color: 'var(--tj-text-primary)' }}>Shipping Address</h5>
                            <div className="p-3 rounded-3" style={{ background: 'var(--tj-bg-secondary)', border: '1px solid var(--tj-border)' }}>
                                <p className="mb-1 fw-semibold" style={{ color: 'var(--tj-text-primary)' }}>Select an address or add a new one</p>
                                <p className="mb-0" style={{ color: 'var(--tj-text-muted)', fontSize: '0.875rem' }}>
                                    <a href="/addresses" style={{ color: 'var(--tj-accent)' }}>Manage your addresses →</a>
                                </p>
                            </div>
                            {formik.touched.shippingAddressId && formik.errors.shippingAddressId && (
                                <div className="text-danger mt-2" style={{ fontSize: '0.8rem' }}>{formik.errors.shippingAddressId}</div>
                            )}
                        </div>

                        {/* Payment Method */}
                        <div className="tj-card p-4 mb-4">
                            <h5 className="fw-bold mb-3" style={{ color: 'var(--tj-text-primary)' }}>Payment Method</h5>
                            <div className="d-flex flex-column gap-2">
                                {paymentMethods.map((method) => (
                                    <label
                                        key={method.value}
                                        className={`d-flex align-items-center gap-3 p-3 rounded-3 cursor-pointer`}
                                        style={{
                                            border: formik.values.paymentMethod === method.value ? '2px solid var(--tj-accent)' : '1px solid var(--tj-border)',
                                            background: formik.values.paymentMethod === method.value ? 'var(--tj-accent-soft)' : 'var(--tj-card)',
                                            cursor: 'pointer',
                                        }}
                                    >
                                        <Form.Check
                                            type="radio"
                                            name="paymentMethod"
                                            value={method.value}
                                            checked={formik.values.paymentMethod === method.value}
                                            onChange={formik.handleChange}
                                        />
                                        <method.icon size={20} style={{ color: formik.values.paymentMethod === method.value ? 'var(--tj-accent)' : 'var(--tj-text-muted)' }} />
                                        <span className="fw-semibold" style={{ color: 'var(--tj-text-primary)', fontSize: '0.9rem' }}>{method.label}</span>
                                    </label>
                                ))}
                            </div>
                        </div>

                        {/* Notes */}
                        <div className="tj-card p-4">
                            <h5 className="fw-bold mb-3" style={{ color: 'var(--tj-text-primary)' }}>Order Notes</h5>
                            <Form.Control
                                as="textarea"
                                rows={3}
                                name="notes"
                                placeholder="Any special instructions for your order..."
                                value={formik.values.notes}
                                onChange={formik.handleChange}
                                onBlur={formik.handleBlur}
                                isInvalid={formik.touched.notes && !!formik.errors.notes}
                            />
                            <Form.Control.Feedback type="invalid">{formik.errors.notes}</Form.Control.Feedback>
                        </div>
                    </Col>

                    {/* Summary */}
                    <Col lg={4}>
                        <div className="tj-card p-4" style={{ position: 'sticky', top: 100 }}>
                            <h5 className="fw-bold mb-3" style={{ color: 'var(--tj-text-primary)' }}>Order Summary</h5>

                            {items.map((item: CartItem) => (
                                <div key={item.productId} className="d-flex justify-content-between mb-2">
                                    <span style={{ color: 'var(--tj-text-secondary)', fontSize: '0.85rem' }}>{item.productName} × {item.quantity}</span>
                                    <span className="fw-semibold" style={{ fontSize: '0.85rem', color: 'var(--tj-text-primary)' }}>{formatPrice(item.subtotal)}</span>
                                </div>
                            ))}

                            <hr style={{ borderColor: 'var(--tj-border)' }} />
                            <div className="d-flex justify-content-between mb-2">
                                <span style={{ color: 'var(--tj-text-muted)' }}>Shipping</span>
                                <span className="fw-semibold" style={{ color: 'var(--tj-success)' }}>Free</span>
                            </div>
                            <div className="d-flex justify-content-between mb-4">
                                <span className="fw-bold" style={{ color: 'var(--tj-text-primary)' }}>Total</span>
                                <span className="fw-bold" style={{ fontSize: '1.25rem', color: 'var(--tj-accent)' }}>{formatPrice(totalAmount)}</span>
                            </div>

                            <Button type="submit" variant="primary" className="w-100 py-3 d-flex align-items-center justify-content-center gap-2" disabled={formik.isSubmitting}>
                                <Lock size={16} /> Place Order
                            </Button>

                            <p className="text-center mt-3 mb-0" style={{ fontSize: '0.75rem', color: 'var(--tj-text-muted)' }}>
                                Your payment information is securely processed.
                            </p>
                        </div>
                    </Col>
                </Row>
            </form>
        </Container>
    );
}
