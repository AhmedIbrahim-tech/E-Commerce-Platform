"use client";

import { useState, useCallback, useEffect, useMemo, useRef } from "react";
import { auditLogService, type AuditLogItem } from "@/app/(dashboard)/shared/modules/api/auth/auditLogService";
import { userManagementService } from "@/app/(dashboard)/shared/modules/api/users/userManagementService";
import { useListTable } from "@/app/(dashboard)/shared/modules/hooks/useListTable";
import CommonTable, { type CommonTableColumn } from "@/components/Common/CommonTable";
import { useToast } from "@/app/(dashboard)/shared/modules/hooks/useToast";

interface ActivityLogModalProps {
  show: boolean;
  userId: string | null;
  userName?: string;
  onClose: () => void;
}

export default function ActivityLogModal({ show, userId, userName, onClose }: ActivityLogModalProps) {
  const toast = useToast();
  const [userDisplayName, setUserDisplayName] = useState<string>(userName || "User");

  // Filters
  const [eventTypeFilter, setEventTypeFilter] = useState<string>("All");
  const [startDate, setStartDate] = useState<string>("");
  const [endDate, setEndDate] = useState<string>("");

  useEffect(() => {
    if (show && userId && !userName) {
      let isMounted = true;
      userManagementService
        .getUserById(userId)
        .then((user) => {
          if (isMounted) {
            setUserDisplayName(user.fullName || user.userName || "User");
          }
        })
        .catch(() => {
          if (isMounted) {
            setUserDisplayName("User");
          }
        });
      return () => {
        isMounted = false;
      };
    } else if (userName) {
      setUserDisplayName(userName);
    }
  }, [show, userId, userName]);

  const fetcher = useCallback(
    async (page: number, pageSize: number, searchTerm?: string) => {
      if (!userId || !show) {
        return { data: [], totalCount: 0 };
      }

      try {
        const response = await auditLogService.getAuditLogPaginatedList({
          pageNumber: page,
          pageSize,
          search: searchTerm || undefined,
          userId: userId,
          eventType: eventTypeFilter !== "All" ? eventTypeFilter : undefined,
          startDate: startDate || undefined,
          endDate: endDate || undefined,
          sortBy: 0,
        });
        return {
          data: (response?.data || []) as unknown as AuditLogItem[],
          totalCount: response?.totalCount || 0,
        };
      } catch (error) {
        toast.error(error instanceof Error ? error.message : "Failed to load activity log");
        return { data: [], totalCount: 0 };
      }
    },
    [userId, eventTypeFilter, startDate, endDate, show]
  );

  const listTable = useListTable({
    fetcher,
    initialPageSize: 10,
    autoFetch: show && !!userId,
  });

  // Store reload function in ref to avoid dependency issues
  const reloadRef = useRef(listTable.reload);
  reloadRef.current = listTable.reload;

  // Reload when filters change (but not when listTable changes)
  useEffect(() => {
    if (show && userId) {
      reloadRef.current();
    }
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [eventTypeFilter, startDate, endDate]);

  const columns = useMemo<Array<CommonTableColumn<AuditLogItem>>>(() => {
    return [
      {
        key: "eventName",
        title: "Action Type",
        render: (v) => {
          const action = String(v ?? "");
          const badgeClass =
            action.toLowerCase().includes("create") || action.toLowerCase().includes("add")
              ? "badge bg-soft-success text-success"
              : action.toLowerCase().includes("update") || action.toLowerCase().includes("edit")
                ? "badge bg-soft-primary text-primary"
                : action.toLowerCase().includes("delete") || action.toLowerCase().includes("remove")
                  ? "badge bg-soft-danger text-danger"
                  : "badge bg-soft-info text-info";
          return <span className={badgeClass}>{action}</span>;
        },
      },
      { key: "eventType", title: "Target Entity", render: (v) => (v ? String(v) : "-") },
      {
        key: "createdTime",
        title: "Date & Time",
        render: (v) => {
          if (!v) return "-";
          try {
            return new Date(String(v)).toLocaleString();
          } catch {
            return String(v);
          }
        },
      },
      { key: "description", title: "Description", render: (v) => (v ? String(v) : "-") },
    ];
  }, []);

  if (!show) return null;

  return (
    <>
      <div className={`modal fade ${show ? "show" : ""}`} tabIndex={-1} aria-hidden={!show} style={{ display: show ? "block" : "none" }}>
        <div className="modal-dialog modal-dialog-centered modal-xl">
          <div className="modal-content">
            <div className="modal-header">
              <h5 className="modal-title">Activity Log for {userDisplayName}</h5>
              <button type="button" className="btn-close" aria-label="Close" onClick={onClose} />
            </div>
            <div className="modal-body">
              <div className="row g-2 mb-3">
                <div className="col-md-4">
                  <label className="form-label">Event Type</label>
                  <select
                    className="form-select"
                    value={eventTypeFilter}
                    onChange={(e) => {
                      setEventTypeFilter(e.target.value);
                      listTable.setCurrentPage(1);
                    }}
                  >
                    <option value="All">All</option>
                    <option value="Create">Create</option>
                    <option value="Update">Update</option>
                    <option value="Delete">Delete</option>
                    <option value="Login">Login</option>
                    <option value="Logout">Logout</option>
                  </select>
                </div>
                <div className="col-md-4">
                  <label className="form-label">Start Date</label>
                  <input
                    type="date"
                    className="form-control"
                    value={startDate}
                    onChange={(e) => {
                      setStartDate(e.target.value);
                      listTable.setCurrentPage(1);
                    }}
                  />
                </div>
                <div className="col-md-4">
                  <label className="form-label">End Date</label>
                  <input
                    type="date"
                    className="form-control"
                    value={endDate}
                    onChange={(e) => {
                      setEndDate(e.target.value);
                      listTable.setCurrentPage(1);
                    }}
                  />
                </div>
              </div>

              {listTable.error ? (
                <div className="alert alert-danger" role="alert">
                  {listTable.error}
                </div>
              ) : null}

              <CommonTable<AuditLogItem>
                columns={columns}
                data={listTable.data}
                loading={listTable.loading}
                searchable
                searchTerm={listTable.searchTerm}
                onSearchChange={listTable.setSearchTerm}
                searchPlaceholder="Search activity logs..."
                pagination={{
                  currentPage: listTable.currentPage,
                  pageSize: listTable.pageSize,
                  total: listTable.totalCount,
                  onPageChange: listTable.setCurrentPage,
                  onPageSizeChange: listTable.setPageSize,
                }}
                showActions={false}
                emptyMessage="No activity logs found"
              />
            </div>
            <div className="modal-footer">
              <button type="button" className="btn btn-light" onClick={onClose}>
                Close
              </button>
            </div>
          </div>
        </div>
      </div>
      {show ? <div className="modal-backdrop fade show" onClick={onClose} /> : null}
    </>
  );
}
