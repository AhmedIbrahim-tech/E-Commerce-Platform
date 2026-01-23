import apiClient from "@/services/core/apiClient";
import { Routes } from "@/constants/apiroutes";
import type { Brand, BrandListResponse, CreateBrandRequest, UpdateBrandRequest } from "@/types";
import { extractApiData } from "@/types/api";
import { handleApiError } from "@/services/core/apiErrorHandler";

class BrandService {
  async getBrandPaginatedList(
    pageNumber: number = 1,
    pageSize: number = 10,
    search?: string,
    sortBy: number = 0
  ): Promise<BrandListResponse> {
    try {
      const requestBody = {
        pageNumber,
        pageSize,
        ...(search && { search }),
        sortBy,
      };

      const response = await apiClient.post(Routes.Brand.Paginated, requestBody);
      return extractApiData<BrandListResponse>(response);
    } catch (error) {
      handleApiError(error);
    }
  }

  async getBrandById(id: string): Promise<Brand> {
    try {
      const response = await apiClient.post(`${Routes.Brand.Prefix}getById`, { id });
      return extractApiData<Brand>(response);
    } catch (error) {
      handleApiError(error);
    }
  }

  async getAllBrands(): Promise<Brand[]> {
    try {
      const response = await apiClient.post(Routes.Brand.GetAll, {});
      return extractApiData<Brand[]>(response);
    } catch (error) {
      handleApiError(error);
    }
  }

  async createBrand(data: CreateBrandRequest): Promise<string> {
    try {
      const response = await apiClient.post(Routes.Brand.Create, data);
      return extractApiData<string>(response);
    } catch (error) {
      handleApiError(error);
    }
  }

  async updateBrand(data: UpdateBrandRequest): Promise<string> {
    try {
      const response = await apiClient.post(Routes.Brand.Edit, data);
      return extractApiData<string>(response);
    } catch (error) {
      handleApiError(error);
    }
  }

  async deleteBrand(id: string): Promise<void> {
    try {
      await apiClient.post(`${Routes.Brand.Prefix}delete`, { id });
    } catch (error) {
      handleApiError(error);
    }
  }
}

export const brandService = new BrandService();

