"use client";

import { Spinner } from 'react-bootstrap';
import { ShoppingBag } from 'lucide-react';

/** Page-level loading spinner (Tajerly branded). */
export function LoadingSpinner() {
  return (
    <div className="position-relative" style={{ width: 64, height: 64 }}>
      <Spinner
        animation="border"
        className="position-absolute"
        style={{ width: 64, height: 64, color: 'var(--tj-accent)', borderWidth: 2 }}
      />
      <div className="position-absolute d-flex align-items-center justify-content-center w-100 h-100" style={{ color: 'var(--tj-accent)' }}>
        <ShoppingBag size={20} />
      </div>
    </div>
  );
}
