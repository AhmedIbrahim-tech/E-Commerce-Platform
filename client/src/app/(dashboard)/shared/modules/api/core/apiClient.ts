import axios, { AxiosError, AxiosInstance, InternalAxiosRequestConfig } from "axios";
import { Routes } from "@/constants/apiroutes";
import { clearStoredTokens, getStoredTokens, setStoredTokens } from "@/services/auth/authStorage";
import { API_BASE_URL, apiUrl } from "@/config/api";

class ApiClient {
  private instance: AxiosInstance;

  constructor() {
    this.instance = axios.create({
      baseURL: API_BASE_URL,
      headers: {
        "Content-Type": "application/json",
      },
    });

    this.setupInterceptors();
  }

  private setupInterceptors(): void {
    this.instance.interceptors.request.use(
      (config: InternalAxiosRequestConfig) => {
        const tokens = getStoredTokens();
        if (tokens?.accessToken && config.headers) {
          config.headers.Authorization = `Bearer ${tokens.accessToken}`;
        }
        if (config.headers && typeof window !== "undefined") {
          const requestId = `${Date.now()}-${Math.random().toString(36).substr(2, 9)}`;
          config.headers["x-request-id"] = requestId;
        }
        return config;
      },
      (error: AxiosError) => Promise.reject(error)
    );

    this.instance.interceptors.response.use(
      (response) => response,
      async (error: AxiosError) => {
        const originalRequest = error.config as InternalAxiosRequestConfig & { _retry?: boolean };

        if (error.response?.status === 401 && !originalRequest._retry) {
          originalRequest._retry = true;

          try {
            const tokens = getStoredTokens();
            if (tokens?.accessToken && tokens?.refreshToken) {
              const newTokens = await this.refreshAccessToken(tokens.accessToken, tokens.refreshToken);
              setStoredTokens(newTokens);

              if (originalRequest.headers) {
                originalRequest.headers.Authorization = `Bearer ${newTokens.accessToken}`;
              }

              return this.instance(originalRequest);
            }
          } catch (refreshError) {
            clearStoredTokens();
            if (typeof window !== "undefined") {
              window.location.href = "/login";
            }
            return Promise.reject(refreshError);
          }
        }

        return Promise.reject(error);
      }
    );
  }

  private async refreshAccessToken(
    accessToken: string,
    refreshToken: string
  ): Promise<{ accessToken: string; refreshToken: string }> {
    const form = new URLSearchParams();
    form.set("accessToken", accessToken);
    form.set("refreshToken", refreshToken);

    const response = await axios.post(apiUrl(Routes.Authentication.RefreshToken), form, {
      headers: { "Content-Type": "application/x-www-form-urlencoded" },
    });
    const responseData = response.data.data || response.data;

    return {
      accessToken: responseData.accessToken || responseData.AccessToken,
      refreshToken:
        typeof responseData.refreshToken === "string"
          ? responseData.refreshToken
          : responseData.refreshToken?.tokenString ||
            responseData.refreshToken?.TokenString ||
            responseData.RefreshToken?.tokenString ||
            responseData.RefreshToken?.TokenString ||
            responseData.refreshToken ||
            responseData.RefreshToken,
    };
  }

  public getInstance(): AxiosInstance {
    return this.instance;
  }

  public setAuthToken(token: string): void {
    this.instance.defaults.headers.common["Authorization"] = `Bearer ${token}`;
  }

  public clearAuthToken(): void {
    delete this.instance.defaults.headers.common["Authorization"];
  }
}

export const apiClient = new ApiClient();
export default apiClient.getInstance();

