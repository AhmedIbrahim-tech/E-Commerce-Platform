import { createSlice, createAsyncThunk, PayloadAction } from '@reduxjs/toolkit';
import { AxiosError } from 'axios';
import { authorizationService } from '@/services/auth/authorizationService';
import type {
  Role,
  RoleListResponse,
  CreateRoleRequest,
  UpdateRoleRequest,
  ManageUserRolesResponse,
  UpdateUserRolesRequest,
  ManageUserClaimsResponse,
  UpdateUserClaimsRequest,
} from '@/types';

interface ApiErrorResponse {
  message?: string;
  errors?: Record<string, string[]>;
}

interface AuthorizationState {
  roles: Role[];
  selectedRole: Role | null;
  loading: boolean;
  error: string | null;
  userRoles: ManageUserRolesResponse | null;
  userClaims: ManageUserClaimsResponse | null;
  rolesLoading: boolean;
  userRolesLoading: boolean;
  userClaimsLoading: boolean;
}

const initialState: AuthorizationState = {
  roles: [],
  selectedRole: null,
  loading: false,
  error: null,
  userRoles: null,
  userClaims: null,
  rolesLoading: false,
  userRolesLoading: false,
  userClaimsLoading: false,
};

function getErrorMessage(error: unknown): string {
  if (error instanceof AxiosError) {
    const responseData = error.response?.data as ApiErrorResponse | undefined;
    if (responseData?.message) {
      return responseData.message;
    }
    if (responseData?.errors) {
      const firstError = Object.values(responseData.errors)[0];
      if (firstError && firstError.length > 0) {
        return firstError[0];
      }
    }
    if (error.response?.status === 401) {
      return 'Unauthorized. Please login again.';
    }
    if (error.response?.status === 403) {
      return 'Access denied. You do not have permission to perform this action.';
    }
    return error.message || 'An error occurred';
  }
  if (error instanceof Error) {
    return error.message;
  }
  return 'An error occurred';
}

export const fetchRolesAsync = createAsyncThunk(
  'authorization/fetchRoles',
  async (_, { rejectWithValue }) => {
    try {
      const response = await authorizationService.getAllRoles();
      return response;
    } catch (error) {
      return rejectWithValue(getErrorMessage(error));
    }
  }
);

export const fetchRoleByIdAsync = createAsyncThunk(
  'authorization/fetchRoleById',
  async (id: string, { rejectWithValue }) => {
    try {
      const role = await authorizationService.getRoleById(id);
      return role;
    } catch (error) {
      return rejectWithValue(getErrorMessage(error));
    }
  }
);

export const createRoleAsync = createAsyncThunk(
  'authorization/createRole',
  async (data: CreateRoleRequest, { rejectWithValue }) => {
    try {
      await authorizationService.createRole(data);
      return data;
    } catch (error) {
      return rejectWithValue(getErrorMessage(error));
    }
  }
);

export const updateRoleAsync = createAsyncThunk(
  'authorization/updateRole',
  async (data: UpdateRoleRequest, { rejectWithValue }) => {
    try {
      await authorizationService.updateRole(data);
      return data;
    } catch (error) {
      return rejectWithValue(getErrorMessage(error));
    }
  }
);

export const deleteRoleAsync = createAsyncThunk(
  'authorization/deleteRole',
  async (id: string, { rejectWithValue }) => {
    try {
      await authorizationService.deleteRole(id);
      return id;
    } catch (error) {
      return rejectWithValue(getErrorMessage(error));
    }
  }
);

export const fetchUserRolesAsync = createAsyncThunk(
  'authorization/fetchUserRoles',
  async (userId: string, { rejectWithValue }) => {
    try {
      const response = await authorizationService.manageUserRoles(userId);
      return response;
    } catch (error) {
      return rejectWithValue(getErrorMessage(error));
    }
  }
);

export const updateUserRolesAsync = createAsyncThunk(
  'authorization/updateUserRoles',
  async (data: UpdateUserRolesRequest, { rejectWithValue }) => {
    try {
      await authorizationService.updateUserRoles(data);
      return data;
    } catch (error) {
      return rejectWithValue(getErrorMessage(error));
    }
  }
);

export const fetchUserClaimsAsync = createAsyncThunk(
  'authorization/fetchUserClaims',
  async (userId: string, { rejectWithValue }) => {
    try {
      const response = await authorizationService.manageUserClaims(userId);
      return response;
    } catch (error) {
      return rejectWithValue(getErrorMessage(error));
    }
  }
);

export const updateUserClaimsAsync = createAsyncThunk(
  'authorization/updateUserClaims',
  async (data: UpdateUserClaimsRequest, { rejectWithValue }) => {
    try {
      await authorizationService.updateUserClaims(data);
      return data;
    } catch (error) {
      return rejectWithValue(getErrorMessage(error));
    }
  }
);

const authorizationSlice = createSlice({
  name: 'authorization',
  initialState,
  reducers: {
    setSelectedRole: (state, action: PayloadAction<Role | null>) => {
      state.selectedRole = action.payload;
    },
    clearError: (state) => {
      state.error = null;
    },
    clearUserRoles: (state) => {
      state.userRoles = null;
    },
    clearUserClaims: (state) => {
      state.userClaims = null;
    },
  },
  extraReducers: (builder) => {
    builder
      .addCase(fetchRolesAsync.pending, (state) => {
        state.rolesLoading = true;
        state.loading = true;
        state.error = null;
      })
      .addCase(fetchRolesAsync.fulfilled, (state, action) => {
        state.rolesLoading = false;
        state.loading = false;
        state.roles = action.payload.roles || [];
        state.error = null;
      })
      .addCase(fetchRolesAsync.rejected, (state, action) => {
        state.rolesLoading = false;
        state.loading = false;
        state.error = action.payload as string;
      })
      .addCase(fetchRoleByIdAsync.pending, (state) => {
        state.loading = true;
        state.error = null;
      })
      .addCase(fetchRoleByIdAsync.fulfilled, (state, action) => {
        state.loading = false;
        state.selectedRole = action.payload;
        state.error = null;
      })
      .addCase(fetchRoleByIdAsync.rejected, (state, action) => {
        state.loading = false;
        state.error = action.payload as string;
      })
      .addCase(createRoleAsync.pending, (state) => {
        state.loading = true;
        state.error = null;
      })
      .addCase(createRoleAsync.fulfilled, (state) => {
        state.loading = false;
        state.error = null;
      })
      .addCase(createRoleAsync.rejected, (state, action) => {
        state.loading = false;
        state.error = action.payload as string;
      })
      .addCase(updateRoleAsync.pending, (state) => {
        state.loading = true;
        state.error = null;
      })
      .addCase(updateRoleAsync.fulfilled, (state, action) => {
        state.loading = false;
        const index = state.roles.findIndex((r) => r.id === action.payload.id);
        if (index !== -1) {
          state.roles[index] = { ...state.roles[index], name: action.payload.name };
        }
        if (state.selectedRole?.id === action.payload.id) {
          state.selectedRole = { ...state.selectedRole, name: action.payload.name };
        }
        state.error = null;
      })
      .addCase(updateRoleAsync.rejected, (state, action) => {
        state.loading = false;
        state.error = action.payload as string;
      })
      .addCase(deleteRoleAsync.pending, (state) => {
        state.loading = true;
        state.error = null;
      })
      .addCase(deleteRoleAsync.fulfilled, (state, action) => {
        state.loading = false;
        state.roles = state.roles.filter((r) => r.id !== action.payload);
        if (state.selectedRole?.id === action.payload) {
          state.selectedRole = null;
        }
        state.error = null;
      })
      .addCase(deleteRoleAsync.rejected, (state, action) => {
        state.loading = false;
        state.error = action.payload as string;
      })
      .addCase(fetchUserRolesAsync.pending, (state) => {
        state.userRolesLoading = true;
        state.loading = true;
        state.error = null;
      })
      .addCase(fetchUserRolesAsync.fulfilled, (state, action) => {
        state.userRolesLoading = false;
        state.loading = false;
        state.userRoles = action.payload;
        state.error = null;
      })
      .addCase(fetchUserRolesAsync.rejected, (state, action) => {
        state.userRolesLoading = false;
        state.loading = false;
        state.error = action.payload as string;
      })
      .addCase(updateUserRolesAsync.pending, (state) => {
        state.userRolesLoading = true;
        state.loading = true;
        state.error = null;
      })
      .addCase(updateUserRolesAsync.fulfilled, (state, action) => {
        state.userRolesLoading = false;
        state.loading = false;
        if (state.userRoles && state.userRoles.userId === action.payload.userId) {
          state.userRoles.userRoles = action.payload.userRoles;
        }
        state.error = null;
      })
      .addCase(updateUserRolesAsync.rejected, (state, action) => {
        state.userRolesLoading = false;
        state.loading = false;
        state.error = action.payload as string;
      })
      .addCase(fetchUserClaimsAsync.pending, (state) => {
        state.userClaimsLoading = true;
        state.loading = true;
        state.error = null;
      })
      .addCase(fetchUserClaimsAsync.fulfilled, (state, action) => {
        state.userClaimsLoading = false;
        state.loading = false;
        state.userClaims = action.payload;
        state.error = null;
      })
      .addCase(fetchUserClaimsAsync.rejected, (state, action) => {
        state.userClaimsLoading = false;
        state.loading = false;
        state.error = action.payload as string;
      })
      .addCase(updateUserClaimsAsync.pending, (state) => {
        state.userClaimsLoading = true;
        state.loading = true;
        state.error = null;
      })
      .addCase(updateUserClaimsAsync.fulfilled, (state, action) => {
        state.userClaimsLoading = false;
        state.loading = false;
        if (state.userClaims && state.userClaims.userId === action.payload.userId) {
          state.userClaims.userClaims = action.payload.userClaims;
        }
        state.error = null;
      })
      .addCase(updateUserClaimsAsync.rejected, (state, action) => {
        state.userClaimsLoading = false;
        state.loading = false;
        state.error = action.payload as string;
      });
  },
});

export const {
  setSelectedRole,
  clearError,
  clearUserRoles,
  clearUserClaims,
} = authorizationSlice.actions;
export default authorizationSlice.reducer;
