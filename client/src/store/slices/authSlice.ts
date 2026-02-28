import { createSlice, createAsyncThunk, PayloadAction } from '@reduxjs/toolkit';
import axios, { AxiosError } from 'axios';
import { Router } from '@/config/Router';
import { AuthUser, LoginCredentials, RegisterCredentials, ConfirmEmailCredentials, LoginApiResponse, ForgotPasswordCredentials, ConfirmResetPasswordCredentials, ResetPasswordCredentials } from '@/types/auth/auth';
import { ApiResponse } from '@/types/App/ApiResponse';
import { ApiError } from '@/types/App/ApiError';
import { initialState } from '@/types/auth/AuthState';

export const registerUser = createAsyncThunk(
    'auth/registerUser',
    async (credentials: RegisterCredentials, { rejectWithValue }) => {
        try {
            const response = await axios.post<ApiResponse<string>>(
                Router.Authentication.Register,
                credentials
            );
            return response.data;
        } catch (error: unknown) {
            if (axios.isAxiosError(error)) {
                const axiosError = error as AxiosError<ApiError>;
                if (axiosError.response && axiosError.response.data) {
                    const errorData = axiosError.response.data;
                    return rejectWithValue(
                        errorData.message ||
                        errorData.detail ||
                        errorData.title ||
                        'Registration failed'
                    );
                }
            }
            return rejectWithValue('Network error or server is down');
        }
    }
);

export const confirmEmail = createAsyncThunk(
    'auth/confirmEmail',
    async (credentials: ConfirmEmailCredentials, { rejectWithValue }) => {
        try {
            const response = await axios.post<ApiResponse<string>>(
                Router.Authentication.ConfirmEmail,
                credentials
            );
            return response.data;
        } catch (error: unknown) {
            if (axios.isAxiosError(error)) {
                const axiosError = error as AxiosError<ApiError>;
                if (axiosError.response && axiosError.response.data) {
                    const errorData = axiosError.response.data;
                    return rejectWithValue(
                        errorData.message ||
                        errorData.detail ||
                        errorData.title ||
                        'Email confirmation failed'
                    );
                }
            }
            return rejectWithValue('Network error or server is down');
        }
    }
);

export const loginUser = createAsyncThunk(
    'auth/loginUser',
    async (credentials: LoginCredentials, { rejectWithValue }) => {
        try {
            const response = await axios.post<ApiResponse<LoginApiResponse>>(
                Router.Authentication.SignIn,
                credentials
            );

            const data = response.data;
            if (data?.succeeded && data?.data) {
                const raw = data.data;
                const user: AuthUser = {
                    accessToken: raw.accessToken,
                    refreshToken: raw.refreshToken?.tokenString,
                    roles: raw.roles ?? []
                };
                if (typeof window !== 'undefined') {
                    localStorage.setItem('accessToken', raw.accessToken);
                    if (raw.refreshToken?.tokenString) {
                        localStorage.setItem('refreshToken', raw.refreshToken.tokenString);
                    }
                }
                return user;
            }

            return rejectWithValue(data?.message || 'Login failed');
        } catch (error: unknown) {
            if (axios.isAxiosError(error)) {
                const axiosError = error as AxiosError<ApiError>;
                if (axiosError.response && axiosError.response.data) {
                    const errorData = axiosError.response.data;
                    return rejectWithValue(
                        errorData.message ||
                        errorData.detail ||
                        errorData.title ||
                        'Login failed'
                    );
                }
            }
            return rejectWithValue('Network error or server is down');
        }
    }
);

export const validateToken = createAsyncThunk(
    'auth/validateToken',
    async (_, { rejectWithValue }) => {
        const accessToken = typeof window !== 'undefined' ? localStorage.getItem('accessToken') : null;
        if (!accessToken) return rejectWithValue('No token');
        try {
            const response = await axios.post<ApiResponse<string>>(
                Router.Authentication.ValidateToken,
                { accessToken },
                { headers: { Authorization: `Bearer ${accessToken}` } }
            );
            if (response.data?.succeeded) return true;
            return rejectWithValue('Invalid token');
        } catch (error: unknown) {
            if (axios.isAxiosError(error)) {
                const axiosError = error as AxiosError<ApiError>;
                if (axiosError.response?.data) {
                    const err = axiosError.response.data as { message?: string };
                    return rejectWithValue(err.message || 'Invalid token');
                }
            }
            return rejectWithValue('Invalid token');
        }
    }
);

export const sendResetPasswordCode = createAsyncThunk(
    'auth/sendResetPasswordCode',
    async (credentials: ForgotPasswordCredentials, { rejectWithValue }) => {
        try {
            const response = await axios.post<ApiResponse<string>>(
                Router.Authentication.SendResetPasswordCode,
                { email: credentials.email }
            );
            return response.data;
        } catch (error: unknown) {
            if (axios.isAxiosError(error)) {
                const err = error.response?.data as { message?: string };
                return rejectWithValue(err?.message || 'Failed to send code');
            }
            return rejectWithValue('Failed to send code');
        }
    }
);

export const confirmResetPasswordCode = createAsyncThunk(
    'auth/confirmResetPasswordCode',
    async (credentials: ConfirmResetPasswordCredentials, { rejectWithValue }) => {
        try {
            const response = await axios.post<ApiResponse<string>>(
                Router.Authentication.ConfirmResetPasswordCode,
                { code: credentials.code, email: credentials.email }
            );
            return response.data;
        } catch (error: unknown) {
            if (axios.isAxiosError(error)) {
                const err = error.response?.data as { message?: string };
                return rejectWithValue(err?.message || 'Invalid code');
            }
            return rejectWithValue('Invalid code');
        }
    }
);

export const resetPassword = createAsyncThunk(
    'auth/resetPassword',
    async (credentials: ResetPasswordCredentials, { rejectWithValue }) => {
        try {
            const response = await axios.post<ApiResponse<string>>(
                Router.Authentication.ResetPassword,
                { email: credentials.email, newPassword: credentials.newPassword, confirmPassword: credentials.confirmPassword }
            );
            return response.data;
        } catch (error: unknown) {
            if (axios.isAxiosError(error)) {
                const err = error.response?.data as { message?: string };
                return rejectWithValue(err?.message || 'Failed to reset password');
            }
            return rejectWithValue('Failed to reset password');
        }
    }
);

export const twoStepVerification = createAsyncThunk(
    'auth/twoStepVerification',
    async (params: { code: string; email?: string }, { rejectWithValue }) => {
        try {
            const response = await axios.post<ApiResponse<string>>(
                Router.Authentication.TwoStepVerification,
                { code: params.code, email: params.email ?? null }
            );
            if (response.data?.succeeded) return true;
            return rejectWithValue('Verification failed');
        } catch (error: unknown) {
            if (axios.isAxiosError(error)) {
                const err = error.response?.data as { message?: string };
                return rejectWithValue(err?.message || 'Verification failed');
            }
            return rejectWithValue('Verification failed');
        }
    }
);

export const refreshToken = createAsyncThunk(
    'auth/refreshToken',
    async (_, { rejectWithValue }) => {
        const accessToken = typeof window !== 'undefined' ? localStorage.getItem('accessToken') : null;
        const refreshTokenValue = typeof window !== 'undefined' ? localStorage.getItem('refreshToken') : null;
        if (!accessToken || !refreshTokenValue) {
            return rejectWithValue('No tokens');
        }
        try {
            const response = await axios.post<ApiResponse<LoginApiResponse>>(
                Router.Authentication.RefreshToken,
                { accessToken, refreshToken: refreshTokenValue }
            );
            const data = response.data?.data;
            if (data?.accessToken && data?.refreshToken?.tokenString) {
                if (typeof window !== 'undefined') {
                    localStorage.setItem('accessToken', data.accessToken);
                    localStorage.setItem('refreshToken', data.refreshToken.tokenString);
                }
                return {
                    accessToken: data.accessToken,
                    refreshToken: data.refreshToken.tokenString,
                    roles: data.roles ?? []
                } as AuthUser;
            }
            return rejectWithValue('Invalid refresh response');
        } catch (error: unknown) {
            if (axios.isAxiosError(error)) {
                const axiosError = error as AxiosError<ApiError>;
                if (axiosError.response?.data) {
                    const err = axiosError.response.data as { message?: string };
                    return rejectWithValue(err.message || 'Refresh failed');
                }
            }
            return rejectWithValue('Refresh failed');
        }
    }
);

const authSlice = createSlice({
    name: 'auth',
    initialState,
    reducers: {
        logout: (state) => {
            state.accessToken = null;
            state.user = null;
            state.error = null;
            if (typeof window !== 'undefined') {
                localStorage.removeItem('accessToken');
                localStorage.removeItem('refreshToken');
            }
        },
        clearError: (state) => {
            state.error = null;
        }
    },
    extraReducers: (builder) => {
        builder
            .addCase(loginUser.pending, (state) => {
                state.loading = true;
                state.error = null;
            })
            .addCase(loginUser.fulfilled, (state, action: PayloadAction<AuthUser>) => {
                state.loading = false;
                state.accessToken = action.payload.accessToken;
                state.user = action.payload;
            })
            .addCase(loginUser.rejected, (state, action: PayloadAction<string | unknown>) => {
                state.loading = false;
                state.error = action.payload as string;
            })
            // Register
            .addCase(registerUser.pending, (state) => {
                state.registerLoading = true;
                state.registerError = null;
            })
            .addCase(registerUser.fulfilled, (state) => {
                state.registerLoading = false;
            })
            .addCase(registerUser.rejected, (state, action: PayloadAction<string | unknown>) => {
                state.registerLoading = false;
                state.registerError = action.payload as string;
            })
            // Confirm Email
            .addCase(confirmEmail.pending, (state) => {
                state.confirmEmailLoading = true;
                state.confirmEmailError = null;
            })
            .addCase(confirmEmail.fulfilled, (state) => {
                state.confirmEmailLoading = false;
            })
            .addCase(confirmEmail.rejected, (state, action: PayloadAction<string | unknown>) => {
                state.confirmEmailLoading = false;
                state.confirmEmailError = action.payload as string;
            })
            .addCase(refreshToken.fulfilled, (state, action: PayloadAction<AuthUser>) => {
                state.accessToken = action.payload.accessToken;
                state.user = action.payload;
            });
    },
});

export const logoutAsync = createAsyncThunk(
    'auth/logoutAsync',
    async (_, { dispatch }) => {
        const refreshTokenValue = typeof window !== 'undefined' ? localStorage.getItem('refreshToken') : null;
        const accessToken = typeof window !== 'undefined' ? localStorage.getItem('accessToken') : null;
        if (refreshTokenValue && accessToken) {
            try {
                await axios.post(
                    Router.Authentication.Logout,
                    { refreshToken: refreshTokenValue },
                    { headers: { Authorization: `Bearer ${accessToken}` } }
                );
            } catch {
                // ignore network errors on logout
            }
        }
        dispatch(logout());
    }
);

export const logoutAllAsync = createAsyncThunk(
    'auth/logoutAllAsync',
    async (_, { dispatch }) => {
        const accessToken = typeof window !== 'undefined' ? localStorage.getItem('accessToken') : null;
        if (accessToken) {
            try {
                await axios.post(
                    Router.Authentication.LogoutAll,
                    {},
                    { headers: { Authorization: `Bearer ${accessToken}` } }
                );
            } catch {
                // ignore network errors
            }
        }
        dispatch(logout());
    }
);

export const { logout, clearError } = authSlice.actions;

export default authSlice.reducer;
