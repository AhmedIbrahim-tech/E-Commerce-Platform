import { createSlice, createAsyncThunk, PayloadAction } from '@reduxjs/toolkit';
import { AxiosError } from 'axios';
import { discountService } from '@/services/promotions/discountService';
import type {
  Discount,
  DiscountListResponse,
  CreateDiscountRequest,
  UpdateDiscountRequest,
} from '@/types';

interface ApiErrorResponse {
  message?: string;
  errors?: Record<string, string[]>;
}

interface DiscountState {
  discounts: Discount[];
  selectedDiscount: Discount | null;
  loading: boolean;
  error: string | null;
  totalCount: number;
  pageNumber: number;
  pageSize: number;
  search?: string;
}

const initialState: DiscountState = {
  discounts: [],
  selectedDiscount: null,
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

export const fetchDiscountsAsync = createAsyncThunk(
  'discount/fetchDiscounts',
  async (
    params: { pageNumber?: number; pageSize?: number; search?: string },
    { rejectWithValue }
  ) => {
    try {
      const response = await discountService.getDiscountPaginatedList(
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

export const fetchDiscountByIdAsync = createAsyncThunk(
  'discount/fetchDiscountById',
  async (id: string, { rejectWithValue }) => {
    try {
      const discount = await discountService.getDiscountById(id);
      return discount;
    } catch (error) {
      return rejectWithValue(getErrorMessage(error));
    }
  }
);

export const createDiscountAsync = createAsyncThunk(
  'discount/createDiscount',
  async (data: CreateDiscountRequest, { rejectWithValue }) => {
    try {
      await discountService.createDiscount(data);
      return data;
    } catch (error) {
      return rejectWithValue(getErrorMessage(error));
    }
  }
);

export const updateDiscountAsync = createAsyncThunk(
  'discount/updateDiscount',
  async (data: UpdateDiscountRequest, { rejectWithValue }) => {
    try {
      await discountService.updateDiscount(data);
      return data;
    } catch (error) {
      return rejectWithValue(getErrorMessage(error));
    }
  }
);

export const deleteDiscountAsync = createAsyncThunk(
  'discount/deleteDiscount',
  async (id: string, { rejectWithValue }) => {
    try {
      await discountService.deleteDiscount(id);
      return id;
    } catch (error) {
      return rejectWithValue(getErrorMessage(error));
    }
  }
);

const discountSlice = createSlice({
  name: 'discount',
  initialState,
  reducers: {
    setSelectedDiscount: (state, action: PayloadAction<Discount | null>) => {
      state.selectedDiscount = action.payload;
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
      .addCase(fetchDiscountsAsync.pending, (state) => {
        state.loading = true;
        state.error = null;
      })
      .addCase(fetchDiscountsAsync.fulfilled, (state, action) => {
        state.loading = false;
        state.discounts = action.payload.response.data || [];
        state.totalCount = action.payload.response.totalCount || 0;
        state.pageNumber = action.payload.response.currentPage || 1;
        state.pageSize = action.payload.response.pageSize || 10;
        if (action.payload.search !== undefined) {
          state.search = action.payload.search;
        }
        state.error = null;
      })
      .addCase(fetchDiscountsAsync.rejected, (state, action) => {
        state.loading = false;
        state.error = action.payload as string;
      })
      .addCase(fetchDiscountByIdAsync.pending, (state) => {
        state.loading = true;
        state.error = null;
      })
      .addCase(fetchDiscountByIdAsync.fulfilled, (state, action) => {
        state.loading = false;
        state.selectedDiscount = action.payload;
        state.error = null;
      })
      .addCase(fetchDiscountByIdAsync.rejected, (state, action) => {
        state.loading = false;
        state.error = action.payload as string;
      })
      .addCase(createDiscountAsync.pending, (state) => {
        state.loading = true;
        state.error = null;
      })
      .addCase(createDiscountAsync.fulfilled, (state) => {
        state.loading = false;
        state.error = null;
      })
      .addCase(createDiscountAsync.rejected, (state, action) => {
        state.loading = false;
        state.error = action.payload as string;
      })
      .addCase(updateDiscountAsync.pending, (state) => {
        state.loading = true;
        state.error = null;
      })
      .addCase(updateDiscountAsync.fulfilled, (state, action) => {
        state.loading = false;
        const index = state.discounts.findIndex((c) => c.id === action.payload.id);
        if (index !== -1 && state.selectedDiscount?.id === action.payload.id) {
          state.selectedDiscount = { ...state.selectedDiscount, ...action.payload };
        }
        state.error = null;
      })
      .addCase(updateDiscountAsync.rejected, (state, action) => {
        state.loading = false;
        state.error = action.payload as string;
      })
      .addCase(deleteDiscountAsync.pending, (state) => {
        state.loading = true;
        state.error = null;
      })
      .addCase(deleteDiscountAsync.fulfilled, (state, action) => {
        state.loading = false;
        state.discounts = state.discounts.filter((c) => c.id !== action.payload);
        state.totalCount = Math.max(0, state.totalCount - 1);
        if (state.selectedDiscount?.id === action.payload) {
          state.selectedDiscount = null;
        }
        state.error = null;
      })
      .addCase(deleteDiscountAsync.rejected, (state, action) => {
        state.loading = false;
        state.error = action.payload as string;
      });
  },
});

export const {
  setSelectedDiscount,
  clearError,
  setPageNumber,
  setPageSize,
  setSearch,
} = discountSlice.actions;
export default discountSlice.reducer;
