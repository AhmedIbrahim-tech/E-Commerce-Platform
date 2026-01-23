interface DeleteModalProps {
  show?: boolean;
  onDeleteClick?: () => void;
  onCloseClick?: () => void;
}

export default function DeleteModal({
  show = false,
  onDeleteClick,
  onCloseClick,
}: DeleteModalProps) {
  return (
    <div
      className={`modal fade zoomIn ${show ? "show" : ""}`}
      id="deleteRecordModal"
      tabIndex={-1}
      aria-hidden="true"
      style={{ display: show ? "block" : "none" }}
    >
      <div className="modal-dialog modal-dialog-centered">
        <div className="modal-content">
          <div className="modal-header">
            <button
              type="button"
              className="btn-close"
              id="deleteRecord-close"
              aria-label="Close"
              onClick={onCloseClick}
            ></button>
          </div>
          <div className="modal-body p-5 text-center">
            <div className="avatar-lg mx-auto my-3">
              <div className="avatar-title bg-soft-danger text-danger fs-36 rounded-circle">
                <i className="ri-delete-bin-line"></i>
              </div>
            </div>
            <div className="mt-4 text-center">
              <h4 className="fs-semibold">You are about to delete a record ?</h4>
              <p className="text-muted fs-14 mb-4 pt-1">
                Deleting your record will remove all of your information from our
                database.
              </p>
              <div className="hstack gap-2 justify-content-center remove">
                <button
                  className="btn btn-link link-success fw-medium text-decoration-none"
                  id="deleteRecord-close"
                  onClick={onCloseClick}
                >
                  <i className="ri-close-line me-1 align-middle"></i> Close
                </button>
                <button
                  className="btn btn-danger"
                  id="delete-record"
                  onClick={onDeleteClick}
                >
                  Yes, Delete It!!
                </button>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
}
