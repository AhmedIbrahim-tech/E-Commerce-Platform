import { createSlice, createAsyncThunk } from '@reduxjs/toolkit';
import axios, { AxiosError } from 'axios';
import { Router } from '@/config/Router';
import { ApiError } from '@/types/App/ApiError';
import type {
    AdminUser,
    CustomerUser,
    VendorUser,
    PaginatedRequest,
    PaginatedResponse,
    CreateAdminPayload,
    CreateVendorPayload,
    CreateCustomerPayload,
    EditAdminPayload,
    EditCustomerPayload,
    EditVendorPayload,
} from '@/types/users/userManagement';

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

// ── Paginated list thunks ───────────────────────────────────────────

export const getAdminsPaginated = createAsyncThunk(
    'userManagement/getAdminsPaginated',
    async (req: PaginatedRequest, { rejectWithValue }) => {
        try {
            const res = await axios.post<PaginatedResponse<AdminUser>>(
                Router.AdminRouting.Paginated, req, { headers: getAuthHeaders() }
            );
            return res.data;
        } catch (e) { return rejectWithValue(extractError(e, 'Failed to load admins')); }
    }
);

export const getCustomersPaginated = createAsyncThunk(
    'userManagement/getCustomersPaginated',
    async (req: PaginatedRequest, { rejectWithValue }) => {
        try {
            const res = await axios.post<PaginatedResponse<CustomerUser>>(
                Router.CustomerRouting.Paginated, req, { headers: getAuthHeaders() }
            );
            return res.data;
        } catch (e) { return rejectWithValue(extractError(e, 'Failed to load customers')); }
    }
);

export const getVendorsPaginated = createAsyncThunk(
    'userManagement/getVendorsPaginated',
    async (req: PaginatedRequest, { rejectWithValue }) => {
        try {
            const res = await axios.post<PaginatedResponse<VendorUser>>(
                Router.VendorRouting.Paginated, req, { headers: getAuthHeaders() }
            );
            return res.data;
        } catch (e) { return rejectWithValue(extractError(e, 'Failed to load merchants')); }
    }
);

// ── Create thunks ───────────────────────────────────────────────────

export const createAdmin = createAsyncThunk(
    'userManagement/createAdmin',
    async (payload: CreateAdminPayload, { rejectWithValue }) => {
        try {
            const res = await axios.post(Router.AdminRouting.Create, payload, { headers: getAuthHeaders() });
            return res.data;
        } catch (e) { return rejectWithValue(extractError(e, 'Failed to create admin')); }
    }
);

export const createVendor = createAsyncThunk(
    'userManagement/createVendor',
    async (payload: CreateVendorPayload, { rejectWithValue }) => {
        try {
            const res = await axios.post(Router.VendorRouting.Create, payload, { headers: getAuthHeaders() });
            return res.data;
        } catch (e) { return rejectWithValue(extractError(e, 'Failed to create merchant')); }
    }
);

export const createCustomer = createAsyncThunk(
    'userManagement/createCustomer',
    async (payload: CreateCustomerPayload, { rejectWithValue }) => {
        try {
            const res = await axios.post(Router.CustomerRouting.Create, payload, { headers: getAuthHeaders() });
            return res.data;
        } catch (e) { return rejectWithValue(extractError(e, 'Failed to create customer')); }
    }
);

// ── Edit thunks ─────────────────────────────────────────────────────

export const editAdmin = createAsyncThunk(
    'userManagement/editAdmin',
    async (payload: EditAdminPayload, { rejectWithValue }) => {
        try {
            const formData = new FormData();
            Object.entries(payload).forEach(([k, v]) => { if (v != null) formData.append(k, v instanceof File ? v : String(v)); });
            const res = await axios.put(Router.AdminRouting.Edit, formData, {
                headers: { ...getAuthHeaders(), 'Content-Type': 'multipart/form-data' },
            });
            return res.data;
        } catch (e) { return rejectWithValue(extractError(e, 'Failed to edit admin')); }
    }
);

export const editCustomer = createAsyncThunk(
    'userManagement/editCustomer',
    async (payload: EditCustomerPayload, { rejectWithValue }) => {
        try {
            const res = await axios.put(Router.CustomerRouting.Edit, payload, { headers: getAuthHeaders() });
            return res.data;
        } catch (e) { return rejectWithValue(extractError(e, 'Failed to edit customer')); }
    }
);

export const editVendor = createAsyncThunk(
    'userManagement/editVendor',
    async (payload: EditVendorPayload, { rejectWithValue }) => {
        try {
            const formData = new FormData();
            Object.entries(payload).forEach(([k, v]) => { if (v != null) formData.append(k, v instanceof File ? v : String(v)); });
            const res = await axios.put(Router.VendorRouting.Edit, formData, {
                headers: { ...getAuthHeaders(), 'Content-Type': 'multipart/form-data' },
            });
            return res.data;
        } catch (e) { return rejectWithValue(extractError(e, 'Failed to edit merchant')); }
    }
);

// ── Delete thunks ───────────────────────────────────────────────────

export const deleteAdmin = createAsyncThunk(
    'userManagement/deleteAdmin',
    async (id: string, { rejectWithValue }) => {
        try {
            await axios.delete(Router.AdminRouting.Delete.replace('{id}', id), { headers: getAuthHeaders() });
            return id;
        } catch (e) { return rejectWithValue(extractError(e, 'Failed to delete admin')); }
    }
);

export const deleteCustomer = createAsyncThunk(
    'userManagement/deleteCustomer',
    async (id: string, { rejectWithValue }) => {
        try {
            await axios.delete(Router.CustomerRouting.Delete.replace('{id}', id), { headers: getAuthHeaders() });
            return id;
        } catch (e) { return rejectWithValue(extractError(e, 'Failed to delete customer')); }
    }
);

export const deleteVendor = createAsyncThunk(
    'userManagement/deleteVendor',
    async (id: string, { rejectWithValue }) => {
        try {
            await axios.delete(Router.VendorRouting.Delete.replace('{id}', id), { headers: getAuthHeaders() });
            return id;
        } catch (e) { return rejectWithValue(extractError(e, 'Failed to delete merchant')); }
    }
);

// ── Toggle status thunks ────────────────────────────────────────────

export const toggleAdminStatus = createAsyncThunk(
    'userManagement/toggleAdminStatus',
    async (id: string, { rejectWithValue }) => {
        try {
            const res = await axios.post(Router.AdminRouting.ToggleStatus, { id }, { headers: getAuthHeaders() });
            return res.data;
        } catch (e) { return rejectWithValue(extractError(e, 'Failed to toggle status')); }
    }
);

export const toggleCustomerStatus = createAsyncThunk(
    'userManagement/toggleCustomerStatus',
    async (id: string, { rejectWithValue }) => {
        try {
            const res = await axios.post(
                Router.CustomerRouting.ToggleStatus.replace('{id}', id), null, { headers: getAuthHeaders() }
            );
            return res.data;
        } catch (e) { return rejectWithValue(extractError(e, 'Failed to toggle status')); }
    }
);

export const toggleVendorStatus = createAsyncThunk(
    'userManagement/toggleVendorStatus',
    async (id: string, { rejectWithValue }) => {
        try {
            const res = await axios.post(Router.VendorRouting.ToggleStatus, { id }, { headers: getAuthHeaders() });
            return res.data;
        } catch (e) { return rejectWithValue(extractError(e, 'Failed to toggle status')); }
    }
);

// ── State ───────────────────────────────────────────────────────────

interface UserManagementState {
    admins: { data: AdminUser[]; totalCount: number; loading: boolean; error: string | null };
    customers: { data: CustomerUser[]; totalCount: number; loading: boolean; error: string | null };
    vendors: { data: VendorUser[]; totalCount: number; loading: boolean; error: string | null };
    actionLoading: boolean;
    actionError: string | null;
}

const initialState: UserManagementState = {
    admins: { data: [], totalCount: 0, loading: false, error: null },
    customers: { data: [], totalCount: 0, loading: false, error: null },
    vendors: { data: [], totalCount: 0, loading: false, error: null },
    actionLoading: false,
    actionError: null,
};

// ── Slice ───────────────────────────────────────────────────────────

const userManagementSlice = createSlice({
    name: 'userManagement',
    initialState,
    reducers: {
        clearActionError: (state) => { state.actionError = null; },
    },
    extraReducers: (builder) => {
        builder
            // Admins paginated
            .addCase(getAdminsPaginated.pending, (s) => { s.admins.loading = true; s.admins.error = null; })
            .addCase(getAdminsPaginated.fulfilled, (s, a) => {
                s.admins.loading = false;
                s.admins.data = a.payload.data ?? [];
                s.admins.totalCount = a.payload.totalCount ?? a.payload.meta?.totalCount ?? 0;
            })
            .addCase(getAdminsPaginated.rejected, (s, a) => { s.admins.loading = false; s.admins.error = a.payload as string; })

            // Customers paginated
            .addCase(getCustomersPaginated.pending, (s) => { s.customers.loading = true; s.customers.error = null; })
            .addCase(getCustomersPaginated.fulfilled, (s, a) => {
                s.customers.loading = false;
                s.customers.data = a.payload.data ?? [];
                s.customers.totalCount = a.payload.totalCount ?? a.payload.meta?.totalCount ?? 0;
            })
            .addCase(getCustomersPaginated.rejected, (s, a) => { s.customers.loading = false; s.customers.error = a.payload as string; })

            // Vendors paginated
            .addCase(getVendorsPaginated.pending, (s) => { s.vendors.loading = true; s.vendors.error = null; })
            .addCase(getVendorsPaginated.fulfilled, (s, a) => {
                s.vendors.loading = false;
                s.vendors.data = a.payload.data ?? [];
                s.vendors.totalCount = a.payload.totalCount ?? a.payload.meta?.totalCount ?? 0;
            })
            .addCase(getVendorsPaginated.rejected, (s, a) => { s.vendors.loading = false; s.vendors.error = a.payload as string; })

            // Action thunks (create, edit, delete, toggle)
            .addCase(createAdmin.pending, (s) => { s.actionLoading = true; s.actionError = null; })
            .addCase(createAdmin.fulfilled, (s) => { s.actionLoading = false; })
            .addCase(createAdmin.rejected, (s, a) => { s.actionLoading = false; s.actionError = a.payload as string; })

            .addCase(createVendor.pending, (s) => { s.actionLoading = true; s.actionError = null; })
            .addCase(createVendor.fulfilled, (s) => { s.actionLoading = false; })
            .addCase(createVendor.rejected, (s, a) => { s.actionLoading = false; s.actionError = a.payload as string; })

            .addCase(createCustomer.pending, (s) => { s.actionLoading = true; s.actionError = null; })
            .addCase(createCustomer.fulfilled, (s) => { s.actionLoading = false; })
            .addCase(createCustomer.rejected, (s, a) => { s.actionLoading = false; s.actionError = a.payload as string; })

            .addCase(editAdmin.pending, (s) => { s.actionLoading = true; s.actionError = null; })
            .addCase(editAdmin.fulfilled, (s) => { s.actionLoading = false; })
            .addCase(editAdmin.rejected, (s, a) => { s.actionLoading = false; s.actionError = a.payload as string; })

            .addCase(editCustomer.pending, (s) => { s.actionLoading = true; s.actionError = null; })
            .addCase(editCustomer.fulfilled, (s) => { s.actionLoading = false; })
            .addCase(editCustomer.rejected, (s, a) => { s.actionLoading = false; s.actionError = a.payload as string; })

            .addCase(editVendor.pending, (s) => { s.actionLoading = true; s.actionError = null; })
            .addCase(editVendor.fulfilled, (s) => { s.actionLoading = false; })
            .addCase(editVendor.rejected, (s, a) => { s.actionLoading = false; s.actionError = a.payload as string; })

            .addCase(deleteAdmin.pending, (s) => { s.actionLoading = true; s.actionError = null; })
            .addCase(deleteAdmin.fulfilled, (s) => { s.actionLoading = false; })
            .addCase(deleteAdmin.rejected, (s, a) => { s.actionLoading = false; s.actionError = a.payload as string; })

            .addCase(deleteCustomer.pending, (s) => { s.actionLoading = true; s.actionError = null; })
            .addCase(deleteCustomer.fulfilled, (s) => { s.actionLoading = false; })
            .addCase(deleteCustomer.rejected, (s, a) => { s.actionLoading = false; s.actionError = a.payload as string; })

            .addCase(deleteVendor.pending, (s) => { s.actionLoading = true; s.actionError = null; })
            .addCase(deleteVendor.fulfilled, (s) => { s.actionLoading = false; })
            .addCase(deleteVendor.rejected, (s, a) => { s.actionLoading = false; s.actionError = a.payload as string; })

            .addCase(toggleAdminStatus.pending, (s) => { s.actionLoading = true; s.actionError = null; })
            .addCase(toggleAdminStatus.fulfilled, (s) => { s.actionLoading = false; })
            .addCase(toggleAdminStatus.rejected, (s, a) => { s.actionLoading = false; s.actionError = a.payload as string; })

            .addCase(toggleCustomerStatus.pending, (s) => { s.actionLoading = true; s.actionError = null; })
            .addCase(toggleCustomerStatus.fulfilled, (s) => { s.actionLoading = false; })
            .addCase(toggleCustomerStatus.rejected, (s, a) => { s.actionLoading = false; s.actionError = a.payload as string; })

            .addCase(toggleVendorStatus.pending, (s) => { s.actionLoading = true; s.actionError = null; })
            .addCase(toggleVendorStatus.fulfilled, (s) => { s.actionLoading = false; })
            .addCase(toggleVendorStatus.rejected, (s, a) => { s.actionLoading = false; s.actionError = a.payload as string; });
    },
});

export const { clearActionError } = userManagementSlice.actions;
export default userManagementSlice.reducer;
