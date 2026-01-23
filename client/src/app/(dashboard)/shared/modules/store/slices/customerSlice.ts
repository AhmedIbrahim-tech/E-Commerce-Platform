import { createSlice, createAsyncThunk, PayloadAction } from '@reduxjs/toolkit';
import { AxiosError } from 'axios';
import { customerService } from '@/app/(dashboard)/shared/modules/api/users/customerService';
import type {
  Customer,
  CustomerListResponse,
  CreateCustomerRequest,
  UpdateCustomerRequest,
} from '@/types';

interface ApiErrorResponse {
  message?: string;
  errors?: Record<string, string[]>;
}

interface CustomerState {
  customers: Customer[];
  selectedCustomer: Customer | null;
  loading: boolean;
  error: string | null;
  totalCount: number;
  pageNumber: number;
  pageSize: number;
  search?: string;
}

const initialState: CustomerState = {
  customers: [],
  selectedCustomer: null,
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

export const fetchCustomersAsync = createAsyncThunk(
  'customer/fetchCustomers',
  async (
    params: { pageNumber?: number; pageSize?: number; search?: string },
    { rejectWithValue }
  ) => {
    try {
      const response = await customerService.getCustomerPaginatedList(
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

export const fetchCustomerByIdAsync = createAsyncThunk(
  'customer/fetchCustomerById',
  async (id: string, { rejectWithValue }) => {
    try {
      const customer = await customerService.getCustomerById(id);
      return customer;
    } catch (error) {
      return rejectWithValue(getErrorMessage(error));
    }
  }
);

export const createCustomerAsync = createAsyncThunk(
  'customer/createCustomer',
  async (data: CreateCustomerRequest, { rejectWithValue }) => {
    try {
      await customerService.createCustomer(data);
      return data;
    } catch (error) {
      return rejectWithValue(getErrorMessage(error));
    }
  }
);

export const updateCustomerAsync = createAsyncThunk(
  'customer/updateCustomer',
  async (data: UpdateCustomerRequest, { rejectWithValue }) => {
    try {
      await customerService.updateCustomer(data);
      return data;
    } catch (error) {
      return rejectWithValue(getErrorMessage(error));
    }
  }
);

export const deleteCustomerAsync = createAsyncThunk(
  'customer/deleteCustomer',
  async (id: string, { rejectWithValue }) => {
    try {
      await customerService.deleteCustomer(id);
      return id;
    } catch (error) {
      return rejectWithValue(getErrorMessage(error));
    }
  }
);

export const toggleCustomerStatusAsync = createAsyncThunk(
  'customer/toggleCustomerStatus',
  async (id: string, { rejectWithValue }) => {
    try {
      const message = await customerService.toggleCustomerStatus(id);
      return { id, message };
    } catch (error) {
      return rejectWithValue(getErrorMessage(error));
    }
  }
);

const customerSlice = createSlice({
  name: 'customer',
  initialState,
  reducers: {
    setSelectedCustomer: (state, action: PayloadAction<Customer | null>) => {
      state.selectedCustomer = action.payload;
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
      .addCase(fetchCustomersAsync.pending, (state) => {
        state.loading = true;
        state.error = null;
      })
      .addCase(fetchCustomersAsync.fulfilled, (state, action) => {
        state.loading = false;
        state.customers = action.payload.response.data || [];
        state.totalCount = action.payload.response.totalCount || 0;
        state.pageNumber = action.payload.response.currentPage || 1;
        state.pageSize = action.payload.response.pageSize || 10;
        if (action.payload.search !== undefined) {
          state.search = action.payload.search;
        }
        state.error = null;
      })
      .addCase(fetchCustomersAsync.rejected, (state, action) => {
        state.loading = false;
        state.error = action.payload as string;
      })
      .addCase(fetchCustomerByIdAsync.pending, (state) => {
        state.loading = true;
        state.error = null;
      })
      .addCase(fetchCustomerByIdAsync.fulfilled, (state, action) => {
        state.loading = false;
        state.selectedCustomer = action.payload;
        state.error = null;
      })
      .addCase(fetchCustomerByIdAsync.rejected, (state, action) => {
        state.loading = false;
        state.error = action.payload as string;
      })
      .addCase(createCustomerAsync.pending, (state) => {
        state.loading = true;
        state.error = null;
      })
      .addCase(createCustomerAsync.fulfilled, (state) => {
        state.loading = false;
        state.error = null;
      })
      .addCase(createCustomerAsync.rejected, (state, action) => {
        state.loading = false;
        state.error = action.payload as string;
      })
      .addCase(updateCustomerAsync.pending, (state) => {
        state.loading = true;
        state.error = null;
      })
      .addCase(updateCustomerAsync.fulfilled, (state, action) => {
        state.loading = false;
        const index = state.customers.findIndex((c) => c.id === action.payload.id);
        if (index !== -1 && state.selectedCustomer?.id === action.payload.id) {
          state.selectedCustomer = { ...state.selectedCustomer, ...action.payload };
        }
        state.error = null;
      })
      .addCase(updateCustomerAsync.rejected, (state, action) => {
        state.loading = false;
        state.error = action.payload as string;
      })
      .addCase(deleteCustomerAsync.pending, (state) => {
        state.loading = true;
        state.error = null;
      })
      .addCase(deleteCustomerAsync.fulfilled, (state, action) => {
        state.loading = false;
        state.customers = state.customers.filter((c) => c.id !== action.payload);
        state.totalCount = Math.max(0, state.totalCount - 1);
        if (state.selectedCustomer?.id === action.payload) {
          state.selectedCustomer = null;
        }
        state.error = null;
      })
      .addCase(deleteCustomerAsync.rejected, (state, action) => {
        state.loading = false;
        state.error = action.payload as string;
      })
      .addCase(toggleCustomerStatusAsync.pending, (state) => {
        state.loading = true;
        state.error = null;
      })
      .addCase(toggleCustomerStatusAsync.fulfilled, (state, action) => {
        state.loading = false;
        const index = state.customers.findIndex((c) => c.id === action.payload.id);
        if (index !== -1) {
          // Toggle the isDeleted status
          state.customers[index] = { ...state.customers[index], isDeleted: !state.customers[index].isDeleted };
        }
        if (state.selectedCustomer?.id === action.payload.id) {
          state.selectedCustomer = { ...state.selectedCustomer, isDeleted: !state.selectedCustomer.isDeleted };
        }
        state.error = null;
      })
      .addCase(toggleCustomerStatusAsync.rejected, (state, action) => {
        state.loading = false;
        state.error = action.payload as string;
      });
  },
});

export const {
  setSelectedCustomer,
  clearError,
  setPageNumber,
  setPageSize,
  setSearch,
} = customerSlice.actions;
export default customerSlice.reducer;
