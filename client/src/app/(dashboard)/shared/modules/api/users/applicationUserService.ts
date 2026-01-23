import apiClient from "@/services/core/apiClient";
import { Routes } from "@/constants/apiroutes";
import type { ChangePasswordRequest, CreateAdminRequest, CreateVendorRequest } from "@/types";
import { extractApiData } from "@/types/api";
import { handleApiError } from "@/services/core/apiErrorHandler";

class ApplicationUserService {
  async createAdmin(data: CreateAdminRequest): Promise<string> {
    try {
      const formData = new FormData();
      formData.append("firstName", data.firstName || "");
      formData.append("lastName", data.lastName || "");
      formData.append("userName", data.userName || "");
      formData.append("email", data.email || "");
      if (data.gender !== undefined) {
        formData.append("gender", data.gender.toString());
      }
      if (data.phoneNumber) {
        formData.append("phoneNumber", data.phoneNumber);
      }
      if (data.secondPhoneNumber) {
        formData.append("secondPhoneNumber", data.secondPhoneNumber);
      }
      formData.append("password", data.password || "");
      formData.append("confirmPassword", data.confirmPassword || "");
      if (data.address) {
        formData.append("address", data.address);
      }
      if (data.profileImage) {
        formData.append("profileImage", data.profileImage);
      }

      const response = await apiClient.post(Routes.User.CreateAdmin, formData, {
        headers: {
          "Content-Type": "multipart/form-data",
        },
      });
      return extractApiData<string>(response);
    } catch (error) {
      handleApiError(error);
    }
  }

  async createVendor(data: CreateVendorRequest): Promise<string> {
    try {
      const formData = new FormData();
      formData.append("firstName", data.firstName || "");
      formData.append("lastName", data.lastName || "");
      formData.append("userName", data.userName || "");
      formData.append("email", data.email || "");
      if (data.gender !== undefined) {
        formData.append("gender", data.gender.toString());
      }
      if (data.phoneNumber) {
        formData.append("phoneNumber", data.phoneNumber);
      }
      if (data.secondPhoneNumber) {
        formData.append("secondPhoneNumber", data.secondPhoneNumber);
      }
      formData.append("password", data.password || "");
      formData.append("confirmPassword", data.confirmPassword || "");
      if (data.storeName) {
        formData.append("storeName", data.storeName);
      }
      if (data.commissionRate !== undefined) {
        formData.append("commissionRate", data.commissionRate.toString());
      }
      if (data.profileImage) {
        formData.append("profileImage", data.profileImage);
      }

      const response = await apiClient.post(Routes.User.CreateVendor, formData, {
        headers: {
          "Content-Type": "multipart/form-data",
        },
      });
      return extractApiData<string>(response);
    } catch (error) {
      handleApiError(error);
    }
  }

  async changePassword(data: ChangePasswordRequest): Promise<void> {
    try {
      await apiClient.put(Routes.User.ChangePassword, data);
    } catch (error) {
      handleApiError(error);
    }
  }
}

export const applicationUserService = new ApplicationUserService();

