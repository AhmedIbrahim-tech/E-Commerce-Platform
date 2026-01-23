"use client";

export default function CustomizerButton() {
  const handleClick = () => {
    // Toggle the offcanvas using Bootstrap's Offcanvas API
    const offcanvasElement = document.getElementById("theme-settings-offcanvas");
    if (offcanvasElement) {
      // Check if Bootstrap is loaded
      if (typeof window !== "undefined" && (window as unknown as { bootstrap?: { Offcanvas: { getOrCreateInstance: (el: HTMLElement) => { toggle: () => void } } } }).bootstrap) {
        const bootstrap = (window as unknown as { bootstrap: { Offcanvas: { getOrCreateInstance: (el: HTMLElement) => { toggle: () => void } } } }).bootstrap;
        const bsOffcanvas = bootstrap.Offcanvas.getOrCreateInstance(offcanvasElement);
        bsOffcanvas.toggle();
      } else {
        // Fallback: manually toggle classes
        offcanvasElement.classList.toggle("show");
        document.body.classList.toggle("overflow-hidden");
        
        // Create or remove backdrop
        let backdrop = document.querySelector(".offcanvas-backdrop");
        if (offcanvasElement.classList.contains("show")) {
          if (!backdrop) {
            backdrop = document.createElement("div");
            backdrop.className = "offcanvas-backdrop fade show";
            backdrop.addEventListener("click", () => {
              offcanvasElement.classList.remove("show");
              document.body.classList.remove("overflow-hidden");
              backdrop?.remove();
            });
            document.body.appendChild(backdrop);
          }
        } else {
          backdrop?.remove();
        }
      }
    }
  };

  return (
    <div className="customizer-setting d-none d-md-block">
      <button
        type="button"
        onClick={handleClick}
        className="btn-info btn-rounded shadow-lg btn btn-icon btn-lg p-2"
        aria-label="Open customizer settings"
        data-bs-toggle="offcanvas"
        data-bs-target="#theme-settings-offcanvas"
      >
        <i className="mdi mdi-spin mdi-cog-outline fs-22"></i>
      </button>
    </div>
  );
}
