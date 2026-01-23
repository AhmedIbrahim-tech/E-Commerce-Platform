"use client";

import ProductDetailsModule from "@/app/(dashboard)/shared/modules/products/ProductDetailsModule";

export default function AdminProductDetailsPage({ params }: { params: { id: string } | null }) {
  if (!params?.id) {
    return <div>Product ID is required</div>;
  }
  
  return (
    <ProductDetailsModule
      productId={params.id}
      title="Product Details"
      pageTitle="Admin"
      routes={{
        backToList: "/admin/modules/products",
        edit: (id) => `/admin/modules/products/${id}/edit`,
      }}
    />
  );
}

