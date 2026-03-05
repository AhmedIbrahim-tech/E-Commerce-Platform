"use client";

import React from "react";
import MyApp from "@/components/app";
import NextTopLoader from "nextjs-toploader";
import { Provider } from "react-redux";
import { RehydrateGate } from "@/components/RehydrateGate";
import { store, persistor } from "@/store/store";
import "./global.css";

export default function RootLayout({
  children,
}: {
  children: React.ReactNode;
}) {
  return (
    <html lang="en" suppressHydrationWarning>
      <body>
        <NextTopLoader color="#5D87FF" />
        <Provider store={store}>
          <RehydrateGate persistor={persistor}>
            <MyApp>{children}</MyApp>
          </RehydrateGate>
        </Provider>
      </body>
    </html>
  );
}
