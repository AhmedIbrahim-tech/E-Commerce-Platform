'use client';

import { Container, Row, Col, Button } from 'react-bootstrap';
import { ShoppingBag, Truck, Shield, Star, Sparkles, ArrowRight, ArrowLeft } from 'lucide-react';
import Link from 'next/link';
import { useTranslation } from 'react-i18next';

export function Hero() {
    const { t, i18n } = useTranslation();
    const isRtl = i18n.language === 'ar';

    return (
        <section className="position-relative overflow-hidden" style={{ background: 'var(--tj-bg-secondary)', paddingTop: '5rem', paddingBottom: '5rem' }}>
            <div className="position-absolute" style={{ top: '-15%', right: '-10%', width: '50%', height: '60%', background: 'var(--tj-accent)', opacity: 0.06, borderRadius: '50%', filter: 'blur(100px)' }} />
            <div className="position-absolute" style={{ bottom: '-10%', left: '-5%', width: '30%', height: '40%', background: 'var(--tj-accent)', opacity: 0.04, borderRadius: '50%', filter: 'blur(80px)' }} />

            <Container className="position-relative">
                <Row className="align-items-center g-5">
                    <Col lg={6}>
                        <div className="d-inline-flex align-items-center gap-2 px-3 py-2 rounded-pill mb-4" style={{ background: 'var(--tj-accent-soft)', border: '1px solid var(--tj-border)' }}>
                            <Sparkles size={14} style={{ color: 'var(--tj-accent)' }} />
                            <span style={{ fontSize: '0.75rem', fontWeight: 700, color: 'var(--tj-accent)', textTransform: 'uppercase', letterSpacing: '0.05em' }}>
                                {t('hero.badge')}
                            </span>
                        </div>

                        <h1 className="fw-bold mb-4" style={{ fontSize: 'clamp(2rem, 5vw, 3.5rem)', lineHeight: 1.1, color: 'var(--tj-text-primary)', letterSpacing: '-0.03em' }}>
                            {t('hero.title')} <br />
                            <span style={{ color: 'var(--tj-accent)' }}>{t('hero.titleAccent')}</span>
                        </h1>

                        <p className="mb-4" style={{ fontSize: '1.125rem', color: 'var(--tj-text-muted)', lineHeight: 1.7, maxWidth: 500 }}>
                            {t('hero.description')}
                        </p>

                        <div className="d-flex gap-3 flex-wrap">
                            <Link href="/products" className="btn btn-primary btn-lg d-flex align-items-center gap-2 px-4" id="hero-shop-now">
                                {t('hero.shopNow')} 
                                {isRtl ? <ArrowLeft size={18} /> : <ArrowRight size={18} />}
                            </Link>
                        </div>

                        <div className="d-flex gap-4 mt-5 flex-wrap">
                            {[
                                { icon: Truck, text: t('hero.freeShipping') },
                                { icon: Shield, text: t('hero.securePayment') },
                                { icon: Star, text: t('hero.topQuality') },
                            ].map((badge, i) => (
                                <div key={i} className="d-flex align-items-center gap-2">
                                    <badge.icon size={18} style={{ color: 'var(--tj-accent)' }} />
                                    <span style={{ fontSize: '0.825rem', fontWeight: 600, color: 'var(--tj-text-secondary)' }}>{badge.text}</span>
                                </div>
                            ))}
                        </div>
                    </Col>

                    <Col lg={6} className="d-none d-lg-block">
                        <div className="position-relative">
                            <img
                                src="https://images.unsplash.com/photo-1441986300917-64674bd600d8?q=80&w=600&auto=format&fit=crop"
                                alt="Shopping collection"
                                className="rounded-4 w-100"
                                style={{ boxShadow: 'var(--tj-shadow-lg)', maxHeight: 480, objectFit: 'cover' }}
                            />
                            <div
                                className="position-absolute rounded-3 p-3 animate-fade-in"
                                style={{ bottom: -20, left: -20, background: 'var(--tj-card)', border: '1px solid var(--tj-border)', boxShadow: 'var(--tj-shadow-lg)' }}
                            >
                                <div className="d-flex align-items-center gap-3">
                                    <div className="rounded-circle d-flex align-items-center justify-content-center" style={{ width: 48, height: 48, background: 'var(--tj-success-soft)' }}>
                                        <ShoppingBag size={20} style={{ color: 'var(--tj-success)' }} />
                                    </div>
                                    <div>
                                        <p className="mb-0 fw-bold" style={{ fontSize: '0.875rem', color: 'var(--tj-text-primary)' }}>{t('hero.productsCount')}</p>
                                        <p className="mb-0" style={{ fontSize: '0.75rem', color: 'var(--tj-text-muted)' }}>{t('hero.curatedForYou')}</p>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </Col>
                </Row>
            </Container>
        </section>
    );
}
