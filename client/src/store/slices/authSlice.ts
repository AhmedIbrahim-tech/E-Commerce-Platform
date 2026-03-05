import { createSlice, createAsyncThunk, PayloadAction } from '@reduxjs/toolkit';
import axios, { AxiosError } from 'axios';
import { Router } from '@/config/Router';
import {
    AuthUser,
    LoginCredentials,
    RegisterCredentials,
    ConfirmEmailCredentials,
    LoginApiResponse,
    ForgotPasswordCredentials,
    ConfirmResetPasswordCredentials,
    ResetPasswordCredentials,
    ChangePasswordCredentials,
    TwoStepVerificationCredentials,
} from '@/types/auth/auth';
import { ApiResponse } from '@/types/App/ApiResponse';
import { ApiError } from '@/types/App/ApiError';
import { initialState } from '@/types/auth/AuthState';


// ── Helper: extract error message from Axios errors ─────────────────
function extractError(error: unknown, fallback: string): string {
    if (axios.isAxiosError(error)) {
        const axiosError = error as AxiosError<ApiError>;
        const data = axiosError.response?.data;
        if (data) {
            return data.message || data.detail || data.title || fallback;
        }
    }
    return 'Network error or server is down';
}

// ── Async Thunks ────────────────────────────────────────────────────

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
            return rejectWithValue(extractError(error, 'Registration failed'));
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
            return rejectWithValue(extractError(error, 'Email confirmation failed'));
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
                    roles: raw.roles ?? [],
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
            return rejectWithValue(extractError(error, 'Login failed'));
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
            return rejectWithValue(extractError(error, 'Invalid token'));
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
            return rejectWithValue(extractError(error, 'Failed to send code'));
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
            return rejectWithValue(extractError(error, 'Invalid code'));
        }
    }
);

export const resetPassword = createAsyncThunk(
    'auth/resetPassword',
    async (credentials: ResetPasswordCredentials, { rejectWithValue }) => {
        try {
            const response = await axios.post<ApiResponse<string>>(
                Router.Authentication.ResetPassword,
                {
                    email: credentials.email,
                    newPassword: credentials.newPassword,
                    confirmPassword: credentials.confirmPassword,
                }
            );
            return response.data;
        } catch (error: unknown) {
            return rejectWithValue(extractError(error, 'Failed to reset password'));
        }
    }
);

export const twoStepVerification = createAsyncThunk(
    'auth/twoStepVerification',
    async (params: TwoStepVerificationCredentials, { rejectWithValue }) => {
        try {
            const response = await axios.post<ApiResponse<string>>(
                Router.Authentication.TwoStepVerification,
                { code: params.code, email: params.email ?? null }
            );
            if (response.data?.succeeded) return true;
            return rejectWithValue('Verification failed');
        } catch (error: unknown) {
            return rejectWithValue(extractError(error, 'Verification failed'));
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
                    roles: data.roles ?? [],
                } as AuthUser;
            }
            return rejectWithValue('Invalid refresh response');
        } catch (error: unknown) {
            return rejectWithValue(extractError(error, 'Refresh failed'));
        }
    }
);

export const changePassword = createAsyncThunk(
    'auth/changePassword',
    async (credentials: ChangePasswordCredentials, { rejectWithValue }) => {
        const accessToken = typeof window !== 'undefined' ? localStorage.getItem('accessToken') : null;
        if (!accessToken) return rejectWithValue('Not authenticated');
        try {
            const response = await axios.put<ApiResponse<string>>(
                Router.Authentication.ChangePassword,
                credentials,
                { headers: { Authorization: `Bearer ${accessToken}` } }
            );
            return response.data;
        } catch (error: unknown) {
            return rejectWithValue(extractError(error, 'Failed to change password'));
        }
    }
);

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

// ── Slice ───────────────────────────────────────────────────────────

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
            state.registerError = null;
            state.confirmEmailError = null;
            state.forgotPasswordError = null;
            state.confirmResetCodeError = null;
            state.resetPasswordError = null;
            state.twoStepError = null;
            state.changePasswordError = null;
        },
    },
    extraReducers: (builder) => {
        builder
            // ── Login ───────────────────────────────────────────
            .addCase(loginUser.pending, (state) => {
                state.loading = true;
                state.error = null;
            })
            .addCase(loginUser.fulfilled, (state, action: PayloadAction<AuthUser>) => {
                state.loading = false;
                state.accessToken = action.payload.accessToken;
                state.user = action.payload;
            })
            .addCase(loginUser.rejected, (state, action) => {
                state.loading = false;
                state.error = action.payload as string;
            })

            // ── Register ────────────────────────────────────────
            .addCase(registerUser.pending, (state) => {
                state.registerLoading = true;
                state.registerError = null;
            })
            .addCase(registerUser.fulfilled, (state) => {
                state.registerLoading = false;
            })
            .addCase(registerUser.rejected, (state, action) => {
                state.registerLoading = false;
                state.registerError = action.payload as string;
            })

            // ── Confirm Email ───────────────────────────────────
            .addCase(confirmEmail.pending, (state) => {
                state.confirmEmailLoading = true;
                state.confirmEmailError = null;
            })
            .addCase(confirmEmail.fulfilled, (state) => {
                state.confirmEmailLoading = false;
            })
            .addCase(confirmEmail.rejected, (state, action) => {
                state.confirmEmailLoading = false;
                state.confirmEmailError = action.payload as string;
            })

            // ── Forgot Password (send code) ─────────────────────
            .addCase(sendResetPasswordCode.pending, (state) => {
                state.forgotPasswordLoading = true;
                state.forgotPasswordError = null;
            })
            .addCase(sendResetPasswordCode.fulfilled, (state) => {
                state.forgotPasswordLoading = false;
            })
            .addCase(sendResetPasswordCode.rejected, (state, action) => {
                state.forgotPasswordLoading = false;
                state.forgotPasswordError = action.payload as string;
            })

            // ── Confirm Reset Code ──────────────────────────────
            .addCase(confirmResetPasswordCode.pending, (state) => {
                state.confirmResetCodeLoading = true;
                state.confirmResetCodeError = null;
            })
            .addCase(confirmResetPasswordCode.fulfilled, (state) => {
                state.confirmResetCodeLoading = false;
            })
            .addCase(confirmResetPasswordCode.rejected, (state, action) => {
                state.confirmResetCodeLoading = false;
                state.confirmResetCodeError = action.payload as string;
            })

            // ── Reset Password ──────────────────────────────────
            .addCase(resetPassword.pending, (state) => {
                state.resetPasswordLoading = true;
                state.resetPasswordError = null;
            })
            .addCase(resetPassword.fulfilled, (state) => {
                state.resetPasswordLoading = false;
            })
            .addCase(resetPassword.rejected, (state, action) => {
                state.resetPasswordLoading = false;
                state.resetPasswordError = action.payload as string;
            })

            // ── Two Step Verification ───────────────────────────
            .addCase(twoStepVerification.pending, (state) => {
                state.twoStepLoading = true;
                state.twoStepError = null;
            })
            .addCase(twoStepVerification.fulfilled, (state) => {
                state.twoStepLoading = false;
            })
            .addCase(twoStepVerification.rejected, (state, action) => {
                state.twoStepLoading = false;
                state.twoStepError = action.payload as string;
            })

            // ── Validate Token ──────────────────────────────────
            .addCase(validateToken.pending, (state) => {
                state.validateTokenLoading = true;
            })
            .addCase(validateToken.fulfilled, (state) => {
                state.validateTokenLoading = false;
            })
            .addCase(validateToken.rejected, (state) => {
                state.validateTokenLoading = false;
            })

            // ── Change Password ─────────────────────────────────
            .addCase(changePassword.pending, (state) => {
                state.changePasswordLoading = true;
                state.changePasswordError = null;
            })
            .addCase(changePassword.fulfilled, (state) => {
                state.changePasswordLoading = false;
            })
            .addCase(changePassword.rejected, (state, action) => {
                state.changePasswordLoading = false;
                state.changePasswordError = action.payload as string;
            })

            // ── Refresh Token ───────────────────────────────────
            .addCase(refreshToken.fulfilled, (state, action: PayloadAction<AuthUser>) => {
                state.accessToken = action.payload.accessToken;
                state.user = action.payload;
            })

            // ── Logout ──────────────────────────────────────────
            .addCase(logoutAsync.pending, (state) => {
                state.logoutLoading = true;
            })
            .addCase(logoutAsync.fulfilled, (state) => {
                state.logoutLoading = false;
            })
            .addCase(logoutAsync.rejected, (state) => {
                state.logoutLoading = false;
            });
    },
});

export const { logout, clearError } = authSlice.actions;

export default authSlice.reducer;
