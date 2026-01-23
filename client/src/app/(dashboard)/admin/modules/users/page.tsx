"use client";

import Image from "next/image";
import BreadCrumb from "@/components/Common/BreadCrumb";
import Table, { type TableColumn, type TableAction } from "@/components/Common/Table";
import Tabs, { type TabItem } from "@/components/Common/Tabs";
import DeleteModal from "@/components/Common/DeleteModal";
import ToastContainer from "@/components/Common/ToastContainer";
import { useToast } from "@/app/(dashboard)/shared/modules/hooks/useToast";
import { useAppDispatch, useAppSelector } from "@/app/(dashboard)/shared/modules/store/hooks";
import {
  fetchAdminsAsync,
  fetchAdminByIdAsync,
  deleteAdminAsync,
  toggleAdminStatusAsync,
  setPageNumber as setAdminPageNumber,
  setPageSize as setAdminPageSize,
  setSearch as setAdminSearch,
} from "@/app/(dashboard)/shared/modules/store/slices/adminSlice";
import {
  fetchVendorsAsync,
  fetchVendorByIdAsync,
  deleteVendorAsync,
  toggleVendorStatusAsync,
  setPageNumber as setVendorPageNumber,
  setPageSize as setVendorPageSize,
  setSearch as setVendorSearch,
} from "@/app/(dashboard)/shared/modules/store/slices/vendorSlice";
import {
  fetchCustomersAsync,
  fetchCustomerByIdAsync,
  deleteCustomerAsync,
  toggleCustomerStatusAsync,
  setPageNumber as setCustomerPageNumber,
  setPageSize as setCustomerPageSize,
  setSearch as setCustomerSearch,
} from "@/app/(dashboard)/shared/modules/store/slices/customerSlice";
import UserViewModal from "./components/UserViewModal";
import CreateUserModal from "./components/CreateUserModal";
import EditUserModal from "./components/EditUserModal";
import PermissionsModal from "./components/PermissionsModal";
import ActivityLogModal from "./components/ActivityLogModal";
import type { UserTab, UserItem } from "./types";
import type { Admin, Vendor, Customer } from "@/types";
import styles from "./users.module.scss";
import { useCallback, useEffect, useLayoutEffect, useMemo, useRef, useState } from "react";

const DEFAULT_AVATAR = "/assets/images/users/user-dummy-img.jpg";

const getAvatarUrl = (profileImage: unknown): string => {
  if (!profileImage || typeof profileImage !== "string" || !profileImage.trim()) {
    return DEFAULT_AVATAR;
  }
  return profileImage.trim();
};

// Avatar component with error handling using Next.js Image
const AvatarImage = ({ src, alt }: { src: string; alt: string }) => {
  const [hasError, setHasError] = useState(false);
  const prevSrcRef = useRef(src);
  const errorMapRef = useRef<Map<string, boolean>>(new Map());

  // Reset error state when src changes - using layout effect for synchronous update
  useLayoutEffect(() => {
    if (prevSrcRef.current !== src) {
      prevSrcRef.current = src;
      const cachedError = errorMapRef.current.get(src);
      if (cachedError !== undefined) {
        setHasError(cachedError);
      } else {
        setHasError(false);
        errorMapRef.current.set(src, false);
      }
    }
  }, [src]);

  // Check if it's an external URL or data URL
  const isExternal = src.startsWith("http") || src.startsWith("//") || src.startsWith("data:");

  // Use default avatar if error occurred
  const finalSrc = hasError ? DEFAULT_AVATAR : src;

  return (
    <>
      {/* Hidden img to test if image loads successfully - native img needed for reliable error detection */}
      {/* eslint-disable-next-line @next/next/no-img-element */}
      <img
        src={src}
        alt=""
        className="d-none"
        onError={() => {
          if (src !== DEFAULT_AVATAR) {
            errorMapRef.current.set(src, true);
            setHasError(true);
          }
        }}
        onLoad={() => {
          errorMapRef.current.set(src, false);
          setHasError(false);
        }}
      />
      <Image
        src={finalSrc}
        alt={alt}
        width={32}
        height={32}
        className="rounded-circle avatar-xs me-2"
        unoptimized={isExternal || finalSrc === DEFAULT_AVATAR}
      />
    </>
  );
};

export default function AdminUsersPage() {
  const toast = useToast();
  const dispatch = useAppDispatch();

  const [activeTab, setActiveTab] = useState<UserTab>("admins");
  const [showViewModal, setShowViewModal] = useState(false);
  const [viewingUser, setViewingUser] = useState<UserItem | null>(null);
  const [showDelete, setShowDelete] = useState(false);
  const [showCreateModal, setShowCreateModal] = useState(false);
  const [showEditModal, setShowEditModal] = useState(false);
  const [editingUser, setEditingUser] = useState<UserItem | null>(null);
  const [showPermissionsModal, setShowPermissionsModal] = useState(false);
  const [permissionsUserId, setPermissionsUserId] = useState<string | null>(null);
  const [permissionsUserName, setPermissionsUserName] = useState<string>("");
  const [showActivityModal, setShowActivityModal] = useState(false);
  const [activityUserId, setActivityUserId] = useState<string | null>(null);
  const [activityUserName, setActivityUserName] = useState<string>("");
  const deleteIdRef = useRef<string | null>(null);
  const tableRef = useRef<{ reload: () => void } | null>(null);

  const adminState = useAppSelector((state) => state.admin);
  const vendorState = useAppSelector((state) => state.vendor);
  const customerState = useAppSelector((state) => state.customer);

  const currentState = useMemo(() => {
    if (activeTab === "admins") return adminState;
    if (activeTab === "merchants") return vendorState;
    return customerState;
  }, [activeTab, adminState, vendorState, customerState]);

  // Show toast for errors instead of alert
  useEffect(() => {
    if (currentState.error) {
      toast.error(currentState.error);
    }
  }, [currentState.error, toast]);


  const fetcher = useCallback(
    async (page: number, pageSize: number, searchTerm?: string) => {
      if (activeTab === "admins") {
        dispatch(setAdminPageNumber(page));
        dispatch(setAdminPageSize(pageSize));
        dispatch(setAdminSearch(searchTerm));
        const result = await dispatch(
          fetchAdminsAsync({
            pageNumber: page,
            pageSize,
            search: searchTerm,
          })
        );
        if (fetchAdminsAsync.fulfilled.match(result)) {
          return {
            data: (result.payload.response.data || []) as unknown as UserItem[],
            totalCount: result.payload.response.totalCount || 0,
          };
        }
        return { data: [], totalCount: 0 };
      } else if (activeTab === "merchants") {
        dispatch(setVendorPageNumber(page));
        dispatch(setVendorPageSize(pageSize));
        dispatch(setVendorSearch(searchTerm));
        const result = await dispatch(
          fetchVendorsAsync({
            pageNumber: page,
            pageSize,
            search: searchTerm,
          })
        );
        if (fetchVendorsAsync.fulfilled.match(result)) {
          return {
            data: (result.payload.response.data || []) as unknown as UserItem[],
            totalCount: result.payload.response.totalCount || 0,
          };
        }
        return { data: [], totalCount: 0 };
      } else {
        dispatch(setCustomerPageNumber(page));
        dispatch(setCustomerPageSize(pageSize));
        dispatch(setCustomerSearch(searchTerm));
        const result = await dispatch(
          fetchCustomersAsync({
            pageNumber: page,
            pageSize,
            search: searchTerm,
          })
        );
        if (fetchCustomersAsync.fulfilled.match(result)) {
          return {
            data: (result.payload.response.data || []) as unknown as UserItem[],
            totalCount: result.payload.response.totalCount || 0,
          };
        }
        return { data: [], totalCount: 0 };
      }
    },
    [activeTab, dispatch]
  );

  const columns = useMemo<Array<TableColumn<UserItem>>>(() => {
    return [
      {
        key: "profileImage",
        title: "Avatar",
        render: (v) => {
          const imgUrl = getAvatarUrl(v);
          return (
            <div className="d-flex align-items-center">
              <AvatarImage src={imgUrl} alt="Avatar" />
            </div>
          );
        },
      },
      { key: "fullName", title: "Name", render: (v) => (v ? String(v) : "-") },
      { key: "email", title: "Email", render: (v) => (v ? String(v) : "-") },
      {
        key: "role",
        title: "Role",
        render: (v) => {
          const role = String(v ?? (activeTab === "admins" ? "Admin" : activeTab === "merchants" ? "Merchant" : "Customer"));
          const badgeClass =
            role === "SuperAdmin"
              ? "badge bg-soft-danger text-danger"
              : role === "Admin"
                ? "badge bg-soft-primary text-primary"
                : role === "Merchant"
                  ? "badge bg-soft-info text-info"
                  : "badge bg-soft-secondary text-secondary";
          return <span className={badgeClass}>{role}</span>;
        },
      },
      {
        key: "status",
        title: "Status",
        render: (v, row) => {
          const deleted = Boolean((row as Admin | Vendor | Customer).isDeleted);
          const badge = deleted ? "badge bg-soft-danger text-danger" : "badge bg-soft-success text-success";
          return <span className={badge}>{deleted ? "Inactive" : "Active"}</span>;
        },
      },
      {
        key: "createdAt",
        title: "Created At",
        render: (v) => {
          if (!v) return "-";
          try {
            return new Date(String(v)).toLocaleString();
          } catch {
            return String(v);
          }
        },
      },
    ];
  }, [activeTab]);

  const handleDelete = useCallback((row: UserItem) => {
    deleteIdRef.current = row.id;
    setShowDelete(true);
  }, []);

  const handleView = useCallback(
    async (row: UserItem) => {
      try {
        let result;
        if (activeTab === "admins") {
          result = await dispatch(fetchAdminByIdAsync(row.id));
          if (fetchAdminByIdAsync.fulfilled.match(result)) {
            setViewingUser(result.payload as UserItem);
            setShowViewModal(true);
          } else {
            toast.error(result.payload as string);
          }
        } else if (activeTab === "merchants") {
          result = await dispatch(fetchVendorByIdAsync(row.id));
          if (fetchVendorByIdAsync.fulfilled.match(result)) {
            setViewingUser(result.payload as UserItem);
            setShowViewModal(true);
          } else {
            toast.error(result.payload as string);
          }
        } else {
          result = await dispatch(fetchCustomerByIdAsync(row.id));
          if (fetchCustomerByIdAsync.fulfilled.match(result)) {
            setViewingUser(result.payload as UserItem);
            setShowViewModal(true);
          } else {
            toast.error(result.payload as string);
          }
        }
      } catch (e) {
        toast.error(e instanceof Error ? e.message : "Failed to load user details");
      }
    },
    [toast, activeTab, dispatch]
  );

  const handleToggleStatus = useCallback(
    async (row: UserItem) => {
      try {
        if (activeTab === "admins") {
          const result = await dispatch(toggleAdminStatusAsync(row.id));
          if (toggleAdminStatusAsync.fulfilled.match(result)) {
            toast.success(result.payload.message || "Admin status updated successfully");
            // Update immediately by refetching
            await dispatch(
              fetchAdminsAsync({
                pageNumber: adminState.pageNumber,
                pageSize: adminState.pageSize,
                search: adminState.search,
              })
            );
            // Also trigger table reload
            if (tableRef.current) {
              tableRef.current.reload();
            }
          } else {
            toast.error(result.payload as string);
          }
        } else if (activeTab === "merchants") {
          const result = await dispatch(toggleVendorStatusAsync(row.id));
          if (toggleVendorStatusAsync.fulfilled.match(result)) {
            toast.success(result.payload.message || "Merchant status updated successfully");
            await dispatch(
              fetchVendorsAsync({
                pageNumber: vendorState.pageNumber,
                pageSize: vendorState.pageSize,
                search: vendorState.search,
              })
            );
            if (tableRef.current) {
              tableRef.current.reload();
            }
          } else {
            toast.error(result.payload as string);
          }
        } else if (activeTab === "customers") {
          const result = await dispatch(toggleCustomerStatusAsync(row.id));
          if (toggleCustomerStatusAsync.fulfilled.match(result)) {
            toast.success(result.payload.message || "Customer status updated successfully");
            await dispatch(
              fetchCustomersAsync({
                pageNumber: customerState.pageNumber,
                pageSize: customerState.pageSize,
                search: customerState.search,
              })
            );
            if (tableRef.current) {
              tableRef.current.reload();
            }
          } else {
            toast.error(result.payload as string);
          }
        }
      } catch (e) {
        toast.error(e instanceof Error ? e.message : "Failed to update status");
      }
    },
    [toast, activeTab, dispatch, adminState, vendorState, customerState]
  );

  const handleEdit = useCallback(
    (row: UserItem) => {
      setEditingUser(row);
      setShowEditModal(true);
    },
    []
  );

  const handleCreateSuccess = useCallback(() => {
    // Reload table after create
    if (tableRef.current) {
      tableRef.current.reload();
    }
    // Also refetch from store
    if (activeTab === "admins") {
      dispatch(
        fetchAdminsAsync({
          pageNumber: adminState.pageNumber,
          pageSize: adminState.pageSize,
          search: adminState.search,
        })
      );
    } else if (activeTab === "merchants") {
      dispatch(
        fetchVendorsAsync({
          pageNumber: vendorState.pageNumber,
          pageSize: vendorState.pageSize,
          search: vendorState.search,
        })
      );
    } else {
      dispatch(
        fetchCustomersAsync({
          pageNumber: customerState.pageNumber,
          pageSize: customerState.pageSize,
          search: customerState.search,
        })
      );
    }
  }, [activeTab, dispatch, adminState, vendorState, customerState]);

  const handleEditSuccess = useCallback(() => {
    // Reload table after edit
    if (tableRef.current) {
      tableRef.current.reload();
    }
    // Also refetch from store
    if (activeTab === "admins") {
      dispatch(
        fetchAdminsAsync({
          pageNumber: adminState.pageNumber,
          pageSize: adminState.pageSize,
          search: adminState.search,
        })
      );
    } else if (activeTab === "merchants") {
      dispatch(
        fetchVendorsAsync({
          pageNumber: vendorState.pageNumber,
          pageSize: vendorState.pageSize,
          search: vendorState.search,
        })
      );
    } else {
      dispatch(
        fetchCustomersAsync({
          pageNumber: customerState.pageNumber,
          pageSize: customerState.pageSize,
          search: customerState.search,
        })
      );
    }
  }, [activeTab, dispatch, adminState, vendorState, customerState]);

  const handlePermissions = useCallback((row: UserItem) => {
    setPermissionsUserId(row.id);
    setPermissionsUserName(row.fullName || row.userName || "");
    setShowPermissionsModal(true);
  }, []);

  const handleActivityLog = useCallback((row: UserItem) => {
    setActivityUserId(row.id);
    setActivityUserName(row.fullName || row.userName || "");
    setShowActivityModal(true);
  }, []);

  const handlePermissionsSuccess = useCallback(() => {
    // Permissions update doesn't affect user list, but we can reload if needed
    if (tableRef.current) {
      tableRef.current.reload();
    }
  }, []);

  const actions = useMemo<Array<TableAction<UserItem>>>(
    () => [
      {
        icon: "ri-eye-line",
        variant: "info",
        tooltip: "View Details",
        onClick: handleView,
      },
      {
        icon: "ri-pencil-line",
        variant: "success",
        tooltip: "Edit",
        onClick: handleEdit,
      },
      {
        icon: "ri-toggle-line",
        variant: "warning",
        tooltip: "Toggle Status",
        onClick: handleToggleStatus,
      },
      {
        icon: "ri-shield-user-line",
        variant: "primary",
        tooltip: "Manage Permissions",
        onClick: handlePermissions,
      },
      {
        icon: "ri-history-line",
        variant: "info",
        tooltip: "View Activity Log",
        onClick: handleActivityLog,
      },
      {
        icon: "ri-delete-bin-line",
        variant: "danger",
        danger: true,
        tooltip: "Delete",
        onClick: handleDelete,
      },
    ],
    [handleView, handleEdit, handleDelete, handleToggleStatus, handlePermissions, handleActivityLog]
  );

  const confirmDelete = useCallback(async () => {
    if (!deleteIdRef.current) return;

    try {
      let result;
      if (activeTab === "admins") {
        result = await dispatch(deleteAdminAsync(deleteIdRef.current));
        if (deleteAdminAsync.fulfilled.match(result)) {
          toast.success("User deleted successfully");
          await dispatch(
            fetchAdminsAsync({
              pageNumber: adminState.pageNumber,
              pageSize: adminState.pageSize,
              search: adminState.search,
            })
          );
          if (tableRef.current) {
            tableRef.current.reload();
          }
        } else {
          toast.error(result.payload as string);
        }
      } else if (activeTab === "merchants") {
        result = await dispatch(deleteVendorAsync(deleteIdRef.current));
        if (deleteVendorAsync.fulfilled.match(result)) {
          toast.success("User deleted successfully");
          await dispatch(
            fetchVendorsAsync({
              pageNumber: vendorState.pageNumber,
              pageSize: vendorState.pageSize,
              search: vendorState.search,
            })
          );
          if (tableRef.current) {
            tableRef.current.reload();
          }
        } else {
          toast.error(result.payload as string);
        }
      } else {
        result = await dispatch(deleteCustomerAsync(deleteIdRef.current));
        if (deleteCustomerAsync.fulfilled.match(result)) {
          toast.success("User deleted successfully");
          await dispatch(
            fetchCustomersAsync({
              pageNumber: customerState.pageNumber,
              pageSize: customerState.pageSize,
              search: customerState.search,
            })
          );
          if (tableRef.current) {
            tableRef.current.reload();
          }
        } else {
          toast.error(result.payload as string);
        }
      }
      setShowDelete(false);
      deleteIdRef.current = null;
    } catch (e) {
      toast.error(e instanceof Error ? e.message : "Failed to delete user");
    }
  }, [activeTab, toast, dispatch, adminState, vendorState, customerState]);

  const handleTabChange = useCallback((tabId: string) => {
    const newTab = tabId as UserTab;
    setActiveTab(newTab);
  }, []);

  // Create a component that renders table for the current active tab
  const TableContent = useMemo(() => {
    return (
      <Table<UserItem>
        key={activeTab}
        columns={columns}
        fetcher={fetcher}
        initialPageSize={currentState.pageSize}
        initialSearchTerm={currentState.search}
        searchPlaceholder={`Search ${activeTab}...`}
        actions={actions}
        showActions={true}
        emptyMessage={`No ${activeTab} found`}
        tableId={`users-table-${activeTab}`}
        ref={tableRef as React.Ref<any>}
      />
    );
  }, [activeTab, currentState, columns, fetcher, actions]);

  const tabItems = useMemo<Array<TabItem>>(
    () => [
      {
        id: "admins",
        label: "Admins",
        icon: "ri-shield-user-line",
        content: TableContent,
      },
      {
        id: "merchants",
        label: "Merchants",
        icon: "ri-store-line",
        content: TableContent,
      },
      {
        id: "customers",
        label: "Customers",
        icon: "ri-user-line",
        content: TableContent,
      },
    ],
    [TableContent]
  );

  return (
    <div className={`container-fluid ${styles["users-container"]}`}>
      <BreadCrumb
        items={[
          { label: "Admin", icon: "ri-shield-user-line" },
          { label: "Users", icon: "ri-user-line" },
        ]}
      />
      <ToastContainer toasts={toast.toasts} onRemove={toast.removeToast} />

      <div className={`card ${styles["users-card"]}`}>
        <div className={styles["users-card-header"]}>
          <h5 className={styles["card-title"]}>Users</h5>
          <button
            type="button"
            className="btn btn-primary"
            onClick={() => setShowCreateModal(true)}
          >
            <i className="ri-add-line me-1"></i>
            {activeTab === "admins" ? "Create Admin" : activeTab === "merchants" ? "Create Merchant" : "Create Customer"}
          </button>
        </div>
        <div className={styles["users-card-body"]}>
          <Tabs
            items={tabItems}
            activeId={activeTab}
            onTabChange={handleTabChange}
            colorVariant="primary"
            bgLight={true}
            className="mb-3"
          />
        </div>
      </div>

      <CreateUserModal
        show={showCreateModal}
        activeTab={activeTab}
        onClose={() => setShowCreateModal(false)}
        onSuccess={handleCreateSuccess}
      />

      <EditUserModal
        show={showEditModal}
        user={editingUser}
        activeTab={activeTab}
        onClose={() => {
          setShowEditModal(false);
          setEditingUser(null);
        }}
        onSuccess={handleEditSuccess}
      />

      <PermissionsModal
        show={showPermissionsModal}
        userId={permissionsUserId}
        userName={permissionsUserName}
        onClose={() => {
          setShowPermissionsModal(false);
          setPermissionsUserId(null);
          setPermissionsUserName("");
        }}
        onSuccess={handlePermissionsSuccess}
      />

      <ActivityLogModal
        show={showActivityModal}
        userId={activityUserId}
        userName={activityUserName}
        onClose={() => {
          setShowActivityModal(false);
          setActivityUserId(null);
          setActivityUserName("");
        }}
      />

      <UserViewModal
        show={showViewModal}
        user={viewingUser}
        activeTab={activeTab}
        onClose={() => setShowViewModal(false)}
        onViewPermissions={handlePermissions}
        onViewActivity={handleActivityLog}
      />

      <DeleteModal
        show={showDelete}
        onCloseClick={() => {
          setShowDelete(false);
          deleteIdRef.current = null;
        }}
        onDeleteClick={confirmDelete}
      />
    </div>
  );
}
