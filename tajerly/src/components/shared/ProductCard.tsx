'use client';

import Link from 'next/link';
import { Heart, ShoppingCart, Star } from 'lucide-react';
import { Card, Button, Badge } from 'react-bootstrap';
import { formatPrice, getDiscountPercent } from '@/utils/formatters';
import { getProductImage } from '@/utils/imageHelpers';
import { useTranslation } from 'react-i18next';

interface ProductCardProps {
    id: number | string;
    name: string;
    slug: string;
    price: number;
    compareAtPrice?: number | null;
    thumbnailUrl?: string;
    averageRating?: number;
    reviewCount?: number;
    stock: number;
    onAddToCart?: () => void;
    onToggleWishlist?: () => void;
    isInWishlist?: boolean;
    index?: number;
}

export function ProductCard({
    id, name, slug, price, compareAtPrice, thumbnailUrl,
    averageRating, reviewCount, stock, onAddToCart, onToggleWishlist, isInWishlist, index = 0
}: ProductCardProps) {
    const { t } = useTranslation();
    const discount = getDiscountPercent(price, compareAtPrice);
    const outOfStock = stock <= 0;

    return (
        <div className="tj-card overflow-hidden h-100 d-flex flex-column position-relative">
            {/* Discount Badge */}
            {discount && <div className="discount-badge">-{discount}%</div>}

            {/* Wishlist Button */}
            <button
                onClick={(e) => { e.preventDefault(); onToggleWishlist?.(); }}
                className="btn btn-link wishlist-btn position-absolute p-2 rounded-circle d-flex align-items-center justify-content-center transition-theme"
                style={{
                    top: 12, zIndex: 2, width: 36, height: 36,
                    background: 'var(--tj-surface)', border: '1px solid var(--tj-border)',
                    color: isInWishlist ? 'var(--tj-danger)' : 'var(--tj-text-muted)',
                }}
            >
                <Heart size={16} fill={isInWishlist ? 'currentColor' : 'none'} />
            </button>

            {/* Image */}
            <Link href={`/products/${slug}`} className="text-decoration-none overflow-hidden">
                <img
                    src={getProductImage(thumbnailUrl, index)}
                    alt={name}
                    className="product-card-img w-100"
                    style={{ opacity: outOfStock ? 0.5 : 1 }}
                />
                {outOfStock && (
                    <div className="position-absolute top-50 start-50 translate-middle w-100 px-4 text-center">
                        <Badge bg="" className="px-3 py-2 fs-6 shadow-sm" style={{ background: 'var(--tj-surface)', color: 'var(--tj-danger)', border: '1px solid var(--tj-border)', fontWeight: 700 }}>
                            {t('common.outOfStock')}
                        </Badge>
                    </div>
                )}
            </Link>

            {/* Details */}
            <div className="p-3 d-flex flex-column flex-grow-1">
                <Link href={`/products/${slug}`} className="text-decoration-none">
                    <h6 className="fw-semibold mb-2" style={{ color: 'var(--tj-text-primary)', fontSize: '0.9rem', lineHeight: 1.4, display: '-webkit-box', WebkitLineClamp: 2, WebkitBoxOrient: 'vertical', overflow: 'hidden' }}>
                        {name}
                    </h6>
                </Link>

                {/* Rating */}
                {averageRating != null && (
                    <div className="d-flex align-items-center gap-1 mb-2">
                        <Star size={14} fill="var(--tj-accent)" style={{ color: 'var(--tj-accent)' }} />
                        <span style={{ fontSize: '0.8rem', fontWeight: 600, color: 'var(--tj-text-primary)' }}>{averageRating.toFixed(1)}</span>
                        {reviewCount != null && (
                            <span style={{ fontSize: '0.75rem', color: 'var(--tj-text-muted)' }}>({reviewCount})</span>
                        )}
                    </div>
                )}

                {/* Price */}
                <div className="d-flex align-items-center gap-2 mt-auto mb-3">
                    <span className="price-current">{formatPrice(price)}</span>
                    {compareAtPrice && compareAtPrice > price && (
                        <span className="price-original">{formatPrice(compareAtPrice)}</span>
                    )}
                </div>

                {/* Add to Cart */}
                <Button
                    variant="primary"
                    size="sm"
                    className="w-100 d-flex align-items-center justify-content-center gap-2"
                    onClick={onAddToCart}
                    disabled={outOfStock}
                >
                    <ShoppingCart size={16} />
                    {outOfStock ? t('common.outOfStock') : t('common.addToCart')}
                </Button>
            </div>
        </div>
    );
}
