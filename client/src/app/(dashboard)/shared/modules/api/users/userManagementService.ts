import apiClient from "@/services/core/apiClient";
import { Routes } from "@/constants/apiroutes";
import type { PaginatedResponse } from "@/types";
import { extractApiData } from "@/types/api";
import { handleApiError } from "@/services/core/apiErrorHandler";

export interface UserListItem {
  id: string;
  appUserId: string;
  fullName?: string;
  userName?: string;
  email?: string;
  phoneNumber?: string;
  role?: string;
  profileImageUrl?: string;
  isActive: boolean;
  createdAt: string;
}

export interface UserDetails {
  id: string;
  appUserId: string;
  fullName?: string;
  userName?: string;
  email?: string;
  phoneNumber?: string;
  role?: string;
  roles: string[];
  profileImageUrl?: string;
  isActive: boolean;
  createdAt: string;
  lastLoginAt?: string;
  claims: string[];
}

export interface CreateUserRequest {
  firstName: string;
  lastName: string;
  userName: string;
  email: string;
  phoneNumber?: string;
  password: string;
  role: string;
  profileImage?: File;
}

export interface UpdateUserRequest {
  id: string;
  firstName: string;
  lastName: string;
  userName: string;
  email: string;
  phoneNumber?: string;
  role?: string;
  profileImage?: File;
}

class UserManagementService {
  async getUsersPaginatedList(params: {
    pageNumber?: number;
    pageSize?: number;
    search?: string;
    role?: string;
    isActive?: boolean;
    sortBy?: number;
  }): Promise<PaginatedResponse<UserListItem>> {
    try {
      const body = {
        pageNumber: params.pageNumber ?? 1,
        pageSize: params.pageSize ?? 10,
        search: params.search || null,
        role: params.role || null,
        isActive: params.isActive ?? null,
        sortBy: params.sortBy ?? 0,
      };
      const response = await apiClient.post(Routes.User.UsersPaginated, body);
      return extractApiData<PaginatedResponse<UserListItem>>(response);
    } catch (error) {
      handleApiError(error);
    }
  }

  async getUserById(id: string): Promise<UserDetails> {
    try {
      const response = await apiClient.post(Routes.User.GetUserById, { id });
      return extractApiData<UserDetails>(response);
    } catch (error) {
      handleApiError(error);
    }
  }

  async createUser(payload: CreateUserRequest): Promise<string> {
    try {
      const formData = new FormData();
      formData.append("firstName", payload.firstName);
      formData.append("lastName", payload.lastName);
      formData.append("userName", payload.userName);
      formData.append("email", payload.email);
      if (payload.phoneNumber) formData.append("phoneNumber", payload.phoneNumber);
      formData.append("password", payload.password);
      if (payload.profileImage) formData.append("profileImage", payload.profileImage);

      // Use appropriate endpoint based on role
      let endpoint: string = Routes.User.Register; // Default to customer
      if (payload.role === "Admin" || payload.role === "SuperAdmin") {
        endpoint = Routes.User.CreateAdmin;
      } else if (payload.role === "Merchant") {
        endpoint = Routes.User.CreateVendor;
      }

      const response = await apiClient.post(endpoint, formData, {
        headers: { "Content-Type": "multipart/form-data" },
      });
      return extractApiData<string>(response);
    } catch (error) {
      handleApiError(error);
    }
  }

  async updateUser(payload: UpdateUserRequest): Promise<string> {
    try {
      // First get user to determine their role
      const user = await this.getUserById(payload.id);
      const userRole = user.role || "Customer";

      const formData = new FormData();
      formData.append("id", payload.id);
      formData.append("firstName", payload.firstName);
      formData.append("lastName", payload.lastName);
      formData.append("userName", payload.userName);
      formData.append("email", payload.email);
      if (payload.phoneNumber) formData.append("phoneNumber", payload.phoneNumber);
      if (payload.profileImage) formData.append("profileImage", payload.profileImage);

      // Use appropriate endpoint based on user role
      let endpoint: string = Routes.Customer.Edit;
      if (userRole === "Admin" || userRole === "SuperAdmin") {
        endpoint = Routes.Admin.Edit;
      } else if (userRole === "Merchant") {
        endpoint = Routes.Vendor.Edit;
      }

      const response = await apiClient.put(endpoint, formData, {
        headers: { "Content-Type": "multipart/form-data" },
      });
      return extractApiData<string>(response);
    } catch (error) {
      handleApiError(error);
    }
  }

  async toggleUserStatus(id: string, isActive: boolean): Promise<string> {
    try {
      const response = await apiClient.post(Routes.User.ToggleUserStatus, { id, isActive });
      return extractApiData<string>(response);
    } catch (error) {
      handleApiError(error);
    }
  }

  async deleteUser(id: string): Promise<void> {
    try {
      // First get user to determine their role
      const user = await this.getUserById(id);
      const userRole = user.role || "Customer";

      // Use appropriate endpoint based on user role
      if (userRole === "Admin" || userRole === "SuperAdmin") {
        await apiClient.delete(Routes.Admin.Delete(id));
      } else if (userRole === "Merchant") {
        await apiClient.delete(Routes.Vendor.Delete(id));
      } else {
        await apiClient.delete(Routes.Customer.Delete(id));
      }
    } catch (error) {
      handleApiError(error);
    }
  }
}

export const userManagementService = new UserManagementService();
