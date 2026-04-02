'use client';

import React from 'react';
import { Container, Row, Col, Form, Button } from 'react-bootstrap';
import { PageHeader } from '@/components/shared/PageHeader';
import { useFormik } from 'formik';
import { changePasswordSchema } from '@/validators/authValidators';
import { authService } from '@/services/authService';
import { notify } from '@/lib/notify';
import { Lock, ShieldCheck, ArrowRight, ArrowLeft } from 'lucide-react';
import { useTranslation } from 'react-i18next';
import { useRouter } from 'next/navigation';

export default function ChangePasswordPage() {
    const { t, i18n } = useTranslation();
    const isRtl = i18n.language === 'ar';
    const router = useRouter();

    const formik = useFormik({
        initialValues: {
            currentPassword: '',
            newPassword: '',
            confirmPassword: '',
        },
        validationSchema: changePasswordSchema,
        onSubmit: async (values) => {
            try {
                const response = await authService.changePassword(values);
                if (response.succeeded) {
                    notify.success(t('auth.changePassword.success') || 'Password changed successfully');
                    router.push('/profile');
                } else {
                    notify.error(response.message || 'Change failed');
                }
            } catch (err: unknown) {
                notify.error('Something went wrong');
            }
        },
    });

    return (
        <Container className="py-4" dir={isRtl ? 'rtl' : 'ltr'}>
            <PageHeader 
                title={t('auth.changePassword.title') || 'Change Password'} 
                subtitle={t('auth.changePassword.subtitle') || 'Update your security credentials'} 
                breadcrumbs={[
                    { label: t('common.profile'), path: '/profile' },
                    { label: t('auth.changePassword.title') || 'Change Password' }
                ]} 
            />

            <Row className="justify-content-center mt-4">
                <Col lg={6}>
                    <div className="tj-card p-4 p-md-5">
                        <div className="d-flex align-items-center gap-3 mb-4 p-3 rounded-4 bg-accent-soft text-accent">
                            <ShieldCheck size={32} />
                            <div>
                                <h6 className="fw-bold mb-0">Security Update</h6>
                                <p className="mb-0 small opacity-75">Ensure your new password is strong.</p>
                            </div>
                        </div>

                        <Form onSubmit={formik.handleSubmit}>
                            <Form.Group className="mb-3">
                                <Form.Label className="fw-semibold small text-secondary">Current Password</Form.Label>
                                <div className="position-relative">
                                    <div className="position-absolute d-flex align-items-center justify-content-center" style={{ [isRtl ? 'right' : 'left']: 14, top: '50%', transform: 'translateY(-50%)', color: 'var(--tj-text-muted)' }}>
                                        <Lock size={18} />
                                    </div>
                                    <Form.Control
                                        type="password"
                                        name="currentPassword"
                                        placeholder="••••••••"
                                        value={formik.values.currentPassword}
                                        onChange={formik.handleChange}
                                        onBlur={formik.handleBlur}
                                        isInvalid={formik.touched.currentPassword && !!formik.errors.currentPassword}
                                        style={{ [isRtl ? 'paddingRight' : 'paddingLeft']: 46, height: 52, borderRadius: '12px' }}
                                    />
                                    <Form.Control.Feedback type="invalid">{formik.errors.currentPassword}</Form.Control.Feedback>
                                </div>
                            </Form.Group>

                            <hr className="my-4 opacity-10" />

                            <Form.Group className="mb-3">
                                <Form.Label className="fw-semibold small text-secondary">New Password</Form.Label>
                                <div className="position-relative">
                                    <div className="position-absolute d-flex align-items-center justify-content-center" style={{ [isRtl ? 'right' : 'left']: 14, top: '50%', transform: 'translateY(-50%)', color: 'var(--tj-text-muted)' }}>
                                        <Lock size={18} />
                                    </div>
                                    <Form.Control
                                        type="password"
                                        name="newPassword"
                                        placeholder="••••••••"
                                        value={formik.values.newPassword}
                                        onChange={formik.handleChange}
                                        onBlur={formik.handleBlur}
                                        isInvalid={formik.touched.newPassword && !!formik.errors.newPassword}
                                        style={{ [isRtl ? 'paddingRight' : 'paddingLeft']: 46, height: 52, borderRadius: '12px' }}
                                    />
                                    <Form.Control.Feedback type="invalid">{formik.errors.newPassword}</Form.Control.Feedback>
                                </div>
                            </Form.Group>

                            <Form.Group className="mb-4">
                                <Form.Label className="fw-semibold small text-secondary">Confirm New Password</Form.Label>
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
                                    <><span className="spinner-border spinner-border-sm" /> updating...</>
                                ) : (
                                    <>{t('auth.resetPassword.reset') || 'Update Password'} {isRtl ? <ArrowLeft size={18} /> : <ArrowRight size={18} />}</>
                                )}
                            </Button>
                        </Form>
                    </div>
                </Col>
            </Row>
        </Container>
    );
}
