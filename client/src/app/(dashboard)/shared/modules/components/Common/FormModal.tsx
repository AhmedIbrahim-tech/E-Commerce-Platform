"use client";

import { type FormEvent, type ReactNode } from "react";

interface FormModalProps {
  show: boolean;
  title: string;
  submitText?: string;
  isSubmitting?: boolean;
  onClose: () => void;
  onSubmit: () => void | Promise<void>;
  children: ReactNode;
}

export default function FormModal({
  show,
  title,
  submitText = "Save",
  isSubmitting = false,
  onClose,
  onSubmit,
  children,
}: FormModalProps) {
  const handleSubmit = async (e: FormEvent) => {
    e.preventDefault();
    await onSubmit();
  };

  return (
    <div
      className={`modal fade ${show ? "show" : ""}`}
      tabIndex={-1}
      aria-hidden={!show}
      style={{ display: show ? "block" : "none" }}
    >
      <div className="modal-dialog modal-dialog-centered">
        <div className="modal-content">
          <div className="modal-header">
            <h5 className="modal-title">{title}</h5>
            <button type="button" className="btn-close" aria-label="Close" onClick={onClose} />
          </div>
          <form onSubmit={handleSubmit}>
            <div className="modal-body">{children}</div>
            <div className="modal-footer">
              <button type="button" className="btn btn-light" onClick={onClose} disabled={isSubmitting}>
                Close
              </button>
              <button type="submit" className="btn btn-primary" disabled={isSubmitting}>
                {isSubmitting ? "Saving..." : submitText}
              </button>
            </div>
          </form>
        </div>
      </div>
      {show ? <div className="modal-backdrop fade show" /> : null}
    </div>
  );
}

