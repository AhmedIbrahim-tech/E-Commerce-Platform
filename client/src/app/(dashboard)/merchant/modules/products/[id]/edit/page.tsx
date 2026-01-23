"use client";

import BreadCrumb from "@/components/Common/BreadCrumb";
import ProductForm from "@/app/(dashboard)/shared/modules/products/components/ProductForm";

export default function MerchantEditProductPage({ params }: { params: { id: string } | null }) {
  if (!params?.id) {
    return <div>Product ID is required</div>;
  }
  
  return (
    <>
      <BreadCrumb
        items={[
          { label: "Merchant", icon: "ri-store-3-line" },
          { label: "Edit Product", icon: "ri-edit-line" },
        ]}
      />
      <ProductForm productId={params.id} afterSaveHref="/merchant/modules/products" />
    </>
  );
}

