"use client";

import Image from "next/image";
import { useState, useEffect } from "react";
import type { Admin, Vendor, Customer } from "@/types";
import type { UserTab } from "../types";

interface UserViewModalProps {
  show: boolean;
  user: Admin | Vendor | Customer | null;
  activeTab: UserTab;
  onClose: () => void;
  onViewPermissions?: (user: Admin | Vendor | Customer) => void;
  onViewActivity?: (user: Admin | Vendor | Customer) => void;
}

const DEFAULT_AVATAR = "/assets/images/users/user-dummy-img.jpg";

const getAvatarUrl = (profileImage: unknown): string => {
  if (!profileImage || typeof profileImage !== "string" || !profileImage.trim()) {
    return DEFAULT_AVATAR;
  }
  return profileImage.trim();
};

// Avatar component with error handling using Next.js Image
const AvatarImage = ({ src, alt }: { src: string; alt: string }) => {
  const [imgSrc, setImgSrc] = useState(src);
  const [hasError, setHasError] = useState(false);

  useEffect(() => {
    setImgSrc(src);
    setHasError(false);
  }, [src]);

  // Check if it's an external URL or data URL
  const isExternal = imgSrc.startsWith("http") || imgSrc.startsWith("//") || imgSrc.startsWith("data:");

  // Use default avatar if error occurred
  const finalSrc = hasError ? DEFAULT_AVATAR : imgSrc;

  return (
    <>
      {/* Hidden img to test if image loads successfully */}
      <img
        src={imgSrc}
        alt=""
        className="d-none"
        onError={() => {
          if (!hasError && imgSrc !== DEFAULT_AVATAR) {
            setHasError(true);
          }
        }}
        onLoad={() => {
          if (hasError) {
            setHasError(false);
          }
        }}
      />
      <Image
        src={finalSrc}
        alt={alt}
        width={120}
        height={120}
        className="rounded-circle"
        unoptimized={isExternal || finalSrc === DEFAULT_AVATAR}
      />
    </>
  );
};

export default function UserViewModal({ show, user, activeTab, onClose, onViewPermissions, onViewActivity }: UserViewModalProps) {
  const handleClose = () => {
    document.body.classList.remove("modal-open");
    document.body.style.overflow = "";
    document.body.style.paddingRight = "";
    onClose();
  };

  if (!user) return null;

  const getRoleBadgeClass = (role: string) => {
    if (role === "SuperAdmin") return "badge bg-soft-danger text-danger";
    if (role === "Admin") return "badge bg-soft-primary text-primary";
    if (role === "Merchant") return "badge bg-soft-info text-info";
    return "badge bg-soft-secondary text-secondary";
  };

  const getStatusBadge = () => {
    const isDeleted = Boolean((user as Admin | Vendor | Customer).isDeleted);
    return (
      <span className={isDeleted ? "badge bg-soft-danger text-danger" : "badge bg-soft-success text-success"}>
        {isDeleted ? "Inactive" : "Active"}
      </span>
    );
  };

  return (
    <>
      <div className={`modal fade ${show ? "show" : ""}`} tabIndex={-1} aria-hidden={!show} style={{ display: show ? "block" : "none" }}>
        <div className="modal-dialog modal-dialog-centered modal-lg">
          <div className="modal-content">
            <div className="modal-header">
              <h5 className="modal-title">User Details</h5>
              <button type="button" className="btn-close" aria-label="Close" onClick={handleClose} />
            </div>
            <div className="modal-body">
              <div className="row g-3">
                <div className="col-md-12 text-center mb-3">
                  <AvatarImage src={getAvatarUrl(user.profileImage)} alt="Avatar" />
                </div>
                <div className="col-md-6">
                  <label className="form-label fw-semibold">Full Name</label>
                  <p>{user.fullName || "-"}</p>
                </div>
                <div className="col-md-6">
                  <label className="form-label fw-semibold">Email</label>
                  <p>{user.email || "-"}</p>
                </div>
                <div className="col-md-6">
                  <label className="form-label fw-semibold">Role</label>
                  <p>
                    <span className={getRoleBadgeClass(user.role || (activeTab === "admins" ? "Admin" : activeTab === "merchants" ? "Merchant" : "Customer"))}>
                      {user.role || (activeTab === "admins" ? "Admin" : activeTab === "merchants" ? "Merchant" : "Customer")}
                    </span>
                  </p>
                </div>
                <div className="col-md-6">
                  <label className="form-label fw-semibold">Status</label>
                  <p>{getStatusBadge()}</p>
                </div>
                <div className="col-md-12">
                  <div className="d-flex gap-2">
                    {onViewPermissions && (
                      <button
                        type="button"
                        className="btn btn-soft-info"
                        onClick={() => {
                          onViewPermissions(user);
                          handleClose();
                        }}
                      >
                        <i className="ri-shield-user-line me-1"></i> Manage Permissions
                      </button>
                    )}
                    {onViewActivity && (
                      <button
                        type="button"
                        className="btn btn-soft-primary"
                        onClick={() => {
                          onViewActivity(user);
                          handleClose();
                        }}
                      >
                        <i className="ri-history-line me-1"></i> View Activity Log
                      </button>
                    )}
                  </div>
                </div>
              </div>
            </div>
            <div className="modal-footer">
              <button type="button" className="btn btn-light" onClick={handleClose}>
                Close
              </button>
            </div>
          </div>
        </div>
      </div>
      {show ? (
        <div
          className="modal-backdrop fade show"
          onClick={handleClose}
        />
      ) : null}
    </>
  );
}
