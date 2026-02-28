export interface AuthUser {
  accessToken: string;
  refreshToken?: string;
  roles: string[];
}

export interface LoginCredentials {
  email: string;
  password?: string;
  rememberMe?: boolean;
}

export interface RegisterCredentials {
  FirstName?: string;
  LastName?: string;
  UserName?: string;
  Email?: string;
  Gender?: number;
  PhoneNumber?: string;
  Password?: string;
  ConfirmPassword?: string;
}

export interface ConfirmEmailCredentials {
  userId: string;
  code: string;
}

export interface LoginApiResponse {
  accessToken: string;
  refreshToken: { userName?: string; tokenString: string; expireAt?: string };
  roles: string[];
}

export interface ForgotPasswordCredentials {
  email: string;
}

export interface ConfirmResetPasswordCredentials {
  code: string;
  email: string;
}

export interface ResetPasswordCredentials {
  email: string;
  newPassword: string;
  confirmPassword: string;
}
