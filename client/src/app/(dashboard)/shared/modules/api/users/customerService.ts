import apiClient from "@/services/core/apiClient";
import { Routes } from "@/constants/apiroutes";
import type { CreateCustomerRequest, Customer, CustomerListResponse, UpdateCustomerRequest } from "@/types";
import { extractApiData } from "@/types/api";
import { handleApiError } from "@/services/core/apiErrorHandler";

class CustomerService {
  async getCustomerPaginatedList(
    pageNumber: number = 1,
    pageSize: number = 10,
    search?: string,
    sortBy: number = 0
  ): Promise<CustomerListResponse> {
    try {
      const requestBody = {
        pageNumber,
        pageSize,
        ...(search && { search }),
        sortBy,
      };

      const response = await apiClient.post(Routes.Customer.Paginated, requestBody);
      return extractApiData<CustomerListResponse>(response);
    } catch (error) {
      handleApiError(error);
    }
  }

  async getCustomerById(id: string): Promise<Customer> {
    try {
      const response = await apiClient.post(`${Routes.Customer.Prefix}getById`, { id });
      return extractApiData<Customer>(response);
    } catch (error) {
      handleApiError(error);
    }
  }

  async createCustomer(data: CreateCustomerRequest): Promise<string> {
    try {
      const response = await apiClient.post(Routes.Customer.Create, data);
      return extractApiData<string>(response);
    } catch (error) {
      handleApiError(error);
    }
  }

  async updateCustomer(data: UpdateCustomerRequest): Promise<string> {
    try {
      const response = await apiClient.put(Routes.Customer.Edit, data);
      return extractApiData<string>(response);
    } catch (error) {
      handleApiError(error);
    }
  }

  async deleteCustomer(id: string): Promise<void> {
    try {
      await apiClient.delete(Routes.Customer.Delete(id));
    } catch (error) {
      handleApiError(error);
    }
  }

  async toggleCustomerStatus(id: string): Promise<string> {
    try {
      const response = await apiClient.post(Routes.Customer.ToggleStatus(id));
      return extractApiData<string>(response);
    } catch (error) {
      handleApiError(error);
    }
  }
}

export const customerService = new CustomerService();

