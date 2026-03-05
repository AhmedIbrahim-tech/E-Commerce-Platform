import { createSlice, createAsyncThunk } from '@reduxjs/toolkit';
import axios, { AxiosError } from 'axios';
import { Router } from '@/config/Router';
import { ApiResponse } from '@/types/App/ApiResponse';
import { ApiError } from '@/types/App/ApiError';
import type {
    Role,
    ManageUserRolesResponse,
    UpdateUserRolesPayload,
    ManageUserClaimsResponse,
    UpdateUserClaimsPayload,
} from '@/types/users/authorization';
import { authorizationInitialState } from '@/types/users/authorization';

// ── Helper ──────────────────────────────────────────────────────────

function getAuthHeaders() {
    const token = typeof window !== 'undefined' ? localStorage.getItem('accessToken') : null;
    return token ? { Authorization: `Bearer ${token}` } : {};
}

function extractError(error: unknown, fallback: string): string {
    if (axios.isAxiosError(error)) {
        const data = (error as AxiosError<ApiError>).response?.data;
        if (data) return data.message || data.detail || data.title || fallback;
    }
    return fallback;
}

// ── Roles thunks ────────────────────────────────────────────────────

export const getRoles = createAsyncThunk(
    'authorization/getRoles',
    async (_, { rejectWithValue }) => {
        try {
            const res = await axios.get<ApiResponse<Role[]>>(
                Router.Authorization.GetAllRoles, { headers: getAuthHeaders() }
            );
            return res.data.data ?? [];
        } catch (e) { return rejectWithValue(extractError(e, 'Failed to load roles')); }
    }
);

export const getRoleById = createAsyncThunk(
    'authorization/getRoleById',
    async (id: string, { rejectWithValue }) => {
        try {
            const res = await axios.get<ApiResponse<Role>>(
                Router.Authorization.GetRoleById.replace('{id}', id), { headers: getAuthHeaders() }
            );
            return res.data.data;
        } catch (e) { return rejectWithValue(extractError(e, 'Failed to load role')); }
    }
);

export const createRole = createAsyncThunk(
    'authorization/createRole',
    async (name: string, { rejectWithValue }) => {
        try {
            const formData = new FormData();
            formData.append('name', name);
            const res = await axios.post<ApiResponse<string>>(
                Router.Authorization.CreateRole, formData, { headers: getAuthHeaders() }
            );
            return res.data;
        } catch (e) { return rejectWithValue(extractError(e, 'Failed to create role')); }
    }
);

export const editRole = createAsyncThunk(
    'authorization/editRole',
    async (payload: { id: string; name: string }, { rejectWithValue }) => {
        try {
            const formData = new FormData();
            formData.append('id', payload.id);
            formData.append('name', payload.name);
            const res = await axios.put<ApiResponse<string>>(
                Router.Authorization.EditRole, formData, { headers: getAuthHeaders() }
            );
            return res.data;
        } catch (e) { return rejectWithValue(extractError(e, 'Failed to edit role')); }
    }
);

export const deleteRole = createAsyncThunk(
    'authorization/deleteRole',
    async (id: string, { rejectWithValue }) => {
        try {
            await axios.delete(
                Router.Authorization.DeleteRole.replace('{id}', id), { headers: getAuthHeaders() }
            );
            return id;
        } catch (e) { return rejectWithValue(extractError(e, 'Failed to delete role')); }
    }
);

// ── User Roles thunks ───────────────────────────────────────────────

export const manageUserRoles = createAsyncThunk(
    'authorization/manageUserRoles',
    async (userId: string, { rejectWithValue }) => {
        try {
            const res = await axios.get<ApiResponse<ManageUserRolesResponse>>(
                Router.Authorization.ManageUserRoles.replace('{id}', userId), { headers: getAuthHeaders() }
            );
            return res.data.data;
        } catch (e) { return rejectWithValue(extractError(e, 'Failed to load user roles')); }
    }
);

export const updateUserRoles = createAsyncThunk(
    'authorization/updateUserRoles',
    async (payload: UpdateUserRolesPayload, { rejectWithValue }) => {
        try {
            const res = await axios.put<ApiResponse<string>>(
                Router.Authorization.UpdateUserRoles, payload, { headers: getAuthHeaders() }
            );
            return res.data;
        } catch (e) { return rejectWithValue(extractError(e, 'Failed to update user roles')); }
    }
);

// ── User Claims thunks ──────────────────────────────────────────────

export const manageUserClaims = createAsyncThunk(
    'authorization/manageUserClaims',
    async (userId: string, { rejectWithValue }) => {
        try {
            const res = await axios.get<ApiResponse<ManageUserClaimsResponse>>(
                Router.Authorization.ManageUserClaims.replace('{id}', userId), { headers: getAuthHeaders() }
            );
            return res.data.data;
        } catch (e) { return rejectWithValue(extractError(e, 'Failed to load user claims')); }
    }
);

export const updateUserClaims = createAsyncThunk(
    'authorization/updateUserClaims',
    async (payload: UpdateUserClaimsPayload, { rejectWithValue }) => {
        try {
            const res = await axios.put<ApiResponse<string>>(
                Router.Authorization.UpdateUserClaims, payload, { headers: getAuthHeaders() }
            );
            return res.data;
        } catch (e) { return rejectWithValue(extractError(e, 'Failed to update user claims')); }
    }
);

// ── Slice ───────────────────────────────────────────────────────────

const authorizationSlice = createSlice({
    name: 'authorization',
    initialState: authorizationInitialState,
    reducers: {
        clearActionError: (s) => { s.actionError = null; },
        setSelectedRole: (s, a) => { s.selectedRole = a.payload; },
    },
    extraReducers: (builder) => {
        builder
            // Roles list
            .addCase(getRoles.pending, (s) => { s.rolesLoading = true; s.rolesError = null; })
            .addCase(getRoles.fulfilled, (s, a) => { s.rolesLoading = false; s.roles = a.payload as Role[]; })
            .addCase(getRoles.rejected, (s, a) => { s.rolesLoading = false; s.rolesError = a.payload as string; })

            // Role CRUD actions
            .addCase(createRole.pending, (s) => { s.actionLoading = true; s.actionError = null; })
            .addCase(createRole.fulfilled, (s) => { s.actionLoading = false; })
            .addCase(createRole.rejected, (s, a) => { s.actionLoading = false; s.actionError = a.payload as string; })

            .addCase(editRole.pending, (s) => { s.actionLoading = true; s.actionError = null; })
            .addCase(editRole.fulfilled, (s) => { s.actionLoading = false; })
            .addCase(editRole.rejected, (s, a) => { s.actionLoading = false; s.actionError = a.payload as string; })

            .addCase(deleteRole.pending, (s) => { s.actionLoading = true; s.actionError = null; })
            .addCase(deleteRole.fulfilled, (s, a) => {
                s.actionLoading = false;
                s.roles = s.roles.filter((r) => r.id !== a.payload);
            })
            .addCase(deleteRole.rejected, (s, a) => { s.actionLoading = false; s.actionError = a.payload as string; })

            // User roles
            .addCase(manageUserRoles.pending, (s) => { s.userRolesLoading = true; })
            .addCase(manageUserRoles.fulfilled, (s, a) => {
                s.userRolesLoading = false;
                s.userRoles = (a.payload as ManageUserRolesResponse) ?? null;
            })
            .addCase(manageUserRoles.rejected, (s) => { s.userRolesLoading = false; })

            .addCase(updateUserRoles.pending, (s) => { s.actionLoading = true; s.actionError = null; })
            .addCase(updateUserRoles.fulfilled, (s) => { s.actionLoading = false; })
            .addCase(updateUserRoles.rejected, (s, a) => { s.actionLoading = false; s.actionError = a.payload as string; })

            // User claims
            .addCase(manageUserClaims.pending, (s) => { s.userClaimsLoading = true; })
            .addCase(manageUserClaims.fulfilled, (s, a) => {
                s.userClaimsLoading = false;
                s.userClaims = (a.payload as ManageUserClaimsResponse) ?? null;
            })
            .addCase(manageUserClaims.rejected, (s) => { s.userClaimsLoading = false; })

            .addCase(updateUserClaims.pending, (s) => { s.actionLoading = true; s.actionError = null; })
            .addCase(updateUserClaims.fulfilled, (s) => { s.actionLoading = false; })
            .addCase(updateUserClaims.rejected, (s, a) => { s.actionLoading = false; s.actionError = a.payload as string; });
    },
});

export const { clearActionError, setSelectedRole } = authorizationSlice.actions;
export default authorizationSlice.reducer;
