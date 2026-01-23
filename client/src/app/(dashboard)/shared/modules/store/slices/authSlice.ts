import { createSlice, createAsyncThunk, PayloadAction } from '@reduxjs/toolkit';
import { AxiosError } from 'axios';
import { authService } from '@/services/auth/authService';
import type { AuthState, LoginRequest, RegisterRequest, User, AuthTokens } from '@/types';
import { clearStoredTokens, getStoredTokens, setStoredTokens } from '@/services/auth/authStorage';

interface ApiErrorResponse {
  message?: string;
}

function getErrorMessage(error: unknown): string {
  if (error instanceof AxiosError) {
    const responseData = error.response?.data as ApiErrorResponse | undefined;
    return responseData?.message || 'An error occurred';
  }
  if (error instanceof Error) {
    return error.message;
  }
  return 'An error occurred';
}

const initialState: AuthState = {
  user: null,
  tokens: null,
  isAuthenticated: false,
  isLoading: false,
  isInitializing: false,
  isInitialized: false,
  error: null,
};

export const loginAsync = createAsyncThunk(
  'auth/login',
  async (credentials: LoginRequest, { rejectWithValue }) => {
    try {
      const response = await authService.login(credentials);
      return response;
    } catch (error) {
      return rejectWithValue(getErrorMessage(error) || 'Login failed');
    }
  }
);

export const registerAsync = createAsyncThunk(
  'auth/register',
  async (data: RegisterRequest, { rejectWithValue }) => {
    try {
      const response = await authService.register(data);
      return response;
    } catch (error) {
      return rejectWithValue(getErrorMessage(error) || 'Registration failed');
    }
  }
);

export const logoutAsync = createAsyncThunk('auth/logout', async () => {
  await authService.logout();
});

export const getCurrentUserAsync = createAsyncThunk(
  'auth/getCurrentUser',
  async (_, { rejectWithValue }) => {
    try {
      const user = await authService.getCurrentUser();
      return user;
    } catch (error) {
      return rejectWithValue(getErrorMessage(error) || 'Failed to get user');
    }
  }
);

export const initializeAuthAsync = createAsyncThunk(
  'auth/initialize',
  async (_, { rejectWithValue }) => {
    try {
      const tokens = getStoredTokens();
      if (!tokens) {
        return { user: null as User | null, tokens: null as AuthTokens | null };
      }

      const isValid = await authService.validateToken(tokens.accessToken);
      let finalTokens = tokens;

      if (!isValid) {
        try {
          finalTokens = await authService.refreshTokens(tokens);
          setStoredTokens(finalTokens);
        } catch {
          clearStoredTokens();
          return { user: null as User | null, tokens: null as AuthTokens | null };
        }
      }

      const user = authService.getUserFromAccessToken(finalTokens.accessToken);
      return { user, tokens: finalTokens };
    } catch (error) {
      clearStoredTokens();
      return rejectWithValue(getErrorMessage(error) || 'Failed to initialize auth');
    }
  }
);

const authSlice = createSlice({
  name: 'auth',
  initialState,
  reducers: {
    setUser: (state, action: PayloadAction<User | null>) => {
      state.user = action.payload;
      // isAuthenticated should be true only if both user and tokens exist
      state.isAuthenticated = !!(action.payload && state.tokens);
      state.error = null;
    },
    setTokens: (state, action: PayloadAction<AuthTokens | null>) => {
      state.tokens = action.payload;
      // isAuthenticated should be true only if both user and tokens exist
      state.isAuthenticated = !!(state.user && action.payload);
      if (action.payload) setStoredTokens(action.payload);
      else clearStoredTokens();
      state.error = null;
    },
    clearAuth: (state) => {
      state.user = null;
      state.tokens = null;
      state.isAuthenticated = false;
      state.error = null;
      clearStoredTokens();
    },
    restoreAuth: (state) => {
      state.tokens = getStoredTokens();
      // Ensure isAuthenticated is consistent with user and tokens
      state.isAuthenticated = !!(state.user && state.tokens);
      state.error = null;
    },
  },
  extraReducers: (builder) => {
    builder
      .addCase(loginAsync.pending, (state) => {
        state.isLoading = true;
        state.error = null;
      })
      .addCase(loginAsync.fulfilled, (state, action) => {
        state.isLoading = false;
        state.user = action.payload.user;
        state.tokens = action.payload.tokens;
        state.isAuthenticated = true;
        setStoredTokens(action.payload.tokens);
        state.error = null;
      })
      .addCase(loginAsync.rejected, (state, action) => {
        state.isLoading = false;
        state.user = null;
        state.tokens = null;
        state.isAuthenticated = false;
        clearStoredTokens();
        state.error = (action.payload as string) || 'Login failed';
      })
      .addCase(registerAsync.pending, (state) => {
        state.isLoading = true;
        state.error = null;
      })
      .addCase(registerAsync.fulfilled, (state, action) => {
        state.isLoading = false;
        state.user = action.payload.user;
        state.tokens = action.payload.tokens;
        state.isAuthenticated = true;
        setStoredTokens(action.payload.tokens);
        state.error = null;
      })
      .addCase(registerAsync.rejected, (state, action) => {
        state.isLoading = false;
        state.user = null;
        state.tokens = null;
        state.isAuthenticated = false;
        clearStoredTokens();
        state.error = (action.payload as string) || 'Registration failed';
      })
      .addCase(logoutAsync.fulfilled, (state) => {
        state.user = null;
        state.tokens = null;
        state.isAuthenticated = false;
        state.isInitialized = true;
        state.error = null;
      })
      .addCase(getCurrentUserAsync.pending, (state) => {
        state.isLoading = true;
        state.error = null;
      })
      .addCase(getCurrentUserAsync.fulfilled, (state, action) => {
        state.isLoading = false;
        state.user = action.payload;
        // Only set isAuthenticated to true if tokens exist
        // If tokens don't exist, clear the user and set isAuthenticated to false
        if (state.tokens) {
          state.isAuthenticated = true;
        } else {
          // Tokens are missing, clear auth state
          state.user = null;
          state.isAuthenticated = false;
          clearStoredTokens();
        }
        state.error = null;
      })
      .addCase(getCurrentUserAsync.rejected, (state, action) => {
        state.isLoading = false;
        state.user = null;
        state.tokens = null;
        state.isAuthenticated = false;
        state.error = (action.payload as string) || 'Failed to get user';
        clearStoredTokens();
      })
      .addCase(initializeAuthAsync.pending, (state) => {
        state.isInitializing = true;
        state.error = null;
      })
      .addCase(initializeAuthAsync.fulfilled, (state, action) => {
        state.isInitializing = false;
        state.isInitialized = true;

        state.user = action.payload.user;
        state.tokens = action.payload.tokens;
        state.isAuthenticated = !!(state.user && state.tokens);
        state.error = null;
      })
      .addCase(initializeAuthAsync.rejected, (state, action) => {
        state.isInitializing = false;
        state.isInitialized = true;
        state.user = null;
        state.tokens = null;
        state.isAuthenticated = false;
        state.error = (action.payload as string) || 'Failed to initialize auth';
      });
  },
});

export const { setUser, setTokens, clearAuth, restoreAuth } = authSlice.actions;
export default authSlice.reducer;
