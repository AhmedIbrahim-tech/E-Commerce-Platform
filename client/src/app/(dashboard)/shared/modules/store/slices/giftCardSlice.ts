import { createSlice, createAsyncThunk, PayloadAction } from '@reduxjs/toolkit';
import { AxiosError } from 'axios';
import { giftCardService } from '@/services/promotions/giftCardService';
import type {
  GiftCard,
  GiftCardListResponse,
  CreateGiftCardRequest,
  UpdateGiftCardRequest,
} from '@/types';

interface ApiErrorResponse {
  message?: string;
  errors?: Record<string, string[]>;
}

interface GiftCardState {
  giftCards: GiftCard[];
  selectedGiftCard: GiftCard | null;
  loading: boolean;
  error: string | null;
  totalCount: number;
  pageNumber: number;
  pageSize: number;
  search?: string;
}

const initialState: GiftCardState = {
  giftCards: [],
  selectedGiftCard: null,
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

export const fetchGiftCardsAsync = createAsyncThunk(
  'giftCard/fetchGiftCards',
  async (
    params: { pageNumber?: number; pageSize?: number; search?: string },
    { rejectWithValue }
  ) => {
    try {
      const response = await giftCardService.getGiftCardPaginatedList(
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

export const fetchGiftCardByIdAsync = createAsyncThunk(
  'giftCard/fetchGiftCardById',
  async (id: string, { rejectWithValue }) => {
    try {
      const giftCard = await giftCardService.getGiftCardById(id);
      return giftCard;
    } catch (error) {
      return rejectWithValue(getErrorMessage(error));
    }
  }
);

export const createGiftCardAsync = createAsyncThunk(
  'giftCard/createGiftCard',
  async (data: CreateGiftCardRequest, { rejectWithValue }) => {
    try {
      await giftCardService.createGiftCard(data);
      return data;
    } catch (error) {
      return rejectWithValue(getErrorMessage(error));
    }
  }
);

export const updateGiftCardAsync = createAsyncThunk(
  'giftCard/updateGiftCard',
  async (data: UpdateGiftCardRequest, { rejectWithValue }) => {
    try {
      await giftCardService.updateGiftCard(data);
      return data;
    } catch (error) {
      return rejectWithValue(getErrorMessage(error));
    }
  }
);

export const deleteGiftCardAsync = createAsyncThunk(
  'giftCard/deleteGiftCard',
  async (id: string, { rejectWithValue }) => {
    try {
      await giftCardService.deleteGiftCard(id);
      return id;
    } catch (error) {
      return rejectWithValue(getErrorMessage(error));
    }
  }
);

const giftCardSlice = createSlice({
  name: 'giftCard',
  initialState,
  reducers: {
    setSelectedGiftCard: (state, action: PayloadAction<GiftCard | null>) => {
      state.selectedGiftCard = action.payload;
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
      .addCase(fetchGiftCardsAsync.pending, (state) => {
        state.loading = true;
        state.error = null;
      })
      .addCase(fetchGiftCardsAsync.fulfilled, (state, action) => {
        state.loading = false;
        state.giftCards = action.payload.response.data || [];
        state.totalCount = action.payload.response.totalCount || 0;
        state.pageNumber = action.payload.response.currentPage || 1;
        state.pageSize = action.payload.response.pageSize || 10;
        if (action.payload.search !== undefined) {
          state.search = action.payload.search;
        }
        state.error = null;
      })
      .addCase(fetchGiftCardsAsync.rejected, (state, action) => {
        state.loading = false;
        state.error = action.payload as string;
      })
      .addCase(fetchGiftCardByIdAsync.pending, (state) => {
        state.loading = true;
        state.error = null;
      })
      .addCase(fetchGiftCardByIdAsync.fulfilled, (state, action) => {
        state.loading = false;
        state.selectedGiftCard = action.payload;
        state.error = null;
      })
      .addCase(fetchGiftCardByIdAsync.rejected, (state, action) => {
        state.loading = false;
        state.error = action.payload as string;
      })
      .addCase(createGiftCardAsync.pending, (state) => {
        state.loading = true;
        state.error = null;
      })
      .addCase(createGiftCardAsync.fulfilled, (state) => {
        state.loading = false;
        state.error = null;
      })
      .addCase(createGiftCardAsync.rejected, (state, action) => {
        state.loading = false;
        state.error = action.payload as string;
      })
      .addCase(updateGiftCardAsync.pending, (state) => {
        state.loading = true;
        state.error = null;
      })
      .addCase(updateGiftCardAsync.fulfilled, (state, action) => {
        state.loading = false;
        const index = state.giftCards.findIndex((c) => c.id === action.payload.id);
        if (index !== -1 && state.selectedGiftCard?.id === action.payload.id) {
          state.selectedGiftCard = { ...state.selectedGiftCard, ...action.payload };
        }
        state.error = null;
      })
      .addCase(updateGiftCardAsync.rejected, (state, action) => {
        state.loading = false;
        state.error = action.payload as string;
      })
      .addCase(deleteGiftCardAsync.pending, (state) => {
        state.loading = true;
        state.error = null;
      })
      .addCase(deleteGiftCardAsync.fulfilled, (state, action) => {
        state.loading = false;
        state.giftCards = state.giftCards.filter((c) => c.id !== action.payload);
        state.totalCount = Math.max(0, state.totalCount - 1);
        if (state.selectedGiftCard?.id === action.payload) {
          state.selectedGiftCard = null;
        }
        state.error = null;
      })
      .addCase(deleteGiftCardAsync.rejected, (state, action) => {
        state.loading = false;
        state.error = action.payload as string;
      });
  },
});

export const {
  setSelectedGiftCard,
  clearError,
  setPageNumber,
  setPageSize,
  setSearch,
} = giftCardSlice.actions;
export default giftCardSlice.reducer;
