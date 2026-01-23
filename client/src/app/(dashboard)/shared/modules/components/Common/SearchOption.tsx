"use client";

import { useState } from "react";

export default function SearchOption() {
  const [isOpen, setIsOpen] = useState(false);
  const [searchValue, setSearchValue] = useState("");

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    // Handle search
    console.log("Searching for:", searchValue);
  };

  return (
    <div className={`dropdown d-md-none topbar-head-dropdown header-item ${isOpen ? "show" : ""}`}>
      <button
        type="button"
        className="btn btn-icon btn-topbar btn-ghost-secondary rounded-circle"
        id="page-header-search-dropdown"
        onClick={() => setIsOpen(!isOpen)}
        data-bs-toggle="dropdown"
        aria-haspopup="true"
        aria-expanded={isOpen}
      >
        <i className="bx bx-search fs-22"></i>
      </button>
      <div
        className={`dropdown-menu dropdown-menu-lg dropdown-menu-end p-0 ${isOpen ? "show" : ""}`}
        aria-labelledby="page-header-search-dropdown"
      >
        <form className="p-3" onSubmit={handleSubmit}>
          <div className="form-group m-0">
            <div className="input-group">
              <input
                type="text"
                className="form-control"
                placeholder="Search ..."
                aria-label="Search"
                value={searchValue}
                onChange={(e) => setSearchValue(e.target.value)}
              />
              <button className="btn btn-primary" type="submit">
                <i className="mdi mdi-magnify"></i>
              </button>
            </div>
          </div>
        </form>
      </div>
    </div>
  );
}
