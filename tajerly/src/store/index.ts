import { configureStore, combineReducers, type UnknownAction } from '@reduxjs/toolkit';
import {
    persistStore,
    persistReducer,
    FLUSH,
    REHYDRATE,
    PAUSE,
    PERSIST,
    PURGE,
    REGISTER,
} from 'redux-persist';
import storage from 'redux-persist/lib/storage';
import authReducer from '@/store/slices/authSlice';
import cartReducer from '@/store/slices/cartSlice';
import productReducer from '@/store/slices/productSlice';
import categoryReducer from '@/store/slices/categorySlice';
import wishlistReducer from '@/store/slices/wishlistSlice';

const persistConfig = {
    key: 'tajerly-root',
    version: 1,
    storage,
    whitelist: ['cart'], // Only Cart persisted to bundled localStorage. Auth is managed manually.
};

const rootReducerMap = {
    auth: authReducer,
    cart: cartReducer,
    products: productReducer,
    categories: categoryReducer,
    wishlist: wishlistReducer,
};

const ROOT_SLICE_KEYS = new Set<string>(Object.keys(rootReducerMap));
const combinedReducer = combineReducers(rootReducerMap);

type CombinedState = ReturnType<typeof combinedReducer>;

/** Drop slices from older persisted state so combineReducers does not warn. */
function stripUnknownRootSlices(state: CombinedState | undefined): CombinedState | undefined {
    if (state === undefined || state === null) return state;
    const record = state as Record<string, unknown>;
    let hasUnknown = false;
    for (const key of Object.keys(record)) {
        if (!ROOT_SLICE_KEYS.has(key)) {
            hasUnknown = true;
            break;
        }
    }
    if (!hasUnknown) return state;
    const next = {} as CombinedState;
    for (const key of ROOT_SLICE_KEYS) {
        if (key in record) (next as Record<string, unknown>)[key] = record[key];
    }
    return next;
}

function rootReducer(state: CombinedState | undefined, action: UnknownAction) {
    return combinedReducer(stripUnknownRootSlices(state), action);
}

const persistedReducer = persistReducer(persistConfig, rootReducer);

export const store = configureStore({
    reducer: persistedReducer,
    middleware: (getDefaultMiddleware) =>
        getDefaultMiddleware({
            serializableCheck: {
                ignoredActions: [FLUSH, REHYDRATE, PAUSE, PERSIST, PURGE, REGISTER],
            },
        }),
});

export const persistor = persistStore(store);

export type RootState = ReturnType<typeof store.getState>;
export type AppDispatch = typeof store.dispatch;
