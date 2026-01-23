import apiClient from "@/services/core/apiClient";
import { Routes } from "@/constants/apiroutes";
import type { ApiResponse } from "@/types/api";
import type { NotificationListResponse } from "@/types";
import { extractApiData } from "@/types/api";
import { handleApiError } from "@/services/core/apiErrorHandler";

type NotificationsMeta = { unreadCount?: number };

class NotificationService {
  async getNotifications(pageNumber: number = 1, pageSize: number = 10): Promise<{ page: NotificationListResponse; unreadCount: number }> {
    try {
      const response = await apiClient.get<ApiResponse<NotificationListResponse>>(Routes.Notifications.List, {
        params: { pageNumber, pageSize },
      });

      const page = extractApiData<NotificationListResponse>(response);
      const meta = (response.data?.meta as NotificationsMeta | undefined) ?? {};
      const unreadCount = typeof meta.unreadCount === "number" ? meta.unreadCount : 0;

      return { page, unreadCount };
    } catch (error) {
      handleApiError(error);
    }
  }

  async markRead(id: string): Promise<string> {
    try {
      const response = await apiClient.post(Routes.Notifications.MarkRead, { id });
      return extractApiData<string>(response);
    } catch (error) {
      handleApiError(error);
    }
  }

  async markAllRead(): Promise<string> {
    try {
      const response = await apiClient.post(Routes.Notifications.MarkRead, { markAll: true });
      return extractApiData<string>(response);
    } catch (error) {
      handleApiError(error);
    }
  }
}

export const notificationService = new NotificationService();

