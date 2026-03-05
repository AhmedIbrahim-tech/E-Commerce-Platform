import {
  createSlice,
  createAsyncThunk,
  type PayloadAction,
  type ActionReducerMapBuilder,
} from "@reduxjs/toolkit";
import { apiClient } from "@/services/apiClient";
import { Router } from "@/config/Router";
import { ApiResponse } from "@/types/App/ApiResponse";
import type {
  BaseLookupDto,
  RoleLookupDto,
  EnumLookupDto,
  LookupsState,
  LookupDataKey,
  AsyncThunkLike,
} from "@/types/lookups/lookups";

export type { BaseLookupDto, RoleLookupDto, EnumLookupDto, LookupsState } from "@/types/lookups/lookups";

// ── Thunk factory ───────────────────────────────────────────────────

type LookupRouteKey = keyof typeof Router.LookUpsRouting;

function createLookupThunk<T>(stateKey: string, routeKey: LookupRouteKey) {
  return createAsyncThunk<T[], void, { rejectValue: string }>(
    `lookups/${stateKey}`,
    async (_, { rejectWithValue }) => {
      try {
        const { data } = await apiClient.get<ApiResponse<T[]>>(
          Router.LookUpsRouting[routeKey]
        );
        return data.data ?? [];
      } catch {
        return rejectWithValue(`Failed to fetch ${stateKey}`);
      }
    }
  );
}

// ── Entity lookups (BaseLookupDto — Guid id) ────────────────────────

export const getCategories = createLookupThunk<BaseLookupDto>("categories", "Categories");
export const getSubCategories = createLookupThunk<BaseLookupDto>("subCategories", "SubCategories");
export const getBrands = createLookupThunk<BaseLookupDto>("brands", "Brands");
export const getUnitOfMeasures = createLookupThunk<BaseLookupDto>("unitOfMeasures", "UnitOfMeasures");
export const getWarranties = createLookupThunk<BaseLookupDto>("warranties", "Warranties");
export const getVariantAttributes = createLookupThunk<BaseLookupDto>("variantAttributes", "VariantAttributes");
export const getTags = createLookupThunk<BaseLookupDto>("tags", "Tags");

// ── Role lookup (RoleLookupDto) ──────────────────────────────────────

export const getRoles = createLookupThunk<RoleLookupDto>("roles", "Roles");

// ── Enum lookups (EnumLookupDto — int id) ───────────────────────────

export const getProductPublishStatuses = createLookupThunk<EnumLookupDto>("productPublishStatuses", "ProductPublishStatuses");
export const getProductVisibilities = createLookupThunk<EnumLookupDto>("productVisibilities", "ProductVisibilities");
export const getProductTypes = createLookupThunk<EnumLookupDto>("productTypes", "ProductTypes");
export const getSellingTypes = createLookupThunk<EnumLookupDto>("sellingTypes", "SellingTypes");
export const getTaxTypes = createLookupThunk<EnumLookupDto>("taxTypes", "TaxTypes");
export const getDiscountTypes = createLookupThunk<EnumLookupDto>("discountTypes", "DiscountTypes");

// ── Parameterised lookup (POST — matches backend [HttpPost]) ────────

export const getSubCategoriesByCategory = createAsyncThunk<
  BaseLookupDto[],
  string,
  { rejectValue: string }
>(
  "lookups/subCategoriesByCategory",
  async (categoryId, { rejectWithValue }) => {
    try {
      const { data } = await apiClient.post<ApiResponse<BaseLookupDto[]>>(
        Router.LookUpsRouting.SubCategoriesByCategory,
        { categoryId }
      );
      return data.data ?? [];
    } catch {
      return rejectWithValue("Failed to fetch subcategories by category");
    }
  }
);

// ── State ───────────────────────────────────────────────────────────

const initialState: LookupsState = {
  categories: [],
  subCategories: [],
  subCategoriesByCategory: [],
  brands: [],
  unitOfMeasures: [],
  warranties: [],
  variantAttributes: [],
  tags: [],
  roles: [],
  productPublishStatuses: [],
  productVisibilities: [],
  productTypes: [],
  sellingTypes: [],
  taxTypes: [],
  discountTypes: [],
  loading: {},
  error: {},
};

// ── Extra-reducer helper ────────────────────────────────────────────

function addLookupCases(
  builder: ActionReducerMapBuilder<LookupsState>,
  thunk: AsyncThunkLike,
  stateKey: LookupDataKey
) {
  builder
    .addMatcher(
      (action): action is PayloadAction => action.type === thunk.pending.type,
      (state) => {
        state.loading[stateKey] = true;
        state.error[stateKey] = null;
      }
    )
    .addMatcher(
      (action): action is PayloadAction<unknown[]> => action.type === thunk.fulfilled.type,
      (state, action) => {
        state.loading[stateKey] = false;
        (state as Record<string, unknown>)[stateKey] = action.payload;
      }
    )
    .addMatcher(
      (action): action is PayloadAction<string> & { payload?: string } =>
        action.type === thunk.rejected.type,
      (state, action) => {
        state.loading[stateKey] = false;
        state.error[stateKey] = action.payload ?? "Request failed";
      }
    );
}

// ── Slice ───────────────────────────────────────────────────────────

const lookupsSlice = createSlice({
  name: "lookups",
  initialState,
  reducers: {
    clearLookupError(state, action: PayloadAction<string>) {
      state.error[action.payload] = null;
    },
    resetLookups: () => initialState,
  },
  extraReducers: (builder) => {
    addLookupCases(builder, getCategories, "categories");
    addLookupCases(builder, getSubCategories, "subCategories");
    addLookupCases(builder, getSubCategoriesByCategory, "subCategoriesByCategory");
    addLookupCases(builder, getBrands, "brands");
    addLookupCases(builder, getUnitOfMeasures, "unitOfMeasures");
    addLookupCases(builder, getWarranties, "warranties");
    addLookupCases(builder, getVariantAttributes, "variantAttributes");
    addLookupCases(builder, getTags, "tags");
    addLookupCases(builder, getRoles, "roles");
    addLookupCases(builder, getProductPublishStatuses, "productPublishStatuses");
    addLookupCases(builder, getProductVisibilities, "productVisibilities");
    addLookupCases(builder, getProductTypes, "productTypes");
    addLookupCases(builder, getSellingTypes, "sellingTypes");
    addLookupCases(builder, getTaxTypes, "taxTypes");
    addLookupCases(builder, getDiscountTypes, "discountTypes");
  },
});

export const { clearLookupError, resetLookups } = lookupsSlice.actions;
export default lookupsSlice.reducer;
