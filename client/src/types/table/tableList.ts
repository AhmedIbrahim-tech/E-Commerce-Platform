import type { ReactNode } from 'react';

// ── Breadcrumb ──────────────────────────────────────────────────────

export interface BreadcrumbItem {
  title: string;
  to?: string;
}

// ── Statistics ──────────────────────────────────────────────────────

export type StatColor = 'primary' | 'secondary' | 'success' | 'warning' | 'error' | 'info';

export type TableListStatisticsVariant = 'default' | 'compact';

export interface TableListStatCard {
  label: string;
  value: string | number;
  icon?: ReactNode;
  color?: StatColor;
  bgcolor?: string;
}

// ── Columns ─────────────────────────────────────────────────────────

export interface TableListColumn<T = Record<string, unknown>> {
  id: string;
  label: string;
  align?: 'left' | 'right' | 'center';
  minWidth?: number;
  hideable?: boolean;
  format?: (value: unknown, row: T) => ReactNode;
  render?: (row: T) => ReactNode;
}

// ── Row actions ─────────────────────────────────────────────────────

export interface TableListActionItem<T = Record<string, unknown>> {
  label: string;
  onClick: (row: T) => void;
  icon?: ReactNode;
  color?: string;
  divider?: boolean;
}

// ── Pagination ──────────────────────────────────────────────────────

export interface TableListPagination {
  page: number;
  rowsPerPage: number;
  totalRows: number;
  onPageChange: (page: number) => void;
  onRowsPerPageChange: (rowsPerPage: number) => void;
  rowsPerPageOptions?: number[];
}

// ── Filter / Snackbar ───────────────────────────────────────────────

export interface TableListFilterPanel {
  open: boolean;
  onToggle: () => void;
  content: ReactNode;
}

export interface TableListSnackbar {
  open: boolean;
  message: string;
  onClose: () => void;
}

// ── TanStack helper type ────────────────────────────────────────────

export type TanStackTableInstance<T> = import('@tanstack/react-table').Table<T>;

// ── Main props ──────────────────────────────────────────────────────

export interface TableListProps<T = object> {
  title: string;
  subtitle?: string;
  breadcrumbItems?: BreadcrumbItem[];

  statistics?: TableListStatCard[];
  statisticsTitle?: string;
  statisticsVariant?: TableListStatisticsVariant;

  columns: TableListColumn<T>[];
  rows: T[];
  getRowId: (row: T) => string | number;

  selectable?: boolean;
  selectedIds?: (string | number)[];
  onSelectionChange?: (selectedIds: (string | number)[]) => void;

  searchValue?: string;
  onSearchChange?: (value: string) => void;
  searchPlaceholder?: string;
  searchToggle?: boolean;

  onFilterClick?: () => void;
  filterPanel?: TableListFilterPanel;

  onToggleColumnsClick?: () => void;
  onDownloadClick?: () => void;
  visibleColumnIds?: string[];
  onVisibleColumnsChange?: (columnIds: string[]) => void;

  /** Custom render for the actions cell (full control). */
  renderActions?: (row: T) => ReactNode;
  /** Built-in row actions shown in a single dropdown menu. */
  actions?: (row: T) => TableListActionItem<T>[];

  pagination?: TableListPagination;
  emptyMessage?: string;
  loading?: boolean;
  tableTitle?: string;

  tanStackTable?: TanStackTableInstance<T>;
  renderExpandedRow?: (row: T) => ReactNode;
  snackbar?: TableListSnackbar;
  onBulkDelete?: () => void;
}
