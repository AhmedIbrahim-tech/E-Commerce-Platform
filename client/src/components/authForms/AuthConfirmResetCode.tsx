'use client';

import React, { useState, useEffect } from 'react';
import Button from '@mui/material/Button';
import Stack from '@mui/material/Stack';
import Link from 'next/link';
import { useRouter } from 'next/navigation';
import { useForm } from 'react-hook-form';
import { yupResolver } from '@hookform/resolvers/yup';
import { useAppDispatch } from '@/hooks/useRedux';
import { confirmResetPasswordCode } from '@/store/slices/authSlice';
import { confirmResetPasswordSchema } from '@/validation/auth.schema';
import CustomTextField from '@/components/forms/theme-elements/CustomTextField';
import CustomFormLabel from '@/components/forms/theme-elements/CustomFormLabel';
import { useNotify } from '@/context/NotifyContext';

export default function AuthConfirmResetCode() {
    const [email, setEmail] = useState('');
    const [submitting, setSubmitting] = useState(false);
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
            notify('No email found. Please start from the forgot password page.', 'error');
            router.push('/forgot-password');
        }
    }, [notify, router]);

    const { register, handleSubmit, setValue, formState: { errors } } = useForm<{ code: string; email: string }>({
        resolver: yupResolver(confirmResetPasswordSchema),
        defaultValues: { code: '', email: '' },
    });

    useEffect(() => {
        if (email) setValue('email', email);
    }, [email, setValue]);

    const onSubmit = async (data: { code: string; email: string }) => {
        setSubmitting(true);
        const emailToUse = email || data.email;
        const result = await dispatch(confirmResetPasswordCode({ code: data.code, email: emailToUse }));
        setSubmitting(false);
        if (confirmResetPasswordCode.fulfilled.match(result)) {
            notify('Code verified successfully.', 'success');
            router.push('/forgot-password/reset');
        } else if (confirmResetPasswordCode.rejected.match(result)) {
            notify(typeof result.payload === 'string' ? result.payload : 'Invalid code', 'error');
        }
    };

    if (!ready) return null;

    return (
        <Stack mt={4} spacing={2} component="form" onSubmit={handleSubmit(onSubmit)}>
            <CustomFormLabel htmlFor="code">Verification Code</CustomFormLabel>
            <CustomTextField
                id="code"
                variant="outlined"
                fullWidth
                placeholder="6-digit code"
                {...register('code')}
                error={!!errors.code}
                helperText={errors.code?.message}
            />
            <Button color="primary" variant="contained" size="large" fullWidth type="submit" disabled={submitting}>
                Verify Code
            </Button>
            <Button color="primary" size="large" fullWidth component={Link} href="/forgot-password">
                Back
            </Button>
        </Stack>
    );
}
