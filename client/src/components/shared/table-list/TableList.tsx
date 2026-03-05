'use client';

import './table-list.css';
import React, { useState } from 'react';
import {
  Box,
  Card,
  CardContent,
  IconButton,
  InputAdornment,
  Menu,
  MenuItem,
  Table,
  TableBody,
  TableCell,
  TableContainer,
  TableHead,
  TablePagination,
  TableRow,
  TextField,
  Typography,
  Checkbox,
  Stack,
  useTheme,
  FormControlLabel,
  InputBase,
  Snackbar,
  SnackbarContent,
  Tooltip,
  Divider,
  CircularProgress,
} from '@mui/material';
import CheckCircleOutlineIcon from '@mui/icons-material/CheckCircleOutline';
import {
  IconSearch,
  IconFilter,
  IconSettings,
  IconDownload,
  IconDotsVertical,
  IconArrowUp,
  IconArrowDown,
  IconArrowsSort,
  IconX,
  IconTrash,
} from '@tabler/icons-react';
import Link from 'next/link';
import { IconCircle } from '@tabler/icons-react';
import { flexRender } from '@tanstack/react-table';
import type {
  TableListProps,
  TableListColumn,
  TableListStatCard,
  BreadcrumbItem,
} from '@/types/table/tableList';

const statColorMap: Record<string, { bg: string; color: string }> = {
  primary: { bg: 'primary.light', color: 'primary.main' },
  secondary: { bg: 'secondary.light', color: 'secondary.main' },
  success: { bg: 'success.light', color: 'success.main' },
  warning: { bg: 'warning.light', color: 'warning.main' },
  error: { bg: 'error.light', color: 'error.main' },
  info: { bg: 'info.light', color: 'info.main' },
};

function TableListInner<T extends object>({
  title,
  subtitle,
  breadcrumbItems = [],
  statistics = [],
  statisticsTitle = 'Statistics',
  statisticsVariant = 'default',
  columns,
  rows,
  getRowId,
  selectable = false,
  selectedIds = [],
  onSelectionChange,
  searchValue = '',
  onSearchChange,
  searchPlaceholder = 'Search…',
  searchToggle = false,
  onFilterClick,
  filterPanel,
  onToggleColumnsClick,
  onDownloadClick,
  visibleColumnIds,
  onVisibleColumnsChange,
  renderActions,
  actions,
  pagination,
  emptyMessage = 'No data',
  loading = false,
  tableTitle,
  tanStackTable,
  renderExpandedRow,
  snackbar,
  onBulkDelete,
}: TableListProps<T>) {
  const theme = useTheme();
  const borderColor = theme.palette.divider;
  const isTanStackMode = Boolean(tanStackTable);

  const visibleIds = visibleColumnIds ?? columns.map((c) => c.id);
  const visibleColumns = columns.filter((c) => visibleIds.includes(c.id));

  const [toggleAnchor, setToggleAnchor] = useState<null | HTMLElement>(null);
  const [showSearchInput, setShowSearchInput] = useState(false);

  const handleSelectAll = (checked: boolean) => {
    if (!onSelectionChange) return;
    if (checked) onSelectionChange(rows.map((r) => getRowId(r)));
    else onSelectionChange([]);
  };

  const handleSelectRow = (id: string | number, checked: boolean) => {
    if (!onSelectionChange) return;
    if (checked) onSelectionChange([...selectedIds, id]);
    else onSelectionChange(selectedIds.filter((x) => x !== id));
  };

  const allSelected = rows.length > 0 && selectedIds.length === rows.length;
  const someSelected = selectedIds.length > 0;

  const getCellValue = (row: T, col: TableListColumn<T>): React.ReactNode => {
    const raw = (row as Record<string, unknown>)[col.id];
    if (col.render) return col.render(row);
    if (col.format) return col.format(raw, row);
    return raw != null ? String(raw) : '—';
  };

  const showBreadcrumbCard = title !== '' || breadcrumbItems.length > 0;
  const hasActions = Boolean(renderActions || actions);
  const totalSimpleCols =
    visibleColumns.length + (selectable ? 1 : 0) + (hasActions ? 1 : 0);

  return (
    <Box className="table-list-root">
      {/* ── Breadcrumb card ─────────────────────────────────────────── */}
      {showBreadcrumbCard && (
        <Card
          className="table-list-breadcrumb-card"
          sx={{ padding: 0, border: `1px solid ${borderColor}`, marginBottom: 3 }}
          elevation={0}
          variant="outlined"
        >
          <CardContent sx={{ p: '30px 25px 20px', position: 'relative', overflow: 'hidden' }}>
            <Typography variant="h4">{title}</Typography>
            {subtitle && (
              <Typography color="textSecondary" variant="h6" fontWeight={400} mt={0.8} mb={0}>
                {subtitle}
              </Typography>
            )}
            {breadcrumbItems.length > 0 && (
              <Box component="nav" sx={{ display: 'flex', alignItems: 'center', mt: 1.25 }} aria-label="breadcrumb">
                {breadcrumbItems.map((item: BreadcrumbItem, idx: number) => (
                  <React.Fragment key={item.title}>
                    {idx > 0 && (
                      <IconCircle size={5} style={{ margin: '0 5px', opacity: 0.6 }} fill="currentColor" />
                    )}
                    {item.to ? (
                      <Link href={item.to} passHref style={{ textDecoration: 'none' }}>
                        <Typography component="span" color="textSecondary" variant="body2">
                          {item.title}
                        </Typography>
                      </Link>
                    ) : (
                      <Typography component="span" color="textPrimary" variant="body2">
                        {item.title}
                      </Typography>
                    )}
                  </React.Fragment>
                ))}
              </Box>
            )}
          </CardContent>
        </Card>
      )}

      {/* ── Statistics – default variant ─────────────────────────────── */}
      {statistics.length > 0 && statisticsVariant === 'default' && (
        <>
          <Typography variant="h6" className="table-list-stats-title" sx={{ mb: 2 }}>
            {statisticsTitle}
          </Typography>
          <Stack className="table-list-stats-wrap" direction="row" spacing={2} flexWrap="wrap" useFlexGap sx={{ mb: 3 }}>
            {statistics.map((stat: TableListStatCard, idx: number) => {
              const colorKey = stat.color ?? 'primary';
              const colors = statColorMap[colorKey] ?? statColorMap.primary;
              return (
                <Card
                  key={idx}
                  className="table-list-stat-card"
                  elevation={0}
                  variant="outlined"
                  sx={{ minWidth: 140, border: `1px solid ${borderColor}`, borderRadius: 2 }}
                >
                  <CardContent sx={{ py: 2, px: 2.5 }}>
                    <Stack direction="row" alignItems="center" spacing={1.5}>
                      <Box
                        sx={{
                          width: 40,
                          height: 40,
                          borderRadius: 1.5,
                          bgcolor: colors.bg,
                          color: colors.color,
                          display: 'flex',
                          alignItems: 'center',
                          justifyContent: 'center',
                        }}
                      >
                        {stat.icon ?? null}
                      </Box>
                      <Box>
                        <Typography variant="body2" color="textSecondary">{stat.label}</Typography>
                        <Typography variant="h6" fontWeight={600}>{stat.value}</Typography>
                      </Box>
                    </Stack>
                  </CardContent>
                </Card>
              );
            })}
          </Stack>
        </>
      )}

      {/* ── Statistics – compact variant ─────────────────────────────── */}
      {statistics.length > 0 && statisticsVariant === 'compact' && (
        <>
          <Typography variant="h6" className="table-list-stats-title" sx={{ mb: 2 }}>
            {statisticsTitle}
          </Typography>
          <Box display="flex" gap={2} flexWrap="wrap" width="100%" sx={{ mb: 3 }}>
            {statistics.map((stat: TableListStatCard, idx: number) => (
              <Card
                key={idx}
                elevation={0}
                sx={{
                  backgroundColor: stat.bgcolor ?? 'grey.100',
                  p: 1.5,
                  flex: { xs: '1 1 calc(50% - 8px)', sm: '1 1 calc(33.33% - 8px)', md: '1 1 0' },
                }}
                className="table-list-stat-card"
              >
                <Stack direction="row" justifyContent="space-between">
                  <Box>
                    <Typography fontSize={16} fontWeight={600}>{stat.value}</Typography>
                    <Typography fontSize={11} color="text.secondary">{stat.label}</Typography>
                  </Box>
                  <Box width={20} height={20} fontSize={16}>{stat.icon ?? null}</Box>
                </Stack>
              </Card>
            ))}
          </Box>
        </>
      )}

      {/* ── Table card ──────────────────────────────────────────────── */}
      <Card className="table-list-table-card" elevation={0} variant="outlined" sx={{ border: `1px solid ${borderColor}` }}>
        <CardContent>
          {/* ── Toolbar ─────────────────────────────────────────────── */}
          <Box display="flex" alignItems="center" justifyContent="space-between" width="100%" sx={{ mb: 2 }}>
            {tableTitle && <Typography variant="h6">{tableTitle}</Typography>}
            <Box display="flex" alignItems="center" className="table-list-toolbar">
              {onSearchChange && searchToggle && (
                <>
                  {!showSearchInput ? (
                    <IconButton onClick={() => setShowSearchInput(true)} aria-label="Search" size="small">
                      <IconSearch size={16} />
                    </IconButton>
                  ) : (
                    <InputBase
                      value={searchValue}
                      onChange={(e) => onSearchChange(e.target.value)}
                      placeholder={searchPlaceholder}
                      onBlur={() => { if (!searchValue) setShowSearchInput(false); }}
                      autoFocus
                      sx={{ borderBottom: '1px solid', borderColor: 'divider', px: 1, fontSize: 14, minWidth: 160 }}
                    />
                  )}
                </>
              )}
              {onSearchChange && !searchToggle && (
                <TextField
                  size="small"
                  placeholder={searchPlaceholder}
                  value={searchValue}
                  onChange={(e) => onSearchChange(e.target.value)}
                  InputProps={{
                    startAdornment: (
                      <InputAdornment position="start">
                        <IconSearch size={20} />
                      </InputAdornment>
                    ),
                  }}
                  sx={{ minWidth: 220, mr: 1 }}
                />
              )}
              {filterPanel && (
                <IconButton onClick={filterPanel.onToggle} aria-label="Toggle filters" size="small">
                  {filterPanel.open ? <IconX size={16} /> : <IconFilter size={16} />}
                </IconButton>
              )}
              {onFilterClick && !filterPanel && (
                <IconButton onClick={onFilterClick} aria-label="Filter" size="small">
                  <IconFilter size={20} />
                </IconButton>
              )}
              {/* Column visibility toggle – TanStack mode */}
              {isTanStackMode && tanStackTable && (
                <>
                  <IconButton onClick={(e) => setToggleAnchor(e.currentTarget)} aria-label="Toggle columns" size="small">
                    <IconSettings size={16} />
                  </IconButton>
                  <Menu
                    anchorEl={toggleAnchor}
                    open={Boolean(toggleAnchor)}
                    onClose={() => setToggleAnchor(null)}
                    anchorOrigin={{ vertical: 'bottom', horizontal: 'right' }}
                    transformOrigin={{ vertical: 'top', horizontal: 'right' }}
                  >
                    <Box sx={{ minWidth: 180, p: 1 }}>
                      <Typography variant="subtitle2" gutterBottom sx={{ fontSize: 14, color: 'grey.600', textTransform: 'capitalize' }}>
                        Toggle Columns
                      </Typography>
                      {tanStackTable.getAllLeafColumns().map((col) => (
                        <FormControlLabel
                          key={col.id}
                          control={
                            <Checkbox
                              checked={col.getIsVisible()}
                              onChange={col.getToggleVisibilityHandler()}
                              size="small"
                            />
                          }
                          label={
                            <Typography sx={{ fontSize: 14, color: 'grey.600', textTransform: 'lowercase' }}>
                              {typeof col.columnDef.header === 'string' ? col.columnDef.header : col.id}
                            </Typography>
                          }
                        />
                      ))}
                    </Box>
                  </Menu>
                </>
              )}
              {/* Column visibility toggle – simple mode */}
              {!isTanStackMode && (onToggleColumnsClick || (columns.some((c) => c.hideable) && onVisibleColumnsChange)) && (
                <>
                  <IconButton
                    onClick={(e) => {
                      if (onToggleColumnsClick) onToggleColumnsClick();
                      else if (columns.some((c) => c.hideable)) setToggleAnchor(e.currentTarget);
                    }}
                    aria-label="Toggle columns"
                    size="small"
                  >
                    <IconSettings size={20} />
                  </IconButton>
                  <Menu
                    anchorEl={toggleAnchor}
                    open={Boolean(toggleAnchor)}
                    onClose={() => setToggleAnchor(null)}
                    anchorOrigin={{ vertical: 'bottom', horizontal: 'right' }}
                    transformOrigin={{ vertical: 'top', horizontal: 'right' }}
                  >
                    {columns.filter((c) => c.hideable).map((col) => {
                      const visible = visibleColumnIds?.includes(col.id) ?? true;
                      return (
                        <MenuItem
                          key={col.id}
                          onClick={() => {
                            if (!onVisibleColumnsChange) return;
                            if (visible)
                              onVisibleColumnsChange(visibleColumnIds?.filter((id) => id !== col.id) ?? []);
                            else onVisibleColumnsChange([...(visibleColumnIds ?? []), col.id]);
                          }}
                        >
                          <Checkbox checked={visible} size="small" sx={{ mr: 1 }} />
                          {col.label}
                        </MenuItem>
                      );
                    })}
                  </Menu>
                </>
              )}
              {onDownloadClick && (
                <IconButton onClick={onDownloadClick} aria-label="Download" size="small">
                  <IconDownload size={16} />
                </IconButton>
              )}
              {isTanStackMode && tanStackTable && onBulkDelete && tanStackTable.getSelectedRowModel().rows.length > 0 && (
                <Tooltip title={`Delete (${tanStackTable.getSelectedRowModel().rows.length})`}>
                  <IconButton color="error" onClick={onBulkDelete} size="small">
                    <IconTrash size={16} />
                  </IconButton>
                </Tooltip>
              )}
            </Box>
          </Box>

          {filterPanel?.open && filterPanel.content}

          {/* ── Table ───────────────────────────────────────────────── */}
          <TableContainer
            className="table-list-table-container"
            sx={{
              overflowX: 'auto',
              border: isTanStackMode ? '1px solid' : undefined,
              borderColor: isTanStackMode ? 'divider' : undefined,
              borderRadius: isTanStackMode ? 1 : undefined,
              mt: filterPanel?.open ? 2 : 0,
            }}
          >
            <Table className="table-list-table" size="small" sx={{ whiteSpace: 'nowrap' }} aria-label="table list">
              {isTanStackMode && tanStackTable ? (
                <TanStackTableContent
                  tanStackTable={tanStackTable}
                  renderExpandedRow={renderExpandedRow}
                  loading={loading}
                />
              ) : (
                <SimpleTableContent
                  columns={visibleColumns}
                  rows={rows}
                  getRowId={getRowId}
                  selectable={selectable}
                  selectedIds={selectedIds}
                  allSelected={allSelected}
                  someSelected={someSelected}
                  handleSelectAll={handleSelectAll}
                  handleSelectRow={handleSelectRow}
                  getCellValue={getCellValue}
                  renderActions={renderActions}
                  actions={actions}
                  hasActions={hasActions}
                  emptyMessage={emptyMessage}
                  loading={loading}
                  totalCols={totalSimpleCols}
                />
              )}
            </Table>
          </TableContainer>

          {/* ── Pagination ──────────────────────────────────────────── */}
          {isTanStackMode && tanStackTable && (
            <TablePagination
              component="div"
              count={tanStackTable.getFilteredRowModel().rows.length}
              page={tanStackTable.getState().pagination.pageIndex}
              onPageChange={(_, newPage) => tanStackTable.setPageIndex(newPage)}
              rowsPerPage={tanStackTable.getState().pagination.pageSize}
              onRowsPerPageChange={(e) => tanStackTable.setPageSize(Number(e.target.value))}
              rowsPerPageOptions={[5, 10, 25, 50]}
              labelRowsPerPage="Rows per page:"
              className="table-list-pagination"
            />
          )}
          {!isTanStackMode && pagination && (
            <TablePagination
              className="table-list-pagination"
              component="div"
              count={pagination.totalRows}
              page={pagination.page}
              onPageChange={(_, newPage) => pagination.onPageChange(newPage)}
              rowsPerPage={pagination.rowsPerPage}
              onRowsPerPageChange={(e) => pagination.onRowsPerPageChange(Number(e.target.value))}
              rowsPerPageOptions={pagination.rowsPerPageOptions ?? [5, 10, 25, 50]}
              labelRowsPerPage="Rows per page:"
            />
          )}
        </CardContent>
      </Card>

      {/* ── Snackbar ────────────────────────────────────────────────── */}
      {snackbar && (
        <Snackbar
          open={snackbar.open}
          autoHideDuration={3000}
          onClose={snackbar.onClose}
          anchorOrigin={{ vertical: 'top', horizontal: 'center' }}
        >
          <SnackbarContent
            sx={{ backgroundColor: 'success.main', display: 'flex', alignItems: 'center' }}
            message={
              <Box display="flex" alignItems="center" gap={1}>
                <CheckCircleOutlineIcon sx={{ color: 'white' }} />
                <Typography variant="body2" sx={{ color: 'white' }}>{snackbar.message}</Typography>
              </Box>
            }
          />
        </Snackbar>
      )}
    </Box>
  );
}

// ── TanStack table content ────────────────────────────────────────────

function TanStackTableContent<T extends object>({
  tanStackTable,
  renderExpandedRow,
  loading,
}: {
  tanStackTable: NonNullable<TableListProps<T>['tanStackTable']>;
  renderExpandedRow?: TableListProps<T>['renderExpandedRow'];
  loading: boolean;
}) {
  return (
    <>
      <TableHead>
        {tanStackTable.getHeaderGroups().map((headerGroup) => (
          <TableRow key={headerGroup.id}>
            {headerGroup.headers.map((header) => {
              const isSortable = header.column.getCanSort();
              return (
                <TableCell
                  key={header.id}
                  onClick={isSortable ? header.column.getToggleSortingHandler() : undefined}
                  sx={{ cursor: isSortable ? 'pointer' : 'default', fontWeight: 600, fontSize: '14px', backgroundColor: 'divider' }}
                >
                  <Box display="flex" alignItems="center">
                    <Typography variant="body2" fontWeight={600}>
                      {flexRender(header.column.columnDef.header, header.getContext())}
                    </Typography>
                    {isSortable && (
                      <Box ml={1} textAlign="center">
                        {header.column.getIsSorted() === 'asc' ? (
                          <IconArrowUp size={14} />
                        ) : header.column.getIsSorted() === 'desc' ? (
                          <IconArrowDown size={14} />
                        ) : (
                          <IconArrowsSort size={12} />
                        )}
                      </Box>
                    )}
                  </Box>
                </TableCell>
              );
            })}
          </TableRow>
        ))}
      </TableHead>
      <TableBody>
        {loading ? (
          <TableRow>
            <TableCell colSpan={tanStackTable.getAllLeafColumns().length} align="center" sx={{ py: 6 }}>
              <CircularProgress size={32} />
            </TableCell>
          </TableRow>
        ) : (
          tanStackTable.getRowModel().rows.map((row) => (
            <React.Fragment key={row.id}>
              <TableRow>
                {row.getVisibleCells().map((cell) => (
                  <TableCell key={cell.id} sx={{ fontSize: '14px', fontWeight: '500' }}>
                    {flexRender(cell.column.columnDef.cell, cell.getContext())}
                  </TableCell>
                ))}
              </TableRow>
              {row.getIsExpanded() && renderExpandedRow && (
                <TableRow sx={{ backgroundColor: 'grey.100' }}>
                  <TableCell colSpan={row.getVisibleCells().length}>
                    <Box p={2}>{renderExpandedRow(row.original as T)}</Box>
                  </TableCell>
                </TableRow>
              )}
            </React.Fragment>
          ))
        )}
      </TableBody>
    </>
  );
}

// ── Simple table content ──────────────────────────────────────────────

function SimpleTableContent<T extends object>({
  columns,
  rows,
  getRowId,
  selectable,
  selectedIds,
  allSelected,
  someSelected,
  handleSelectAll,
  handleSelectRow,
  getCellValue,
  renderActions,
  actions,
  hasActions,
  emptyMessage,
  loading,
  totalCols,
}: {
  columns: TableListProps<T>['columns'];
  rows: T[];
  getRowId: (row: T) => string | number;
  selectable: boolean;
  selectedIds: (string | number)[];
  allSelected: boolean;
  someSelected: boolean;
  handleSelectAll: (checked: boolean) => void;
  handleSelectRow: (id: string | number, checked: boolean) => void;
  getCellValue: (row: T, col: TableListProps<T>['columns'][number]) => React.ReactNode;
  renderActions?: TableListProps<T>['renderActions'];
  actions?: (row: T) => import('@/types/table/tableList').TableListActionItem<T>[];
  hasActions: boolean;
  emptyMessage: string;
  loading: boolean;
  totalCols: number;
}) {
  return (
    <>
      <TableHead>
        <TableRow>
          {selectable && (
            <TableCell padding="checkbox">
              <Checkbox
                indeterminate={someSelected && !allSelected}
                checked={allSelected}
                onChange={(_, checked) => handleSelectAll(checked)}
                size="small"
              />
            </TableCell>
          )}
          {columns.map((col) => (
            <TableCell key={col.id} align={col.align ?? 'left'}>
              <Typography variant="subtitle2" fontWeight={600}>{col.label}</Typography>
            </TableCell>
          ))}
          {hasActions && (
            <TableCell align="right">
              <Typography variant="subtitle2" fontWeight={600}>ACTIONS</Typography>
            </TableCell>
          )}
        </TableRow>
      </TableHead>
      <TableBody>
        {loading ? (
          <TableRow>
            <TableCell colSpan={totalCols} align="center" sx={{ py: 6 }}>
              <CircularProgress size={32} />
            </TableCell>
          </TableRow>
        ) : rows.length === 0 ? (
          <TableRow>
            <TableCell colSpan={totalCols}>
              <Typography className="table-list-empty" color="textSecondary" textAlign="center" py={4}>
                {emptyMessage}
              </Typography>
            </TableCell>
          </TableRow>
        ) : (
          rows.map((row) => {
            const rowId = getRowId(row);
            const selected = selectedIds.includes(rowId);
            return (
              <TableRow key={String(rowId)} hover selected={selected}>
                {selectable && (
                  <TableCell padding="checkbox">
                    <Checkbox checked={selected} onChange={(_, checked) => handleSelectRow(rowId, checked)} size="small" />
                  </TableCell>
                )}
                {columns.map((col) => (
                  <TableCell key={col.id} align={col.align ?? 'left'}>
                    {getCellValue(row, col)}
                  </TableCell>
                ))}
                {hasActions && (
                  <TableCell className="table-list-actions-cell" align="right" sx={{ whiteSpace: 'nowrap' }}>
                    {renderActions ? (
                      renderActions(row)
                    ) : actions ? (
                      <RowActionsMenu row={row} actions={actions} />
                    ) : null}
                  </TableCell>
                )}
              </TableRow>
            );
          })
        )}
      </TableBody>
    </>
  );
}

// ── Row actions dropdown (rendered per-row so Menu anchors correctly) ─

function RowActionsMenu<T extends object>({
  row,
  actions,
}: {
  row: T;
  actions: (row: T) => import('@/types/table/tableList').TableListActionItem<T>[];
}) {
  const [anchorEl, setAnchorEl] = useState<null | HTMLElement>(null);
  const items = actions(row);

  if (items.length === 0) return null;

  return (
    <>
      <IconButton
        size="small"
        onClick={(e) => setAnchorEl(e.currentTarget)}
        aria-label="Row actions"
      >
        <IconDotsVertical size={18} />
      </IconButton>
      <Menu
        anchorEl={anchorEl}
        open={Boolean(anchorEl)}
        onClose={() => setAnchorEl(null)}
        anchorOrigin={{ vertical: 'bottom', horizontal: 'right' }}
        transformOrigin={{ vertical: 'top', horizontal: 'right' }}
      >
        {items.map((item, idx) => (
          <React.Fragment key={idx}>
            {item.divider && idx > 0 && <Divider />}
            <MenuItem
              onClick={() => {
                item.onClick(row);
                setAnchorEl(null);
              }}
              sx={item.color ? { color: item.color } : undefined}
            >
              {item.icon && <Box component="span" sx={{ mr: 1, display: 'inline-flex', alignItems: 'center' }}>{item.icon}</Box>}
              {item.label}
            </MenuItem>
          </React.Fragment>
        ))}
      </Menu>
    </>
  );
}

// ── Public export ─────────────────────────────────────────────────────

export function TableList<T extends object>(props: TableListProps<T>) {
  return <TableListInner {...props} />;
}

export default TableList;
