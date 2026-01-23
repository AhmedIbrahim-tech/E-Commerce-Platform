import apiClient from "@/services/core/apiClient";
import { Routes } from "@/constants/apiroutes";
import type { Category, CategoryListResponse, CreateCategoryRequest, UpdateCategoryRequest } from "@/types";
import { extractApiData } from "@/types/api";
import { handleApiError } from "@/services/core/apiErrorHandler";

class CategoryService {
  async getCategoryPaginatedList(
    pageNumber: number = 1,
    pageSize: number = 10,
    search?: string,
    sortBy: number = 0
  ): Promise<CategoryListResponse> {
    try {
      const response = await apiClient.get(Routes.Category.Paginated, {
        params: {
          pageNumber,
          pageSize,
          ...(search && { search }),
          sortBy,
        },
      });
      return extractApiData<CategoryListResponse>(response);
    } catch (error) {
      handleApiError(error);
    }
  }

  async getCategoryById(id: string): Promise<Category> {
    try {
      const response = await apiClient.post(Routes.Category.GetById, { id });
      return extractApiData<Category>(response);
    } catch (error) {
      handleApiError(error);
    }
  }

  async getAllCategories(): Promise<Category[]> {
    try {
      const response = await apiClient.get(Routes.Category.GetAll);
      return extractApiData<Category[]>(response);
    } catch (error) {
      handleApiError(error);
    }
  }

  async createCategory(data: CreateCategoryRequest): Promise<string> {
    try {
      const response = await apiClient.post(Routes.Category.Create, data);
      return extractApiData<string>(response);
    } catch (error) {
      handleApiError(error);
    }
  }

  async updateCategory(data: UpdateCategoryRequest): Promise<string> {
    try {
      const response = await apiClient.put(Routes.Category.Edit, data);
      return extractApiData<string>(response);
    } catch (error) {
      handleApiError(error);
    }
  }

  async deleteCategory(id: string): Promise<void> {
    try {
      await apiClient.post(Routes.Category.Delete, { id });
    } catch (error) {
      handleApiError(error);
    }
  }
}

export const categoryService = new CategoryService();

