import apiClient from "@/services/core/apiClient";
import { Routes } from "@/constants/apiroutes";
import type { Account, AccountListResponse, CreateAccountRequest, UpdateAccountRequest } from "@/types";
import { extractApiData } from "@/types/api";
import { handleApiError } from "@/services/core/apiErrorHandler";

class AccountService {
  async getAccountPaginatedList(pageNumber: number = 1, pageSize: number = 10, search?: string): Promise<AccountListResponse> {
    try {
      const requestBody = {
        pageNumber,
        pageSize,
        ...(search && { search }),
      };

      const response = await apiClient.post(Routes.Account.Paginated, requestBody);
      return extractApiData<AccountListResponse>(response);
    } catch (error) {
      handleApiError(error);
    }
  }

  async getAccountById(id: string): Promise<Account> {
    try {
      const response = await apiClient.post(`${Routes.Account.Prefix}getById`, { id });
      return extractApiData<Account>(response);
    } catch (error) {
      handleApiError(error);
    }
  }

  async getAllAccounts(): Promise<Account[]> {
    try {
      const response = await apiClient.post(Routes.Account.GetAll, {});
      return extractApiData<Account[]>(response);
    } catch (error) {
      handleApiError(error);
    }
  }

  async createAccount(data: CreateAccountRequest): Promise<string> {
    try {
      const response = await apiClient.post(Routes.Account.Create, data);
      return extractApiData<string>(response);
    } catch (error) {
      handleApiError(error);
    }
  }

  async updateAccount(data: UpdateAccountRequest): Promise<string> {
    try {
      const response = await apiClient.post(Routes.Account.Edit, data);
      return extractApiData<string>(response);
    } catch (error) {
      handleApiError(error);
    }
  }

  async deleteAccount(id: string): Promise<void> {
    try {
      await apiClient.post(`${Routes.Account.Prefix}delete`, { id });
    } catch (error) {
      handleApiError(error);
    }
  }
}

export const accountService = new AccountService();

