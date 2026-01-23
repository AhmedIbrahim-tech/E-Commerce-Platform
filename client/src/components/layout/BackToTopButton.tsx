"use client";

import { useEffect, useState, useCallback } from "react";

const SCROLL_THRESHOLD = 300;

export default function BackToTopButton() {
  const [isVisible, setIsVisible] = useState(false);

  const toggleVisibility = useCallback(() => {
    setIsVisible(window.scrollY > SCROLL_THRESHOLD);
  }, []);

  const scrollToTop = useCallback(() => {
    window.scrollTo({
      top: 0,
      behavior: "smooth",
    });
  }, []);

  useEffect(() => {
    window.addEventListener("scroll", toggleVisibility);
    return () => window.removeEventListener("scroll", toggleVisibility);
  }, [toggleVisibility]);

  return (
    <button
      onClick={scrollToTop}
      className="btn btn-danger btn-icon"
      id="back-to-top"
      style={{ display: isVisible ? "block" : "none" }}
      aria-label="Back to top"
    >
      <i className="ri-arrow-up-line"></i>
    </button>
  );
}
