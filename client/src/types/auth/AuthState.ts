import { AuthUser } from "./auth";

export interface AuthState {
    accessToken: string | null;
    user: AuthUser | null;
    loading: boolean;
    error: string | null;
    registerLoading: boolean;
    registerError: string | null;
    confirmEmailLoading: boolean;
    confirmEmailError: string | null;
}

export const initialState: AuthState = {
    accessToken: typeof window !== 'undefined' ? localStorage.getItem('accessToken') : null,
    user: null,
    loading: false,
    error: null,
    registerLoading: false,
    registerError: null,
    confirmEmailLoading: false,
    confirmEmailError: null,
};
