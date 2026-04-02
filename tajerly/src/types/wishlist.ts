export interface WishlistItem {
    id: number | string;
    productId: number;
    productName: string;
    productSlug: string;
    thumbnailUrl?: string;
    price: number;
    compareAtPrice?: number | null;
    stock: number;
    addedAt: string;
}
