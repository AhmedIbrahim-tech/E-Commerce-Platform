import { createSlice, createAsyncThunk, PayloadAction } from '@reduxjs/toolkit';
import { AxiosError } from 'axios';
import { couponService } from '@/services/promotions/couponService';
import type {
  Coupon,
  CouponListResponse,
  CreateCouponRequest,
  UpdateCouponRequest,
} from '@/types';

interface ApiErrorResponse {
  message?: string;
  errors?: Record<string, string[]>;
}

interface CouponState {
  coupons: Coupon[];
  selectedCoupon: Coupon | null;
  loading: boolean;
  error: string | null;
  totalCount: number;
  pageNumber: number;
  pageSize: number;
  search?: string;
}

const initialState: CouponState = {
  coupons: [],
  selectedCoupon: null,
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

export const fetchCouponsAsync = createAsyncThunk(
  'coupon/fetchCoupons',
  async (
    params: { pageNumber?: number; pageSize?: number; search?: string },
    { rejectWithValue }
  ) => {
    try {
      const response = await couponService.getCouponPaginatedList(
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

export const fetchCouponByIdAsync = createAsyncThunk(
  'coupon/fetchCouponById',
  async (id: string, { rejectWithValue }) => {
    try {
      const coupon = await couponService.getCouponById(id);
      return coupon;
    } catch (error) {
      return rejectWithValue(getErrorMessage(error));
    }
  }
);

export const createCouponAsync = createAsyncThunk(
  'coupon/createCoupon',
  async (data: CreateCouponRequest, { rejectWithValue }) => {
    try {
      await couponService.createCoupon(data);
      return data;
    } catch (error) {
      return rejectWithValue(getErrorMessage(error));
    }
  }
);

export const updateCouponAsync = createAsyncThunk(
  'coupon/updateCoupon',
  async (data: UpdateCouponRequest, { rejectWithValue }) => {
    try {
      await couponService.updateCoupon(data);
      return data;
    } catch (error) {
      return rejectWithValue(getErrorMessage(error));
    }
  }
);

export const deleteCouponAsync = createAsyncThunk(
  'coupon/deleteCoupon',
  async (id: string, { rejectWithValue }) => {
    try {
      await couponService.deleteCoupon(id);
      return id;
    } catch (error) {
      return rejectWithValue(getErrorMessage(error));
    }
  }
);

const couponSlice = createSlice({
  name: 'coupon',
  initialState,
  reducers: {
    setSelectedCoupon: (state, action: PayloadAction<Coupon | null>) => {
      state.selectedCoupon = action.payload;
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
      .addCase(fetchCouponsAsync.pending, (state) => {
        state.loading = true;
        state.error = null;
      })
      .addCase(fetchCouponsAsync.fulfilled, (state, action) => {
        state.loading = false;
        state.coupons = action.payload.response.data || [];
        state.totalCount = action.payload.response.totalCount || 0;
        state.pageNumber = action.payload.response.currentPage || 1;
        state.pageSize = action.payload.response.pageSize || 10;
        if (action.payload.search !== undefined) {
          state.search = action.payload.search;
        }
        state.error = null;
      })
      .addCase(fetchCouponsAsync.rejected, (state, action) => {
        state.loading = false;
        state.error = action.payload as string;
      })
      .addCase(fetchCouponByIdAsync.pending, (state) => {
        state.loading = true;
        state.error = null;
      })
      .addCase(fetchCouponByIdAsync.fulfilled, (state, action) => {
        state.loading = false;
        state.selectedCoupon = action.payload;
        state.error = null;
      })
      .addCase(fetchCouponByIdAsync.rejected, (state, action) => {
        state.loading = false;
        state.error = action.payload as string;
      })
      .addCase(createCouponAsync.pending, (state) => {
        state.loading = true;
        state.error = null;
      })
      .addCase(createCouponAsync.fulfilled, (state) => {
        state.loading = false;
        state.error = null;
      })
      .addCase(createCouponAsync.rejected, (state, action) => {
        state.loading = false;
        state.error = action.payload as string;
      })
      .addCase(updateCouponAsync.pending, (state) => {
        state.loading = true;
        state.error = null;
      })
      .addCase(updateCouponAsync.fulfilled, (state, action) => {
        state.loading = false;
        const index = state.coupons.findIndex((c) => c.id === action.payload.id);
        if (index !== -1 && state.selectedCoupon?.id === action.payload.id) {
          state.selectedCoupon = { ...state.selectedCoupon, ...action.payload };
        }
        state.error = null;
      })
      .addCase(updateCouponAsync.rejected, (state, action) => {
        state.loading = false;
        state.error = action.payload as string;
      })
      .addCase(deleteCouponAsync.pending, (state) => {
        state.loading = true;
        state.error = null;
      })
      .addCase(deleteCouponAsync.fulfilled, (state, action) => {
        state.loading = false;
        state.coupons = state.coupons.filter((c) => c.id !== action.payload);
        state.totalCount = Math.max(0, state.totalCount - 1);
        if (state.selectedCoupon?.id === action.payload) {
          state.selectedCoupon = null;
        }
        state.error = null;
      })
      .addCase(deleteCouponAsync.rejected, (state, action) => {
        state.loading = false;
        state.error = action.payload as string;
      });
  },
});

export const {
  setSelectedCoupon,
  clearError,
  setPageNumber,
  setPageSize,
  setSearch,
} = couponSlice.actions;
export default couponSlice.reducer;
