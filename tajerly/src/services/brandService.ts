import api from '@/config/api';
import { Brand, CreateBrandDto, UpdateBrandDto } from '@/types/brand';
import { ApiResponse, PagedResult } from '@/types/api';

export const brandService = {
    getAllBrands: async (activeOnly = false): Promise<ApiResponse<Brand[]>> => {
        // Backend: api/v1/brand/getAll?activeOnly={activeOnly}
        const response = await api.get<ApiResponse<Brand[]>>('v1/brand/getAll', {
            params: { activeOnly }
        });
        return response.data;
    },

    getBrandsPaged: async (page = 1, pageSize = 20, searchTerm?: string): Promise<ApiResponse<PagedResult<Brand>>> => {
        // Backend: api/v1/brand/paginated
        const response = await api.get<ApiResponse<PagedResult<Brand>>>('v1/brand/paginated', {
            params: { page, pageSize, searchTerm }
        });
        return response.data;
    },

    getBrandById: async (id: number): Promise<ApiResponse<Brand>> => {
        // Backend: api/v1/brand/{id}
        const response = await api.get<ApiResponse<Brand>>(`v1/brand/${id}`);
        return response.data;
    },

    createBrand: async (data: CreateBrandDto): Promise<ApiResponse<Brand>> => {
        const response = await api.post<ApiResponse<Brand>>('v1/brand/create', data);
        return response.data;
    },

    updateBrand: async (id: number, data: UpdateBrandDto): Promise<ApiResponse<Brand>> => {
        const response = await api.put<ApiResponse<Brand>>('v1/brand/edit', data);
        return response.data;
    },

    deleteBrand: async (id: number): Promise<ApiResponse<void>> => {
        const response = await api.delete<ApiResponse<void>>(`v1/brand/${id}`);
        return response.data;
    }
};
