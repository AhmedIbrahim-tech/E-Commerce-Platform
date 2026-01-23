import apiClient from "@/services/core/apiClient";
import { Routes } from "@/constants/apiroutes";
import type { CreateDiscountRequest, Discount, DiscountListResponse, UpdateDiscountRequest } from "@/types";
import { extractApiData } from "@/types/api";
import { handleApiError } from "@/services/core/apiErrorHandler";

class DiscountService {
  async getDiscountPaginatedList(pageNumber: number = 1, pageSize: number = 10, search?: string): Promise<DiscountListResponse> {
    try {
      const requestBody = {
        pageNumber,
        pageSize,
        ...(search && { search }),
      };

      const response = await apiClient.post(Routes.Discount.Paginated, requestBody);
      return extractApiData<DiscountListResponse>(response);
    } catch (error) {
      handleApiError(error);
    }
  }

  async getDiscountById(id: string): Promise<Discount> {
    try {
      const response = await apiClient.post(`${Routes.Discount.Prefix}getById`, { id });
      return extractApiData<Discount>(response);
    } catch (error) {
      handleApiError(error);
    }
  }

  async getAllDiscounts(): Promise<Discount[]> {
    try {
      const response = await apiClient.post(Routes.Discount.GetAll, {});
      return extractApiData<Discount[]>(response);
    } catch (error) {
      handleApiError(error);
    }
  }

  async createDiscount(data: CreateDiscountRequest): Promise<string> {
    try {
      const response = await apiClient.post(Routes.Discount.Create, data);
      return extractApiData<string>(response);
    } catch (error) {
      handleApiError(error);
    }
  }

  async updateDiscount(data: UpdateDiscountRequest): Promise<string> {
    try {
      const response = await apiClient.post(Routes.Discount.Edit, data);
      return extractApiData<string>(response);
    } catch (error) {
      handleApiError(error);
    }
  }

  async deleteDiscount(id: string): Promise<void> {
    try {
      await apiClient.post(`${Routes.Discount.Prefix}delete`, { id });
    } catch (error) {
      handleApiError(error);
    }
  }
}

export const discountService = new DiscountService();

