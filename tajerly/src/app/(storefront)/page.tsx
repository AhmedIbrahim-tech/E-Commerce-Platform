'use client';

import { useTranslation } from 'react-i18next';
import { Laptop, Shirt, Sofa, Sparkle, Dumbbell } from 'lucide-react';
import { Hero } from '@/components/home/Hero';
import { CategorySlider } from '@/components/home/CategorySlider';
import { FeaturedSlider } from '@/components/home/FeaturedSlider';
import { Newsletter } from '@/components/home/Newsletter';

import { usePathname, useRouter } from 'next/navigation';
import { useAppSelector } from '@/store/hooks';
import { useEffect } from 'react';
import type { RootState } from '@/store';

type PersistedRootState = RootState & { _persist?: { rehydrated?: boolean } };

export default function HomePage() {
    const { t } = useTranslation();
    const router = useRouter();
    const userRole = useAppSelector((state) => state.auth.user?.role);
    const rehydrated = useAppSelector((state: PersistedRootState) => state._persist?.rehydrated === true);

    useEffect(() => {
        if (rehydrated && userRole === 'Admin') {
            router.replace('/admin');
        }
    }, [rehydrated, userRole, router]);

    // Mock data for the sliders
    const featuredProducts = [
        { id: 1, name: t('featured.products.headphones', 'Premium Wireless Headphones'), slug: 'premium-wireless-headphones', price: 1299.99, compareAtPrice: 1799.99, thumbnailUrl: 'https://images.unsplash.com/photo-1505740420928-5e560c06d30e?q=80&w=400', averageRating: 4.8, reviewCount: 124, stock: 15, isFeatured: true, isActive: true, categoryId: 1, categoryName: t('categoriesSection.electronics'), createdAt: '' },
        { id: 2, name: t('featured.products.bag', 'Leather Crossbody Bag'), slug: 'leather-crossbody-bag', price: 849.99, compareAtPrice: null, thumbnailUrl: 'https://images.unsplash.com/photo-1548036328-c9fa89d128fa?q=80&w=400', averageRating: 4.6, reviewCount: 89, stock: 8, isFeatured: true, isActive: true, categoryId: 2, categoryName: t('categoriesSection.fashion'), createdAt: '' },
        { id: 3, name: t('featured.products.watch', 'Smart Fitness Watch Pro'), slug: 'smart-fitness-watch-pro', price: 2499.99, compareAtPrice: 2999.99, thumbnailUrl: 'https://images.unsplash.com/photo-1523275335684-37898b6baf30?q=80&w=400', averageRating: 4.9, reviewCount: 256, stock: 22, isFeatured: true, isActive: true, categoryId: 1, categoryName: t('categoriesSection.electronics'), createdAt: '' },
        { id: 4, name: t('featured.products.skincare', 'Organic Skincare Set'), slug: 'organic-skincare-set', price: 599.99, compareAtPrice: 749.99, thumbnailUrl: 'https://images.unsplash.com/photo-1596462502278-27bfdc403348?q=80&w=400', averageRating: 4.7, reviewCount: 67, stock: 30, isFeatured: true, isActive: true, categoryId: 3, categoryName: t('categoriesSection.beauty'), createdAt: '' },
        { id: 5, name: t('featured.products.shoes', 'Performance Running Shoes'), slug: 'performance-running-shoes', price: 1099.99, compareAtPrice: null, thumbnailUrl: 'https://images.unsplash.com/photo-1542291026-7eec264c27ff?q=80&w=400', averageRating: 4.5, reviewCount: 42, stock: 10, isFeatured: true, isActive: true, categoryId: 4, categoryName: t('categoriesSection.sports'), createdAt: '' },
        { id: 6, name: t('featured.products.lamp', 'Minimalist Desk Lamp'), slug: 'minimalist-desk-lamp', price: 399.99, compareAtPrice: 499.99, thumbnailUrl: 'https://images.unsplash.com/photo-1534073828943-f801091bb18c?q=80&w=400', averageRating: 4.3, reviewCount: 31, stock: 10, isFeatured: true, isActive: true, categoryId: 5, categoryName: t('categoriesSection.home'), createdAt: '' },
        { id: 7, name: t('featured.products.speaker', 'Bluetooth Portable Speaker'), slug: 'bluetooth-portable-speaker', price: 699.99, compareAtPrice: 899.99, thumbnailUrl: 'https://images.unsplash.com/photo-1608043152269-423dbba4e7e1?q=80&w=400', averageRating: 4.4, reviewCount: 98, stock: 18, isFeatured: true, isActive: true, categoryId: 1, categoryName: t('categoriesSection.electronics'), createdAt: '' },
        { id: 8, name: t('featured.products.shirt', 'Cotton Casual T-Shirt'), slug: 'cotton-casual-tshirt', price: 199.99, compareAtPrice: null, thumbnailUrl: 'https://images.unsplash.com/photo-1521572163474-6864f9cf17ab?q=80&w=400', averageRating: 4.2, reviewCount: 53, stock: 45, isFeatured: true, isActive: true, categoryId: 2, categoryName: t('categoriesSection.fashion'), createdAt: '' },
    ];

    const categoriesList = [
        { name: t('categoriesSection.electronics'), icon: Laptop, color: '#3B82F6', image: 'https://images.unsplash.com/photo-1498049794561-7780e7231661?q=80&w=400' },
        { name: t('categoriesSection.fashion'), icon: Shirt, color: '#EC4899', image: 'https://images.unsplash.com/photo-1445205170230-053b83016050?q=80&w=400' },
        { name: t('categoriesSection.home'), icon: Sofa, color: '#10B981', image: 'https://images.unsplash.com/photo-1556909114-f6e7ad7d3136?q=80&w=400' },
        { name: t('categoriesSection.beauty'), icon: Sparkle, color: '#F59E0B', image: 'https://images.unsplash.com/photo-1596462502278-27bfdc403348?q=80&w=400' },
        { name: t('categoriesSection.sports'), icon: Dumbbell, color: '#8B5CF6', image: 'https://images.unsplash.com/photo-1461896836934-bd45ba0c5530?q=80&w=400' },
    ];

    return (
        <main>
            <Hero />
            <CategorySlider categories={categoriesList} />
            <FeaturedSlider products={featuredProducts} />
            <Newsletter />
        </main>
    );
}
