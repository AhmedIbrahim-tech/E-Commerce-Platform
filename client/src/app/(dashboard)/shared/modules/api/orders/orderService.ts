import apiClient from "@/services/core/apiClient";
import { Routes } from "@/constants/apiroutes";
import { extractApiData } from "@/types/api";
import { handleApiError } from "@/services/core/apiErrorHandler";

class OrderService {
  async getOrderPaginatedList(pageNumber: number = 1, pageSize: number = 10, search?: string, sortBy: number = 0) {
    try {
      const response = await apiClient.get(Routes.Order.Paginated, {
        params: {
          pageNumber,
          pageSize,
          ...(search && { search }),
          sortBy,
        },
      });
      return extractApiData<unknown>(response) as any;
    } catch (error) {
      handleApiError(error);
    }
  }
}

export const orderService = new OrderService();

