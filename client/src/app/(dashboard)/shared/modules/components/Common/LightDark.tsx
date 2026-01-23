"use client";

import { useState } from "react";

export default function LightDark() {
  const [isDarkMode, setIsDarkMode] = useState(() => {
    if (typeof document === "undefined") return false;
    return document.documentElement.getAttribute("data-layout-mode") === "dark";
  });

  const toggleTheme = () => {
    const newMode = isDarkMode ? "light" : "dark";
    document.documentElement.setAttribute("data-layout-mode", newMode);
    document.documentElement.setAttribute("data-sidebar", newMode === "dark" ? "dark" : "light");
    document.documentElement.setAttribute("data-topbar", newMode === "dark" ? "dark" : "light");
    setIsDarkMode(!isDarkMode);
  };

  return (
    <div className="ms-1 header-item d-none d-sm-flex">
      <button
        type="button"
        className="btn btn-icon btn-topbar btn-ghost-secondary rounded-circle light-dark-mode"
        onClick={toggleTheme}
        title={isDarkMode ? "Switch to Light Mode" : "Switch to Dark Mode"}
      >
        <i className={`bx ${isDarkMode ? "bx-sun" : "bx-moon"} fs-22`}></i>
      </button>
    </div>
  );
}
