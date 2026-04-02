'use client';

import React from 'react';
import Link from 'next/link';
import { Container, Row, Col, Form, Button } from 'react-bootstrap';
import { PageHeader } from '@/components/shared/PageHeader';
import { useAppSelector } from '@/store/hooks';
import { User, Mail, Phone, Camera } from 'lucide-react';
import { useTranslation } from 'react-i18next';

export default function ProfilePage() {
    const user = useAppSelector((state) => state.auth.user);
    const { t, i18n } = useTranslation();
    const isRtl = i18n.language === 'ar';

    return (
        <Container className="py-4" dir={isRtl ? 'rtl' : 'ltr'}>
            <PageHeader 
                title={t('common.profile')} 
                subtitle={t('footer.accountSection')} 
                breadcrumbs={[{ label: t('common.profile') }]} 
            />

            <Row className="g-4">
                <Col lg={4}>
                    <div className="tj-card p-4 text-center">
                        <div className="d-flex justify-content-center mb-3">
                            <div className="rounded-circle d-flex align-items-center justify-content-center position-relative" style={{ width: 100, height: 100, background: 'var(--tj-accent-soft)' }}>
                                <User size={40} style={{ color: 'var(--tj-accent)' }} />
                                <button className="btn btn-link p-1 rounded-circle position-absolute" style={{ bottom: 0, right: 0, width: 32, height: 32, background: 'var(--tj-accent)', color: 'var(--tj-accent-foreground)' }}>
                                    <Camera size={14} />
                                </button>
                            </div>
                        </div>
                        <h5 className="fw-bold mb-1" style={{ color: 'var(--tj-text-primary)' }}>{user?.name || 'User'}</h5>
                        <p className="mb-0" style={{ color: 'var(--tj-text-muted)', fontSize: '0.875rem' }}>{user?.email || ''}</p>
                    </div>
                </Col>

                <Col lg={8}>
                    <div className="tj-card p-4">
                        <h5 className="fw-bold mb-4" style={{ color: 'var(--tj-text-primary)' }}>{t('footer.accountSection')}</h5>
                        <Form>
                            <Row className="g-3">
                                <Col md={6}>
                                    <Form.Group>
                                        <Form.Label>{t('auth.register.firstNameLabel')}</Form.Label>
                                        <Form.Control defaultValue={user?.name?.split(' ')[0] || ''} />
                                    </Form.Group>
                                </Col>
                                <Col md={6}>
                                    <Form.Group>
                                        <Form.Label>{t('auth.register.lastNameLabel')}</Form.Label>
                                        <Form.Control defaultValue={user?.name?.split(' ').slice(1).join(' ') || ''} />
                                    </Form.Group>
                                </Col>
                                <Col md={6}>
                                    <Form.Group>
                                        <Form.Label>{t('auth.login.emailLabel')}</Form.Label>
                                        <Form.Control type="email" defaultValue={user?.email || ''} />
                                    </Form.Group>
                                </Col>
                                <Col md={6}>
                                    <Form.Group>
                                        <Form.Label>{t('auth.register.phoneLabel')}</Form.Label>
                                        <Form.Control placeholder="+20 1234 567 890" />
                                    </Form.Group>
                                </Col>
                            </Row>
                            <Button variant="primary" className="mt-4 px-4">{'Save Changes'}</Button>
                        </Form>
                    </div>

                    <div className="tj-card p-4 mt-4">
                        <div className="d-flex align-items-center justify-content-between">
                            <div>
                                <h5 className="fw-bold mb-1" style={{ color: 'var(--tj-text-primary)' }}>{t('auth.changePassword.title')}</h5>
                                <p className="mb-0 text-muted small">{t('auth.changePassword.subtitle')}</p>
                            </div>
                            <Link href="/profile/change-password">
                                <Button variant="outline-primary" className="px-4">{t('auth.changePassword.title')}</Button>
                            </Link>
                        </div>
                    </div>
                </Col>
            </Row>
        </Container>
    );
}
