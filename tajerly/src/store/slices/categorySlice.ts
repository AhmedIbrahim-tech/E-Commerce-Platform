import { createSlice, createAsyncThunk, PayloadAction } from '@reduxjs/toolkit';
import { Category } from '@/types/category';
import { categoryService } from '@/services/categoryService';

interface CategoryState {
    items: Category[];
    loading: boolean;
    error: string | null;
}

const initialState: CategoryState = {
    items: [],
    loading: false,
    error: null,
};

export const fetchCategories = createAsyncThunk(
    'categories/fetchAll',
    async (options: { activeOnly?: boolean } | void, { rejectWithValue }) => {
        try {
            const activeOnly = options?.activeOnly ?? false;
            const response = await categoryService.getAllCategories(activeOnly);
            if (response.succeeded) {
                return response.data;
            } else {
                return rejectWithValue(response.message);
            }
        } catch (error: unknown) {
            const err = error as { response?: { data?: { message?: string } } };
            return rejectWithValue(err.response?.data?.message || 'Failed to fetch categories');
        }
    }
);

export const addCategory = createAsyncThunk(
    'categories/add',
    async (data: Parameters<typeof categoryService.createCategory>[0], { rejectWithValue }) => {
        try {
            const response = await categoryService.createCategory(data);
            if (response.succeeded) return response.data;
            return rejectWithValue(response.message);
        } catch (error: unknown) {
            const err = error as { response?: { data?: { message?: string } } };
            return rejectWithValue(err.response?.data?.message || 'Failed to create category');
        }
    }
);

export const editCategory = createAsyncThunk(
    'categories/edit',
    async ({ id, data }: { id: number; data: Parameters<typeof categoryService.updateCategory>[1] }, { rejectWithValue }) => {
        try {
            const response = await categoryService.updateCategory(id, data);
            if (response.succeeded) return response.data;
            return rejectWithValue(response.message);
        } catch (error: unknown) {
            const err = error as { response?: { data?: { message?: string } } };
            return rejectWithValue(err.response?.data?.message || 'Failed to update category');
        }
    }
);

export const removeCategory = createAsyncThunk(
    'categories/remove',
    async (id: number, { rejectWithValue }) => {
        try {
            const response = await categoryService.deleteCategory(id);
            if (response.succeeded) return id;
            return rejectWithValue(response.message);
        } catch (error: unknown) {
            const err = error as { response?: { data?: { message?: string } } };
            return rejectWithValue(err.response?.data?.message || 'Failed to delete category');
        }
    }
);

const categorySlice = createSlice({
    name: 'categories',
    initialState,
    reducers: {
        clearError: (state) => {
            state.error = null;
        }
    },
    extraReducers: (builder) => {
        builder
            .addCase(fetchCategories.pending, (state) => {
                state.loading = true;
                state.error = null;
            })
            .addCase(fetchCategories.fulfilled, (state, action: PayloadAction<Category[]>) => {
                state.loading = false;
                state.items = action.payload;
            })
            .addCase(fetchCategories.rejected, (state, action) => {
                state.loading = false;
                state.error = action.payload as string;
            })
            .addCase(addCategory.fulfilled, (state, action: PayloadAction<Category>) => {
                state.items.push(action.payload);
            })
            .addCase(editCategory.fulfilled, (state, action: PayloadAction<Category>) => {
                const index = state.items.findIndex(i => i.id === action.payload.id);
                if (index !== -1) state.items[index] = action.payload;
            })
            .addCase(removeCategory.fulfilled, (state, action: PayloadAction<number>) => {
                state.items = state.items.filter(i => Number(i.id) !== action.payload);
            });
    },
});

export const { clearError } = categorySlice.actions;
export default categorySlice.reducer;
