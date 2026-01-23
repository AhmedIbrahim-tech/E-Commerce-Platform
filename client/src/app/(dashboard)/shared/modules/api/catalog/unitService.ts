import apiClient from "@/services/core/apiClient";
import { Routes } from "@/constants/apiroutes";
import type { CreateUnitRequest, Unit, UnitListResponse, UpdateUnitRequest } from "@/types";
import { extractApiData } from "@/types/api";
import { handleApiError } from "@/services/core/apiErrorHandler";

class UnitService {
  async getUnitPaginatedList(
    pageNumber: number = 1,
    pageSize: number = 10,
    search?: string,
    sortBy: number = 0
  ): Promise<UnitListResponse> {
    try {
      const requestBody = {
        pageNumber,
        pageSize,
        ...(search && { search }),
        sortBy,
      };

      const response = await apiClient.post(Routes.Unit.Paginated, requestBody);
      return extractApiData<UnitListResponse>(response);
    } catch (error) {
      handleApiError(error);
    }
  }

  async getUnitById(id: string): Promise<Unit> {
    try {
      const response = await apiClient.post(`${Routes.Unit.Prefix}getById`, { id });
      return extractApiData<Unit>(response);
    } catch (error) {
      handleApiError(error);
    }
  }

  async getAllUnits(): Promise<Unit[]> {
    try {
      const response = await apiClient.post(Routes.Unit.GetAll, {});
      return extractApiData<Unit[]>(response);
    } catch (error) {
      handleApiError(error);
    }
  }

  async createUnit(data: CreateUnitRequest): Promise<string> {
    try {
      const response = await apiClient.post(Routes.Unit.Create, data);
      return extractApiData<string>(response);
    } catch (error) {
      handleApiError(error);
    }
  }

  async updateUnit(data: UpdateUnitRequest): Promise<string> {
    try {
      const response = await apiClient.post(Routes.Unit.Edit, data);
      return extractApiData<string>(response);
    } catch (error) {
      handleApiError(error);
    }
  }

  async deleteUnit(id: string): Promise<void> {
    try {
      await apiClient.post(`${Routes.Unit.Prefix}delete`, { id });
    } catch (error) {
      handleApiError(error);
    }
  }
}

export const unitService = new UnitService();

