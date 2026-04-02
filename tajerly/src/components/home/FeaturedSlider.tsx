'use client';

import { useRef, useState, useEffect } from 'react';
import { Container, Button, Row, Col } from 'react-bootstrap';
import { ChevronLeft, ChevronRight, ArrowRight, ArrowLeft } from 'lucide-react';
import { useTranslation } from 'react-i18next';
import Link from 'next/link';
import { ProductCard } from '@/components/shared/ProductCard';

interface FeaturedSliderProps {
    products: any[];
}

export function FeaturedSlider({ products }: FeaturedSliderProps) {
    const { t, i18n } = useTranslation();
    const isRtl = i18n.language === 'ar';
    const rowRef = useRef<HTMLDivElement>(null);
    const [activePage, setActivePage] = useState(0);

    const scroll = (direction: 'left' | 'right') => {
        if (!rowRef.current) return;
        const scrollAmount = 320;
        rowRef.current.scrollBy({
            left: direction === 'left' ? -scrollAmount : scrollAmount,
            behavior: 'smooth'
        });
    };

    useEffect(() => {
        const handleScroll = () => {
            if (rowRef.current) {
                const scrollLeft = Math.abs(rowRef.current.scrollLeft);
                const page = Math.round(scrollLeft / 600);
                setActivePage(page);
            }
        };
        const row = rowRef.current;
        row?.addEventListener('scroll', handleScroll);
        return () => row?.removeEventListener('scroll', handleScroll);
    }, []);

    return (
        <section className="py-5" style={{ background: 'var(--tj-bg-secondary)' }}>
            <Container>
                <div className="d-flex justify-content-between align-items-center mb-4">
                    <div>
                        <h2 className="fw-bold mb-1" style={{ color: 'var(--tj-text-primary)', fontSize: '1.5rem' }}>{t('featured.title')}</h2>
                        <p className="mb-0" style={{ color: 'var(--tj-text-muted)', fontSize: '0.875rem' }}>{t('featured.subtitle')}</p>
                    </div>
                    <Link href="/products?featured=true" className="btn btn-outline-primary btn-sm px-3 d-flex align-items-center gap-2">
                        {t('common.viewAll')}
                        {isRtl ? <ArrowLeft size={14} /> : <ArrowRight size={14} />}
                    </Link>
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

                    <Row ref={rowRef} className="flex-nowrap overflow-hidden g-4" style={{ scrollBehavior: 'smooth', msOverflowStyle: 'none', scrollbarWidth: 'none' }}>
                        {products.map((product, idx) => (
                            <Col key={product.id} lg={3} md={6}>
                                <ProductCard {...product} index={idx} onAddToCart={() => {}} onToggleWishlist={() => {}} />
                            </Col>
                        ))}
                    </Row>
                    
                    <div className="d-flex justify-content-center gap-2 mt-4">
                        {[0, 1, 2, 3].map((i) => (
                            <button
                                key={i}
                                onClick={() => {
                                    setActivePage(i);
                                    rowRef.current?.scrollTo({
                                        left: i * 600 * (isRtl ? -1 : 1),
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
