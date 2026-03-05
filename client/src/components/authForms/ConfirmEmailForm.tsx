'use client'
import React, { useState, useEffect } from 'react';
import { useRouter, useSearchParams } from 'next/navigation';
import { useAppDispatch, useAppSelector } from '@/hooks/useRedux';
import { confirmEmail, clearError } from '@/store/slices/authSlice';
import Box from '@mui/material/Box';
import Button from '@mui/material/Button';
import Typography from '@mui/material/Typography';
import CircularProgress from '@mui/material/CircularProgress';
import { useNotify } from '@/context/NotifyContext';

const ConfirmEmailForm = () => {
    const dispatch = useAppDispatch();
    const router = useRouter();
    const searchParams = useSearchParams();
    const userId = searchParams.get('userId');
    const code = searchParams.get('code');

    const { notify } = useNotify();
    const { confirmEmailLoading } = useAppSelector((state) => state.auth);
    const [initiated, setInitiated] = useState(false);

    useEffect(() => {
        if (!userId || !code) {
            setInitiated(true);
            notify('Invalid confirmation link. User ID or temporary code is missing.', 'error');
            return;
        }

        if (!initiated) {
            setInitiated(true);
            dispatch(clearError());
            dispatch(confirmEmail({ userId, code })).then((resultAction) => {
                if (confirmEmail.fulfilled.match(resultAction)) {
                    notify('Email confirmed successfully! You can now log in.', 'success');
                    setTimeout(() => router.push('/login'), 3000);
                } else if (confirmEmail.rejected.match(resultAction)) {
                    const msg = typeof resultAction.payload === 'string' ? resultAction.payload : 'Email confirmation failed';
                    notify(msg, 'error');
                }
            });
        }
    }, [userId, code, dispatch, router, initiated, notify]);

    return (
        <Box>
            <Typography fontWeight="700" variant="h3" mb={3} textAlign="center">
                Confirming Your Email
            </Typography>

            {confirmEmailLoading && (
                <Box display="flex" justifyContent="center" my={4}>
                    <CircularProgress />
                </Box>
            )}

            <Button
                color="primary"
                variant="outlined"
                size="large"
                fullWidth
                onClick={() => router.push('/login')}
                sx={{ mt: 2 }}
            >
                Back to Login
            </Button>
        </Box>
    );
};

export default ConfirmEmailForm;
