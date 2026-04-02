import { createSlice, createAsyncThunk, PayloadAction } from '@reduxjs/toolkit';
import { CartItem } from '@/types/cart';

interface CartState {
    items: CartItem[];
    totalAmount: number;
    itemCount: number;
    loading: boolean;
    error: string | null;
}

const initialState: CartState = {
    items: [],
    totalAmount: 0,
    itemCount: 0,
    loading: false,
    error: null,
};

function recalcTotals(state: CartState) {
    state.totalAmount = state.items.reduce((sum, item) => sum + item.subtotal, 0);
    state.itemCount = state.items.length;
}

const cartSlice = createSlice({
    name: 'cart',
    initialState,
    reducers: {
        addItem: (state, action: PayloadAction<CartItem>) => {
            const existing = state.items.find((i) => i.productId === action.payload.productId);
            if (existing) {
                existing.quantity += action.payload.quantity;
                existing.subtotal = existing.price * existing.quantity;
            } else {
                state.items.push(action.payload);
            }
            recalcTotals(state);
        },
        updateItemQuantity: (state, action: PayloadAction<{ productId: number; quantity: number }>) => {
            const item = state.items.find((i) => i.productId === action.payload.productId);
            if (item) {
                item.quantity = action.payload.quantity;
                item.subtotal = item.price * item.quantity;
            }
            recalcTotals(state);
        },
        removeItem: (state, action: PayloadAction<number>) => {
            state.items = state.items.filter((i) => i.productId !== action.payload);
            recalcTotals(state);
        },
        clearCart: (state) => {
            state.items = [];
            state.totalAmount = 0;
            state.itemCount = 0;
        },
        setCartError: (state, action: PayloadAction<string | null>) => {
            state.error = action.payload;
        },
    },
});

export const { addItem, updateItemQuantity, removeItem, clearCart, setCartError } = cartSlice.actions;
export default cartSlice.reducer;
