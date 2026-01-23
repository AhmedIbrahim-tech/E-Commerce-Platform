"use client";

import { ReactNode, useEffect } from "react";

export type OffcanvasPlacement = "start" | "end" | "top" | "bottom";

interface OffcanvasProps {
  show: boolean;
  onClose: () => void;
  title?: string;
  placement?: OffcanvasPlacement;
  children: ReactNode;
  className?: string;
}

export default function Offcanvas({
  show,
  onClose,
  title,
  placement = "end",
  children,
  className = "",
}: OffcanvasProps) {
  useEffect(() => {
    if (show) {
      document.body.style.overflow = "hidden";
    } else {
      document.body.style.overflow = "";
    }
    return () => {
      document.body.style.overflow = "";
    };
  }, [show]);

  if (!show) return null;

  return (
    <>
      <div
        className={`offcanvas offcanvas-${placement} ${show ? "show" : ""} ${className}`}
        tabIndex={-1}
        style={{ visibility: show ? "visible" : "hidden" }}
      >
        {title && (
          <div className="offcanvas-header border-bottom">
            <h5 className="offcanvas-title">{title}</h5>
            <button
              type="button"
              className="btn-close text-reset"
              onClick={onClose}
              aria-label="Close"
            ></button>
          </div>
        )}
        <div className="offcanvas-body">{children}</div>
      </div>
      {show && <div className="modal-backdrop fade show" onClick={onClose}></div>}
    </>
  );
}
