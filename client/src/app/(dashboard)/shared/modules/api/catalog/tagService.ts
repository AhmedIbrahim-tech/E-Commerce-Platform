import apiClient from "@/services/core/apiClient";
import { Routes } from "@/constants/apiroutes";
import { extractApiData } from "@/types/api";
import { handleApiError } from "@/services/core/apiErrorHandler";

class TagService {
  async getAllTags() {
    try {
      const response = await apiClient.post(Routes.Tag.GetAll, {});
      return extractApiData<unknown>(response) as any;
    } catch (error) {
      handleApiError(error);
    }
  }

  async getTagPaginatedList(pageNumber: number = 1, pageSize: number = 10, search?: string) {
    try {
      const requestBody = {
        pageNumber,
        pageSize,
        ...(search && { search }),
      };
      const response = await apiClient.post(Routes.Tag.Paginated, requestBody);
      return extractApiData<unknown>(response) as any;
    } catch (error) {
      handleApiError(error);
    }
  }

  async getTagById(id: string) {
    try {
      const response = await apiClient.post(Routes.Tag.GetById, { id });
      return extractApiData<unknown>(response) as any;
    } catch (error) {
      handleApiError(error);
    }
  }

  async createTag(data: { name: string; isActive: boolean }) {
    try {
      const response = await apiClient.post(Routes.Tag.Create, data);
      return extractApiData<unknown>(response) as any;
    } catch (error) {
      handleApiError(error);
    }
  }

  async updateTag(data: { id: string; name: string; isActive: boolean }) {
    try {
      const response = await apiClient.post(Routes.Tag.Edit, data);
      return extractApiData<unknown>(response) as any;
    } catch (error) {
      handleApiError(error);
    }
  }

  async deleteTag(id: string) {
    try {
      const response = await apiClient.post(Routes.Tag.Delete, { id });
      return extractApiData<unknown>(response) as any;
    } catch (error) {
      handleApiError(error);
    }
  }
}

export const tagService = new TagService();

