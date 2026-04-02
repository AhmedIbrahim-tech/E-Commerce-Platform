'use client';

import Link from 'next/link';
import { usePathname, useRouter } from 'next/navigation';
import { Container, Navbar, Nav, Badge, Form, Button } from 'react-bootstrap';
import { ShoppingCart, Heart, User, Search, Menu, ShoppingBag, X, Trash2, ArrowRight, ArrowLeft, Home, LogIn, LayoutDashboard } from 'lucide-react';
import { ThemeToggle } from './ThemeToggle';
import { LocaleToggle } from './LocaleToggle';
import { useAppSelector, useAppDispatch } from '@/store/hooks';
import { removeItem } from '@/store/slices/cartSlice';
import { siteConfig } from '@/config/site';
import { formatPrice } from '@/utils/formatters';
import { useState, useRef, useEffect, useCallback } from 'react';
import { useDebounce } from '@/hooks/useDebounce';
import { useTranslation } from 'react-i18next';
import { WishlistItem } from '@/types/wishlist';
import { CartItem } from '@/types/cart';

export function StorefrontHeader() {
    const pathname = usePathname();
    const router = useRouter();
    const dispatch = useAppDispatch();
    const { items: cartItems, itemCount: cartCount, totalAmount } = useAppSelector((state) => state.cart);
    const wishlistItems = useAppSelector((state) => state.wishlist.items);
    const wishlistCount = wishlistItems.length;
    const isAuthenticated = useAppSelector((state) => state.auth.isAuthenticated);
    const user = useAppSelector((state) => state.auth.user);
    const authRoles = useAppSelector((state) => state.auth.roles);
    const isAdmin =
        user?.role?.toLowerCase() === 'admin' ||
        authRoles.some((r) => r.toLowerCase() === 'admin');

    // ── Search state ────────────────────────────────────────────────
    const [searchQuery, setSearchQuery] = useState('');
    const [showDropdown, setShowDropdown] = useState(false);
    const [selectedIndex, setSelectedIndex] = useState(-1);
    const debouncedQuery = useDebounce(searchQuery, 250);
    const searchRef = useRef<HTMLDivElement>(null);
    const inputRef = useRef<HTMLInputElement>(null);

    // ── Wishlist & Cart dropdown state ──────────────────────────────
    const [showWishlistDrop, setShowWishlistDrop] = useState(false);
    const [showCartDrop, setShowCartDrop] = useState(false);
    const wishlistRef = useRef<HTMLDivElement>(null);
    const cartRef = useRef<HTMLDivElement>(null);

    const { t, i18n } = useTranslation();
    const isRtl = i18n.language === 'ar';

    // ── Mock suggestions for demo (properly localized) ────────
    const localizedSuggestions = [
        { id: 1, name: t('featured.products.headphones'), slug: 'premium-wireless-headphones', categoryName: t('categoriesSection.electronics'), price: 1299.99 },
        { id: 2, name: t('featured.products.bag'), slug: 'leather-crossbody-bag', categoryName: t('categoriesSection.fashion'), price: 849.99 },
        { id: 3, name: t('featured.products.watch'), slug: 'smart-fitness-watch-pro', categoryName: t('categoriesSection.electronics'), price: 2499.99 },
        { id: 4, name: t('featured.products.skincare'), slug: 'organic-skincare-set', categoryName: t('categoriesSection.beauty'), price: 599.99 },
    ];

    // Filter suggestions using localized names
    const suggestions = debouncedQuery.length >= 2
        ? localizedSuggestions.filter((p) =>
            p.name.toLowerCase().includes(debouncedQuery.toLowerCase()) ||
            p.categoryName.toLowerCase().includes(debouncedQuery.toLowerCase())
        ).slice(0, 5)
        : [];

    // Close dropdowns on outside click
    useEffect(() => {
        const handle = (e: MouseEvent) => {
            const t = e.target as Node;
            if (searchRef.current && !searchRef.current.contains(t)) setShowDropdown(false);
            if (wishlistRef.current && !wishlistRef.current.contains(t)) setShowWishlistDrop(false);
            if (cartRef.current && !cartRef.current.contains(t)) setShowCartDrop(false);
        };
        document.addEventListener('mousedown', handle);
        return () => document.removeEventListener('mousedown', handle);
    }, []);

    // Navigate to full search page
    const goToSearch = useCallback((query: string) => {
        if (!query.trim()) return;
        setShowDropdown(false);
        setSearchQuery('');
        router.push(`/search?q=${encodeURIComponent(query.trim())}`);
    }, [router]);

    // Keyboard navigation
    const handleKeyDown = (e: React.KeyboardEvent) => {
        if (e.key === 'Enter') {
            if (selectedIndex >= 0 && selectedIndex < suggestions.length) {
                router.push(`/products/${suggestions[selectedIndex].slug}`);
                setShowDropdown(false);
                setSearchQuery('');
            } else {
                goToSearch(searchQuery);
            }
        } else if (e.key === 'ArrowDown') {
            e.preventDefault();
            setSelectedIndex((prev) => Math.min(prev + 1, suggestions.length - 1));
        } else if (e.key === 'ArrowUp') {
            e.preventDefault();
            setSelectedIndex((prev) => Math.max(prev - 1, -1));
        } else if (e.key === 'Escape') {
            setShowDropdown(false);
            inputRef.current?.blur();
        }
    };

    const navLinks = [
        { href: '/', label: t('common.home'), icon: Home },
        { href: '/products', label: t('common.shop'), icon: ShoppingBag },
    ];

    return (
        <Navbar
            expand="lg"
            fixed="top"
            className="py-2"
            style={{
                background: 'var(--tj-navbar)',
                backdropFilter: 'blur(12px)',
                borderBottom: '1px solid var(--tj-border)',
                zIndex: 1030,
            }}
        >
            <Container fluid className="px-lg-5">
                {/* Brand */}
                <Link href="/" className="text-decoration-none d-flex align-items-center gap-3 me-lg-2" id="brand-logo">
                    <div
                        className="d-flex align-items-center justify-content-center rounded-3"
                        style={{
                            width: 40, height: 40,
                            background: 'var(--tj-accent)',
                            color: 'var(--tj-accent-foreground)',
                            fontWeight: 900,
                            fontSize: '1.125rem',
                        }}
                    >
                        <ShoppingBag size={20} />
                    </div>
                    <span className="d-none d-md-inline" style={{ fontWeight: 800, fontSize: '1.25rem', color: 'var(--tj-text-primary)', letterSpacing: '-0.025em' }}>
                        {t('common.siteName', 'Tajerly')}
                    </span>
                </Link>

                <Navbar.Toggle aria-controls="main-nav" className="border-0">
                    <Menu size={24} style={{ color: 'var(--tj-text-primary)' }} />
                </Navbar.Toggle>

                <Navbar.Collapse id="main-nav">
                    {/* Navigation Links */}
                    <Nav className="flex-grow-1 ms-lg-5 gap-3" style={{ paddingInlineStart: '1.5rem' }}>
                        {navLinks.map((link) => (
                            <Link
                                key={link.href}
                                href={link.href}
                                className={`nav-link px-3 py-2 rounded-3 text-decoration-none fw-semibold d-flex align-items-center gap-2 ${pathname === link.href ? 'active' : ''}`}
                                style={{
                                    color: pathname === link.href ? 'var(--tj-accent)' : 'var(--tj-text-secondary)',
                                    background: pathname === link.href ? 'var(--tj-accent-soft)' : 'transparent',
                                    fontSize: '0.9rem',
                                    transition: 'all 0.2s',
                                }}
                                id={`nav-${link.label.toLowerCase()}`}
                            >
                                <link.icon size={16} />
                                {link.label}
                            </Link>
                        ))}
                    </Nav>

                    {/* ── Search Box ──────────────────────────────────── */}
                    <div ref={searchRef} className="position-relative mx-lg-auto my-2 my-lg-0" style={{ maxWidth: 600, width: '100%' }}>
                        <div className="position-relative">
                            <div className="position-absolute d-flex align-items-center justify-content-center search-icon-wrapper" style={{ left: 12, top: '50%', transform: 'translateY(-50%)', color: 'var(--tj-text-light)', pointerEvents: 'none' }}>
                                <Search size={16} />
                            </div>
                            <Form.Control
                                ref={inputRef}
                                type="text"
                                placeholder={t('common.searchPlaceholder')}
                                value={searchQuery}
                                onChange={(e) => {
                                    setSearchQuery(e.target.value);
                                    setShowDropdown(true);
                                    setSelectedIndex(-1);
                                }}
                                onFocus={() => { if (searchQuery.length >= 2) setShowDropdown(true); }}
                                onKeyDown={handleKeyDown}
                                size="sm"
                                className="rounded-pill border-0"
                                style={{
                                    paddingLeft: 38,
                                    paddingRight: searchQuery ? 36 : 12,
                                    background: 'var(--tj-bg-secondary)',
                                    color: 'var(--tj-text-primary)',
                                    fontSize: '0.9rem',
                                    height: 44,
                                }}
                                id="navbar-search"
                            />
                            {searchQuery && (
                                <button
                                    className="btn btn-link position-absolute p-0 d-flex align-items-center justify-content-center"
                                    style={{ right: 10, top: '50%', transform: 'translateY(-50%)', color: 'var(--tj-text-muted)', width: 24, height: 24 }}
                                    onClick={() => { setSearchQuery(''); setShowDropdown(false); }}
                                    aria-label="Clear search"
                                >
                                    <X size={14} />
                                </button>
                            )}
                        </div>

                        {/* ── Search Dropdown ────────────────────────── */}
                        {showDropdown && debouncedQuery.length >= 2 && (
                            <div
                                className="position-absolute w-100 mt-1 rounded-3 overflow-hidden animate-slide-down"
                                style={{
                                    background: 'var(--tj-surface-elevated)',
                                    border: '1px solid var(--tj-border)',
                                    boxShadow: 'var(--tj-shadow-lg)',
                                    zIndex: 1050,
                                    top: '100%',
                                }}
                            >
                                {suggestions.length > 0 ? (
                                    <>
                                        {suggestions.map((item, idx) => (
                                            <Link
                                                key={item.id}
                                                href={`/products/${item.slug}`}
                                                className="d-flex align-items-center gap-3 px-3 py-2 text-decoration-none"
                                                style={{
                                                    background: idx === selectedIndex ? 'var(--tj-accent-soft)' : 'transparent',
                                                    transition: 'background 0.1s',
                                                }}
                                                onClick={() => { setShowDropdown(false); setSearchQuery(''); }}
                                                onMouseEnter={() => setSelectedIndex(idx)}
                                            >
                                                <Search size={14} style={{ color: 'var(--tj-text-light)', flexShrink: 0 }} />
                                                <div className="flex-grow-1 overflow-hidden">
                                                    <p className="mb-0 text-truncate fw-semibold" style={{ fontSize: '0.85rem', color: 'var(--tj-text-primary)' }}>
                                                        {/* Highlight matching text */}
                                                        {highlightMatch(item.name, debouncedQuery)}
                                                    </p>
                                                    <span style={{ fontSize: '0.7rem', color: 'var(--tj-text-muted)' }}>
                                                        {t('common.in')} {item.categoryName}
                                                    </span>
                                                </div>
                                                <span className="fw-bold flex-shrink-0" style={{ fontSize: '0.8rem', color: 'var(--tj-accent)' }}>
                                                    EGP {item.price.toLocaleString()}
                                                </span>
                                            </Link>
                                        ))}
                                        {/* View all results link */}
                                        <button
                                            className="d-flex align-items-center justify-content-center gap-2 w-100 border-0 px-3 py-2"
                                            style={{
                                                background: 'var(--tj-bg-secondary)',
                                                color: 'var(--tj-accent)',
                                                fontWeight: 600,
                                                fontSize: '0.8rem',
                                                cursor: 'pointer',
                                                borderTop: '1px solid var(--tj-border)',
                                            }}
                                            onClick={() => goToSearch(searchQuery)}
                                        >
                                            {t('featured.viewAll')} &ldquo;{debouncedQuery}&rdquo; {isRtl ? <ArrowLeft size={14} /> : <ArrowRight size={14} />}
                                        </button>
                                    </>
                                ) : (
                                    <div className="px-3 py-4 text-center">
                                        <p className="mb-1 fw-semibold" style={{ fontSize: '0.85rem', color: 'var(--tj-text-primary)' }}>
                                            {t('common.noResults')}
                                        </p>
                                        <p className="mb-0" style={{ fontSize: '0.75rem', color: 'var(--tj-text-muted)' }}>
                                            Try a different keyword or{' '}
                                            <button
                                                className="btn btn-link p-0 fw-semibold border-0"
                                                style={{ fontSize: '0.75rem', color: 'var(--tj-accent)' }}
                                                onClick={() => goToSearch(searchQuery)}
                                            >
                                                {t('common.searchAll')}
                                            </button>
                                        </p>
                                    </div>
                                )}
                            </div>
                        )}
                    </div>

                    {/* Right Actions */}
                    <div className="d-flex align-items-center gap-1 flex-grow-1 justify-content-end">
                        <LocaleToggle />
                        <ThemeToggle />

                        {/* ── Wishlist with Dropdown ──────────────── */}
                        <div ref={wishlistRef} className="position-relative d-none d-lg-block">
                            <button
                                className="btn btn-link p-2 rounded-circle d-flex align-items-center justify-content-center position-relative"
                                style={{ width: 40, height: 40, color: 'var(--tj-text-muted)' }}
                                onClick={() => { setShowWishlistDrop(!showWishlistDrop); setShowCartDrop(false); }}
                                id="btn-wishlist"
                            >
                                <Heart size={18} />
                                {wishlistCount > 0 && (
                                    <Badge bg="" pill className="position-absolute" style={{ top: 2, right: isRtl ? 'auto' : 2, left: isRtl ? 2 : 'auto', fontSize: '0.625rem', background: 'var(--tj-danger)', color: '#fff', minWidth: 18, lineHeight: '18px', padding: '0 5px' }}>
                                        {wishlistCount}
                                    </Badge>
                                )}
                            </button>

                            {showWishlistDrop && (
                                <div className="position-absolute animate-slide-down" style={{ top: '100%', right: 0, width: 320, marginTop: 8, background: 'var(--tj-surface-elevated)', border: '1px solid var(--tj-border)', borderRadius: 'var(--tj-radius-md)', boxShadow: 'var(--tj-shadow-lg)', zIndex: 1060, overflow: 'hidden' }}>
                                    <div className="d-flex align-items-center justify-content-between px-3 py-2" style={{ borderBottom: '1px solid var(--tj-border)' }}>
                                        <span className="fw-bold" style={{ fontSize: '0.85rem', color: 'var(--tj-text-primary)' }}>{t('header.wishlistTitle')} ({wishlistCount})</span>
                                        <Heart size={16} style={{ color: 'var(--tj-danger)' }} />
                                    </div>

                                    {wishlistItems.length === 0 ? (
                                        <div className="px-3 py-4 text-center">
                                            <Heart size={28} style={{ color: 'var(--tj-text-light)', marginBottom: 8 }} />
                                            <p className="mb-0 fw-semibold" style={{ fontSize: '0.85rem', color: 'var(--tj-text-primary)' }}>{t('header.wishlistEmpty')}</p>
                                            <p className="mb-0" style={{ fontSize: '0.75rem', color: 'var(--tj-text-muted)' }}>{t('header.wishlistSave')}</p>
                                        </div>
                                    ) : (
                                        <div style={{ maxHeight: 260, overflowY: 'auto' }}>
                                            {wishlistItems.slice(0, 4).map((item: WishlistItem) => (
                                                <Link
                                                    key={item.productId}
                                                    href={`/products/${item.productSlug}`}
                                                    className="d-flex align-items-center gap-3 px-3 py-2 text-decoration-none"
                                                    style={{ borderBottom: '1px solid var(--tj-border)', transition: 'background 0.1s' }}
                                                    onClick={() => setShowWishlistDrop(false)}
                                                    onMouseEnter={(e) => (e.currentTarget.style.background = 'var(--tj-bg-secondary)')}
                                                    onMouseLeave={(e) => (e.currentTarget.style.background = 'transparent')}
                                                >
                                                    <img src={item.thumbnailUrl || 'https://images.unsplash.com/photo-1523275335684-37898b6baf30?q=80&w=80'} alt={item.productName} className="rounded-2 flex-shrink-0" style={{ width: 48, height: 48, objectFit: 'cover' }} />
                                                    <div className="flex-grow-1 overflow-hidden">
                                                        <p className="mb-0 text-truncate fw-semibold" style={{ fontSize: '0.825rem', color: 'var(--tj-text-primary)' }}>{item.productName}</p>
                                                        <span className="fw-bold" style={{ fontSize: '0.8rem', color: 'var(--tj-accent)' }}>{formatPrice(item.price)}</span>
                                                    </div>
                                                </Link>
                                            ))}
                                        </div>
                                    )}

                                    <Link
                                        href="/wishlist"
                                        className="d-flex align-items-center justify-content-center gap-2 px-3 py-2 text-decoration-none fw-semibold"
                                        style={{ background: 'var(--tj-bg-secondary)', color: 'var(--tj-accent)', fontSize: '0.8rem', borderTop: '1px solid var(--tj-border)' }}
                                        onClick={() => setShowWishlistDrop(false)}
                                    >
                                        {t('header.viewAllWishlist')} {isRtl ? <ArrowLeft size={14} /> : <ArrowRight size={14} />}
                                    </Link>
                                </div>
                            )}
                        </div>
                        {/* Mobile wishlist — simple link */}
                        <Link href="/wishlist" className="btn btn-link p-2 rounded-circle d-flex align-items-center justify-content-center position-relative d-lg-none" style={{ width: 40, height: 40, color: 'var(--tj-text-muted)' }}>
                            <Heart size={18} />
                            {wishlistCount > 0 && <Badge bg="" pill className="position-absolute" style={{ top: 2, right: isRtl ? 'auto' : 2, left: isRtl ? 2 : 'auto', fontSize: '0.625rem', background: 'var(--tj-danger)', color: '#fff', minWidth: 18, lineHeight: '18px', padding: '0 5px' }}>{wishlistCount}</Badge>}
                        </Link>

                        {/* ── Cart with Dropdown ─────────────────── */}
                        <div ref={cartRef} className="position-relative d-none d-lg-block">
                            <button
                                className="btn btn-link p-2 rounded-circle d-flex align-items-center justify-content-center position-relative"
                                style={{ width: 40, height: 40, color: 'var(--tj-text-muted)' }}
                                onClick={() => { setShowCartDrop(!showCartDrop); setShowWishlistDrop(false); }}
                                id="btn-cart"
                            >
                                <ShoppingCart size={18} />
                                {cartCount > 0 && (
                                    <Badge bg="" pill className="position-absolute" style={{ top: 2, right: isRtl ? 'auto' : 2, left: isRtl ? 2 : 'auto', fontSize: '0.625rem', background: 'var(--tj-accent)', color: 'var(--tj-accent-foreground)', minWidth: 18, lineHeight: '18px', padding: '0 5px' }}>
                                        {cartCount}
                                    </Badge>
                                )}
                            </button>

                            {showCartDrop && (
                                <div className="position-absolute animate-slide-down" style={{ top: '100%', right: 0, width: 360, marginTop: 8, background: 'var(--tj-surface-elevated)', border: '1px solid var(--tj-border)', borderRadius: 'var(--tj-radius-md)', boxShadow: 'var(--tj-shadow-lg)', zIndex: 1060, overflow: 'hidden' }}>
                                    <div className="d-flex align-items-center justify-content-between px-3 py-2" style={{ borderBottom: '1px solid var(--tj-border)' }}>
                                        <span className="fw-bold" style={{ fontSize: '0.85rem', color: 'var(--tj-text-primary)' }}>{t('header.cartTitle')} ({cartCount})</span>
                                        <ShoppingCart size={16} style={{ color: 'var(--tj-accent)' }} />
                                    </div>

                                    {cartItems.length === 0 ? (
                                        <div className="px-3 py-4 text-center">
                                            <ShoppingBag size={28} style={{ color: 'var(--tj-text-light)', marginBottom: 8 }} />
                                            <p className="mb-0 fw-semibold" style={{ fontSize: '0.85rem', color: 'var(--tj-text-primary)' }}>{t('header.cartEmpty')}</p>
                                            <p className="mb-0" style={{ fontSize: '0.75rem', color: 'var(--tj-text-muted)' }}>{t('header.cartStart')}</p>
                                        </div>
                                    ) : (
                                        <div style={{ maxHeight: 280, overflowY: 'auto' }}>
                                            {cartItems.slice(0, 4).map((item: CartItem) => (
                                                <div
                                                    key={item.productId}
                                                    className="d-flex align-items-center gap-3 px-3 py-2"
                                                    style={{ borderBottom: '1px solid var(--tj-border)' }}
                                                >
                                                    <Link href={`/products/${item.productSlug}`} onClick={() => setShowCartDrop(false)} className="flex-shrink-0">
                                                        <img src={item.thumbnailUrl || 'https://images.unsplash.com/photo-1523275335684-37898b6baf30?q=80&w=80'} alt={item.productName} className="rounded-2" style={{ width: 48, height: 48, objectFit: 'cover' }} />
                                                    </Link>
                                                    <div className="flex-grow-1 overflow-hidden">
                                                        <Link href={`/products/${item.productSlug}`} onClick={() => setShowCartDrop(false)} className="text-decoration-none">
                                                            <p className="mb-0 text-truncate fw-semibold" style={{ fontSize: '0.825rem', color: 'var(--tj-text-primary)' }}>{item.productName}</p>
                                                        </Link>
                                                        <div className="d-flex align-items-center gap-2">
                                                            <span style={{ fontSize: '0.75rem', color: 'var(--tj-text-muted)' }}>{t('common.qty')}: {item.quantity}</span>
                                                            <span className="fw-bold" style={{ fontSize: '0.8rem', color: 'var(--tj-accent)' }}>{formatPrice(item.subtotal)}</span>
                                                        </div>
                                                    </div>
                                                    <button
                                                        className="btn btn-link p-1 flex-shrink-0"
                                                        style={{ color: 'var(--tj-text-light)' }}
                                                        onClick={() => dispatch(removeItem(item.productId))}
                                                        aria-label="Remove item"
                                                    >
                                                        <Trash2 size={14} />
                                                    </button>
                                                </div>
                                            ))}
                                        </div>
                                    )}

                                    {cartItems.length > 0 && (
                                        <div className="px-3 py-2" style={{ borderTop: '1px solid var(--tj-border)', background: 'var(--tj-bg-secondary)' }}>
                                            <div className="d-flex justify-content-between mb-2">
                                                <span style={{ fontSize: '0.8rem', color: 'var(--tj-text-muted)' }}>{t('common.subtotal')}</span>
                                                <span className="fw-bold" style={{ fontSize: '0.9rem', color: 'var(--tj-accent)' }}>{formatPrice(totalAmount)}</span>
                                            </div>
                                            <div className="d-flex gap-2">
                                                <Link href="/cart" className="btn btn-outline-primary btn-sm flex-grow-1" onClick={() => setShowCartDrop(false)}>
                                                    {t('header.viewCart')}
                                                </Link>
                                                <Link href="/checkout" className="btn btn-primary btn-sm flex-grow-1" onClick={() => setShowCartDrop(false)}>
                                                    {t('header.checkout')}
                                                </Link>
                                            </div>
                                        </div>
                                    )}

                                    {cartItems.length === 0 && (
                                        <Link
                                            href="/products"
                                            className="d-flex align-items-center justify-content-center gap-2 px-3 py-2 text-decoration-none fw-semibold"
                                            style={{ background: 'var(--tj-bg-secondary)', color: 'var(--tj-accent)', fontSize: '0.8rem', borderTop: '1px solid var(--tj-border)' }}
                                            onClick={() => setShowCartDrop(false)}
                                        >
                                            {t('header.startShopping')} {isRtl ? <ArrowLeft size={14} /> : <ArrowRight size={14} />}
                                        </Link>
                                    )}
                                </div>
                            )}
                        </div>
                        {/* Mobile cart — simple link */}
                        <Link href="/cart" className="btn btn-link p-2 rounded-circle d-flex align-items-center justify-content-center position-relative d-lg-none" style={{ width: 40, height: 40, color: 'var(--tj-text-muted)' }}>
                            <ShoppingCart size={18} />
                            {cartCount > 0 && <Badge bg="" pill className="position-absolute" style={{ top: 2, right: isRtl ? 'auto' : 2, left: isRtl ? 2 : 'auto', fontSize: '0.625rem', background: 'var(--tj-accent)', color: 'var(--tj-accent-foreground)', minWidth: 18, lineHeight: '18px', padding: '0 5px' }}>{cartCount}</Badge>}
                        </Link>

                        {isAdmin && (
                            <Link
                                href="/admin"
                                className="btn btn-link p-2 rounded-circle d-flex align-items-center justify-content-center text-decoration-none me-1"
                                style={{ width: 40, height: 40, color: 'var(--tj-accent)' }}
                                title={t('header.adminDashboard')}
                                aria-label={t('header.adminDashboard')}
                            >
                                <LayoutDashboard size={20} strokeWidth={2.25} />
                            </Link>
                        )}

                        {/* User */}
                        {isAuthenticated ? (
                            <Link
                                href="/profile"
                                className="btn btn-link p-2 rounded-circle d-flex align-items-center justify-content-center"
                                style={{ width: 40, height: 40, color: 'var(--tj-text-muted)' }}
                                id="btn-profile"
                            >
                                <User size={18} />
                            </Link>
                        ) : (
                            <Link
                                href="/login"
                                className="btn btn-primary btn-sm px-3 ms-1 d-flex align-items-center gap-2"
                                id="btn-login"
                            >
                                <LogIn size={16} />
                                {t('common.signIn')}
                            </Link>
                        )}
                    </div>
                </Navbar.Collapse>
            </Container>
        </Navbar>
    );
}

/** Highlight matching portion of text in search results */
function highlightMatch(text: string, query: string) {
    if (!query) return text;
    const idx = text.toLowerCase().indexOf(query.toLowerCase());
    if (idx === -1) return text;
    const before = text.substring(0, idx);
    const match = text.substring(idx, idx + query.length);
    const after = text.substring(idx + query.length);
    return (
        <>
            {before}
            <strong style={{ color: 'var(--tj-accent)' }}>{match}</strong>
            {after}
        </>
    );
}
