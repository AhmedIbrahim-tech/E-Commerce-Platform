'use client';

import React, { useState } from 'react';
import Box from '@mui/material/Box';
import Button from '@mui/material/Button';
import Typography from '@mui/material/Typography';
import Link from 'next/link';
import { useRouter } from 'next/navigation';
import { useForm } from 'react-hook-form';
import { yupResolver } from '@hookform/resolvers/yup';
import { useDispatch } from 'react-redux';
import { AppDispatch } from '@/store/store';
import { twoStepVerification } from '@/store/slices/authSlice';
import { twoStepSchema } from '@/validation/auth.schema';
import CustomTextField from '@/components/forms/theme-elements/CustomTextField';
import CustomFormLabel from '@/components/forms/theme-elements/CustomFormLabel';
import { Stack } from '@mui/system';
import { useNotify } from '@/context/NotifyContext';

const AuthTwoSteps = () => {
    const [submitting, setSubmitting] = useState(false);
    const router = useRouter();
    const dispatch = useDispatch<AppDispatch>();
    const { notify } = useNotify();

    const { register, handleSubmit, formState: { errors } } = useForm<{ code: string }>({
        resolver: yupResolver(twoStepSchema),
    });

    const onSubmit = async (data: { code: string }) => {
        setSubmitting(true);
        const result = await dispatch(twoStepVerification({ code: data.code }));
        setSubmitting(false);
        if (twoStepVerification.fulfilled.match(result)) {
            router.push('/');
        } else if (twoStepVerification.rejected.match(result)) {
            notify(typeof result.payload === 'string' ? result.payload : 'Verification failed', 'error');
        }
    };

    return (
        <Box mt={4}>
            <Stack mb={3} component="form" onSubmit={handleSubmit(onSubmit)}>
                <CustomFormLabel htmlFor="code">Type your 6 digits security code</CustomFormLabel>
                <CustomTextField
                    id="code"
                    variant="outlined"
                    fullWidth
                    placeholder="000000"
                    {...register('code')}
                    error={!!errors.code}
                    helperText={errors.code?.message}
                />
                <Button
                    color="primary"
                    variant="contained"
                    size="large"
                    fullWidth
                    type="submit"
                    disabled={submitting}
                    sx={{ mt: 2 }}
                >
                    Verify My Account
                </Button>
            </Stack>

            <Stack direction="row" spacing={1} mt={3}>
                <Typography color="textSecondary" variant="h6" fontWeight="400">
                    Didn&apos;t get the code?
                </Typography>
                <Typography
                    component={Link}
                    href="/"
                    fontWeight="500"
                    sx={{ textDecoration: 'none', color: 'primary.main' }}
                >
                    Resend
                </Typography>
            </Stack>
        </Box>
    );
};

export default AuthTwoSteps;
