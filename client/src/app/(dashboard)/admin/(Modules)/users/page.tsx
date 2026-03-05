'use client';

export const dynamic = 'force-dynamic';

import React, { useState, useEffect, useCallback } from 'react';
import Box from '@mui/material/Box';
import Button from '@mui/material/Button';
import Chip from '@mui/material/Chip';
import Dialog from '@mui/material/Dialog';
import DialogTitle from '@mui/material/DialogTitle';
import DialogContent from '@mui/material/DialogContent';
import DialogActions from '@mui/material/DialogActions';
import TextField from '@mui/material/TextField';
import Stack from '@mui/material/Stack';
import Typography from '@mui/material/Typography';
import Avatar from '@mui/material/Avatar';
import IconButton from '@mui/material/IconButton';
import Tooltip from '@mui/material/Tooltip';
import { IconPlus, IconEdit, IconTrash, IconToggleLeft, IconToggleRight } from '@tabler/icons-react';
import { useAppDispatch, useAppSelector } from '@/hooks/useRedux';
import SharedTabs, { TabPanel } from '@/components/ui-components/SharedTabs';
import TableList from '@/components/shared/table-list/TableList';
import { useNotify } from '@/context/NotifyContext';
import type { TableListColumn } from '@/types/table/tableList';
import type {
    AdminUser,
    CustomerUser,
    VendorUser,
    UserTab,
    PaginatedRequest,
} from '@/types/users/userManagement';
import {
    getAdminsPaginated,
    getCustomersPaginated,
    getVendorsPaginated,
    createAdmin,
    createVendor,
    createCustomer,
    deleteAdmin,
    deleteCustomer,
    deleteVendor,
    toggleAdminStatus,
    toggleCustomerStatus,
    toggleVendorStatus,
} from '@/store/slices/userManagementSlice';

// ── Column definitions ──────────────────────────────────────────────

const commonColumns: TableListColumn<AdminUser | CustomerUser | VendorUser>[] = [
    {
        id: 'profileImage',
        label: '',
        render: (row) => (
            <Avatar src={row.profileImage} alt={row.firstName} sx={{ width: 32, height: 32 }}>
                {row.firstName?.[0]}
            </Avatar>
        ),
    },
    { id: 'firstName', label: 'First Name' },
    { id: 'lastName', label: 'Last Name' },
    { id: 'userName', label: 'Username' },
    { id: 'email', label: 'Email' },
    { id: 'phoneNumber', label: 'Phone' },
    {
        id: 'isActive',
        label: 'Status',
        render: (row) => (
            <Chip
                label={row.isActive ? 'Active' : 'Inactive'}
                color={row.isActive ? 'success' : 'error'}
                size="small"
                variant="outlined"
            />
        ),
    },
];

// ── Page component ──────────────────────────────────────────────────

export default function UsersPage() {
    const dispatch = useAppDispatch();
    const { notify } = useNotify();
    const { admins, customers, vendors, actionLoading } = useAppSelector((s) => s.userManagement);

    // Tab state persisted in localStorage
    const [tab, setTab] = useState<UserTab>(() => {
        if (typeof window !== 'undefined') {
            return (localStorage.getItem('usersTab') as UserTab) || 'admins';
        }
        return 'admins';
    });

    const handleTabChange = (v: string) => {
        setTab(v as UserTab);
        if (typeof window !== 'undefined') localStorage.setItem('usersTab', v);
    };

    // Pagination
    const [adminPage, setAdminPage] = useState(0);
    const [customerPage, setCustomerPage] = useState(0);
    const [vendorPage, setVendorPage] = useState(0);
    const [rowsPerPage, setRowsPerPage] = useState(10);
    const [search, setSearch] = useState('');

    // Create dialog
    const [createOpen, setCreateOpen] = useState(false);
    const [form, setForm] = useState({ firstName: '', lastName: '', userName: '', email: '', password: '', confirmPassword: '', phoneNumber: '' });

    const resetForm = () => setForm({ firstName: '', lastName: '', userName: '', email: '', password: '', confirmPassword: '', phoneNumber: '' });

    // Fetch
    const fetchTab = useCallback((currentTab: UserTab, page: number) => {
        const req: PaginatedRequest = { pageNumber: page + 1, pageSize: rowsPerPage, search };
        if (currentTab === 'admins') dispatch(getAdminsPaginated(req));
        else if (currentTab === 'customers') dispatch(getCustomersPaginated(req));
        else dispatch(getVendorsPaginated(req));
    }, [dispatch, rowsPerPage, search]);

    useEffect(() => {
        const page = tab === 'admins' ? adminPage : tab === 'customers' ? customerPage : vendorPage;
        fetchTab(tab, page);
    }, [tab, adminPage, customerPage, vendorPage, rowsPerPage, search, fetchTab]);

    // Actions
    const handleCreate = async () => {
        let result;
        if (tab === 'admins') result = await dispatch(createAdmin(form));
        else if (tab === 'merchants') result = await dispatch(createVendor(form));
        else result = await dispatch(createCustomer(form));

        if (result.meta.requestStatus === 'fulfilled') {
            notify('User created successfully', 'success');
            setCreateOpen(false);
            resetForm();
            fetchTab(tab, 0);
        } else {
            notify(typeof result.payload === 'string' ? result.payload : 'Failed to create user', 'error');
        }
    };

    const handleDelete = async (id: string) => {
        let result;
        if (tab === 'admins') result = await dispatch(deleteAdmin(id));
        else if (tab === 'merchants') result = await dispatch(deleteVendor(id));
        else result = await dispatch(deleteCustomer(id));

        if (result.meta.requestStatus === 'fulfilled') {
            notify('User deleted', 'success');
            fetchTab(tab, 0);
        } else {
            notify(typeof result.payload === 'string' ? result.payload : 'Failed to delete user', 'error');
        }
    };

    const handleToggle = async (id: string) => {
        let result;
        if (tab === 'admins') result = await dispatch(toggleAdminStatus(id));
        else if (tab === 'merchants') result = await dispatch(toggleVendorStatus(id));
        else result = await dispatch(toggleCustomerStatus(id));

        if (result.meta.requestStatus === 'fulfilled') {
            notify('Status updated', 'success');
            const page = tab === 'admins' ? adminPage : tab === 'customers' ? customerPage : vendorPage;
            fetchTab(tab, page);
        } else {
            notify(typeof result.payload === 'string' ? result.payload : 'Failed to toggle status', 'error');
        }
    };

    const renderActions = (row: AdminUser | CustomerUser | VendorUser) => (
        <Stack direction="row" spacing={0.5}>
            <Tooltip title="Edit">
                <IconButton size="small" color="primary">
                    <IconEdit size={16} />
                </IconButton>
            </Tooltip>
            <Tooltip title={row.isActive ? 'Deactivate' : 'Activate'}>
                <IconButton size="small" color={row.isActive ? 'warning' : 'success'} onClick={() => handleToggle(row.id)}>
                    {row.isActive ? <IconToggleRight size={16} /> : <IconToggleLeft size={16} />}
                </IconButton>
            </Tooltip>
            <Tooltip title="Delete">
                <IconButton size="small" color="error" onClick={() => handleDelete(row.id)}>
                    <IconTrash size={16} />
                </IconButton>
            </Tooltip>
        </Stack>
    );

    const tabLabel = tab === 'admins' ? 'Admin' : tab === 'merchants' ? 'Merchant' : 'Customer';

    const renderTable = (
        data: (AdminUser | CustomerUser | VendorUser)[],
        totalCount: number,
        loading: boolean,
        page: number,
        setPage: (p: number) => void,
    ) => (
        <TableList
            title=""
            columns={commonColumns}
            rows={data}
            getRowId={(r) => r.id}
            loading={loading}
            searchValue={search}
            onSearchChange={setSearch}
            searchPlaceholder={`Search ${tabLabel}s...`}
            renderActions={renderActions}
            pagination={{
                page,
                rowsPerPage,
                totalRows: totalCount,
                onPageChange: setPage,
                onRowsPerPageChange: (rpp) => { setRowsPerPage(rpp); setPage(0); },
            }}
        />
    );

    return (
        <Box>
            {/* Header */}
            <Box display="flex" justifyContent="space-between" alignItems="center" mb={3}>
                <Typography variant="h4">Users Management</Typography>
                <Button
                    variant="contained"
                    startIcon={<IconPlus size={16} />}
                    onClick={() => { resetForm(); setCreateOpen(true); }}
                >
                    Create {tabLabel}
                </Button>
            </Box>

            {/* Tabs */}
            <SharedTabs
                tabs={[
                    { label: 'Admins', value: 'admins' },
                    { label: 'Merchants', value: 'merchants' },
                    { label: 'Customers', value: 'customers' },
                ]}
                value={tab}
                onChange={handleTabChange}
            >
                <TabPanel value="admins">
                    {renderTable(admins.data, admins.totalCount, admins.loading, adminPage, setAdminPage)}
                </TabPanel>
                <TabPanel value="merchants">
                    {renderTable(vendors.data, vendors.totalCount, vendors.loading, vendorPage, setVendorPage)}
                </TabPanel>
                <TabPanel value="customers">
                    {renderTable(customers.data, customers.totalCount, customers.loading, customerPage, setCustomerPage)}
                </TabPanel>
            </SharedTabs>

            {/* Create Dialog */}
            <Dialog open={createOpen} onClose={() => setCreateOpen(false)} maxWidth="sm" fullWidth>
                <DialogTitle>Create {tabLabel}</DialogTitle>
                <DialogContent>
                    <Stack spacing={2} mt={1}>
                        <TextField label="First Name" fullWidth value={form.firstName} onChange={(e) => setForm({ ...form, firstName: e.target.value })} />
                        <TextField label="Last Name" fullWidth value={form.lastName} onChange={(e) => setForm({ ...form, lastName: e.target.value })} />
                        <TextField label="Username" fullWidth value={form.userName} onChange={(e) => setForm({ ...form, userName: e.target.value })} />
                        <TextField label="Email" fullWidth value={form.email} onChange={(e) => setForm({ ...form, email: e.target.value })} />
                        <TextField label="Phone" fullWidth value={form.phoneNumber} onChange={(e) => setForm({ ...form, phoneNumber: e.target.value })} />
                        <TextField label="Password" type="password" fullWidth value={form.password} onChange={(e) => setForm({ ...form, password: e.target.value })} />
                        <TextField label="Confirm Password" type="password" fullWidth value={form.confirmPassword} onChange={(e) => setForm({ ...form, confirmPassword: e.target.value })} />
                    </Stack>
                </DialogContent>
                <DialogActions>
                    <Button onClick={() => setCreateOpen(false)}>Cancel</Button>
                    <Button variant="contained" onClick={handleCreate} disabled={actionLoading}>
                        {actionLoading ? 'Creating...' : 'Create'}
                    </Button>
                </DialogActions>
            </Dialog>
        </Box>
    );
}
