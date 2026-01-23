"use client";

import BreadCrumb from "@/components/Common/BreadCrumb";
import CommonTable, { type CommonTableColumn } from "@/components/Common/CommonTable";
import DeleteModal from "@/components/Common/DeleteModal";
import FormModal from "@/components/Common/FormModal";
import { warrantyService } from "@/services/catalog/warrantyService";
import { useCallback, useEffect, useMemo, useRef, useState } from "react";

export default function CatalogWarrantiesPage() {
  type WarrantyRow = {
    id: string;
    name: string;
    description?: string | null;
    duration: number;
    durationPeriod: string;
    isActive: boolean;
  };

  const [rows, setRows] = useState<WarrantyRow[]>([]);
  const [reloadToken, setReloadToken] = useState(0);
  const [error, setError] = useState<string | null>(null);

  const [showForm, setShowForm] = useState(false);
  const [isSubmitting, setIsSubmitting] = useState(false);
  const [editingId, setEditingId] = useState<string | null>(null);

  const [name, setName] = useState("");
  const [description, setDescription] = useState("");
  const [duration, setDuration] = useState<number>(12);
  const [durationPeriod, setDurationPeriod] = useState("Month");
  const [isActive, setIsActive] = useState(true);

  const [showDelete, setShowDelete] = useState(false);
  const deleteIdRef = useRef<string | null>(null);

  const load = useCallback(async () => {
    setError(null);
    try {
      const list = await warrantyService.getAllWarranties();
      setRows((list || []) as unknown as WarrantyRow[]);
    } catch {
      setError("Failed to load warranties");
      setRows([]);
    }
  }, []);

  useEffect(() => {
    void load();
  }, [load, reloadToken]);

  const columns = useMemo<Array<CommonTableColumn<WarrantyRow>>>(() => {
    return [
      { key: "name", title: "Name" },
      { key: "duration", title: "Duration", render: (v) => (v ? String(v) : "-") },
      { key: "durationPeriod", title: "Period", render: (v) => (v ? String(v) : "-") },
      {
        key: "isActive",
        title: "Status",
        render: (v) => {
          const active = Boolean(v);
          const badge = active ? "badge bg-soft-success text-success" : "badge bg-soft-danger text-danger";
          return <span className={badge}>{active ? "Active" : "Inactive"}</span>;
        },
      },
    ];
  }, []);

  const openCreate = () => {
    setEditingId(null);
    setName("");
    setDescription("");
    setDuration(12);
    setDurationPeriod("Month");
    setIsActive(true);
    setShowForm(true);
  };

  const handleEdit = useCallback(
    async (row: WarrantyRow) => {
      try {
        setError(null);
        const w = (await warrantyService.getWarrantyById(row.id)) as unknown as WarrantyRow;
        setEditingId(row.id);
        setName(w.name || "");
        setDescription(w.description || "");
        setDuration(Number(w.duration ?? 0));
        setDurationPeriod(w.durationPeriod || "Month");
        setIsActive(Boolean(w.isActive));
        setShowForm(true);
      } catch {
        setError("Failed to load warranty");
      }
    },
    []
  );

  const handleDelete = useCallback((row: WarrantyRow) => {
    deleteIdRef.current = row.id;
    setShowDelete(true);
  }, []);

  const submit = async () => {
    if (!name.trim()) {
      setError("Name is required");
      return;
    }
    if (!Number.isFinite(duration) || duration <= 0) {
      setError("Duration must be a positive number");
      return;
    }

    setIsSubmitting(true);
    setError(null);
    try {
      if (editingId) {
        await warrantyService.updateWarranty({ id: editingId, name: name.trim(), description: description.trim() || undefined, duration, durationPeriod, isActive } as never);
      } else {
        await warrantyService.createWarranty({ name: name.trim(), description: description.trim() || undefined, duration, durationPeriod, isActive } as never);
      }
      setShowForm(false);
      setReloadToken((v) => v + 1);
    } catch {
      setError("Failed to save warranty");
    } finally {
      setIsSubmitting(false);
    }
  };

  const confirmDelete = async () => {
    const id = deleteIdRef.current;
    if (!id) return;
    setError(null);
    try {
      await warrantyService.deleteWarranty(id);
      setShowDelete(false);
      deleteIdRef.current = null;
      setReloadToken((v) => v + 1);
    } catch {
      setError("Failed to delete warranty");
    }
  };

  return (
    <div className="container-fluid">
      <BreadCrumb
        items={[
          { label: "Admin", icon: "ri-shield-user-line" },
          { label: "Warranties", icon: "ri-shield-check-line" },
        ]}
      />
      {error ? (
        <div className="alert alert-danger" role="alert">
          {error}
        </div>
      ) : null}

      <div className="card">
        <div className="card-header">
          <h5 className="card-title mb-0">Warranties</h5>
        </div>
        <div className="card-body">
          <CommonTable<WarrantyRow>
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
            emptyMessage="No warranties found"
          />
        </div>
      </div>

      <FormModal show={showForm} title={editingId ? "Edit Warranty" : "Add Warranty"} submitText={editingId ? "Update" : "Create"} isSubmitting={isSubmitting} onClose={() => setShowForm(false)} onSubmit={submit}>
        <div className="mb-3">
          <label className="form-label">Name</label>
          <input className="form-control" value={name} onChange={(e) => setName(e.target.value)} />
        </div>
        <div className="mb-3">
          <label className="form-label">Description</label>
          <textarea className="form-control" rows={3} value={description} onChange={(e) => setDescription(e.target.value)} />
        </div>
        <div className="row">
          <div className="col-sm-6 mb-3">
            <label className="form-label">Duration</label>
            <input className="form-control" type="number" min={1} value={duration} onChange={(e) => setDuration(Number(e.target.value))} />
          </div>
          <div className="col-sm-6 mb-3">
            <label className="form-label">Period</label>
            <select className="form-select" value={durationPeriod} onChange={(e) => setDurationPeriod(e.target.value)}>
              {["Day", "Week", "Month", "Year"].map((p) => (
                <option key={p} value={p}>
                  {p}
                </option>
              ))}
            </select>
          </div>
        </div>
        <div className="form-check">
          <input className="form-check-input" type="checkbox" id="warranty-isActive" checked={isActive} onChange={(e) => setIsActive(e.target.checked)} />
          <label className="form-check-label" htmlFor="warranty-isActive">
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

