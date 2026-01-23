"use client";

import ProductDetailsModule from "@/app/(dashboard)/shared/modules/products/ProductDetailsModule";

export default function MerchantProductDetailsPage({ params }: { params: { id: string } | null }) {
  if (!params?.id) {
    return <div>Product ID is required</div>;
  }
  
  return (
    <ProductDetailsModule
      productId={params.id}
      title="Product Details"
      pageTitle="Merchant"
      routes={{
        backToList: "/merchant/modules/products",
        edit: (id) => `/merchant/modules/products/${id}/edit`,
      }}
    />
  );
}

