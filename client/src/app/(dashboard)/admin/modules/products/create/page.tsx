"use client";

import BreadCrumb from "@/components/Common/BreadCrumb";
import ProductForm from "@/app/(dashboard)/shared/modules/products/components/ProductForm";

export default function AdminCreateProductPage() {
  return (
    <>
      <BreadCrumb
        items={[
          { label: "Admin", icon: "ri-shield-user-line" },
          { label: "Create Product", icon: "ri-add-circle-line" },
        ]}
      />
      <ProductForm afterSaveHref="/admin/modules/products" />
    </>
  );
}

