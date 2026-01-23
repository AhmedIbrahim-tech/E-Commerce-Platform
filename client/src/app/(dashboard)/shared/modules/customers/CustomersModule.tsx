"use client";

import BreadCrumb from "@/components/Common/BreadCrumb";
import CommonTable, { type CommonTableColumn } from "@/components/Common/CommonTable";
import { customerService } from "@/services/users/customerService";
import { useCallback, useEffect, useMemo, useState } from "react";

export default function CustomersModule({
  title = "Customers",
  pageTitle = "Ecommerce",
}: {
  title?: string;
  pageTitle?: string;
}) {
  type CustomerRow = {
    id: string;
    fullName?: string | null;
    email?: string | null;
    phoneNumber?: string | null;
    gender?: number | null;
  };

  const [reloadToken, setReloadToken] = useState(0);
  const [error, setError] = useState<string | null>(null);
  const [data, setData] = useState<CustomerRow[]>([]);
  const [loading, setLoading] = useState(false);
  const [currentPage, setCurrentPage] = useState(1);
  const [pageSize, setPageSize] = useState(10);
  const [totalCount, setTotalCount] = useState(0);
  const [searchTerm, setSearchTerm] = useState("");

  const loadData = useCallback(async () => {
    setLoading(true);
    setError(null);
    try {
      const response = await customerService.getCustomerPaginatedList(currentPage, pageSize, searchTerm || undefined, 0);
      setData((response?.data || []) as unknown as CustomerRow[]);
      setTotalCount(response?.totalCount || 0);
    } catch {
      setError("Failed to load customers");
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

  const columns = useMemo<Array<CommonTableColumn<CustomerRow>>>(() => {
    return [
      { key: "fullName", title: "Full Name", render: (v) => (v ? String(v) : "-") },
      { key: "email", title: "Email", render: (v) => (v ? String(v) : "-") },
      { key: "phoneNumber", title: "Phone", render: (v) => (v ? String(v) : "-") },
      { key: "gender", title: "Gender", render: (v) => (v == null ? "-" : String(v)) },
    ];
  }, []);

  return (
    <div className="container-fluid">
      <BreadCrumb
        items={[
          { label: pageTitle, icon: "ri-store-line" },
          { label: title, icon: "ri-user-3-line" },
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
          <CommonTable<CustomerRow>
            columns={columns}
            data={data}
            loading={loading}
            searchable
            searchTerm={searchTerm}
            onSearchChange={handleSearchChange}
            searchPlaceholder="Search customers..."
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
            emptyMessage="No customers found"
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

