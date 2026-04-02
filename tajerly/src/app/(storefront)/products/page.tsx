'use client';

import { Container, Row, Col, Form, Button, Badge, Offcanvas, Accordion } from 'react-bootstrap';
import { PageHeader } from '@/components/shared/PageHeader';
import { ProductCard } from '@/components/shared/ProductCard';
import { SORT_OPTIONS } from '@/constants/sortOptions';
import { useState, useMemo, useCallback } from 'react';
import { SlidersHorizontal, X, Star, RotateCcw, Grid3x3, List, ChevronDown, ChevronLeft, ChevronRight } from 'lucide-react';
import { useTranslation } from 'react-i18next';
import { formatPrice } from '@/utils/formatters';

// ── Mock Data ───────────────────────────────────────────────────────
const mockProducts = [
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

// ── Derive filter options from data ─────────────────────────────────
const ALL_CATEGORIES = [...new Set(mockProducts.map((p) => p.categoryName))].sort();
const ALL_BRANDS = [...new Set(mockProducts.map((p) => p.brand))].sort();
const PRICE_MIN = 0;
const PRICE_MAX = 3500;
const PRICE_STEP = 50;

// ── Filter state type ───────────────────────────────────────────────
interface FilterState {
    categories: string[];
    brands: string[];
    priceMin: number;
    priceMax: number;
    minRating: number;
    inStockOnly: boolean;
    onSaleOnly: boolean;
}

const initialFilters: FilterState = {
    categories: [],
    brands: [],
    priceMin: PRICE_MIN,
    priceMax: PRICE_MAX,
    minRating: 0,
    inStockOnly: false,
    onSaleOnly: false,
};

// ── Helper: count active filters ────────────────────────────────────
function countActiveFilters(f: FilterState): number {
    let count = 0;
    if (f.categories.length > 0) count++;
    if (f.brands.length > 0) count++;
    if (f.priceMin > PRICE_MIN || f.priceMax < PRICE_MAX) count++;
    if (f.minRating > 0) count++;
    if (f.inStockOnly) count++;
    if (f.onSaleOnly) count++;
    return count;
}

// ═════════════════════════════════════════════════════════════════════
// COMPONENT
// ═════════════════════════════════════════════════════════════════════
export default function ProductsPage() {
    const { t, i18n } = useTranslation();
    const isRtl = i18n.language === 'ar';

    const [sortBy, setSortBy] = useState('newest');
    const [filters, setFilters] = useState<FilterState>(initialFilters);
    const [showMobileFilter, setShowMobileFilter] = useState(false);
    const [viewMode, setViewMode] = useState<'grid' | 'list'>('grid');

    // ── Toggle helpers ──────────────────────────────────────────────
    const toggleArrayFilter = useCallback(
        (key: 'categories' | 'brands', value: string) => {
            setFilters((prev) => {
                const arr = prev[key];
                return {
                    ...prev,
                    [key]: arr.includes(value)
                        ? arr.filter((v) => v !== value)
                        : [...arr, value],
                };
            });
        },
        []
    );

    const clearFilters = useCallback(() => setFilters(initialFilters), []);

    // ── Apply filters + sort (client‑side on mock) ──────────────────
    const filteredProducts = useMemo(() => {
        let result = [...mockProducts];

        // Category
        if (filters.categories.length > 0) {
            result = result.filter((p) => filters.categories.includes(p.categoryName));
        }
        // Brand
        if (filters.brands.length > 0) {
            result = result.filter((p) => filters.brands.includes(p.brand));
        }
        // Price range
        result = result.filter((p) => p.price >= filters.priceMin && p.price <= filters.priceMax);
        // Rating
        if (filters.minRating > 0) {
            result = result.filter((p) => (p.averageRating ?? 0) >= filters.minRating);
        }
        // In stock
        if (filters.inStockOnly) {
            result = result.filter((p) => p.stock > 0);
        }
        // On sale
        if (filters.onSaleOnly) {
            result = result.filter((p) => p.compareAtPrice != null && p.compareAtPrice > p.price);
        }
        // Sort
        switch (sortBy) {
            case 'price_asc': result.sort((a, b) => a.price - b.price); break;
            case 'price_desc': result.sort((a, b) => b.price - a.price); break;
            case 'rating': result.sort((a, b) => (b.averageRating ?? 0) - (a.averageRating ?? 0)); break;
            case 'name_asc': result.sort((a, b) => a.name.localeCompare(b.name)); break;
            case 'name_desc': result.sort((a, b) => b.name.localeCompare(a.name)); break;
            case 'popular': result.sort((a, b) => (b.reviewCount ?? 0) - (a.reviewCount ?? 0)); break;
            default: break; // newest — keep original order
        }

        return result;
    }, [filters, sortBy]);

    const activeCount = countActiveFilters(filters);

    // ── Filter Sidebar Content ──────────────────────────────────────
    const filterContent = (
        <div className="d-flex flex-column gap-4">
            {/* Header */}
            <div className="d-flex align-items-center justify-content-between">
                <h6 className="fw-bold mb-0 d-flex align-items-center gap-2" style={{ color: 'var(--tj-text-primary)' }}>
                    <SlidersHorizontal size={18} style={{ color: 'var(--tj-accent)' }} />
                    Filters
                    {activeCount > 0 && (
                        <Badge pill bg="" style={{ background: 'var(--tj-accent)', color: 'var(--tj-accent-foreground)', fontSize: '0.65rem' }}>
                            {activeCount}
                        </Badge>
                    )}
                </h6>
                {activeCount > 0 && (
                    <Button
                        variant="link"
                        size="sm"
                        className="p-0 d-flex align-items-center gap-1 text-decoration-none"
                        style={{ color: 'var(--tj-danger)', fontSize: '0.8rem', fontWeight: 600 }}
                        onClick={clearFilters}
                        id="btn-clear-filters"
                    >
                        <RotateCcw size={14} /> Clear All
                    </Button>
                )}
            </div>

            {/* ── Category Filter ──────────────────────────────────── */}
            <Accordion defaultActiveKey={['0', '1', '2']} alwaysOpen flush>
                <Accordion.Item eventKey="0" style={{ background: 'transparent', border: 'none' }}>
                    <Accordion.Header
                        className="filter-accordion-header"
                        style={{ padding: 0 }}
                    >
                        <span className="fw-bold" style={{ fontSize: '0.825rem', color: 'var(--tj-text-primary)' }}>
                            Category
                        </span>
                    </Accordion.Header>
                    <Accordion.Body className="px-0 pt-2 pb-0">
                        <div className="d-flex flex-column gap-2">
                            {ALL_CATEGORIES.map((cat) => {
                                const count = mockProducts.filter((p) => p.categoryName === cat).length;
                                const active = filters.categories.includes(cat);
                                return (
                                    <label
                                        key={cat}
                                        className="d-flex align-items-center justify-content-between px-2 py-1 rounded-2"
                                        style={{
                                            cursor: 'pointer',
                                            background: active ? 'var(--tj-accent-soft)' : 'transparent',
                                            transition: 'background 0.15s',
                                        }}
                                    >
                                        <div className="d-flex align-items-center gap-2">
                                            <Form.Check
                                                type="checkbox"
                                                checked={active}
                                                onChange={() => toggleArrayFilter('categories', cat)}
                                                id={`filter-cat-${cat}`}
                                            />
                                            <span style={{ fontSize: '0.85rem', fontWeight: active ? 600 : 400, color: active ? 'var(--tj-accent)' : 'var(--tj-text-secondary)' }}>
                                                {cat}
                                            </span>
                                        </div>
                                        <span style={{ fontSize: '0.75rem', color: 'var(--tj-text-light)', fontWeight: 600 }}>
                                            {count}
                                        </span>
                                    </label>
                                );
                            })}
                        </div>
                    </Accordion.Body>
                </Accordion.Item>

                {/* ── Price Range ──────────────────────────────────── */}
                <Accordion.Item eventKey="1" style={{ background: 'transparent', border: 'none', borderTop: '1px solid var(--tj-border)' }}>
                    <Accordion.Header className="filter-accordion-header" style={{ padding: 0 }}>
                        <span className="fw-bold" style={{ fontSize: '0.825rem', color: 'var(--tj-text-primary)' }}>
                            Price Range
                        </span>
                    </Accordion.Header>
                    <Accordion.Body className="px-0 pt-2 pb-0">
                        <div className="d-flex align-items-center justify-content-between mb-2">
                            <span className="fw-semibold" style={{ fontSize: '0.8rem', color: 'var(--tj-accent)' }}>
                                {formatPrice(filters.priceMin)}
                            </span>
                            <span style={{ fontSize: '0.75rem', color: 'var(--tj-text-light)' }}>—</span>
                            <span className="fw-semibold" style={{ fontSize: '0.8rem', color: 'var(--tj-accent)' }}>
                                {formatPrice(filters.priceMax)}
                            </span>
                        </div>
                        <div className="mb-2">
                            <Form.Label style={{ fontSize: '0.75rem', color: 'var(--tj-text-muted)' }}>Min Price</Form.Label>
                            <Form.Range
                                min={PRICE_MIN}
                                max={PRICE_MAX}
                                step={PRICE_STEP}
                                value={filters.priceMin}
                                onChange={(e) => setFilters((prev) => ({ ...prev, priceMin: Math.min(Number(e.target.value), prev.priceMax - PRICE_STEP) }))}
                                id="filter-price-min"
                                style={{ accentColor: 'var(--tj-accent)' }}
                            />
                        </div>
                        <div>
                            <Form.Label style={{ fontSize: '0.75rem', color: 'var(--tj-text-muted)' }}>Max Price</Form.Label>
                            <Form.Range
                                min={PRICE_MIN}
                                max={PRICE_MAX}
                                step={PRICE_STEP}
                                value={filters.priceMax}
                                onChange={(e) => setFilters((prev) => ({ ...prev, priceMax: Math.max(Number(e.target.value), prev.priceMin + PRICE_STEP) }))}
                                id="filter-price-max"
                                style={{ accentColor: 'var(--tj-accent)' }}
                            />
                        </div>
                        {/* Quick presets */}
                        <div className="d-flex gap-2 flex-wrap mt-2">
                            {[
                                { label: 'Under 500', min: 0, max: 500 },
                                { label: '500 – 1000', min: 500, max: 1000 },
                                { label: '1000 – 2000', min: 1000, max: 2000 },
                                { label: '2000+', min: 2000, max: PRICE_MAX },
                            ].map((preset) => {
                                const isActive = filters.priceMin === preset.min && filters.priceMax === preset.max;
                                return (
                                    <button
                                        key={preset.label}
                                        onClick={() => setFilters((prev) => ({ ...prev, priceMin: preset.min, priceMax: preset.max }))}
                                        className="border-0 rounded-pill px-3 py-1"
                                        style={{
                                            fontSize: '0.7rem',
                                            fontWeight: 600,
                                            cursor: 'pointer',
                                            background: isActive ? 'var(--tj-accent)' : 'var(--tj-bg-secondary)',
                                            color: isActive ? 'var(--tj-accent-foreground)' : 'var(--tj-text-muted)',
                                            transition: 'all 0.15s',
                                        }}
                                    >
                                        {preset.label}
                                    </button>
                                );
                            })}
                        </div>
                    </Accordion.Body>
                </Accordion.Item>

                {/* ── Brand Filter ─────────────────────────────────── */}
                <Accordion.Item eventKey="2" style={{ background: 'transparent', border: 'none', borderTop: '1px solid var(--tj-border)' }}>
                    <Accordion.Header className="filter-accordion-header" style={{ padding: 0 }}>
                        <span className="fw-bold" style={{ fontSize: '0.825rem', color: 'var(--tj-text-primary)' }}>
                            Brand
                        </span>
                    </Accordion.Header>
                    <Accordion.Body className="px-0 pt-2 pb-0">
                        <div className="d-flex flex-column gap-2">
                            {ALL_BRANDS.map((brand) => {
                                const count = mockProducts.filter((p) => p.brand === brand).length;
                                const active = filters.brands.includes(brand);
                                return (
                                    <label
                                        key={brand}
                                        className="d-flex align-items-center justify-content-between px-2 py-1 rounded-2"
                                        style={{
                                            cursor: 'pointer',
                                            background: active ? 'var(--tj-accent-soft)' : 'transparent',
                                            transition: 'background 0.15s',
                                        }}
                                    >
                                        <div className="d-flex align-items-center gap-2">
                                            <Form.Check
                                                type="checkbox"
                                                checked={active}
                                                onChange={() => toggleArrayFilter('brands', brand)}
                                                id={`filter-brand-${brand}`}
                                            />
                                            <span style={{ fontSize: '0.85rem', fontWeight: active ? 600 : 400, color: active ? 'var(--tj-accent)' : 'var(--tj-text-secondary)' }}>
                                                {brand}
                                            </span>
                                        </div>
                                        <span style={{ fontSize: '0.75rem', color: 'var(--tj-text-light)', fontWeight: 600 }}>
                                            {count}
                                        </span>
                                    </label>
                                );
                            })}
                        </div>
                    </Accordion.Body>
                </Accordion.Item>
            </Accordion>

            {/* ── Rating Filter ────────────────────────────────────── */}
            <div style={{ borderTop: '1px solid var(--tj-border)', paddingTop: 16 }}>
                <h6 className="fw-bold mb-3" style={{ fontSize: '0.825rem', color: 'var(--tj-text-primary)' }}>
                    Minimum Rating
                </h6>
                <div className="d-flex flex-column gap-1">
                    {[4, 3, 2, 1].map((rating) => {
                        const active = filters.minRating === rating;
                        return (
                            <button
                                key={rating}
                                onClick={() => setFilters((prev) => ({ ...prev, minRating: prev.minRating === rating ? 0 : rating }))}
                                className="d-flex align-items-center gap-2 border-0 px-2 py-2 rounded-2"
                                style={{
                                    background: active ? 'var(--tj-accent-soft)' : 'transparent',
                                    cursor: 'pointer',
                                    transition: 'background 0.15s',
                                }}
                            >
                                <div className="d-flex align-items-center gap-1">
                                    {Array.from({ length: 5 }).map((_, i) => (
                                        <Star
                                            key={i}
                                            size={14}
                                            fill={i < rating ? 'var(--tj-accent)' : 'none'}
                                            style={{ color: i < rating ? 'var(--tj-accent)' : 'var(--tj-text-light)' }}
                                        />
                                    ))}
                                </div>
                                <span style={{ fontSize: '0.8rem', color: active ? 'var(--tj-accent)' : 'var(--tj-text-muted)', fontWeight: active ? 600 : 400 }}>
                                    & Up
                                </span>
                            </button>
                        );
                    })}
                </div>
            </div>

            {/* ── Availability / On Sale ───────────────────────────── */}
            <div style={{ borderTop: '1px solid var(--tj-border)', paddingTop: 16 }}>
                <h6 className="fw-bold mb-3" style={{ fontSize: '0.825rem', color: 'var(--tj-text-primary)' }}>
                    Availability
                </h6>
                <div className="d-flex flex-column gap-2">
                    <Form.Check
                        type="switch"
                        label="In Stock Only"
                        checked={filters.inStockOnly}
                        onChange={(e) => setFilters((prev) => ({ ...prev, inStockOnly: e.target.checked }))}
                        id="filter-in-stock"
                        style={{ fontSize: '0.85rem', color: 'var(--tj-text-secondary)' }}
                    />
                    <Form.Check
                        type="switch"
                        label="On Sale Only"
                        checked={filters.onSaleOnly}
                        onChange={(e) => setFilters((prev) => ({ ...prev, onSaleOnly: e.target.checked }))}
                        id="filter-on-sale"
                        style={{ fontSize: '0.85rem', color: 'var(--tj-text-secondary)' }}
                    />
                </div>
            </div>
        </div>
    );

    return (
        <Container fluid className="py-4 px-lg-5">
            <div className="px-lg-3">
                <PageHeader
                    title="All Products"
                    subtitle="Browse our complete collection"
                    breadcrumbs={[{ label: 'Shop', href: '/products' }]}
                />
            </div>

            <Row className="g-4 px-lg-3">
                {/* ── Desktop Filter Sidebar ──────────────────────── */}
                <Col lg={3} className="d-none d-lg-block">
                    <div
                        className="tj-card p-4 position-sticky"
                        style={{ top: 100, maxHeight: 'calc(100vh - 120px)', overflowY: 'auto' }}
                        id="desktop-filter-sidebar"
                    >
                        {filterContent}
                    </div>
                </Col>

                {/* ── Products Grid ───────────────────────────────── */}
                <Col lg={9}>
                    {/* Top bar: results count + sort + mobile filter toggle + view mode */}
                    <div
                        className="d-flex flex-wrap align-items-center justify-content-between gap-3 mb-4 p-3 rounded-3"
                        style={{ background: 'var(--tj-card)', border: '1px solid var(--tj-border)' }}
                    >
                        <div className="d-flex align-items-center gap-3">
                            {/* Mobile filter button */}
                            <Button
                                variant="outline-primary"
                                size="sm"
                                className="d-lg-none d-flex align-items-center gap-2"
                                onClick={() => setShowMobileFilter(true)}
                                id="btn-mobile-filter"
                            >
                                <SlidersHorizontal size={16} />
                                Filters
                                {activeCount > 0 && (
                                    <Badge pill bg="" style={{ background: 'var(--tj-accent)', color: 'var(--tj-accent-foreground)', fontSize: '0.6rem' }}>
                                        {activeCount}
                                    </Badge>
                                )}
                            </Button>

                            <span style={{ color: 'var(--tj-text-muted)', fontSize: '0.85rem' }}>
                                <strong style={{ color: 'var(--tj-text-primary)' }}>{filteredProducts.length}</strong> product{filteredProducts.length !== 1 ? 's' : ''} found
                            </span>
                        </div>

                        <div className="d-flex align-items-center gap-3">
                            {/* View mode toggle */}
                            <div className="d-none d-md-flex align-items-center gap-1 border rounded-2 p-1" style={{ borderColor: 'var(--tj-border)' }}>
                                <button
                                    onClick={() => setViewMode('grid')}
                                    className="border-0 rounded-2 p-1 d-flex align-items-center justify-content-center"
                                    style={{
                                        width: 32, height: 32, cursor: 'pointer',
                                        background: viewMode === 'grid' ? 'var(--tj-accent)' : 'transparent',
                                        color: viewMode === 'grid' ? 'var(--tj-accent-foreground)' : 'var(--tj-text-muted)',
                                        transition: 'all 0.15s',
                                    }}
                                    aria-label="Grid view"
                                >
                                    <Grid3x3 size={16} />
                                </button>
                                <button
                                    onClick={() => setViewMode('list')}
                                    className="border-0 rounded-2 p-1 d-flex align-items-center justify-content-center"
                                    style={{
                                        width: 32, height: 32, cursor: 'pointer',
                                        background: viewMode === 'list' ? 'var(--tj-accent)' : 'transparent',
                                        color: viewMode === 'list' ? 'var(--tj-accent-foreground)' : 'var(--tj-text-muted)',
                                        transition: 'all 0.15s',
                                    }}
                                    aria-label="List view"
                                >
                                    <List size={16} />
                                </button>
                            </div>

                            {/* Sort */}
                            <Form.Select
                                size="sm"
                                value={sortBy}
                                onChange={(e) => setSortBy(e.target.value)}
                                style={{ maxWidth: 200 }}
                                id="sort-select"
                            >
                                {SORT_OPTIONS.map((opt) => (
                                    <option key={opt.value} value={opt.value}>{opt.label}</option>
                                ))}
                            </Form.Select>
                        </div>
                    </div>

                    {/* Active filter badges */}
                    {activeCount > 0 && (
                        <div className="d-flex flex-wrap gap-2 mb-3 animate-fade-in">
                            {filters.categories.map((cat) => (
                                <Badge
                                    key={`cat-${cat}`}
                                    bg=""
                                    className="d-flex align-items-center gap-1 px-3 py-2 rounded-pill"
                                    style={{ background: 'var(--tj-accent-soft)', color: 'var(--tj-accent)', fontWeight: 600, fontSize: '0.75rem', cursor: 'pointer' }}
                                    onClick={() => toggleArrayFilter('categories', cat)}
                                >
                                    {cat} <X size={12} />
                                </Badge>
                            ))}
                            {filters.brands.map((brand) => (
                                <Badge
                                    key={`brand-${brand}`}
                                    bg=""
                                    className="d-flex align-items-center gap-1 px-3 py-2 rounded-pill"
                                    style={{ background: 'var(--tj-accent-soft)', color: 'var(--tj-accent)', fontWeight: 600, fontSize: '0.75rem', cursor: 'pointer' }}
                                    onClick={() => toggleArrayFilter('brands', brand)}
                                >
                                    {brand} <X size={12} />
                                </Badge>
                            ))}
                            {(filters.priceMin > PRICE_MIN || filters.priceMax < PRICE_MAX) && (
                                <Badge
                                    bg=""
                                    className="d-flex align-items-center gap-1 px-3 py-2 rounded-pill"
                                    style={{ background: 'var(--tj-info-soft)', color: 'var(--tj-info)', fontWeight: 600, fontSize: '0.75rem', cursor: 'pointer' }}
                                    onClick={() => setFilters((prev) => ({ ...prev, priceMin: PRICE_MIN, priceMax: PRICE_MAX }))}
                                >
                                    {formatPrice(filters.priceMin)} – {formatPrice(filters.priceMax)} <X size={12} />
                                </Badge>
                            )}
                            {filters.minRating > 0 && (
                                <Badge
                                    bg=""
                                    className="d-flex align-items-center gap-1 px-3 py-2 rounded-pill"
                                    style={{ background: 'var(--tj-warning-soft)', color: 'var(--tj-warning)', fontWeight: 600, fontSize: '0.75rem', cursor: 'pointer' }}
                                    onClick={() => setFilters((prev) => ({ ...prev, minRating: 0 }))}
                                >
                                    {filters.minRating}+ Stars <X size={12} />
                                </Badge>
                            )}
                            {filters.inStockOnly && (
                                <Badge
                                    bg=""
                                    className="d-flex align-items-center gap-1 px-3 py-2 rounded-pill"
                                    style={{ background: 'var(--tj-success-soft)', color: 'var(--tj-success)', fontWeight: 600, fontSize: '0.75rem', cursor: 'pointer' }}
                                    onClick={() => setFilters((prev) => ({ ...prev, inStockOnly: false }))}
                                >
                                    In Stock <X size={12} />
                                </Badge>
                            )}
                            {filters.onSaleOnly && (
                                <Badge
                                    bg=""
                                    className="d-flex align-items-center gap-1 px-3 py-2 rounded-pill"
                                    style={{ background: 'var(--tj-danger-soft)', color: 'var(--tj-danger)', fontWeight: 600, fontSize: '0.75rem', cursor: 'pointer' }}
                                    onClick={() => setFilters((prev) => ({ ...prev, onSaleOnly: false }))}
                                >
                                    On Sale <X size={12} />
                                </Badge>
                            )}
                        </div>
                    )}

                    {/* Products */}
                    {filteredProducts.length === 0 ? (
                        <div className="text-center py-5">
                            <div className="d-flex justify-content-center mb-3">
                                <div className="rounded-circle d-flex align-items-center justify-content-center" style={{ width: 72, height: 72, background: 'var(--tj-accent-soft)' }}>
                                    <SlidersHorizontal size={28} style={{ color: 'var(--tj-accent)' }} />
                                </div>
                            </div>
                            <h5 className="fw-bold mb-2" style={{ color: 'var(--tj-text-primary)' }}>No products found</h5>
                            <p style={{ color: 'var(--tj-text-muted)', maxWidth: 400, margin: '0 auto' }}>
                                Try adjusting your filters or clearing them to see more results.
                            </p>
                            <Button variant="outline-primary" className="mt-3" onClick={clearFilters}>
                                Clear All Filters
                            </Button>
                        </div>
                    ) : (
                        <>
                        <Row className="g-4 stagger-children">
                            {filteredProducts.map((product, idx) => (
                                <Col key={product.id} lg={viewMode === 'grid' ? 4 : 12} md={viewMode === 'grid' ? 6 : 12}>
                                    <ProductCard
                                        {...product}
                                        index={idx}
                                        onAddToCart={() => {}}
                                        onToggleWishlist={() => {}}
                                    />
                                </Col>
                            ))}
                        </Row>
                        
                        {/* Pagination Section */}
                        <div className="d-flex justify-content-center align-items-center gap-2 mt-5 pt-4 border-top" style={{ borderColor: 'var(--tj-border)' }}>
                            <Button variant="outline-primary" size="sm" className="rounded-circle p-0 d-flex align-items-center justify-content-center" style={{ width: 36, height: 36 }}>
                                {isRtl ? <ChevronRight size={18} /> : <ChevronLeft size={18} />}
                            </Button>
                            
                            <div className="d-flex align-items-center gap-1">
                                {[1, 2, 3, 4].map((p) => (
                                    <Button
                                        key={p}
                                        variant={p === 1 ? 'primary' : 'outline-primary'}
                                        size="sm"
                                        className="rounded-3 p-0 d-flex align-items-center justify-content-center"
                                        style={{ width: 36, height: 36 }}
                                    >
                                        {p}
                                    </Button>
                                ))}
                                <span className="mx-1" style={{ color: 'var(--tj-text-light)' }}>...</span>
                                <Button
                                    variant="outline-primary"
                                    size="sm"
                                    className="rounded-3 p-0 d-flex align-items-center justify-content-center"
                                    style={{ width: 36, height: 36 }}
                                >
                                    12
                                </Button>
                            </div>

                            <Button variant="outline-primary" size="sm" className="rounded-circle p-0 d-flex align-items-center justify-content-center" style={{ width: 36, height: 36 }}>
                                {isRtl ? <ChevronLeft size={18} /> : <ChevronRight size={18} />}
                            </Button>
                        </div>
                        </>
                    )}
                </Col>
            </Row>

            {/* ── Mobile Filter Offcanvas ─────────────────────────── */}
            <Offcanvas
                show={showMobileFilter}
                onHide={() => setShowMobileFilter(false)}
                placement="start"
                className="d-lg-none"
                style={{ background: 'var(--tj-card)', maxWidth: 320 }}
            >
                <Offcanvas.Header closeButton style={{ borderBottom: '1px solid var(--tj-border)' }}>
                    <Offcanvas.Title className="fw-bold" style={{ color: 'var(--tj-text-primary)' }}>Filters</Offcanvas.Title>
                </Offcanvas.Header>
                <Offcanvas.Body>{filterContent}</Offcanvas.Body>
                <div className="p-3" style={{ borderTop: '1px solid var(--tj-border)' }}>
                    <Button
                        variant="primary"
                        className="w-100"
                        onClick={() => setShowMobileFilter(false)}
                    >
                        Show {filteredProducts.length} Results
                    </Button>
                </div>
            </Offcanvas>
        </Container>
    );
}
