"use client";

import { useState, useRef, useEffect } from "react";

export default function GlobalSearchFilter() {
  const [searchValue, setSearchValue] = useState("");
  const [isOpen, setIsOpen] = useState(false);
  const inputRef = useRef<HTMLInputElement>(null);
  const dropdownRef = useRef<HTMLFormElement>(null);

  const handleInputChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const value = e.target.value;
    setSearchValue(value);
    setIsOpen(value.length > 0);
  };

  const handleClear = () => {
    setSearchValue("");
    setIsOpen(false);
    inputRef.current?.focus();
  };

  // Close dropdown when clicking outside
  useEffect(() => {
    const handleClickOutside = (event: MouseEvent) => {
      if (dropdownRef.current && !dropdownRef.current.contains(event.target as Node)) {
        setIsOpen(false);
      }
    };

    document.addEventListener("mousedown", handleClickOutside);
    return () => document.removeEventListener("mousedown", handleClickOutside);
  }, []);

  return (
    <form className="app-search d-none d-md-block" ref={dropdownRef}>
      <div className="position-relative">
        <input
          ref={inputRef}
          type="text"
          className="form-control"
          placeholder="Search..."
          autoComplete="off"
          id="search-options"
          value={searchValue}
          onChange={handleInputChange}
          onFocus={() => searchValue.length > 0 && setIsOpen(true)}
        />
        <span className="mdi mdi-magnify search-widget-icon"></span>
        <span
          className={`mdi mdi-close-circle search-widget-icon search-widget-icon-close ${searchValue.length > 0 ? "" : "d-none"}`}
          id="search-close-options"
          onClick={handleClear}
          style={{ cursor: "pointer" }}
        ></span>
      </div>
      <div className={`dropdown-menu dropdown-menu-lg ${isOpen ? "show" : ""}`} id="search-dropdown">
        <div style={{ maxHeight: "320px", overflowY: "auto" }}>
          {/* Recent Searches */}
          <div className="dropdown-header">
            <h6 className="text-overflow text-muted mb-0 text-uppercase">
              Recent Searches
            </h6>
          </div>

          <div className="dropdown-item bg-transparent text-wrap">
            <a href="#" className="btn btn-soft-secondary btn-sm rounded-pill">
              how to setup <i className="mdi mdi-magnify ms-1"></i>
            </a>
            <a href="#" className="btn btn-soft-secondary btn-sm rounded-pill">
              buttons <i className="mdi mdi-magnify ms-1"></i>
            </a>
          </div>

          {/* Pages */}
          <div className="dropdown-header mt-2">
            <h6 className="text-overflow text-muted mb-1 text-uppercase">
              Pages
            </h6>
          </div>

          <a href="#" className="dropdown-item notify-item">
            <i className="ri-bubble-chart-line align-middle fs-18 text-muted me-2"></i>
            <span>Analytics Dashboard</span>
          </a>

          <a href="#" className="dropdown-item notify-item">
            <i className="ri-lifebuoy-line align-middle fs-18 text-muted me-2"></i>
            <span>Help Center</span>
          </a>

          <a href="#" className="dropdown-item notify-item">
            <i className="ri-user-settings-line align-middle fs-18 text-muted me-2"></i>
            <span>My account settings</span>
          </a>

          {/* Members */}
          <div className="dropdown-header mt-2">
            <h6 className="text-overflow text-muted mb-2 text-uppercase">
              Members
            </h6>
          </div>

          <div className="notification-list">
            <a href="#" className="dropdown-item notify-item py-2">
              <div className="d-flex">
                <img
                  src="/assets/images/users/avatar-2.jpg"
                  className="me-3 rounded-circle avatar-xs flex-shrink-0"
                  alt="user-pic"
                />
                <div className="flex-grow-1">
                  <h6 className="m-0">Angela Bernier</h6>
                  <span className="fs-11 mb-0 text-muted">Manager</span>
                </div>
              </div>
            </a>
            <a href="#" className="dropdown-item notify-item py-2">
              <div className="d-flex">
                <img
                  src="/assets/images/users/avatar-3.jpg"
                  className="me-3 rounded-circle avatar-xs flex-shrink-0"
                  alt="user-pic"
                />
                <div className="flex-grow-1">
                  <h6 className="m-0">David Grasso</h6>
                  <span className="fs-11 mb-0 text-muted">Web Designer</span>
                </div>
              </div>
            </a>
            <a href="#" className="dropdown-item notify-item py-2">
              <div className="d-flex">
                <img
                  src="/assets/images/users/avatar-5.jpg"
                  className="me-3 rounded-circle avatar-xs flex-shrink-0"
                  alt="user-pic"
                />
                <div className="flex-grow-1">
                  <h6 className="m-0">Mike Bunch</h6>
                  <span className="fs-11 mb-0 text-muted">React Developer</span>
                </div>
              </div>
            </a>
          </div>
        </div>

        <div className="text-center pt-3 pb-1">
          <a href="#" className="btn btn-primary btn-sm">
            View All Results <i className="ri-arrow-right-line ms-1"></i>
          </a>
        </div>
      </div>
    </form>
  );
}
