import apiClient from "@/services/core/apiClient";
import { Routes } from "@/constants/apiroutes";
import type { CreateVendorRequest, UpdateVendorRequest, Vendor, VendorListResponse } from "@/types";
import { extractApiData } from "@/types/api";
import { handleApiError } from "@/services/core/apiErrorHandler";

class VendorService {
  async getVendorPaginatedList(pageNumber: number = 1, pageSize: number = 10, search?: string): Promise<VendorListResponse> {
    try {
      const requestBody = {
        pageNumber,
        pageSize,
        ...(search && { search }),
      };

      const response = await apiClient.post(Routes.Vendor.Paginated, requestBody);
      return extractApiData<VendorListResponse>(response);
    } catch (error) {
      handleApiError(error);
    }
  }

  async getVendorById(id: string): Promise<Vendor> {
    try {
      const response = await apiClient.post(`${Routes.Vendor.Prefix}getById`, { id });
      return extractApiData<Vendor>(response);
    } catch (error) {
      handleApiError(error);
    }
  }

  async createVendor(data: CreateVendorRequest): Promise<string> {
    try {
      const response = await apiClient.post(Routes.Vendor.Create, data);
      return extractApiData<string>(response);
    } catch (error) {
      handleApiError(error);
    }
  }

  async updateVendor(data: UpdateVendorRequest): Promise<string> {
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
      if (data.storeName) formData.append("storeName", data.storeName);
      if (data.commissionRate !== undefined) formData.append("commissionRate", data.commissionRate.toString());
      if (data.profileImage) {
        formData.append("profileImage", data.profileImage);
      }

      const response = await apiClient.put(Routes.Vendor.Edit, formData, {
        headers: {
          "Content-Type": "multipart/form-data",
        },
      });
      return extractApiData<string>(response);
    } catch (error) {
      handleApiError(error);
    }
  }

  async deleteVendor(id: string): Promise<void> {
    try {
      await apiClient.delete(Routes.Vendor.Delete(id));
    } catch (error) {
      handleApiError(error);
    }
  }

  async toggleVendorStatus(id: string): Promise<string> {
    try {
      const response = await apiClient.post(Routes.Vendor.ToggleStatus(id));
      return extractApiData<string>(response);
    } catch (error) {
      handleApiError(error);
    }
  }
}

export const vendorService = new VendorService();

