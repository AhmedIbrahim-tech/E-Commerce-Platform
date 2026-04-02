'use client';

import React, { Suspense } from 'react';
import Link from 'next/link';
import { Form, Button, Container } from 'react-bootstrap';
import { useFormik } from 'formik';
import { verifyCodeSchema } from '@/validators/authValidators';
import { authService } from '@/services/authService';
import { notify } from '@/lib/notify';
import { ThemeToggle } from '@/components/layout/ThemeToggle';
import { LocaleToggle } from '@/components/layout/LocaleToggle';
import { CheckCircle2, ArrowRight, ArrowLeft, ShoppingBag, ShieldCheck } from 'lucide-react';
import { useTranslation } from 'react-i18next';
import { useRouter, useSearchParams } from 'next/navigation';

function ConfirmCodeContent() {
    const { t, i18n } = useTranslation();
    const isRtl = i18n.language === 'ar';
    const router = useRouter();
    const searchParams = useSearchParams();
    const email = searchParams.get('email') || '';

    const formik = useFormik({
        initialValues: { code: '' },
        validationSchema: verifyCodeSchema,
        onSubmit: async (values) => {
            try {
                const response = await authService.confirmResetPasswordCode({ email, code: values.code });
                if (response.succeeded) {
                    notify.success(t('auth.verifyCode.success') || 'Code verified!');
                    router.push(`/forgot-password/reset?email=${encodeURIComponent(email)}&code=${values.code}`);
                } else {
                    notify.error(response.message || 'Verification failed');
                }
            } catch (err: unknown) {
                notify.error('Something went wrong');
            }
        },
    });

    const handleResend = async () => {
        try {
            const response = await authService.sendResetPasswordCode(email);
            if (response.succeeded) {
                notify.success(t('auth.forgotPassword.success'));
            } else {
                notify.error(response.message || 'Error resending code');
            }
        } catch {
            notify.error('Something went wrong');
        }
    };

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
                    <ShieldCheck size={40} />
                </div>

                <h1 className="fw-bold mb-2" style={{ fontSize: '2.25rem', color: 'var(--tj-text-primary)', letterSpacing: '-0.02em' }}>
                    {t('auth.verifyCode.title')}
                </h1>
                <p style={{ color: 'var(--tj-text-muted)', fontSize: '1.1rem', maxWidth: 320, margin: '0 auto' }}>
                    {t('auth.verifyCode.description')}
                    <br /><span className="fw-bold text-accent">{email}</span>
                </p>
            </div>

            <div className="tj-card p-4 p-md-5">
                <Form onSubmit={formik.handleSubmit}>
                    <Form.Group className="mb-4 text-center">
                        <Form.Label className="fw-semibold small d-block mb-3" style={{ color: 'var(--tj-text-secondary)' }}>{t('auth.verifyCode.codeLabel')}</Form.Label>
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
                            style={{ height: 64, borderRadius: '12px', fontSize: '2rem', letterSpacing: '0.5em', borderStyle: 'dashed', borderWidth: '2px' }}
                        />
                        <Form.Control.Feedback type="invalid" className="text-start">{formik.errors.code}</Form.Control.Feedback>
                    </Form.Group>

                    <Button type="submit" variant="primary" className="w-100 py-3 rounded-3 d-flex align-items-center justify-content-center gap-2 fw-bold shadow-sm mb-3" disabled={formik.isSubmitting} style={{ height: 52 }}>
                        {formik.isSubmitting ? (
                            <><span className="spinner-border spinner-border-sm" /> {t('auth.verifyCode.verifying')}</>
                        ) : (
                            <>{t('auth.verifyCode.verify')} {isRtl ? <ArrowLeft size={18} /> : <ArrowRight size={18} />}</>
                        )}
                    </Button>

                    <Button variant="link" className="w-100 text-decoration-none fw-bold" style={{ color: 'var(--tj-accent)' }} onClick={handleResend}>
                        {t('auth.verifyCode.resendCode')}
                    </Button>
                </Form>
            </div>

            <div className="text-center mt-5">
                <Link href="/forgot-password" className="text-decoration-none d-inline-flex align-items-center gap-2 fw-semibold" style={{ color: 'var(--tj-text-muted)', fontSize: '0.95rem' }}>
                    {isRtl ? <ArrowRight size={18} /> : <ArrowLeft size={18} />} {t('auth.forgotPassword.backToLogin')}
                </Link>
            </div>
        </Container>
    );
}

export default function ConfirmCodePage() {
    const { i18n } = useTranslation();
    const isRtl = i18n.language === 'ar';

    return (
        <div className="min-vh-100 d-flex align-items-center justify-content-center p-4" dir={isRtl ? 'rtl' : 'ltr'} style={{ background: 'var(--tj-bg)' }}>
            <div className={`position-absolute top-0 p-3 d-flex gap-2 ${isRtl ? 'start-0' : 'end-0'}`}>
                <LocaleToggle />
                <ThemeToggle />
            </div>

            <Suspense fallback={<div className="spinner-border text-primary" />}>
                <ConfirmCodeContent />
            </Suspense>
        </div>
    );
}
