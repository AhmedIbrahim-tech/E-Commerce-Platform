import React from "react";
import MyApp from "@/components/app";
import NextTopLoader from 'nextjs-toploader';
import "./global.css";
import { StoreProvider } from "@/store/StoreProvider";


export const metadata = {
  title: "Modernize Main Demo",
  description: "Modernize Main kit",
};

export default function RootLayout({
  children,
}: {
  children: React.ReactNode;
}) {
  return (
    <html lang="en" suppressHydrationWarning>
      <body>
        <NextTopLoader color="#5D87FF" />
        <StoreProvider>
          <MyApp>{children}</MyApp>
        </StoreProvider>
      </body>
    </html>
  );
}
