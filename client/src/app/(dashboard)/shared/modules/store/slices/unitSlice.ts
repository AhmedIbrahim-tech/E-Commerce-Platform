import { createSlice, createAsyncThunk, PayloadAction } from '@reduxjs/toolkit';
import { AxiosError } from 'axios';
import { unitService } from '@/services/catalog/unitService';
import type {
  Unit,
  UnitListResponse,
  CreateUnitRequest,
  UpdateUnitRequest,
} from '@/types';

interface ApiErrorResponse {
  message?: string;
  errors?: Record<string, string[]>;
}

interface UnitState {
  units: Unit[];
  selectedUnit: Unit | null;
  loading: boolean;
  error: string | null;
  totalCount: number;
  pageNumber: number;
  pageSize: number;
  search?: string;
}

const initialState: UnitState = {
  units: [],
  selectedUnit: null,
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

export const fetchUnitsAsync = createAsyncThunk(
  'unit/fetchUnits',
  async (
    params: { pageNumber?: number; pageSize?: number; search?: string },
    { rejectWithValue }
  ) => {
    try {
      const response = await unitService.getUnitPaginatedList(
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

export const fetchUnitByIdAsync = createAsyncThunk(
  'unit/fetchUnitById',
  async (id: string, { rejectWithValue }) => {
    try {
      const unit = await unitService.getUnitById(id);
      return unit;
    } catch (error) {
      return rejectWithValue(getErrorMessage(error));
    }
  }
);

export const createUnitAsync = createAsyncThunk(
  'unit/createUnit',
  async (data: CreateUnitRequest, { rejectWithValue }) => {
    try {
      await unitService.createUnit(data);
      return data;
    } catch (error) {
      return rejectWithValue(getErrorMessage(error));
    }
  }
);

export const updateUnitAsync = createAsyncThunk(
  'unit/updateUnit',
  async (data: UpdateUnitRequest, { rejectWithValue }) => {
    try {
      await unitService.updateUnit(data);
      return data;
    } catch (error) {
      return rejectWithValue(getErrorMessage(error));
    }
  }
);

export const deleteUnitAsync = createAsyncThunk(
  'unit/deleteUnit',
  async (id: string, { rejectWithValue }) => {
    try {
      await unitService.deleteUnit(id);
      return id;
    } catch (error) {
      return rejectWithValue(getErrorMessage(error));
    }
  }
);

const unitSlice = createSlice({
  name: 'unit',
  initialState,
  reducers: {
    setSelectedUnit: (state, action: PayloadAction<Unit | null>) => {
      state.selectedUnit = action.payload;
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
      .addCase(fetchUnitsAsync.pending, (state) => {
        state.loading = true;
        state.error = null;
      })
      .addCase(fetchUnitsAsync.fulfilled, (state, action) => {
        state.loading = false;
        state.units = action.payload.response.data || [];
        state.totalCount = action.payload.response.totalCount || 0;
        state.pageNumber = action.payload.response.currentPage || 1;
        state.pageSize = action.payload.response.pageSize || 10;
        if (action.payload.search !== undefined) {
          state.search = action.payload.search;
        }
        state.error = null;
      })
      .addCase(fetchUnitsAsync.rejected, (state, action) => {
        state.loading = false;
        state.error = action.payload as string;
      })
      .addCase(fetchUnitByIdAsync.pending, (state) => {
        state.loading = true;
        state.error = null;
      })
      .addCase(fetchUnitByIdAsync.fulfilled, (state, action) => {
        state.loading = false;
        state.selectedUnit = action.payload;
        state.error = null;
      })
      .addCase(fetchUnitByIdAsync.rejected, (state, action) => {
        state.loading = false;
        state.error = action.payload as string;
      })
      .addCase(createUnitAsync.pending, (state) => {
        state.loading = true;
        state.error = null;
      })
      .addCase(createUnitAsync.fulfilled, (state) => {
        state.loading = false;
        state.error = null;
      })
      .addCase(createUnitAsync.rejected, (state, action) => {
        state.loading = false;
        state.error = action.payload as string;
      })
      .addCase(updateUnitAsync.pending, (state) => {
        state.loading = true;
        state.error = null;
      })
      .addCase(updateUnitAsync.fulfilled, (state, action) => {
        state.loading = false;
        const index = state.units.findIndex((c) => c.id === action.payload.id);
        if (index !== -1 && state.selectedUnit?.id === action.payload.id) {
          state.selectedUnit = { ...state.selectedUnit, ...action.payload };
        }
        state.error = null;
      })
      .addCase(updateUnitAsync.rejected, (state, action) => {
        state.loading = false;
        state.error = action.payload as string;
      })
      .addCase(deleteUnitAsync.pending, (state) => {
        state.loading = true;
        state.error = null;
      })
      .addCase(deleteUnitAsync.fulfilled, (state, action) => {
        state.loading = false;
        state.units = state.units.filter((c) => c.id !== action.payload);
        state.totalCount = Math.max(0, state.totalCount - 1);
        if (state.selectedUnit?.id === action.payload) {
          state.selectedUnit = null;
        }
        state.error = null;
      })
      .addCase(deleteUnitAsync.rejected, (state, action) => {
        state.loading = false;
        state.error = action.payload as string;
      });
  },
});

export const {
  setSelectedUnit,
  clearError,
  setPageNumber,
  setPageSize,
  setSearch,
} = unitSlice.actions;
export default unitSlice.reducer;
