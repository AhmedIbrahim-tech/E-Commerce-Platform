const PRODUCT_PLACEHOLDER_IMAGES = [
    'https://images.unsplash.com/photo-1523275335684-37898b6baf30?q=80&w=400&auto=format&fit=crop',
    'https://images.unsplash.com/photo-1505740420928-5e560c06d30e?q=80&w=400&auto=format&fit=crop',
    'https://images.unsplash.com/photo-1526170375885-4d8ecf77b99f?q=80&w=400&auto=format&fit=crop',
    'https://images.unsplash.com/photo-1572635196237-14b3f281503f?q=80&w=400&auto=format&fit=crop',
    'https://images.unsplash.com/photo-1560343090-f0409e92791a?q=80&w=400&auto=format&fit=crop',
    'https://images.unsplash.com/photo-1491553895911-0055eca6402d?q=80&w=400&auto=format&fit=crop',
];

export function getProductImage(providedUrl?: string | null, index: number = 0): string {
    if (providedUrl) return providedUrl;
    return PRODUCT_PLACEHOLDER_IMAGES[index % PRODUCT_PLACEHOLDER_IMAGES.length];
}

const CATEGORY_IMAGES: Record<string, string> = {
    electronics: 'https://images.unsplash.com/photo-1498049794561-7780e7231661?q=80&w=400&auto=format&fit=crop',
    fashion: 'https://images.unsplash.com/photo-1445205170230-053b83016050?q=80&w=400&auto=format&fit=crop',
    home: 'https://images.unsplash.com/photo-1556909114-f6e7ad7d3136?q=80&w=400&auto=format&fit=crop',
    beauty: 'https://images.unsplash.com/photo-1596462502278-27bfdc403348?q=80&w=400&auto=format&fit=crop',
    sports: 'https://images.unsplash.com/photo-1461896836934-bd45ba0c5530?q=80&w=400&auto=format&fit=crop',
    books: 'https://images.unsplash.com/photo-1495446815901-a7297e633e8d?q=80&w=400&auto=format&fit=crop',
    default: 'https://images.unsplash.com/photo-1472851294608-062f824d29cc?q=80&w=400&auto=format&fit=crop',
};

export function getCategoryImage(name: string, providedImage?: string): string {
    if (providedImage) return providedImage;
    const lowerName = name.toLowerCase();
    for (const [key, url] of Object.entries(CATEGORY_IMAGES)) {
        if (key !== 'default' && lowerName.includes(key)) return url;
    }
    return CATEGORY_IMAGES.default;
}
