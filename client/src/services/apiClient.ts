import axios, { AxiosError, InternalAxiosRequestConfig } from 'axios';
import { API_URL } from '@/config/apiConfig';
import { Router } from '@/config/Router';
import { store } from '@/store/store';
import { logout } from '@/store/slices/authSlice';
import { ApiResponse } from '@/types/App/ApiResponse';
import { LoginApiResponse } from '@/types/auth/auth';

const baseURL = `${API_URL}/api/v1`;

export const apiClient = axios.create({
    baseURL,
    headers: { 'Content-Type': 'application/json' },
});

let isRefreshing = false;
let failedQueue: { resolve: (value?: unknown) => void; reject: (reason?: unknown) => void }[] = [];

const processQueue = (error: Error | null, newAccessToken: string | null = null) => {
    failedQueue.forEach((prom) => {
        if (error) prom.reject(error);
        else prom.resolve(newAccessToken);
    });
    failedQueue = [];
};

apiClient.interceptors.request.use(
    (config: InternalAxiosRequestConfig) => {
        if (typeof window === 'undefined') return config;
        const accessToken = localStorage.getItem('accessToken');
        if (accessToken) {
            config.headers.Authorization = `Bearer ${accessToken}`;
        }
        return config;
    },
    (error) => Promise.reject(error)
);

apiClient.interceptors.response.use(
    (response) => response,
    async (error: AxiosError) => {
        const originalRequest = error.config as InternalAxiosRequestConfig & { _retry?: boolean };

        if (error.response?.status !== 401 || originalRequest._retry) {
            return Promise.reject(error);
        }

        if (isRefreshing) {
            return new Promise((resolve, reject) => {
                failedQueue.push({ resolve, reject });
            }).then(() => apiClient(originalRequest));
        }

        const refreshTokenValue = typeof window !== 'undefined' ? localStorage.getItem('refreshToken') : null;
        const accessTokenValue = typeof window !== 'undefined' ? localStorage.getItem('accessToken') : null;

        if (!refreshTokenValue || !accessTokenValue) {
            store.dispatch(logout());
            if (typeof window !== 'undefined') window.location.href = '/login';
            return Promise.reject(error);
        }

        originalRequest._retry = true;
        isRefreshing = true;

        try {
            const response = await axios.post<ApiResponse<LoginApiResponse>>(
                Router.Authentication.RefreshToken,
                { accessToken: accessTokenValue, refreshToken: refreshTokenValue }
            );
            const data = response.data?.data;
            if (data?.accessToken && data?.refreshToken?.tokenString) {
                if (typeof window !== 'undefined') {
                    localStorage.setItem('accessToken', data.accessToken);
                    localStorage.setItem('refreshToken', data.refreshToken.tokenString);
                }
                processQueue(null, data.accessToken);
                originalRequest.headers.Authorization = `Bearer ${data.accessToken}`;
                return apiClient(originalRequest);
            }
            throw new Error('Invalid refresh response');
        } catch (refreshError) {
            processQueue(refreshError as Error, null);
            store.dispatch(logout());
            if (typeof window !== 'undefined') window.location.href = '/login';
            return Promise.reject(refreshError);
        } finally {
            isRefreshing = false;
        }
    }
);
