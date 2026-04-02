import api from '@/config/api';
import { Order, CreateOrderDto, UpdateOrderStatusDto } from '@/types/order';
import { ApiResponse, PagedResult } from '@/types/api';

export const orderService = {
    /** Get current user's orders (paged). */
    getMyOrders: async (page = 1, pageSize = 10): Promise<ApiResponse<PagedResult<Order>>> => {
        // Backend: api/v1/order/getMyOrders
        const response = await api.get<ApiResponse<PagedResult<Order>>>('v1/order/getMyOrders', {
            params: { page, pageSize }
        });
        return response.data;
    },

    getOrderById: async (id: number): Promise<ApiResponse<Order>> => {
        // Backend: api/v1/order/{id}
        const response = await api.get<ApiResponse<Order>>(`v1/order/${id}`);
        return response.data;
    },

    createOrder: async (data: CreateOrderDto): Promise<ApiResponse<Order>> => {
        // Backend: api/v1/order/create
        const response = await api.post<ApiResponse<Order>>('v1/order/create', data);
        return response.data;
    },

    /** Admin: get all orders (paged). */
    getAllOrders: async (page = 1, pageSize = 20, status?: string): Promise<ApiResponse<PagedResult<Order>>> => {
        // Backend: api/v1/order/paginated
        const response = await api.get<ApiResponse<PagedResult<Order>>>('v1/order/paginated', {
            params: { page, pageSize, status: status ?? undefined }
        });
        return response.data;
    },

    placeOrder: async (id: number): Promise<ApiResponse<Order>> => {
        // Backend: api/v1/order/placeOrder/{id}
        const response = await api.post<ApiResponse<Order>>(`v1/order/placeOrder/${id}`);
        return response.data;
    },
};
