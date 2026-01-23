import { createSlice, createAsyncThunk, PayloadAction } from '@reduxjs/toolkit';
import { AxiosError } from 'axios';
import { subCategoryService } from '@/services/catalog/subCategoryService';
import type {
  SubCategory,
  SubCategoryListResponse,
  CreateSubCategoryRequest,
  UpdateSubCategoryRequest,
} from '@/types';

interface ApiErrorResponse {
  message?: string;
  errors?: Record<string, string[]>;
}

interface SubCategoryState {
  subCategories: SubCategory[];
  selectedSubCategory: SubCategory | null;
  loading: boolean;
  error: string | null;
  totalCount: number;
  pageNumber: number;
  pageSize: number;
  search?: string;
}

const initialState: SubCategoryState = {
  subCategories: [],
  selectedSubCategory: null,
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

export const fetchSubCategoriesAsync = createAsyncThunk(
  'subCategory/fetchSubCategories',
  async (
    params: { pageNumber?: number; pageSize?: number; search?: string },
    { rejectWithValue }
  ) => {
    try {
      const response = await subCategoryService.getSubCategoryPaginatedList(
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

export const fetchSubCategoryByIdAsync = createAsyncThunk(
  'subCategory/fetchSubCategoryById',
  async (id: string, { rejectWithValue }) => {
    try {
      const subCategory = await subCategoryService.getSubCategoryById(id);
      return subCategory;
    } catch (error) {
      return rejectWithValue(getErrorMessage(error));
    }
  }
);

export const createSubCategoryAsync = createAsyncThunk(
  'subCategory/createSubCategory',
  async (data: CreateSubCategoryRequest, { rejectWithValue }) => {
    try {
      await subCategoryService.createSubCategory(data);
      return data;
    } catch (error) {
      return rejectWithValue(getErrorMessage(error));
    }
  }
);

export const updateSubCategoryAsync = createAsyncThunk(
  'subCategory/updateSubCategory',
  async (data: UpdateSubCategoryRequest, { rejectWithValue }) => {
    try {
      await subCategoryService.updateSubCategory(data);
      return data;
    } catch (error) {
      return rejectWithValue(getErrorMessage(error));
    }
  }
);

export const deleteSubCategoryAsync = createAsyncThunk(
  'subCategory/deleteSubCategory',
  async (id: string, { rejectWithValue }) => {
    try {
      await subCategoryService.deleteSubCategory(id);
      return id;
    } catch (error) {
      return rejectWithValue(getErrorMessage(error));
    }
  }
);

const subCategorySlice = createSlice({
  name: 'subCategory',
  initialState,
  reducers: {
    setSelectedSubCategory: (state, action: PayloadAction<SubCategory | null>) => {
      state.selectedSubCategory = action.payload;
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
      .addCase(fetchSubCategoriesAsync.pending, (state) => {
        state.loading = true;
        state.error = null;
      })
      .addCase(fetchSubCategoriesAsync.fulfilled, (state, action) => {
        state.loading = false;
        state.subCategories = action.payload.response.data || [];
        state.totalCount = action.payload.response.totalCount || 0;
        state.pageNumber = action.payload.response.currentPage || 1;
        state.pageSize = action.payload.response.pageSize || 10;
        if (action.payload.search !== undefined) {
          state.search = action.payload.search;
        }
        state.error = null;
      })
      .addCase(fetchSubCategoriesAsync.rejected, (state, action) => {
        state.loading = false;
        state.error = action.payload as string;
      })
      .addCase(fetchSubCategoryByIdAsync.pending, (state) => {
        state.loading = true;
        state.error = null;
      })
      .addCase(fetchSubCategoryByIdAsync.fulfilled, (state, action) => {
        state.loading = false;
        state.selectedSubCategory = action.payload;
        state.error = null;
      })
      .addCase(fetchSubCategoryByIdAsync.rejected, (state, action) => {
        state.loading = false;
        state.error = action.payload as string;
      })
      .addCase(createSubCategoryAsync.pending, (state) => {
        state.loading = true;
        state.error = null;
      })
      .addCase(createSubCategoryAsync.fulfilled, (state) => {
        state.loading = false;
        state.error = null;
      })
      .addCase(createSubCategoryAsync.rejected, (state, action) => {
        state.loading = false;
        state.error = action.payload as string;
      })
      .addCase(updateSubCategoryAsync.pending, (state) => {
        state.loading = true;
        state.error = null;
      })
      .addCase(updateSubCategoryAsync.fulfilled, (state, action) => {
        state.loading = false;
        const index = state.subCategories.findIndex((c) => c.id === action.payload.id);
        if (index !== -1 && state.selectedSubCategory?.id === action.payload.id) {
          state.selectedSubCategory = { ...state.selectedSubCategory, ...action.payload };
        }
        state.error = null;
      })
      .addCase(updateSubCategoryAsync.rejected, (state, action) => {
        state.loading = false;
        state.error = action.payload as string;
      })
      .addCase(deleteSubCategoryAsync.pending, (state) => {
        state.loading = true;
        state.error = null;
      })
      .addCase(deleteSubCategoryAsync.fulfilled, (state, action) => {
        state.loading = false;
        state.subCategories = state.subCategories.filter((c) => c.id !== action.payload);
        state.totalCount = Math.max(0, state.totalCount - 1);
        if (state.selectedSubCategory?.id === action.payload) {
          state.selectedSubCategory = null;
        }
        state.error = null;
      })
      .addCase(deleteSubCategoryAsync.rejected, (state, action) => {
        state.loading = false;
        state.error = action.payload as string;
      });
  },
});

export const {
  setSelectedSubCategory,
  clearError,
  setPageNumber,
  setPageSize,
  setSearch,
} = subCategorySlice.actions;
export default subCategorySlice.reducer;
