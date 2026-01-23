"use client";

import { useState } from "react";
import { useRouter } from "next/navigation";

import { AppRoutes } from "@/constants/routes";
import { useAppDispatch, useAppSelector } from "@/store/hooks";
import { logoutAsync } from "@/store/slices/authSlice";
import ProfileImage from "./ProfileImage";

export default function ProfileDropdown() {
  const router = useRouter();
  const dispatch = useAppDispatch();
  const { user } = useAppSelector((s) => s.auth);
  const [isOpen, setIsOpen] = useState(false);

  const displayName = user?.displayName || user?.userName || "User";
  const roleLabel = user?.role || "Customer";

  return (
    <div className={`dropdown ms-sm-3 header-item topbar-user ${isOpen ? "show" : ""}`}>
      <button
        type="button"
        className="btn"
        id="page-header-user-dropdown"
        onClick={() => setIsOpen(!isOpen)}
        data-bs-toggle="dropdown"
        aria-haspopup="true"
        aria-expanded={isOpen}
      >
        <span className="d-flex align-items-center">
          <ProfileImage
            profileImageUrl={user?.profileImageUrl}
            alt="Header Avatar"
            className="rounded-circle header-profile-user"
            size={32}
          />
          <span className="text-start ms-xl-2">
            <span className="d-none d-xl-inline-block ms-1 fw-medium user-name-text">
              {displayName}
            </span>
            <span className="d-none d-xl-block ms-1 fs-12 text-muted user-name-sub-text">
              {roleLabel}
            </span>
          </span>
        </span>
      </button>
      <div className={`dropdown-menu dropdown-menu-end ${isOpen ? "show" : ""}`}>
        <h6 className="dropdown-header">Welcome {displayName}!</h6>
        <a className="dropdown-item" href="#">
          <i className="mdi mdi-account-circle text-muted fs-16 align-middle me-1"></i>
          <span className="align-middle">Profile</span>
        </a>
        <a className="dropdown-item" href="#">
          <i className="mdi mdi-message-text-outline text-muted fs-16 align-middle me-1"></i>
          <span className="align-middle">Messages</span>
        </a>
        <a className="dropdown-item" href="#">
          <i className="mdi mdi-calendar-check-outline text-muted fs-16 align-middle me-1"></i>
          <span className="align-middle">Taskboard</span>
        </a>
        <a className="dropdown-item" href="#">
          <i className="mdi mdi-lifebuoy text-muted fs-16 align-middle me-1"></i>
          <span className="align-middle">Help</span>
        </a>
        <div className="dropdown-divider"></div>
        <a className="dropdown-item" href="#">
          <i className="mdi mdi-wallet text-muted fs-16 align-middle me-1"></i>
          <span className="align-middle">
            Balance : <b>$5971.67</b>
          </span>
        </a>
        <a className="dropdown-item" href="#">
          <span className="badge bg-success-subtle text-success mt-1 float-end">
            New
          </span>
          <i className="mdi mdi-cog-outline text-muted fs-16 align-middle me-1"></i>
          <span className="align-middle">Settings</span>
        </a>
        <a className="dropdown-item" href="#">
          <i className="mdi mdi-lock text-muted fs-16 align-middle me-1"></i>
          <span className="align-middle">Lock screen</span>
        </a>
        <a
          className="dropdown-item"
          href="#"
          onClick={async (e) => {
            e.preventDefault();
            setIsOpen(false);
            await dispatch(logoutAsync());
            router.replace(AppRoutes.Auth.Login);
          }}
        >
          <i className="mdi mdi-logout text-muted fs-16 align-middle me-1"></i>
          <span className="align-middle">Logout</span>
        </a>
      </div>
    </div>
  );
}
