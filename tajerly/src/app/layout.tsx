import type { Metadata } from "next";
import "@fontsource-variable/plus-jakarta-sans";
import "./globals.css";
import { Providers } from "@/components/layout/Providers";
import { siteConfig } from "@/config/site";

export const metadata: Metadata = {
  title: siteConfig.fullTitle,
  description: siteConfig.description,
  icons: siteConfig.icons,
  openGraph: {
    title: siteConfig.seoTitle,
    description: siteConfig.description,
    images: [siteConfig.ogImage],
  },
  twitter: {
    card: "summary_large_image",
    title: siteConfig.seoTitle,
    description: siteConfig.description,
    images: [siteConfig.ogImage],
  },
};

export default function RootLayout({
  children,
}: Readonly<{
  children: React.ReactNode;
}>) {
  return (
    <html lang="en" dir="ltr" suppressHydrationWarning>
      <head>
        <script
          dangerouslySetInnerHTML={{
            __html: `(function(){var t=localStorage.getItem('tajerly-theme');if(t==='dark'||t==='light'){document.documentElement.classList.add(t)}else if(window.matchMedia('(prefers-color-scheme: dark)').matches){document.documentElement.classList.add('dark')}else{document.documentElement.classList.add('light')}var l=localStorage.getItem('i18nextLng');if(l==='ar'){document.documentElement.setAttribute('lang','ar');document.documentElement.setAttribute('dir','rtl')}})();`,
          }}
        />
      </head>
      <body>
        <Providers>
          {children}
        </Providers>
      </body>
    </html>
  );
}
