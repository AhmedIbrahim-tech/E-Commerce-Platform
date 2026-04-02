'use client';

import { Container, Button } from 'react-bootstrap';
import { useTranslation } from 'react-i18next';

export function Newsletter() {
    const { t } = useTranslation();

    return (
        <section className="py-5">
            <Container>
                <div className="rounded-4 p-5 text-center position-relative overflow-hidden" style={{ background: 'linear-gradient(135deg, var(--tj-accent), #B45309)', color: '#fff' }}>
                    <div className="position-absolute" style={{ top: 0, left: 0, right: 0, bottom: 0, background: "url('https://grainy-gradients.vercel.app/noise.svg')", opacity: 0.15 }} />
                    <div className="position-relative">
                        <h2 className="fw-bold mb-3" style={{ fontSize: '2rem' }}>{t('newsletter.title')}</h2>
                        <p className="mb-4" style={{ opacity: 0.9, maxWidth: 500, margin: '0 auto' }}>
                            {t('newsletter.description')}
                        </p>
                        <div className="d-flex gap-2 justify-content-center flex-wrap" style={{ maxWidth: 450, margin: '0 auto' }}>
                            <input type="email" className="form-control" placeholder={t('newsletter.placeholder')} style={{ maxWidth: 280, background: 'rgba(255,255,255,0.15)', border: '1px solid rgba(255,255,255,0.3)', color: '#fff' }} />
                            <Button variant="light" className="fw-bold px-4" style={{ color: 'var(--tj-accent)' }}>{t('newsletter.subscribe')}</Button>
                        </div>
                    </div>
                </div>
            </Container>
        </section>
    );
}
