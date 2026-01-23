"use client";

import BreadCrumb from "@/components/Common/BreadCrumb";
import CommonTable, { type CommonTableColumn } from "@/components/Common/CommonTable";
import ToastContainer from "@/components/Common/ToastContainer";
import { useToast } from "@/app/(dashboard)/shared/modules/hooks/useToast";
import { useListTable } from "@/app/(dashboard)/shared/modules/hooks/useListTable";
import { auditLogService, type AuditLogItem } from "@/app/(dashboard)/shared/modules/api/auth/auditLogService";
import ActivityFilters from "../components/ActivityFilters";
import styles from "../users.module.scss";
import { useCallback, useMemo, useState } from "react";

export default function ActivityLogPage() {
  const toast = useToast();

  // Filters
  const [userFilter, setUserFilter] = useState<string>("");
  const [roleFilter, setRoleFilter] = useState<string>("All");
  const [eventTypeFilter, setEventTypeFilter] = useState<string>("All");
  const [startDate, setStartDate] = useState<string>("");
  const [endDate, setEndDate] = useState<string>("");

  const fetcher = useCallback(
    async (page: number, pageSize: number, searchTerm?: string) => {
      const response = await auditLogService.getAuditLogPaginatedList({
        pageNumber: page,
        pageSize,
        search: searchTerm || userFilter || undefined,
        eventType: eventTypeFilter !== "All" ? eventTypeFilter : undefined,
        startDate: startDate || undefined,
        endDate: endDate || undefined,
        sortBy: 0,
      });
      return {
        data: (response?.data || []) as unknown as AuditLogItem[],
        totalCount: response?.totalCount || 0,
      };
    },
    [userFilter, eventTypeFilter, startDate, endDate]
  );

  const listTable = useListTable({
    fetcher,
    initialPageSize: 10,
  });

  const columns = useMemo<Array<CommonTableColumn<AuditLogItem>>>(() => {
    return [
      { key: "userEmail", title: "User", render: (v) => (v ? String(v) : "-") },
      {
        key: "additionalData",
        title: "Role",
        render: (v) => {
          try {
            if (v && typeof v === "string") {
              const data = JSON.parse(v);
              return data.role || "-";
            }
          } catch {
            // Ignore parse errors
          }
          return "-";
        },
      },
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
      { key: "eventType", title: "Target", render: (v) => (v ? String(v) : "-") },
      {
        key: "createdTime",
        title: "Date",
        render: (v) => {
          if (!v) return "-";
          try {
            return new Date(String(v)).toLocaleString();
          } catch {
            return String(v);
          }
        },
      },
    ];
  }, []);

  return (
    <div className={`container-fluid ${styles["users-container"]}`}>
      <BreadCrumb
        items={[
          { label: "Admin", icon: "ri-shield-user-line" },
          { label: "Activity Log", icon: "ri-file-list-3-line" },
        ]}
      />
      <ToastContainer toasts={toast.toasts} onRemove={toast.removeToast} />

      {listTable.error ? (
        <div className={`alert alert-danger ${styles["users-error-alert"]}`} role="alert">
          {listTable.error}
        </div>
      ) : null}

      <div className={`card ${styles["users-card"]} ${styles["activity-log-page"]}`}>
        <div className={styles["users-card-header"]}>
          <h5 className={styles["card-title"]}>Activity Log</h5>
          <div className={styles["users-filters"]}>
            <input
              type="text"
              className="form-control form-control-sm"
              placeholder="Search user..."
              style={{ width: "200px" }}
              value={userFilter}
              onChange={(e) => {
                setUserFilter(e.target.value);
                listTable.setCurrentPage(1);
                listTable.reload();
              }}
            />
            <ActivityFilters
              eventTypeFilter={eventTypeFilter}
              startDate={startDate}
              endDate={endDate}
              onEventTypeChange={(value) => {
                setEventTypeFilter(value);
                listTable.setCurrentPage(1);
                listTable.reload();
              }}
              onStartDateChange={(value) => {
                setStartDate(value);
                listTable.setCurrentPage(1);
                listTable.reload();
              }}
              onEndDateChange={(value) => {
                setEndDate(value);
                listTable.setCurrentPage(1);
                listTable.reload();
              }}
            />
          </div>
        </div>
        <div className={styles["users-card-body"]}>
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
      </div>
    </div>
  );
}
