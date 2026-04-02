'use client';

import React, { useEffect, useState, Suspense } from 'react';
import Link from 'next/link';
import { Container, Button } from 'react-bootstrap';
import { authService } from '@/services/authService';
import { ThemeToggle } from '@/components/layout/ThemeToggle';
import { LocaleToggle } from '@/components/layout/LocaleToggle';
import { CheckCircle2, XCircle, ShoppingBag, Loader2, ArrowRight, ArrowLeft } from 'lucide-react';
import { useTranslation } from 'react-i18next';
import { useSearchParams } from 'next/navigation';

function ConfirmEmailContent() {
    const { t, i18n } = useTranslation();
    const isRtl = i18n.language === 'ar';
    const searchParams = useSearchParams();
    const [status, setStatus] = useState<'loading' | 'success' | 'error'>('loading');

    useEffect(() => {
        const userId = searchParams.get('userId');
        const code = searchParams.get('code');

        if (userId && code) {
            authService.confirmEmail({ userId, code })
                .then(response => {
                    if (response.succeeded) {
                        setStatus('success');
                    } else {
                        setStatus('error');
                    }
                })
                .catch(() => setStatus('error'));
        } else {
            setStatus('error');
        }
    }, [searchParams]);

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

                <div className="tj-card p-5 shadow-lg border-0 bg-opacity-10 backdrop-blur" style={{ background: 'var(--tj-card-bg)', borderRadius: '24px' }}>
                    {status === 'loading' && (
                        <div className="py-4">
                            <Loader2 size={64} className="text-accent animate-spin mb-4" />
                            <h2 className="fw-bold mb-2">{t('auth.confirmEmail.title')}</h2>
                            <p className="text-muted">{t('auth.confirmEmail.checking')}</p>
                        </div>
                    )}

                    {status === 'success' && (
                        <div className="py-4">
                            <div className="d-inline-flex align-items-center justify-content-center rounded-circle bg-success-subtle mb-4" style={{ width: 80, height: 80, color: 'var(--bs-success)' }}>
                                <CheckCircle2 size={40} />
                            </div>
                            <h2 className="fw-bold mb-2 text-success">{t('auth.confirmEmail.title')}</h2>
                            <p className="text-muted mb-4">{t('auth.confirmEmail.success')}</p>
                            <Link href="/login">
                                <Button variant="primary" className="w-100 py-3 rounded-3 d-flex align-items-center justify-content-center gap-2 fw-bold">
                                    {t('auth.confirmEmail.backToLogin')} {isRtl ? <ArrowLeft size={18} /> : <ArrowRight size={18} />}
                                </Button>
                            </Link>
                        </div>
                    )}

                    {status === 'error' && (
                        <div className="py-4">
                            <div className="d-inline-flex align-items-center justify-content-center rounded-circle bg-danger-subtle mb-4" style={{ width: 80, height: 80, color: 'var(--bs-danger)' }}>
                                <XCircle size={40} />
                            </div>
                            <h2 className="fw-bold mb-2 text-danger">{t('auth.confirmEmail.title')}</h2>
                            <p className="text-muted mb-4">{t('auth.confirmEmail.error')}</p>
                            <Link href="/login">
                                <Button variant="outline-primary" className="w-100 py-3 rounded-3 d-flex align-items-center justify-content-center gap-2 fw-bold">
                                    {t('auth.confirmEmail.backToLogin')}
                                </Button>
                            </Link>
                        </div>
                    )}
                </div>
            </div>
        </Container>
    );
}

export default function ConfirmEmailPage() {
    const { i18n } = useTranslation();
    const isRtl = i18n.language === 'ar';

    return (
        <div className="min-vh-100 d-flex align-items-center justify-content-center p-4" dir={isRtl ? 'rtl' : 'ltr'} style={{ background: 'var(--tj-bg)' }}>
            <div className={`position-absolute top-0 p-3 d-flex gap-2 ${isRtl ? 'start-0' : 'end-0'}`}>
                <LocaleToggle />
                <ThemeToggle />
            </div>

            <Suspense fallback={<div className="spinner-border text-primary" />}>
                <ConfirmEmailContent />
            </Suspense>
        </div>
    );
}
