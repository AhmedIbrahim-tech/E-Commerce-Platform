import { createSlice, createAsyncThunk, PayloadAction } from '@reduxjs/toolkit';
import { AxiosError } from 'axios';
import { adminService } from '@/app/(dashboard)/shared/modules/api/users/adminService';
import type {
  Admin,
  CreateAdminRequest,
  UpdateAdminRequest,
} from '@/types';

interface ApiErrorResponse {
  message?: string;
  errors?: Record<string, string[]>;
}

interface AdminState {
  admins: Admin[];
  selectedAdmin: Admin | null;
  loading: boolean;
  error: string | null;
  totalCount: number;
  pageNumber: number;
  pageSize: number;
  search?: string;
}

const initialState: AdminState = {
  admins: [],
  selectedAdmin: null,
  loading: false,
  error: null,
  totalCount: 0,
  pageNumber: 1,
  pageSize: 10,
  search: undefined,
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

export const fetchAdminsAsync = createAsyncThunk(
  'admin/fetchAdmins',
  async (
    params: { pageNumber?: number; pageSize?: number; search?: string },
    { rejectWithValue }
  ) => {
    try {
      const response = await adminService.getAdminPaginatedList(
        params.pageNumber || 1,
        params.pageSize || 10,
        params.search
      );
      return { response, search: params.search };
    } catch (error) {
      return rejectWithValue(getErrorMessage(error));
    }
  }
);

export const fetchAdminByIdAsync = createAsyncThunk(
  'admin/fetchAdminById',
  async (id: string, { rejectWithValue }) => {
    try {
      const admin = await adminService.getAdminById(id);
      return admin;
    } catch (error) {
      return rejectWithValue(getErrorMessage(error));
    }
  }
);

export const createAdminAsync = createAsyncThunk(
  'admin/createAdmin',
  async (data: CreateAdminRequest, { rejectWithValue }) => {
    try {
      await adminService.createAdmin(data);
      return data;
    } catch (error) {
      return rejectWithValue(getErrorMessage(error));
    }
  }
);

export const updateAdminAsync = createAsyncThunk(
  'admin/updateAdmin',
  async (data: UpdateAdminRequest, { rejectWithValue }) => {
    try {
      await adminService.updateAdmin(data);
      return data;
    } catch (error) {
      return rejectWithValue(getErrorMessage(error));
    }
  }
);

export const deleteAdminAsync = createAsyncThunk(
  'admin/deleteAdmin',
  async (id: string, { rejectWithValue }) => {
    try {
      await adminService.deleteAdmin(id);
      return id;
    } catch (error) {
      return rejectWithValue(getErrorMessage(error));
    }
  }
);

export const toggleAdminStatusAsync = createAsyncThunk(
  'admin/toggleAdminStatus',
  async (id: string, { rejectWithValue }) => {
    try {
      const message = await adminService.toggleAdminStatus(id);
      return { id, message };
    } catch (error) {
      return rejectWithValue(getErrorMessage(error));
    }
  }
);

const adminSlice = createSlice({
  name: 'admin',
  initialState,
  reducers: {
    setSelectedAdmin: (state, action: PayloadAction<Admin | null>) => {
      state.selectedAdmin = action.payload;
    },
    clearError: (state) => {
      state.error = null;
    },
    setPageNumber: (state, action: PayloadAction<number>) => {
      state.pageNumber = action.payload;
    },
    setPageSize: (state, action: PayloadAction<number>) => {
      state.pageSize = action.payload;
    },
    setSearch: (state, action: PayloadAction<string | undefined>) => {
      state.search = action.payload;
      state.pageNumber = 1;
    },
  },
  extraReducers: (builder) => {
    builder
      .addCase(fetchAdminsAsync.pending, (state) => {
        state.loading = true;
        state.error = null;
      })
      .addCase(fetchAdminsAsync.fulfilled, (state, action) => {
        state.loading = false;
        state.admins = action.payload.response.data || [];
        state.totalCount = action.payload.response.totalCount || 0;
        state.pageNumber = action.payload.response.currentPage || 1;
        state.pageSize = action.payload.response.pageSize || 10;
        if (action.payload.search !== undefined) {
          state.search = action.payload.search;
        }
        state.error = null;
      })
      .addCase(fetchAdminsAsync.rejected, (state, action) => {
        state.loading = false;
        state.error = action.payload as string;
      })
      .addCase(fetchAdminByIdAsync.pending, (state) => {
        state.loading = true;
        state.error = null;
      })
      .addCase(fetchAdminByIdAsync.fulfilled, (state, action) => {
        state.loading = false;
        state.selectedAdmin = action.payload;
        state.error = null;
      })
      .addCase(fetchAdminByIdAsync.rejected, (state, action) => {
        state.loading = false;
        state.error = action.payload as string;
      })
      .addCase(createAdminAsync.pending, (state) => {
        state.loading = true;
        state.error = null;
      })
      .addCase(createAdminAsync.fulfilled, (state) => {
        state.loading = false;
        state.error = null;
      })
      .addCase(createAdminAsync.rejected, (state, action) => {
        state.loading = false;
        state.error = action.payload as string;
      })
      .addCase(updateAdminAsync.pending, (state) => {
        state.loading = true;
        state.error = null;
      })
      .addCase(updateAdminAsync.fulfilled, (state, action) => {
        state.loading = false;
        const index = state.admins.findIndex((a) => a.id === action.payload.id);
        const updates: Partial<Admin> = {};
        (Object.keys(action.payload) as (keyof UpdateAdminRequest)[]).forEach((key) => {
          if (key === "profileImage") return;
          const value = action.payload[key];
          if (value !== undefined) {
            (updates as unknown as Record<string, unknown>)[key] = value;
          }
        });

        if (index !== -1) {
          state.admins[index] = { ...state.admins[index], ...updates };
        }

        if (state.selectedAdmin?.id === action.payload.id) {
          state.selectedAdmin = { ...state.selectedAdmin, ...updates };
        }
        state.error = null;
      })
      .addCase(updateAdminAsync.rejected, (state, action) => {
        state.loading = false;
        state.error = action.payload as string;
      })
      .addCase(deleteAdminAsync.pending, (state) => {
        state.loading = true;
        state.error = null;
      })
      .addCase(deleteAdminAsync.fulfilled, (state, action) => {
        state.loading = false;
        state.admins = state.admins.filter((a) => a.id !== action.payload);
        state.totalCount = Math.max(0, state.totalCount - 1);
        if (state.selectedAdmin?.id === action.payload) {
          state.selectedAdmin = null;
        }
        state.error = null;
      })
      .addCase(deleteAdminAsync.rejected, (state, action) => {
        state.loading = false;
        state.error = action.payload as string;
      })
      .addCase(toggleAdminStatusAsync.pending, (state) => {
        state.loading = true;
        state.error = null;
      })
      .addCase(toggleAdminStatusAsync.fulfilled, (state, action) => {
        state.loading = false;
        const index = state.admins.findIndex((a) => a.id === action.payload.id);
        if (index !== -1) {
          // Toggle the isDeleted status
          state.admins[index] = { ...state.admins[index], isDeleted: !state.admins[index].isDeleted };
        }
        if (state.selectedAdmin?.id === action.payload.id) {
          state.selectedAdmin = { ...state.selectedAdmin, isDeleted: !state.selectedAdmin.isDeleted };
        }
        state.error = null;
      })
      .addCase(toggleAdminStatusAsync.rejected, (state, action) => {
        state.loading = false;
        state.error = action.payload as string;
      });
  },
});

export const {
  setSelectedAdmin,
  clearError,
  setPageNumber,
  setPageSize,
  setSearch,
} = adminSlice.actions;
export default adminSlice.reducer;
