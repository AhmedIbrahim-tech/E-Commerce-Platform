"use client";

import { useEffect, useCallback } from "react";

export default function RightSidebar() {
  const handleClose = useCallback(() => {
    const offcanvasElement = document.getElementById("theme-settings-offcanvas");
    if (offcanvasElement) {
      offcanvasElement.classList.remove("show");
      document.body.classList.remove("overflow-hidden");
      const backdrop = document.querySelector(".offcanvas-backdrop");
      backdrop?.remove();
    }
  }, []);

  const handleLayoutChange = (attribute: string, value: string) => {
    document.documentElement.setAttribute(attribute, value);
  };

  const handleReset = () => {
    // Reset to default values
    handleLayoutChange("data-layout", "vertical");
    handleLayoutChange("data-sidebar", "dark");
    handleLayoutChange("data-sidebar-size", "lg");
    handleLayoutChange("data-layout-mode", "light");
    handleLayoutChange("data-topbar", "light");
    handleLayoutChange("data-layout-width", "fluid");
    handleLayoutChange("data-layout-position", "fixed");
    handleLayoutChange("data-layout-style", "default");
    handleLayoutChange("data-sidebar-image", "none");
  };

  useEffect(() => {
    // Handle radio button changes
    const handleRadioChange = (e: Event) => {
      const target = e.target as HTMLInputElement;
      if (target.type === "radio" && target.name) {
        handleLayoutChange(target.name, target.value);
      }
    };

    const offcanvasElement = document.getElementById("theme-settings-offcanvas");
    offcanvasElement?.addEventListener("change", handleRadioChange);

    return () => {
      offcanvasElement?.removeEventListener("change", handleRadioChange);
    };
  }, []);

  return (
    <div
      className="offcanvas offcanvas-end border-0"
      tabIndex={-1}
      id="theme-settings-offcanvas"
      aria-labelledby="theme-settings-label"
    >
      <div className="d-flex align-items-center bg-primary bg-gradient p-3 offcanvas-header">
        <h5 className="m-0 me-2 text-white" id="theme-settings-label">Theme Customizer</h5>
        <button
          type="button"
          className="btn-close btn-close-white ms-auto"
          aria-label="Close"
          onClick={handleClose}
        ></button>
      </div>
      <div className="offcanvas-body p-0">
        <div className="h-100" style={{ overflowY: "auto" }}>
          <div className="p-4">
            {/* Layout */}
            <h6 className="mb-0 fw-bold text-uppercase">Layout</h6>
            <p className="text-muted">Choose your layout</p>

            <div className="row">
              <div className="col-4">
                <div className="form-check card-radio">
                  <input
                    id="customizer-layout01"
                    name="data-layout"
                    type="radio"
                    value="vertical"
                    className="form-check-input"
                    defaultChecked
                    onChange={(e) => handleLayoutChange("data-layout", e.target.value)}
                  />
                  <label
                    className="form-check-label p-0 avatar-md w-100"
                    htmlFor="customizer-layout01"
                  >
                    <span className="d-flex gap-1 h-100">
                      <span className="flex-shrink-0">
                        <span className="bg-light d-flex h-100 flex-column gap-1 p-1">
                          <span className="d-block p-1 px-2 bg-primary-subtle rounded mb-2"></span>
                          <span className="d-block p-1 px-2 pb-0 bg-primary-subtle"></span>
                          <span className="d-block p-1 px-2 pb-0 bg-primary-subtle"></span>
                          <span className="d-block p-1 px-2 pb-0 bg-primary-subtle"></span>
                        </span>
                      </span>
                      <span className="flex-grow-1">
                        <span className="d-flex h-100 flex-column">
                          <span className="bg-light d-block p-1"></span>
                          <span className="bg-light d-block p-1 mt-auto"></span>
                        </span>
                      </span>
                    </span>
                  </label>
                </div>
                <h5 className="fs-13 text-center mt-2">Vertical</h5>
              </div>
              <div className="col-4">
                <div className="form-check card-radio">
                  <input
                    id="customizer-layout02"
                    name="data-layout"
                    type="radio"
                    value="horizontal"
                    className="form-check-input"
                    onChange={(e) => handleLayoutChange("data-layout", e.target.value)}
                  />
                  <label
                    className="form-check-label p-0 avatar-md w-100"
                    htmlFor="customizer-layout02"
                  >
                    <span className="d-flex h-100 flex-column gap-1">
                      <span className="bg-light d-flex p-1 gap-1 align-items-center">
                        <span className="d-block p-1 bg-primary-subtle rounded me-1"></span>
                        <span className="d-block p-1 pb-0 px-2 bg-primary-subtle ms-auto"></span>
                        <span className="d-block p-1 pb-0 px-2 bg-primary-subtle"></span>
                      </span>
                      <span className="bg-light d-block p-1"></span>
                      <span className="bg-light d-block p-1 mt-auto"></span>
                    </span>
                  </label>
                </div>
                <h5 className="fs-13 text-center mt-2">Horizontal</h5>
              </div>
              <div className="col-4">
                <div className="form-check card-radio">
                  <input
                    id="customizer-layout03"
                    name="data-layout"
                    type="radio"
                    value="twocolumn"
                    className="form-check-input"
                    onChange={(e) => handleLayoutChange("data-layout", e.target.value)}
                  />
                  <label
                    className="form-check-label p-0 avatar-md w-100"
                    htmlFor="customizer-layout03"
                  >
                    <span className="d-flex gap-1 h-100">
                      <span className="flex-shrink-0">
                        <span className="bg-light d-flex h-100 flex-column gap-1">
                          <span className="d-block p-1 bg-primary-subtle mb-2"></span>
                          <span className="d-block p-1 pb-0 bg-primary-subtle"></span>
                          <span className="d-block p-1 pb-0 bg-primary-subtle"></span>
                          <span className="d-block p-1 pb-0 bg-primary-subtle"></span>
                        </span>
                      </span>
                      <span className="flex-shrink-0">
                        <span className="bg-light d-flex h-100 flex-column gap-1 p-1">
                          <span className="d-block p-1 px-2 pb-0 bg-primary-subtle"></span>
                          <span className="d-block p-1 px-2 pb-0 bg-primary-subtle"></span>
                          <span className="d-block p-1 px-2 pb-0 bg-primary-subtle"></span>
                          <span className="d-block p-1 px-2 pb-0 bg-primary-subtle"></span>
                        </span>
                      </span>
                      <span className="flex-grow-1">
                        <span className="d-flex h-100 flex-column">
                          <span className="bg-light d-block p-1"></span>
                          <span className="bg-light d-block p-1 mt-auto"></span>
                        </span>
                      </span>
                    </span>
                  </label>
                </div>
                <h5 className="fs-13 text-center mt-2">Two Column</h5>
              </div>
            </div>

            {/* Color Scheme */}
            <h6 className="mt-4 mb-0 fw-bold text-uppercase">Color Scheme</h6>
            <p className="text-muted">Choose Light or Dark Scheme.</p>

            <div className="colorscheme-cardradio">
              <div className="row">
                <div className="col-4">
                  <div className="form-check card-radio">
                    <input
                      className="form-check-input"
                      type="radio"
                      name="data-layout-mode"
                      id="layout-mode-light"
                      value="light"
                      defaultChecked
                      onChange={(e) => handleLayoutChange("data-layout-mode", e.target.value)}
                    />
                    <label
                      className="form-check-label p-0 avatar-md w-100"
                      htmlFor="layout-mode-light"
                    >
                      <span className="d-flex gap-1 h-100">
                        <span className="flex-shrink-0">
                          <span className="bg-light d-flex h-100 flex-column gap-1 p-1">
                            <span className="d-block p-1 px-2 bg-primary-subtle rounded mb-2"></span>
                            <span className="d-block p-1 px-2 pb-0 bg-primary-subtle"></span>
                            <span className="d-block p-1 px-2 pb-0 bg-primary-subtle"></span>
                            <span className="d-block p-1 px-2 pb-0 bg-primary-subtle"></span>
                          </span>
                        </span>
                        <span className="flex-grow-1">
                          <span className="d-flex h-100 flex-column">
                            <span className="bg-light d-block p-1"></span>
                            <span className="bg-light d-block p-1 mt-auto"></span>
                          </span>
                        </span>
                      </span>
                    </label>
                  </div>
                  <h5 className="fs-13 text-center mt-2">Light</h5>
                </div>

                <div className="col-4">
                  <div className="form-check card-radio dark">
                    <input
                      className="form-check-input"
                      type="radio"
                      name="data-layout-mode"
                      id="layout-mode-dark"
                      value="dark"
                      onChange={(e) => handleLayoutChange("data-layout-mode", e.target.value)}
                    />
                    <label
                      className="form-check-label p-0 avatar-md w-100 bg-dark"
                      htmlFor="layout-mode-dark"
                    >
                      <span className="d-flex gap-1 h-100">
                        <span className="flex-shrink-0">
                          <span className="bg-light bg-opacity-10 d-flex h-100 flex-column gap-1 p-1">
                            <span className="d-block p-1 px-2 bg-light bg-opacity-10 rounded mb-2"></span>
                            <span className="d-block p-1 px-2 pb-0 bg-light bg-opacity-10"></span>
                            <span className="d-block p-1 px-2 pb-0 bg-light bg-opacity-10"></span>
                            <span className="d-block p-1 px-2 pb-0 bg-light bg-opacity-10"></span>
                          </span>
                        </span>
                        <span className="flex-grow-1">
                          <span className="d-flex h-100 flex-column">
                            <span className="bg-light bg-opacity-10 d-block p-1"></span>
                            <span className="bg-light bg-opacity-10 d-block p-1 mt-auto"></span>
                          </span>
                        </span>
                      </span>
                    </label>
                  </div>
                  <h5 className="fs-13 text-center mt-2">Dark</h5>
                </div>
              </div>
            </div>

            {/* Layout Width */}
            <div id="layout-width">
              <h6 className="mt-4 mb-0 fw-bold text-uppercase">Layout Width</h6>
              <p className="text-muted">Choose Fluid or Boxed layout.</p>

              <div className="row">
                <div className="col-4">
                  <div className="form-check card-radio">
                    <input
                      className="form-check-input"
                      type="radio"
                      name="data-layout-width"
                      id="layout-width-fluid"
                      value="fluid"
                      defaultChecked
                      onChange={(e) => handleLayoutChange("data-layout-width", e.target.value)}
                    />
                    <label
                      className="form-check-label p-0 avatar-md w-100"
                      htmlFor="layout-width-fluid"
                    >
                      <span className="d-flex gap-1 h-100">
                        <span className="flex-shrink-0">
                          <span className="bg-light d-flex h-100 flex-column gap-1 p-1">
                            <span className="d-block p-1 px-2 bg-primary-subtle rounded mb-2"></span>
                            <span className="d-block p-1 px-2 pb-0 bg-primary-subtle"></span>
                            <span className="d-block p-1 px-2 pb-0 bg-primary-subtle"></span>
                            <span className="d-block p-1 px-2 pb-0 bg-primary-subtle"></span>
                          </span>
                        </span>
                        <span className="flex-grow-1">
                          <span className="d-flex h-100 flex-column">
                            <span className="bg-light d-block p-1"></span>
                            <span className="bg-light d-block p-1 mt-auto"></span>
                          </span>
                        </span>
                      </span>
                    </label>
                  </div>
                  <h5 className="fs-13 text-center mt-2">Fluid</h5>
                </div>
                <div className="col-4">
                  <div className="form-check card-radio">
                    <input
                      className="form-check-input"
                      type="radio"
                      name="data-layout-width"
                      id="layout-width-boxed"
                      value="boxed"
                      onChange={(e) => handleLayoutChange("data-layout-width", e.target.value)}
                    />
                    <label
                      className="form-check-label p-0 avatar-md w-100 px-2"
                      htmlFor="layout-width-boxed"
                    >
                      <span className="d-flex gap-1 h-100 border-start border-end">
                        <span className="flex-shrink-0">
                          <span className="bg-light d-flex h-100 flex-column gap-1 p-1">
                            <span className="d-block p-1 px-2 bg-primary-subtle rounded mb-2"></span>
                            <span className="d-block p-1 px-2 pb-0 bg-primary-subtle"></span>
                            <span className="d-block p-1 px-2 pb-0 bg-primary-subtle"></span>
                            <span className="d-block p-1 px-2 pb-0 bg-primary-subtle"></span>
                          </span>
                        </span>
                        <span className="flex-grow-1">
                          <span className="d-flex h-100 flex-column">
                            <span className="bg-light d-block p-1"></span>
                            <span className="bg-light d-block p-1 mt-auto"></span>
                          </span>
                        </span>
                      </span>
                    </label>
                  </div>
                  <h5 className="fs-13 text-center mt-2">Boxed</h5>
                </div>
              </div>
            </div>

            {/* Layout Position */}
            <div id="layout-position">
              <h6 className="mt-4 mb-0 fw-bold text-uppercase">Layout Position</h6>
              <p className="text-muted">Choose Fixed or Scrollable Layout Position.</p>

              <div className="btn-group radio" role="group">
                <input
                  type="radio"
                  className="btn-check"
                  name="data-layout-position"
                  id="layout-position-fixed"
                  value="fixed"
                  defaultChecked
                  onChange={(e) => handleLayoutChange("data-layout-position", e.target.value)}
                />
                <label className="btn btn-light w-sm" htmlFor="layout-position-fixed">
                  Fixed
                </label>

                <input
                  type="radio"
                  className="btn-check"
                  name="data-layout-position"
                  id="layout-position-scrollable"
                  value="scrollable"
                  onChange={(e) => handleLayoutChange("data-layout-position", e.target.value)}
                />
                <label
                  className="btn btn-light w-sm ms-0"
                  htmlFor="layout-position-scrollable"
                >
                  Scrollable
                </label>
              </div>
            </div>

            {/* Topbar Color */}
            <h6 className="mt-4 mb-0 fw-bold text-uppercase">Topbar Color</h6>
            <p className="text-muted">Choose Light or Dark Topbar Color.</p>

            <div className="row">
              <div className="col-4">
                <div className="form-check card-radio">
                  <input
                    className="form-check-input"
                    type="radio"
                    name="data-topbar"
                    id="topbar-color-light"
                    value="light"
                    defaultChecked
                    onChange={(e) => handleLayoutChange("data-topbar", e.target.value)}
                  />
                  <label
                    className="form-check-label p-0 avatar-md w-100"
                    htmlFor="topbar-color-light"
                  >
                    <span className="d-flex gap-1 h-100">
                      <span className="flex-shrink-0">
                        <span className="bg-light d-flex h-100 flex-column gap-1 p-1">
                          <span className="d-block p-1 px-2 bg-primary-subtle rounded mb-2"></span>
                          <span className="d-block p-1 px-2 pb-0 bg-primary-subtle"></span>
                          <span className="d-block p-1 px-2 pb-0 bg-primary-subtle"></span>
                          <span className="d-block p-1 px-2 pb-0 bg-primary-subtle"></span>
                        </span>
                      </span>
                      <span className="flex-grow-1">
                        <span className="d-flex h-100 flex-column">
                          <span className="bg-light d-block p-1"></span>
                          <span className="bg-light d-block p-1 mt-auto"></span>
                        </span>
                      </span>
                    </span>
                  </label>
                </div>
                <h5 className="fs-13 text-center mt-2">Light</h5>
              </div>
              <div className="col-4">
                <div className="form-check card-radio">
                  <input
                    className="form-check-input"
                    type="radio"
                    name="data-topbar"
                    id="topbar-color-dark"
                    value="dark"
                    onChange={(e) => handleLayoutChange("data-topbar", e.target.value)}
                  />
                  <label
                    className="form-check-label p-0 avatar-md w-100"
                    htmlFor="topbar-color-dark"
                  >
                    <span className="d-flex gap-1 h-100">
                      <span className="flex-shrink-0">
                        <span className="bg-light d-flex h-100 flex-column gap-1 p-1">
                          <span className="d-block p-1 px-2 bg-primary-subtle rounded mb-2"></span>
                          <span className="d-block p-1 px-2 pb-0 bg-primary-subtle"></span>
                          <span className="d-block p-1 px-2 pb-0 bg-primary-subtle"></span>
                          <span className="d-block p-1 px-2 pb-0 bg-primary-subtle"></span>
                        </span>
                      </span>
                      <span className="flex-grow-1">
                        <span className="d-flex h-100 flex-column">
                          <span className="bg-primary d-block p-1"></span>
                          <span className="bg-light d-block p-1 mt-auto"></span>
                        </span>
                      </span>
                    </span>
                  </label>
                </div>
                <h5 className="fs-13 text-center mt-2">Dark</h5>
              </div>
            </div>

            {/* Sidebar Size */}
            <div id="sidebar-size">
              <h6 className="mt-4 mb-0 fw-bold text-uppercase">Sidebar Size</h6>
              <p className="text-muted">Choose a size of Sidebar.</p>

              <div className="row">
                <div className="col-4">
                  <div className="form-check sidebar-setting card-radio">
                    <input
                      className="form-check-input"
                      type="radio"
                      name="data-sidebar-size"
                      id="sidebar-size-default"
                      value="lg"
                      defaultChecked
                      onChange={(e) => handleLayoutChange("data-sidebar-size", e.target.value)}
                    />
                    <label
                      className="form-check-label p-0 avatar-md w-100"
                      htmlFor="sidebar-size-default"
                    >
                      <span className="d-flex gap-1 h-100">
                        <span className="flex-shrink-0">
                          <span className="bg-light d-flex h-100 flex-column gap-1 p-1">
                            <span className="d-block p-1 px-2 bg-primary-subtle rounded mb-2"></span>
                            <span className="d-block p-1 px-2 pb-0 bg-primary-subtle"></span>
                            <span className="d-block p-1 px-2 pb-0 bg-primary-subtle"></span>
                            <span className="d-block p-1 px-2 pb-0 bg-primary-subtle"></span>
                          </span>
                        </span>
                        <span className="flex-grow-1">
                          <span className="d-flex h-100 flex-column">
                            <span className="bg-light d-block p-1"></span>
                            <span className="bg-light d-block p-1 mt-auto"></span>
                          </span>
                        </span>
                      </span>
                    </label>
                  </div>
                  <h5 className="fs-13 text-center mt-2">Default</h5>
                </div>

                <div className="col-4">
                  <div className="form-check sidebar-setting card-radio">
                    <input
                      className="form-check-input"
                      type="radio"
                      name="data-sidebar-size"
                      id="sidebar-size-compact"
                      value="md"
                      onChange={(e) => handleLayoutChange("data-sidebar-size", e.target.value)}
                    />
                    <label
                      className="form-check-label p-0 avatar-md w-100"
                      htmlFor="sidebar-size-compact"
                    >
                      <span className="d-flex gap-1 h-100">
                        <span className="flex-shrink-0">
                          <span className="bg-light d-flex h-100 flex-column gap-1 p-1">
                            <span className="d-block p-1 bg-primary-subtle rounded mb-2"></span>
                            <span className="d-block p-1 pb-0 bg-primary-subtle"></span>
                            <span className="d-block p-1 pb-0 bg-primary-subtle"></span>
                            <span className="d-block p-1 pb-0 bg-primary-subtle"></span>
                          </span>
                        </span>
                        <span className="flex-grow-1">
                          <span className="d-flex h-100 flex-column">
                            <span className="bg-light d-block p-1"></span>
                            <span className="bg-light d-block p-1 mt-auto"></span>
                          </span>
                        </span>
                      </span>
                    </label>
                  </div>
                  <h5 className="fs-13 text-center mt-2">Compact</h5>
                </div>

                <div className="col-4">
                  <div className="form-check sidebar-setting card-radio">
                    <input
                      className="form-check-input"
                      type="radio"
                      name="data-sidebar-size"
                      id="sidebar-size-small"
                      value="sm"
                      onChange={(e) => handleLayoutChange("data-sidebar-size", e.target.value)}
                    />
                    <label
                      className="form-check-label p-0 avatar-md w-100"
                      htmlFor="sidebar-size-small"
                    >
                      <span className="d-flex gap-1 h-100">
                        <span className="flex-shrink-0">
                          <span className="bg-light d-flex h-100 flex-column gap-1">
                            <span className="d-block p-1 bg-primary-subtle mb-2"></span>
                            <span className="d-block p-1 pb-0 bg-primary-subtle"></span>
                            <span className="d-block p-1 pb-0 bg-primary-subtle"></span>
                            <span className="d-block p-1 pb-0 bg-primary-subtle"></span>
                          </span>
                        </span>
                        <span className="flex-grow-1">
                          <span className="d-flex h-100 flex-column">
                            <span className="bg-light d-block p-1"></span>
                            <span className="bg-light d-block p-1 mt-auto"></span>
                          </span>
                        </span>
                      </span>
                    </label>
                  </div>
                  <h5 className="fs-13 text-center mt-2">Small (Icon View)</h5>
                </div>
              </div>
            </div>

            {/* Sidebar Color */}
            <div id="sidebar-color">
              <h6 className="mt-4 mb-0 fw-bold text-uppercase">Sidebar Color</h6>
              <p className="text-muted">Choose a color of Sidebar.</p>

              <div className="row">
                <div className="col-4">
                  <div className="form-check sidebar-setting card-radio">
                    <input
                      className="form-check-input"
                      type="radio"
                      name="data-sidebar"
                      id="sidebar-color-light"
                      value="light"
                      onChange={(e) => handleLayoutChange("data-sidebar", e.target.value)}
                    />
                    <label
                      className="form-check-label p-0 avatar-md w-100"
                      htmlFor="sidebar-color-light"
                    >
                      <span className="d-flex gap-1 h-100">
                        <span className="flex-shrink-0">
                          <span className="bg-white border-end d-flex h-100 flex-column gap-1 p-1">
                            <span className="d-block p-1 px-2 bg-primary-subtle rounded mb-2"></span>
                            <span className="d-block p-1 px-2 pb-0 bg-primary-subtle"></span>
                            <span className="d-block p-1 px-2 pb-0 bg-primary-subtle"></span>
                            <span className="d-block p-1 px-2 pb-0 bg-primary-subtle"></span>
                          </span>
                        </span>
                        <span className="flex-grow-1">
                          <span className="d-flex h-100 flex-column">
                            <span className="bg-light d-block p-1"></span>
                            <span className="bg-light d-block p-1 mt-auto"></span>
                          </span>
                        </span>
                      </span>
                    </label>
                  </div>
                  <h5 className="fs-13 text-center mt-2">Light</h5>
                </div>
                <div className="col-4">
                  <div className="form-check sidebar-setting card-radio">
                    <input
                      className="form-check-input"
                      type="radio"
                      name="data-sidebar"
                      id="sidebar-color-dark"
                      value="dark"
                      defaultChecked
                      onChange={(e) => handleLayoutChange("data-sidebar", e.target.value)}
                    />
                    <label
                      className="form-check-label p-0 avatar-md w-100"
                      htmlFor="sidebar-color-dark"
                    >
                      <span className="d-flex gap-1 h-100">
                        <span className="flex-shrink-0">
                          <span className="bg-primary d-flex h-100 flex-column gap-1 p-1">
                            <span className="d-block p-1 px-2 bg-light bg-opacity-10 rounded mb-2"></span>
                            <span className="d-block p-1 px-2 pb-0 bg-light bg-opacity-10"></span>
                            <span className="d-block p-1 px-2 pb-0 bg-light bg-opacity-10"></span>
                            <span className="d-block p-1 px-2 pb-0 bg-light bg-opacity-10"></span>
                          </span>
                        </span>
                        <span className="flex-grow-1">
                          <span className="d-flex h-100 flex-column">
                            <span className="bg-light d-block p-1"></span>
                            <span className="bg-light d-block p-1 mt-auto"></span>
                          </span>
                        </span>
                      </span>
                    </label>
                  </div>
                  <h5 className="fs-13 text-center mt-2">Dark</h5>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
      <div className="offcanvas-footer border-top p-3 text-center">
        <div className="row">
          <div className="col-6">
            <button 
              type="button" 
              className="btn btn-light w-100" 
              id="reset-layout"
              onClick={handleReset}
            >
              Reset
            </button>
          </div>
          <div className="col-6">
            <a
              href="https://1.envato.market/velzon-admin"
              target="_blank"
              rel="noopener noreferrer"
              className="btn btn-primary w-100"
            >
              Buy Now
            </a>
          </div>
        </div>
      </div>
    </div>
  );
}
