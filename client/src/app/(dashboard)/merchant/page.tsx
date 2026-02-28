"use client";
import React from "react";
import { useEffect, useState } from "react";
import { Box, Grid } from "@mui/material";
import PageContainer from "@/components/container/PageContainer";
import WeeklyStats from "@/components/dashboards/ecommerce/WeeklyStats";
import YearlySales from "@/components/dashboards/ecommerce/YearlySales";
import PaymentGateways from "@/components/dashboards/ecommerce/PaymentGateways";
import WelcomeCard from "@/components/dashboards/ecommerce/WelcomeCard";
import Expence from "@/components/dashboards/ecommerce/Expence";
import Growth from "@/components/dashboards/ecommerce/Growth";
import RevenueUpdates from "@/components/dashboards/ecommerce/RevenueUpdates";
import SalesOverview from "@/components/dashboards/ecommerce/SalesOverview";
import SalesTwo from "@/components/dashboards/ecommerce/SalesTwo";
import Sales from "@/components/dashboards/ecommerce/Sales";
import MonthlyEarnings from "@/components/dashboards/ecommerce/MonthlyEarnings";
import ProductPerformances from "@/components/dashboards/ecommerce/ProductPerformances";
import RecentTransactions from "@/components/dashboards/ecommerce/RecentTransactions";

export default function EcommerceDashboard() {
  const [isLoading, setLoading] = useState(true);
  useEffect(() => {
    setLoading(false);
  }, []);

  return (
    <PageContainer title="eCommerce Dashboard" description="this is eCommerce Dashboard">
      <Box mt={3}>
        <Grid container spacing={3}>
          <Grid size={{ xs: 12, lg: 8 }}>
            <WelcomeCard />
          </Grid>
          <Grid size={{ xs: 12, lg: 4 }}>
            <Grid container spacing={3}>
              <Grid size={{ xs: 12, sm: 6 }}>
                <Expence isLoading={isLoading} />
              </Grid>
              <Grid size={{ xs: 12, sm: 6 }}>
                <Sales isLoading={isLoading} />
              </Grid>
            </Grid>
          </Grid>
          <Grid size={{ xs: 12, sm: 6, lg: 4 }}>
            <RevenueUpdates isLoading={isLoading} />
          </Grid>
          <Grid size={{ xs: 12, sm: 6, lg: 4 }}>
            <SalesOverview isLoading={isLoading} />
          </Grid>
          <Grid size={{ xs: 12, sm: 6, lg: 4 }}>
            <Grid container spacing={3}>
              <Grid size={{ xs: 12, sm: 6 }}>
                <SalesTwo isLoading={isLoading} />
              </Grid>
              <Grid size={{ xs: 12, sm: 6 }}>
                <Growth isLoading={isLoading} />
              </Grid>
              <Grid size={12}>
                <MonthlyEarnings isLoading={isLoading} />
              </Grid>
            </Grid>
          </Grid>
          <Grid size={{ xs: 12, sm: 6, lg: 4 }}>
            <WeeklyStats isLoading={isLoading} />
          </Grid>
          <Grid size={{ xs: 12, lg: 4 }}>
            <YearlySales isLoading={isLoading} />
          </Grid>
          <Grid size={{ xs: 12, lg: 4 }}>
            <PaymentGateways />
          </Grid>
          <Grid size={{ xs: 12, lg: 4 }}>
            <RecentTransactions />
          </Grid>
          <Grid size={{ xs: 12, lg: 8 }}>
            <ProductPerformances />
          </Grid>
        </Grid>
      </Box>
    </PageContainer>
  );
}
