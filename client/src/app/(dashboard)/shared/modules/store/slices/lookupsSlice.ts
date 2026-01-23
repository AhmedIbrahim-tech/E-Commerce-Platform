import { createSlice, createAsyncThunk, PayloadAction } from '@reduxjs/toolkit';
import { AxiosError } from 'axios';
import { lookupsService, type BaseLookup, type RoleLookup } from '@/services/lookups/lookupsService';

interface ApiErrorResponse {
  message?: string;
  errors?: Record<string, string[]>;
}

interface LookUpsState {
  categories: BaseLookup[];
  subCategories: BaseLookup[];
  subCategoriesByCategory: Record<string, BaseLookup[]>;
  brands: BaseLookup[];
  unitOfMeasures: BaseLookup[];
  warranties: BaseLookup[];
  variantAttributes: BaseLookup[];
  roles: RoleLookup[];
  
  loadingCategories: boolean;
  loadingSubCategories: boolean;
  loadingBrands: boolean;
  loadingUnitOfMeasures: boolean;
  loadingWarranties: boolean;
  loadingVariantAttributes: boolean;
  loadingRoles: boolean;
  
  categoriesLoaded: boolean;
  subCategoriesLoaded: boolean;
  brandsLoaded: boolean;
  unitOfMeasuresLoaded: boolean;
  warrantiesLoaded: boolean;
  variantAttributesLoaded: boolean;
  rolesLoaded: boolean;
  
  error: string | null;
}

const initialState: LookUpsState = {
  categories: [],
  subCategories: [],
  subCategoriesByCategory: {},
  brands: [],
  unitOfMeasures: [],
  warranties: [],
  variantAttributes: [],
  roles: [],
  
  loadingCategories: false,
  loadingSubCategories: false,
  loadingBrands: false,
  loadingUnitOfMeasures: false,
  loadingWarranties: false,
  loadingVariantAttributes: false,
  loadingRoles: false,
  
  categoriesLoaded: false,
  subCategoriesLoaded: false,
  brandsLoaded: false,
  unitOfMeasuresLoaded: false,
  warrantiesLoaded: false,
  variantAttributesLoaded: false,
  rolesLoaded: false,
  
  error: null,
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

const createLookupThunk = <T>(
  name: string,
  fetchFn: () => Promise<T>
) => {
  return createAsyncThunk<T, void, { rejectValue: string }>(
    `lookups/${name}`,
    async (_, { rejectWithValue }) => {
      try {
        return await fetchFn();
      } catch (error) {
        return rejectWithValue(getErrorMessage(error));
      }
    }
  );
};

export const fetchCategoriesAsync = createLookupThunk<BaseLookup[]>(
  'fetchCategories',
  () => lookupsService.getCategories()
);

export const fetchSubCategoriesAsync = createLookupThunk<BaseLookup[]>(
  'fetchSubCategories',
  () => lookupsService.getSubCategories()
);

export const fetchSubCategoriesByCategoryAsync = createAsyncThunk<
  { categoryId: string; subCategories: BaseLookup[] },
  string,
  { rejectValue: string }
>(
  'lookups/fetchSubCategoriesByCategory',
  async (categoryId: string, { rejectWithValue }) => {
    try {
      const subCategories = await lookupsService.getSubCategoriesByCategory(categoryId);
      return { categoryId, subCategories };
    } catch (error) {
      return rejectWithValue(getErrorMessage(error));
    }
  }
);

export const fetchBrandsAsync = createLookupThunk<BaseLookup[]>(
  'fetchBrands',
  () => lookupsService.getBrands()
);

export const fetchUnitOfMeasuresAsync = createLookupThunk<BaseLookup[]>(
  'fetchUnitOfMeasures',
  () => lookupsService.getUnitOfMeasures()
);

export const fetchWarrantiesAsync = createLookupThunk<BaseLookup[]>(
  'fetchWarranties',
  () => lookupsService.getWarranties()
);

export const fetchVariantAttributesAsync = createLookupThunk<BaseLookup[]>(
  'fetchVariantAttributes',
  () => lookupsService.getVariantAttributes()
);

export const fetchRolesAsync = createLookupThunk<RoleLookup[]>(
  'fetchRoles',
  () => lookupsService.getRoles()
);

export const fetchAllLookupsAsync = createAsyncThunk(
  'lookups/fetchAll',
  async (_, { dispatch, rejectWithValue }) => {
    try {
      const results = await Promise.allSettled([
        dispatch(fetchCategoriesAsync()),
        dispatch(fetchSubCategoriesAsync()),
        dispatch(fetchBrandsAsync()),
        dispatch(fetchUnitOfMeasuresAsync()),
        dispatch(fetchWarrantiesAsync()),
        dispatch(fetchVariantAttributesAsync()),
        dispatch(fetchRolesAsync()),
      ]);
      
      const hasErrors = results.some(result => result.status === 'rejected');
      if (hasErrors) {
        const errors = results
          .filter((r): r is PromiseRejectedResult => r.status === 'rejected')
          .map(r => r.reason);
        return rejectWithValue(errors[0] || 'Failed to fetch some lookups');
      }
      
      return true;
    } catch (error) {
      return rejectWithValue(getErrorMessage(error));
    }
  }
);


const lookUpsSlice = createSlice({
  name: 'lookups',
  initialState,
  reducers: {
    clearError: (state) => {
      state.error = null;
    },
    clearSubCategoriesByCategory: (state, action: PayloadAction<string>) => {
      delete state.subCategoriesByCategory[action.payload];
    },
    clearAllSubCategoriesByCategory: (state) => {
      state.subCategoriesByCategory = {};
    },
    resetLookups: (state) => {
      Object.assign(state, initialState);
    },
  },
  extraReducers: (builder) => {
    builder
      .addCase(fetchCategoriesAsync.pending, (state) => {
        state.loadingCategories = true;
        state.error = null;
      })
      .addCase(fetchCategoriesAsync.fulfilled, (state, action: PayloadAction<BaseLookup[]>) => {
        state.loadingCategories = false;
        state.categories = action.payload;
        state.categoriesLoaded = true;
        state.error = null;
      })
      .addCase(fetchCategoriesAsync.rejected, (state, action) => {
        state.loadingCategories = false;
        state.error = (action.payload as string) || 'An error occurred';
      })
      
      .addCase(fetchSubCategoriesAsync.pending, (state) => {
        state.loadingSubCategories = true;
        state.error = null;
      })
      .addCase(fetchSubCategoriesAsync.fulfilled, (state, action) => {
        state.loadingSubCategories = false;
        state.subCategories = action.payload;
        state.subCategoriesLoaded = true;
        state.error = null;
      })
      .addCase(fetchSubCategoriesAsync.rejected, (state, action) => {
        state.loadingSubCategories = false;
        state.error = (action.payload as string) || 'An error occurred';
      })
      
      .addCase(fetchSubCategoriesByCategoryAsync.pending, (state) => {
        state.loadingSubCategories = true;
        state.error = null;
      })
      .addCase(fetchSubCategoriesByCategoryAsync.fulfilled, (state, action) => {
        state.loadingSubCategories = false;
        state.subCategoriesByCategory[action.payload.categoryId] = action.payload.subCategories;
        state.error = null;
      })
      .addCase(fetchSubCategoriesByCategoryAsync.rejected, (state, action) => {
        state.loadingSubCategories = false;
        state.error = (action.payload as string) || 'An error occurred';
      })
      
      .addCase(fetchBrandsAsync.pending, (state) => {
        state.loadingBrands = true;
        state.error = null;
      })
      .addCase(fetchBrandsAsync.fulfilled, (state, action: PayloadAction<BaseLookup[]>) => {
        state.loadingBrands = false;
        state.brands = action.payload;
        state.brandsLoaded = true;
        state.error = null;
      })
      .addCase(fetchBrandsAsync.rejected, (state, action) => {
        state.loadingBrands = false;
        state.error = (action.payload as string) || 'An error occurred';
      })
      
      .addCase(fetchUnitOfMeasuresAsync.pending, (state) => {
        state.loadingUnitOfMeasures = true;
        state.error = null;
      })
      .addCase(fetchUnitOfMeasuresAsync.fulfilled, (state, action) => {
        state.loadingUnitOfMeasures = false;
        state.unitOfMeasures = action.payload;
        state.unitOfMeasuresLoaded = true;
        state.error = null;
      })
      .addCase(fetchUnitOfMeasuresAsync.rejected, (state, action) => {
        state.loadingUnitOfMeasures = false;
        state.error = (action.payload as string) || 'An error occurred';
      })
      
      .addCase(fetchWarrantiesAsync.pending, (state) => {
        state.loadingWarranties = true;
        state.error = null;
      })
      .addCase(fetchWarrantiesAsync.fulfilled, (state, action: PayloadAction<BaseLookup[]>) => {
        state.loadingWarranties = false;
        state.warranties = action.payload;
        state.warrantiesLoaded = true;
        state.error = null;
      })
      .addCase(fetchWarrantiesAsync.rejected, (state, action) => {
        state.loadingWarranties = false;
        state.error = (action.payload as string) || 'An error occurred';
      })
      
      .addCase(fetchVariantAttributesAsync.pending, (state) => {
        state.loadingVariantAttributes = true;
        state.error = null;
      })
      .addCase(fetchVariantAttributesAsync.fulfilled, (state, action) => {
        state.loadingVariantAttributes = false;
        state.variantAttributes = action.payload;
        state.variantAttributesLoaded = true;
        state.error = null;
      })
      .addCase(fetchVariantAttributesAsync.rejected, (state, action) => {
        state.loadingVariantAttributes = false;
        state.error = (action.payload as string) || 'An error occurred';
      })
      
      .addCase(fetchRolesAsync.pending, (state) => {
        state.loadingRoles = true;
        state.error = null;
      })
      .addCase(fetchRolesAsync.fulfilled, (state, action: PayloadAction<RoleLookup[]>) => {
        state.loadingRoles = false;
        state.roles = action.payload;
        state.rolesLoaded = true;
        state.error = null;
      })
      .addCase(fetchRolesAsync.rejected, (state, action) => {
        state.loadingRoles = false;
        state.error = (action.payload as string) || 'An error occurred';
      });
  },
});

export const {
  clearError,
  clearSubCategoriesByCategory,
  clearAllSubCategoriesByCategory,
  resetLookups,
} = lookUpsSlice.actions;

export default lookUpsSlice.reducer;
