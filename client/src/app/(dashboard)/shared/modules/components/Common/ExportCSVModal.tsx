interface ExportCSVModalProps {
  show?: boolean;
  onCloseClick?: () => void;
  data?: unknown[];
}

export default function ExportCSVModal({
  show = false,
  onCloseClick,
}: ExportCSVModalProps) {
  return (
    <div
      className={`modal fade ${show ? "show" : ""}`}
      id="exportCSVModal"
      tabIndex={-1}
      aria-hidden="true"
      style={{ display: show ? "block" : "none" }}
    >
      <div className="modal-dialog modal-dialog-centered">
        <div className="modal-content">
          <div className="modal-header">
            <h5 className="modal-title" id="exportCSVModalLabel">
              Export CSV
            </h5>
            <button
              type="button"
              className="btn-close"
              aria-label="Close"
              onClick={onCloseClick}
            ></button>
          </div>
          <div className="modal-body">
            <form>
              <div className="row g-3">
                <div className="col-xxl-6">
                  <div>
                    <label htmlFor="firstName" className="form-label">
                      First Name
                    </label>
                    <input
                      type="text"
                      className="form-control"
                      id="firstName"
                      placeholder="Enter first name"
                    />
                  </div>
                </div>
                <div className="col-xxl-6">
                  <div>
                    <label htmlFor="lastName" className="form-label">
                      Last Name
                    </label>
                    <input
                      type="text"
                      className="form-control"
                      id="lastName"
                      placeholder="Enter last name"
                    />
                  </div>
                </div>
                <div className="col-lg-12">
                  <div>
                    <label htmlFor="companyName" className="form-label">
                      Company Name
                    </label>
                    <input
                      type="text"
                      className="form-control"
                      id="companyName"
                      placeholder="Enter company name"
                    />
                  </div>
                </div>
                <div className="col-lg-12">
                  <label htmlFor="checkAll" className="form-label">
                    Choose Fields
                  </label>
                  <div className="form-check">
                    <input
                      className="form-check-input"
                      type="checkbox"
                      id="checkAll"
                      defaultValue="option"
                    />
                    <label className="form-check-label" htmlFor="checkAll">
                      Check All
                    </label>
                  </div>
                  <div className="form-check">
                    <input
                      className="form-check-input"
                      type="checkbox"
                      id="firstName1"
                      defaultValue="First Name"
                    />
                    <label className="form-check-label" htmlFor="firstName1">
                      First Name
                    </label>
                  </div>
                  <div className="form-check">
                    <input
                      className="form-check-input"
                      type="checkbox"
                      id="lastName1"
                      defaultValue="Last Name"
                    />
                    <label className="form-check-label" htmlFor="lastName1">
                      Last Name
                    </label>
                  </div>
                  <div className="form-check">
                    <input
                      className="form-check-input"
                      type="checkbox"
                      id="companyName1"
                      defaultValue="Company Name"
                    />
                    <label className="form-check-label" htmlFor="companyName1">
                      Company Name
                    </label>
                  </div>
                </div>
                <div className="col-lg-12">
                  <div className="hstack gap-2 justify-content-end">
                    <button
                      type="button"
                      className="btn btn-light"
                      onClick={onCloseClick}
                    >
                      Close
                    </button>
                    <button type="submit" className="btn btn-success">
                      Export
                    </button>
                  </div>
                </div>
              </div>
            </form>
          </div>
        </div>
      </div>
    </div>
  );
}
