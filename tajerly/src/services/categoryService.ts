import api from '@/config/api';
import { Category, SubCategory, CreateCategoryDto, UpdateCategoryDto } from '@/types/category';
import { ApiResponse } from '@/types/api';

export const categoryService = {
    getAllCategories: async (activeOnly = false): Promise<ApiResponse<Category[]>> => {
        const response = await api.get<ApiResponse<Category[]>>('v1/category/getAll', {
            params: activeOnly ? { activeOnly: 'true' } : undefined
        });
        return response.data;
    },

    getCategoryById: async (id: number): Promise<ApiResponse<Category>> => {
        // Backend: api/v1/category/getById?id={id}
        const response = await api.get<ApiResponse<Category>>('v1/category/getById', {
            params: { id }
        });
        return response.data;
    },

    getCategoryBySlug: async (slug: string): Promise<ApiResponse<Category>> => {
        // Slugs aren't in Router.cs, but common in frontend.
        // If not in backend, fallback to getById or check if backend supports by slug.
        const response = await api.get<ApiResponse<Category>>(`v1/category/getBySlug/${slug}`);
        return response.data;
    },

    getSubCategoriesByCategoryId: async (categoryId: number): Promise<ApiResponse<SubCategory[]>> => {
        // Backend: api/v1/lookups/subCategoriesByCategory?categoryId={categoryId}
        const response = await api.get<ApiResponse<SubCategory[]>>('v1/lookups/subCategoriesByCategory', {
            params: { categoryId }
        });
        return response.data;
    },

    createCategory: async (data: CreateCategoryDto): Promise<ApiResponse<Category>> => {
        const response = await api.post<ApiResponse<Category>>('v1/category/create', data);
        return response.data;
    },

    updateCategory: async (id: number, data: UpdateCategoryDto): Promise<ApiResponse<Category>> => {
        const response = await api.put<ApiResponse<Category>>('v1/category/edit', data);
        return response.data;
    },

    deleteCategory: async (id: number): Promise<ApiResponse<void>> => {
        const response = await api.delete<ApiResponse<void>>('v1/category/delete', {
            params: { id }
        });
        return response.data;
    }
};
