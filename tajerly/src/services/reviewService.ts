import api from '@/config/api';
import { ApiResponse, PagedResult } from '@/types/api';

export interface Review {
    id: number;
    productId: number;
    userId: string;
    userName: string;
    rating: number;
    comment: string;
    createdAt: string;
}

export interface CreateReviewDto {
    productId: number;
    rating: number;
    comment: string;
}

export interface UpdateReviewDto {
    rating: number;
    comment: string;
}

export const reviewService = {
    getReviewsPaged: async (productId: number, page = 1, pageSize = 10): Promise<ApiResponse<PagedResult<Review>>> => {
        // Backend: api/v1/review/paginated
        const response = await api.get<ApiResponse<PagedResult<Review>>>('v1/review/paginated', {
            params: { productId, page, pageSize }
        });
        return response.data;
    },

    createReview: async (data: CreateReviewDto): Promise<ApiResponse<Review>> => {
        // Backend: api/v1/review/create
        const response = await api.post<ApiResponse<Review>>('v1/review/create', data);
        return response.data;
    },

    updateReview: async (data: UpdateReviewDto): Promise<ApiResponse<Review>> => {
        // Backend: api/v1/review/edit
        const response = await api.put<ApiResponse<Review>>('v1/review/edit', data);
        return response.data;
    },

    deleteReview: async (id: number): Promise<ApiResponse<void>> => {
        // Backend: api/v1/review/{id}
        const response = await api.delete<ApiResponse<void>>(`v1/review/${id}`);
        return response.data;
    },
};
