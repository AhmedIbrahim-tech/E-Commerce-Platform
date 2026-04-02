import api from '@/config/api';
import { ApiResponse } from '@/types/api';

export interface LookupItem {
    id: number | string;
    name: string;
}

export const lookupService = {
    getCategories: async (): Promise<ApiResponse<LookupItem[]>> => {
        const response = await api.get<ApiResponse<LookupItem[]>>('v1/lookups/categories');
        return response.data;
    },

    getSubCategories: async (categoryId?: number): Promise<ApiResponse<LookupItem[]>> => {
        const response = await api.get<ApiResponse<LookupItem[]>>('v1/lookups/subCategoriesByCategory', {
            params: categoryId ? { categoryId } : undefined
        });
        return response.data;
    },

    getBrands: async (): Promise<ApiResponse<LookupItem[]>> => {
        const response = await api.get<ApiResponse<LookupItem[]>>('v1/lookups/brands');
        return response.data;
    },

    getTags: async (): Promise<ApiResponse<LookupItem[]>> => {
        const response = await api.get<ApiResponse<LookupItem[]>>('v1/lookups/tags');
        return response.data;
    },

    getUnitOfMeasures: async (): Promise<ApiResponse<LookupItem[]>> => {
        const response = await api.get<ApiResponse<LookupItem[]>>('v1/lookups/unitOfMeasures');
        return response.data;
    },

    getWarranties: async (): Promise<ApiResponse<LookupItem[]>> => {
        const response = await api.get<ApiResponse<LookupItem[]>>('v1/lookups/warranties');
        return response.data;
    },

    getVariantAttributes: async (): Promise<ApiResponse<LookupItem[]>> => {
        const response = await api.get<ApiResponse<LookupItem[]>>('v1/lookups/variantAttributes');
        return response.data;
    },

    getRoles: async (): Promise<ApiResponse<LookupItem[]>> => {
        const response = await api.get<ApiResponse<LookupItem[]>>('v1/lookups/roles');
        return response.data;
    },

    getProductPublishStatuses: async (): Promise<ApiResponse<LookupItem[]>> => {
        const response = await api.get<ApiResponse<LookupItem[]>>('v1/lookups/productPublishStatuses');
        return response.data;
    },

    getProductVisibilities: async (): Promise<ApiResponse<LookupItem[]>> => {
        const response = await api.get<ApiResponse<LookupItem[]>>('v1/lookups/productVisibilities');
        return response.data;
    },
};
