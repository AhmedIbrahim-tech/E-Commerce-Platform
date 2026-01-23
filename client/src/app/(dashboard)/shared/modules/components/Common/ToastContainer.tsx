"use client";

import { useEffect, useRef } from "react";

export type ToastType = "success" | "error" | "warning" | "info";

export interface Toast {
  id: string;
  message: string;
  type: ToastType;
  duration?: number;
}

interface ToastContainerProps {
  toasts: Toast[];
  onRemove: (id: string) => void;
}

export default function ToastContainer({ toasts, onRemove }: ToastContainerProps) {
  const timeoutRefs = useRef<Map<string, NodeJS.Timeout>>(new Map());

  useEffect(() => {
    toasts.forEach((toast) => {
      if (toast.duration && toast.duration > 0) {
        const timeoutId = setTimeout(() => {
          onRemove(toast.id);
          timeoutRefs.current.delete(toast.id);
        }, toast.duration);

        timeoutRefs.current.set(toast.id, timeoutId);
      }
    });

    return () => {
      timeoutRefs.current.forEach((timeoutId) => clearTimeout(timeoutId));
      timeoutRefs.current.clear();
    };
  }, [toasts, onRemove]);

  if (toasts.length === 0) return null;

  const getToastClass = (type: ToastType) => {
    switch (type) {
      case "success":
        return "bg-success";
      case "error":
        return "bg-danger";
      case "warning":
        return "bg-warning";
      case "info":
        return "bg-info";
      default:
        return "bg-primary";
    }
  };

  const getIcon = (type: ToastType) => {
    switch (type) {
      case "success":
        return "ri-checkbox-circle-fill";
      case "error":
        return "ri-error-warning-fill";
      case "warning":
        return "ri-alert-fill";
      case "info":
        return "ri-information-fill";
      default:
        return "ri-notification-fill";
    }
  };

  return (
    <div
      className="toast-container position-fixed top-0 end-0 p-3"
      style={{ zIndex: 1055, marginTop: "70px", pointerEvents: "none" }}
    >
      {toasts.map((toast) => (
        <div
          key={toast.id}
          className={`toast show align-items-center text-white ${getToastClass(toast.type)} border-0 mb-2`}
          role="alert"
          aria-live="assertive"
          aria-atomic="true"
          style={{ pointerEvents: "auto" }}
        >
          <div className="d-flex">
            <div className="toast-body d-flex align-items-center">
              <i className={`${getIcon(toast.type)} me-2 fs-18`}></i>
              <span>{toast.message}</span>
            </div>
            <button
              type="button"
              className="btn-close btn-close-white me-2 m-auto"
              aria-label="Close"
              onClick={() => onRemove(toast.id)}
            ></button>
          </div>
        </div>
      ))}
    </div>
  );
}
