'use client'
import Grid from '@mui/material/Grid';
import Box from '@mui/material/Box';
import PageContainer from "@/components/container/PageContainer";
import Logo from "@/layouts/dashboard/shared/logo/Logo";
import ConfirmEmailForm from "@/components/authForms/ConfirmEmailForm";
import Image from "next/image";

export default function ConfirmEmailPage() {
    return (
        <PageContainer title="Confirm Email" description="Confirm your email to complete registration">
            <Grid
                container
                spacing={0}
                justifyContent="center"
                sx={{ overflowX: "hidden" }}
            >
                <Grid
                    size={{
                        xs: 12,
                        sm: 12,
                        lg: 7,
                        xl: 8
                    }}
                    sx={{
                        position: "relative",
                        "&:before": {
                            content: '""',
                            background: "radial-gradient(#d2f1df, #d3d7fa, #bad8f4)",
                            backgroundSize: "400% 400%",
                            animation: "gradient 15s ease infinite",
                            position: "absolute",
                            height: "100%",
                            width: "100%",
                            opacity: "0.3",
                        },
                    }}>
                    <Box position="relative">
                        <Box px={3}>
                            <Logo />
                        </Box>
                        <Box
                            alignItems="center"
                            justifyContent="center"
                            height={"calc(100vh - 75px)"}
                            sx={{
                                display: {
                                    xs: "none",
                                    lg: "flex",
                                },
                            }}
                        >
                            <Image
                                src={"/images/backgrounds/login-bg.svg"}
                                alt="bg" width={500} height={500}
                                style={{
                                    width: "100%",
                                    maxWidth: "500px", maxHeight: '500px',
                                }}
                            />
                        </Box>
                    </Box>
                </Grid>
                <Grid
                    size={{
                        xs: 12,
                        sm: 12,
                        lg: 5,
                        xl: 4
                    }}
                    display="flex"
                    justifyContent="center"
                    alignItems="center"
                >
                    <Box p={4} sx={{ width: '100%', maxWidth: 450 }}>
                        <ConfirmEmailForm />
                    </Box>
                </Grid>
            </Grid>
        </PageContainer>
    );
}
