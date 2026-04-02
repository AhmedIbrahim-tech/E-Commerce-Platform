import { createSlice, createAsyncThunk, PayloadAction } from '@reduxjs/toolkit';
import { ProductListItem } from '@/types/product';
import { PagedResult } from '@/types/api';
import { productService, ProductsPagedParams } from '@/services/productService';

interface ProductState {
    items: ProductListItem[];
    totalCount: number;
    totalPages: number;
    currentPage: number;
    pageSize: number;
    loading: boolean;
    error: string | null;
}

const initialState: ProductState = {
    items: [],
    totalCount: 0,
    totalPages: 0,
    currentPage: 1,
    pageSize: 20,
    loading: false,
    error: null,
};

export const fetchProducts = createAsyncThunk(
    'products/fetchAll',
    async (params: ProductsPagedParams, { rejectWithValue }) => {
        try {
            const response = await productService.getProductsPaged(params);
            if (response.succeeded) {
                return response.data;
            } else {
                return rejectWithValue(response.message);
            }
        } catch (error: unknown) {
            const err = error as { response?: { data?: { message?: string } } };
            return rejectWithValue(err.response?.data?.message || 'Failed to fetch products');
        }
    }
);

const productSlice = createSlice({
    name: 'products',
    initialState,
    reducers: {
        clearError: (state) => {
            state.error = null;
        },
    },
    extraReducers: (builder) => {
        builder
            .addCase(fetchProducts.pending, (state) => {
                state.loading = true;
                state.error = null;
            })
            .addCase(fetchProducts.fulfilled, (state, action: PayloadAction<PagedResult<ProductListItem>>) => {
                state.loading = false;
                state.items = action.payload.items;
                state.totalCount = action.payload.totalCount;
                state.currentPage = action.payload.pageIndex;
                state.pageSize = action.payload.pageSize;
                state.totalPages = action.payload.totalPages ?? Math.ceil(action.payload.totalCount / action.payload.pageSize);
            })
            .addCase(fetchProducts.rejected, (state, action) => {
                state.loading = false;
                state.error = action.payload as string;
            });
    },
});

export const { clearError } = productSlice.actions;
export default productSlice.reducer;
