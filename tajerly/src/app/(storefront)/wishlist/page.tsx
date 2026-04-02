'use client';

import { Container, Row, Col } from 'react-bootstrap';
import { PageHeader } from '@/components/shared/PageHeader';
import { ProductCard } from '@/components/shared/ProductCard';
import { EmptyState } from '@/components/shared/EmptyState';
import { Heart } from 'lucide-react';
import { useAppSelector } from '@/store/hooks';
import type { WishlistItem } from '@/types/wishlist';

export default function WishlistPage() {
    const items = useAppSelector((state) => state.wishlist.items);

    if (items.length === 0) {
        return (
            <Container className="py-5">
                <EmptyState icon={Heart} title="Your wishlist is empty" description="Save your favorite products to buy later." actionLabel="Explore Products" actionHref="/products" />
            </Container>
        );
    }

    return (
        <Container className="py-4">
            <PageHeader title="My Wishlist" subtitle={`${items.length} saved item${items.length !== 1 ? 's' : ''}`} breadcrumbs={[{ label: 'Wishlist' }]} />
            <Row className="g-4">
                {items.map((item: WishlistItem, idx: number) => (
                    <Col key={item.productId} lg={3} md={4} sm={6}>
                        <ProductCard
                            id={item.productId} name={item.productName} slug={item.productSlug}
                            price={item.price} compareAtPrice={item.compareAtPrice}
                            thumbnailUrl={item.thumbnailUrl} stock={item.stock}
                            isInWishlist onAddToCart={() => {}} onToggleWishlist={() => {}} index={idx}
                        />
                    </Col>
                ))}
            </Row>
        </Container>
    );
}
