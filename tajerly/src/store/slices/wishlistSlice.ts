import { createSlice, createAsyncThunk, PayloadAction } from '@reduxjs/toolkit';
import { WishlistItem } from '@/types/wishlist';

interface WishlistState {
    items: WishlistItem[];
    loading: boolean;
    error: string | null;
}

const initialState: WishlistState = {
    items: [],
    loading: false,
    error: null,
};

const wishlistSlice = createSlice({
    name: 'wishlist',
    initialState,
    reducers: {
        setWishlistItems: (state, action: PayloadAction<WishlistItem[]>) => {
            state.items = action.payload;
        },
        addWishlistItem: (state, action: PayloadAction<WishlistItem>) => {
            if (!state.items.find((i) => i.productId === action.payload.productId)) {
                state.items.push(action.payload);
            }
        },
        removeWishlistItem: (state, action: PayloadAction<number>) => {
            state.items = state.items.filter((i) => i.productId !== action.payload);
        },
        clearWishlist: (state) => {
            state.items = [];
        },
        setWishlistLoading: (state, action: PayloadAction<boolean>) => {
            state.loading = action.payload;
        },
        setWishlistError: (state, action: PayloadAction<string | null>) => {
            state.error = action.payload;
        },
    },
});

export const {
    setWishlistItems,
    addWishlistItem,
    removeWishlistItem,
    clearWishlist,
    setWishlistLoading,
    setWishlistError,
} = wishlistSlice.actions;
export default wishlistSlice.reducer;
