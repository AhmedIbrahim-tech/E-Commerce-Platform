import { configureStore } from '@reduxjs/toolkit';
import { persistStore, FLUSH, REHYDRATE, PAUSE, PERSIST, PURGE, REGISTER } from 'redux-persist';
import { persistedRootReducer } from './parentReducer';

// ── HMR-safe singleton ──────────────────────────────────────────────
// During development, Next.js HMR re-imports modules on every file change.
// Without caching, a brand-new store + persistor would be created each time,
// causing redux-persist to re-rehydrate and briefly losing all state (including
// auth tokens), which redirects the user to the login page.
//
// By caching on globalThis the store and persistor survive HMR cycles.

function makeStore() {
    return configureStore({
        reducer: persistedRootReducer,
        middleware: (getDefaultMiddleware) =>
            getDefaultMiddleware({
                serializableCheck: {
                    ignoredActions: [FLUSH, REHYDRATE, PAUSE, PERSIST, PURGE, REGISTER],
                },
            }),
    });
}

type AppStore = ReturnType<typeof makeStore>;

interface GlobalWithStore {
    __REDUX_STORE__?: AppStore;
    __REDUX_PERSISTOR__?: ReturnType<typeof persistStore>;
}

const g = globalThis as unknown as GlobalWithStore;

export const store = g.__REDUX_STORE__ ?? (g.__REDUX_STORE__ = makeStore());
export const persistor = g.__REDUX_PERSISTOR__ ?? (g.__REDUX_PERSISTOR__ = persistStore(store));

export type { RootState } from './parentReducer';
export type AppDispatch = typeof store.dispatch;
