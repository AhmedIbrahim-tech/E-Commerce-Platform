"use client";
import { useCustomizer } from '@/hooks/useCustomizer';
import React, { useContext } from "react";
import { ThemeProvider } from "@mui/material/styles";
import CssBaseline from "@mui/material/CssBaseline";
import RTL from "@/layouts/dashboard/shared/customizer/RTL";
import { ThemeSettings } from "@/utils/theme/Theme";
import { AppRouterCacheProvider } from '@mui/material-nextjs/v14-appRouter';
import "@/utils/i18n";


const MyApp = ({ children }: { children: React.ReactNode }) => {
    const theme = ThemeSettings();
    const { activeDir } = useCustomizer();

    return (
        <>
            <AppRouterCacheProvider options={{ enableCssLayer: true }}>
                <ThemeProvider theme={theme}>
                    <RTL direction={activeDir}>
                        <CssBaseline />
                        {children}
                    </RTL>
                </ThemeProvider>
            </AppRouterCacheProvider>
        </>
    );
};

export default MyApp;
