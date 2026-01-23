"use client";

import BreadCrumb from "@/components/Common/BreadCrumb";
import CommonTable, { type CommonTableColumn } from "@/components/Common/CommonTable";
import DeleteModal from "@/components/Common/DeleteModal";
import FormModal from "@/components/Common/FormModal";
import { lookupsService, type BaseLookup } from "@/services/lookups/lookupsService";
import { subCategoryService } from "@/services/catalog/subCategoryService";
import { useCallback, useEffect, useMemo, useRef, useState } from "react";

export default function CatalogSubCategoriesPage() {
  type SubCategoryRow = {
    id: string;
    name: string;
    description?: string | null;
    imageUrl?: string | null;
    code?: string | null;
    categoryId: string;
    categoryName?: string | null;
    isActive: boolean;
    createdTime?: string;
  };

  const [reloadToken, setReloadToken] = useState(0);
  const [error, setError] = useState<string | null>(null);
  const [data, setData] = useState<SubCategoryRow[]>([]);
  const [loading, setLoading] = useState(false);
  const [currentPage, setCurrentPage] = useState(1);
  const [pageSize, setPageSize] = useState(10);
  const [totalCount, setTotalCount] = useState(0);
  const [searchTerm, setSearchTerm] = useState("");

  const [categories, setCategories] = useState<BaseLookup[]>([]);
  const [categoryFilterId, setCategoryFilterId] = useState<string | "">("");

  const [showForm, setShowForm] = useState(false);
  const [isSubmitting, setIsSubmitting] = useState(false);
  const [editingId, setEditingId] = useState<string | null>(null);

  const [categoryId, setCategoryId] = useState("");
  const [name, setName] = useState("");
  const [description, setDescription] = useState("");
  const [code, setCode] = useState("");
  const [imageUrl, setImageUrl] = useState("");
  const [isActive, setIsActive] = useState(true);

  const [showDelete, setShowDelete] = useState(false);
  const deleteIdRef = useRef<string | null>(null);

  useEffect(() => {
    (async () => {
      try {
        const cats = await lookupsService.getCategories();
        setCategories(cats);
      } catch {
        setError("Failed to load categories lookup");
      }
    })();
  }, []);

  const loadData = useCallback(async () => {
    setLoading(true);
    setError(null);
    try {
      const response = await subCategoryService.getSubCategoryPaginatedList(
        currentPage,
        pageSize,
        searchTerm || undefined,
        categoryFilterId || undefined,
        0
      );
      setData((response?.data || []) as unknown as SubCategoryRow[]);
      setTotalCount(response?.totalCount || 0);
    } catch {
      setError("Failed to load sub-categories");
      setData([]);
      setTotalCount(0);
    } finally {
      setLoading(false);
    }
  }, [currentPage, pageSize, searchTerm, categoryFilterId]);

  useEffect(() => {
    loadData();
  }, [loadData, reloadToken]);

  const handleSearchChange = useCallback((term: string) => {
    setSearchTerm(term);
    setCurrentPage(1);
  }, []);

  const columns = useMemo<Array<CommonTableColumn<SubCategoryRow>>>(() => {
    return [
      { key: "name", title: "Name" },
      { key: "categoryName", title: "Category", render: (v) => (v ? String(v) : "-") },
      { key: "code", title: "Code", render: (v) => (v ? String(v) : "-") },
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
    setCategoryId(categories[0]?.id || "");
    setName("");
    setDescription("");
    setCode("");
    setImageUrl("");
    setIsActive(true);
    setShowForm(true);
  };

  const handleEdit = useCallback(
    async (row: SubCategoryRow) => {
      try {
        setError(null);
        const sc = (await subCategoryService.getSubCategoryById(row.id)) as unknown as SubCategoryRow;
        setEditingId(row.id);
        setCategoryId(sc.categoryId || "");
        setName(sc.name || "");
        setDescription(sc.description || "");
        setCode(sc.code || "");
        setImageUrl(sc.imageUrl || "");
        setIsActive(Boolean(sc.isActive));
        setShowForm(true);
      } catch {
        setError("Failed to load sub-category");
      }
    },
    []
  );

  const handleDelete = useCallback((row: SubCategoryRow) => {
    deleteIdRef.current = row.id;
    setShowDelete(true);
  }, []);

  const submit = async () => {
    if (!categoryId) {
      setError("Category is required");
      return;
    }
    if (!name.trim()) {
      setError("Name is required");
      return;
    }

    setIsSubmitting(true);
    setError(null);
    try {
      if (editingId) {
        await subCategoryService.updateSubCategory({
          id: editingId,
          categoryId,
          name: name.trim(),
          description: description.trim() || undefined,
          code: code.trim() || undefined,
          imageUrl: imageUrl.trim() || undefined,
          isActive,
        } as never);
      } else {
        await subCategoryService.createSubCategory({
          categoryId,
          name: name.trim(),
          description: description.trim() || undefined,
          code: code.trim() || undefined,
          imageUrl: imageUrl.trim() || undefined,
          isActive,
        } as never);
      }

      setShowForm(false);
      setReloadToken((v) => v + 1);
      setCurrentPage(1);
    } catch {
      setError("Failed to save sub-category");
    } finally {
      setIsSubmitting(false);
    }
  };

  const confirmDelete = async () => {
    const id = deleteIdRef.current;
    if (!id) return;
    setError(null);
    try {
      await subCategoryService.deleteSubCategory(id);
      setShowDelete(false);
      deleteIdRef.current = null;
      setReloadToken((v) => v + 1);
      setCurrentPage(1);
    } catch {
      setError("Failed to delete sub-category");
    }
  };

  return (
    <div className="container-fluid">
      <BreadCrumb
        items={[
          { label: "Admin", icon: "ri-shield-user-line" },
          { label: "Sub-Categories", icon: "ri-folder-2-line" },
        ]}
      />

      {error ? (
        <div className="alert alert-danger" role="alert">
          {error}
        </div>
      ) : null}

      <div className="card">
        <div className="card-header">
          <h5 className="card-title mb-0">Sub-Categories</h5>
        </div>
        <div className="card-body">
          <div className="row g-3 mb-3">
            <div className="col-sm-4">
              <label className="form-label">Filter by Category</label>
              <select
                className="form-select"
                value={categoryFilterId}
                onChange={(e) => {
                  setCategoryFilterId(e.target.value);
                  setCurrentPage(1);
                  setReloadToken((v) => v + 1);
                }}
              >
                <option value="">All</option>
                {categories.map((c) => (
                  <option key={c.id} value={c.id}>
                    {c.name}
                  </option>
                ))}
              </select>
            </div>
          </div>

          <CommonTable<SubCategoryRow>
            columns={columns}
            data={data}
            loading={loading}
            searchable
            searchTerm={searchTerm}
            onSearchChange={handleSearchChange}
            searchPlaceholder="Search sub-categories..."
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
            emptyMessage="No sub-categories found"
          />
        </div>
      </div>

      <FormModal show={showForm} title={editingId ? "Edit Sub-Category" : "Add Sub-Category"} submitText={editingId ? "Update" : "Create"} isSubmitting={isSubmitting} onClose={() => setShowForm(false)} onSubmit={submit}>
        <div className="mb-3">
          <label className="form-label">Category</label>
          <select className="form-select" value={categoryId} onChange={(e) => setCategoryId(e.target.value)}>
            <option value="" disabled>
              Select category
            </option>
            {categories.map((c) => (
              <option key={c.id} value={c.id}>
                {c.name}
              </option>
            ))}
          </select>
        </div>
        <div className="mb-3">
          <label className="form-label">Name</label>
          <input className="form-control" value={name} onChange={(e) => setName(e.target.value)} />
        </div>
        <div className="mb-3">
          <label className="form-label">Code</label>
          <input className="form-control" value={code} onChange={(e) => setCode(e.target.value)} />
        </div>
        <div className="mb-3">
          <label className="form-label">Image URL</label>
          <input className="form-control" value={imageUrl} onChange={(e) => setImageUrl(e.target.value)} />
        </div>
        <div className="mb-3">
          <label className="form-label">Description</label>
          <textarea className="form-control" rows={3} value={description} onChange={(e) => setDescription(e.target.value)} />
        </div>
        <div className="form-check">
          <input className="form-check-input" type="checkbox" id="subCategory-isActive" checked={isActive} onChange={(e) => setIsActive(e.target.checked)} />
          <label className="form-check-label" htmlFor="subCategory-isActive">
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

