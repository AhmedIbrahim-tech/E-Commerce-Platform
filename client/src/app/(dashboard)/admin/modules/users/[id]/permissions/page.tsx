"use client";

import BreadCrumb from "@/components/Common/BreadCrumb";
import ToastContainer from "@/components/Common/ToastContainer";
import { useToast } from "@/app/(dashboard)/shared/modules/hooks/useToast";
import { roleService, type UserClaimsResponse } from "@/app/(dashboard)/shared/modules/api/auth/roleService";
import styles from "../../users.module.scss";
import { useCallback, useEffect, useState } from "react";
import { useParams, useRouter } from "next/navigation";

export default function UserPermissionsPage() {
  const params = useParams();
  const router = useRouter();
  const toast = useToast();
  const userId = (params?.id as string) || "";

  const [loading, setLoading] = useState(true);
  const [saving, setSaving] = useState(false);
  const [userClaims, setUserClaims] = useState<UserClaimsResponse | null>(null);
  const [selectedClaims, setSelectedClaims] = useState<Set<string>>(new Set());

  const loadUserClaims = useCallback(async () => {
    if (!userId) return;

    setLoading(true);
    try {
      const data = await roleService.getUserClaims(userId);
      setUserClaims(data);
      // Initialize selected claims from user's current claims
      const current = new Set(data.userClaims.filter((c) => c.value === true).map((c) => c.type));
      setSelectedClaims(current);
    } catch (e) {
      toast.error(e instanceof Error ? e.message : "Failed to load user permissions");
    } finally {
      setLoading(false);
    }
  }, [userId, toast]);

  useEffect(() => {
    if (!userId) {
      router.push("/admin/modules/users");
      return;
    }
    void loadUserClaims();
  }, [loadUserClaims, userId, router]);

  const toggleClaim = (claimType: string) => {
    setSelectedClaims((prev) => {
      const next = new Set(prev);
      if (next.has(claimType)) {
        next.delete(claimType);
      } else {
        next.add(claimType);
      }
      return next;
    });
  };

  const savePermissions = async () => {
    if (!userId || !userClaims) return;

    setSaving(true);
    try {
      const claimsToUpdate = userClaims.userClaims.map((claim) => ({
        type: claim.type,
        value: selectedClaims.has(claim.type),
      }));

      await roleService.updateUserClaims({
        userId,
        userClaims: claimsToUpdate,
      });

      toast.success("Permissions updated successfully");
      router.back();
    } catch (e) {
      toast.error(e instanceof Error ? e.message : "Failed to update permissions");
    } finally {
      setSaving(false);
    }
  };

  if (loading) {
    return (
      <div className={`container-fluid ${styles["users-container"]}`}>
        <BreadCrumb
          items={[
            { label: "Admin", icon: "ri-shield-user-line" },
            { label: "User Permissions", icon: "ri-user-settings-line" },
          ]}
        />
        <div className={styles["users-loading"]}>
          <div className="spinner-border text-primary" role="status">
            <span className="visually-hidden">Loading...</span>
          </div>
        </div>
      </div>
    );
  }

  return (
    <div className="container-fluid">
      <BreadCrumb
        items={[
          { label: "Admin", icon: "ri-shield-user-line" },
          { label: "User Permissions", icon: "ri-user-settings-line" },
        ]}
      />
      <ToastContainer toasts={toast.toasts} onRemove={toast.removeToast} />

      <div className="card">
        <div className="card-header d-flex align-items-center justify-content-between">
          <h5 className="card-title mb-0">Manage Permissions for {userClaims?.userName || "User"}</h5>
          <div className="d-flex gap-2">
            <button type="button" className="btn btn-light" onClick={() => router.back()}>
              Cancel
            </button>
            <button type="button" className="btn btn-primary" onClick={savePermissions} disabled={saving}>
              {saving ? "Saving..." : "Save Permissions"}
            </button>
          </div>
        </div>
        <div className="card-body">
          {userClaims && userClaims.userClaims.length > 0 ? (
            <div className="row g-3">
              {userClaims.userClaims.map((claim) => {
                const isSelected = selectedClaims.has(claim.type);
                return (
                  <div key={claim.type} className="col-md-6 col-lg-4">
                    <div className="form-check form-switch form-switch-lg">
                      <input
                        className="form-check-input"
                        type="checkbox"
                        id={`claim-${claim.type}`}
                        checked={isSelected}
                        onChange={() => toggleClaim(claim.type)}
                      />
                      <label className="form-check-label" htmlFor={`claim-${claim.type}`}>
                        {claim.type.replace(/_/g, " ").replace(/\b\w/g, (l) => l.toUpperCase())}
                      </label>
                    </div>
                  </div>
                );
              })}
            </div>
          ) : (
            <div className="text-center text-muted py-4">No permissions available</div>
          )}
        </div>
      </div>
    </div>
  );
}
