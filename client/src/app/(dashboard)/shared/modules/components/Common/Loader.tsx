export default function Loader() {
  return (
    <div id="preloader">
      <div id="status">
        <div className="spinner-border text-primary avatar-sm" role="status">
          <span className="visually-hidden">Loading...</span>
        </div>
      </div>
    </div>
  );
}
