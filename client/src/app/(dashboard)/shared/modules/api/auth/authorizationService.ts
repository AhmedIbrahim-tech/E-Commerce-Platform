import apiClient from "@/services/core/apiClient";
import { Routes } from "@/constants/apiroutes";
import type {
  CreateRoleRequest,
  ManageUserClaimsResponse,
  ManageUserRolesResponse,
  Role,
  RoleListResponse,
  UpdateRoleRequest,
  UpdateUserClaimsRequest,
  UpdateUserRolesRequest,
} from "@/types";
import { extractApiData } from "@/types/api";
import { handleApiError } from "@/services/core/apiErrorHandler";

class AuthorizationService {
  async getAllRoles(): Promise<RoleListResponse> {
    try {
      const response = await apiClient.get(Routes.Authorization.GetAllRoles);
      const extractedData = extractApiData<RoleListResponse | Array<Record<string, unknown>>>(response);

      let rolesArray: Array<Record<string, unknown>> = [];

      if (Array.isArray(extractedData)) {
        rolesArray = extractedData;
      } else if (extractedData && typeof extractedData === "object" && "roles" in extractedData) {
        rolesArray = ((extractedData as RoleListResponse).roles || []).map((r) => r as unknown as Record<string, unknown>);
      }

      const asString = (v: unknown) => (typeof v === "string" ? v : "");
      const transformedRoles: Role[] = rolesArray.map((role) => ({
        id: asString(role["roleId"] ?? role["id"]),
        name: asString(role["roleName"] ?? role["name"]),
        normalizedName: typeof role["normalizedName"] === "string" ? role["normalizedName"] : undefined,
        createdDate: typeof role["createdDate"] === "string" ? role["createdDate"] : undefined,
        status: asString(role["status"]) === "Inactive" ? "Inactive" : "Active",
      }));

      return { roles: transformedRoles };
    } catch (error) {
      handleApiError(error);
    }
  }

  async getRoleById(id: string): Promise<Role> {
    try {
      const response = await apiClient.get(Routes.Authorization.GetRoleById(id));
      return extractApiData<Role>(response);
    } catch (error) {
      handleApiError(error);
    }
  }

  async createRole(data: CreateRoleRequest): Promise<string> {
    try {
      const formData = new FormData();
      formData.append("name", data.name);
      const response = await apiClient.post(Routes.Authorization.CreateRole, formData, {
        headers: {
          "Content-Type": "multipart/form-data",
        },
      });
      return extractApiData<string>(response);
    } catch (error) {
      handleApiError(error);
    }
  }

  async updateRole(data: UpdateRoleRequest): Promise<string> {
    try {
      const formData = new FormData();
      formData.append("id", data.id);
      formData.append("name", data.name);
      const response = await apiClient.put(Routes.Authorization.EditRole, formData, {
        headers: {
          "Content-Type": "multipart/form-data",
        },
      });
      return extractApiData<string>(response);
    } catch (error) {
      handleApiError(error);
    }
  }

  async deleteRole(id: string): Promise<void> {
    try {
      await apiClient.delete(Routes.Authorization.DeleteRole(id));
    } catch (error) {
      handleApiError(error);
    }
  }

  async manageUserRoles(userId: string): Promise<ManageUserRolesResponse> {
    try {
      const response = await apiClient.post(`${Routes.Authorization.Role}manageUserRoles`, { userId });
      return extractApiData<ManageUserRolesResponse>(response);
    } catch (error) {
      handleApiError(error);
    }
  }

  async updateUserRoles(data: UpdateUserRolesRequest): Promise<void> {
    try {
      await apiClient.put(Routes.Authorization.UpdateUserRoles, data);
    } catch (error) {
      handleApiError(error);
    }
  }

  async manageUserClaims(userId: string): Promise<ManageUserClaimsResponse> {
    try {
      const response = await apiClient.post(`${Routes.Authorization.Claim}manageUserClaims`, { userId });
      return extractApiData<ManageUserClaimsResponse>(response);
    } catch (error) {
      handleApiError(error);
    }
  }

  async updateUserClaims(data: UpdateUserClaimsRequest): Promise<void> {
    try {
      await apiClient.put(Routes.Authorization.UpdateUserClaims, data);
    } catch (error) {
      handleApiError(error);
    }
  }
}

export const authorizationService = new AuthorizationService();

