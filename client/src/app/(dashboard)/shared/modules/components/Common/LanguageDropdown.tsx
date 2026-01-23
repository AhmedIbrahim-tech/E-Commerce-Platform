"use client";

import { useState } from "react";

const languages = [
  { code: "en", name: "English", flag: "/assets/images/flags/us.svg" },
  { code: "es", name: "Española", flag: "/assets/images/flags/spain.svg" },
  { code: "de", name: "Deutsche", flag: "/assets/images/flags/germany.svg" },
  { code: "it", name: "Italiana", flag: "/assets/images/flags/italy.svg" },
  { code: "ru", name: "русский", flag: "/assets/images/flags/russia.svg" },
  { code: "zh", name: "中国人", flag: "/assets/images/flags/china.svg" },
  { code: "fr", name: "français", flag: "/assets/images/flags/french.svg" },
];

export default function LanguageDropdown() {
  const [selectedLang, setSelectedLang] = useState(languages[0]);
  const [isOpen, setIsOpen] = useState(false);

  const handleLanguageChange = (lang: typeof languages[0]) => {
    setSelectedLang(lang);
    setIsOpen(false);
  };

  return (
    <div className={`dropdown ms-1 topbar-head-dropdown header-item ${isOpen ? "show" : ""}`}>
      <button
        type="button"
        className="btn btn-icon btn-topbar btn-ghost-secondary rounded-circle"
        onClick={() => setIsOpen(!isOpen)}
        data-bs-toggle="dropdown"
        aria-haspopup="true"
        aria-expanded={isOpen}
      >
        <img
          id="header-lang-img"
          src={selectedLang.flag}
          alt="Header Language"
          height="20"
          className="rounded"
        />
      </button>
      <div className={`dropdown-menu dropdown-menu-end ${isOpen ? "show" : ""}`}>
        {languages.map((lang) => (
          <a
            key={lang.code}
            href="#"
            className={`dropdown-item notify-item language py-2 ${selectedLang.code === lang.code ? "active" : ""}`}
            title={lang.name}
            onClick={(e) => {
              e.preventDefault();
              handleLanguageChange(lang);
            }}
          >
            <img
              src={lang.flag}
              alt={lang.name}
              className="me-2 rounded"
              height="18"
            />
            <span className="align-middle">{lang.name}</span>
          </a>
        ))}
      </div>
    </div>
  );
}
