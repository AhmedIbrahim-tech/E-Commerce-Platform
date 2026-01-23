"use client";

import BreadCrumb from "@/components/Common/BreadCrumb";
import CommonTable, { type CommonTableColumn } from "@/components/Common/CommonTable";
import DeleteModal from "@/components/Common/DeleteModal";
import FormModal from "@/components/Common/FormModal";
import { variantAttributeService } from "@/services/catalog/variantAttributeService";
import { useCallback, useEffect, useMemo, useRef, useState } from "react";

export default function CatalogVariantAttributesPage() {
  type VariantAttributeRow = { id: string; name: string; description?: string | null; isActive: boolean; createdTime?: string };

  const [reloadToken, setReloadToken] = useState(0);
  const [error, setError] = useState<string | null>(null);
  const [data, setData] = useState<VariantAttributeRow[]>([]);
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
  const [isActive, setIsActive] = useState(true);

  const [showDelete, setShowDelete] = useState(false);
  const deleteIdRef = useRef<string | null>(null);

  const loadData = useCallback(async () => {
    setLoading(true);
    setError(null);
    try {
      const response = await variantAttributeService.getVariantAttributePaginatedList(currentPage, pageSize, searchTerm || undefined, 0);
      setData((response?.data || []) as unknown as VariantAttributeRow[]);
      setTotalCount(response?.totalCount || 0);
    } catch {
      setError("Failed to load variant attributes");
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

  const columns = useMemo<Array<CommonTableColumn<VariantAttributeRow>>>(() => {
    return [
      { key: "name", title: "Name" },
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
    setIsActive(true);
    setShowForm(true);
  };

  const handleEdit = useCallback(
    async (row: VariantAttributeRow) => {
      try {
        setError(null);
        const a = (await variantAttributeService.getVariantAttributeById(row.id)) as unknown as VariantAttributeRow;
        setEditingId(row.id);
        setName(a.name || "");
        setDescription(a.description || "");
        setIsActive(Boolean(a.isActive));
        setShowForm(true);
      } catch {
        setError("Failed to load variant attribute");
      }
    },
    []
  );

  const handleDelete = useCallback((row: VariantAttributeRow) => {
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
        await variantAttributeService.updateVariantAttribute({ id: editingId, name: name.trim(), description: description.trim() || undefined, isActive } as never);
      } else {
        await variantAttributeService.createVariantAttribute({ name: name.trim(), description: description.trim() || undefined, isActive } as never);
      }
      setShowForm(false);
      setReloadToken((v) => v + 1);
      setCurrentPage(1);
    } catch {
      setError("Failed to save variant attribute");
    } finally {
      setIsSubmitting(false);
    }
  };

  const confirmDelete = async () => {
    const id = deleteIdRef.current;
    if (!id) return;
    setError(null);
    try {
      await variantAttributeService.deleteVariantAttribute(id);
      setShowDelete(false);
      deleteIdRef.current = null;
      setReloadToken((v) => v + 1);
      setCurrentPage(1);
    } catch {
      setError("Failed to delete variant attribute");
    }
  };

  return (
    <div className="container-fluid">
      <BreadCrumb
        items={[
          { label: "Admin", icon: "ri-shield-user-line" },
          { label: "Variant Attributes", icon: "ri-list-check-2-line" },
        ]}
      />
      {error ? (
        <div className="alert alert-danger" role="alert">
          {error}
        </div>
      ) : null}
      <div className="card">
        <div className="card-header">
          <h5 className="card-title mb-0">Variant Attributes</h5>
        </div>
        <div className="card-body">
          <CommonTable<VariantAttributeRow>
            columns={columns}
            data={data}
            loading={loading}
            searchable
            searchTerm={searchTerm}
            onSearchChange={handleSearchChange}
            searchPlaceholder="Search variant attributes..."
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
            emptyMessage="No variant attributes found"
          />
        </div>
      </div>

      <FormModal show={showForm} title={editingId ? "Edit Variant Attribute" : "Add Variant Attribute"} submitText={editingId ? "Update" : "Create"} isSubmitting={isSubmitting} onClose={() => setShowForm(false)} onSubmit={submit}>
        <div className="mb-3">
          <label className="form-label">Name</label>
          <input className="form-control" value={name} onChange={(e) => setName(e.target.value)} />
        </div>
        <div className="mb-3">
          <label className="form-label">Description</label>
          <textarea className="form-control" rows={3} value={description} onChange={(e) => setDescription(e.target.value)} />
        </div>
        <div className="form-check">
          <input className="form-check-input" type="checkbox" id="variantAttribute-isActive" checked={isActive} onChange={(e) => setIsActive(e.target.checked)} />
          <label className="form-check-label" htmlFor="variantAttribute-isActive">
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

