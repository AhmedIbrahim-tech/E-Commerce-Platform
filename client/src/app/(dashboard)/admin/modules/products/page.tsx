"use client";

import ProductsListModule from "@/app/(dashboard)/shared/modules/products/ProductsListModule";

export default function AdminProductsPage() {
  return (
    <ProductsListModule
      title="Products"
      pageTitle="Admin"
      routes={{
        list: "/admin/modules/products",
        create: "/admin/modules/products/create",
        view: (id) => `/admin/modules/products/${id}`,
        edit: (id) => `/admin/modules/products/${id}/edit`,
      }}
    />
  );
}

