import apiClient from "@/services/core/apiClient";
import { Routes } from "@/constants/apiroutes";
import type { Coupon, CouponListResponse, CreateCouponRequest, UpdateCouponRequest } from "@/types";
import { extractApiData } from "@/types/api";
import { handleApiError } from "@/services/core/apiErrorHandler";

class CouponService {
  async getCouponPaginatedList(pageNumber: number = 1, pageSize: number = 10, search?: string): Promise<CouponListResponse> {
    try {
      const requestBody = {
        pageNumber,
        pageSize,
        ...(search && { search }),
      };

      const response = await apiClient.post(Routes.Coupon.Paginated, requestBody);
      return extractApiData<CouponListResponse>(response);
    } catch (error) {
      handleApiError(error);
    }
  }

  async getCouponById(id: string): Promise<Coupon> {
    try {
      const response = await apiClient.post(`${Routes.Coupon.Prefix}getById`, { id });
      return extractApiData<Coupon>(response);
    } catch (error) {
      handleApiError(error);
    }
  }

  async getAllCoupons(): Promise<Coupon[]> {
    try {
      const response = await apiClient.post(Routes.Coupon.GetAll, {});
      return extractApiData<Coupon[]>(response);
    } catch (error) {
      handleApiError(error);
    }
  }

  async createCoupon(data: CreateCouponRequest): Promise<string> {
    try {
      const response = await apiClient.post(Routes.Coupon.Create, data);
      return extractApiData<string>(response);
    } catch (error) {
      handleApiError(error);
    }
  }

  async updateCoupon(data: UpdateCouponRequest): Promise<string> {
    try {
      const response = await apiClient.post(Routes.Coupon.Edit, data);
      return extractApiData<string>(response);
    } catch (error) {
      handleApiError(error);
    }
  }

  async deleteCoupon(id: string): Promise<void> {
    try {
      await apiClient.post(`${Routes.Coupon.Prefix}delete`, { id });
    } catch (error) {
      handleApiError(error);
    }
  }
}

export const couponService = new CouponService();

