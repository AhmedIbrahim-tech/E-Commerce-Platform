import { createSlice, createAsyncThunk, PayloadAction } from '@reduxjs/toolkit';
import { AxiosError } from 'axios';
import { vendorService } from '@/app/(dashboard)/shared/modules/api/users/vendorService';
import type {
  Vendor,
  VendorListResponse,
  CreateVendorRequest,
  UpdateVendorRequest,
} from '@/types';

interface ApiErrorResponse {
  message?: string;
  errors?: Record<string, string[]>;
}

interface VendorState {
  vendors: Vendor[];
  selectedVendor: Vendor | null;
  loading: boolean;
  error: string | null;
  totalCount: number;
  pageNumber: number;
  pageSize: number;
  search?: string;
}

const initialState: VendorState = {
  vendors: [],
  selectedVendor: null,
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

export const fetchVendorsAsync = createAsyncThunk(
  'vendor/fetchVendors',
  async (
    params: { pageNumber?: number; pageSize?: number; search?: string },
    { rejectWithValue }
  ) => {
    try {
      const response = await vendorService.getVendorPaginatedList(
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

export const fetchVendorByIdAsync = createAsyncThunk(
  'vendor/fetchVendorById',
  async (id: string, { rejectWithValue }) => {
    try {
      const vendor = await vendorService.getVendorById(id);
      return vendor;
    } catch (error) {
      return rejectWithValue(getErrorMessage(error));
    }
  }
);

export const createVendorAsync = createAsyncThunk(
  'vendor/createVendor',
  async (data: CreateVendorRequest, { rejectWithValue }) => {
    try {
      await vendorService.createVendor(data);
      return data;
    } catch (error) {
      return rejectWithValue(getErrorMessage(error));
    }
  }
);

export const updateVendorAsync = createAsyncThunk(
  'vendor/updateVendor',
  async (data: UpdateVendorRequest, { rejectWithValue }) => {
    try {
      await vendorService.updateVendor(data);
      return data;
    } catch (error) {
      return rejectWithValue(getErrorMessage(error));
    }
  }
);

export const deleteVendorAsync = createAsyncThunk(
  'vendor/deleteVendor',
  async (id: string, { rejectWithValue }) => {
    try {
      await vendorService.deleteVendor(id);
      return id;
    } catch (error) {
      return rejectWithValue(getErrorMessage(error));
    }
  }
);

export const toggleVendorStatusAsync = createAsyncThunk(
  'vendor/toggleVendorStatus',
  async (id: string, { rejectWithValue }) => {
    try {
      const message = await vendorService.toggleVendorStatus(id);
      return { id, message };
    } catch (error) {
      return rejectWithValue(getErrorMessage(error));
    }
  }
);

const vendorSlice = createSlice({
  name: 'vendor',
  initialState,
  reducers: {
    setSelectedVendor: (state, action: PayloadAction<Vendor | null>) => {
      state.selectedVendor = action.payload;
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
      .addCase(fetchVendorsAsync.pending, (state) => {
        state.loading = true;
        state.error = null;
      })
      .addCase(fetchVendorsAsync.fulfilled, (state, action) => {
        state.loading = false;
        state.vendors = action.payload.response.data || [];
        state.totalCount = action.payload.response.totalCount || 0;
        state.pageNumber = action.payload.response.currentPage || 1;
        state.pageSize = action.payload.response.pageSize || 10;
        if (action.payload.search !== undefined) {
          state.search = action.payload.search;
        }
        state.error = null;
      })
      .addCase(fetchVendorsAsync.rejected, (state, action) => {
        state.loading = false;
        state.error = action.payload as string;
      })
      .addCase(fetchVendorByIdAsync.pending, (state) => {
        state.loading = true;
        state.error = null;
      })
      .addCase(fetchVendorByIdAsync.fulfilled, (state, action) => {
        state.loading = false;
        state.selectedVendor = action.payload;
        state.error = null;
      })
      .addCase(fetchVendorByIdAsync.rejected, (state, action) => {
        state.loading = false;
        state.error = action.payload as string;
      })
      .addCase(createVendorAsync.pending, (state) => {
        state.loading = true;
        state.error = null;
      })
      .addCase(createVendorAsync.fulfilled, (state) => {
        state.loading = false;
        state.error = null;
      })
      .addCase(createVendorAsync.rejected, (state, action) => {
        state.loading = false;
        state.error = action.payload as string;
      })
      .addCase(updateVendorAsync.pending, (state) => {
        state.loading = true;
        state.error = null;
      })
      .addCase(updateVendorAsync.fulfilled, (state, action) => {
        state.loading = false;
        const index = state.vendors.findIndex((v) => v.id === action.payload.id);
        const updates: Partial<Vendor> = {};
        (Object.keys(action.payload) as (keyof UpdateVendorRequest)[]).forEach((key) => {
          if (key === "profileImage") return;
          const value = action.payload[key];
          if (value !== undefined) {
            (updates as unknown as Record<string, unknown>)[key] = value;
          }
        });

        if (index !== -1) {
          state.vendors[index] = { ...state.vendors[index], ...updates };
        }

        if (state.selectedVendor?.id === action.payload.id) {
          state.selectedVendor = { ...state.selectedVendor, ...updates };
        }
        state.error = null;
      })
      .addCase(updateVendorAsync.rejected, (state, action) => {
        state.loading = false;
        state.error = action.payload as string;
      })
      .addCase(deleteVendorAsync.pending, (state) => {
        state.loading = true;
        state.error = null;
      })
      .addCase(deleteVendorAsync.fulfilled, (state, action) => {
        state.loading = false;
        state.vendors = state.vendors.filter((v) => v.id !== action.payload);
        state.totalCount = Math.max(0, state.totalCount - 1);
        if (state.selectedVendor?.id === action.payload) {
          state.selectedVendor = null;
        }
        state.error = null;
      })
      .addCase(deleteVendorAsync.rejected, (state, action) => {
        state.loading = false;
        state.error = action.payload as string;
      })
      .addCase(toggleVendorStatusAsync.pending, (state) => {
        state.loading = true;
        state.error = null;
      })
      .addCase(toggleVendorStatusAsync.fulfilled, (state, action) => {
        state.loading = false;
        const index = state.vendors.findIndex((v) => v.id === action.payload.id);
        if (index !== -1) {
          // Toggle the isDeleted status
          state.vendors[index] = { ...state.vendors[index], isDeleted: !state.vendors[index].isDeleted };
        }
        if (state.selectedVendor?.id === action.payload.id) {
          state.selectedVendor = { ...state.selectedVendor, isDeleted: !state.selectedVendor.isDeleted };
        }
        state.error = null;
      })
      .addCase(toggleVendorStatusAsync.rejected, (state, action) => {
        state.loading = false;
        state.error = action.payload as string;
      });
  },
});

export const {
  setSelectedVendor,
  clearError,
  setPageNumber,
  setPageSize,
  setSearch,
} = vendorSlice.actions;
export default vendorSlice.reducer;
