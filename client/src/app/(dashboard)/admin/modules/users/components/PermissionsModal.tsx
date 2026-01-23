"use client";

import { useState, useEffect, useCallback } from "react";
import { roleService, type UserClaimsResponse } from "@/app/(dashboard)/shared/modules/api/auth/roleService";
import { useToast } from "@/app/(dashboard)/shared/modules/hooks/useToast";

interface PermissionsModalProps {
  show: boolean;
  userId: string | null;
  userName?: string;
  onClose: () => void;
  onSuccess: () => void;
}

export default function PermissionsModal({ show, userId, userName, onClose, onSuccess }: PermissionsModalProps) {
  const toast = useToast();
  const [loading, setLoading] = useState(false);
  const [saving, setSaving] = useState(false);
  const [userClaims, setUserClaims] = useState<UserClaimsResponse | null>(null);
  const [selectedClaims, setSelectedClaims] = useState<Set<string>>(new Set());

  useEffect(() => {
    if (show && userId) {
      let isMounted = true;
      const loadUserClaims = async () => {
        setLoading(true);
        try {
          const data = await roleService.getUserClaims(userId);
          if (isMounted) {
            setUserClaims(data);
            const current = new Set(data.userClaims.filter((c) => c.value === true).map((c) => c.type));
            setSelectedClaims(current);
          }
        } catch (e) {
          if (isMounted) {
            toast.error(e instanceof Error ? e.message : "Failed to load user permissions");
          }
        } finally {
          if (isMounted) {
            setLoading(false);
          }
        }
      };
      loadUserClaims();
      return () => {
        isMounted = false;
      };
    } else {
      setUserClaims(null);
      setSelectedClaims(new Set());
    }
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [show, userId]);

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
      onSuccess();
      onClose();
    } catch (e) {
      toast.error(e instanceof Error ? e.message : "Failed to update permissions");
    } finally {
      setSaving(false);
    }
  };

  if (!show) return null;

  return (
    <>
      <div className={`modal fade ${show ? "show" : ""}`} tabIndex={-1} aria-hidden={!show} style={{ display: show ? "block" : "none" }}>
        <div className="modal-dialog modal-dialog-centered modal-lg">
          <div className="modal-content">
            <div className="modal-header">
              <h5 className="modal-title">Manage Permissions for {userName || "User"}</h5>
              <button type="button" className="btn-close" aria-label="Close" onClick={onClose} />
            </div>
            <div className="modal-body">
              {loading ? (
                <div className="text-center py-5">
                  <div className="spinner-border text-primary" role="status">
                    <span className="visually-hidden">Loading...</span>
                  </div>
                </div>
              ) : userClaims && userClaims.userClaims.length > 0 ? (
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
            <div className="modal-footer">
              <button type="button" className="btn btn-light" onClick={onClose} disabled={saving}>
                Cancel
              </button>
              <button type="button" className="btn btn-primary" onClick={savePermissions} disabled={saving || loading}>
                {saving ? "Saving..." : "Save Permissions"}
              </button>
            </div>
          </div>
        </div>
      </div>
      {show ? <div className="modal-backdrop fade show" onClick={onClose} /> : null}
    </>
  );
}
