import { createSlice, createAsyncThunk, PayloadAction } from '@reduxjs/toolkit';
import { AxiosError } from 'axios';
import { variantAttributeService } from '@/services/catalog/variantAttributeService';
import type {
  VariantAttribute,
  VariantAttributeListResponse,
  CreateVariantAttributeRequest,
  UpdateVariantAttributeRequest,
} from '@/types';

interface ApiErrorResponse {
  message?: string;
  errors?: Record<string, string[]>;
}

interface VariantAttributeState {
  variantAttributes: VariantAttribute[];
  selectedVariantAttribute: VariantAttribute | null;
  loading: boolean;
  error: string | null;
  totalCount: number;
  pageNumber: number;
  pageSize: number;
  search?: string;
}

const initialState: VariantAttributeState = {
  variantAttributes: [],
  selectedVariantAttribute: null,
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

export const fetchVariantAttributesAsync = createAsyncThunk(
  'variantAttribute/fetchVariantAttributes',
  async (
    params: { pageNumber?: number; pageSize?: number; search?: string },
    { rejectWithValue }
  ) => {
    try {
      const response = await variantAttributeService.getVariantAttributePaginatedList(
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

export const fetchVariantAttributeByIdAsync = createAsyncThunk(
  'variantAttribute/fetchVariantAttributeById',
  async (id: string, { rejectWithValue }) => {
    try {
      const variantAttribute = await variantAttributeService.getVariantAttributeById(id);
      return variantAttribute;
    } catch (error) {
      return rejectWithValue(getErrorMessage(error));
    }
  }
);

export const createVariantAttributeAsync = createAsyncThunk(
  'variantAttribute/createVariantAttribute',
  async (data: CreateVariantAttributeRequest, { rejectWithValue }) => {
    try {
      await variantAttributeService.createVariantAttribute(data);
      return data;
    } catch (error) {
      return rejectWithValue(getErrorMessage(error));
    }
  }
);

export const updateVariantAttributeAsync = createAsyncThunk(
  'variantAttribute/updateVariantAttribute',
  async (data: UpdateVariantAttributeRequest, { rejectWithValue }) => {
    try {
      await variantAttributeService.updateVariantAttribute(data);
      return data;
    } catch (error) {
      return rejectWithValue(getErrorMessage(error));
    }
  }
);

export const deleteVariantAttributeAsync = createAsyncThunk(
  'variantAttribute/deleteVariantAttribute',
  async (id: string, { rejectWithValue }) => {
    try {
      await variantAttributeService.deleteVariantAttribute(id);
      return id;
    } catch (error) {
      return rejectWithValue(getErrorMessage(error));
    }
  }
);

const variantAttributeSlice = createSlice({
  name: 'variantAttribute',
  initialState,
  reducers: {
    setSelectedVariantAttribute: (state, action: PayloadAction<VariantAttribute | null>) => {
      state.selectedVariantAttribute = action.payload;
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
      .addCase(fetchVariantAttributesAsync.pending, (state) => {
        state.loading = true;
        state.error = null;
      })
      .addCase(fetchVariantAttributesAsync.fulfilled, (state, action) => {
        state.loading = false;
        state.variantAttributes = action.payload.response.data || [];
        state.totalCount = action.payload.response.totalCount || 0;
        state.pageNumber = action.payload.response.currentPage || 1;
        state.pageSize = action.payload.response.pageSize || 10;
        if (action.payload.search !== undefined) {
          state.search = action.payload.search;
        }
        state.error = null;
      })
      .addCase(fetchVariantAttributesAsync.rejected, (state, action) => {
        state.loading = false;
        state.error = action.payload as string;
      })
      .addCase(fetchVariantAttributeByIdAsync.pending, (state) => {
        state.loading = true;
        state.error = null;
      })
      .addCase(fetchVariantAttributeByIdAsync.fulfilled, (state, action) => {
        state.loading = false;
        state.selectedVariantAttribute = action.payload;
        state.error = null;
      })
      .addCase(fetchVariantAttributeByIdAsync.rejected, (state, action) => {
        state.loading = false;
        state.error = action.payload as string;
      })
      .addCase(createVariantAttributeAsync.pending, (state) => {
        state.loading = true;
        state.error = null;
      })
      .addCase(createVariantAttributeAsync.fulfilled, (state) => {
        state.loading = false;
        state.error = null;
      })
      .addCase(createVariantAttributeAsync.rejected, (state, action) => {
        state.loading = false;
        state.error = action.payload as string;
      })
      .addCase(updateVariantAttributeAsync.pending, (state) => {
        state.loading = true;
        state.error = null;
      })
      .addCase(updateVariantAttributeAsync.fulfilled, (state, action) => {
        state.loading = false;
        const index = state.variantAttributes.findIndex((c) => c.id === action.payload.id);
        if (index !== -1 && state.selectedVariantAttribute?.id === action.payload.id) {
          state.selectedVariantAttribute = { ...state.selectedVariantAttribute, ...action.payload };
        }
        state.error = null;
      })
      .addCase(updateVariantAttributeAsync.rejected, (state, action) => {
        state.loading = false;
        state.error = action.payload as string;
      })
      .addCase(deleteVariantAttributeAsync.pending, (state) => {
        state.loading = true;
        state.error = null;
      })
      .addCase(deleteVariantAttributeAsync.fulfilled, (state, action) => {
        state.loading = false;
        state.variantAttributes = state.variantAttributes.filter((c) => c.id !== action.payload);
        state.totalCount = Math.max(0, state.totalCount - 1);
        if (state.selectedVariantAttribute?.id === action.payload) {
          state.selectedVariantAttribute = null;
        }
        state.error = null;
      })
      .addCase(deleteVariantAttributeAsync.rejected, (state, action) => {
        state.loading = false;
        state.error = action.payload as string;
      });
  },
});

export const {
  setSelectedVariantAttribute,
  clearError,
  setPageNumber,
  setPageSize,
  setSearch,
} = variantAttributeSlice.actions;
export default variantAttributeSlice.reducer;
