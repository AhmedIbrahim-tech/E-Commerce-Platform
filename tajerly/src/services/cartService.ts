import api from '@/config/api';
import { Cart, AddToCartDto, UpdateCartItemDto } from '@/types/cart';
import { ApiResponse } from '@/types/api';

export const cartService = {
    getCart: async (): Promise<ApiResponse<Cart>> => {
        // Backend: api/v1/cart/myCart
        const response = await api.get<ApiResponse<Cart>>('v1/cart/myCart');
        return response.data;
    },

    addToCart: async (data: AddToCartDto): Promise<ApiResponse<Cart>> => {
        // Backend: api/v1/cart/addToCart
        const response = await api.post<ApiResponse<Cart>>('v1/cart/addToCart', data);
        return response.data;
    },

    updateCartItem: async (itemId: number, data: UpdateCartItemDto): Promise<ApiResponse<Cart>> => {
        // Backend: api/v1/cart/updateItemQuantity
        const response = await api.put<ApiResponse<Cart>>('v1/cart/updateItemQuantity', {
            cartItemId: itemId,
            quantity: data.quantity
        });
        return response.data;
    },

    removeCartItem: async (itemId: number): Promise<ApiResponse<Cart>> => {
        // Backend: api/v1/cart/removeFromCart/{itemId}
        const response = await api.delete<ApiResponse<Cart>>(`v1/cart/removeFromCart/${itemId}`);
        return response.data;
    },

    clearCart: async (): Promise<ApiResponse<void>> => {
        // Backend: api/v1/cart/{id}
        const response = await api.delete<ApiResponse<void>>('v1/cart/delete');
        return response.data;
    },
};
