"use client";

import BreadCrumb from "@/components/Common/BreadCrumb";
import CommonTable, { type CommonTableColumn } from "@/components/Common/CommonTable";
import DeleteModal from "@/components/Common/DeleteModal";
import FormModal from "@/components/Common/FormModal";
import { brandService } from "@/services/catalog/brandService";
import { useCallback, useEffect, useMemo, useRef, useState } from "react";

export default function CatalogBrandsPage() {
  type BrandRow = {
    id: string;
    name: string;
    description?: string | null;
    imageUrl?: string | null;
    isActive: boolean;
    createdTime?: string;
  };

  const [reloadToken, setReloadToken] = useState(0);
  const [error, setError] = useState<string | null>(null);
  const [data, setData] = useState<BrandRow[]>([]);
  const [loading, setLoading] = useState(false);
  const [currentPage, setCurrentPage] = useState(1);
  const [pageSize, setPageSize] = useState(10);
  const [totalCount, setTotalCount] = useState(0);
  const [searchTerm, setSearchTerm] = useState("");

  const [showForm, setShowForm] = useState(false);
  const [isSubmitting, setIsSubmitting] = useState(false);
  const [editingId, setEditingId] = useState<string | null>(null);

  const [name, setName] = useState("");
  const [description, setDescription] = useState("");
  const [imageUrl, setImageUrl] = useState("");
  const [isActive, setIsActive] = useState(true);

  const [showDelete, setShowDelete] = useState(false);
  const deleteIdRef = useRef<string | null>(null);

  const loadData = useCallback(async () => {
    setLoading(true);
    setError(null);
    try {
      const response = await brandService.getBrandPaginatedList(currentPage, pageSize, searchTerm || undefined, 0);
      setData((response?.data || []) as unknown as BrandRow[]);
      setTotalCount(response?.totalCount || 0);
    } catch {
      setError("Failed to load brands");
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

  const columns = useMemo<Array<CommonTableColumn<BrandRow>>>(() => {
    return [
      {
        key: "name",
        title: "Name",
        render: (_v, row) => {
          const img = row.imageUrl || "/assets/images/companies/img-1.png";
          return (
            <div className="d-flex align-items-center">
              <div className="flex-shrink-0 me-2">
                <img src={img} className="avatar-xxs rounded-circle" alt="" />
              </div>
              <div className="flex-grow-1">{row.name}</div>
            </div>
          );
        },
      },
      { key: "description", title: "Description", render: (v) => (v ? String(v) : "-") },
      {
        key: "isActive",
        title: "Status",
        render: (v) => {
          const active = Boolean(v);
          const badge = active ? "badge bg-soft-success text-success" : "badge bg-soft-danger text-danger";
          return <span className={badge}>{active ? "Active" : "Inactive"}</span>;
        },
      },
      {
        key: "createdTime",
        title: "Created",
        render: (v) => {
          if (!v) return "-";
          try {
            return new Date(String(v)).toLocaleDateString();
          } catch {
            return String(v);
          }
        },
      },
    ];
  }, []);

  const openCreate = () => {
    setEditingId(null);
    setName("");
    setDescription("");
    setImageUrl("");
    setIsActive(true);
    setShowForm(true);
  };

  const handleEdit = useCallback(
    async (row: BrandRow) => {
      try {
        setError(null);
        const b = (await brandService.getBrandById(row.id)) as unknown as BrandRow;
        setEditingId(row.id);
        setName(b.name || "");
        setDescription(b.description || "");
        setImageUrl(b.imageUrl || "");
        setIsActive(Boolean(b.isActive));
        setShowForm(true);
      } catch {
        setError("Failed to load brand");
      }
    },
    []
  );

  const handleDelete = useCallback((row: BrandRow) => {
    deleteIdRef.current = row.id;
    setShowDelete(true);
  }, []);

  const submit = async () => {
    if (!name.trim()) {
      setError("Name is required");
      return;
    }
    setIsSubmitting(true);
    setError(null);
    try {
      if (editingId) {
        await brandService.updateBrand({
          id: editingId,
          name: name.trim(),
          description: description.trim() || undefined,
          imageUrl: imageUrl.trim() || undefined,
          isActive,
        } as never);
      } else {
        await brandService.createBrand({
          name: name.trim(),
          description: description.trim() || undefined,
          imageUrl: imageUrl.trim() || undefined,
          isActive,
        } as never);
      }
      setShowForm(false);
      setReloadToken((v) => v + 1);
      setCurrentPage(1);
    } catch {
      setError("Failed to save brand");
    } finally {
      setIsSubmitting(false);
    }
  };

  const confirmDelete = async () => {
    const id = deleteIdRef.current;
    if (!id) return;
    setError(null);
    try {
      await brandService.deleteBrand(id);
      setShowDelete(false);
      deleteIdRef.current = null;
      setReloadToken((v) => v + 1);
      setCurrentPage(1);
    } catch {
      setError("Failed to delete brand");
    }
  };

  return (
    <div className="container-fluid">
      <BreadCrumb
        items={[
          { label: "Admin", icon: "ri-shield-user-line" },
          { label: "Brands", icon: "ri-price-tag-3-line" },
        ]}
      />

      {error ? (
        <div className="alert alert-danger" role="alert">
          {error}
        </div>
      ) : null}

      <div className="card">
        <div className="card-header">
          <h5 className="card-title mb-0">Brands</h5>
        </div>
        <div className="card-body">
          <CommonTable<BrandRow>
            columns={columns}
            data={data}
            loading={loading}
            searchable
            searchTerm={searchTerm}
            onSearchChange={handleSearchChange}
            searchPlaceholder="Search brands..."
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
            onAdd={openCreate}
            renderActions={(row) => (
              <div className="d-flex gap-2">
                <button
                  type="button"
                  className="btn btn-sm btn-soft-success"
                  onClick={() => handleEdit(row)}
                  title="Edit"
                >
                  <i className="ri-pencil-line"></i>
                </button>
                <button
                  type="button"
                  className="btn btn-sm btn-soft-danger"
                  onClick={() => handleDelete(row)}
                  title="Delete"
                >
                  <i className="ri-delete-bin-line"></i>
                </button>
              </div>
            )}
            emptyMessage="No brands found"
          />
        </div>
      </div>

      <FormModal
        show={showForm}
        title={editingId ? "Edit Brand" : "Add Brand"}
        submitText={editingId ? "Update" : "Create"}
        isSubmitting={isSubmitting}
        onClose={() => setShowForm(false)}
        onSubmit={submit}
      >
        <div className="mb-3">
          <label className="form-label">Name</label>
          <input className="form-control" value={name} onChange={(e) => setName(e.target.value)} />
        </div>
        <div className="mb-3">
          <label className="form-label">Description</label>
          <textarea className="form-control" rows={3} value={description} onChange={(e) => setDescription(e.target.value)} />
        </div>
        <div className="mb-3">
          <label className="form-label">Image URL</label>
          <input className="form-control" value={imageUrl} onChange={(e) => setImageUrl(e.target.value)} />
        </div>
        <div className="form-check">
          <input className="form-check-input" type="checkbox" id="brand-isActive" checked={isActive} onChange={(e) => setIsActive(e.target.checked)} />
          <label className="form-check-label" htmlFor="brand-isActive">
            Active
          </label>
        </div>
      </FormModal>

      <DeleteModal
        show={showDelete}
        onCloseClick={() => {
          setShowDelete(false);
          deleteIdRef.current = null;
        }}
        onDeleteClick={confirmDelete}
      />
    </div>
  );
}

