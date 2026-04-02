'use client';

import React from 'react';
import Link from 'next/link';
import { Form, Button, Container } from 'react-bootstrap';
import { useFormik } from 'formik';
import { forgotPasswordSchema } from '@/validators/authValidators';
import { authService } from '@/services/authService';
import { notify } from '@/lib/notify';
import { ThemeToggle } from '@/components/layout/ThemeToggle';
import { LocaleToggle } from '@/components/layout/LocaleToggle';
import { Mail, ArrowRight, ArrowLeft, ShoppingBag, KeyRound } from 'lucide-react';
import { useTranslation } from 'react-i18next';
import { useRouter } from 'next/navigation';

export default function ForgotPasswordPage() {
    const { t, i18n } = useTranslation();
    const isRtl = i18n.language === 'ar';
    const router = useRouter();

    const formik = useFormik({
        initialValues: { email: '' },
        validationSchema: forgotPasswordSchema,
        onSubmit: async (values) => {
            try {
                const response = await authService.sendResetPasswordCode(values.email);
                if (response.succeeded) {
                    notify.success(t('auth.forgotPassword.success'));
                    // Pass email in query string to next step
                    router.push(`/forgot-password/confirm?email=${encodeURIComponent(values.email)}`);
                } else {
                    notify.error(response.message || 'Error sending code');
                }
            } catch (err: unknown) {
                notify.error('Something went wrong');
            }
        },
    });

    return (
        <div className="min-vh-100 d-flex align-items-center justify-content-center p-4" dir={isRtl ? 'rtl' : 'ltr'} style={{ background: 'var(--tj-bg)' }}>
            <div className={`position-absolute top-0 p-3 d-flex gap-2 ${isRtl ? 'start-0' : 'end-0'}`}>
                <LocaleToggle />
                <ThemeToggle />
            </div>

            <Container style={{ maxWidth: 460 }}>
                <div className="text-center mb-5">
                    <Link href="/" className="d-inline-flex align-items-center gap-2 text-decoration-none mb-4">
                        <div className="d-flex align-items-center justify-content-center rounded-3 bg-accent" style={{ width: 44, height: 44 }}>
                            <ShoppingBag size={22} className="text-white" />
                        </div>
                        <span style={{ fontWeight: 800, fontSize: '1.375rem', color: 'var(--tj-text-primary)' }}>
                            {t('common.siteName')}
                        </span>
                    </Link>

                    <div className="d-inline-flex align-items-center justify-content-center rounded-circle bg-accent-subtle mb-4" style={{ width: 80, height: 80, color: 'var(--tj-accent)' }}>
                        <KeyRound size={40} />
                    </div>

                    <h1 className="fw-bold mb-2" style={{ fontSize: '2.25rem', color: 'var(--tj-text-primary)', letterSpacing: '-0.02em' }}>
                        {t('auth.forgotPassword.title')}
                    </h1>
                    <p style={{ color: 'var(--tj-text-muted)', fontSize: '1.1rem', maxWidth: 320, margin: '0 auto' }}>
                        {t('auth.forgotPassword.description')}
                    </p>
                </div>

                <div className="tj-card p-4 p-md-5">
                    <Form onSubmit={formik.handleSubmit}>
                        <Form.Group className="mb-4">
                            <Form.Label className="fw-semibold small" style={{ color: 'var(--tj-text-secondary)' }}>{t('auth.forgotPassword.emailLabel')}</Form.Label>
                            <div className="position-relative">
                                <div className="position-absolute d-flex align-items-center justify-content-center" style={{ [isRtl ? 'right' : 'left']: 14, top: '50%', transform: 'translateY(-50%)', color: 'var(--tj-text-muted)' }}>
                                    <Mail size={18} />
                                </div>
                                <Form.Control
                                    type="email"
                                    name="email"
                                    placeholder="you@example.com"
                                    value={formik.values.email}
                                    onChange={formik.handleChange}
                                    onBlur={formik.handleBlur}
                                    isInvalid={formik.touched.email && !!formik.errors.email}
                                    style={{ [isRtl ? 'paddingRight' : 'paddingLeft']: 46, height: 52, borderRadius: '12px' }}
                                />
                                <Form.Control.Feedback type="invalid">{formik.errors.email}</Form.Control.Feedback>
                            </div>
                        </Form.Group>

                        <Button type="submit" variant="primary" className="w-100 py-3 rounded-3 d-flex align-items-center justify-content-center gap-2 fw-bold shadow-sm" disabled={formik.isSubmitting} style={{ height: 52 }}>
                            {formik.isSubmitting ? (
                                <><span className="spinner-border spinner-border-sm" /> {t('auth.forgotPassword.sending')}</>
                            ) : (
                                <>{t('auth.forgotPassword.sendCode')} {isRtl ? <ArrowLeft size={18} /> : <ArrowRight size={18} />}</>
                            )}
                        </Button>
                    </Form>
                </div>

                <div className="text-center mt-5">
                    <Link href="/login" className="text-decoration-none d-inline-flex align-items-center gap-2 fw-semibold" style={{ color: 'var(--tj-text-muted)', fontSize: '0.95rem' }}>
                        {isRtl ? <ArrowRight size={18} /> : <ArrowLeft size={18} />} {t('auth.forgotPassword.backToLogin')}
                    </Link>
                </div>
            </Container>
        </div>
    );
}
