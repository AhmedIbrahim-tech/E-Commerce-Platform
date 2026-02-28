"use client";

import { useCustomizer } from '@/hooks/useCustomizer';
import Box from "@mui/material/Box";
import Container from "@mui/material/Container";
import { styled, useTheme } from "@mui/material/styles";
import React, { useState, useContext, Activity } from "react";
import Header from "@/layouts/dashboard/vertical/header/Header";
import Sidebar from "@/layouts/dashboard/vertical/sidebar/Sidebar";
import Customizer from "@/layouts/dashboard/shared/customizer/Customizer";
import Navigation from "@/layouts/dashboard/horizontal/navbar/Navigation";
import HorizontalHeader from "@/layouts/dashboard/horizontal/header/Header";
import config from "@/config/config";

const MainWrapper = styled("div")(() => ({
  display: "flex",
  minHeight: "100vh",
  width: "100%",
}));

const PageWrapper = styled("div")(() => ({
  display: "flex",
  flexGrow: 1,
  paddingBottom: "60px",
  flexDirection: "column",
  zIndex: 1,
  width: "100%",
  backgroundColor: "transparent",
}));

interface Props {
  children: React.ReactNode;
}

export default function RootLayout({
  children,
}: {
  children: React.ReactNode;
}) {
  const { activeLayout, isLayout, activeMode, isCollapse } = useCustomizer();
  const MiniSidebarWidth = config.miniSidebarWidth;


  const theme = useTheme();

  return (
    <MainWrapper>
      {/* ------------------------------------------- */}
      {/* Sidebar */}
      {/* ------------------------------------------- */}
      {/* {activeLayout === 'horizontal' ? "" : <Sidebar />} */}

      <Activity mode={activeLayout === "horizontal" ? "hidden" : "visible"}>

        <Sidebar />

      </Activity>
      {/* ------------------------------------------- */}
      {/* Main Wrapper */}
      {/* ------------------------------------------- */}
      <PageWrapper
        className="page-wrapper"
        sx={{
          ...(isCollapse === "mini-sidebar" && {
            [theme.breakpoints.up("lg")]: {
              ml: `${MiniSidebarWidth}px`,
            },
          }),
        }}
      >
        {/* ------------------------------------------- */}
        {/* Header */}
        {/* ------------------------------------------- */}
        {activeLayout === 'horizontal' ? <HorizontalHeader /> : <Header />}

        {/* PageContent */}
        {activeLayout === 'horizontal' ? <Navigation /> : ""}
        <Container
          sx={{
            maxWidth: isLayout === "boxed" ? "lg" : "100%!important",
          }}
        >
          {/* ------------------------------------------- */}
          {/* PageContent */}
          {/* ------------------------------------------- */}

          <Box sx={{ minHeight: "calc(100vh - 170px)" }}>
            {/* <Outlet /> */}
            {children}
            {/* <Index /> */}
          </Box>

          {/* ------------------------------------------- */}
          {/* End Page */}
          {/* ------------------------------------------- */}
        </Container>
        <Customizer />
      </PageWrapper>
    </MainWrapper>
  );
}
