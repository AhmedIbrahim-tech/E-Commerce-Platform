import apiClient from "@/services/core/apiClient";
import { Routes } from "@/constants/apiroutes";
import type { CreateShippingAddressRequest, ShippingAddress, UpdateShippingAddressRequest } from "@/types";
import { extractApiData } from "@/types/api";
import { handleApiError } from "@/services/core/apiErrorHandler";

class ShippingAddressService {
  async getShippingAddressList(): Promise<ShippingAddress[]> {
    try {
      const response = await apiClient.get(Routes.ShippingAddress.GetAll);
      const data = extractApiData<Array<{ id: string; firstName: string; lastName: string; street: string; city: string; state: string }>>(response);
      return data.map((item) => ({
        id: item.id,
        firstName: item.firstName,
        lastName: item.lastName,
        street: item.street,
        city: item.city,
        state: item.state,
      }));
    } catch (error) {
      handleApiError(error);
    }
  }

  async getShippingAddressById(id: string): Promise<ShippingAddress> {
    try {
      const response = await apiClient.get(Routes.ShippingAddress.GetById(id));
      const data = extractApiData<{ id: string; firstName: string; lastName: string; street: string; city: string; state: string }>(response);
      return {
        id: data.id,
        firstName: data.firstName,
        lastName: data.lastName,
        street: data.street,
        city: data.city,
        state: data.state,
      };
    } catch (error) {
      handleApiError(error);
    }
  }

  async createShippingAddress(payload: CreateShippingAddressRequest): Promise<string> {
    try {
      const formData = new FormData();
      formData.append("firstName", payload.firstName);
      formData.append("lastName", payload.lastName);
      formData.append("street", payload.street);
      formData.append("city", payload.city);
      formData.append("state", payload.state);

      const response = await apiClient.post(Routes.ShippingAddress.Create, formData);
      return extractApiData<string>(response);
    } catch (error) {
      handleApiError(error);
    }
  }

  async updateShippingAddress(payload: UpdateShippingAddressRequest): Promise<string> {
    try {
      const response = await apiClient.put(Routes.ShippingAddress.Edit, {
        id: payload.id,
        firstName: payload.firstName,
        lastName: payload.lastName,
        street: payload.street,
        city: payload.city,
        state: payload.state,
      });
      return extractApiData<string>(response);
    } catch (error) {
      handleApiError(error);
    }
  }

  async deleteShippingAddress(id: string): Promise<void> {
    try {
      await apiClient.delete(Routes.ShippingAddress.Delete(id));
    } catch (error) {
      handleApiError(error);
    }
  }
}

export const shippingAddressService = new ShippingAddressService();
