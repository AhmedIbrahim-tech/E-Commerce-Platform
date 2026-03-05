'use client';

import React, { useState, useEffect } from 'react';
import Button from '@mui/material/Button';
import Stack from '@mui/material/Stack';
import Typography from '@mui/material/Typography';
import Link from 'next/link';
import { useRouter } from 'next/navigation';
import { useForm } from 'react-hook-form';
import { yupResolver } from '@hookform/resolvers/yup';
import { useAppDispatch } from '@/hooks/useRedux';
import { resetPassword } from '@/store/slices/authSlice';
import { resetPasswordSchema } from '@/validation/auth.schema';
import CustomTextField from '@/components/forms/theme-elements/CustomTextField';
import CustomFormLabel from '@/components/forms/theme-elements/CustomFormLabel';
import { useNotify } from '@/context/NotifyContext';

type FormData = { newPassword: string; confirmPassword: string };

export default function AuthResetPassword() {
    const [email, setEmail] = useState('');
    const [submitting, setSubmitting] = useState(false);
    const [success, setSuccess] = useState(false);
    const [ready, setReady] = useState(false);
    const router = useRouter();
    const dispatch = useAppDispatch();
    const { notify } = useNotify();

    useEffect(() => {
        const stored = sessionStorage.getItem('resetPasswordEmail');
        if (stored) {
            setEmail(stored);
            setReady(true);
        } else {
            notify('Session expired. Please start from the forgot password page.', 'error');
            router.push('/forgot-password');
        }
    }, [notify, router]);

    const { register, handleSubmit, formState: { errors } } = useForm<FormData>({
        resolver: yupResolver(resetPasswordSchema),
    });

    const onSubmit = async (data: FormData) => {
        if (!email) return;
        setSubmitting(true);
        const result = await dispatch(resetPassword({
            email,
            newPassword: data.newPassword,
            confirmPassword: data.confirmPassword,
        }));
        setSubmitting(false);
        if (resetPassword.fulfilled.match(result)) {
            setSuccess(true);
            sessionStorage.removeItem('resetPasswordEmail');
            notify('Password reset successfully. Redirecting to login...', 'success');
            setTimeout(() => router.push('/login'), 2000);
        } else if (resetPassword.rejected.match(result)) {
            notify(typeof result.payload === 'string' ? result.payload : 'Failed to reset password', 'error');
        }
    };

    if (!ready) return null;

    if (success) {
        return (
            <Stack mt={4} spacing={2}>
                <Typography color="text.secondary">Redirecting to login...</Typography>
            </Stack>
        );
    }

    return (
        <Stack mt={4} spacing={2} component="form" onSubmit={handleSubmit(onSubmit)}>
            <CustomFormLabel htmlFor="newPassword">New Password</CustomFormLabel>
            <CustomTextField
                id="newPassword"
                type="password"
                variant="outlined"
                fullWidth
                {...register('newPassword')}
                error={!!errors.newPassword}
                helperText={errors.newPassword?.message}
            />
            <CustomFormLabel htmlFor="confirmPassword">Confirm Password</CustomFormLabel>
            <CustomTextField
                id="confirmPassword"
                type="password"
                variant="outlined"
                fullWidth
                {...register('confirmPassword')}
                error={!!errors.confirmPassword}
                helperText={errors.confirmPassword?.message}
            />
            <Button color="primary" variant="contained" size="large" fullWidth type="submit" disabled={submitting}>
                Reset Password
            </Button>
            <Button color="primary" size="large" fullWidth component={Link} href="/login">
                Back to Login
            </Button>
        </Stack>
    );
}
