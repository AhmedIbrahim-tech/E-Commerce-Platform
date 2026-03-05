export interface BaseLookupDto {
  id: string;
  name: string;
}

export interface RoleLookupDto extends BaseLookupDto {
  displayName: string;
}

export interface EnumLookupDto {
  id: number;
  name: string;
}

export interface LookupsState {
  categories: BaseLookupDto[];
  subCategories: BaseLookupDto[];
  subCategoriesByCategory: BaseLookupDto[];
  brands: BaseLookupDto[];
  unitOfMeasures: BaseLookupDto[];
  warranties: BaseLookupDto[];
  variantAttributes: BaseLookupDto[];
  tags: BaseLookupDto[];
  roles: RoleLookupDto[];
  productPublishStatuses: EnumLookupDto[];
  productVisibilities: EnumLookupDto[];
  productTypes: EnumLookupDto[];
  sellingTypes: EnumLookupDto[];
  taxTypes: EnumLookupDto[];
  discountTypes: EnumLookupDto[];
  loading: Record<string, boolean>;
  error: Record<string, string | null>;
}

export type LookupDataKey = keyof Omit<LookupsState, "loading" | "error">;

export interface AsyncThunkLike {
  pending: { type: string };
  fulfilled: { type: string };
  rejected: { type: string };
}
