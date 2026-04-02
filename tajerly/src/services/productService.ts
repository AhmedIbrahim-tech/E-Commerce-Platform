import api from '@/config/api';
import { Product, ProductListItem, CreateProductDto, UpdateProductDto } from '@/types/product';
import { ApiResponse, PagedResult } from '@/types/api';

export interface ProductsPagedParams {
    categoryId?: number;
    subCategoryId?: number;
    searchTerm?: string;
    minPrice?: number;
    maxPrice?: number;
    brands?: string[];
    tags?: string[];
    isActive?: boolean;
    isFeatured?: boolean;
    page?: number;
    pageSize?: number;
    sortBy?: string;
    sortDescending?: boolean;
}

export const productService = {
    getProductsPaged: async (params: ProductsPagedParams): Promise<ApiResponse<PagedResult<ProductListItem>>> => {
        const queryParams = {
            categoryId: params.categoryId ?? null,
            subCategoryId: params.subCategoryId ?? null,
            searchTerm: params.searchTerm ?? null,
            minPrice: params.minPrice ?? null,
            maxPrice: params.maxPrice ?? null,
            brands: params.brands ?? null,
            tags: params.tags ?? null,
            isActive: params.isActive ?? null,
            isFeatured: params.isFeatured ?? null,
            page: params.page ?? 1,
            pageSize: params.pageSize ?? 20,
            sortBy: params.sortBy ?? 'CreatedAt',
            sortDescending: params.sortDescending ?? true,
        };
        const response = await api.get<ApiResponse<PagedResult<ProductListItem>>>('v1/product/paginated', { params: queryParams });
        return response.data;
    },

    getProductById: async (id: number): Promise<ApiResponse<Product>> => {
        // Backend: api/v1/product/getSingle?id={id}
        const response = await api.get<ApiResponse<Product>>('v1/product/getSingle', {
            params: { id }
        });
        return response.data;
    },

    getProductBySlug: async (slug: string): Promise<ApiResponse<Product>> => {
        const response = await api.get<ApiResponse<Product>>(`v1/product/by-slug/${slug}`);
        return response.data;
    },

    getFeaturedProducts: async (): Promise<ApiResponse<ProductListItem[]>> => {
        const response = await api.get<ApiResponse<ProductListItem[]>>('v1/product/paginated', {
            params: { isFeatured: true, pageSize: 8 }
        });
        return response.data;
    },

    getRelatedProducts: async (id: number): Promise<ApiResponse<ProductListItem[]>> => {
        const response = await api.get<ApiResponse<ProductListItem[]>>(`v1/product/${id}/related`);
        return response.data;
    },

    createProduct: async (data: CreateProductDto): Promise<ApiResponse<Product>> => {
        const response = await api.post<ApiResponse<Product>>('v1/product/create', data);
        return response.data;
    },

    updateProduct: async (id: number, data: UpdateProductDto): Promise<ApiResponse<Product>> => {
        const response = await api.put<ApiResponse<Product>>('v1/product/edit', data);
        return response.data;
    },

    deleteProduct: async (id: number): Promise<ApiResponse<void>> => {
        const response = await api.delete<ApiResponse<void>>(`v1/product/${id}`);
        return response.data;
    },

    toggleStatus: async (id: number): Promise<ApiResponse<Product>> => {
        const response = await api.patch<ApiResponse<Product>>(`v1/product/${id}/toggleStatus`);
        return response.data;
    },
};
