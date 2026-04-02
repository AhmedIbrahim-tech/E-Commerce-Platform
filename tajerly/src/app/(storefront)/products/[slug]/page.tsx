'use client';

import { Container, Row, Col, Button, Badge } from 'react-bootstrap';
import { PageHeader } from '@/components/shared/PageHeader';
import { ProductCard } from '@/components/shared/ProductCard';
import { QuantitySelector } from '@/components/shared/QuantitySelector';
import { Star, Heart, ShoppingCart, Truck, Shield, RefreshCw } from 'lucide-react';
import { formatPrice, getDiscountPercent } from '@/utils/formatters';
import { useState } from 'react';
import { use } from 'react';

interface ProductDetailPageProps {
    params: Promise<{ slug: string }>;
}

// Mock product detail
const mockProduct = {
    id: 3, name: 'Smart Fitness Watch Pro', slug: 'smart-fitness-watch-pro',
    description: 'The Smart Fitness Watch Pro is your ultimate companion for an active lifestyle. Featuring advanced health monitoring, GPS tracking, and a stunning AMOLED display. Built with premium materials and water resistance up to 50 meters.',
    shortDescription: 'Advanced smartwatch with health monitoring and GPS',
    price: 2499.99, compareAtPrice: 2999.99, sku: 'SFW-PRO-001', stock: 22,
    thumbnailUrl: 'https://images.unsplash.com/photo-1523275335684-37898b6baf30?q=80&w=600',
    images: [
        { id: 1, url: 'https://images.unsplash.com/photo-1523275335684-37898b6baf30?q=80&w=600', alt: 'Watch front', displayOrder: 0 },
        { id: 2, url: 'https://images.unsplash.com/photo-1546868871-af0de0ae72be?q=80&w=600', alt: 'Watch side', displayOrder: 1 },
    ],
    categoryId: 1, categoryName: 'Electronics', brand: 'TechPro',
    tags: ['smartwatch', 'fitness', 'gps'], averageRating: 4.9, reviewCount: 256,
    isActive: true, isFeatured: true, specifications: { 'Display': '1.4" AMOLED', 'Battery': '14 days', 'Water Resistance': '50m', 'Weight': '52g' },
    createdAt: '2025-01-15',
};

export default function ProductDetailPage({ params }: ProductDetailPageProps) {
    const { slug } = use(params);
    const [quantity, setQuantity] = useState(1);
    const [selectedImage, setSelectedImage] = useState(0);
    const discount = getDiscountPercent(mockProduct.price, mockProduct.compareAtPrice);

    return (
        <Container className="py-4">
            <PageHeader title="" breadcrumbs={[{ label: 'Shop', href: '/products' }, { label: mockProduct.name }]} />

            <Row className="g-5">
                {/* Images */}
                <Col lg={6}>
                    <div className="position-relative rounded-4 overflow-hidden mb-3" style={{ background: 'var(--tj-bg-secondary)' }}>
                        <img
                            src={mockProduct.images[selectedImage]?.url || mockProduct.thumbnailUrl}
                            alt={mockProduct.name}
                            className="w-100"
                            style={{ height: 480, objectFit: 'cover' }}
                        />
                        {discount && <div className="discount-badge">-{discount}%</div>}
                    </div>
                    <div className="d-flex gap-2">
                        {mockProduct.images.map((img, idx) => (
                            <button
                                key={img.id}
                                onClick={() => setSelectedImage(idx)}
                                className="rounded-3 overflow-hidden p-0 border-0"
                                style={{
                                    width: 80, height: 80, cursor: 'pointer',
                                    border: selectedImage === idx ? '2px solid var(--tj-accent)' : '2px solid var(--tj-border)',
                                    opacity: selectedImage === idx ? 1 : 0.7,
                                }}
                            >
                                <img src={img.url} alt={img.alt} className="w-100 h-100" style={{ objectFit: 'cover' }} />
                            </button>
                        ))}
                    </div>
                </Col>

                {/* Details */}
                <Col lg={6}>
                    <Badge bg="" className="mb-3 px-3 py-2" style={{ background: 'var(--tj-accent-soft)', color: 'var(--tj-accent)', fontWeight: 600 }}>
                        {mockProduct.categoryName}
                    </Badge>

                    <h1 className="fw-bold mb-3" style={{ fontSize: '1.75rem', color: 'var(--tj-text-primary)' }}>{mockProduct.name}</h1>

                    {/* Rating */}
                    <div className="d-flex align-items-center gap-2 mb-3">
                        <div className="d-flex align-items-center gap-1">
                            {Array.from({ length: 5 }).map((_, i) => (
                                <Star key={i} size={16} fill={i < Math.floor(mockProduct.averageRating!) ? 'var(--tj-accent)' : 'none'} style={{ color: 'var(--tj-accent)' }} />
                            ))}
                        </div>
                        <span className="fw-semibold" style={{ fontSize: '0.875rem', color: 'var(--tj-text-primary)' }}>{mockProduct.averageRating}</span>
                        <span style={{ fontSize: '0.875rem', color: 'var(--tj-text-muted)' }}>({mockProduct.reviewCount} reviews)</span>
                    </div>

                    {/* Price */}
                    <div className="d-flex align-items-center gap-3 mb-4">
                        <span className="fw-bold" style={{ fontSize: '2rem', color: 'var(--tj-accent)' }}>{formatPrice(mockProduct.price)}</span>
                        {mockProduct.compareAtPrice && (
                            <span className="price-original" style={{ fontSize: '1.25rem' }}>{formatPrice(mockProduct.compareAtPrice)}</span>
                        )}
                    </div>

                    <p className="mb-4" style={{ color: 'var(--tj-text-secondary)', lineHeight: 1.7, fontSize: '0.95rem' }}>{mockProduct.description}</p>

                    {/* Quantity + Add to Cart */}
                    <div className="d-flex align-items-center gap-3 mb-4">
                        <QuantitySelector value={quantity} max={mockProduct.stock} onChange={setQuantity} />
                        <Button variant="primary" size="lg" className="flex-grow-1 d-flex align-items-center justify-content-center gap-2">
                            <ShoppingCart size={20} /> Add to Cart
                        </Button>
                        <Button variant="outline-secondary" size="lg" className="px-3">
                            <Heart size={20} />
                        </Button>
                    </div>

                    {/* Trust badges */}
                    <div className="d-flex flex-column gap-3 p-3 rounded-3" style={{ background: 'var(--tj-bg-secondary)' }}>
                        {[
                            { icon: Truck, text: 'Free shipping on orders over EGP 500' },
                            { icon: Shield, text: 'Secure checkout & payment' },
                            { icon: RefreshCw, text: '30-day return policy' },
                        ].map((item, i) => (
                            <div key={i} className="d-flex align-items-center gap-2">
                                <item.icon size={16} style={{ color: 'var(--tj-accent)' }} />
                                <span style={{ fontSize: '0.825rem', color: 'var(--tj-text-secondary)' }}>{item.text}</span>
                            </div>
                        ))}
                    </div>

                    {/* Specifications */}
                    {mockProduct.specifications && (
                        <div className="mt-4">
                            <h6 className="fw-bold mb-3" style={{ color: 'var(--tj-text-primary)' }}>Specifications</h6>
                            <div className="rounded-3 overflow-hidden" style={{ border: '1px solid var(--tj-border)' }}>
                                {Object.entries(mockProduct.specifications).map(([key, value], i) => (
                                    <div key={key} className="d-flex justify-content-between px-3 py-2" style={{ background: i % 2 === 0 ? 'var(--tj-card)' : 'var(--tj-bg-secondary)', borderBottom: '1px solid var(--tj-border)' }}>
                                        <span className="fw-semibold" style={{ fontSize: '0.85rem', color: 'var(--tj-text-secondary)' }}>{key}</span>
                                        <span style={{ fontSize: '0.85rem', color: 'var(--tj-text-primary)' }}>{value}</span>
                                    </div>
                                ))}
                            </div>
                        </div>
                    )}
                </Col>
            </Row>
        </Container>
    );
}
