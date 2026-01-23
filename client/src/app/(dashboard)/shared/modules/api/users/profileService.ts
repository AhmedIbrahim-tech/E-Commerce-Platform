import apiClient from "@/services/core/apiClient";
import { Routes } from "@/constants/apiroutes";
import type { ActivityItem, MyProfile, PaginatedResponse, UpdateMyProfileRequest, UserDocument } from "@/types";
import { extractApiData } from "@/types/api";
import { handleApiError } from "@/services/core/apiErrorHandler";

class ProfileService {
  async getMyProfile(): Promise<MyProfile> {
    try {
      const response = await apiClient.get(Routes.User.Profile);
      return extractApiData<MyProfile>(response);
    } catch (error) {
      handleApiError(error);
    }
  }

  async updateMyProfile(payload: UpdateMyProfileRequest): Promise<MyProfile> {
    try {
      const formData = new FormData();
      formData.append("displayName", payload.displayName);
      if (payload.phoneNumber) formData.append("phoneNumber", payload.phoneNumber);
      if (payload.profileImage) formData.append("profileImage", payload.profileImage);

      const response = await apiClient.put(Routes.User.Profile, formData, {
        headers: { "Content-Type": "multipart/form-data" },
      });
      return extractApiData<MyProfile>(response);
    } catch (error) {
      handleApiError(error);
    }
  }

  async getMyActivitiesPaginatedList(params?: {
    pageNumber?: number;
    pageSize?: number;
    search?: string;
    category?: "Orders" | "Profile" | "Documents" | "Security";
    sortBy?: number;
  }): Promise<PaginatedResponse<ActivityItem>> {
    try {
      const body = {
        pageNumber: params?.pageNumber ?? 1,
        pageSize: params?.pageSize ?? 10,
        search: params?.search || null,
        category: params?.category || null,
        sortBy: params?.sortBy ?? 0,
        startDate: null,
        endDate: null,
      };
      const response = await apiClient.post(Routes.User.MyActivitiesPaginated, body);
      return extractApiData<PaginatedResponse<ActivityItem>>(response);
    } catch (error) {
      handleApiError(error);
    }
  }

  async getMyDocuments(): Promise<UserDocument[]> {
    try {
      const response = await apiClient.get(Routes.User.Documents);
      return extractApiData<UserDocument[]>(response);
    } catch (error) {
      handleApiError(error);
    }
  }

  async uploadMyDocument(payload: { type: string; file: File }): Promise<UserDocument> {
    try {
      const formData = new FormData();
      formData.append("type", payload.type);
      formData.append("file", payload.file);

      const response = await apiClient.post(Routes.User.Documents, formData, {
        headers: { "Content-Type": "multipart/form-data" },
      });
      return extractApiData<UserDocument>(response);
    } catch (error) {
      handleApiError(error);
    }
  }

  async deleteMyDocument(id: string): Promise<void> {
    try {
      await apiClient.delete(Routes.User.DocumentById(id));
    } catch (error) {
      handleApiError(error);
    }
  }

  async downloadMyDocument(id: string): Promise<Blob> {
    try {
      const response = await apiClient.get(Routes.User.DocumentDownload(id), {
        responseType: "blob",
      });
      return response.data as Blob;
    } catch (error) {
      handleApiError(error);
    }
  }
}

export const profileService = new ProfileService();

