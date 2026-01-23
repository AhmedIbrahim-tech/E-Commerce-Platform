import { createSlice, createAsyncThunk, PayloadAction } from '@reduxjs/toolkit';
import { AxiosError } from 'axios';
import { accountService } from '@/services/accounts/accountService';
import type {
  Account,
  AccountListResponse,
  CreateAccountRequest,
  UpdateAccountRequest,
} from '@/types';

interface ApiErrorResponse {
  message?: string;
  errors?: Record<string, string[]>;
}

interface AccountState {
  accounts: Account[];
  selectedAccount: Account | null;
  loading: boolean;
  error: string | null;
  totalCount: number;
  pageNumber: number;
  pageSize: number;
  search?: string;
}

const initialState: AccountState = {
  accounts: [],
  selectedAccount: null,
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

export const fetchAccountsAsync = createAsyncThunk(
  'account/fetchAccounts',
  async (
    params: { pageNumber?: number; pageSize?: number; search?: string },
    { rejectWithValue }
  ) => {
    try {
      const response = await accountService.getAccountPaginatedList(
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

export const fetchAccountByIdAsync = createAsyncThunk(
  'account/fetchAccountById',
  async (id: string, { rejectWithValue }) => {
    try {
      const account = await accountService.getAccountById(id);
      return account;
    } catch (error) {
      return rejectWithValue(getErrorMessage(error));
    }
  }
);

export const createAccountAsync = createAsyncThunk(
  'account/createAccount',
  async (data: CreateAccountRequest, { rejectWithValue }) => {
    try {
      await accountService.createAccount(data);
      return data;
    } catch (error) {
      return rejectWithValue(getErrorMessage(error));
    }
  }
);

export const updateAccountAsync = createAsyncThunk(
  'account/updateAccount',
  async (data: UpdateAccountRequest, { rejectWithValue }) => {
    try {
      await accountService.updateAccount(data);
      return data;
    } catch (error) {
      return rejectWithValue(getErrorMessage(error));
    }
  }
);

export const deleteAccountAsync = createAsyncThunk(
  'account/deleteAccount',
  async (id: string, { rejectWithValue }) => {
    try {
      await accountService.deleteAccount(id);
      return id;
    } catch (error) {
      return rejectWithValue(getErrorMessage(error));
    }
  }
);

const accountSlice = createSlice({
  name: 'account',
  initialState,
  reducers: {
    setSelectedAccount: (state, action: PayloadAction<Account | null>) => {
      state.selectedAccount = action.payload;
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
      .addCase(fetchAccountsAsync.pending, (state) => {
        state.loading = true;
        state.error = null;
      })
      .addCase(fetchAccountsAsync.fulfilled, (state, action) => {
        state.loading = false;
        state.accounts = action.payload.response.data || [];
        state.totalCount = action.payload.response.totalCount || 0;
        state.pageNumber = action.payload.response.currentPage || 1;
        state.pageSize = action.payload.response.pageSize || 10;
        if (action.payload.search !== undefined) {
          state.search = action.payload.search;
        }
        state.error = null;
      })
      .addCase(fetchAccountsAsync.rejected, (state, action) => {
        state.loading = false;
        state.error = action.payload as string;
      })
      .addCase(fetchAccountByIdAsync.pending, (state) => {
        state.loading = true;
        state.error = null;
      })
      .addCase(fetchAccountByIdAsync.fulfilled, (state, action) => {
        state.loading = false;
        state.selectedAccount = action.payload;
        state.error = null;
      })
      .addCase(fetchAccountByIdAsync.rejected, (state, action) => {
        state.loading = false;
        state.error = action.payload as string;
      })
      .addCase(createAccountAsync.pending, (state) => {
        state.loading = true;
        state.error = null;
      })
      .addCase(createAccountAsync.fulfilled, (state) => {
        state.loading = false;
        state.error = null;
      })
      .addCase(createAccountAsync.rejected, (state, action) => {
        state.loading = false;
        state.error = action.payload as string;
      })
      .addCase(updateAccountAsync.pending, (state) => {
        state.loading = true;
        state.error = null;
      })
      .addCase(updateAccountAsync.fulfilled, (state, action) => {
        state.loading = false;
        const index = state.accounts.findIndex((c) => c.id === action.payload.id);
        if (index !== -1 && state.selectedAccount?.id === action.payload.id) {
          state.selectedAccount = { ...state.selectedAccount, ...action.payload };
        }
        state.error = null;
      })
      .addCase(updateAccountAsync.rejected, (state, action) => {
        state.loading = false;
        state.error = action.payload as string;
      })
      .addCase(deleteAccountAsync.pending, (state) => {
        state.loading = true;
        state.error = null;
      })
      .addCase(deleteAccountAsync.fulfilled, (state, action) => {
        state.loading = false;
        state.accounts = state.accounts.filter((c) => c.id !== action.payload);
        state.totalCount = Math.max(0, state.totalCount - 1);
        if (state.selectedAccount?.id === action.payload) {
          state.selectedAccount = null;
        }
        state.error = null;
      })
      .addCase(deleteAccountAsync.rejected, (state, action) => {
        state.loading = false;
        state.error = action.payload as string;
      });
  },
});

export const {
  setSelectedAccount,
  clearError,
  setPageNumber,
  setPageSize,
  setSearch,
} = accountSlice.actions;
export default accountSlice.reducer;
