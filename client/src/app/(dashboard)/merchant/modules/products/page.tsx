"use client";

import ProductsListModule from "@/app/(dashboard)/shared/modules/products/ProductsListModule";

export default function MerchantProductsPage() {
  return (
    <ProductsListModule
      title="Products"
      pageTitle="Merchant"
      routes={{
        list: "/merchant/modules/products",
        create: "/merchant/modules/products/create",
        view: (id) => `/merchant/modules/products/${id}`,
        edit: (id) => `/merchant/modules/products/${id}/edit`,
      }}
    />
  );
}

