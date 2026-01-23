import apiClient from "@/services/core/apiClient";
import { Routes } from "@/constants/apiroutes";
import { extractApiData } from "@/types/api";
import { handleApiError } from "@/services/core/apiErrorHandler";

export interface Role {
  roleId: string;
  roleName?: string;
}

export interface UserRolesResponse {
  userId: string;
  userName?: string;
  userRoles: string[];
  allRoles: Role[];
}

export interface UserClaimsResponse {
  userId: string;
  userName?: string;
  userClaims: Array<{ type: string; value: boolean }>;
}

export interface UpdateUserRolesRequest {
  userId: string;
  roles: string[];
}

export interface UpdateUserClaimsRequest {
  userId: string;
  userClaims: Array<{ type: string; value: boolean }>;
}

class RoleService {
  async getAllRoles(): Promise<Role[]> {
    try {
      const response = await apiClient.get(Routes.Authorization.GetAllRoles);
      return extractApiData<Role[]>(response);
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

  async createRole(name: string): Promise<string> {
    try {
      const formData = new FormData();
      formData.append("name", name);
      const response = await apiClient.post(Routes.Authorization.CreateRole, formData);
      return extractApiData<string>(response);
    } catch (error) {
      handleApiError(error);
    }
  }

  async updateRole(id: string, name: string): Promise<string> {
    try {
      const formData = new FormData();
      formData.append("id", id);
      formData.append("name", name);
      const response = await apiClient.put(Routes.Authorization.EditRole, formData);
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

  async getUserRoles(userId: string): Promise<UserRolesResponse> {
    try {
      const response = await apiClient.get(Routes.Authorization.ManageUserRoles(userId));
      return extractApiData<UserRolesResponse>(response);
    } catch (error) {
      handleApiError(error);
    }
  }

  async updateUserRoles(payload: UpdateUserRolesRequest): Promise<string> {
    try {
      const response = await apiClient.put(Routes.Authorization.UpdateUserRoles, payload);
      return extractApiData<string>(response);
    } catch (error) {
      handleApiError(error);
    }
  }

  async getUserClaims(userId: string): Promise<UserClaimsResponse> {
    try {
      const response = await apiClient.get(Routes.Authorization.ManageUserClaims(userId));
      return extractApiData<UserClaimsResponse>(response);
    } catch (error) {
      handleApiError(error);
    }
  }

  async updateUserClaims(payload: UpdateUserClaimsRequest): Promise<string> {
    try {
      const response = await apiClient.put(Routes.Authorization.UpdateUserClaims, payload);
      return extractApiData<string>(response);
    } catch (error) {
      handleApiError(error);
    }
  }
}

export const roleService = new RoleService();
