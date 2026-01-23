"use client";

import { ButtonHTMLAttributes, ReactNode } from "react";

export type ButtonVariant =
  | "primary"
  | "secondary"
  | "success"
  | "danger"
  | "warning"
  | "info"
  | "light"
  | "dark"
  | "outline-primary"
  | "outline-secondary"
  | "outline-success"
  | "outline-danger"
  | "outline-warning"
  | "outline-info"
  | "outline-light"
  | "outline-dark";

export type ButtonSize = "sm" | "md" | "lg";

interface ButtonProps extends ButtonHTMLAttributes<HTMLButtonElement> {
  variant?: ButtonVariant;
  size?: ButtonSize;
  label?: boolean;
  labelIcon?: string;
  labelPosition?: "left" | "right";
  icon?: string;
  iconOnly?: boolean;
  loading?: boolean;
  rounded?: boolean;
  children?: ReactNode;
}

export default function Button({
  variant = "primary",
  size,
  label = false,
  labelIcon,
  labelPosition = "left",
  icon,
  iconOnly = false,
  loading = false,
  rounded = false,
  children,
  className = "",
  disabled,
  ...props
}: ButtonProps) {
  const buttonClasses = [
    "btn",
    `btn-${variant}`,
    size && `btn-${size}`,
    label && "btn-label",
    label && labelPosition === "right" && "right",
    rounded && "rounded-pill",
    loading && "btn-load",
    iconOnly && "btn-icon",
    className,
  ]
    .filter(Boolean)
    .join(" ");

  if (iconOnly) {
    return (
      <button className={buttonClasses} disabled={disabled || loading} {...props}>
        {icon && <i className={icon}></i>}
      </button>
    );
  }

  if (loading) {
    return (
      <button className={buttonClasses} disabled={disabled || loading} {...props}>
        <span className="d-flex align-items-center">
          <span className="spinner-border flex-shrink-0" role="status">
            <span className="visually-hidden">Loading...</span>
          </span>
          <span className="flex-grow-1 ms-2">Loading...</span>
        </span>
      </button>
    );
  }

  if (label) {
    return (
      <button className={buttonClasses} disabled={disabled} {...props}>
        {labelIcon && labelPosition === "left" && (
          <i className={`${labelIcon} label-icon align-middle fs-16 me-2`}></i>
        )}
        {children}
        {labelIcon && labelPosition === "right" && (
          <i className={`${labelIcon} label-icon align-middle fs-16 ms-2`}></i>
        )}
      </button>
    );
  }

  return (
    <button className={buttonClasses} disabled={disabled} {...props}>
      {icon && <i className={icon}></i>}
      {children}
    </button>
  );
}
