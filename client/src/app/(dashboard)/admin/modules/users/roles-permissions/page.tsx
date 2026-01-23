"use client";

import BreadCrumb from "@/components/Common/BreadCrumb";
import FormModal from "@/components/Common/FormModal";
import DeleteModal from "@/components/Common/DeleteModal";
import ToastContainer from "@/components/Common/ToastContainer";
import { useToast } from "@/app/(dashboard)/shared/modules/hooks/useToast";
import { roleService, type Role } from "@/app/(dashboard)/shared/modules/api/auth/roleService";
import { getPermissionsForRole } from "@/constants/permissions";
import { useCallback, useEffect, useMemo, useRef, useState } from "react";

export default function RolesPermissionsPage() {
  const toast = useToast();
  const [roles, setRoles] = useState<Role[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  const [showForm, setShowForm] = useState(false);
  const [isSubmitting, setIsSubmitting] = useState(false);
  const [editingId, setEditingId] = useState<string | null>(null);
  const [roleName, setRoleName] = useState("");

  const [showDelete, setShowDelete] = useState(false);
  const deleteIdRef = useRef<string | null>(null);

  const [selectedRole, setSelectedRole] = useState<Role | null>(null);
  const [showPermissionsPanel, setShowPermissionsPanel] = useState(false);

  const loadRoles = useCallback(async () => {
    setError(null);
    setLoading(true);
    try {
      const list = await roleService.getAllRoles();
      setRoles(list || []);
    } catch (e) {
      setError(e instanceof Error ? e.message : "Failed to load roles");
    } finally {
      setLoading(false);
    }
  }, []);

  useEffect(() => {
    void loadRoles();
  }, [loadRoles]);

  const openCreate = () => {
    setEditingId(null);
    setRoleName("");
    setShowForm(true);
  };

  const openEdit = (role: Role) => {
    setEditingId(role.roleId);
    setRoleName(role.roleName || "");
    setShowForm(true);
  };

  const viewPermissions = (role: Role) => {
    setSelectedRole(role);
    setShowPermissionsPanel(true);
  };

  const closePermissionsPanel = () => {
    setShowPermissionsPanel(false);
    setSelectedRole(null);
  };

  const rolePermissions = useMemo(() => {
    if (!selectedRole?.roleName) return [];
    return getPermissionsForRole(selectedRole.roleName);
  }, [selectedRole]);

  const submit = async () => {
    if (isSubmitting) return;

    if (!roleName.trim()) {
      toast.error("Role name is required");
      return;
    }

    setIsSubmitting(true);
    try {
      if (editingId) {
        await roleService.updateRole(editingId, roleName.trim());
        toast.success("Role updated successfully");
      } else {
        await roleService.createRole(roleName.trim());
        toast.success("Role created successfully");
      }
      setShowForm(false);
      await loadRoles();
    } catch (e) {
      toast.error(e instanceof Error ? e.message : "Failed to save role");
    } finally {
      setIsSubmitting(false);
    }
  };

  const confirmDelete = async () => {
    if (!deleteIdRef.current) return;

    try {
      await roleService.deleteRole(deleteIdRef.current);
      toast.success("Role deleted successfully");
      setShowDelete(false);
      deleteIdRef.current = null;
      await loadRoles();
    } catch (e) {
      toast.error(e instanceof Error ? e.message : "Failed to delete role");
    }
  };

  return (
    <div className="container-fluid">
      <BreadCrumb
        items={[
          { label: "Admin", icon: "ri-shield-user-line" },
          { label: "Roles & Permissions", icon: "ri-shield-keyhole-line" },
        ]}
      />
      <ToastContainer toasts={toast.toasts} onRemove={toast.removeToast} />

      {error ? (
        <div className="alert alert-danger" role="alert">
          {error}
        </div>
      ) : null}

      <div className="card">
        <div className="card-header d-flex align-items-center justify-content-between">
          <h5 className="card-title mb-0">Roles</h5>
          <button type="button" className="btn btn-success" onClick={openCreate}>
            <i className="ri-add-line align-bottom me-1"></i> Add Role
          </button>
        </div>
        <div className="card-body">
          {loading ? (
            <div className="text-center py-4">
              <div className="spinner-border text-primary" role="status">
                <span className="visually-hidden">Loading...</span>
              </div>
            </div>
          ) : (
            <div className="table-responsive">
              <table className="table table-hover align-middle mb-0">
                <thead className="table-light">
                  <tr>
                    <th>Role Name</th>
                    <th>Actions</th>
                  </tr>
                </thead>
                <tbody>
                  {roles.length === 0 ? (
                    <tr>
                      <td colSpan={2} className="text-center text-muted py-4">
                        No roles found
                      </td>
                    </tr>
                  ) : (
                    roles.map((role) => (
                      <tr key={role.roleId}>
                        <td>
                          <span className="badge bg-soft-primary text-primary">{role.roleName}</span>
                        </td>
                        <td>
                          <div className="d-flex gap-2">
                            <button type="button" className="btn btn-sm btn-soft-info" onClick={() => viewPermissions(role)} title="View Permissions">
                              <i className="ri-eye-line"></i>
                            </button>
                            <button type="button" className="btn btn-sm btn-soft-primary" onClick={() => openEdit(role)} title="Edit">
                              <i className="ri-pencil-line"></i>
                            </button>
                            <button
                              type="button"
                              className="btn btn-sm btn-soft-danger"
                              onClick={() => {
                                deleteIdRef.current = role.roleId;
                                setShowDelete(true);
                              }}
                              title="Delete"
                            >
                              <i className="ri-delete-bin-line"></i>
                            </button>
                          </div>
                        </td>
                      </tr>
                    ))
                  )}
                </tbody>
              </table>
            </div>
          )}
        </div>
      </div>

      <FormModal show={showForm} title={editingId ? "Edit Role" : "Add Role"} submitText={editingId ? "Update" : "Create"} isSubmitting={isSubmitting} onClose={() => setShowForm(false)} onSubmit={submit}>
        <div className="mb-3">
          <label className="form-label">Role Name*</label>
          <input className="form-control" value={roleName} onChange={(e) => setRoleName(e.target.value)} required />
        </div>
      </FormModal>

      <DeleteModal
        show={showDelete}
        onCloseClick={() => {
          setShowDelete(false);
          deleteIdRef.current = null;
        }}
        onDeleteClick={confirmDelete}
      />

      {/* Permissions Panel */}
      {showPermissionsPanel && selectedRole ? (
        <div className="modal fade show" tabIndex={-1} style={{ display: "block" }}>
          <div className="modal-dialog modal-dialog-centered modal-lg">
            <div className="modal-content">
              <div className="modal-header">
                <h5 className="modal-title">Permissions for {selectedRole.roleName}</h5>
                <button
                  type="button"
                  className="btn-close"
                  aria-label="Close"
                  onClick={() => {
                    closePermissionsPanel();
                    document.body.classList.remove("modal-open");
                    document.body.style.overflow = "";
                    document.body.style.paddingRight = "";
                  }}
                />
              </div>
              <div className="modal-body">
                {rolePermissions.length > 0 ? (
                  <div className="row g-2">
                    {rolePermissions.map((permission) => (
                      <div key={permission} className="col-md-6">
                        <div className="d-flex align-items-center p-2 border rounded">
                          <i className="ri-checkbox-circle-fill text-success me-2"></i>
                          <span className="small">{permission}</span>
                        </div>
                      </div>
                    ))}
                  </div>
                ) : (
                  <div className="text-center text-muted py-4">No default permissions assigned to this role</div>
                )}
              </div>
              <div className="modal-footer">
                <button
                  type="button"
                  className="btn btn-light"
                  onClick={() => {
                    closePermissionsPanel();
                    document.body.classList.remove("modal-open");
                    document.body.style.overflow = "";
                    document.body.style.paddingRight = "";
                  }}
                >
                  Close
                </button>
              </div>
            </div>
          </div>
          <div
            className="modal-backdrop fade show"
            onClick={() => {
              closePermissionsPanel();
              document.body.classList.remove("modal-open");
              document.body.style.overflow = "";
              document.body.style.paddingRight = "";
            }}
          />
        </div>
      ) : null}
    </div>
  );
}
