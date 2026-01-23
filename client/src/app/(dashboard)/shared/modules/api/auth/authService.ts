import axios from "axios";
import apiClient from "@/services/core/apiClient";
import { Routes } from "@/constants/apiroutes";
import type { LoginRequest, RegisterRequest, AuthTokens, User, UserRole } from "@/types";
import { clearStoredTokens, getStoredTokens } from "./authStorage";
import { apiUrl } from "@/config/api";

class AuthService {
  async login(credentials: LoginRequest): Promise<{ user: User; tokens: AuthTokens }> {
    const response = await apiClient.post(Routes.Authentication.SignIn, credentials);
    const responseData = response.data.data || response.data;

    // Extract tokens from response
    // refreshToken comes as an object with tokenString property
    const accessToken =
      responseData.accessToken || responseData.AccessToken || responseData.token || responseData.Token;
    const refreshTokenRaw = responseData.refreshToken ?? responseData.RefreshToken;

    const tokens: AuthTokens = {
      accessToken,
      refreshToken:
        typeof refreshTokenRaw === "string"
          ? refreshTokenRaw
          : refreshTokenRaw?.tokenString || refreshTokenRaw?.TokenString || refreshTokenRaw,
    };

    const user = this.getUserFromAccessToken(tokens.accessToken);
    return { user, tokens };
  }

  async register(data: RegisterRequest): Promise<{ user: User; tokens: AuthTokens }> {
    const formData = new FormData();
    formData.append("firstName", data.firstName || "");
    formData.append("lastName", data.lastName || "");
    formData.append("userName", data.userName || "");
    formData.append("email", data.email || "");
    if (data.gender !== undefined) {
      formData.append("gender", data.gender.toString());
    }
    if (data.phoneNumber) {
      formData.append("phoneNumber", data.phoneNumber);
    }
    if (data.secondPhoneNumber) {
      formData.append("secondPhoneNumber", data.secondPhoneNumber);
    }
    formData.append("password", data.password || "");
    formData.append("confirmPassword", data.confirmPassword || "");
    if (data.profileImage) {
      formData.append("profileImage", data.profileImage);
    }

    await apiClient.post(Routes.User.Register, formData, {
      headers: {
        "Content-Type": "multipart/form-data",
      },
    });

    // Backend register does not return tokens; perform login to get tokens + user.
    return await this.login({ email: data.email, password: data.password });
  }

  async logout(): Promise<void> {
    try {
      const tokens = getStoredTokens();
      if (tokens?.accessToken && tokens?.refreshToken) {
        const form = new URLSearchParams();
        form.set("refreshToken", tokens.refreshToken);
        await axios.post(apiUrl(Routes.Authentication.Logout), form, {
          headers: {
            "Content-Type": "application/x-www-form-urlencoded",
            Authorization: `Bearer ${tokens.accessToken}`,
          },
        });
      }
    } catch {
    } finally {
      clearStoredTokens();
    }
  }

  async getCurrentUser(): Promise<User> {
    const tokens = getStoredTokens();
    if (!tokens?.accessToken) {
      throw new Error("No access token available");
    }

    return this.getUserFromAccessToken(tokens.accessToken);
  }

  async validateToken(accessToken: string): Promise<boolean> {
    try {
      await axios.get(apiUrl(Routes.Authentication.ValidateToken), {
        params: { accessToken },
        headers: { Authorization: `Bearer ${accessToken}` },
      });
      return true;
    } catch {
      return false;
    }
  }

  async refreshTokens(tokens: AuthTokens): Promise<AuthTokens> {
    const form = new URLSearchParams();
    form.set("accessToken", tokens.accessToken);
    form.set("refreshToken", tokens.refreshToken);

    const response = await axios.post(apiUrl(Routes.Authentication.RefreshToken), form, {
      headers: { "Content-Type": "application/x-www-form-urlencoded" },
    });
    const responseData = response.data.data || response.data;

    const accessToken =
      responseData.accessToken || responseData.AccessToken || responseData.token || responseData.Token;
    const refreshTokenRaw = responseData.refreshToken ?? responseData.RefreshToken;

    return {
      accessToken,
      refreshToken:
        typeof refreshTokenRaw === "string"
          ? refreshTokenRaw
          : refreshTokenRaw?.tokenString || refreshTokenRaw?.TokenString || refreshTokenRaw,
    };
  }

  getUserFromAccessToken(accessToken: string): User {
    const tokenParts = accessToken.split(".");
    if (tokenParts.length !== 3) throw new Error("Invalid token format");

    const payload = this.decodeJwtPayload(tokenParts[1]);
    const asString = (v: unknown) => (typeof v === "string" ? v : "");
    const firstString = (v: unknown) => (Array.isArray(v) ? asString(v[0]) : asString(v));

    const emailClaim = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress";
    const roleClaim = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role";
    const nameIdentifierClaim = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier";
    const nameClaim = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name";

    const role = asString(payload["role"]) || firstString(payload[roleClaim]) || "Customer";

    const user: User = {
      id: asString(payload["Id"]) || asString(payload["id"]) || asString(payload["sub"]),
      email: asString(payload["email"]) || asString(payload["Email"]) || asString(payload[emailClaim]),
      firstName: asString(payload["firstName"]) || asString(payload["given_name"]),
      lastName: asString(payload["lastName"]) || asString(payload["family_name"]),
      userName:
        asString(payload[nameIdentifierClaim]) || asString(payload["userName"]) || asString(payload["UserName"]),
      displayName: asString(payload[nameClaim]) || asString(payload["displayName"]) || asString(payload["DisplayName"]),
      role: role as UserRole,
      claims: this.extractClaims(payload),
      phoneNumber: asString(payload["PhoneNumber"]) || asString(payload["phoneNumber"]) || undefined,
      profileImageUrl: asString(payload["ProfileImage"]) || asString(payload["profileImage"]) || undefined,
    };

    // Backend uses ClaimTypes.Name for display name; if displayName is missing, fallback to userName.
    if (!user.displayName) user.displayName = user.userName || "";

    return user;
  }

  private extractClaims(payload: Record<string, unknown>): string[] {
    const claims: string[] = [];

    for (const key in payload) {
      if (payload[key] === "True" || payload[key] === true) {
        if (key.includes(".") || key.includes("/")) {
          claims.push(key);
        }
      }
    }

    if (claims.length > 0) return claims;

    const permissions = payload["permissions"];
    if (Array.isArray(permissions) && permissions.every((x) => typeof x === "string")) return permissions;

    const fallbackClaims = payload["claims"];
    if (Array.isArray(fallbackClaims) && fallbackClaims.every((x) => typeof x === "string")) return fallbackClaims;

    return [];
  }

  private decodeJwtPayload(base64Url: string): Record<string, unknown> {
    try {
      // base64url -> base64
      const base64 = base64Url.replace(/-/g, "+").replace(/_/g, "/");
      const padded = base64.padEnd(base64.length + ((4 - (base64.length % 4)) % 4), "=");
      const json = atob(padded);
      const parsed = JSON.parse(json) as unknown;
      if (parsed && typeof parsed === "object") return parsed as Record<string, unknown>;
      throw new Error("Invalid token payload");
    } catch {
      throw new Error("Failed to decode token");
    }
  }
}

export const authService = new AuthService();

