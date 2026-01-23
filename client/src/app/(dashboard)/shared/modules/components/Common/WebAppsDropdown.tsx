"use client";

import { useState } from "react";

const apps = [
  { name: "GitHub", icon: "/assets/images/brands/github.png", href: "#" },
  { name: "Bitbucket", icon: "/assets/images/brands/bitbucket.png", href: "#" },
  { name: "Dribbble", icon: "/assets/images/brands/dribbble.png", href: "#" },
  { name: "Dropbox", icon: "/assets/images/brands/dropbox.png", href: "#" },
  { name: "Mail Chimp", icon: "/assets/images/brands/mail_chimp.png", href: "#" },
  { name: "Slack", icon: "/assets/images/brands/slack.png", href: "#" },
];

export default function WebAppsDropdown() {
  const [isOpen, setIsOpen] = useState(false);

  return (
    <div className={`dropdown topbar-head-dropdown ms-1 header-item ${isOpen ? "show" : ""}`}>
      <button
        type="button"
        className="btn btn-icon btn-topbar btn-ghost-secondary rounded-circle"
        onClick={() => setIsOpen(!isOpen)}
        data-bs-toggle="dropdown"
        aria-haspopup="true"
        aria-expanded={isOpen}
      >
        <i className="bx bx-category-alt fs-22"></i>
      </button>
      <div className={`dropdown-menu dropdown-menu-lg p-0 dropdown-menu-end ${isOpen ? "show" : ""}`}>
        <div className="p-3 border-top-0 border-start-0 border-end-0 border-dashed border">
          <div className="row align-items-center">
            <div className="col">
              <h6 className="m-0 fw-semibold fs-15">Web Apps</h6>
            </div>
            <div className="col-auto">
              <a href="#" className="btn btn-sm btn-soft-info">
                View All Apps
                <i className="ri-arrow-right-s-line align-middle"></i>
              </a>
            </div>
          </div>
        </div>

        <div className="p-2">
          <div className="row g-0">
            {apps.slice(0, 3).map((app) => (
              <div key={app.name} className="col">
                <a className="dropdown-icon-item" href={app.href}>
                  <img src={app.icon} alt={app.name} />
                  <span>{app.name}</span>
                </a>
              </div>
            ))}
          </div>

          <div className="row g-0">
            {apps.slice(3, 6).map((app) => (
              <div key={app.name} className="col">
                <a className="dropdown-icon-item" href={app.href}>
                  <img src={app.icon} alt={app.name} />
                  <span>{app.name}</span>
                </a>
              </div>
            ))}
          </div>
        </div>
      </div>
    </div>
  );
}
