// Layout configuration constants
export const LAYOUT_CONFIG = {
  layout: "vertical",
  sidebar: "dark",
  sidebarSize: "lg",
  sidebarImage: "none",
  topbar: "light",
  layoutMode: "light",
} as const;

// Metadata configuration
export const SITE_METADATA = {
  title: "Dashboard | Velzon - Admin & Dashboard Template",
  description: "Premium Multipurpose Admin & Dashboard Template",
  author: "Themesbrand",
  favicon: "/assets/images/favicon.ico",
} as const;

// CSS stylesheets to load
export const CSS_ASSETS = [
  { href: "/assets/libs/jsvectormap/css/jsvectormap.min.css", comment: "jsvectormap" },
  { href: "/assets/libs/swiper/swiper-bundle.min.css", comment: "Swiper slider" },
  { href: "/assets/css/bootstrap.min.css", comment: "Bootstrap" },
  { href: "/assets/css/icons.min.css", comment: "Icons" },
  { href: "/assets/css/app.min.css", comment: "App" },
  { href: "/assets/css/custom.min.css", comment: "Custom" },
] as const;

// JavaScript files to load
// Note: app.js and plugins.js are excluded as their functionality is handled by React components
export const JS_ASSETS = [
  // Core libraries
  { src: "/assets/libs/bootstrap/js/bootstrap.bundle.min.js" },
  { src: "/assets/libs/simplebar/simplebar.min.js" },
  { src: "/assets/libs/node-waves/waves.min.js" },
  { src: "/assets/libs/feather-icons/feather.min.js" },
  { src: "/assets/js/pages/plugins/lord-icon-2.1.0.js" },
  // Charts
  { src: "/assets/libs/apexcharts/apexcharts.min.js" },
  // Vector map
  { src: "/assets/libs/jsvectormap/js/jsvectormap.min.js" },
  { src: "/assets/libs/jsvectormap/maps/world-merc.js" },
  // Swiper
  { src: "/assets/libs/swiper/swiper-bundle.min.js" },
] as const;

// Type exports for layout attributes
export type LayoutConfig = typeof LAYOUT_CONFIG;
export type SiteMetadata = typeof SITE_METADATA;
