"use client";

import React, { useCallback } from "react";

export interface CommonTableColumn<RowT> {
  key: keyof RowT | string;
  title: string;
  render?: (value: unknown, row: RowT) => React.ReactNode;
  className?: string;
  sortable?: boolean;
}

export interface CommonTableProps<RowT extends object> {
  /** Columns definition */
  columns: Array<CommonTableColumn<RowT>>;
  /** Data rows */
  data: RowT[];
  /** Loading state */
  loading?: boolean;
  /** Search functionality */
  searchable?: boolean;
  /** Search placeholder */
  searchPlaceholder?: string;
  /** Current search term (controlled) */
  searchTerm?: string;
  /** Callback when search term changes */
  onSearchChange?: (searchTerm: string) => void;
  /** Pagination */
  pagination?: {
    currentPage: number;
    pageSize: number;
    total: number;
    onPageChange: (page: number) => void;
    onPageSizeChange?: (size: number) => void;
  };
  /** Action handlers */
  onAdd?: () => void;
  /** Show action column */
  showActions?: boolean;
  /** Custom action renderer */
  renderActions?: (row: RowT) => React.ReactNode;
  /** Empty state message */
  emptyMessage?: string;
  /** Table ID for unique identification */
  tableId?: string;
}

export default function CommonTable<RowT extends object>({
  columns,
  data,
  loading = false,
  searchable = true,
  searchPlaceholder = "Search...",
  searchTerm = "",
  onSearchChange,
  pagination,
  onAdd,
  showActions = true,
  renderActions,
  emptyMessage = "No data available",
  tableId = "common-table",
}: CommonTableProps<RowT>) {
  const totalPages = pagination ? Math.ceil(pagination.total / pagination.pageSize) : 1;

  const handlePageChange = useCallback(
    (page: number) => {
      if (pagination && page >= 1 && page <= totalPages) {
        pagination.onPageChange(page);
      }
    },
    [pagination, totalPages]
  );

  const getCellValue = useCallback(
    (row: RowT, column: CommonTableColumn<RowT>) => {
      const value = row[column.key as keyof RowT];
      if (column.render) {
        return column.render(value, row);
      }
      return value != null ? String(value) : "-";
    },
    []
  );

  const hasData = data.length > 0;
  const displayTotal = pagination ? pagination.total : data.length;

  const actionColSpan = showActions ? 1 : 0;
  const totalCols = columns.length + actionColSpan;

  return (
    <div id={tableId}>
      <div className="row g-4 mb-3">
        {onAdd && (
          <div className="col-sm-auto">
            <div>
              <button type="button" className="btn btn-success" onClick={onAdd}>
                <i className="ri-add-line align-bottom me-1"></i> Add
              </button>
            </div>
          </div>
        )}
        {searchable && (
          <div className="col-sm">
            <div className="d-flex justify-content-sm-end">
              <div className="search-box ms-2">
                <input
                  type="text"
                  className="form-control search"
                  placeholder={searchPlaceholder}
                  value={searchTerm}
                  onChange={(e) => {
                    onSearchChange?.(e.target.value);
                  }}
                />
                <i className="ri-search-line search-icon"></i>
              </div>
            </div>
          </div>
        )}
      </div>

      <div className="table-responsive table-card mt-3 mb-1">
        <table className="table align-middle table-nowrap">
          <thead className="table-light">
            <tr>
              {columns.map((col, idx) => (
                <th key={idx} className={col.className || ""} scope="col">
                  {col.title}
                </th>
              ))}
              {showActions && <th scope="col">Action</th>}
            </tr>
          </thead>
          <tbody>
            {loading ? (
              <tr>
                <td colSpan={totalCols} className="text-center py-4">
                  <div className="spinner-border text-primary" role="status">
                    <span className="visually-hidden">Loading...</span>
                  </div>
                </td>
              </tr>
            ) : hasData ? (
              data.map((row, rowIdx) => (
                <tr key={rowIdx}>
                  {columns.map((col, colIdx) => (
                    <td key={colIdx} className={col.className || ""}>
                      {getCellValue(row, col)}
                    </td>
                  ))}
                  {showActions && (
                    <td>
                      {renderActions ? renderActions(row) : null}
                    </td>
                  )}
                </tr>
              ))
            ) : (
              <tr>
                <td colSpan={totalCols} className="text-center py-4">
                  <div className="noresult">
                    <div className="text-center">
                      {/* @ts-ignore - lord-icon is a custom web component */}
                      {/* eslint-disable-next-line @typescript-eslint/ban-ts-comment */}
                      {/* @ts-ignore */}
                      {React.createElement(
                        "lord-icon",
                        {
                          src: "https://cdn.lordicon.com/msoeawqm.json",
                          trigger: "loop",
                          colors: "primary:#121331,secondary:#08a88a",
                          style: { width: "75px", height: "75px" },
                        } as any
                      )}
                      <h5 className="mt-2">Sorry! No Result Found</h5>
                      <p className="text-muted mb-0">{emptyMessage}</p>
                    </div>
                  </div>
                </td>
              </tr>
            )}
          </tbody>
        </table>
      </div>

      {pagination && totalPages > 1 && (
        <div className="d-flex justify-content-end">
          <div className="pagination-wrap hstack gap-2">
            <button
              type="button"
              className="page-item pagination-prev btn btn-sm btn-soft-secondary"
              disabled={pagination.currentPage <= 1}
              onClick={() => handlePageChange(pagination.currentPage - 1)}
            >
              Previous
            </button>
            <div className="d-flex gap-1">
              {Array.from({ length: totalPages }, (_, i) => i + 1).map((page) => (
                <button
                  key={page}
                  type="button"
                  className={`page-item btn btn-sm ${
                    page === pagination.currentPage
                      ? "btn-primary"
                      : "btn-soft-secondary"
                  }`}
                  onClick={() => handlePageChange(page)}
                >
                  {page}
                </button>
              ))}
            </div>
            <button
              type="button"
              className="page-item pagination-next btn btn-sm btn-soft-secondary"
              disabled={pagination.currentPage >= totalPages}
              onClick={() => handlePageChange(pagination.currentPage + 1)}
            >
              Next
            </button>
          </div>
        </div>
      )}

      {pagination && (
        <div className="d-flex justify-content-between align-items-center mt-3">
          <div className="text-muted">
            Showing {((pagination.currentPage - 1) * pagination.pageSize) + 1} to{" "}
            {Math.min(pagination.currentPage * pagination.pageSize, displayTotal)} of{" "}
            {displayTotal} entries
          </div>
          {pagination.onPageSizeChange && (
            <div className="d-flex align-items-center gap-2">
              <label className="text-muted mb-0">Show:</label>
              <select
                className="form-select form-select-sm"
                style={{ width: "auto" }}
                value={pagination.pageSize}
                onChange={(e) => pagination.onPageSizeChange?.(Number(e.target.value))}
              >
                <option value={10}>10</option>
                <option value={20}>20</option>
                <option value={50}>50</option>
                <option value={100}>100</option>
              </select>
            </div>
          )}
        </div>
      )}
    </div>
  );
}
