"use client";

import Link from "next/link";
import { useSidebar } from "./useSidebar";
import type { SidebarProps, MenuItem } from "./SidebarTypes";

/**
 * Base sidebar component that handles common sidebar functionality
 * Used by both AdminSidebar and MerchantSidebar
 */
export default function BaseSidebar({ menuItems, basePath }: SidebarProps) {
  const { openMenus, toggleMenu } = useSidebar();

  return (
    <div className="app-menu navbar-menu">
      <div className="navbar-brand-box">
        <Link href={basePath} className="logo logo-dark">
          <span className="logo-sm">
            <img src="/assets/images/logo-sm.png" alt="" height="22" />
          </span>
          <span className="logo-lg">
            <img src="/assets/images/logo-dark.png" alt="" height="17" />
          </span>
        </Link>
        <Link href={basePath} className="logo logo-light">
          <span className="logo-sm">
            <img src="/assets/images/logo-sm.png" alt="" height="22" />
          </span>
          <span className="logo-lg">
            <img src="/assets/images/logo-light.png" alt="" height="17" />
          </span>
        </Link>
        <button
          type="button"
          className="btn btn-sm p-0 fs-20 header-item float-end btn-vertical-sm-hover"
          id="vertical-hover"
        >
          <i className="ri-record-circle-line"></i>
        </button>
      </div>

      <div id="scrollbar" data-simplebar="init" className="h-100">
        <div className="container-fluid">
          <div id="two-column-menu"></div>
          <ul className="navbar-nav" id="navbar-nav">
            {menuItems.map((item) => (
              <SidebarMenuItem key={item.id} item={item} openMenus={openMenus} toggleMenu={toggleMenu} basePath={basePath} />
            ))}
          </ul>
        </div>
      </div>

      <div className="sidebar-background"></div>
    </div>
  );
}

/**
 * Renders a single menu item (title, link, or collapsible menu)
 */
function SidebarMenuItem({
  item,
  openMenus,
  toggleMenu,
  basePath,
}: {
  item: MenuItem;
  openMenus: string[];
  toggleMenu: (menuId: string) => void;
  basePath: string;
}) {
  if (item.isTitle) {
    return (
      <li className="menu-title">
        {item.icon && <i className={item.icon}></i>}
        <span data-key={`t-${item.id}`}>{item.label}</span>
      </li>
    );
  }

  const isOpen = openMenus.includes(item.id);
  const hasSubItems = item.subItems && item.subItems.length > 0;

  return (
    <li className="nav-item">
      {hasSubItems ? (
        <>
          <a
            className={`nav-link menu-link ${isOpen ? "" : "collapsed"}`}
            href={`#sidebar${item.id}`}
            onClick={(e) => {
              e.preventDefault();
              toggleMenu(item.id);
            }}
            data-bs-toggle="collapse"
            role="button"
            aria-expanded={isOpen}
            aria-controls={`sidebar${item.id}`}
          >
            <i className={item.icon}></i>
            <span data-key={`t-${item.id}`}>{item.label}</span>
          </a>
          <div className={`collapse menu-dropdown ${isOpen ? "show" : ""}`} id={`sidebar${item.id}`}>
            <ul className="nav nav-sm flex-column">
              {item.subItems?.map((subItem) => (
                <li key={subItem.id} className="nav-item">
                  <Link href={subItem.link} className="nav-link" data-key={`t-${subItem.id}`}>
                    {subItem.label}
                  </Link>
                </li>
              ))}
            </ul>
          </div>
        </>
      ) : (
        <Link className="nav-link menu-link" href={item.link || basePath}>
          <i className={item.icon}></i>
          <span data-key={`t-${item.id}`}>{item.label}</span>
        </Link>
      )}
    </li>
  );
}
