/** id/categoryId are number from API; route params may be string. */
export interface SubCategory {
    id: number | string;
    name: string;
    slug: string;
    categoryId: number | string;
    displayOrder?: number;
    categoryName?: string;
    productCount?: number;
}

export interface Category {
    id: number | string;
    name: string;
    slug: string;
    description?: string;
    imageUrl?: string;
    isActive?: boolean;
    displayOrder?: number;
    productCount?: number;
    subCategories: SubCategory[];
}

export interface CreateCategoryDto {
    name: string;
    description?: string | null;
    imageUrl?: string | null;
    isActive: boolean;
    displayOrder?: number;
}

export interface UpdateCategoryDto {
    name: string;
    description?: string | null;
    imageUrl?: string | null;
    isActive: boolean;
    displayOrder: number;
}
