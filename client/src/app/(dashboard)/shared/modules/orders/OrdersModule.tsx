"use client";

import BreadCrumb from "@/components/Common/BreadCrumb";
import CommonTable, { type CommonTableColumn } from "@/components/Common/CommonTable";
import { orderService } from "@/services/orders/orderService";
import { useCallback, useEffect, useMemo, useState } from "react";

export default function OrdersModule({ title = "Orders", pageTitle = "Ecommerce" }: { title?: string; pageTitle?: string }) {
  type OrderRow = {
    id: string;
    orderDate: string;
    orderStatus: number | string;
    totalAmount: number;
    customerName?: string | null;
    paymentMethod?: string | null;
    paymentStatus?: number | string | null;
    deliveryMethod?: string | null;
    deliveryStatus?: number | string | null;
  };

  const [reloadToken, setReloadToken] = useState(0);
  const [error, setError] = useState<string | null>(null);
  const [data, setData] = useState<OrderRow[]>([]);
  const [loading, setLoading] = useState(false);
  const [currentPage, setCurrentPage] = useState(1);
  const [pageSize, setPageSize] = useState(10);
  const [totalCount, setTotalCount] = useState(0);
  const [searchTerm, setSearchTerm] = useState("");

  const loadData = useCallback(async () => {
    setLoading(true);
    setError(null);
    try {
      const response = await orderService.getOrderPaginatedList(currentPage, pageSize, searchTerm || undefined, 0);
      setData((response?.data || []) as unknown as OrderRow[]);
      setTotalCount(response?.totalCount || 0);
    } catch {
      setError("Failed to load orders");
      setData([]);
      setTotalCount(0);
    } finally {
      setLoading(false);
    }
  }, [currentPage, pageSize, searchTerm]);

  useEffect(() => {
    loadData();
  }, [loadData, reloadToken]);

  const handleSearchChange = useCallback((term: string) => {
    setSearchTerm(term);
    setCurrentPage(1);
  }, []);

  const columns = useMemo<Array<CommonTableColumn<OrderRow>>>(() => {
    return [
      {
        key: "orderDate",
        title: "Order Date",
        render: (v) => {
          try {
            return new Date(String(v)).toLocaleString();
          } catch {
            return v ? String(v) : "-";
          }
        },
      },
      { key: "customerName", title: "Customer", render: (v) => (v ? String(v) : "-") },
      {
        key: "totalAmount",
        title: "Total",
        render: (v) => {
          const n = Number(v ?? 0);
          return `$${n.toFixed(2)}`;
        },
      },
      { key: "orderStatus", title: "Status", render: (v) => (v ? String(v) : "-") },
      { key: "paymentMethod", title: "Payment", render: (v) => (v ? String(v) : "-") },
      { key: "paymentStatus", title: "Payment Status", render: (v) => (v ? String(v) : "-") },
      { key: "deliveryMethod", title: "Delivery", render: (v) => (v ? String(v) : "-") },
      { key: "deliveryStatus", title: "Delivery Status", render: (v) => (v ? String(v) : "-") },
    ];
  }, []);

  return (
    <div className="container-fluid">
      <BreadCrumb
        items={[
          { label: pageTitle, icon: "ri-store-line" },
          { label: title, icon: "ri-shopping-cart-line" },
        ]}
      />
      {error ? (
        <div className="alert alert-danger" role="alert">
          {error}
        </div>
      ) : null}
      <div className="card">
        <div className="card-header">
          <h5 className="card-title mb-0">{title}</h5>
        </div>
        <div className="card-body">
          <CommonTable<OrderRow>
            columns={columns}
            data={data}
            loading={loading}
            searchable
            searchTerm={searchTerm}
            onSearchChange={handleSearchChange}
            searchPlaceholder="Search orders..."
            pagination={{
              currentPage,
              pageSize,
              total: totalCount,
              onPageChange: setCurrentPage,
              onPageSizeChange: (size) => {
                setPageSize(size);
                setCurrentPage(1);
              },
            }}
            showActions={false}
            emptyMessage="No orders found"
          />
          <div className="mt-2">
            <button type="button" className="btn btn-soft-secondary btn-sm" onClick={() => setReloadToken((v) => v + 1)}>
              Refresh
            </button>
          </div>
        </div>
      </div>
    </div>
  );
}

