import apiClient from "@/services/core/apiClient";
import { Routes } from "@/constants/apiroutes";
import type { CreateGiftCardRequest, GiftCard, GiftCardListResponse, UpdateGiftCardRequest } from "@/types";
import { extractApiData } from "@/types/api";
import { handleApiError } from "@/services/core/apiErrorHandler";

class GiftCardService {
  async getGiftCardPaginatedList(pageNumber: number = 1, pageSize: number = 10, search?: string): Promise<GiftCardListResponse> {
    try {
      const requestBody = {
        pageNumber,
        pageSize,
        ...(search && { search }),
      };

      const response = await apiClient.post(Routes.GiftCard.Paginated, requestBody);
      return extractApiData<GiftCardListResponse>(response);
    } catch (error) {
      handleApiError(error);
    }
  }

  async getGiftCardById(id: string): Promise<GiftCard> {
    try {
      const response = await apiClient.post(`${Routes.GiftCard.Prefix}getById`, { id });
      return extractApiData<GiftCard>(response);
    } catch (error) {
      handleApiError(error);
    }
  }

  async getAllGiftCards(): Promise<GiftCard[]> {
    try {
      const response = await apiClient.post(Routes.GiftCard.GetAll, {});
      return extractApiData<GiftCard[]>(response);
    } catch (error) {
      handleApiError(error);
    }
  }

  async createGiftCard(data: CreateGiftCardRequest): Promise<string> {
    try {
      const response = await apiClient.post(Routes.GiftCard.Create, data);
      return extractApiData<string>(response);
    } catch (error) {
      handleApiError(error);
    }
  }

  async updateGiftCard(data: UpdateGiftCardRequest): Promise<string> {
    try {
      const response = await apiClient.post(Routes.GiftCard.Edit, data);
      return extractApiData<string>(response);
    } catch (error) {
      handleApiError(error);
    }
  }

  async deleteGiftCard(id: string): Promise<void> {
    try {
      await apiClient.post(`${Routes.GiftCard.Prefix}delete`, { id });
    } catch (error) {
      handleApiError(error);
    }
  }
}

export const giftCardService = new GiftCardService();

