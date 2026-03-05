'use client';

import React, { useMemo, useState } from 'react';
import PageContainer from '@/components/container/PageContainer';
import Breadcrumb from '@/layouts/dashboard/shared/breadcrumb/Breadcrumb';
import TableList from '@/components/shared/table-list/TableList';
import {
  useReactTable,
  getCoreRowModel,
  getFilteredRowModel,
  getSortedRowModel,
  getExpandedRowModel,
  getPaginationRowModel,
  createColumnHelper,
  type SortingState,
  type Row,
  type Table as TanStackTable,
} from '@tanstack/react-table';
import {
  Table,
  TableBody,
  TableCell,
  TableHead,
  TableRow,
  IconButton,
  Menu,
  MenuItem,
  Typography,
  Grid,
  Box,
  Chip,
  Avatar,
  FormControl,
  InputLabel,
  ListItemIcon,
  ListItemText,
  Stack,
  Button,
  Divider,
  useTheme,
} from '@mui/material';
import MoreVertIcon from '@mui/icons-material/MoreVert';
import { KeyboardArrowDown } from '@mui/icons-material';
import KeyboardArrowUpIcon from '@mui/icons-material/KeyboardArrowUp';
import CloseIcon from '@mui/icons-material/Close';
import CheckIcon from '@mui/icons-material/Check';
import { orderData } from '@/data/orderData';
import { OrderType } from '@/types/table/order';
import CustomTextField from '@/components/forms/theme-elements/CustomTextField';
import { LocalizationProvider } from '@mui/x-date-pickers/LocalizationProvider';
import { DatePicker } from '@mui/x-date-pickers';
import dayjs, { Dayjs } from 'dayjs';
import { AdapterDayjs } from '@mui/x-date-pickers/AdapterDayjs';
import CustomSelect from '@/components/forms/theme-elements/CustomSelect';
import CustomCheckbox from '@/components/forms/theme-elements/CustomCheckbox';
import {
  IconClockFilled,
  IconPremiumRights,
  IconProgressX,
  IconRefresh,
  IconRosetteDiscountCheckFilled,
  IconTruckFilled,
  IconPencil,
  IconTrash,
} from '@tabler/icons-react';
import type { TableListColumn } from '@/types/table/tableList';

const BCrumb = [
  { to: '/', title: 'Dashboard' },
  { title: 'Order Table' },
];

const placeholderColumns: TableListColumn<OrderType>[] = [
  { id: 'id', label: 'ID' },
  { id: 'customer.name', label: 'CUSTOMER' },
  { id: 'status', label: 'STATUS' },
  { id: 'date', label: 'DATE' },
  { id: 'amount', label: 'AMOUNT' },
  { id: 'address', label: 'ADDRESS' },
];

export default function OrderTablePage() {
  const theme = useTheme();
  const statsData = useMemo(
    () => [
      {
        value: '$95.3k',
        label: 'Income',
        icon: <IconPremiumRights color={theme.palette.secondary.main} />,
        bgcolor: 'secondary.light',
      },
      {
        value: '485',
        label: 'Pending',
        icon: <IconClockFilled color={theme.palette.success.main} />,
        bgcolor: 'success.light',
      },
      {
        value: '1.4k',
        label: 'Completed',
        icon: <IconRosetteDiscountCheckFilled color={theme.palette.warning.main} />,
        bgcolor: 'warning.light',
      },
      {
        value: '996',
        label: 'Shipping',
        icon: <IconTruckFilled color={theme.palette.info.main} />,
        bgcolor: 'info.light',
      },
      {
        value: '2.1k',
        label: 'Processing',
        icon: <IconRefresh color={theme.palette.primary.main} />,
        bgcolor: 'primary.light',
      },
      {
        value: '1.1k',
        label: 'Cancelled',
        icon: <IconProgressX color={theme.palette.error.main} />,
        bgcolor: 'error.light',
      },
    ],
    [theme]
  );

  const [data, setData] = useState(orderData);
  const [globalFilter, setGlobalFilter] = useState('');
  const [columnVisibility, setColumnVisibility] = useState({});
  const [anchorEl, setAnchorEl] = useState<null | HTMLElement>(null);
  const [selectedRowId, setSelectedRowId] = useState<string | null>(null);
  const [dateRange, setDateRange] = useState<Dayjs | null>(null);
  const [filterOpen, setFilterOpen] = useState(false);
  const [editingRowId, setEditingRowId] = useState<string | null>(null);
  const [editedRowData, setEditedRowData] = useState<Partial<OrderType>>({});
  const [snackbarOpen, setSnackbarOpen] = useState(false);
  const [snackbarMsg, setSnackbarMsg] = useState('');
  const [sorting, setSorting] = useState<SortingState>([]);
  const [columnFilters, setColumnFilters] = useState({
    id: '',
    'customer.name': '',
    product: '',
    status: '',
    date: '',
    amount: '',
    address: '',
  });

  const columnHelper = createColumnHelper<OrderType>();

  const columns = useMemo(
    () => [
      columnHelper.display({
        id: 'Row Selection',
        header: ({ table }: { table: TanStackTable<OrderType> }) => (
          <CustomCheckbox
            checked={table.getIsAllRowsSelected()}
            onChange={table.getToggleAllRowsSelectedHandler()}
          />
        ),
        cell: ({ row }: { row: Row<OrderType> }) => (
          <CustomCheckbox
            checked={row.getIsSelected()}
            onChange={row.getToggleSelectedHandler()}
          />
        ),
      }),
      columnHelper.accessor('id', { header: ' ID' }),
      columnHelper.accessor('customer.name', {
        header: 'CUSTOMER',
        cell: ({ row }: { row: Row<OrderType> }) =>
          editingRowId === row.original.id ? (
            <CustomTextField
              size="small"
              value={editedRowData.customer?.name ?? row.original.customer.name}
              onChange={(e: { target: { value: string } }) =>
                setEditedRowData((prev) => ({
                  ...prev,
                  customer: {
                    ...(prev.customer ?? row.original.customer),
                    name: e.target.value,
                  },
                }))
              }
            />
          ) : (
            <Box display="flex" alignItems="center" gap={1}>
              <Avatar
                src={row.original.customer.avatar}
                alt={row.original.customer.name}
                sx={{ width: 24, height: 24 }}
              />
              {row.original.customer.name}
            </Box>
          ),
      }),
      columnHelper.accessor('status', {
        header: 'STATUS',
        cell: ({ row }: { row: Row<OrderType> }) =>
          editingRowId === row.original.id ? (
            <FormControl fullWidth size="small">
              <CustomSelect
                value={editedRowData.status ?? row.original.status}
                onChange={(e: { target: { value: string } }) =>
                  setEditedRowData((prev) => ({ ...prev, status: e.target.value as OrderType['status'] }))
                }
              >
                {['Pending', 'Shipped', 'Completed', 'Cancelled', 'Processing'].map((status) => (
                  <MenuItem key={status} value={status}>
                    {status}
                  </MenuItem>
                ))}
              </CustomSelect>
            </FormControl>
          ) : (
            <Chip
              label={row.original.status}
              size="small"
              sx={{
                bgcolor: (t) =>
                  ({
                    Shipped: t.palette.info.light,
                    Pending: t.palette.success.light,
                    Completed: t.palette.warning.light,
                    Cancelled: t.palette.error.light,
                    Processing: t.palette.primary.light,
                  }[row.original.status] ?? t.palette.secondary.light),
                color: (t) =>
                  ({
                    Shipped: t.palette.info.main,
                    Pending: t.palette.success.main,
                    Completed: t.palette.warning.main,
                    Cancelled: t.palette.error.main,
                    Processing: t.palette.primary.main,
                  }[row.original.status] ?? t.palette.secondary.main),
              }}
            />
          ),
      }),
      columnHelper.accessor('date', {
        header: 'DATE',
        cell: ({ row }: { row: Row<OrderType> }) => {
          if (editingRowId === row.original.id) {
            return (
              <LocalizationProvider dateAdapter={AdapterDayjs}>
                <DatePicker
                  value={dayjs(editedRowData.date ?? row.original.date)}
                  onChange={(newValue) => {
                    if (dayjs.isDayjs(newValue)) {
                      setEditedRowData((prev) => ({
                        ...prev,
                        date: newValue.format('DD-MM-YYYY'),
                      }));
                    }
                  }}
                  slotProps={{ textField: { size: 'small', fullWidth: true } }}
                />
              </LocalizationProvider>
            );
          }
          return (
            <Box>
              <Typography>{row.original.date}</Typography>
              <Typography fontSize="0.75rem" color="text.secondary">
                {row.original.time}
              </Typography>
            </Box>
          );
        },
      }),
      columnHelper.accessor('amount', {
        header: 'AMOUNT',
        cell: ({ row }: { row: Row<OrderType> }) =>
          editingRowId === row.original.id ? (
            <CustomTextField
              value={editedRowData.amount ?? row.original.amount}
              size="small"
              onChange={(e: { target: { value: string } }) =>
                setEditedRowData((prev) => ({ ...prev, amount: parseFloat(e.target.value) }))
              }
            />
          ) : (
            ` $${row.original.amount.toFixed(2)}`
          ),
      }),
      columnHelper.accessor('address', {
        header: 'ADDRESS',
        cell: ({ row }: { row: Row<OrderType> }) =>
          editingRowId === row.original.id ? (
            <CustomTextField
              size="small"
              fullWidth
              value={editedRowData.address ?? row.original.address}
              onChange={(e: { target: { value: string } }) =>
                setEditedRowData((prev) => ({ ...prev, address: e.target.value }))
              }
            />
          ) : (
            row.original.address
          ),
      }),
      columnHelper.display({
        id: 'actions',
        header: 'ACTIONS',
        cell: ({ row }: { row: Row<OrderType> }) => {
          const isEditing = editingRowId === row.original.id;

          return (
            <Box display="flex" alignItems="center">
              {/* Expand/collapse */}
              <IconButton onClick={() => row.toggleExpanded()}>
                {row.getIsExpanded() ? (
                  <KeyboardArrowUpIcon fontSize="small" />
                ) : (
                  <KeyboardArrowDown fontSize="small" />
                )}
              </IconButton>
              {/* If this row is being edited, show check/close */}
              {isEditing ? (
                <>
                  <IconButton
                    onClick={() => {
                      setData((prev) =>
                        prev.map((item) =>
                          item.id === row.original.id ? { ...item, ...editedRowData } : item
                        )
                      );
                      setEditingRowId(null);
                      setEditedRowData({});
                    }}
                  >
                    <CheckIcon color="success" fontSize="small" />
                  </IconButton>
                  <IconButton
                    onClick={() => {
                      setEditingRowId(null);
                      setEditedRowData({});
                    }}
                  >
                    <CloseIcon color="error" fontSize="small" />
                  </IconButton>
                </>
              ) : (
                // Default More menu icon
                <>
                  <IconButton
                    onClick={(e) => {
                      e.stopPropagation();
                      setSelectedRowId(row.original.id);
                      setAnchorEl(e.currentTarget);
                    }}
                    aria-haspopup="true"
                    aria-expanded={Boolean(anchorEl) && selectedRowId === row.original.id}
                    aria-controls={Boolean(anchorEl) && selectedRowId === row.original.id ? 'order-table-row-menu' : undefined}
                    id={selectedRowId === row.original.id ? 'order-table-row-menu-button' : undefined}
                  >
                    <MoreVertIcon fontSize="small" />
                  </IconButton>
                  <Menu
                    id="order-table-row-menu"
                    anchorEl={anchorEl}
                    open={Boolean(anchorEl) && selectedRowId === row.original.id}
                    onClose={() => {
                      setAnchorEl(null);
                      setSelectedRowId(null);
                    }}
                    anchorOrigin={{ vertical: 'bottom', horizontal: 'right' }}
                    transformOrigin={{ vertical: 'top', horizontal: 'right' }}
                    MenuListProps={{
                      'aria-labelledby': 'order-table-row-menu-button',
                    }}
                  >
                    <MenuItem
                      onClick={() => {
                        setEditingRowId(row.original.id);
                        setAnchorEl(null);
                        setSelectedRowId(null);
                      }}
                    >
                      <ListItemIcon>
                        <IconPencil size={20} />
                      </ListItemIcon>
                      <ListItemText> Edit</ListItemText>
                    </MenuItem>
                    <MenuItem onClick={handleDelete}>
                      <ListItemIcon>
                        <IconTrash stroke="red" size={20} />
                      </ListItemIcon>
                      <ListItemText> Delete</ListItemText>
                    </MenuItem>
                  </Menu>
                </>
              )}
            </Box>
          );
        },
      }),
    ],
    [editingRowId, editedRowData, anchorEl, selectedRowId]
  );

  const filteredData = useMemo(() => {
    const formattedDate =
      dateRange && dayjs.isDayjs(dateRange) ? dateRange.format('MM-DD-YYYY') : null;
    return data.filter(
      (item) =>
        item.id.toLowerCase().includes(columnFilters.id.toLowerCase()) &&
        item.customer.name.toLowerCase().includes(columnFilters['customer.name'].toLowerCase()) &&
        item.status.toLowerCase().includes(columnFilters.status.toLowerCase()) &&
        item.amount.toString().includes(columnFilters.amount.toLowerCase()) &&
        item.address.toLowerCase().includes(columnFilters.address.toLowerCase()) &&
        (!formattedDate || item.date === formattedDate)
    );
  }, [data, columnFilters, dateRange]);

  const handleDownload = () => {
    const headers = [
      'Order ID',
      'Customer Name',
      'Status',
      'Date',
      'Amount',
      'Payment Status',
      'Address',
    ];
    const rows = data.map((item) => [
      item.id,
      item.customer.name,
      item.status,
      item.date,
      item.amount.toFixed(2),
      item.address,
    ]);
    const csvContent = [headers.join(','), ...rows.map((e) => e.join(','))].join('\n');
    const blob = new Blob([csvContent], { type: 'text/csv;charset=utf-8;' });
    const url = URL.createObjectURL(blob);
    const link = document.createElement('a');
    link.href = url;
    link.setAttribute('download', 'order-data.csv');
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
  };

  const handleCloseMenu = () => {
    setAnchorEl(null);
    setSelectedRowId(null);
  };

  const handleDelete = () => {
    setData((prev) => prev.filter((item) => item.id !== selectedRowId));
    setSnackbarMsg('Order deleted');
    setSnackbarOpen(true);
    handleCloseMenu();
  };

  const tableInstance = useReactTable({
    data: filteredData,
    columns,
    state: { globalFilter, columnVisibility, sorting },
    onColumnVisibilityChange: setColumnVisibility,
    getCoreRowModel: getCoreRowModel(),
    getFilteredRowModel: getFilteredRowModel(),
    getSortedRowModel: getSortedRowModel(),
    getExpandedRowModel: getExpandedRowModel(),
    getPaginationRowModel: getPaginationRowModel(),
    enableRowSelection: true,
    onGlobalFilterChange: setGlobalFilter,
    onSortingChange: setSorting,
  });

  const handleBulkDelete = () => {
    const selectedIds = tableInstance.getSelectedRowModel().rows.map((r) => r.original.id);
    setData((prev) => prev.filter((item) => !selectedIds.includes(item.id)));
    tableInstance.resetRowSelection();
    setSnackbarMsg(`Deleted ${selectedIds.length} order(s)`);
    setSnackbarOpen(true);
  };

  const filterPanelContent = (
    <Grid container spacing={2} sx={{ mt: 1 }}>
      <Grid size={{ xs: 12, sm: 6, md: 3 }}>
        <CustomTextField
          label="Order ID"
          variant="outlined"
          size="small"
          fullWidth
          value={columnFilters.id}
          onChange={(e: { target: { value: string } }) =>
            setColumnFilters((prev) => ({ ...prev, id: e.target.value }))
          }
        />
      </Grid>
      <Grid size={{ xs: 12, sm: 6, md: 3 }}>
        <CustomTextField
          label="Customer Name"
          variant="outlined"
          size="small"
          fullWidth
          value={columnFilters['customer.name']}
          onChange={(e: { target: { value: string } }) =>
            setColumnFilters((prev) => ({ ...prev, 'customer.name': e.target.value }))
          }
        />
      </Grid>
      <Grid size={{ xs: 12, sm: 6, md: 3 }}>
        <FormControl fullWidth size="small">
          <InputLabel>Status</InputLabel>
          <CustomSelect
            value={columnFilters.status}
            onChange={(e: { target: { value: string } }) =>
              setColumnFilters((prev) => ({ ...prev, status: e.target.value }))
            }
            label="Status"
          >
            <MenuItem value="">All</MenuItem>
            <MenuItem value="Pending">Pending</MenuItem>
            <MenuItem value="Shipped">Shipped</MenuItem>
            <MenuItem value="Completed">Completed</MenuItem>
            <MenuItem value="Cancelled">Cancelled</MenuItem>
            <MenuItem value="Processing">Processing</MenuItem>
          </CustomSelect>
        </FormControl>
      </Grid>
      <Grid size={{ xs: 12, sm: 6, md: 3 }}>
        <CustomTextField
          label="Amount"
          variant="outlined"
          size="small"
          fullWidth
          value={columnFilters.amount}
          onChange={(e: { target: { value: string } }) =>
            setColumnFilters((prev) => ({ ...prev, amount: e.target.value }))
          }
        />
      </Grid>
      <Grid size={{ xs: 12, sm: 6, md: 3 }}>
        <CustomTextField
          label="Address"
          variant="outlined"
          size="small"
          fullWidth
          value={columnFilters.address}
          onChange={(e: { target: { value: string } }) =>
            setColumnFilters((prev) => ({ ...prev, address: e.target.value }))
          }
        />
      </Grid>
      <Grid size={{ xs: 12, sm: 6, md: 3 }}>
        <LocalizationProvider dateAdapter={AdapterDayjs}>
          <DatePicker
            label="Filter Date"
            value={dateRange}
            onChange={(newValue) => setDateRange(newValue as Dayjs)}
            slotProps={{ textField: { size: 'small', variant: 'outlined', fullWidth: true } }}
          />
        </LocalizationProvider>
      </Grid>
    </Grid>
  );

  const renderExpandedRow = (row: OrderType) => (
    <>
      <Typography variant="subtitle1" fontWeight="bold">
        Customer orders:
      </Typography>
      <Table size="small">
        <TableHead>
          <TableRow>
            <TableCell>Name</TableCell>
            <TableCell>SKU</TableCell>
            <TableCell>Quantity</TableCell>
            <TableCell>Price</TableCell>
          </TableRow>
        </TableHead>
        <TableBody>
          {row.products.map((product, idx) => (
            <TableRow key={idx}>
              <TableCell>
                <Box display="flex" alignItems="center" gap={1}>
                  <Avatar
                    src={product.image}
                    alt={product.name}
                    variant="rounded"
                    sx={{ width: 30, height: 30 }}
                  />
                  <Typography variant="body2">{product.name}</Typography>
                </Box>
              </TableCell>
              <TableCell>{product.sku}</TableCell>
              <TableCell>{product.quantity}</TableCell>
              <TableCell>${product.price.toFixed(2)}</TableCell>
            </TableRow>
          ))}
        </TableBody>
      </Table>
      <Divider />
      <Box display="flex" justifyContent="flex-end" mt={3}>
        <Box>
          <Stack spacing={1}>
            <Box display="flex" justifyContent="space-between" minWidth={200}>
              <Typography variant="body2">Delivery Fee:</Typography>
              <Typography variant="body2" fontWeight="bold">
                $10.00
              </Typography>
            </Box>
            <Box display="flex" justifyContent="space-between" minWidth={200}>
              <Typography variant="body2">Tax:</Typography>
              <Typography variant="body2" fontWeight="bold">
                $5.00
              </Typography>
            </Box>
            <Box display="flex" justifyContent="space-between" minWidth={200}>
              <Typography variant="body1" color="primary">
                Total:
              </Typography>
              <Typography variant="body1" color="primary">
                $866.00
              </Typography>
            </Box>
          </Stack>
          <Stack direction="row" spacing={1} justifyContent="flex-end" mt={2}>
            <Button
              variant="contained"
              size="small"
              sx={{ px: 1, minWidth: 'auto', fontSize: 10, backgroundColor: 'grey.200', color: 'grey.900' }}
            >
              View
            </Button>
            <Button variant="contained" size="small" sx={{ px: 1, minWidth: 'auto', fontSize: 10 }}>
              Invoice
            </Button>
          </Stack>
        </Box>
      </Box>
    </>
  );

  return (
    <PageContainer title="Order Table" description="Order table with statistics, filters, and actions">
      <Breadcrumb title="Order Table" items={BCrumb} />
      <TableList<OrderType>
        title=""
        breadcrumbItems={[]}
        statistics={statsData}
        statisticsTitle="Orders History"
        statisticsVariant="compact"
        columns={placeholderColumns}
        rows={filteredData}
        getRowId={(r) => r.id}
        searchValue={globalFilter}
        onSearchChange={setGlobalFilter}
        searchPlaceholder="Search..."
        searchToggle
        filterPanel={{
          open: filterOpen,
          onToggle: () => setFilterOpen((prev) => !prev),
          content: filterPanelContent,
        }}
        onDownloadClick={handleDownload}
        tableTitle="Orders Table"
        tanStackTable={tableInstance}
        renderExpandedRow={renderExpandedRow}
        snackbar={{
          open: snackbarOpen,
          message: snackbarMsg,
          onClose: () => setSnackbarOpen(false),
        }}
        onBulkDelete={handleBulkDelete}
      />
    </PageContainer>
  );
}
