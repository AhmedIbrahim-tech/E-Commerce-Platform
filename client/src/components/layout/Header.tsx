"use client";

import { useEffect } from "react";
import Link from "next/link";
import { AppRoutes } from "@/constants/routes";
import SearchOption from "@/components/Common/SearchOption";
import GlobalSearchFilter from "@/components/Common/GlobalSearchFilter";
import LanguageDropdown from "@/components/Common/LanguageDropdown";
import WebAppsDropdown from "@/components/Common/WebAppsDropdown";
import MyCartDropdown from "@/components/Common/MyCartDropdown";
import FullScreenDropdown from "@/components/Common/FullScreenDropdown";
import LightDark from "@/components/Common/LightDark";
import NotificationDropdown from "@/components/Common/NotificationDropdown";
import ProfileDropdown from "@/components/Common/ProfileDropdown";

export default function Header() {
  useEffect(() => {
    // Handle hamburger menu click
    const hamburgerIcon = document.getElementById("topnav-hamburger-icon");
    
    const handleHamburgerClick = () => {
      document.body.classList.toggle("sidebar-enable");
      const windowSize = document.documentElement.clientWidth;
      
      if (windowSize > 767) {
        document.documentElement.setAttribute(
          "data-sidebar-size",
          document.documentElement.getAttribute("data-sidebar-size") === "sm" ? "lg" : "sm"
        );
      }
    };

    hamburgerIcon?.addEventListener("click", handleHamburgerClick);

    return () => {
      hamburgerIcon?.removeEventListener("click", handleHamburgerClick);
    };
  }, []);

  return (
    <header id="page-topbar">
      <div className="layout-width">
        <div className="navbar-header">
          <div className="d-flex">
            {/* LOGO */}
            <div className="navbar-brand-box horizontal-logo">
              <Link href={AppRoutes.Dashboard.Home} className="logo logo-dark">
                <span className="logo-sm">
                  <img src="/assets/images/logo-sm.png" alt="" height="22" />
                </span>
                <span className="logo-lg">
                  <img src="/assets/images/logo-dark.png" alt="" height="17" />
                </span>
              </Link>

              <Link href={AppRoutes.Dashboard.Home} className="logo logo-light">
                <span className="logo-sm">
                  <img src="/assets/images/logo-sm.png" alt="" height="22" />
                </span>
                <span className="logo-lg">
                  <img src="/assets/images/logo-light.png" alt="" height="17" />
                </span>
              </Link>
            </div>

            <button
              type="button"
              className="btn btn-sm px-3 fs-16 header-item vertical-menu-btn topnav-hamburger"
              id="topnav-hamburger-icon"
            >
              <span className="hamburger-icon">
                <span></span>
                <span></span>
                <span></span>
              </span>
            </button>

            {/* App Search */}
            <GlobalSearchFilter />
          </div>

          <div className="d-flex align-items-center">
            <SearchOption />
            <LanguageDropdown />
            <WebAppsDropdown />
            <MyCartDropdown />
            <FullScreenDropdown />
            <LightDark />
            <NotificationDropdown />
            <ProfileDropdown />
          </div>
        </div>
      </div>
    </header>
  );
}
