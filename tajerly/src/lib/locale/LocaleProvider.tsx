'use client';

import React, { createContext, useContext, useEffect, useState, useCallback } from 'react';
import { I18nextProvider } from 'react-i18next';
import i18n from '@/lib/i18n/config';

const LOCALE_KEY = 'i18nextLng'; // i18next-browser-languagedetector default key

export type Locale = 'en' | 'ar';

type LocaleContextValue = {
    locale: Locale;
    setLocale: (locale: Locale) => void;
    toggleLocale: () => void;
    dir: 'ltr' | 'rtl';
};

const LocaleContext = createContext<LocaleContextValue | null>(null);

function applyLocaleAttributes(locale: string) {
    const root = document.documentElement;
    const dir = locale === 'ar' ? 'rtl' : 'ltr';
    root.setAttribute('lang', locale);
    root.setAttribute('dir', dir);
    
    // Update document title dynamically based on locale
    const siteName = i18n.t('common.siteName', 'Tajerly');
    document.title = siteName;
}

export function LocaleProvider({ children }: { children: React.ReactNode }) {
    // Use the direct i18n instance since we are at the provider level
    const [locale, setLocaleState] = useState<Locale>((i18n.language as Locale) || 'en');

    useEffect(() => {
        // Sync attributes initially and listen for language changes
        applyLocaleAttributes(i18n.language);
        
        const handleLangChange = (lng: string) => {
            setLocaleState(lng as Locale);
            applyLocaleAttributes(lng);
        };

        i18n.on('languageChanged', handleLangChange);
        return () => {
            i18n.off('languageChanged', handleLangChange);
        };
    }, []);

    const setLocale = useCallback((next: Locale) => {
        i18n.changeLanguage(next);
    }, []);

    const toggleLocale = useCallback(() => {
        const next: Locale = i18n.language === 'en' ? 'ar' : 'en';
        setLocale(next);
    }, [setLocale]);

    const dir = locale === 'ar' ? 'rtl' : 'ltr';

    return (
        <I18nextProvider i18n={i18n}>
            <LocaleContext.Provider value={{ locale, setLocale, toggleLocale, dir }}>
                {children}
            </LocaleContext.Provider>
        </I18nextProvider>
    );
}

export function useLocale() {
    const ctx = useContext(LocaleContext);
    if (!ctx) throw new Error('useLocale must be used within LocaleProvider');
    return ctx;
}
