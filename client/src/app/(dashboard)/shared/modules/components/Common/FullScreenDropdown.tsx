"use client";

import { useState, useEffect, useCallback } from "react";

export default function FullScreenDropdown() {
  const [isFullScreen, setIsFullScreen] = useState(false);

  const toggleFullScreen = useCallback(() => {
    if (!document.fullscreenElement) {
      document.documentElement.requestFullscreen().catch((err) => {
        console.error(`Error attempting to enable fullscreen: ${err.message}`);
      });
    } else {
      if (document.exitFullscreen) {
        document.exitFullscreen();
      }
    }
  }, []);

  useEffect(() => {
    const handleFullScreenChange = () => {
      setIsFullScreen(!!document.fullscreenElement);
    };

    document.addEventListener("fullscreenchange", handleFullScreenChange);

    return () => {
      document.removeEventListener("fullscreenchange", handleFullScreenChange);
    };
  }, []);

  return (
    <div className="ms-1 header-item d-none d-sm-flex">
      <button
        type="button"
        className="btn btn-icon btn-topbar btn-ghost-secondary rounded-circle"
        onClick={toggleFullScreen}
        title={isFullScreen ? "Exit Fullscreen" : "Enter Fullscreen"}
      >
        <i className={`bx ${isFullScreen ? "bx-exit-fullscreen" : "bx-fullscreen"} fs-22`}></i>
      </button>
    </div>
  );
}
