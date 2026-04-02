import api from '@/config/api';
import { WishlistItem } from '@/types/wishlist';
import { ApiResponse } from '@/types/api';

export const wishlistService = {
    getWishlist: async (): Promise<ApiResponse<WishlistItem[]>> => {
        const response = await api.get<ApiResponse<WishlistItem[]>>('/wishlist');
        return response.data;
    },

    addToWishlist: async (productId: number): Promise<ApiResponse<WishlistItem>> => {
        const response = await api.post<ApiResponse<WishlistItem>>('/wishlist', { productId });
        return response.data;
    },

    removeFromWishlist: async (productId: number): Promise<ApiResponse<void>> => {
        const response = await api.delete<ApiResponse<void>>(`/wishlist/${productId}`);
        return response.data;
    },
};
