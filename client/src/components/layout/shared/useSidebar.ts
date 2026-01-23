/**
 * Shared hook for sidebar functionality
 */

import { useEffect, useState } from "react";

export function useSidebar() {
  const [openMenus, setOpenMenus] = useState<string[]>([]);

  const toggleMenu = (menuId: string) => {
    setOpenMenus((prev) => (prev.includes(menuId) ? prev.filter((id) => id !== menuId) : [...prev, menuId]));
  };

  useEffect(() => {
    const scrollbar = document.getElementById("scrollbar");
    if (scrollbar && typeof window !== "undefined") {
      // SimpleBar will be initialized by the external script
    }

    const verticalHover = document.getElementById("vertical-hover");
    const handleVerticalHover = () => {
      const body = document.body;
      if (body.getAttribute("data-sidebar-size") === "sm-hover") {
        body.setAttribute("data-sidebar-size", "sm-hover-active");
      } else if (body.getAttribute("data-sidebar-size") === "sm-hover-active") {
        body.setAttribute("data-sidebar-size", "sm-hover");
      } else {
        body.setAttribute("data-sidebar-size", "sm-hover");
      }
    };

    verticalHover?.addEventListener("click", handleVerticalHover);
    return () => {
      verticalHover?.removeEventListener("click", handleVerticalHover);
    };
  }, []);

  return { openMenus, toggleMenu };
}
