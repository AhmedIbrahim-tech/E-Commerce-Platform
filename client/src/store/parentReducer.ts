import { combineReducers } from '@reduxjs/toolkit';
import { persistReducer } from 'redux-persist';
import storage from 'redux-persist/lib/storage';
import appReducer from './slices/appSlice';
import authReducer from './slices/authSlice';
import lookupsReducer from './slices/lookupsSlice';
import userManagementReducer from './slices/userManagementSlice';
import authorizationReducer from './slices/authorizationSlice';

const rootPersistConfig = {
    key: 'root',
    storage,
    whitelist: ['auth', 'app'],
};

const rootReducer = combineReducers({
    app: appReducer,
    auth: authReducer,
    lookups: lookupsReducer,
    userManagement: userManagementReducer,
    authorization: authorizationReducer,
});

export const persistedRootReducer = persistReducer(rootPersistConfig, rootReducer);

export type RootState = ReturnType<typeof rootReducer>;
