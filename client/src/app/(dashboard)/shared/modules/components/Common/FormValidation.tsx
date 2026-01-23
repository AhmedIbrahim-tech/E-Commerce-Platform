"use client";

import { ReactNode, FormHTMLAttributes } from "react";

interface FormFieldProps {
  label: string;
  name: string;
  type?: string;
  placeholder?: string;
  required?: boolean;
  validFeedback?: string;
  invalidFeedback?: string;
  tooltip?: boolean;
  children?: ReactNode;
  className?: string;
}

export function FormField({
  label,
  name,
  type = "text",
  placeholder,
  required = false,
  validFeedback,
  invalidFeedback,
  tooltip = false,
  children,
  className = "",
}: FormFieldProps) {
  const fieldClasses = tooltip ? "position-relative" : "";

  return (
    <div className={fieldClasses}>
      <label htmlFor={name} className="form-label">
        {label}
      </label>
      {children || (
        <input
          type={type}
          className="form-control"
          id={name}
          name={name}
          placeholder={placeholder}
          required={required}
        />
      )}
      {validFeedback && (
        <div className={tooltip ? "valid-tooltip" : "valid-feedback"}>{validFeedback}</div>
      )}
      {invalidFeedback && (
        <div className={tooltip ? "invalid-tooltip" : "invalid-feedback"}>{invalidFeedback}</div>
      )}
    </div>
  );
}

interface FormValidationProps extends FormHTMLAttributes<HTMLFormElement> {
  children: ReactNode;
  customStyles?: boolean;
  tooltips?: boolean;
  className?: string;
}

export default function FormValidation({
  children,
  customStyles = false,
  tooltips = false,
  className = "",
  ...props
}: FormValidationProps) {
  const formClasses = [
    "needs-validation",
    customStyles && "row g-3",
    className,
  ]
    .filter(Boolean)
    .join(" ");

  return (
    <form className={formClasses} noValidate {...props}>
      {children}
    </form>
  );
}
