"use client";

import type { UserTab } from "../types";

interface UserActionButtonsProps {
  userId: string;
  activeTab: UserTab;
  onView: (id: string) => void;
  onEdit: (id: string) => void;
  onToggleStatus: (id: string) => void;
  onPermissions: (id: string) => void;
  onEditPermissions: (id: string) => void;
  onActivity: (id: string) => void;
  onDelete: (id: string) => void;
}

export function escapeHtml(value: unknown): string {
  const s = String(value ?? "");
  return s
    .replaceAll("&", "&amp;")
    .replaceAll("<", "&lt;")
    .replaceAll(">", "&gt;")
    .replaceAll('"', "&quot;")
    .replaceAll("'", "&#039;");
}

export default function UserActionButtons({
  userId,
  activeTab,
  onView,
  onEdit,
  onToggleStatus,
  onPermissions,
  onEditPermissions,
  onActivity,
  onDelete,
}: UserActionButtonsProps) {
  const id = escapeHtml(userId);

  return `<div class="dropdown">
    <button type="button" class="btn btn-sm btn-light" data-bs-toggle="dropdown" aria-expanded="false">
      <i class="ri-more-2-fill"></i>
    </button>
    <ul class="dropdown-menu dropdown-menu-end">
      <li>
        <button type="button" class="dropdown-item" data-action="view" data-id="${id}">
          <i class="ri-eye-line me-2"></i>View Details
        </button>
      </li>
      <li>
        <button type="button" class="dropdown-item" data-action="edit" data-id="${id}">
          <i class="ri-pencil-line me-2"></i>Edit
        </button>
      </li>
      <li>
        <button type="button" class="dropdown-item" data-action="toggleStatus" data-id="${id}">
          <i class="ri-toggle-line me-2"></i>Toggle Status
        </button>
      </li>
      <li>
        <button type="button" class="dropdown-item" data-action="permissions" data-id="${id}">
          <i class="ri-shield-user-line me-2"></i>View Permissions
        </button>
      </li>
      <li>
        <button type="button" class="dropdown-item" data-action="editPermissions" data-id="${id}">
          <i class="ri-shield-check-line me-2"></i>Edit Permissions
        </button>
      </li>
      <li>
        <button type="button" class="dropdown-item" data-action="activity" data-id="${id}">
          <i class="ri-history-line me-2"></i>View Activity Log
        </button>
      </li>
      <li><hr class="dropdown-divider" /></li>
      <li>
        <button type="button" class="dropdown-item text-danger" data-action="delete" data-id="${id}">
          <i class="ri-delete-bin-line me-2"></i>Delete
        </button>
      </li>
    </ul>
  </div>`;
}
