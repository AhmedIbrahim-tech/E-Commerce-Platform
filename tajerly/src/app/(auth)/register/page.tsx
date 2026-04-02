'use client';

import React from 'react';
import { useRouter } from 'next/navigation';
import Link from 'next/link';
import { Form, Button, Row, Col } from 'react-bootstrap';
import { useFormik } from 'formik';
import { registerSchema, registerInitialValues } from '@/validators/registerValidator';
import { useAppDispatch } from '@/store/hooks';
import { registerUser } from '@/store/slices/authSlice';
import { notify } from '@/lib/notify';
import { ThemeToggle } from '@/components/layout/ThemeToggle';
import { LocaleToggle } from '@/components/layout/LocaleToggle';
import { Mail, Lock, User, Phone, ShoppingBag, ArrowRight, ArrowLeft, Eye, EyeOff, Sparkles, Truck, Shield, CreditCard, AtSign } from 'lucide-react';
import { useState } from 'react';
import { useTranslation } from 'react-i18next';

export default function RegisterPage() {
    const router = useRouter();
    const dispatch = useAppDispatch();
    const [showPassword, setShowPassword] = useState(false);
    const { t, i18n } = useTranslation();
    const isRtl = i18n.language === 'ar';

    const formik = useFormik({
        initialValues: registerInitialValues,
        validationSchema: registerSchema,
        onSubmit: async (values) => {
            try {
                await dispatch(registerUser(values)).unwrap();
                notify.success(t('auth.register.success'));
                router.push('/login');
            } catch (err: unknown) {
                notify.error((err as string) || 'Registration failed');
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
                        <span style={{ fontSize: '0.7rem', fontWeight: 700, color: 'var(--tj-accent)', textTransform: 'uppercase', letterSpacing: '0.1em' }}>{t('auth.register.titleAccent')}</span>
                    </div>

                    <h2 className="fw-bold mb-4" style={{ fontSize: '3rem', lineHeight: 1.1, color: '#fff', letterSpacing: '-0.03em' }}>
                        {t('auth.register.title')}
                    </h2>

                    <p className="mb-5" style={{ color: 'rgba(255,255,255,0.8)', fontSize: '1.1rem', lineHeight: 1.7, maxWidth: 460 }}>
                        {t('auth.register.description')}
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
            <div className="d-flex flex-column align-items-center justify-content-center flex-grow-1 p-4 p-md-5 position-relative" style={{ background: 'var(--tj-bg)', overflowY: 'auto' }}>
                <div className={`position-absolute top-0 p-3 d-flex gap-2 ${isRtl ? 'start-0' : 'end-0'}`} style={{ zIndex: 10 }}>
                    <LocaleToggle />
                    <ThemeToggle />
                </div>

                <div style={{ width: '100%', maxWidth: 500, paddingTop: '2rem', paddingBottom: '2rem' }}>
                    <div className="d-lg-none d-flex align-items-center gap-2 mb-4 justify-content-center">
                        <div className="d-flex align-items-center justify-content-center rounded-3 bg-accent" style={{ width: 40, height: 40 }}>
                            <ShoppingBag size={20} className="text-white" />
                        </div>
                        <span className="fw-bold fs-4" style={{ color: 'var(--tj-text-primary)' }}>{t('common.siteName')}</span>
                    </div>

                    <div className="text-center text-lg-start mb-4">
                        <h1 className="fw-bold mb-2" style={{ fontSize: '2.25rem', color: 'var(--tj-text-primary)', letterSpacing: '-0.02em' }}>{t('auth.register.formTitle')}</h1>
                        <p style={{ color: 'var(--tj-text-muted)', fontSize: '1rem' }}>{t('auth.register.formSubtitle')}</p>
                    </div>

                    <Form onSubmit={formik.handleSubmit}>
                        <Row>
                            <Col md={6}>
                                <Form.Group className="mb-3">
                                    <Form.Label className="fw-semibold small" style={{ color: 'var(--tj-text-secondary)' }}>{t('auth.register.firstNameLabel')}</Form.Label>
                                    <div className="position-relative">
                                        <div className="position-absolute d-flex align-items-center justify-content-center" style={{ [isRtl ? 'right' : 'left']: 14, top: '50%', transform: 'translateY(-50%)', color: 'var(--tj-text-muted)' }}>
                                            <User size={18} />
                                        </div>
                                        <Form.Control
                                            name="firstName"
                                            placeholder="John"
                                            value={formik.values.firstName}
                                            onChange={formik.handleChange}
                                            onBlur={formik.handleBlur}
                                            isInvalid={formik.touched.firstName && !!formik.errors.firstName}
                                            style={{ [isRtl ? 'paddingRight' : 'paddingLeft']: 46, height: 52, borderRadius: '12px' }}
                                        />
                                        <Form.Control.Feedback type="invalid">{formik.errors.firstName}</Form.Control.Feedback>
                                    </div>
                                </Form.Group>
                            </Col>
                            <Col md={6}>
                                <Form.Group className="mb-3">
                                    <Form.Label className="fw-semibold small" style={{ color: 'var(--tj-text-secondary)' }}>{t('auth.register.lastNameLabel')}</Form.Label>
                                    <Form.Control
                                        name="lastName"
                                        placeholder="Doe"
                                        value={formik.values.lastName}
                                        onChange={formik.handleChange}
                                        onBlur={formik.handleBlur}
                                        isInvalid={formik.touched.lastName && !!formik.errors.lastName}
                                        style={{ height: 52, borderRadius: '12px' }}
                                    />
                                    <Form.Control.Feedback type="invalid">{formik.errors.lastName}</Form.Control.Feedback>
                                </Form.Group>
                            </Col>
                        </Row>

                        <Form.Group className="mb-3">
                            <Form.Label className="fw-semibold small" style={{ color: 'var(--tj-text-secondary)' }}>{t('auth.register.userNameLabel')}</Form.Label>
                            <div className="position-relative">
                                <div className="position-absolute d-flex align-items-center justify-content-center" style={{ [isRtl ? 'right' : 'left']: 14, top: '50%', transform: 'translateY(-50%)', color: 'var(--tj-text-muted)' }}>
                                    <AtSign size={18} />
                                </div>
                                <Form.Control
                                    name="userName"
                                    placeholder="johndoe"
                                    value={formik.values.userName}
                                    onChange={formik.handleChange}
                                    onBlur={formik.handleBlur}
                                    isInvalid={formik.touched.userName && !!formik.errors.userName}
                                    style={{ [isRtl ? 'paddingRight' : 'paddingLeft']: 46, height: 52, borderRadius: '12px' }}
                                />
                                <Form.Control.Feedback type="invalid">{formik.errors.userName}</Form.Control.Feedback>
                            </div>
                        </Form.Group>

                        <Form.Group className="mb-3">
                            <Form.Label className="fw-semibold small" style={{ color: 'var(--tj-text-secondary)' }}>{t('auth.register.emailLabel')}</Form.Label>
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

                        <Row>
                            <Col md={12}>
                                <Form.Group className="mb-3">
                                    <Form.Label className="fw-semibold small" style={{ color: 'var(--tj-text-secondary)' }}>{t('auth.register.phoneLabel')}</Form.Label>
                                    <div className="position-relative">
                                        <div className="position-absolute d-flex align-items-center justify-content-center" style={{ [isRtl ? 'right' : 'left']: 14, top: '50%', transform: 'translateY(-50%)', color: 'var(--tj-text-muted)' }}>
                                            <Phone size={18} />
                                        </div>
                                        <Form.Control
                                            name="phone"
                                            placeholder="+20 1234 567 890"
                                            value={formik.values.phone}
                                            onChange={formik.handleChange}
                                            onBlur={formik.handleBlur}
                                            isInvalid={formik.touched.phone && !!formik.errors.phone}
                                            style={{ [isRtl ? 'paddingRight' : 'paddingLeft']: 46, height: 52, borderRadius: '12px' }}
                                        />
                                        <Form.Control.Feedback type="invalid">{formik.errors.phone}</Form.Control.Feedback>
                                    </div>
                                </Form.Group>
                            </Col>
                        </Row>

                        <Row>
                            <Col md={6}>
                                <Form.Group className="mb-3">
                                    <Form.Label className="fw-semibold small" style={{ color: 'var(--tj-text-secondary)' }}>{t('auth.register.passwordLabel')}</Form.Label>
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
                            </Col>
                            <Col md={6}>
                                <Form.Group className="mb-4">
                                    <Form.Label className="fw-semibold small" style={{ color: 'var(--tj-text-secondary)' }}>{t('auth.register.confirmPasswordLabel')}</Form.Label>
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
                            </Col>
                        </Row>

                        <Button type="submit" variant="primary" className="w-100 py-3 rounded-3 d-flex align-items-center justify-content-center gap-2 fw-bold shadow-sm" disabled={formik.isSubmitting} id="register-submit" style={{ height: 52 }}>
                            {formik.isSubmitting ? (
                                <><span className="spinner-border spinner-border-sm" /> {t('auth.register.creating')}</>
                            ) : (
                                <>{t('auth.register.createAccount')} {isRtl ? <ArrowLeft size={18} /> : <ArrowRight size={18} />}</>
                            )}
                        </Button>
                    </Form>

                    <p className="text-center mt-4" style={{ color: 'var(--tj-text-muted)', fontSize: '0.9rem' }}>
                        {t('auth.register.alreadyHaveAccount')} <Link href="/login" className="text-decoration-none" style={{ color: 'var(--tj-accent)', fontWeight: 700 }}>{t('auth.register.signIn')}</Link>
                    </p>
                </div>
            </div>
        </div>
    );
}
