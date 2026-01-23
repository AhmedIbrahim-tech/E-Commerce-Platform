"use client";

import { ReactNode, useEffect } from "react";

export type ModalSize = "sm" | "lg" | "xl" | "";

interface ModalProps {
  show: boolean;
  onClose: () => void;
  title?: string;
  size?: ModalSize;
  centered?: boolean;
  scrollable?: boolean;
  children: ReactNode;
  footer?: ReactNode;
  className?: string;
}

export default function Modal({
  show,
  onClose,
  title,
  size = "",
  centered = false,
  scrollable = false,
  children,
  footer,
  className = "",
}: ModalProps) {
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

  const dialogClasses = [
    "modal-dialog",
    size && `modal-${size}`,
    centered && "modal-dialog-centered",
    scrollable && "modal-dialog-scrollable",
    className,
  ]
    .filter(Boolean)
    .join(" ");

  return (
    <>
      <div
        className={`modal fade ${show ? "show" : ""}`}
        tabIndex={-1}
        style={{ display: show ? "block" : "none" }}
        onClick={onClose}
      >
        <div className={dialogClasses} onClick={(e) => e.stopPropagation()}>
          <div className="modal-content">
            {title && (
              <div className="modal-header">
                <h5 className="modal-title">{title}</h5>
                <button
                  type="button"
                  className="btn-close"
                  onClick={onClose}
                  aria-label="Close"
                ></button>
              </div>
            )}
            <div className="modal-body">{children}</div>
            {footer && <div className="modal-footer">{footer}</div>}
          </div>
        </div>
      </div>
      {show && <div className="modal-backdrop fade show" onClick={onClose}></div>}
    </>
  );
}
