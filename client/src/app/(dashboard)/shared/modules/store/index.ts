import { configureStore } from '@reduxjs/toolkit';
import { combineReducers } from '@reduxjs/toolkit';
import authReducer from './slices/authSlice';
import adminReducer from './slices/adminSlice';
import vendorReducer from './slices/vendorSlice';
import customerReducer from './slices/customerSlice';
import authorizationReducer from './slices/authorizationSlice';
import auditLogReducer from './slices/auditLogSlice';
import subCategoryReducer from './slices/subCategorySlice';
import brandReducer from './slices/brandSlice';
import unitReducer from './slices/unitSlice';
import variantAttributeReducer from './slices/variantAttributeSlice';
import couponReducer from './slices/couponSlice';
import giftCardReducer from './slices/giftCardSlice';
import discountReducer from './slices/discountSlice';
import accountReducer from './slices/accountSlice';
import productReducer from './slices/productSlice';
import lookUpsReducer from './slices/lookupsSlice';
import notificationReducer from './slices/notificationSlice';

const rootReducer = combineReducers({
  auth: authReducer,
  admin: adminReducer,
  vendor: vendorReducer,
  customer: customerReducer,
  authorization: authorizationReducer,
  auditLog: auditLogReducer,
  subCategory: subCategoryReducer,
  brand: brandReducer,
  unit: unitReducer,
  variantAttribute: variantAttributeReducer,
  coupon: couponReducer,
  giftCard: giftCardReducer,
  discount: discountReducer,
  account: accountReducer,
  product: productReducer,
  lookups: lookUpsReducer,
  notifications: notificationReducer,
});

export const store = configureStore({
  reducer: rootReducer,
});

export type RootState = ReturnType<typeof store.getState>;
export type AppDispatch = typeof store.dispatch;
