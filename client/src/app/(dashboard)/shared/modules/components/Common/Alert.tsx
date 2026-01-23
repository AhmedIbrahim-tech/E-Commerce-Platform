"use client";

import { ReactNode } from "react";

export type AlertVariant = "primary" | "secondary" | "success" | "danger" | "warning" | "info" | "light" | "dark";

interface AlertProps {
  variant: AlertVariant;
  label: string;
  message: string;
  icon?: string;
  dismissible?: boolean;
  onDismiss?: () => void;
  className?: string;
}

export default function Alert({
  variant,
  label,
  message,
  icon,
  dismissible = false,
  onDismiss,
  className = "",
}: AlertProps) {
  const alertClasses = `alert alert-${variant} alert-dismissible alert-label-icon label-arrow fade show ${className}`;

  return (
    <div className={alertClasses} role="alert">
      {icon && <i className={`${icon} label-icon`}></i>}
      <strong>{label}</strong> - {message}
      {dismissible && (
        <button
          type="button"
          className="btn-close"
          data-bs-dismiss="alert"
          aria-label="Close"
          onClick={onDismiss}
        ></button>
      )}
    </div>
  );
}
