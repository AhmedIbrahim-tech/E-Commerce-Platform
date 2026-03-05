'use client';

import React, { useState } from 'react';
import Button from '@mui/material/Button';
import Stack from '@mui/material/Stack';
import Link from 'next/link';
import { useRouter } from 'next/navigation';
import { useForm } from 'react-hook-form';
import { yupResolver } from '@hookform/resolvers/yup';
import { useAppDispatch } from '@/hooks/useRedux';
import { sendResetPasswordCode } from '@/store/slices/authSlice';
import { forgotPasswordSchema } from '@/validation/auth.schema';
import CustomTextField from '@/components/forms/theme-elements/CustomTextField';
import CustomFormLabel from '@/components/forms/theme-elements/CustomFormLabel';
import { useNotify } from '@/context/NotifyContext';

export default function AuthForgotPassword() {
    const [submitting, setSubmitting] = useState(false);
    const router = useRouter();
    const dispatch = useAppDispatch();
    const { notify } = useNotify();

    const { register, handleSubmit, formState: { errors } } = useForm<{ email: string }>({
        resolver: yupResolver(forgotPasswordSchema),
    });

    const onSubmit = async (data: { email: string }) => {
        setSubmitting(true);
        const result = await dispatch(sendResetPasswordCode({ email: data.email }));
        setSubmitting(false);
        if (sendResetPasswordCode.fulfilled.match(result)) {
            if (typeof window !== 'undefined') sessionStorage.setItem('resetPasswordEmail', data.email);
            notify('A reset code has been sent to your email.', 'success');
            router.push('/forgot-password/confirm-code');
        } else if (sendResetPasswordCode.rejected.match(result)) {
            notify(typeof result.payload === 'string' ? result.payload : 'Failed to send code', 'error');
        }
    };

    return (
        <Stack mt={4} spacing={2} component="form" onSubmit={handleSubmit(onSubmit)}>
            <CustomFormLabel htmlFor="reset-email">Email Address</CustomFormLabel>
            <CustomTextField
                id="reset-email"
                variant="outlined"
                fullWidth
                {...register('email')}
                error={!!errors.email}
                helperText={errors.email?.message}
            />
            <Button color="primary" variant="contained" size="large" fullWidth type="submit" disabled={submitting}>
                Send Reset Code
            </Button>
            <Button color="primary" size="large" fullWidth component={Link} href="/login">
                Back to Login
            </Button>
        </Stack>
    );
}
