import { createSlice, createAsyncThunk, PayloadAction } from '@reduxjs/toolkit';
import { AxiosError } from 'axios';
import { brandService } from '@/services/catalog/brandService';
import type {
  Brand,
  BrandListResponse,
  CreateBrandRequest,
  UpdateBrandRequest,
} from '@/types';

interface ApiErrorResponse {
  message?: string;
  errors?: Record<string, string[]>;
}

interface BrandState {
  brands: Brand[];
  selectedBrand: Brand | null;
  loading: boolean;
  error: string | null;
  totalCount: number;
  pageNumber: number;
  pageSize: number;
  search?: string;
}

const initialState: BrandState = {
  brands: [],
  selectedBrand: null,
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

export const fetchBrandsAsync = createAsyncThunk(
  'brand/fetchBrands',
  async (
    params: { pageNumber?: number; pageSize?: number; search?: string },
    { rejectWithValue }
  ) => {
    try {
      const response = await brandService.getBrandPaginatedList(
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

export const fetchBrandByIdAsync = createAsyncThunk(
  'brand/fetchBrandById',
  async (id: string, { rejectWithValue }) => {
    try {
      const brand = await brandService.getBrandById(id);
      return brand;
    } catch (error) {
      return rejectWithValue(getErrorMessage(error));
    }
  }
);

export const createBrandAsync = createAsyncThunk(
  'brand/createBrand',
  async (data: CreateBrandRequest, { rejectWithValue }) => {
    try {
      await brandService.createBrand(data);
      return data;
    } catch (error) {
      return rejectWithValue(getErrorMessage(error));
    }
  }
);

export const updateBrandAsync = createAsyncThunk(
  'brand/updateBrand',
  async (data: UpdateBrandRequest, { rejectWithValue }) => {
    try {
      await brandService.updateBrand(data);
      return data;
    } catch (error) {
      return rejectWithValue(getErrorMessage(error));
    }
  }
);

export const deleteBrandAsync = createAsyncThunk(
  'brand/deleteBrand',
  async (id: string, { rejectWithValue }) => {
    try {
      await brandService.deleteBrand(id);
      return id;
    } catch (error) {
      return rejectWithValue(getErrorMessage(error));
    }
  }
);

const brandSlice = createSlice({
  name: 'brand',
  initialState,
  reducers: {
    setSelectedBrand: (state, action: PayloadAction<Brand | null>) => {
      state.selectedBrand = action.payload;
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
      .addCase(fetchBrandsAsync.pending, (state) => {
        state.loading = true;
        state.error = null;
      })
      .addCase(fetchBrandsAsync.fulfilled, (state, action) => {
        state.loading = false;
        state.brands = action.payload.response.data || [];
        state.totalCount = action.payload.response.totalCount || 0;
        state.pageNumber = action.payload.response.currentPage || 1;
        state.pageSize = action.payload.response.pageSize || 10;
        if (action.payload.search !== undefined) {
          state.search = action.payload.search;
        }
        state.error = null;
      })
      .addCase(fetchBrandsAsync.rejected, (state, action) => {
        state.loading = false;
        state.error = action.payload as string;
      })
      .addCase(fetchBrandByIdAsync.pending, (state) => {
        state.loading = true;
        state.error = null;
      })
      .addCase(fetchBrandByIdAsync.fulfilled, (state, action) => {
        state.loading = false;
        state.selectedBrand = action.payload;
        state.error = null;
      })
      .addCase(fetchBrandByIdAsync.rejected, (state, action) => {
        state.loading = false;
        state.error = action.payload as string;
      })
      .addCase(createBrandAsync.pending, (state) => {
        state.loading = true;
        state.error = null;
      })
      .addCase(createBrandAsync.fulfilled, (state) => {
        state.loading = false;
        state.error = null;
      })
      .addCase(createBrandAsync.rejected, (state, action) => {
        state.loading = false;
        state.error = action.payload as string;
      })
      .addCase(updateBrandAsync.pending, (state) => {
        state.loading = true;
        state.error = null;
      })
      .addCase(updateBrandAsync.fulfilled, (state, action) => {
        state.loading = false;
        const index = state.brands.findIndex((c) => c.id === action.payload.id);
        if (index !== -1 && state.selectedBrand?.id === action.payload.id) {
          state.selectedBrand = { ...state.selectedBrand, ...action.payload };
        }
        state.error = null;
      })
      .addCase(updateBrandAsync.rejected, (state, action) => {
        state.loading = false;
        state.error = action.payload as string;
      })
      .addCase(deleteBrandAsync.pending, (state) => {
        state.loading = true;
        state.error = null;
      })
      .addCase(deleteBrandAsync.fulfilled, (state, action) => {
        state.loading = false;
        state.brands = state.brands.filter((c) => c.id !== action.payload);
        state.totalCount = Math.max(0, state.totalCount - 1);
        if (state.selectedBrand?.id === action.payload) {
          state.selectedBrand = null;
        }
        state.error = null;
      })
      .addCase(deleteBrandAsync.rejected, (state, action) => {
        state.loading = false;
        state.error = action.payload as string;
      });
  },
});

export const {
  setSelectedBrand,
  clearError,
  setPageNumber,
  setPageSize,
  setSearch,
} = brandSlice.actions;
export default brandSlice.reducer;
