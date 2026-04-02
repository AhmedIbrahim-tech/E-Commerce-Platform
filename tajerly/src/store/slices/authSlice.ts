import { createSlice, createAsyncThunk, PayloadAction } from '@reduxjs/toolkit';
import { authService, LoginRequest, RegisterRequest, GoogleLoginRequest, ChangePasswordRequest } from '@/services/authService';
import type { RootState } from '@/store';

// ── Types ───────────────────────────────────────────────────────────
export interface AuthUser {
    id?: number;
    name: string;
    email?: string;
    role: string;
    roles: string[];
    profilePicture?: string;
}

// ── Local Storage Helpers ───────────────────────────────────────────
const KEYS = {
    ACCESS_TOKEN: 'accessToken',
    REFRESH_TOKEN: 'refreshToken',
    USER: 'user',
    ROLES: 'roles'
};

const saveToStorage = (data: { accessToken?: string; refreshToken?: string; user?: AuthUser; roles?: string[] }) => {
    if (typeof window === 'undefined') return;
    if (data.accessToken) localStorage.setItem(KEYS.ACCESS_TOKEN, data.accessToken);
    if (data.refreshToken) localStorage.setItem(KEYS.REFRESH_TOKEN, data.refreshToken);
    if (data.user) localStorage.setItem(KEYS.USER, JSON.stringify(data.user));
    if (data.roles) localStorage.setItem(KEYS.ROLES, JSON.stringify(data.roles));
};

const clearStorage = () => {
    if (typeof window === 'undefined') return;
    localStorage.removeItem(KEYS.ACCESS_TOKEN);
    localStorage.removeItem(KEYS.REFRESH_TOKEN);
    localStorage.removeItem(KEYS.USER);
    localStorage.removeItem(KEYS.ROLES);
};

const getFromStorage = () => {
    if (typeof window === 'undefined') return initialState;
    const accessToken = localStorage.getItem(KEYS.ACCESS_TOKEN);
    const refreshToken = localStorage.getItem(KEYS.REFRESH_TOKEN);
    const userStr = localStorage.getItem(KEYS.USER);
    const rolesStr = localStorage.getItem(KEYS.ROLES);

    return {
        accessToken,
        refreshToken,
        user: userStr ? JSON.parse(userStr) : null,
        roles: rolesStr ? JSON.parse(rolesStr) : [],
        isAuthenticated: !!accessToken,
    };
};

interface AuthState {
    user: AuthUser | null;
    accessToken: string | null;
    refreshToken: string | null;
    roles: string[];
    isAuthenticated: boolean;
    loading: boolean;
    error: string | null;
}

const initialState: AuthState = {
    user: null,
    accessToken: null,
    refreshToken: null,
    roles: [],
    isAuthenticated: false,
    loading: false,
    error: null,
};

// Client-side initialization helper
const getInitialState = (): AuthState => {
    const fromStorage = getFromStorage();
    return {
        ...initialState,
        ...fromStorage,
    };
};

// ── Async Thunks ────────────────────────────────────────────────────

export const loginUser = createAsyncThunk<
    { user: AuthUser; accessToken: string; refreshToken: string; roles: string[] },
    LoginRequest,
    { rejectValue: string }
>(
    'auth/loginUser',
    async (credentials, { rejectWithValue }) => {
        try {
            const response = await authService.login(credentials);
            if (response.succeeded && response.data) {
                const { accessToken, refreshToken, roles } = response.data;
                // Since JwtAuthResponse doesn't contain full user details, we take what we have
                // or you might want to decode the JWT here for name/email
                const result = {
                    user: { 
                        name: refreshToken.userName, 
                        role: roles[0] || 'User',
                        roles: roles
                    },
                    accessToken,
                    refreshToken: refreshToken.tokenString,
                    roles
                };
                saveToStorage(result);
                return result;
            }
            return rejectWithValue(response.message || 'Login failed');
        } catch (err: unknown) {
            const error = err as { response?: { data?: { message?: string } } };
            return rejectWithValue(error.response?.data?.message || 'Something went wrong');
        }
    }
);

export const registerUser = createAsyncThunk<
    string,
    RegisterRequest,
    { rejectValue: string }
>(
    'auth/registerUser',
    async (data, { rejectWithValue }) => {
        try {
            const response = await authService.register(data);
            if (response.succeeded && response.data) {
                return response.data; // The string message or user identifier
            }
            return rejectWithValue(response.message || 'Registration failed');
        } catch (err: unknown) {
            const error = err as { response?: { data?: { message?: string } } };
            return rejectWithValue(error.response?.data?.message || 'Something went wrong');
        }
    }
);

export const googleLogin = createAsyncThunk<
    { user: AuthUser; accessToken: string; refreshToken: string; roles: string[] },
    GoogleLoginRequest,
    { rejectValue: string }
>(
    'auth/googleLogin',
    async (data, { rejectWithValue }) => {
        try {
            const response = await authService.signInViaGoogle(data);
            if (response.succeeded && response.data) {
                const { accessToken, refreshToken, roles } = response.data;
                const result = {
                    user: { 
                        name: refreshToken.userName, 
                        role: roles[0] || 'User',
                        roles: roles
                    },
                    accessToken,
                    refreshToken: refreshToken.tokenString,
                    roles
                };
                saveToStorage(result);
                return result;
            }
            return rejectWithValue(response.message || 'Google login failed');
        } catch (err: unknown) {
            const error = err as { response?: { data?: { message?: string } } };
            return rejectWithValue(error.response?.data?.message || 'Something went wrong');
        }
    }
);

export const validateToken = createAsyncThunk<
    string,
    string,
    { rejectValue: string }
>(
    'auth/validateToken',
    async (token, { rejectWithValue }) => {
        try {
            const response = await authService.validateToken(token);
            if (response.succeeded && response.data) {
                return response.data;
            }
            return rejectWithValue(response.message || 'Token validation failed');
        } catch (err: unknown) {
            const error = err as { response?: { data?: { message?: string } } };
            return rejectWithValue(error.response?.data?.message || 'Invalid token');
        }
    }
);

export const logoutUser = createAsyncThunk<void, void, { state: RootState }>(
    'auth/logoutUser',
    async (_, { getState, dispatch }) => {
        const { refreshToken } = getState().auth;
        try {
            if (refreshToken) {
                await authService.logout(refreshToken);
            }
        } catch {
            // Silently fail — still log out client-side
        }
        dispatch(logout());
    }
);

export const logoutAllDevices = createAsyncThunk<void, void, { state: RootState }>(
    'auth/logoutAllDevices',
    async (_, { dispatch }) => {
        try {
            await authService.logoutAll();
        } catch {
            // Silently fail
        }
        dispatch(logout());
    }
);

// ── Slice ───────────────────────────────────────────────────────────

const authSlice = createSlice({
    name: 'auth',
    initialState: typeof window !== 'undefined' ? getInitialState() : initialState,
    reducers: {
        tokenRefreshed: (state, action: PayloadAction<{ accessToken: string; refreshToken: string }>) => {
            state.accessToken = action.payload.accessToken;
            state.refreshToken = action.payload.refreshToken;
            saveToStorage({ accessToken: action.payload.accessToken, refreshToken: action.payload.refreshToken });
        },
        updateCredentials: (state, action: PayloadAction<{ user: AuthUser; accessToken: string; refreshToken: string; roles: string[] }>) => {
            state.user = action.payload.user;
            state.accessToken = action.payload.accessToken;
            state.refreshToken = action.payload.refreshToken;
            state.roles = action.payload.roles;
            state.isAuthenticated = true;
            saveToStorage(action.payload);
        },
        logout: (state) => {
            state.user = null;
            state.accessToken = null;
            state.refreshToken = null;
            state.roles = [];
            state.isAuthenticated = false;
            state.loading = false;
            state.error = null;
            clearStorage();
        },
    },
    extraReducers: (builder) => {
        builder
            // loginUser
            .addCase(loginUser.pending, (state) => {
                state.loading = true;
                state.error = null;
            })
            .addCase(loginUser.fulfilled, (state, action) => {
                state.loading = false;
                state.user = action.payload.user;
                state.accessToken = action.payload.accessToken;
                state.refreshToken = action.payload.refreshToken;
                state.roles = action.payload.roles;
                state.isAuthenticated = true;
            })
            .addCase(loginUser.rejected, (state, action) => {
                state.loading = false;
                state.error = action.payload || 'Login failed';
            })
            // googleLogin
            .addCase(googleLogin.pending, (state) => {
                state.loading = true;
                state.error = null;
            })
            .addCase(googleLogin.fulfilled, (state, action) => {
                state.loading = false;
                state.user = action.payload.user;
                state.accessToken = action.payload.accessToken;
                state.refreshToken = action.payload.refreshToken;
                state.roles = action.payload.roles;
                state.isAuthenticated = true;
            })
            .addCase(googleLogin.rejected, (state, action) => {
                state.loading = false;
                state.error = action.payload || 'Google login failed';
            })
            // registerUser
            .addCase(registerUser.pending, (state) => {
                state.loading = true;
                state.error = null;
            })
            .addCase(registerUser.fulfilled, (state) => {
                state.loading = false;
                state.error = null;
                // Since register returns a string (e.g. success message), we don't auto-login here
                // unless the user flow dictates otherwise.
            })
            .addCase(registerUser.rejected, (state, action) => {
                state.loading = false;
                state.error = action.payload || 'Registration failed';
            })
            // logoutUser
            .addCase(logoutUser.fulfilled, (state) => {
                state.user = null;
                state.accessToken = null;
                state.refreshToken = null;
                state.roles = [];
                state.isAuthenticated = false;
                state.loading = false;
                state.error = null;
            });
    },
});

export const { tokenRefreshed, logout, updateCredentials } = authSlice.actions;
export default authSlice.reducer;
