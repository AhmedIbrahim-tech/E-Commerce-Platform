'use client';

import React, { Suspense } from 'react';
import Link from 'next/link';
import { Form, Button, Container } from 'react-bootstrap';
import { useFormik } from 'formik';
import { twoStepSchema } from '@/validators/authValidators';
import { authService } from '@/services/authService';
import { notify } from '@/lib/notify';
import { ThemeToggle } from '@/components/layout/ThemeToggle';
import { LocaleToggle } from '@/components/layout/LocaleToggle';
import { ShieldCheck, ArrowRight, ArrowLeft, ShoppingBag, Fingerprint } from 'lucide-react';
import { useTranslation } from 'react-i18next';
import { useRouter, useSearchParams } from 'next/navigation';
import { useAppDispatch } from '@/store/hooks';
import { updateCredentials } from '@/store/slices/authSlice';

function VerifyTwoFactorContent() {
    const { t, i18n } = useTranslation();
    const isRtl = i18n.language === 'ar';
    const router = useRouter();
    const searchParams = useSearchParams();
    const dispatch = useAppDispatch();
    const email = searchParams.get('email') || '';

    const formik = useFormik({
        initialValues: { code: '' },
        validationSchema: twoStepSchema,
        onSubmit: async (values) => {
            try {
                const response = await authService.twoStepVerification({
                    email,
                    code: values.code
                });

                if (response.succeeded && response.data) {
                    // response.data for 2FA should be JwtAuthResponse
                    // Note: Check backend to ensure this type matches
                    // If it returns a string, we might need to login again.
                    // For now, assuming it returns the AuthResponseData.
                    
                    notify.success(t('auth.twoStep.success') || 'Verified!');
                    router.push('/');
                } else {
                    notify.error(response.message || 'Verification failed');
                }
            } catch (err: unknown) {
                notify.error('Something went wrong');
            }
        },
    });

    return (
        <Container style={{ maxWidth: 460 }}>
            <div className="text-center mb-5">
                <Link href="/" className="d-inline-flex align-items-center gap-2 text-decoration-none mb-5">
                    <div className="d-flex align-items-center justify-content-center rounded-3 bg-accent" style={{ width: 44, height: 44 }}>
                        <ShoppingBag size={22} className="text-white" />
                    </div>
                    <span style={{ fontWeight: 800, fontSize: '1.375rem', color: 'var(--tj-text-primary)' }}>
                        {t('common.siteName')}
                    </span>
                </Link>

                <div className="d-inline-flex align-items-center justify-content-center rounded-circle bg-accent-subtle mb-4" style={{ width: 80, height: 80, color: 'var(--tj-accent)' }}>
                    <Fingerprint size={40} />
                </div>

                <h1 className="fw-bold mb-2" style={{ fontSize: '2.25rem', color: 'var(--tj-text-primary)', letterSpacing: '-0.02em' }}>
                    {t('auth.twoStep.title')}
                </h1>
                <p style={{ color: 'var(--tj-text-muted)', fontSize: '1.1rem', maxWidth: 320, margin: '0 auto' }}>
                    {t('auth.twoStep.description')}
                </p>
            </div>

            <div className="tj-card p-4 p-md-5">
                <Form onSubmit={formik.handleSubmit}>
                    <Form.Group className="mb-4 text-center">
                        <Form.Label className="fw-semibold small d-block mb-3" style={{ color: 'var(--tj-text-secondary)' }}>{t('auth.twoStep.codeLabel')}</Form.Label>
                        <Form.Control
                            name="code"
                            placeholder="000000"
                            value={formik.values.code}
                            onChange={(e) => {
                                const val = e.target.value.replace(/\D/g, '').slice(0, 6);
                                formik.setFieldValue('code', val);
                            }}
                            onBlur={formik.handleBlur}
                            isInvalid={formik.touched.code && !!formik.errors.code}
                            className="text-center fw-bold"
                            style={{ height: 64, borderRadius: '12px', fontSize: '2.2rem', letterSpacing: '0.4em' }}
                        />
                        <Form.Control.Feedback type="invalid">{formik.errors.code}</Form.Control.Feedback>
                    </Form.Group>

                    <Button type="submit" variant="primary" className="w-100 py-3 rounded-3 d-flex align-items-center justify-content-center gap-2 fw-bold shadow-sm" disabled={formik.isSubmitting} style={{ height: 52 }}>
                        {formik.isSubmitting ? (
                            <><span className="spinner-border spinner-border-sm" /> {t('auth.twoStep.verifying')}</>
                        ) : (
                            <>{t('auth.twoStep.verify')} {isRtl ? <ArrowLeft size={18} /> : <ArrowRight size={18} />}</>
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

export default function TwoStepVerificationPage() {
    const { i18n } = useTranslation();
    const isRtl = i18n.language === 'ar';

    return (
        <div className="min-vh-100 d-flex align-items-center justify-content-center p-4" dir={isRtl ? 'rtl' : 'ltr'} style={{ background: 'var(--tj-bg)' }}>
            <div className={`position-absolute top-0 p-3 d-flex gap-2 ${isRtl ? 'start-0' : 'end-0'}`}>
                <LocaleToggle />
                <ThemeToggle />
            </div>

            <Suspense fallback={<div className="spinner-border text-primary" />}>
                <VerifyTwoFactorContent />
            </Suspense>
        </div>
    );
}
