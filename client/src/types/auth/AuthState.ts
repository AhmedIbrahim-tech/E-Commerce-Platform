import { AuthUser } from "./auth";

export interface AuthState {
    accessToken: string | null;
    user: AuthUser | null;

    // Login
    loading: boolean;
    error: string | null;

    // Register
    registerLoading: boolean;
    registerError: string | null;

    // Confirm Email
    confirmEmailLoading: boolean;
    confirmEmailError: string | null;

    // Forgot Password (send code)
    forgotPasswordLoading: boolean;
    forgotPasswordError: string | null;

    // Confirm Reset Code
    confirmResetCodeLoading: boolean;
    confirmResetCodeError: string | null;

    // Reset Password
    resetPasswordLoading: boolean;
    resetPasswordError: string | null;

    // Two Step Verification
    twoStepLoading: boolean;
    twoStepError: string | null;

    // Validate Token
    validateTokenLoading: boolean;

    // Change Password
    changePasswordLoading: boolean;
    changePasswordError: string | null;

    // Logout
    logoutLoading: boolean;
}

export const initialState: AuthState = {
    accessToken: null,
    user: null,

    loading: false,
    error: null,

    registerLoading: false,
    registerError: null,

    confirmEmailLoading: false,
    confirmEmailError: null,

    forgotPasswordLoading: false,
    forgotPasswordError: null,

    confirmResetCodeLoading: false,
    confirmResetCodeError: null,

    resetPasswordLoading: false,
    resetPasswordError: null,

    twoStepLoading: false,
    twoStepError: null,

    validateTokenLoading: false,

    changePasswordLoading: false,
    changePasswordError: null,

    logoutLoading: false,
};
