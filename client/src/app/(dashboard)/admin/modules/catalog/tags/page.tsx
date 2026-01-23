"use client";

import BreadCrumb from "@/components/Common/BreadCrumb";
import CommonTable, { type CommonTableColumn } from "@/components/Common/CommonTable";
import DeleteModal from "@/components/Common/DeleteModal";
import FormModal from "@/components/Common/FormModal";
import { tagService } from "@/services/catalog/tagService";
import { useCallback, useEffect, useMemo, useRef, useState } from "react";

export default function CatalogTagsPage() {
  type TagRow = { id: string; name: string; isActive: boolean; createdTime?: string };

  const [rows, setRows] = useState<TagRow[]>([]);
  const [reloadToken, setReloadToken] = useState(0);
  const [error, setError] = useState<string | null>(null);

  const [showForm, setShowForm] = useState(false);
  const [isSubmitting, setIsSubmitting] = useState(false);
  const [editingId, setEditingId] = useState<string | null>(null);

  const [name, setName] = useState("");
  const [isActive, setIsActive] = useState(true);

  const [showDelete, setShowDelete] = useState(false);
  const deleteIdRef = useRef<string | null>(null);

  const load = useCallback(async () => {
    setError(null);
    try {
      const list = await tagService.getAllTags();
      setRows((list || []) as unknown as TagRow[]);
    } catch {
      setError("Failed to load tags");
      setRows([]);
    }
  }, []);

  useEffect(() => {
    void load();
  }, [load, reloadToken]);

  const columns = useMemo<Array<CommonTableColumn<TagRow>>>(() => {
    return [
      { key: "name", title: "Name" },
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
    setIsActive(true);
    setShowForm(true);
  };

  const handleEdit = useCallback((row: TagRow) => {
    setEditingId(row.id);
    setName(row.name || "");
    setIsActive(Boolean(row.isActive));
    setShowForm(true);
  }, []);

  const handleDelete = useCallback((row: TagRow) => {
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
      if (editingId) await tagService.updateTag({ id: editingId, name: name.trim(), isActive });
      else await tagService.createTag({ name: name.trim(), isActive });

      setShowForm(false);
      setReloadToken((v) => v + 1);
    } catch {
      setError("Failed to save tag");
    } finally {
      setIsSubmitting(false);
    }
  };

  const confirmDelete = async () => {
    const id = deleteIdRef.current;
    if (!id) return;
    setError(null);
    try {
      await tagService.deleteTag(id);
      setShowDelete(false);
      deleteIdRef.current = null;
      setReloadToken((v) => v + 1);
    } catch {
      setError("Failed to delete tag");
    }
  };

  return (
    <div className="container-fluid">
      <BreadCrumb
        items={[
          { label: "Admin", icon: "ri-shield-user-line" },
          { label: "Tags", icon: "ri-price-tag-line" },
        ]}
      />
      {error ? (
        <div className="alert alert-danger" role="alert">
          {error}
        </div>
      ) : null}

      <div className="card">
        <div className="card-header">
          <h5 className="card-title mb-0">Tags</h5>
        </div>
        <div className="card-body">
          <CommonTable<TagRow>
            columns={columns}
            data={rows}
            loading={false}
            searchable
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
            emptyMessage="No tags found"
          />
        </div>
      </div>

      <FormModal show={showForm} title={editingId ? "Edit Tag" : "Add Tag"} submitText={editingId ? "Update" : "Create"} isSubmitting={isSubmitting} onClose={() => setShowForm(false)} onSubmit={submit}>
        <div className="mb-3">
          <label className="form-label">Name</label>
          <input className="form-control" value={name} onChange={(e) => setName(e.target.value)} />
        </div>
        <div className="form-check">
          <input className="form-check-input" type="checkbox" id="tag-isActive" checked={isActive} onChange={(e) => setIsActive(e.target.checked)} />
          <label className="form-check-label" htmlFor="tag-isActive">
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

