import api from '@/config/api';
import { ApiResponse } from '@/types/api';

export interface LoginRequest {
    email: string;
    password: string;
}

export interface RegisterRequest {
    firstName: string;
    lastName: string;
    userName: string;
    email: string;
    password: string;
    confirmPassword: string;
    phone?: string;
}

// Matches the backend JwtAuthResponse
export interface RefreshToken {
    userName: string;
    tokenString: string;
    expireAt: string;
}

export interface AuthResponseData {
    accessToken: string;
    refreshToken: RefreshToken;
    roles: string[];
}

export interface GoogleLoginRequest {
    idToken: string;
}

export interface ConfirmEmailRequest {
    userId: string;
    code: string;
}

export interface SendResetPasswordRequest {
    email: string;
}

export interface ConfirmResetPasswordRequest {
    email: string;
    code: string;
}

export interface ResetPasswordRequest {
    email: string;
    password: string;
    confirmPassword: string;
}

export interface TwoStepVerificationRequest {
    code: string;
    email?: string;
}

export interface ChangePasswordRequest {
    currentPassword: string;
    newPassword: string;
    confirmPassword: string;
}

export interface ValidateTokenRequest {
    accessToken: string;
}

export const authService = {
    login: async (data: LoginRequest): Promise<ApiResponse<AuthResponseData>> => {
        // Backend: api/v1/authenticate/signIn
        const response = await api.post<ApiResponse<AuthResponseData>>('v1/authenticate/signIn', data);
        return response.data;
    },

    register: async (data: RegisterRequest): Promise<ApiResponse<string>> => {
        // Backend: api/v1/authenticate/register
        const response = await api.post<ApiResponse<string>>('v1/authenticate/register', data);
        return response.data;
    },

    signInViaGoogle: async (data: GoogleLoginRequest): Promise<ApiResponse<AuthResponseData>> => {
        // Backend: api/v1/authenticate/signInViaGoogle
        const response = await api.post<ApiResponse<AuthResponseData>>('v1/authenticate/signInViaGoogle', data);
        return response.data;
    },

    refresh: async (refreshToken: string): Promise<ApiResponse<AuthResponseData>> => {
        // Backend: api/v1/authenticate/refreshToken
        const response = await api.post<ApiResponse<AuthResponseData>>('v1/authenticate/refreshToken', { refreshToken });
        return response.data;
    },

    validateToken: async (accessToken: string): Promise<ApiResponse<string>> => {
        // Backend: api/v1/authenticate/validateToken
        const response = await api.post<ApiResponse<string>>('v1/authenticate/validateToken', { accessToken });
        return response.data;
    },

    sendResetPasswordCode: async (email: string): Promise<ApiResponse<string>> => {
        // Backend: api/v1/authenticate/sendResetPasswordCode
        const response = await api.post<ApiResponse<string>>('v1/authenticate/sendResetPasswordCode', { email });
        return response.data;
    },

    confirmResetPasswordCode: async (data: ConfirmResetPasswordRequest): Promise<ApiResponse<string>> => {
        // Backend: api/v1/authenticate/confirmResetPasswordCode
        const response = await api.post<ApiResponse<string>>('v1/authenticate/confirmResetPasswordCode', data);
        return response.data;
    },

    resetPassword: async (data: ResetPasswordRequest): Promise<ApiResponse<string>> => {
        // Backend: api/v1/authenticate/resetPassword
        const response = await api.post<ApiResponse<string>>('v1/authenticate/resetPassword', data);
        return response.data;
    },

    confirmEmail: async (data: ConfirmEmailRequest): Promise<ApiResponse<string>> => {
        // Backend: api/authenticate/confirmEmail (No v1 for this path in Router.cs)
        const response = await api.post<ApiResponse<string>>('authenticate/confirmEmail', data);
        return response.data;
    },

    twoStepVerification: async (data: TwoStepVerificationRequest): Promise<ApiResponse<string>> => {
        // Backend: api/v1/authenticate/twoStepVerification
        const response = await api.post<ApiResponse<string>>('v1/authenticate/twoStepVerification', data);
        return response.data;
    },

    logout: async (refreshToken: string): Promise<void> => {
        // Backend: api/v1/authenticate/logout
        await api.post('v1/authenticate/logout', { refreshToken });
    },

    logoutAll: async (): Promise<void> => {
        // Backend: api/v1/authenticate/logoutAll
        await api.post('v1/authenticate/logoutAll');
    },

    changePassword: async (data: ChangePasswordRequest): Promise<ApiResponse<string>> => {
        // Backend: api/v1/authenticate/changePassword
        const response = await api.post<ApiResponse<string>>('v1/authenticate/changePassword', data);
        return response.data;
    }
};
