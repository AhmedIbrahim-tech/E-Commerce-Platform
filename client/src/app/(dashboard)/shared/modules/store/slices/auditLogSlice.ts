import { createSlice, createAsyncThunk, PayloadAction } from '@reduxjs/toolkit';
import { AxiosError } from 'axios';
import { auditLogService } from '@/services/auth/auditLogService';
import type {
  AuditLog,
  AuditLogListResponse,
  AuditLogFilters,
} from '@/types';

interface ApiErrorResponse {
  message?: string;
  errors?: Record<string, string[]>;
}

interface AuditLogState {
  auditLogs: AuditLog[];
  selectedAuditLog: AuditLog | null;
  loading: boolean;
  error: string | null;
  totalCount: number;
  pageNumber: number;
  pageSize: number;
  filters: AuditLogFilters;
}

const initialState: AuditLogState = {
  auditLogs: [],
  selectedAuditLog: null,
  loading: false,
  error: null,
  totalCount: 0,
  pageNumber: 1,
  pageSize: 10,
  filters: {
    sortBy: 'CreatedAtDesc',
  },
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

export const fetchAuditLogsAsync = createAsyncThunk(
  'auditLog/fetchAuditLogs',
  async (
    params: { pageNumber?: number; pageSize?: number; filters?: AuditLogFilters },
    { rejectWithValue }
  ) => {
    try {
      const response = await auditLogService.getAuditLogPaginatedList({
        pageNumber: params.pageNumber || 1,
        pageSize: params.pageSize || 10,
        eventType: params.filters?.eventType,
        userId: params.filters?.userId,
        startDate: params.filters?.startDate,
        endDate: params.filters?.endDate,
        search: params.filters?.search,
        sortBy: params.filters?.sortBy === 'CreatedAtAsc' ? 1 : params.filters?.sortBy === 'CreatedAtDesc' ? 2 : 0,
      });
      return { response, filters: params.filters };
    } catch (error) {
      return rejectWithValue(getErrorMessage(error));
    }
  }
);

export const fetchAuditLogByIdAsync = createAsyncThunk(
  'auditLog/fetchAuditLogById',
  async (id: string, { rejectWithValue }) => {
    try {
      // Note: getAuditLogById method doesn't exist in auditLogService
      // This thunk is kept for compatibility but returns null
      return null;
    } catch (error) {
      return rejectWithValue(getErrorMessage(error));
    }
  }
);

const auditLogSlice = createSlice({
  name: 'auditLog',
  initialState,
  reducers: {
    setSelectedAuditLog: (state, action: PayloadAction<AuditLog | null>) => {
      state.selectedAuditLog = action.payload;
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
    setFilters: (state, action: PayloadAction<Partial<AuditLogFilters>>) => {
      state.filters = { ...state.filters, ...action.payload };
      state.pageNumber = 1;
    },
    clearFilters: (state) => {
      state.filters = { sortBy: 'CreatedAtDesc' };
      state.pageNumber = 1;
    },
  },
  extraReducers: (builder) => {
    builder
      .addCase(fetchAuditLogsAsync.pending, (state) => {
        state.loading = true;
        state.error = null;
      })
      .addCase(fetchAuditLogsAsync.fulfilled, (state, action) => {
        state.loading = false;
        // Map AuditLogItem to AuditLog (createdTime -> createdAt)
        state.auditLogs = (action.payload.response.data || []).map((item) => ({
          ...item,
          createdAt: (item as any).createdTime || (item as any).createdAt || '',
        })) as AuditLog[];
        state.totalCount = action.payload.response.totalCount || 0;
        state.pageNumber = action.payload.response.currentPage || 1;
        state.pageSize = action.payload.response.pageSize || 10;
        if (action.payload.filters) {
          state.filters = { ...state.filters, ...action.payload.filters };
        }
        state.error = null;
      })
      .addCase(fetchAuditLogsAsync.rejected, (state, action) => {
        state.loading = false;
        state.error = action.payload as string;
      })
      .addCase(fetchAuditLogByIdAsync.pending, (state) => {
        state.loading = true;
        state.error = null;
      })
      .addCase(fetchAuditLogByIdAsync.fulfilled, (state, action) => {
        state.loading = false;
        state.selectedAuditLog = action.payload;
        state.error = null;
      })
      .addCase(fetchAuditLogByIdAsync.rejected, (state, action) => {
        state.loading = false;
        state.error = action.payload as string;
      });
  },
});

export const {
  setSelectedAuditLog,
  clearError,
  setPageNumber,
  setPageSize,
  setFilters,
  clearFilters,
} = auditLogSlice.actions;
export default auditLogSlice.reducer;
