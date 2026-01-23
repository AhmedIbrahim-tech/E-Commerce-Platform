"use client";

import BreadCrumb from "@/components/Common/BreadCrumb";
import CommonTable, { type CommonTableColumn } from "@/components/Common/CommonTable";
import DeleteModal from "@/components/Common/DeleteModal";
import FormModal from "@/components/Common/FormModal";
import { categoryService } from "@/services/catalog/categoryService";
import { useCallback, useEffect, useMemo, useRef, useState } from "react";

export default function CategoriesModule({
  title = "Categories",
  pageTitle = "Ecommerce",
}: {
  title?: string;
  pageTitle?: string;
}) {
  type CategoryRow = {
    id: string;
    name: string;
    description?: string | null;
  };

  const [reloadToken, setReloadToken] = useState(0);
  const [error, setError] = useState<string | null>(null);
  const [data, setData] = useState<CategoryRow[]>([]);
  const [loading, setLoading] = useState(false);
  const [currentPage, setCurrentPage] = useState(1);
  const [pageSize, setPageSize] = useState(10);
  const [totalCount, setTotalCount] = useState(0);
  const [searchTerm, setSearchTerm] = useState("");

  const handleSearchChange = useCallback((term: string) => {
    setSearchTerm(term);
    setCurrentPage(1);
  }, []);

  const [showForm, setShowForm] = useState(false);
  const [isSubmitting, setIsSubmitting] = useState(false);
  const [editingId, setEditingId] = useState<string | null>(null);
  const [name, setName] = useState("");
  const [description, setDescription] = useState("");

  const [showDelete, setShowDelete] = useState(false);
  const deleteIdRef = useRef<string | null>(null);

  const loadData = useCallback(async () => {
    setLoading(true);
    setError(null);
    try {
      const response = await categoryService.getCategoryPaginatedList(currentPage, pageSize, searchTerm || undefined, 0);
      setData((response?.data || []) as unknown as CategoryRow[]);
      setTotalCount(response?.totalCount || 0);
    } catch {
      setError("Failed to load categories");
      setData([]);
      setTotalCount(0);
    } finally {
      setLoading(false);
    }
  }, [currentPage, pageSize, searchTerm]);

  useEffect(() => {
    loadData();
  }, [loadData, reloadToken]);

  const columns = useMemo<Array<CommonTableColumn<CategoryRow>>>(() => {
    return [
      { key: "name", title: "Name" },
      { key: "description", title: "Description", render: (v) => (v ? String(v) : "-") },
    ];
  }, []);

  const openCreate = () => {
    setEditingId(null);
    setName("");
    setDescription("");
    setShowForm(true);
  };

  const handleEdit = useCallback(
    async (row: CategoryRow) => {
      try {
        setError(null);
        const cat = await categoryService.getCategoryById(row.id);
        setEditingId(row.id);
        setName((cat as unknown as CategoryRow).name || "");
        setDescription(((cat as unknown as CategoryRow).description as string | undefined) || "");
        setShowForm(true);
      } catch {
        setError("Failed to load category");
      }
    },
    []
  );

  const handleDelete = useCallback((row: CategoryRow) => {
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
        await categoryService.updateCategory({ id: editingId, name: name.trim(), description: description.trim() || undefined } as never);
      } else {
        await categoryService.createCategory({ name: name.trim(), description: description.trim() || undefined } as never);
      }
      setShowForm(false);
      setReloadToken((v) => v + 1);
      setCurrentPage(1);
    } catch {
      setError("Failed to save category");
    } finally {
      setIsSubmitting(false);
    }
  };

  const confirmDelete = async () => {
    const id = deleteIdRef.current;
    if (!id) return;
    setError(null);
    try {
      await categoryService.deleteCategory(id);
      setShowDelete(false);
      deleteIdRef.current = null;
      setReloadToken((v) => v + 1);
      setCurrentPage(1);
    } catch {
      setError("Failed to delete category");
    }
  };

  return (
    <div className="container-fluid">
      <BreadCrumb
        items={[
          { label: pageTitle, icon: "ri-store-line" },
          { label: title, icon: "ri-folder-line" },
        ]}
      />
      {error ? (
        <div className="alert alert-danger" role="alert">
          {error}
        </div>
      ) : null}
      <div className="card">
        <div className="card-header d-flex align-items-center justify-content-between">
          <h5 className="card-title mb-0">{title}</h5>
        </div>
        <div className="card-body">
          <CommonTable<CategoryRow>
            columns={columns}
            data={data}
            loading={loading}
            searchable
            searchTerm={searchTerm}
            onSearchChange={handleSearchChange}
            searchPlaceholder="Search categories..."
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
            emptyMessage="No categories found"
          />
        </div>
      </div>

      <FormModal
        show={showForm}
        title={editingId ? "Edit Category" : "Add Category"}
        submitText={editingId ? "Update" : "Create"}
        isSubmitting={isSubmitting}
        onClose={() => setShowForm(false)}
        onSubmit={submit}
      >
        <div className="mb-3">
          <label className="form-label">Name</label>
          <input className="form-control" value={name} onChange={(e) => setName(e.target.value)} />
        </div>
        <div className="mb-0">
          <label className="form-label">Description</label>
          <textarea className="form-control" rows={3} value={description} onChange={(e) => setDescription(e.target.value)} />
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

