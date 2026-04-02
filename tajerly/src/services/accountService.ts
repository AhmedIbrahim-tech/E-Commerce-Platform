import api from '@/config/api';
import { User, Address, UpdateProfileDto, ChangePasswordDto, CreateAddressDto, UpdateAddressDto } from '@/types/user';
import { ApiResponse } from '@/types/api';

export const accountService = {
    getProfile: async (): Promise<ApiResponse<User>> => {
        // Backend: api/v1/user/profile
        const response = await api.get<ApiResponse<User>>('v1/user/profile');
        return response.data;
    },

    updateProfile: async (data: UpdateProfileDto): Promise<ApiResponse<User>> => {
        // Backend: api/v1/user/profile/edit (Assuming there's an edit endpoint or it's just put on profile)
        const response = await api.put<ApiResponse<User>>('v1/user/profile', data);
        return response.data;
    },

    changePassword: async (data: ChangePasswordDto): Promise<ApiResponse<boolean>> => {
        // Backend: api/v1/authenticate/changePassword
        const response = await api.post<ApiResponse<boolean>>('v1/authenticate/changePassword', data);
        return response.data;
    },

    // ── Shipping Addresses ───────────────────────────────────────────
    getAddresses: async (): Promise<ApiResponse<Address[]>> => {
        // Backend: api/v1/shippingAddress/getAll
        const response = await api.get<ApiResponse<Address[]>>('v1/shippingAddress/getAll');
        return response.data;
    },

    getAddressById: async (id: number): Promise<ApiResponse<Address>> => {
        // Backend: api/v1/shippingAddress/{id}
        const response = await api.get<ApiResponse<Address>>(`v1/shippingAddress/${id}`);
        return response.data;
    },

    createAddress: async (data: CreateAddressDto): Promise<ApiResponse<Address>> => {
        // Backend: api/v1/shippingAddress/create
        const response = await api.post<ApiResponse<Address>>('v1/shippingAddress/create', data);
        return response.data;
    },

    updateAddress: async (id: number, data: UpdateAddressDto): Promise<ApiResponse<Address>> => {
        // Backend: api/v1/shippingAddress/edit
        const response = await api.put<ApiResponse<Address>>('v1/shippingAddress/edit', data);
        return response.data;
    },

    deleteAddress: async (id: number): Promise<ApiResponse<void>> => {
        // Backend: api/v1/shippingAddress/{id}
        const response = await api.delete<ApiResponse<void>>(`v1/shippingAddress/${id}`);
        return response.data;
    },

    setDefaultAddress: async (id: number): Promise<ApiResponse<Address>> => {
        // Backend: api/v1/shippingAddress/setShippingAddress (usually takes an ID)
        const response = await api.post<ApiResponse<Address>>('v1/shippingAddress/setShippingAddress', { addressId: id });
        return response.data;
    },
};
