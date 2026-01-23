"use client";

import BreadCrumb from "@/components/Common/BreadCrumb";
import ProductForm from "@/app/(dashboard)/shared/modules/products/components/ProductForm";

export default function MerchantCreateProductPage() {
  return (
    <>
      <BreadCrumb
        items={[
          { label: "Merchant", icon: "ri-store-3-line" },
          { label: "Create Product", icon: "ri-add-circle-line" },
        ]}
      />
      <ProductForm afterSaveHref="/merchant/modules/products" />
    </>
  );
}

