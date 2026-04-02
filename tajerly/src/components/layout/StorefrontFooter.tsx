'use client';

import Link from 'next/link';
import { Container, Row, Col } from 'react-bootstrap';
import { ShoppingBag, Mail, Phone, MapPin } from 'lucide-react';
import { siteConfig } from '@/config/site';
import { useTranslation } from 'react-i18next';

export function StorefrontFooter() {
    const { t } = useTranslation();
    return (
        <footer style={{ background: 'var(--tj-card)', borderTop: '1px solid var(--tj-border)' }} className="mt-5 pt-5 pb-4">
            <Container>
                <Row className="g-5">
                    {/* Brand Section */}
                    <Col lg={4} md={12} className="text-center text-lg-start">
                        <div className="d-flex align-items-center justify-content-center justify-content-lg-start gap-3 mb-4">
                            <div
                                className="d-flex align-items-center justify-content-center rounded-3 shadow-sm bg-accent"
                                style={{ width: 44, height: 44 }}
                            >
                                <ShoppingBag size={22} />
                            </div>
                            <span style={{ fontWeight: 900, fontSize: '1.5rem', color: 'var(--tj-text-primary)', letterSpacing: '-0.02em' }}>
                                {siteConfig.name}
                            </span>
                        </div>
                        <p style={{ color: 'var(--tj-text-muted)', fontSize: '0.925rem', lineHeight: 1.8, maxWidth: 360 }} className="mb-4 mx-auto mx-lg-0">
                            {t('footer.description')}
                        </p>
                    </Col>

                    {/* Navigation Columns */}
                    <Col lg={4} md={12}>
                        <Row className="g-4">
                            <Col xs={6} className="text-center text-lg-start">
                                <h6 className="fw-bold mb-4" style={{ color: 'var(--tj-text-primary)', fontSize: '0.9rem', textTransform: 'uppercase', letterSpacing: '0.05em' }}>
                                    {t('footer.shopSection')}
                                </h6>
                                <ul className="list-unstyled d-flex flex-column gap-3">
                                    {[
                                        { href: '/products', label: t('footer.allProducts') },
                                        { href: '/products?featured=true', label: t('footer.featured') },
                                    ].map((link, i) => (
                                        <li key={i}>
                                            <Link href={link.href} className="text-decoration-none footer-link" style={{ color: 'var(--tj-text-muted)', fontSize: '0.875rem' }}>
                                                {link.label}
                                            </Link>
                                        </li>
                                    ))}
                                </ul>
                            </Col>
                            <Col xs={6} className="text-center text-lg-start">
                                <h6 className="fw-bold mb-4" style={{ color: 'var(--tj-text-primary)', fontSize: '0.9rem', textTransform: 'uppercase', letterSpacing: '0.05em' }}>
                                    {t('footer.accountSection')}
                                </h6>
                                <ul className="list-unstyled d-flex flex-column gap-3">
                                    {[
                                        { href: '/profile', label: t('footer.myProfile') },
                                        { href: '/orders', label: t('footer.myOrders') },
                                        { href: '/wishlist', label: t('footer.wishlist') },
                                        { href: '/addresses', label: t('footer.addresses') },
                                    ].map((link, i) => (
                                        <li key={i}>
                                            <Link href={link.href} className="text-decoration-none footer-link" style={{ color: 'var(--tj-text-muted)', fontSize: '0.875rem' }}>
                                                {link.label}
                                            </Link>
                                        </li>
                                    ))}
                                </ul>
                            </Col>
                        </Row>
                    </Col>

                    {/* Contact Section */}
                    <Col lg={4} md={12} className="text-center text-lg-start">
                        <h6 className="fw-bold mb-4" style={{ color: 'var(--tj-text-primary)', fontSize: '0.9rem', textTransform: 'uppercase', letterSpacing: '0.05em' }}>
                            {t('footer.contactSection')}
                        </h6>
                        <ul className="list-unstyled d-flex flex-column gap-3 align-items-center align-items-lg-start">
                            <li className="d-flex align-items-center gap-3" style={{ color: 'var(--tj-text-muted)', fontSize: '0.875rem' }}>
                                <div className="contact-icon-wrapper rounded-circle d-flex align-items-center justify-content-center shadow-sm" style={{ width: 32, height: 32, background: 'var(--tj-bg-secondary)', border: '1px solid var(--tj-border)' }}>
                                    <Mail size={14} style={{ color: 'var(--tj-accent)' }} />
                                </div>
                                <span className="footer-link cursor-pointer">{t('common.contactEmail')}</span>
                            </li>
                            <li className="d-flex align-items-center gap-3" style={{ color: 'var(--tj-text-muted)', fontSize: '0.875rem' }}>
                                <div className="contact-icon-wrapper rounded-circle d-flex align-items-center justify-content-center shadow-sm" style={{ width: 32, height: 32, background: 'var(--tj-bg-secondary)', border: '1px solid var(--tj-border)' }}>
                                    <Phone size={14} style={{ color: 'var(--tj-accent)' }} />
                                </div>
                                <span className="footer-link cursor-pointer">{t('common.contactPhone')}</span>
                            </li>
                            <li className="d-flex align-items-center gap-3" style={{ color: 'var(--tj-text-muted)', fontSize: '0.875rem' }}>
                                <div className="contact-icon-wrapper rounded-circle d-flex align-items-center justify-content-center shadow-sm" style={{ width: 32, height: 32, background: 'var(--tj-bg-secondary)', border: '1px solid var(--tj-border)' }}>
                                    <MapPin size={14} style={{ color: 'var(--tj-accent)' }} />
                                </div>
                                <span className="footer-link cursor-pointer">{t('common.contactAddress')}</span>
                            </li>
                        </ul>
                    </Col>
                </Row>

                {/* Bottom Bar */}
                <hr style={{ borderColor: 'var(--tj-border)', opacity: 0.5 }} className="mt-5 mb-4" />
                <div className="d-flex flex-column flex-md-row justify-content-between align-items-center gap-3">
                    <p className="mb-0" style={{ color: 'var(--tj-text-muted)', fontSize: '0.8rem', fontWeight: 500 }}>
                        {t('common.copyright')}
                    </p>
                    <div className="d-flex gap-4">
                        <Link href="#" className="text-decoration-none footer-link" style={{ color: 'var(--tj-text-muted)', fontSize: '0.8rem', fontWeight: 500 }}>
                            {t('footer.privacyPolicy')}
                        </Link>
                        <Link href="#" className="text-decoration-none footer-link" style={{ color: 'var(--tj-text-muted)', fontSize: '0.8rem', fontWeight: 500 }}>
                            {t('footer.termsOfService')}
                        </Link>
                    </div>
                </div>
            </Container>
        </footer>
    );
}
