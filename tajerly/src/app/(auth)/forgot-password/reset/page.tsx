'use client';

import React, { Suspense, useState } from 'react';
import Link from 'next/link';
import { Form, Button, Container } from 'react-bootstrap';
import { useFormik } from 'formik';
import { resetPasswordSchema } from '@/validators/authValidators';
import { authService } from '@/services/authService';
import { notify } from '@/lib/notify';
import { ThemeToggle } from '@/components/layout/ThemeToggle';
import { LocaleToggle } from '@/components/layout/LocaleToggle';
import { Lock, Eye, EyeOff, ArrowRight, ArrowLeft, ShoppingBag, ShieldCheck } from 'lucide-react';
import { useTranslation } from 'react-i18next';
import { useRouter, useSearchParams } from 'next/navigation';

function ResetPasswordContent() {
    const { t, i18n } = useTranslation();
    const isRtl = i18n.language === 'ar';
    const router = useRouter();
    const searchParams = useSearchParams();
    const email = searchParams.get('email') || '';
    const [showPassword, setShowPassword] = useState(false);

    const formik = useFormik({
        initialValues: { password: '', confirmPassword: '' },
        validationSchema: resetPasswordSchema,
        onSubmit: async (values) => {
            try {
                const response = await authService.resetPassword({
                    email,
                    password: values.password,
                    confirmPassword: values.confirmPassword
                });

                if (response.succeeded) {
                    notify.success(t('auth.resetPassword.success'));
                    router.push('/login');
                } else {
                    notify.error(response.message || 'Reset failed');
                }
            } catch (err: unknown) {
                notify.error('Something went wrong');
            }
        },
    });

    return (
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
                    <Lock size={40} />
                </div>

                <h1 className="fw-bold mb-2" style={{ fontSize: '2.25rem', color: 'var(--tj-text-primary)', letterSpacing: '-0.02em' }}>
                    {t('auth.resetPassword.title')}
                </h1>
                <p style={{ color: 'var(--tj-text-muted)', fontSize: '1.1rem', maxWidth: 320, margin: '0 auto' }}>
                    {t('auth.resetPassword.description')}
                </p>
            </div>

            <div className="tj-card p-4 p-md-5">
                <Form onSubmit={formik.handleSubmit}>
                    <Form.Group className="mb-4">
                        <Form.Label className="fw-semibold small" style={{ color: 'var(--tj-text-secondary)' }}>{t('auth.resetPassword.passwordLabel')}</Form.Label>
                        <div className="position-relative">
                            <div className="position-absolute d-flex align-items-center justify-content-center" style={{ [isRtl ? 'right' : 'left']: 14, top: '50%', transform: 'translateY(-50%)', color: 'var(--tj-text-muted)' }}>
                                <Lock size={18} />
                            </div>
                            <Form.Control
                                type={showPassword ? 'text' : 'password'}
                                name="password"
                                placeholder="••••••••"
                                value={formik.values.password}
                                onChange={formik.handleChange}
                                onBlur={formik.handleBlur}
                                isInvalid={formik.touched.password && !!formik.errors.password}
                                style={{ [isRtl ? 'paddingRight' : 'paddingLeft']: 46, height: 52, borderRadius: '12px' }}
                            />
                            <button type="button" onClick={() => setShowPassword(!showPassword)} className="btn btn-link position-absolute p-1" style={{ [isRtl ? 'left' : 'right']: 10, top: '50%', transform: 'translateY(-50%)', color: 'var(--tj-text-muted)' }}>
                                {showPassword ? <EyeOff size={18} /> : <Eye size={18} />}
                            </button>
                            <Form.Control.Feedback type="invalid">{formik.errors.password}</Form.Control.Feedback>
                        </div>
                    </Form.Group>

                    <Form.Group className="mb-4">
                        <Form.Label className="fw-semibold small" style={{ color: 'var(--tj-text-secondary)' }}>{t('auth.resetPassword.confirmPasswordLabel')}</Form.Label>
                        <Form.Control
                            type="password"
                            name="confirmPassword"
                            placeholder="••••••••"
                            value={formik.values.confirmPassword}
                            onChange={formik.handleChange}
                            onBlur={formik.handleBlur}
                            isInvalid={formik.touched.confirmPassword && !!formik.errors.confirmPassword}
                            style={{ height: 52, borderRadius: '12px' }}
                        />
                        <Form.Control.Feedback type="invalid">{formik.errors.confirmPassword}</Form.Control.Feedback>
                    </Form.Group>

                    <Button type="submit" variant="primary" className="w-100 py-3 rounded-3 d-flex align-items-center justify-content-center gap-2 fw-bold shadow-sm" disabled={formik.isSubmitting} style={{ height: 52 }}>
                        {formik.isSubmitting ? (
                            <><span className="spinner-border spinner-border-sm" /> {t('auth.resetPassword.resetting')}</>
                        ) : (
                            <>{t('auth.resetPassword.reset')} {isRtl ? <ArrowLeft size={18} /> : <ArrowRight size={18} />}</>
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
    );
}

export default function ResetPasswordPage() {
    const { i18n } = useTranslation();
    const isRtl = i18n.language === 'ar';

    return (
        <div className="min-vh-100 d-flex align-items-center justify-content-center p-4" dir={isRtl ? 'rtl' : 'ltr'} style={{ background: 'var(--tj-bg)' }}>
            <div className={`position-absolute top-0 p-3 d-flex gap-2 ${isRtl ? 'start-0' : 'end-0'}`}>
                <LocaleToggle />
                <ThemeToggle />
            </div>

            <Suspense fallback={<div className="spinner-border text-primary" />}>
                <ResetPasswordContent />
            </Suspense>
        </div>
    );
}
