import type { Metadata } from "next";

// Scripts
import ScriptLoader from "@/components/layout/ScriptLoader";

// Store
import StoreProvider from "@/store/StoreProvider";

// Utils - ensure jsVectorMap is available early
import "@/utils/jsVectorMap";

// Constants
import {
  LAYOUT_CONFIG,
  SITE_METADATA,
  CSS_ASSETS,
} from "@/constants/layout";

// Styles
import "./globals.scss";

export const metadata: Metadata = {
  title: SITE_METADATA.title,
  description: SITE_METADATA.description,
};

interface RootLayoutProps {
  children: React.ReactNode;
}

export default function RootLayout({ children }: RootLayoutProps) {
  return (
    <html
      lang="en"
      suppressHydrationWarning
      data-layout={LAYOUT_CONFIG.layout}
      data-sidebar={LAYOUT_CONFIG.sidebar}
      data-sidebar-size={LAYOUT_CONFIG.sidebarSize}
      data-sidebar-image={LAYOUT_CONFIG.sidebarImage}
      data-topbar={LAYOUT_CONFIG.topbar}
      data-layout-mode={LAYOUT_CONFIG.layoutMode}
      data-layout-width="fluid"
      data-layout-position="fixed"
      data-layout-style="default"
    >
      <head>
        <meta charSet="utf-8" />
        <meta name="viewport" content="width=device-width, initial-scale=1.0" />
        <meta content={SITE_METADATA.description} name="description" />
        <meta content={SITE_METADATA.author} name="author" />
        <link rel="shortcut icon" href={SITE_METADATA.favicon} />

        {/* CSS Assets */}
        {CSS_ASSETS.map(({ href }) => (
          <link key={href} href={href} rel="stylesheet" type="text/css" />
        ))}
      </head>

      <body suppressHydrationWarning>
        <StoreProvider>{children}</StoreProvider>

        {/* JavaScript Assets - loaded after React components mount */}
        <ScriptLoader />
      </body>
    </html>
  );
}
