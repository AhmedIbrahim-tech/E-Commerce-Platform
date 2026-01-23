"use client";

import { useMemo, useState, forwardRef, useImperativeHandle } from "react";
import { useListTable } from "@/app/(dashboard)/shared/modules/hooks/useListTable";

export interface TableColumn<RowT> {
  key: keyof RowT | string;
  title: string;
  render?: (value: any, row: RowT) => React.ReactNode;
}

export type ActionVariant = "primary" | "success" | "info" | "warning" | "danger" | "secondary" | "light";

export interface TableAction<RowT> {
  label?: string;
  icon?: string;
  variant?: ActionVariant;
  className?: string;
  onClick: (row: RowT) => void;
  show?: (row: RowT) => boolean;
  danger?: boolean;
  tooltip?: string;
}

interface TableProps<RowT extends { id: string }> {
  columns: TableColumn<RowT>[];
  fetcher: (
    page: number,
    pageSize: number,
    searchTerm?: string
  ) => Promise<{ data: RowT[]; totalCount: number }>;

  actions?: TableAction<RowT>[];

  onAdd?: () => void;
  onDeleteMultiple?: (ids: string[]) => void;

  initialPageSize?: number;
  initialSearchTerm?: string;
  searchPlaceholder?: string;
  emptyMessage?: string;
  tableId?: string;
  useActionDropdown?: boolean;
  showActions?: boolean;
}

export interface TableRef {
  reload: () => void;
}

const Table = forwardRef<TableRef, TableProps<any>>(
  function TableComponent<RowT extends { id: string }>(
    {
      columns,
      fetcher,
      actions = [],
      onAdd,
      onDeleteMultiple,
      initialPageSize = 10,
      initialSearchTerm = "",
      searchPlaceholder = "Search...",
      emptyMessage = "No data found",
      tableId = "table",
      useActionDropdown = false,
      showActions = true,
    }: TableProps<RowT>,
    ref: React.Ref<TableRef>
  ) {
    const [selected, setSelected] = useState<string[]>([]);

    const {
      data,
      loading,
      currentPage,
      pageSize,
      totalCount,
      searchTerm,
      setCurrentPage,
      setSearchTerm,
      reload,
    } = useListTable<RowT>({
      fetcher,
      initialPageSize,
      initialSearchTerm,
      autoFetch: true,
    });

    useImperativeHandle(ref, () => ({
      reload,
    }));

  const totalPages = Math.ceil(totalCount / pageSize);

  const toggleAll = (checked: boolean) => {
    if (checked) {
      setSelected(data.map((d) => d.id));
    } else {
      setSelected([]);
    }
  };

  const toggleOne = (id: string) => {
    setSelected((prev) =>
      prev.includes(id) ? prev.filter((x) => x !== id) : [...prev, id]
    );
  };

  const getActionButtonClass = (action: TableAction<RowT>) => {
    if (action.className) {
      return action.className;
    }

    const variant = action.variant || (action.danger ? "danger" : "primary");
    const baseClass = useActionDropdown
      ? "dropdown-item"
      : "btn btn-sm";

    if (useActionDropdown) {
      return action.danger
        ? `${baseClass} text-danger`
        : baseClass;
    }

    // Button styling - icon-only buttons use btn-icon, buttons with labels use regular btn classes
    if (!action.label) {
      // Icon-only button styling
      const variantMap: Record<ActionVariant, string> = {
        primary: "btn-soft-primary",
        success: "btn-soft-success",
        info: "btn-soft-info",
        warning: "btn-soft-warning",
        danger: "btn-soft-danger",
        secondary: "btn-soft-secondary",
        light: "btn-light",
      };
      return `btn btn-sm btn-icon ${variantMap[variant]}`;
    }

    // Button with label styling
    const variantMap: Record<ActionVariant, string> = {
      primary: "btn-primary",
      success: "btn-success",
      info: "btn-info",
      warning: "btn-warning",
      danger: "btn-danger",
      secondary: "btn-secondary",
      light: "btn-light",
    };

    return `${baseClass} ${variantMap[variant]}`;
  };

  const visibleActions = useMemo(() => {
    return (row: RowT) => {
      return actions.filter((action) => {
        if (action.show) {
          return action.show(row);
        }
        return true;
      });
    };
  }, [actions]);

  const renderActionButtons = (row: RowT) => {
    const rowActions = visibleActions(row);

    if (rowActions.length === 0) {
      return null;
    }

    if (useActionDropdown) {
      const dropdownId = `${tableId}-dropdown-${row.id}`;
      return (
        <div className="dropdown">
          <button
            className="btn btn-light btn-sm dropdown-toggle"
            type="button"
            data-bs-toggle="dropdown"
            aria-expanded="false"
            id={dropdownId}
          >
            <i className="ri-more-2-line"></i>
          </button>
          <ul className="dropdown-menu" aria-labelledby={dropdownId}>
            {rowActions.map((action, i) => (
              <li key={i}>
                <button
                  className={getActionButtonClass(action)}
                  onClick={() => action.onClick(row)}
                  title={action.tooltip || action.label}
                >
                  {action.icon && <i className={`${action.icon} align-bottom me-2`}></i>}
                  {action.label}
                </button>
              </li>
            ))}
          </ul>
        </div>
      );
    }

    return (
      <div className="d-flex gap-2">
        {rowActions.map((action, i) => (
          <button
            key={i}
            className={getActionButtonClass(action)}
            onClick={() => action.onClick(row)}
            title={action.tooltip || action.label || ""}
            type="button"
          >
            {action.icon && (
              <i className={`${action.icon} ${action.label ? "align-bottom me-1" : ""}`}></i>
            )}
            {action.label && action.label}
          </button>
        ))}
      </div>
    );
  };

  const actionColSpan = showActions && actions.length > 0 ? 1 : 0;
  const checkboxColSpan = onDeleteMultiple ? 1 : 0;
  const totalColSpan = columns.length + actionColSpan + checkboxColSpan;

  return (
    <div id={tableId}>
      {/* Toolbar */}
      <div className="row g-4 mb-3">
        <div className="col-sm-auto">
          <div>
            {onAdd && (
              <button
                className="btn btn-success add-btn"
                onClick={onAdd}
                type="button"
              >
                <i className="ri-add-line align-bottom me-1"></i> Add
              </button>
            )}

            {onDeleteMultiple && selected.length > 0 && (
              <button
                className="btn btn-soft-danger ms-2"
                onClick={() => onDeleteMultiple(selected)}
                type="button"
              >
                <i className="ri-delete-bin-2-line"></i>
              </button>
            )}
          </div>
        </div>

        <div className="col-sm">
          <div className="d-flex justify-content-sm-end">
            <div className="search-box ms-2">
              <input
                type="text"
                className="form-control search"
                placeholder={searchPlaceholder}
                value={searchTerm}
                onChange={(e) => setSearchTerm(e.target.value)}
              />
              <i className="ri-search-line search-icon"></i>
            </div>
          </div>
        </div>
      </div>

      {/* Table */}
      <div className="table-responsive table-card mt-3 mb-1">
        <table className="table align-middle table-nowrap">
          <thead className="table-light">
            <tr>
              {onDeleteMultiple && (
                <th style={{ width: 50 }}>
                  <div className="form-check">
                    <input
                      type="checkbox"
                      className="form-check-input"
                      checked={
                        data.length > 0 && selected.length === data.length
                      }
                      onChange={(e) => toggleAll(e.target.checked)}
                    />
                  </div>
                </th>
              )}

              {columns.map((col) => (
                <th key={String(col.key)}>{col.title}</th>
              ))}

              {showActions && actions.length > 0 && <th>Action</th>}
            </tr>
          </thead>

          <tbody className="list form-check-all">
            {loading ? (
              <tr>
                <td colSpan={totalColSpan} className="text-center py-4">
                  <div className="spinner-border text-primary" role="status">
                    <span className="visually-hidden">Loading...</span>
                  </div>
                </td>
              </tr>
            ) : data.length === 0 ? (
              <tr>
                <td colSpan={totalColSpan}>
                  <div className="noresult text-center py-4">
                    <h5 className="mt-2">Sorry! No Result Found</h5>
                    <p className="text-muted">{emptyMessage}</p>
                  </div>
                </td>
              </tr>
            ) : (
              data.map((row) => (
                <tr key={row.id}>
                  {onDeleteMultiple && (
                    <th>
                      <div className="form-check">
                        <input
                          type="checkbox"
                          className="form-check-input"
                          checked={selected.includes(row.id)}
                          onChange={() => toggleOne(row.id)}
                        />
                      </div>
                    </th>
                  )}

                  {columns.map((col) => (
                    <td key={String(col.key)}>
                      {col.render
                        ? col.render((row as any)[col.key], row)
                        : String((row as any)[col.key] ?? "-")}
                    </td>
                  ))}

                  {showActions && actions.length > 0 && (
                    <td>
                      {renderActionButtons(row)}
                    </td>
                  )}
                </tr>
              ))
            )}
          </tbody>
        </table>
      </div>

      {/* Pagination */}
      {totalPages > 1 && (
        <div className="d-flex justify-content-end">
          <div className="pagination-wrap hstack gap-2">
            <button
              className="page-item pagination-prev btn btn-sm btn-soft-secondary"
              disabled={currentPage === 1}
              onClick={() => setCurrentPage(currentPage - 1)}
              type="button"
            >
              Previous
            </button>

            <div className="d-flex gap-1">
              {Array.from({ length: Math.min(totalPages, 10) }, (_, i) => {
                let pageNum: number;
                if (totalPages <= 10) {
                  pageNum = i + 1;
                } else if (currentPage <= 5) {
                  pageNum = i + 1;
                } else if (currentPage >= totalPages - 4) {
                  pageNum = totalPages - 9 + i;
                } else {
                  pageNum = currentPage - 5 + i;
                }

                return (
                  <button
                    key={pageNum}
                    className={`page-item btn btn-sm ${
                      pageNum === currentPage
                        ? "btn-primary"
                        : "btn-soft-secondary"
                    }`}
                    onClick={() => setCurrentPage(pageNum)}
                    type="button"
                  >
                    {pageNum}
                  </button>
                );
              })}
            </div>

            <button
              className="page-item pagination-next btn btn-sm btn-soft-secondary"
              disabled={currentPage === totalPages}
              onClick={() => setCurrentPage(currentPage + 1)}
              type="button"
            >
              Next
            </button>
          </div>
        </div>
      )}

      {totalPages > 0 && (
        <div className="d-flex justify-content-between align-items-center mt-3">
          <div className="text-muted">
            Showing {((currentPage - 1) * pageSize) + 1} to{" "}
            {Math.min(currentPage * pageSize, totalCount)} of {totalCount} entries
          </div>
        </div>
      )}
    </div>
  );
  }
);

(Table as any).displayName = "Table";

export default Table as <RowT extends { id: string }>(
  props: TableProps<RowT> & { ref?: React.Ref<TableRef> }
) => React.ReactElement;
