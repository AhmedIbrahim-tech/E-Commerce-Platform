"use client";

import { useEffect, useMemo, useRef, useState } from "react";
import { useAppDispatch, useAppSelector } from "@/store/hooks";
import {
  fetchNotificationsAsync,
  markAllNotificationsReadAsync,
  markNotificationReadAsync,
} from "@/store/slices/notificationSlice";
import type { NotificationItem } from "@/types";

export default function NotificationDropdown() {
  const [isOpen, setIsOpen] = useState(false);
  const [activeTab, setActiveTab] = useState("all");
  const dispatch = useAppDispatch();
  const { items, unreadCount, loading } = useAppSelector((s) => s.notifications);
  const fetchedOnceRef = useRef(false);

  useEffect(() => {
    if (!isOpen) return;
    if (fetchedOnceRef.current) return;
    fetchedOnceRef.current = true;
    dispatch(fetchNotificationsAsync({ pageNumber: 1, pageSize: 10 }));
  }, [dispatch, isOpen]);

  const visibleItems = useMemo(() => {
    if (activeTab === "all") return items;
    return items;
  }, [activeTab, items]);

  const onMarkAllRead = async () => {
    await dispatch(markAllNotificationsReadAsync());
  };

  const onMarkRead = async (id: string) => {
    await dispatch(markNotificationReadAsync(id));
  };

  const renderNotification = (n: NotificationItem) => {
    const { title, description, iconClass } = getNotificationPresentation(n);

    return (
      <div
        key={n.id}
        className={`text-reset notification-item d-block dropdown-item position-relative ${n.isRead ? "" : "active"}`}
        role="button"
        onClick={() => onMarkRead(n.id)}
      >
        <div className="d-flex">
          <div className="avatar-xs me-3 flex-shrink-0">
            <span className="avatar-title bg-info-subtle text-info rounded-circle fs-16">
              <i className={iconClass}></i>
            </span>
          </div>
          <div className="flex-grow-1">
            <div className="stretched-link">
              <h6 className="mt-0 mb-1 fs-13 fw-semibold">{title}</h6>
            </div>
            {description ? (
              <div className="fs-13 text-muted">
                <p className="mb-1">{description}</p>
              </div>
            ) : null}
            <p className="mb-0 fs-11 fw-medium text-uppercase text-muted">
              <span>
                <i className="mdi mdi-clock-outline"></i> {formatTimeAgo(n.createdAt)}
              </span>
            </p>
          </div>
        </div>
      </div>
    );
  };

  return (
    <div className={`dropdown topbar-head-dropdown ms-1 header-item ${isOpen ? "show" : ""}`}>
      <button
        type="button"
        className="btn btn-icon btn-topbar btn-ghost-secondary rounded-circle"
        id="page-header-notifications-dropdown"
        onClick={() => setIsOpen(!isOpen)}
        data-bs-toggle="dropdown"
        aria-haspopup="true"
        aria-expanded={isOpen}
      >
        <i className="bx bx-bell fs-22"></i>
        {unreadCount > 0 ? (
          <span className="position-absolute topbar-badge fs-10 translate-middle badge rounded-pill bg-danger">
            {unreadCount}
            <span className="visually-hidden">unread notifications</span>
          </span>
        ) : null}
      </button>
      <div
        className={`dropdown-menu dropdown-menu-lg dropdown-menu-end p-0 ${isOpen ? "show" : ""}`}
        aria-labelledby="page-header-notifications-dropdown"
      >
        <div className="dropdown-head bg-primary bg-pattern rounded-top">
          <div className="p-3">
            <div className="row align-items-center">
              <div className="col">
                <h6 className="m-0 fs-16 fw-semibold text-white">Notifications</h6>
              </div>
              <div className="col-auto dropdown-tabs">
                <button
                  type="button"
                  className="btn btn-sm btn-light-subtle text-body fs-13"
                  disabled={unreadCount === 0}
                  onClick={onMarkAllRead}
                >
                  Mark all as read
                </button>
              </div>
            </div>
          </div>

          <div className="px-2 pt-2">
            <ul className="nav nav-tabs dropdown-tabs nav-tabs-custom" role="tablist">
              <li className="nav-item waves-effect waves-light">
                <button
                  className={`nav-link ${activeTab === "all" ? "active" : ""}`}
                  onClick={() => setActiveTab("all")}
                  type="button"
                  role="tab"
                >
                  All ({items.length})
                </button>
              </li>
              <li className="nav-item waves-effect waves-light">
                <button
                  className={`nav-link ${activeTab === "messages" ? "active" : ""}`}
                  onClick={() => setActiveTab("messages")}
                  type="button"
                  role="tab"
                  disabled
                >
                  Messages
                </button>
              </li>
              <li className="nav-item waves-effect waves-light">
                <button
                  className={`nav-link ${activeTab === "alerts" ? "active" : ""}`}
                  onClick={() => setActiveTab("alerts")}
                  type="button"
                  role="tab"
                  disabled
                >
                  Alerts
                </button>
              </li>
            </ul>
          </div>
        </div>

        <div className="tab-content" id="notificationItemsTabContent">
          {/* All Notifications Tab */}
          <div
            className={`tab-pane fade py-2 ps-2 ${activeTab === "all" ? "show active" : ""}`}
            role="tabpanel"
          >
            <div style={{ maxHeight: "300px" }} className="pe-2">
              {loading ? (
                <div className="p-3 text-center text-muted">Loading...</div>
              ) : visibleItems.length === 0 ? (
                <div className="p-3 text-center text-muted">No notifications</div>
              ) : (
                visibleItems.map(renderNotification)
              )}

              <div className="my-3 text-center">
                <button type="button" className="btn btn-soft-success waves-effect waves-light" onClick={() => setIsOpen(false)}>
                  Close <i className="ri-close-line align-middle"></i>
                </button>
              </div>
            </div>
          </div>

          {/* Messages Tab */}
          <div
            className={`tab-pane fade py-2 ps-2 ${activeTab === "messages" ? "show active" : ""}`}
            role="tabpanel"
          >
            <div className="p-4 text-center text-muted">Not implemented</div>
          </div>

          {/* Alerts Tab */}
          <div
            className={`tab-pane fade p-4 ${activeTab === "alerts" ? "show active" : ""}`}
            role="tabpanel"
          >
            <div className="p-4 text-center text-muted">Not implemented</div>
          </div>
        </div>
      </div>
    </div>
  );
}

function formatTimeAgo(iso: string): string {
  const d = new Date(iso);
  const diffMs = Date.now() - d.getTime();
  const sec = Math.floor(diffMs / 1000);
  if (sec < 60) return "Just now";
  const min = Math.floor(sec / 60);
  if (min < 60) return `${min} min ago`;
  const hr = Math.floor(min / 60);
  if (hr < 24) return `${hr} hrs ago`;
  const days = Math.floor(hr / 24);
  return `${days} days ago`;
}

function getNotificationPresentation(n: NotificationItem): { title: string; description?: string; iconClass: string } {
  const t = n.type;
  const d = n.data || {};

  switch (t) {
    case "new_user":
      return {
        title: "New user created",
        description: typeof d.userName === "string" ? d.userName : typeof d.userId === "string" ? d.userId : undefined,
        iconClass: "bx bx-user-plus",
      };
    case "new_order":
      return {
        title: "New order created",
        description: typeof d.orderId === "string" ? `Order #${d.orderId}` : undefined,
        iconClass: "bx bx-cart",
      };
    case "order_status_updated":
      return {
        title: "Order status updated",
        description: typeof d.orderId === "string" ? `Order #${d.orderId}` : undefined,
        iconClass: "bx bx-refresh",
      };
    case "merchant_registered":
      return {
        title: "Merchant registered",
        description: typeof d.storeName === "string" ? d.storeName : undefined,
        iconClass: "bx bx-store",
      };
    case "merchant_product_added":
      return {
        title: "Merchant added a product",
        description: typeof d.productName === "string" ? d.productName : undefined,
        iconClass: "bx bx-package",
      };
    case "merchant_order_received":
      return {
        title: "Merchant received an order",
        description: typeof d.orderId === "string" ? `Order #${d.orderId}` : undefined,
        iconClass: "bx bx-bell",
      };
    default:
      return { title: t, iconClass: "bx bx-bell" };
  }
}
