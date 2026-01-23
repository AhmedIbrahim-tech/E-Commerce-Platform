import apiClient from "@/services/core/apiClient";
import { Routes } from "@/constants/apiroutes";
import type { Admin, AdminListResponse, CreateAdminRequest, UpdateAdminRequest } from "@/types";
import { extractApiData } from "@/types/api";
import { handleApiError } from "@/services/core/apiErrorHandler";

class AdminService {
  async getAdminPaginatedList(
    pageNumber: number = 1,
    pageSize: number = 10,
    search?: string
  ): Promise<AdminListResponse> {
    try {
      const requestBody = {
        pageNumber,
        pageSize,
        ...(search && { search }),
      };

      const response = await apiClient.post(Routes.Admin.Paginated, requestBody);
      return extractApiData<AdminListResponse>(response);
    } catch (error) {
      handleApiError(error);
    }
  }

  async getAdminById(id: string): Promise<Admin> {
    try {
      const response = await apiClient.post(`${Routes.Admin.Prefix}getById`, { id });
      return extractApiData<Admin>(response);
    } catch (error) {
      handleApiError(error);
    }
  }

  async createAdmin(data: CreateAdminRequest): Promise<string> {
    try {
      const response = await apiClient.post(Routes.Admin.Create, data);
      return extractApiData<string>(response);
    } catch (error) {
      handleApiError(error);
    }
  }

  async updateAdmin(data: UpdateAdminRequest): Promise<string> {
    try {
      const formData = new FormData();
      formData.append("id", data.id);
      if (data.firstName) formData.append("firstName", data.firstName);
      if (data.lastName) formData.append("lastName", data.lastName);
      if (data.userName) formData.append("userName", data.userName);
      if (data.email) formData.append("email", data.email);
      if (data.gender !== undefined) formData.append("gender", data.gender.toString());
      if (data.phoneNumber) formData.append("phoneNumber", data.phoneNumber);
      if (data.secondPhoneNumber) formData.append("secondPhoneNumber", data.secondPhoneNumber);
      if (data.address) formData.append("address", data.address);
      if (data.profileImage) {
        formData.append("profileImage", data.profileImage);
      }

      const response = await apiClient.put(Routes.Admin.Edit, formData, {
        headers: {
          "Content-Type": "multipart/form-data",
        },
      });
      return extractApiData<string>(response);
    } catch (error) {
      handleApiError(error);
    }
  }

  async deleteAdmin(id: string): Promise<void> {
    try {
      await apiClient.delete(Routes.Admin.Delete(id));
    } catch (error) {
      handleApiError(error);
    }
  }

  async toggleAdminStatus(id: string): Promise<string> {
    try {
      const response = await apiClient.post(Routes.Admin.ToggleStatus(id));
      return extractApiData<string>(response);
    } catch (error) {
      handleApiError(error);
    }
  }
}

export const adminService = new AdminService();

