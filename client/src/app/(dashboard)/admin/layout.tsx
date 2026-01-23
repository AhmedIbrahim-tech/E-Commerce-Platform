// Layout components
import Header from "@/components/layout/Header";
import AdminSidebar from "@/components/layout/AdminSidebar";
import Footer from "@/components/layout/footer";
import RightSidebar from "@/components/Common/RightSidebar";
import BackToTopButton from "@/components/layout/BackToTopButton";
import CustomizerButton from "@/components/layout/CustomizerButton";
import VerticalOverlay from "@/components/layout/VerticalOverlay";

interface AdminLayoutProps {
  children: React.ReactNode;
}

export default function AdminLayout({ children }: AdminLayoutProps) {
  return (
    <>
      <div id="layout-wrapper">
        <Header />
        <AdminSidebar />

        <VerticalOverlay />

        <div className="main-content">
          <div className="page-content">{children}</div>
          <Footer />
        </div>
      </div>

      <BackToTopButton />
      <CustomizerButton />
      <RightSidebar />
    </>
  );
}

