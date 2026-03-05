'use client';

export const dynamic = 'force-dynamic';

import React, { useState, useEffect } from 'react';
import Box from '@mui/material/Box';
import Button from '@mui/material/Button';
import Card from '@mui/material/Card';
import CardContent from '@mui/material/CardContent';
import Chip from '@mui/material/Chip';
import Checkbox from '@mui/material/Checkbox';
import Dialog from '@mui/material/Dialog';
import DialogTitle from '@mui/material/DialogTitle';
import DialogContent from '@mui/material/DialogContent';
import DialogActions from '@mui/material/DialogActions';
import TextField from '@mui/material/TextField';
import Stack from '@mui/material/Stack';
import Typography from '@mui/material/Typography';
import List from '@mui/material/List';
import ListItem from '@mui/material/ListItem';
import ListItemText from '@mui/material/ListItemText';
import ListItemSecondaryAction from '@mui/material/ListItemSecondaryAction';
import IconButton from '@mui/material/IconButton';
import Tooltip from '@mui/material/Tooltip';
import CircularProgress from '@mui/material/CircularProgress';
import Divider from '@mui/material/Divider';
import { IconPlus, IconEdit, IconTrash } from '@tabler/icons-react';
import { useAppDispatch, useAppSelector } from '@/hooks/useRedux';
import SharedTabs, { TabPanel } from '@/components/ui-components/SharedTabs';
import { useNotify } from '@/context/NotifyContext';
import {
    getRoles,
    createRole,
    editRole,
    deleteRole,
    manageUserClaims,
    updateUserClaims,
    setSelectedRole,
} from '@/store/slices/authorizationSlice';
import type { UserClaimItem } from '@/types/users/authorization';

// ── Page component ──────────────────────────────────────────────────

export default function RolesPermissionsPage() {
    const dispatch = useAppDispatch();
    const { notify } = useNotify();
    const { roles, rolesLoading, userClaims, userClaimsLoading, actionLoading } = useAppSelector((s) => s.authorization);

    // Tab state
    const [tab, setTab] = useState(() => {
        if (typeof window !== 'undefined') return localStorage.getItem('rolesPermissionsTab') || 'roles';
        return 'roles';
    });
    const handleTabChange = (v: string) => {
        setTab(v);
        if (typeof window !== 'undefined') localStorage.setItem('rolesPermissionsTab', v);
    };

    // Dialogs
    const [createOpen, setCreateOpen] = useState(false);
    const [editOpen, setEditOpen] = useState(false);
    const [roleName, setRoleName] = useState('');
    const [editingRole, setEditingRole] = useState<{ id: string; name: string } | null>(null);

    // Permissions state
    const [selectedRoleId, setSelectedRoleId] = useState<string | null>(null);
    const [localClaims, setLocalClaims] = useState<UserClaimItem[]>([]);

    // Fetch roles on mount
    useEffect(() => { dispatch(getRoles()); }, [dispatch]);

    // When a role is selected for permissions, fetch claims
    useEffect(() => {
        if (selectedRoleId) {
            dispatch(manageUserClaims(selectedRoleId));
        }
    }, [selectedRoleId, dispatch]);

    // Sync local claims when loaded
    useEffect(() => {
        if (userClaims?.claims) setLocalClaims([...userClaims.claims]);
    }, [userClaims]);

    // ── Roles handlers ──────────────────────────────────────────────

    const handleCreateRole = async () => {
        if (!roleName.trim()) return;
        const result = await dispatch(createRole(roleName.trim()));
        if (result.meta.requestStatus === 'fulfilled') {
            notify('Role created successfully', 'success');
            setCreateOpen(false);
            setRoleName('');
            dispatch(getRoles());
        } else {
            notify(typeof result.payload === 'string' ? result.payload : 'Failed to create role', 'error');
        }
    };

    const handleEditRole = async () => {
        if (!editingRole || !roleName.trim()) return;
        const result = await dispatch(editRole({ id: editingRole.id, name: roleName.trim() }));
        if (result.meta.requestStatus === 'fulfilled') {
            notify('Role updated', 'success');
            setEditOpen(false);
            setEditingRole(null);
            setRoleName('');
            dispatch(getRoles());
        } else {
            notify(typeof result.payload === 'string' ? result.payload : 'Failed to update role', 'error');
        }
    };

    const handleDeleteRole = async (id: string) => {
        const result = await dispatch(deleteRole(id));
        if (result.meta.requestStatus === 'fulfilled') {
            notify('Role deleted', 'success');
        } else {
            notify(typeof result.payload === 'string' ? result.payload : 'Failed to delete role', 'error');
        }
    };

    const openEdit = (role: { id: string; name: string }) => {
        setEditingRole(role);
        setRoleName(role.name);
        setEditOpen(true);
    };

    // ── Permissions handlers ────────────────────────────────────────

    const toggleClaim = (index: number) => {
        setLocalClaims((prev) => {
            const next = [...prev];
            next[index] = { ...next[index], value: !next[index].value };
            return next;
        });
    };

    const handleSaveClaims = async () => {
        if (!selectedRoleId) return;
        const result = await dispatch(updateUserClaims({ userId: selectedRoleId, claims: localClaims }));
        if (result.meta.requestStatus === 'fulfilled') {
            notify('Permissions updated', 'success');
        } else {
            notify(typeof result.payload === 'string' ? result.payload : 'Failed to update permissions', 'error');
        }
    };

    return (
        <Box>
            <Typography variant="h4" mb={3}>Roles & Permissions</Typography>

            <SharedTabs
                tabs={[
                    { label: 'Roles', value: 'roles' },
                    { label: 'Permissions', value: 'permissions' },
                ]}
                value={tab}
                onChange={handleTabChange}
            >
                {/* ── Roles Tab ──────────────────────────────────────── */}
                <TabPanel value="roles">
                    <Box display="flex" justifyContent="flex-end" mb={2}>
                        <Button variant="contained" startIcon={<IconPlus size={16} />} onClick={() => { setRoleName(''); setCreateOpen(true); }}>
                            Add Role
                        </Button>
                    </Box>

                    {rolesLoading ? (
                        <Box display="flex" justifyContent="center" py={6}><CircularProgress /></Box>
                    ) : roles.length === 0 ? (
                        <Typography color="text.secondary" textAlign="center" py={4}>No roles found</Typography>
                    ) : (
                        <Card variant="outlined">
                            <List disablePadding>
                                {roles.map((role, idx) => (
                                    <React.Fragment key={role.id}>
                                        {idx > 0 && <Divider />}
                                        <ListItem sx={{ py: 1.5 }}>
                                            <ListItemText
                                                primary={
                                                    <Stack direction="row" alignItems="center" spacing={1}>
                                                        <Typography fontWeight={600}>{role.name}</Typography>
                                                        <Chip label="Role" size="small" variant="outlined" color="primary" />
                                                    </Stack>
                                                }
                                            />
                                            <ListItemSecondaryAction>
                                                <Tooltip title="Edit">
                                                    <IconButton size="small" color="primary" onClick={() => openEdit(role)}>
                                                        <IconEdit size={16} />
                                                    </IconButton>
                                                </Tooltip>
                                                <Tooltip title="Delete">
                                                    <IconButton size="small" color="error" onClick={() => handleDeleteRole(role.id)}>
                                                        <IconTrash size={16} />
                                                    </IconButton>
                                                </Tooltip>
                                            </ListItemSecondaryAction>
                                        </ListItem>
                                    </React.Fragment>
                                ))}
                            </List>
                        </Card>
                    )}
                </TabPanel>

                {/* ── Permissions Tab ────────────────────────────────── */}
                <TabPanel value="permissions">
                    <Stack direction={{ xs: 'column', md: 'row' }} spacing={3}>
                        {/* Role selector */}
                        <Card variant="outlined" sx={{ minWidth: 220, maxHeight: 500, overflowY: 'auto' }}>
                            <CardContent>
                                <Typography variant="subtitle2" fontWeight={600} mb={1.5}>Select Role</Typography>
                                {rolesLoading ? <CircularProgress size={20} /> : (
                                    <List disablePadding>
                                        {roles.map((role) => (
                                            <ListItem
                                                key={role.id}
                                                component="div"
                                                onClick={() => {
                                                    setSelectedRoleId(role.id);
                                                    dispatch(setSelectedRole(role));
                                                }}
                                                sx={{
                                                    cursor: 'pointer',
                                                    borderRadius: 1,
                                                    mb: 0.5,
                                                    bgcolor: selectedRoleId === role.id ? 'primary.light' : 'transparent',
                                                    '&:hover': { bgcolor: selectedRoleId === role.id ? 'primary.light' : 'grey.100' },
                                                }}
                                            >
                                                <ListItemText primary={role.name} />
                                            </ListItem>
                                        ))}
                                    </List>
                                )}
                            </CardContent>
                        </Card>

                        {/* Claims editor */}
                        <Card variant="outlined" sx={{ flex: 1 }}>
                            <CardContent>
                                {!selectedRoleId ? (
                                    <Typography color="text.secondary" textAlign="center" py={4}>
                                        Select a role to view and edit its permissions
                                    </Typography>
                                ) : userClaimsLoading ? (
                                    <Box display="flex" justifyContent="center" py={4}><CircularProgress /></Box>
                                ) : (
                                    <>
                                        <Box display="flex" justifyContent="space-between" alignItems="center" mb={2}>
                                            <Typography variant="h6" fontWeight={600}>
                                                Permissions for: {roles.find((r) => r.id === selectedRoleId)?.name}
                                            </Typography>
                                            <Button variant="contained" onClick={handleSaveClaims} disabled={actionLoading} size="small">
                                                {actionLoading ? 'Saving...' : 'Save Changes'}
                                            </Button>
                                        </Box>
                                        {localClaims.length === 0 ? (
                                            <Typography color="text.secondary">No permissions found</Typography>
                                        ) : (
                                            <List disablePadding>
                                                {localClaims.map((claim, idx) => (
                                                    <React.Fragment key={claim.type}>
                                                        {idx > 0 && <Divider />}
                                                        <ListItem sx={{ py: 0.5 }}>
                                                            <Checkbox
                                                                checked={claim.value}
                                                                onChange={() => toggleClaim(idx)}
                                                                size="small"
                                                            />
                                                            <ListItemText
                                                                primary={claim.type}
                                                                primaryTypographyProps={{ fontSize: 14 }}
                                                            />
                                                        </ListItem>
                                                    </React.Fragment>
                                                ))}
                                            </List>
                                        )}
                                    </>
                                )}
                            </CardContent>
                        </Card>
                    </Stack>
                </TabPanel>
            </SharedTabs>

            {/* Create Role Dialog */}
            <Dialog open={createOpen} onClose={() => setCreateOpen(false)} maxWidth="xs" fullWidth>
                <DialogTitle>Create Role</DialogTitle>
                <DialogContent>
                    <TextField
                        autoFocus
                        fullWidth
                        label="Role Name"
                        value={roleName}
                        onChange={(e) => setRoleName(e.target.value)}
                        sx={{ mt: 1 }}
                    />
                </DialogContent>
                <DialogActions>
                    <Button onClick={() => setCreateOpen(false)}>Cancel</Button>
                    <Button variant="contained" onClick={handleCreateRole} disabled={actionLoading}>
                        {actionLoading ? 'Creating...' : 'Create'}
                    </Button>
                </DialogActions>
            </Dialog>

            {/* Edit Role Dialog */}
            <Dialog open={editOpen} onClose={() => setEditOpen(false)} maxWidth="xs" fullWidth>
                <DialogTitle>Edit Role</DialogTitle>
                <DialogContent>
                    <TextField
                        autoFocus
                        fullWidth
                        label="Role Name"
                        value={roleName}
                        onChange={(e) => setRoleName(e.target.value)}
                        sx={{ mt: 1 }}
                    />
                </DialogContent>
                <DialogActions>
                    <Button onClick={() => setEditOpen(false)}>Cancel</Button>
                    <Button variant="contained" onClick={handleEditRole} disabled={actionLoading}>
                        {actionLoading ? 'Saving...' : 'Save'}
                    </Button>
                </DialogActions>
            </Dialog>
        </Box>
    );
}
