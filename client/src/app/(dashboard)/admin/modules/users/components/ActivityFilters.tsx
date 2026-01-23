"use client";

interface ActivityFiltersProps {
  eventTypeFilter: string;
  startDate: string;
  endDate: string;
  onEventTypeChange: (value: string) => void;
  onStartDateChange: (value: string) => void;
  onEndDateChange: (value: string) => void;
  onBack?: () => void;
  showBackButton?: boolean;
}

export default function ActivityFilters({
  eventTypeFilter,
  startDate,
  endDate,
  onEventTypeChange,
  onStartDateChange,
  onEndDateChange,
  onBack,
  showBackButton = false,
}: ActivityFiltersProps) {
  return (
    <div className="d-flex gap-2 align-items-center">
      <select
        className="form-select form-select-sm"
        style={{ width: "auto" }}
        value={eventTypeFilter}
        onChange={(e) => onEventTypeChange(e.target.value)}
      >
        <option value="All">All Actions</option>
        <option value="create">Create</option>
        <option value="update">Update</option>
        <option value="delete">Delete</option>
        <option value="login">Login</option>
        <option value="status_change">Status Change</option>
      </select>
      <input
        type="date"
        className="form-control form-control-sm"
        style={{ width: "auto" }}
        value={startDate}
        onChange={(e) => onStartDateChange(e.target.value)}
        placeholder="Start Date"
      />
      <input
        type="date"
        className="form-control form-control-sm"
        style={{ width: "auto" }}
        value={endDate}
        onChange={(e) => onEndDateChange(e.target.value)}
        placeholder="End Date"
      />
      {showBackButton && onBack && (
        <button type="button" className="btn btn-soft-secondary btn-sm" onClick={onBack}>
          Back
        </button>
      )}
    </div>
  );
}
