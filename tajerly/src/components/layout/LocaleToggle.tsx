'use client';

import { useLocale } from '@/lib/locale/LocaleProvider';
import { Button } from 'react-bootstrap';

export function LocaleToggle() {
    const { locale, toggleLocale } = useLocale();

    return (
        <Button
            variant="link"
            onClick={toggleLocale}
            className="p-1 px-2 rounded-pill d-flex align-items-center gap-2 text-decoration-none"
            style={{
                transition: 'all 0.2s',
                border: '1.5px solid var(--tj-border)',
                color: 'var(--tj-text-muted)',
                background: 'var(--tj-bg-secondary)',
                minWidth: 64,
            }}
            aria-label={`Switch to ${locale === 'en' ? 'Arabic' : 'English'}`}
            id="btn-locale-toggle"
        >
            <div className="rounded-circle overflow-hidden d-flex" style={{ width: 18, height: 18 }}>
                <img
                    src={locale === 'en'
                        ? 'https://flagcdn.com/w40/gb.png'
                        : 'https://flagcdn.com/w40/sa.png'
                    }
                    alt={locale === 'en' ? 'English' : 'العربية'}
                    style={{
                        width: '100%',
                        height: '100%',
                        objectFit: 'cover',
                    }}
                />
            </div>
            <span className="fw-bold" style={{ fontSize: '0.7rem', textTransform: 'uppercase' }}>
                {locale === 'en' ? 'EN' : 'AR'}
            </span>
        </Button>
    );
}
