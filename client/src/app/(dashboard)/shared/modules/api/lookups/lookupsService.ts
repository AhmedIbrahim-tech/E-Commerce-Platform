import apiClient from "@/services/core/apiClient";
import { Routes } from "@/constants/apiroutes";
import { extractApiData } from "@/types/api";
import { handleApiError } from "@/services/core/apiErrorHandler";

export interface BaseLookup {
  id: string;
  name: string;
}

export interface RoleLookup {
  name: string;
  displayName: string;
}

export interface EnumLookup {
  id: number;
  name: string;
}

class LookupsService {
  private async fetchLookup<T>(route: string, method: "get" | "post" = "get", body?: unknown): Promise<T[]> {
    try {
      const response = method === "post" ? await apiClient.post(route, body) : await apiClient.get(route);
      const data = extractApiData<T[]>(response);
      return data || [];
    } catch (error) {
      handleApiError(error);
      return [];
    }
  }

  async getCategories(): Promise<BaseLookup[]> {
    return this.fetchLookup<BaseLookup>(Routes.LookUps.Categories);
  }

  async getSubCategories(): Promise<BaseLookup[]> {
    return this.fetchLookup<BaseLookup>(Routes.LookUps.SubCategories);
  }

  async getSubCategoriesByCategory(categoryId: string): Promise<BaseLookup[]> {
    return this.fetchLookup<BaseLookup>(Routes.LookUps.SubCategoriesByCategory, "post", { categoryId });
  }

  async getBrands(): Promise<BaseLookup[]> {
    return this.fetchLookup<BaseLookup>(Routes.LookUps.Brands);
  }

  async getUnitOfMeasures(): Promise<BaseLookup[]> {
    return this.fetchLookup<BaseLookup>(Routes.LookUps.UnitOfMeasures);
  }

  async getWarranties(): Promise<BaseLookup[]> {
    return this.fetchLookup<BaseLookup>(Routes.LookUps.Warranties);
  }

  async getVariantAttributes(): Promise<BaseLookup[]> {
    return this.fetchLookup<BaseLookup>(Routes.LookUps.VariantAttributes);
  }

  async getRoles(): Promise<RoleLookup[]> {
    return this.fetchLookup<RoleLookup>(Routes.LookUps.Roles);
  }

  async getProductPublishStatuses(): Promise<EnumLookup[]> {
    return this.fetchLookup<EnumLookup>(Routes.LookUps.ProductPublishStatuses);
  }

  async getProductVisibilities(): Promise<EnumLookup[]> {
    return this.fetchLookup<EnumLookup>(Routes.LookUps.ProductVisibilities);
  }

  async getProductTypes(): Promise<EnumLookup[]> {
    return this.fetchLookup<EnumLookup>(Routes.LookUps.ProductTypes);
  }

  async getSellingTypes(): Promise<EnumLookup[]> {
    return this.fetchLookup<EnumLookup>(Routes.LookUps.SellingTypes);
  }

  async getTaxTypes(): Promise<EnumLookup[]> {
    return this.fetchLookup<EnumLookup>(Routes.LookUps.TaxTypes);
  }

  async getDiscountTypes(): Promise<EnumLookup[]> {
    return this.fetchLookup<EnumLookup>(Routes.LookUps.DiscountTypes);
  }

  async getTags(): Promise<BaseLookup[]> {
    return this.fetchLookup<BaseLookup>(Routes.LookUps.Tags);
  }
}

export const lookupsService = new LookupsService();

