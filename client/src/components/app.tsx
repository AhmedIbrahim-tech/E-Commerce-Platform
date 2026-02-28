"use client";
import { useCustomizer } from '@/hooks/useCustomizer';
import React from "react";
import { ThemeProvider } from "@mui/material/styles";
import CssBaseline from "@mui/material/CssBaseline";
import RTL from "@/layouts/dashboard/shared/customizer/RTL";
import { ThemeSettings } from "@/utils/theme/Theme";
import { AppRouterCacheProvider } from '@mui/material-nextjs/v14-appRouter';
import "@/utils/i18n";
import AuthValidator from "@/components/AuthValidator";
import { NotifyProvider } from "@/context/NotifyContext";


const MyApp = ({ children }: { children: React.ReactNode }) => {
    const theme = ThemeSettings();
    const { activeDir } = useCustomizer();

    return (
        <>
            <AppRouterCacheProvider options={{ enableCssLayer: true }}>
                <ThemeProvider theme={theme}>
                    <NotifyProvider>
                        <RTL direction={activeDir}>
                            <CssBaseline />
                            <AuthValidator>
                                {children}
                            </AuthValidator>
                        </RTL>
                    </NotifyProvider>
                </ThemeProvider>
            </AppRouterCacheProvider>
        </>
    );
};

export default MyApp;
