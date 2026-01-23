import apiClient from "@/services/core/apiClient";
import { Routes } from "@/constants/apiroutes";
import type { CreateWarrantyRequest, UpdateWarrantyRequest, Warranty, WarrantyListResponse } from "@/types";
import { extractApiData } from "@/types/api";
import { handleApiError } from "@/services/core/apiErrorHandler";

class WarrantyService {
  async getWarrantyPaginatedList(
    pageNumber: number = 1,
    pageSize: number = 10,
    search?: string
  ): Promise<WarrantyListResponse> {
    try {
      const response = await apiClient.get(Routes.Warranty.Paginated, {
        params: {
          pageNumber,
          pageSize,
          ...(search && { search }),
        },
      });
      return extractApiData<WarrantyListResponse>(response);
    } catch (error) {
      handleApiError(error);
    }
  }

  async getWarrantyById(id: string): Promise<Warranty> {
    try {
      const response = await apiClient.post(Routes.Warranty.GetById, { id });
      return extractApiData<Warranty>(response);
    } catch (error) {
      handleApiError(error);
    }
  }

  async getAllWarranties(): Promise<Warranty[]> {
    try {
      const response = await apiClient.get(Routes.Warranty.GetAll);
      return extractApiData<Warranty[]>(response);
    } catch (error) {
      handleApiError(error);
    }
  }

  async createWarranty(data: CreateWarrantyRequest): Promise<string> {
    try {
      const response = await apiClient.post(Routes.Warranty.Create, data);
      return extractApiData<string>(response);
    } catch (error) {
      handleApiError(error);
    }
  }

  async updateWarranty(data: UpdateWarrantyRequest): Promise<string> {
    try {
      const response = await apiClient.put(Routes.Warranty.Edit, data);
      return extractApiData<string>(response);
    } catch (error) {
      handleApiError(error);
    }
  }

  async deleteWarranty(id: string): Promise<void> {
    try {
      await apiClient.post(Routes.Warranty.Delete, { id });
    } catch (error) {
      handleApiError(error);
    }
  }
}

export const warrantyService = new WarrantyService();

