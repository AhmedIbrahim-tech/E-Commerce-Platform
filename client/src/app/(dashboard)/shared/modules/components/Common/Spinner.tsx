"use client";

export type SpinnerVariant = "primary" | "secondary" | "success" | "danger" | "warning" | "info" | "light" | "dark";
export type SpinnerType = "border" | "grow";

interface SpinnerProps {
  variant?: SpinnerVariant;
  type?: SpinnerType;
  size?: "sm" | "";
  className?: string;
}

export default function Spinner({
  variant = "primary",
  type = "border",
  size,
  className = "",
}: SpinnerProps) {
  const spinnerClasses = [
    `spinner-${type}`,
    `text-${variant}`,
    size && `spinner-${type}-${size}`,
    className,
  ]
    .filter(Boolean)
    .join(" ");

  return (
    <div className={spinnerClasses} role="status">
      <span className="visually-hidden">Loading...</span>
    </div>
  );
}
