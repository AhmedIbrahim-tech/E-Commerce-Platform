import apiClient from "@/services/core/apiClient";
import { Routes } from "@/constants/apiroutes";
import type { CreateSubCategoryRequest, SubCategory, SubCategoryListResponse, UpdateSubCategoryRequest } from "@/types";
import { extractApiData } from "@/types/api";
import { handleApiError } from "@/services/core/apiErrorHandler";

class SubCategoryService {
  async getSubCategoryPaginatedList(
    pageNumber: number = 1,
    pageSize: number = 10,
    search?: string,
    categoryId?: string,
    sortBy: number = 0
  ): Promise<SubCategoryListResponse> {
    try {
      const requestBody = {
        pageNumber,
        pageSize,
        ...(search && { search }),
        ...(categoryId && { categoryId }),
        sortBy,
      };

      const response = await apiClient.post(Routes.SubCategory.Paginated, requestBody);
      return extractApiData<SubCategoryListResponse>(response);
    } catch (error) {
      handleApiError(error);
    }
  }

  async getSubCategoryById(id: string): Promise<SubCategory> {
    try {
      const response = await apiClient.post(`${Routes.SubCategory.Prefix}getById`, { id });
      return extractApiData<SubCategory>(response);
    } catch (error) {
      handleApiError(error);
    }
  }

  async getAllSubCategories(): Promise<SubCategory[]> {
    try {
      const response = await apiClient.post(Routes.SubCategory.GetAll, {});
      return extractApiData<SubCategory[]>(response);
    } catch (error) {
      handleApiError(error);
    }
  }

  async createSubCategory(data: CreateSubCategoryRequest): Promise<string> {
    try {
      const response = await apiClient.post(Routes.SubCategory.Create, data);
      return extractApiData<string>(response);
    } catch (error) {
      handleApiError(error);
    }
  }

  async updateSubCategory(data: UpdateSubCategoryRequest): Promise<string> {
    try {
      const response = await apiClient.post(Routes.SubCategory.Edit, data);
      return extractApiData<string>(response);
    } catch (error) {
      handleApiError(error);
    }
  }

  async deleteSubCategory(id: string): Promise<void> {
    try {
      await apiClient.post(`${Routes.SubCategory.Prefix}delete`, { id });
    } catch (error) {
      handleApiError(error);
    }
  }
}

export const subCategoryService = new SubCategoryService();

