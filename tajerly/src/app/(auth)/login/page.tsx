'use client';

import React, { useEffect } from 'react';
import { useRouter } from 'next/navigation';
import Link from 'next/link';
import { Container, Row, Col, Form, Button } from 'react-bootstrap';
import { useFormik } from 'formik';
import { loginSchema, loginInitialValues } from '@/validators/loginValidator';
import { useAppDispatch, useAppSelector } from '@/store/hooks';
import { loginUser } from '@/store/slices/authSlice';
import { notify } from '@/lib/notify';
import { ThemeToggle } from '@/components/layout/ThemeToggle';
import { LocaleToggle } from '@/components/layout/LocaleToggle';
import { siteConfig } from '@/config/site';
import { Mail, Lock, Eye, EyeOff, ArrowRight, ArrowLeft, ShoppingBag, Sparkles, Truck, Shield, CreditCard } from 'lucide-react';
import { useState } from 'react';
import { useTranslation } from 'react-i18next';
import type { RootState } from '@/store';

type PersistedRootState = RootState & { _persist?: { rehydrated?: boolean } };

export default function LoginPage() {
    const router = useRouter();
    const dispatch = useAppDispatch();
    const user = useAppSelector((state) => state.auth.user);
    const accessToken = useAppSelector((state) => state.auth.accessToken);
    const rehydrated = useAppSelector((state: PersistedRootState) => state._persist?.rehydrated === true);
    const [showPassword, setShowPassword] = useState(false);
    const { t, i18n } = useTranslation();
    const isRtl = i18n.language === 'ar';

    useEffect(() => {
        if (rehydrated && accessToken && user) {
            if (user.role?.toLowerCase() === 'admin') {
                router.replace('/admin');
            } else {
                router.replace('/');
            }
        }
    }, [rehydrated, accessToken, user, router]);

    const formik = useFormik({
        initialValues: loginInitialValues,
        validationSchema: loginSchema,
        onSubmit: async (values) => {
            try {
                const result = await dispatch(loginUser(values)).unwrap();
                notify.success(t('common.signIn'));

                if (result.user.role?.toLowerCase() === 'admin') {
                    router.push('/admin');
                } else {
                    router.push('/');
                }
            } catch (err: unknown) {
                notify.error((err as string) || 'Invalid credentials');
            }
        },
    });

    return (
        <div className={`min-vh-100 d-flex ${isRtl ? 'flex-row-reverse' : ''}`} dir={isRtl ? 'rtl' : 'ltr'}>
            {/* Left Panel - Branding */}
            <div
                className="d-none d-lg-flex flex-column justify-content-between p-5 position-relative"
                style={{
                    width: '50%',
                    background: `linear-gradient(rgba(0,0,0,0.7), rgba(0,0,0,0.7)), url('https://images.unsplash.com/photo-1441986300917-64674bd600d8?q=80&w=1200')`,
                    backgroundSize: 'cover',
                    backgroundPosition: 'center',
                    overflow: 'hidden',
                    color: '#fff'
                }}
            >
                <div className="position-absolute" style={{ top: '-10%', left: '-10%', width: '60%', height: '60%', background: 'var(--tj-accent)', opacity: 0.15, borderRadius: '50%', filter: 'blur(120px)' }} />

                <div className="position-relative z-1">
                    <Link href="/" className="d-flex align-items-center gap-2 text-decoration-none mb-5">
                        <div className="d-flex align-items-center justify-content-center rounded-3 bg-accent" style={{ width: 44, height: 44 }}>
                            <ShoppingBag size={22} className="text-white" />
                        </div>
                        <span style={{ fontWeight: 800, fontSize: '1.375rem', color: '#fff', letterSpacing: '-0.025em' }}>
                            {t('common.siteName')}
                        </span>
                    </Link>

                    <div className="d-inline-flex align-items-center gap-2 px-3 py-2 rounded-pill mb-4" style={{ background: 'rgba(255,255,255,0.1)', backdropFilter: 'blur(10px)', border: '1px solid rgba(255,255,255,0.2)' }}>
                        <Sparkles size={14} style={{ color: 'var(--tj-accent)' }} />
                        <span style={{ fontSize: '0.7rem', fontWeight: 700, color: 'var(--tj-accent)', textTransform: 'uppercase', letterSpacing: '0.1em' }}>{t('auth.login.badge')}</span>
                    </div>

                    <h2 className="fw-bold mb-4" style={{ fontSize: '3rem', lineHeight: 1.1, color: '#fff', letterSpacing: '-0.03em' }}>
                        {t('auth.login.title')} <br /><span style={{ color: 'var(--tj-accent)' }}>{t('auth.login.titleAccent')}</span>
                    </h2>

                    <p className="mb-5" style={{ color: 'rgba(255,255,255,0.8)', fontSize: '1.1rem', lineHeight: 1.7, maxWidth: 460 }}>
                        {t('auth.login.description')}
                    </p>

                    <div className="d-flex flex-column gap-4">
                        {[
                            { icon: Truck, title: t('auth.login.feat1Title'), desc: t('auth.login.feat1Desc') },
                            { icon: Shield, title: t('auth.login.feat2Title'), desc: t('auth.login.feat2Desc') },
                            { icon: CreditCard, title: t('auth.login.feat3Title'), desc: t('auth.login.feat3Desc') },
                        ].map((feat, i) => (
                            <div key={i} className="d-flex align-items-start gap-3">
                                <div className="p-2 rounded-3" style={{ background: 'rgba(255,255,255,0.1)', border: '1px solid rgba(255,255,255,0.1)' }}>
                                    <feat.icon size={20} style={{ color: 'var(--tj-accent)' }} />
                                </div>
                                <div>
                                    <h6 className="fw-bold mb-1" style={{ color: '#fff', fontSize: '1rem' }}>{feat.title}</h6>
                                    <p className="mb-0" style={{ color: 'rgba(255,255,255,0.6)', fontSize: '0.85rem' }}>{feat.desc}</p>
                                </div>
                            </div>
                        ))}
                    </div>
                </div>

                <p className="position-relative z-1" style={{ color: 'rgba(255,255,255,0.5)', fontSize: '0.7rem', fontWeight: 700, textTransform: 'uppercase', letterSpacing: '0.1em' }}>
                    {t('common.copyright')}
                </p>
            </div>

            {/* Right Panel - Form */}
            <div className="d-flex flex-column align-items-center justify-content-center flex-grow-1 p-4 p-md-5 position-relative" style={{ background: 'var(--tj-bg)' }}>
                <div className={`position-absolute top-0 p-3 d-flex gap-2 ${isRtl ? 'start-0' : 'end-0'}`}>
                    <LocaleToggle />
                    <ThemeToggle />
                </div>

                <div style={{ width: '100%', maxWidth: 400 }}>
                    <div className="d-lg-none d-flex align-items-center gap-2 mb-4 justify-content-center">
                        <div className="d-flex align-items-center justify-content-center rounded-3 bg-accent" style={{ width: 40, height: 40 }}>
                            <ShoppingBag size={20} className="text-white" />
                        </div>
                        <span className="fw-bold fs-4" style={{ color: 'var(--tj-text-primary)' }}>{t('common.siteName')}</span>
                    </div>

                    <div className="text-center text-lg-start mb-5">
                        <h1 className="fw-bold mb-2" style={{ fontSize: '2.25rem', color: 'var(--tj-text-primary)', letterSpacing: '-0.02em' }}>{t('auth.login.formTitle')}</h1>
                        <p style={{ color: 'var(--tj-text-muted)', fontSize: '1rem' }}>{t('auth.login.formSubtitle')}</p>
                    </div>

                    <Form onSubmit={formik.handleSubmit}>
                        <Form.Group className="mb-3">
                            <Form.Label className="fw-semibold small" style={{ color: 'var(--tj-text-secondary)' }}>{t('auth.login.emailLabel')}</Form.Label>
                            <div className="position-relative">
                                <div className={`position-absolute d-flex align-items-center justify-content-center`} style={{ [isRtl ? 'right' : 'left']: 14, top: '50%', transform: 'translateY(-50%)', color: 'var(--tj-text-muted)' }}>
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
                                    id="login-email"
                                />
                                <Form.Control.Feedback type="invalid">{formik.errors.email}</Form.Control.Feedback>
                            </div>
                        </Form.Group>

                        <Form.Group className="mb-4">
                            <div className="d-flex justify-content-between mb-1">
                                <Form.Label className="fw-semibold small" style={{ color: 'var(--tj-text-secondary)' }}>{t('auth.login.passwordLabel')}</Form.Label>
                                <Link href="/forgot-password" title={t('auth.login.forgot')} className="text-decoration-none fw-semibold" style={{ fontSize: '0.8rem', color: 'var(--tj-accent)' }}>{t('auth.login.forgot')}</Link>
                            </div>
                            <div className="position-relative">
                                <div className={`position-absolute d-flex align-items-center justify-content-center`} style={{ [isRtl ? 'right' : 'left']: 14, top: '50%', transform: 'translateY(-50%)', color: 'var(--tj-text-muted)' }}>
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
                                    style={{ [isRtl ? 'paddingRight' : 'paddingLeft']: 46, [isRtl ? 'paddingLeft' : 'paddingRight']: 46, height: 52, borderRadius: '12px' }}
                                    id="login-password"
                                />
                                <button type="button" onClick={() => setShowPassword(!showPassword)} className="btn btn-link position-absolute p-1" style={{ [isRtl ? 'left' : 'right']: 10, top: '50%', transform: 'translateY(-50%)', color: 'var(--tj-text-muted)' }}>
                                    {showPassword ? <EyeOff size={18} /> : <Eye size={18} />}
                                </button>
                                <Form.Control.Feedback type="invalid">{formik.errors.password}</Form.Control.Feedback>
                            </div>
                        </Form.Group>

                        <Button type="submit" variant="primary" className="w-100 py-3 rounded-3 d-flex align-items-center justify-content-center gap-2 fw-bold shadow-sm mt-2" disabled={formik.isSubmitting} id="login-submit" style={{ height: 52 }}>
                            {formik.isSubmitting ? (
                                <><span className="spinner-border spinner-border-sm" /> {t('auth.login.signingIn')}</>
                            ) : (
                                <>{t('auth.login.signInCount')} {isRtl ? <ArrowLeft size={18} /> : <ArrowRight size={18} />}</>
                            )}
                        </Button>

                        <div className="text-center my-4 position-relative">
                            <hr className="my-0" style={{ opacity: 0.1 }} />
                            <span className="position-absolute top-50 start-50 translate-middle px-3 small text-muted" style={{ background: 'var(--tj-bg)' }}>{t('common.or') || 'OR'}</span>
                        </div>

                        <Button
                            variant="outline-secondary"
                            className="w-100 py-3 rounded-3 d-flex align-items-center justify-content-center gap-2 fw-bold mb-3 border-light-subtle"
                            style={{ height: 52, background: 'var(--tj-card-bg)', color: 'var(--tj-text-primary)' }}
                            onClick={() => {/* Google Login Logic */ }}
                        >
                            <svg width="18" height="18" viewBox="0 0 24 24">
                                <path fill="#4285F4" d="M22.56 12.25c0-.78-.07-1.53-.2-2.25H12v4.26h5.92c-.26 1.37-1.04 2.53-2.21 3.31v2.77h3.57c2.08-1.92 3.28-4.74 3.28-8.09z" />
                                <path fill="#34A853" d="M12 23c2.97 0 5.46-.98 7.28-2.66l-3.57-2.77c-.98.66-2.23 1.06-3.71 1.06-2.86 0-5.29-1.93-6.16-4.53H2.18v2.84C3.99 20.53 7.7 23 12 23z" />
                                <path fill="#FBBC05" d="M5.84 14.09c-.22-.66-.35-1.36-.35-2.09s.13-1.43.35-2.09V7.07H2.18C1.43 8.55 1 10.22 1 12s.43 3.45 1.18 4.93l3.66-2.84z" />
                                <path fill="#EA4335" d="M12 5.38c1.62 0 3.06.56 4.21 1.64l3.15-3.15C17.45 2.09 14.97 1 12 1 7.7 1 3.99 3.47 2.18 7.07l3.66 2.84c.87-2.6 3.3-4.53 6.16-4.53z" />
                            </svg>
                            {t('auth.login.googleSignIn')}
                        </Button>
                    </Form>

                    <p className="text-center mt-5" style={{ color: 'var(--tj-text-muted)', fontSize: '0.9rem' }}>
                        {t('auth.login.noAccount')} <Link href="/register" className="text-decoration-none" style={{ color: 'var(--tj-accent)', fontWeight: 700 }}>{t('auth.login.createOne')}</Link>
                    </p>
                </div>
            </div>
        </div>
    );
}
