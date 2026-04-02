import api from '@/config/api';
import { User, CreateUserDto, UpdateUserDto } from '@/types/user';
import { ApiResponse, PagedResult } from '@/types/api';

export interface AdminDashboardStatsDto {
    totalProducts: number;
    totalOrders: number;
    totalUsers: number;
    totalRevenue: number;
    pendingOrders: number;
    recentOrders: number;
    productsByCategory: { categoryName: string; productCount: number }[];
    revenueByMonth: { month: string; revenue: number }[];
}

export const adminService = {
    getDashboardStats: async (): Promise<ApiResponse<AdminDashboardStatsDto>> => {
        // Backend doesn't have a direct Stats endpoint in Router.cs, but often exists.
        // For now using a placeholder or common path.
        const response = await api.get<ApiResponse<AdminDashboardStatsDto>>('v1/admin/stats');
        return response.data;
    },

    // ── User Management (via UserRouting) ────────────────────────────
    getAllUsers: async (page = 1, pageSize = 20): Promise<ApiResponse<PagedResult<User>>> => {
        // Backend: api/v1/user/users/paginated
        const response = await api.get<ApiResponse<PagedResult<User>>>('v1/user/users/paginated', {
            params: { page, pageSize }
        });
        return response.data;
    },

    getUserById: async (id: number): Promise<ApiResponse<User>> => {
        // Backend: api/v1/user/users/{id}
        const response = await api.get<ApiResponse<User>>(`v1/user/users/${id}`);
        return response.data;
    },

    createUser: async (data: CreateUserDto): Promise<ApiResponse<User>> => {
        // Backend: api/v1/user/users/create
        const response = await api.post<ApiResponse<User>>('v1/user/users/create', data);
        return response.data;
    },

    updateUser: async (id: number, data: UpdateUserDto): Promise<ApiResponse<User>> => {
        // Backend: api/v1/user/users/edit
        const response = await api.put<ApiResponse<User>>('v1/user/users/edit', data);
        return response.data;
    },

    deleteUser: async (id: number): Promise<ApiResponse<boolean>> => {
        // Backend: api/v1/user/users/{id}
        const response = await api.delete<ApiResponse<boolean>>(`v1/user/users/${id}`);
        return response.data;
    },

    toggleUserStatus: async (id: number): Promise<ApiResponse<User>> => {
        // Backend: api/v1/user/users/{id}/toggleStatus
        const response = await api.patch<ApiResponse<User>>(`v1/user/users/${id}/toggleStatus`);
        return response.data;
    },
};
