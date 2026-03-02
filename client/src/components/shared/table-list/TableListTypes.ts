import { ReactNode } from 'react';

export interface BreadcrumbItem {
  title: string;
  to?: string;
}

export type StatColor = 'primary' | 'secondary' | 'success' | 'warning' | 'error' | 'info';

export interface TableListStatCard {
  label: string;
  value: string | number;
  icon?: ReactNode;
  color?: StatColor;
  /** When statisticsVariant is 'compact', use this as Card backgroundColor (e.g. 'secondary.light') */
  bgcolor?: string;
}

/** 'default' = icon left + label/value; 'compact' = value/label left, icon right, Card with bgcolor (Orders History style) */
export type TableListStatisticsVariant = 'default' | 'compact';

export interface TableListColumn<T = Record<string, unknown>> {
  id: string;
  label: string;
  align?: 'left' | 'right' | 'center';
  minWidth?: number;
  hideable?: boolean;
  format?: (value: unknown, row: T) => ReactNode;
  render?: (row: T) => ReactNode;
}

export interface TableListActionItem<T = Record<string, unknown>> {
  label: string;
  onClick: (row: T) => void;
  icon?: ReactNode;
}

export interface TableListActions<T = Record<string, unknown>> {
  dropdown?: TableListActionItem<T>[];
  more?: TableListActionItem<T>[];
}

export interface TableListPagination {
  page: number;
  rowsPerPage: number;
  totalRows: number;
  onPageChange: (page: number) => void;
  onRowsPerPageChange: (rowsPerPage: number) => void;
  rowsPerPageOptions?: number[];
}

/** TanStack table instance (return type of useReactTable). When provided, TableList renders TanStack table with sorting, expandable rows, column visibility menu, bulk delete, and optional snackbar. */
export type TanStackTableInstance<T> = import('@tanstack/react-table').Table<T>;

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

export interface TableListProps<T = object> {
  title: string;
  subtitle?: string;
  breadcrumbItems?: BreadcrumbItem[];
  statistics?: TableListStatCard[];
  statisticsTitle?: string;
  /** 'default' | 'compact' (Orders History style with bgcolor cards) */
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
  /** When true, toolbar shows search icon that toggles visibility of search input (old Order Table behavior) */
  searchToggle?: boolean;
  onFilterClick?: () => void;
  filterPanel?: TableListFilterPanel;
  onToggleColumnsClick?: () => void;
  onDownloadClick?: () => void;
  visibleColumnIds?: string[];
  onVisibleColumnsChange?: (columnIds: string[]) => void;
  renderActions?: (row: T) => ReactNode;
  actions?: (row: T) => TableListActions<T>;
  pagination?: TableListPagination;
  emptyMessage?: string;
  /** Title shown to the left of toolbar (e.g. "Orders Table") */
  tableTitle?: string;
  /** When provided, TableList renders TanStack table (sorting, expandable rows, column visibility from table). Requires renderExpandedRow when table has expandable rows. */
  tanStackTable?: TanStackTableInstance<T>;
  /** Renders expanded row content. Required when tanStackTable is used with expandable rows. */
  renderExpandedRow?: (row: T) => ReactNode;
  /** Snackbar (e.g. delete confirmation). Rendered when provided. */
  snackbar?: TableListSnackbar;
  /** When tanStackTable is provided and rows are selected, called when bulk delete icon is clicked. Icon shown only when selection count > 0. */
  onBulkDelete?: () => void;
}
