import { Grid } from "@mui/material";
import Breadcrumb from "@/layouts/dashboard/shared/breadcrumb/Breadcrumb";
import PageContainer from "@/components/container/PageContainer";
import YearlyBreakup from "@/components/dashboards/ecommerce/YearlyBreakup";
import Projects from "@/components/dashboards/ecommerce/Projects";
import Customers from "@/components/dashboards/ecommerce/Customers";
import SalesTwo from "@/components/dashboards/ecommerce/SalesTwo";
import MonthlyEarnings from "@/components/dashboards/ecommerce/MonthlyEarnings";
import SalesOverview from "@/components/dashboards/ecommerce/SalesOverview";
import RevenueUpdates from "@/components/dashboards/ecommerce/RevenueUpdates";
import YearlySales from "@/components/dashboards/ecommerce/YearlySales";
import MostVisited from "@/components/widgets/charts/MostVisited";
import PageImpressions from "@/components/widgets/charts/PageImpressions";
import Followers from "@/components/widgets/charts/Followers";
import Views from "@/components/widgets/charts/Views";
import Earned from "@/components/widgets/charts/Earned";
import CurrentValue from "@/components/widgets/charts/CurrentValue";

const BCrumb = [
  { to: "/dashboards", title: "Dashboard" },
  { to: "/widgets", title: "Widgets" },
  { title: "Charts" },
];

export default function WidgetCharts() {
  return (
    <PageContainer title="Charts" description="this is Charts">
      <Breadcrumb title="Charts" items={BCrumb} />
      <Grid container spacing={3}>
        <Grid size={{ xs: 12, sm: 3 }}><Followers /></Grid>
        <Grid size={{ xs: 12, sm: 3 }}><Views /></Grid>
        <Grid size={{ xs: 12, sm: 3 }}><Earned /></Grid>
        <Grid size={{ xs: 12, sm: 3 }}><SalesTwo /></Grid>
        <Grid size={12}><CurrentValue /></Grid>
        <Grid size={{ xs: 12, lg: 4 }}>
          <Grid container spacing={3}>
            <Grid size={12}><YearlyBreakup /></Grid>
            <Grid size={12}><MonthlyEarnings /></Grid>
            <Grid size={12}><MostVisited /></Grid>
          </Grid>
        </Grid>
        <Grid size={{ xs: 12, lg: 4 }}>
          <Grid container spacing={3}>
            <Grid size={12}><YearlySales /></Grid>
            <Grid size={12}><PageImpressions /></Grid>
            <Grid size={{ xs: 12, sm: 6 }}><Customers /></Grid>
            <Grid size={{ xs: 12, sm: 6 }}><Projects /></Grid>
          </Grid>
        </Grid>
        <Grid size={{ xs: 12, lg: 4 }}>
          <Grid container spacing={3}>
            <Grid size={12}><RevenueUpdates /></Grid>
            <Grid size={12}><SalesOverview /></Grid>
          </Grid>
        </Grid>
      </Grid>
    </PageContainer>
  );
}
