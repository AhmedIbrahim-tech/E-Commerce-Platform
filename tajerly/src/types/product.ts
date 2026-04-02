/** Lightweight product for lists (no full description). */
export interface ProductListItem {
    id: number | string;
    name: string;
    slug: string;
    price: number;
    compareAtPrice?: number | null;
    thumbnailUrl?: string;
    categoryId: number;
    categoryName?: string;
    averageRating?: number;
    reviewCount?: number;
    isActive: boolean;
    isFeatured: boolean;
    stock: number;
    createdAt: string;
}

/** Full product detail. */
export interface Product {
    id: number | string;
    name: string;
    slug: string;
    description: string;
    shortDescription?: string;
    price: number;
    compareAtPrice?: number | null;
    sku?: string;
    stock: number;
    thumbnailUrl?: string;
    images: ProductImage[];
    categoryId: number;
    categoryName?: string;
    subCategoryId?: number | null;
    subCategoryName?: string;
    brand?: string;
    tags?: string[];
    averageRating?: number;
    reviewCount?: number;
    isActive: boolean;
    isFeatured: boolean;
    specifications?: Record<string, string>;
    createdAt: string;
    updatedAt?: string;
}

export interface ProductImage {
    id: number | string;
    url: string;
    alt?: string;
    displayOrder: number;
}

export interface CreateProductDto {
    name: string;
    description: string;
    shortDescription?: string;
    price: number;
    compareAtPrice?: number | null;
    sku?: string;
    stock: number;
    categoryId: number;
    subCategoryId?: number | null;
    brand?: string;
    tags?: string[];
    isActive: boolean;
    isFeatured: boolean;
}

export interface UpdateProductDto {
    name: string;
    description: string;
    shortDescription?: string;
    price: number;
    compareAtPrice?: number | null;
    sku?: string;
    stock: number;
    categoryId: number;
    subCategoryId?: number | null;
    brand?: string;
    tags?: string[];
    isActive: boolean;
    isFeatured: boolean;
}
