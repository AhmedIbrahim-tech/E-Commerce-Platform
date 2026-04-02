'use client';

import { Suspense } from 'react';
import { useSearchParams, useRouter } from 'next/navigation';
import { Container, Row, Col, Form, Badge, Button } from 'react-bootstrap';
import { PageHeader } from '@/components/shared/PageHeader';
import { ProductCard } from '@/components/shared/ProductCard';
import { EmptyState } from '@/components/shared/EmptyState';
import { Search, SlidersHorizontal, X } from 'lucide-react';
import { useState, useMemo, useEffect } from 'react';
import { SORT_OPTIONS } from '@/constants/sortOptions';
import { formatPrice } from '@/utils/formatters';

// ── Mock searchable products ────────────────────────────────────
const ALL_PRODUCTS = [
    { id: 1, name: 'Premium Wireless Headphones', slug: 'premium-wireless-headphones', price: 1299.99, compareAtPrice: 1799.99, thumbnailUrl: 'https://images.unsplash.com/photo-1505740420928-5e560c06d30e?q=80&w=400', averageRating: 4.8, reviewCount: 124, stock: 15, isFeatured: true, isActive: true, categoryId: 1, categoryName: 'Electronics', brand: 'SoundMax', createdAt: '' },
    { id: 2, name: 'Leather Crossbody Bag', slug: 'leather-crossbody-bag', price: 849.99, compareAtPrice: null, thumbnailUrl: 'https://images.unsplash.com/photo-1548036328-c9fa89d128fa?q=80&w=400', averageRating: 4.6, reviewCount: 89, stock: 8, isFeatured: false, isActive: true, categoryId: 2, categoryName: 'Fashion', brand: 'Elegance', createdAt: '' },
    { id: 3, name: 'Smart Fitness Watch Pro', slug: 'smart-fitness-watch-pro', price: 2499.99, compareAtPrice: 2999.99, thumbnailUrl: 'https://images.unsplash.com/photo-1523275335684-37898b6baf30?q=80&w=400', averageRating: 4.9, reviewCount: 256, stock: 22, isFeatured: true, isActive: true, categoryId: 1, categoryName: 'Electronics', brand: 'TechPro', createdAt: '' },
    { id: 4, name: 'Organic Skincare Set', slug: 'organic-skincare-set', price: 599.99, compareAtPrice: 749.99, thumbnailUrl: 'https://images.unsplash.com/photo-1596462502278-27bfdc403348?q=80&w=400', averageRating: 4.7, reviewCount: 67, stock: 30, isFeatured: false, isActive: true, categoryId: 3, categoryName: 'Beauty', brand: 'NaturGlow', createdAt: '' },
    { id: 5, name: 'Running Shoes Ultra', slug: 'running-shoes-ultra', price: 1099.99, compareAtPrice: null, thumbnailUrl: 'https://images.unsplash.com/photo-1491553895911-0055eca6402d?q=80&w=400', averageRating: 4.5, reviewCount: 42, stock: 10, isFeatured: false, isActive: true, categoryId: 4, categoryName: 'Sports', brand: 'AthletX', createdAt: '' },
    { id: 6, name: 'Minimalist Desk Lamp', slug: 'minimalist-desk-lamp', price: 399.99, compareAtPrice: 499.99, thumbnailUrl: 'https://images.unsplash.com/photo-1507473885765-e6ed057ab6fe?q=80&w=400', averageRating: 4.3, reviewCount: 31, stock: 0, isFeatured: false, isActive: true, categoryId: 5, categoryName: 'Home', brand: 'LuxLight', createdAt: '' },
    { id: 7, name: 'Bluetooth Portable Speaker', slug: 'bluetooth-portable-speaker', price: 699.99, compareAtPrice: 899.99, thumbnailUrl: 'https://images.unsplash.com/photo-1608043152269-423dbba4e7e1?q=80&w=400', averageRating: 4.4, reviewCount: 98, stock: 18, isFeatured: false, isActive: true, categoryId: 1, categoryName: 'Electronics', brand: 'SoundMax', createdAt: '' },
    { id: 8, name: 'Cotton Casual T-Shirt', slug: 'cotton-casual-tshirt', price: 199.99, compareAtPrice: null, thumbnailUrl: 'https://images.unsplash.com/photo-1521572163474-6864f9cf17ab?q=80&w=400', averageRating: 4.2, reviewCount: 53, stock: 45, isFeatured: false, isActive: true, categoryId: 2, categoryName: 'Fashion', brand: 'UrbanFit', createdAt: '' },
    { id: 9, name: 'Professional Yoga Mat', slug: 'professional-yoga-mat', price: 349.99, compareAtPrice: null, thumbnailUrl: 'https://images.unsplash.com/photo-1601925260368-ae2f83cf8b7f?q=80&w=400', averageRating: 4.6, reviewCount: 73, stock: 25, isFeatured: false, isActive: true, categoryId: 4, categoryName: 'Sports', brand: 'ZenFlow', createdAt: '' },
    { id: 10, name: 'Luxury Perfume Collection', slug: 'luxury-perfume-collection', price: 1899.99, compareAtPrice: 2199.99, thumbnailUrl: 'https://images.unsplash.com/photo-1541643600914-78b084683601?q=80&w=400', averageRating: 4.8, reviewCount: 112, stock: 12, isFeatured: true, isActive: true, categoryId: 3, categoryName: 'Beauty', brand: 'NaturGlow', createdAt: '' },
    { id: 11, name: 'Ceramic Coffee Mug Set', slug: 'ceramic-coffee-mug-set', price: 249.99, compareAtPrice: null, thumbnailUrl: 'https://images.unsplash.com/photo-1514228742587-6b1558fcca3d?q=80&w=400', averageRating: 4.1, reviewCount: 28, stock: 40, isFeatured: false, isActive: true, categoryId: 5, categoryName: 'Home', brand: 'CozyHome', createdAt: '' },
    { id: 12, name: 'Noise Cancelling Earbuds', slug: 'noise-cancelling-earbuds', price: 899.99, compareAtPrice: 1199.99, thumbnailUrl: 'https://images.unsplash.com/photo-1590658268037-6bf12f032f55?q=80&w=400', averageRating: 4.7, reviewCount: 184, stock: 0, isFeatured: true, isActive: true, categoryId: 1, categoryName: 'Electronics', brand: 'TechPro', createdAt: '' },
];

// ── Inner component (reads searchParams) ────────────────────────
function SearchResults() {
    const searchParams = useSearchParams();
    const router = useRouter();
    const queryParam = searchParams.get('q') || '';

    const [query, setQuery] = useState(queryParam);
    const [sortBy, setSortBy] = useState('rating');
    const [selectedCategory, setSelectedCategory] = useState<string | null>(null);

    // Sync from URL param
    useEffect(() => {
        setQuery(queryParam);
    }, [queryParam]);

    // Search + filter
    const { results, categories } = useMemo(() => {
        if (!query.trim()) return { results: [], categories: [] };

        const q = query.toLowerCase();
        let matched = ALL_PRODUCTS.filter(
            (p) =>
                p.name.toLowerCase().includes(q) ||
                p.categoryName.toLowerCase().includes(q) ||
                p.brand.toLowerCase().includes(q)
        );

        // Extract unique categories from results
        const cats = [...new Set(matched.map((p) => p.categoryName))].map((cat) => ({
            name: cat,
            count: matched.filter((p) => p.categoryName === cat).length,
        }));

        // Category filter
        if (selectedCategory) {
            matched = matched.filter((p) => p.categoryName === selectedCategory);
        }

        // Sort
        switch (sortBy) {
            case 'price_asc': matched.sort((a, b) => a.price - b.price); break;
            case 'price_desc': matched.sort((a, b) => b.price - a.price); break;
            case 'rating': matched.sort((a, b) => (b.averageRating ?? 0) - (a.averageRating ?? 0)); break;
            case 'popular': matched.sort((a, b) => (b.reviewCount ?? 0) - (a.reviewCount ?? 0)); break;
            case 'name_asc': matched.sort((a, b) => a.name.localeCompare(b.name)); break;
            default: break;
        }

        return { results: matched, categories: cats };
    }, [query, sortBy, selectedCategory]);

    // Submit new search from the page input
    const handleSubmit = (e: React.FormEvent) => {
        e.preventDefault();
        if (query.trim()) {
            router.push(`/search?q=${encodeURIComponent(query.trim())}`);
        }
    };

    return (
        <Container className="py-4">
            <PageHeader
                title="Search"
                subtitle={queryParam ? `Results for "${queryParam}"` : 'Find what you need'}
                breadcrumbs={[{ label: 'Search' }]}
            />

            {/* ── Search Input ─────────────────────────────────── */}
            <form onSubmit={handleSubmit} className="mb-4">
                <div className="position-relative">
                    <div
                        className="position-absolute d-flex align-items-center justify-content-center"
                        style={{ left: 16, top: '50%', transform: 'translateY(-50%)', color: 'var(--tj-text-light)', pointerEvents: 'none' }}
                    >
                        <Search size={20} />
                    </div>
                    <Form.Control
                        type="text"
                        size="lg"
                        placeholder="Search for products, brands, categories..."
                        value={query}
                        onChange={(e) => setQuery(e.target.value)}
                        autoFocus
                        id="search-page-input"
                        className="rounded-3"
                        style={{
                            paddingLeft: 50,
                            paddingRight: query ? 48 : 16,
                            height: 56,
                            fontSize: '1rem',
                            background: 'var(--tj-card)',
                            border: '2px solid var(--tj-border)',
                        }}
                    />
                    {query && (
                        <button
                            type="button"
                            className="btn btn-link position-absolute p-0 d-flex align-items-center justify-content-center"
                            style={{ right: 16, top: '50%', transform: 'translateY(-50%)', color: 'var(--tj-text-muted)' }}
                            onClick={() => { setQuery(''); router.push('/search'); }}
                            aria-label="Clear search"
                        >
                            <X size={18} />
                        </button>
                    )}
                </div>
            </form>

            {/* ── No query state ───────────────────────────────── */}
            {!queryParam && (
                <div className="text-center py-5">
                    <div className="d-flex justify-content-center mb-3">
                        <div className="rounded-circle d-flex align-items-center justify-content-center" style={{ width: 80, height: 80, background: 'var(--tj-accent-soft)' }}>
                            <Search size={32} style={{ color: 'var(--tj-accent)' }} />
                        </div>
                    </div>
                    <h5 className="fw-bold mb-2" style={{ color: 'var(--tj-text-primary)' }}>What are you looking for?</h5>
                    <p className="mb-4" style={{ color: 'var(--tj-text-muted)', maxWidth: 400, margin: '0 auto' }}>
                        Search by product name, brand, or category to find exactly what you need.
                    </p>
                    {/* Popular searches */}
                    <div className="d-flex gap-2 justify-content-center flex-wrap">
                        {['Headphones', 'Watch', 'Skincare', 'Shoes', 'Bag'].map((term) => (
                            <Button
                                key={term}
                                variant="outline-secondary"
                                size="sm"
                                className="rounded-pill px-3"
                                onClick={() => router.push(`/search?q=${encodeURIComponent(term)}`)}
                                style={{ fontSize: '0.8rem' }}
                            >
                                {term}
                            </Button>
                        ))}
                    </div>
                </div>
            )}

            {/* ── Results ──────────────────────────────────────── */}
            {queryParam && (
                <>
                    {/* Results bar */}
                    <div
                        className="d-flex flex-wrap align-items-center justify-content-between gap-3 mb-3 p-3 rounded-3"
                        style={{ background: 'var(--tj-card)', border: '1px solid var(--tj-border)' }}
                    >
                        <span style={{ color: 'var(--tj-text-muted)', fontSize: '0.85rem' }}>
                            <strong style={{ color: 'var(--tj-text-primary)' }}>{results.length}</strong> result{results.length !== 1 ? 's' : ''} found
                        </span>
                        <Form.Select
                            size="sm"
                            value={sortBy}
                            onChange={(e) => setSortBy(e.target.value)}
                            style={{ maxWidth: 180 }}
                            id="search-sort"
                        >
                            {SORT_OPTIONS.map((opt) => (
                                <option key={opt.value} value={opt.value}>{opt.label}</option>
                            ))}
                        </Form.Select>
                    </div>

                    {/* Category chips */}
                    {categories.length > 1 && (
                        <div className="d-flex gap-2 flex-wrap mb-4">
                            <Badge
                                bg=""
                                className="rounded-pill px-3 py-2"
                                style={{
                                    background: selectedCategory === null ? 'var(--tj-accent)' : 'var(--tj-bg-secondary)',
                                    color: selectedCategory === null ? 'var(--tj-accent-foreground)' : 'var(--tj-text-muted)',
                                    fontWeight: 600, fontSize: '0.8rem', cursor: 'pointer',
                                }}
                                onClick={() => setSelectedCategory(null)}
                            >
                                All ({categories.reduce((a, c) => a + c.count, 0)})
                            </Badge>
                            {categories.map((cat) => (
                                <Badge
                                    key={cat.name}
                                    bg=""
                                    className="rounded-pill px-3 py-2"
                                    style={{
                                        background: selectedCategory === cat.name ? 'var(--tj-accent)' : 'var(--tj-bg-secondary)',
                                        color: selectedCategory === cat.name ? 'var(--tj-accent-foreground)' : 'var(--tj-text-muted)',
                                        fontWeight: 600, fontSize: '0.8rem', cursor: 'pointer',
                                    }}
                                    onClick={() => setSelectedCategory(selectedCategory === cat.name ? null : cat.name)}
                                >
                                    {cat.name} ({cat.count})
                                </Badge>
                            ))}
                        </div>
                    )}

                    {/* Product grid or empty */}
                    {results.length === 0 ? (
                        <EmptyState
                            icon={Search}
                            title={`No results for "${queryParam}"`}
                            description="Try searching with different keywords or browse our categories."
                            actionLabel="Browse All Products"
                            actionHref="/products"
                        />
                    ) : (
                        <Row className="g-4 stagger-children">
                            {results.map((product, idx) => (
                                <Col key={product.id} lg={3} md={4} sm={6}>
                                    <ProductCard
                                        {...product}
                                        index={idx}
                                        onAddToCart={() => {}}
                                        onToggleWishlist={() => {}}
                                    />
                                </Col>
                            ))}
                        </Row>
                    )}
                </>
            )}
        </Container>
    );
}

// ── Page component (Suspense boundary for useSearchParams) ──────
export default function SearchPage() {
    return (
        <Suspense fallback={<Container className="py-5 text-center"><span style={{ color: 'var(--tj-text-muted)' }}>Loading...</span></Container>}>
            <SearchResults />
        </Suspense>
    );
}
