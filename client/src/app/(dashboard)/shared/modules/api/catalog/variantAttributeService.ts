import apiClient from "@/services/core/apiClient";
import { Routes } from "@/constants/apiroutes";
import type {
  CreateVariantAttributeRequest,
  UpdateVariantAttributeRequest,
  VariantAttribute,
  VariantAttributeListResponse,
} from "@/types";
import { extractApiData } from "@/types/api";
import { handleApiError } from "@/services/core/apiErrorHandler";

class VariantAttributeService {
  async getVariantAttributePaginatedList(
    pageNumber: number = 1,
    pageSize: number = 10,
    search?: string,
    sortBy: number = 0
  ): Promise<VariantAttributeListResponse> {
    try {
      const requestBody = {
        pageNumber,
        pageSize,
        ...(search && { search }),
        sortBy,
      };

      const response = await apiClient.post(Routes.VariantAttribute.Paginated, requestBody);
      return extractApiData<VariantAttributeListResponse>(response);
    } catch (error) {
      handleApiError(error);
    }
  }

  async getVariantAttributeById(id: string): Promise<VariantAttribute> {
    try {
      const response = await apiClient.post(`${Routes.VariantAttribute.Prefix}getById`, { id });
      return extractApiData<VariantAttribute>(response);
    } catch (error) {
      handleApiError(error);
    }
  }

  async getAllVariantAttributes(): Promise<VariantAttribute[]> {
    try {
      const response = await apiClient.post(Routes.VariantAttribute.GetAll, {});
      return extractApiData<VariantAttribute[]>(response);
    } catch (error) {
      handleApiError(error);
    }
  }

  async createVariantAttribute(data: CreateVariantAttributeRequest): Promise<string> {
    try {
      const response = await apiClient.post(Routes.VariantAttribute.Create, data);
      return extractApiData<string>(response);
    } catch (error) {
      handleApiError(error);
    }
  }

  async updateVariantAttribute(data: UpdateVariantAttributeRequest): Promise<string> {
    try {
      const response = await apiClient.post(Routes.VariantAttribute.Edit, data);
      return extractApiData<string>(response);
    } catch (error) {
      handleApiError(error);
    }
  }

  async deleteVariantAttribute(id: string): Promise<void> {
    try {
      await apiClient.post(`${Routes.VariantAttribute.Prefix}delete`, { id });
    } catch (error) {
      handleApiError(error);
    }
  }
}

export const variantAttributeService = new VariantAttributeService();

