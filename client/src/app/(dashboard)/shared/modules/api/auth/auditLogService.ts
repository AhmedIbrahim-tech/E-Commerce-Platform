import apiClient from "@/services/core/apiClient";
import { Routes } from "@/constants/apiroutes";
import type { PaginatedResponse } from "@/types";
import { extractApiData } from "@/types/api";
import { handleApiError } from "@/services/core/apiErrorHandler";

export interface AuditLogItem {
  id: string;
  eventType: string;
  eventName: string;
  description?: string;
  userId?: string;
  userEmail?: string;
  additionalData?: string;
  createdTime: string;
}

class AuditLogService {
  async getAuditLogPaginatedList(params: {
    pageNumber?: number;
    pageSize?: number;
    search?: string;
    userId?: string;
    eventType?: string;
    startDate?: string;
    endDate?: string;
    sortBy?: number;
  }): Promise<PaginatedResponse<AuditLogItem>> {
    try {
      const body = {
        pageNumber: params.pageNumber ?? 1,
        pageSize: params.pageSize ?? 10,
        search: params.search || null,
        userId: params.userId || null,
        eventType: params.eventType || null,
        startDate: params.startDate || null,
        endDate: params.endDate || null,
        sortBy: params.sortBy ?? 0,
      };
      const response = await apiClient.post(Routes.AuditLog.Paginated, body);
      return extractApiData<PaginatedResponse<AuditLogItem>>(response);
    } catch (error) {
      handleApiError(error);
    }
  }
}

export const auditLogService = new AuditLogService();
