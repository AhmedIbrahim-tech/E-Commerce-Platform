'use client';

export const dynamic = 'force-dynamic';

import Box from '@mui/material/Box';
import Card from '@mui/material/Card';
import Grid from '@mui/material/Grid';
import Typography from '@mui/material/Typography';
import Logo from '@/layouts/dashboard/shared/logo/Logo';
import PageContainer from '@/components/container/PageContainer';
import AuthConfirmResetCode from '@/components/authForms/AuthConfirmResetCode';

export default function ConfirmCodePage() {
    return (
        <PageContainer title="Confirm Reset Code" description="Enter the code sent to your email">
            <Box
                sx={{
                    position: 'relative',
                    '&:before': {
                        content: '""',
                        background: 'radial-gradient(#d2f1df, #d3d7fa, #bad8f4)',
                        backgroundSize: '400% 400%',
                        animation: 'gradient 15s ease infinite',
                        position: 'absolute',
                        height: '100%',
                        width: '100%',
                        opacity: '0.3',
                    },
                }}
            >
                <Grid container spacing={0} justifyContent="center" sx={{ height: '100vh' }}>
                    <Grid display="flex" justifyContent="center" alignItems="center" size={{ xs: 12, sm: 12, lg: 4, xl: 3 }}>
                        <Card elevation={9} sx={{ p: 4, zIndex: 1, width: '100%', maxWidth: '500px' }}>
                            <Box display="flex" alignItems="center" justifyContent="center">
                                <Logo />
                            </Box>
                            <Typography color="textSecondary" textAlign="center" variant="subtitle2" fontWeight="400">
                                Enter the 6-digit code sent to your email.
                            </Typography>
                            <AuthConfirmResetCode />
                        </Card>
                    </Grid>
                </Grid>
            </Box>
        </PageContainer>
    );
}
