'use client';

import { useRef, useState, useEffect } from 'react';
import { Container, Button, Row, Col } from 'react-bootstrap';
import { ChevronLeft, ChevronRight } from 'lucide-react';
import { useTranslation } from 'react-i18next';
import Link from 'next/link';

interface CategorySliderProps {
    categories: any[];
}

export function CategorySlider({ categories }: CategorySliderProps) {
    const { t, i18n } = useTranslation();
    const isRtl = i18n.language === 'ar';
    const rowRef = useRef<HTMLDivElement>(null);
    const [activePage, setActivePage] = useState(0);

    const scroll = (direction: 'left' | 'right') => {
        if (!rowRef.current) return;
        const scrollAmount = 240;
        rowRef.current.scrollBy({
            left: direction === 'left' ? -scrollAmount : scrollAmount,
            behavior: 'smooth'
        });
    };

    useEffect(() => {
        const handleScroll = () => {
            if (rowRef.current) {
                const scrollLeft = Math.abs(rowRef.current.scrollLeft);
                const page = Math.round(scrollLeft / 400);
                setActivePage(page);
            }
        };
        const row = rowRef.current;
        row?.addEventListener('scroll', handleScroll);
        return () => row?.removeEventListener('scroll', handleScroll);
    }, []);

    return (
        <section className="py-5">
            <Container>
                <div className="text-center mb-5">
                    <h2 className="fw-bold mb-2" style={{ color: 'var(--tj-text-primary)', fontSize: '1.75rem' }}>{t('categoriesSection.popularCategories')}</h2>
                    <p className="mb-0 mx-auto" style={{ color: 'var(--tj-text-muted)', fontSize: '0.9rem', maxWidth: 600 }}>
                        {t('categoriesSection.browseCategories')}
                    </p>
                </div>
                <div className="position-relative px-md-5">
                    <Button 
                        variant="outline-primary" 
                        className="position-absolute rounded-circle p-0 d-md-flex align-items-center justify-content-center d-none glass-panel shadow-sm" 
                        style={{ width: 44, height: 44, left: -36, top: '50%', transform: 'translateY(-50%)', zIndex: 10, border: '1px solid var(--tj-border)' }}
                        onClick={() => scroll(isRtl ? 'right' : 'left')}
                    >
                        {isRtl ? <ChevronRight size={24} /> : <ChevronLeft size={24} />}
                    </Button>

                    <Button 
                        variant="outline-primary" 
                        className="position-absolute rounded-circle p-0 d-md-flex align-items-center justify-content-center d-none glass-panel shadow-sm" 
                        style={{ width: 44, height: 44, right: -36, top: '50%', transform: 'translateY(-50%)', zIndex: 10, border: '1px solid var(--tj-border)' }}
                        onClick={() => scroll(isRtl ? 'left' : 'right')}
                    >
                        {isRtl ? <ChevronLeft size={24} /> : <ChevronRight size={24} />}
                    </Button>

                    <Row ref={rowRef} className="flex-nowrap overflow-hidden g-4 justify-content-lg-center" style={{ scrollBehavior: 'smooth', msOverflowStyle: 'none', scrollbarWidth: 'none' }}>
                        {categories.map((cat, i) => (
                            <Col key={i} xs={6} md={4} lg={true} className="text-center flex-shrink-0" style={{ minWidth: 150 }}>
                                <Link href={`/products?category=${cat.name}`} className="text-decoration-none group">
                                    <div 
                                        className="mx-auto mb-3 rounded-4 d-flex align-items-center justify-content-center category-card-hover"
                                        style={{ width: 80, height: 80, background: 'var(--tj-bg-secondary)', border: '1px solid var(--tj-border)', transition: 'all 0.3s ease' }}
                                    >
                                        <cat.icon size={32} style={{ color: cat.color }} />
                                    </div>
                                    <h3 className="fw-bold" style={{ fontSize: '1rem', color: 'var(--tj-text-primary)' }}>{cat.name}</h3>
                                </Link>
                            </Col>
                        ))}
                    </Row>
                    
                    <div className="d-flex justify-content-center gap-2 mt-4">
                        {[0, 1].map((i) => (
                            <button
                                key={i}
                                onClick={() => {
                                    setActivePage(i);
                                    rowRef.current?.scrollTo({
                                        left: i * 400 * (isRtl ? -1 : 1),
                                        behavior: 'smooth'
                                    });
                                }}
                                className="border-0 rounded-circle p-0"
                                style={{
                                    width: 10,
                                    height: 10,
                                    background: i === activePage ? 'var(--tj-accent)' : 'var(--tj-accent-soft)',
                                    transition: 'all 0.3s ease',
                                    cursor: 'pointer'
                                }}
                            />
                        ))}
                    </div>
                </div>
            </Container>
        </section>
    );
}
