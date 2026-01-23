// Layout components
import Header from "@/components/layout/Header";
import MerchantSidebar from "@/components/layout/MerchantSidebar";
import Footer from "@/components/layout/footer";
import RightSidebar from "@/components/Common/RightSidebar";
import BackToTopButton from "@/components/layout/BackToTopButton";
import CustomizerButton from "@/components/layout/CustomizerButton";
import VerticalOverlay from "@/components/layout/VerticalOverlay";

interface MerchantLayoutProps {
  children: React.ReactNode;
}

export default function MerchantLayout({ children }: MerchantLayoutProps) {
  return (
    <>
      <div id="layout-wrapper">
        <Header />
        <MerchantSidebar />

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

