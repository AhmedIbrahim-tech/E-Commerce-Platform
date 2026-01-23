"use client";

import Link from "next/link";
import { useEffect, useMemo, useRef, useState } from "react";

import BreadCrumb from "@/components/Common/BreadCrumb";
import { fetchProductByIdAsync } from "@/store/slices/productSlice";
import { useAppDispatch, useAppSelector } from "@/store/hooks";
import { ProductPublishStatus, ProductVisibility, type Product } from "@/types";

export type ProductDetailsRoutes = {
  edit: (id: string) => string;
  backToList: string;
};

function formatDate(value?: string): string {
  if (!value) return "-";
  const d = new Date(value);
  return Number.isNaN(d.getTime()) ? value : d.toLocaleDateString();
}

function formatMoney(value?: number): string {
  if (value === undefined || value === null) return "-";
  return `$${value.toFixed(2)}`;
}

function publishStatusLabel(p: Product): string {
  if (p.publishStatus === ProductPublishStatus.Scheduled) return "Scheduled";
  if (p.publishStatus === ProductPublishStatus.Draft) return "Draft";
  return p.isActive ? "Published" : "Draft";
}

function visibilityLabel(p: Product): string {
  if (p.visibility === ProductVisibility.Hidden) return "Hidden";
  return "Public";
}

function pickPrimaryImage(p?: Product | null): string {
  const imgs = p?.productImages || [];
  const primary = imgs.find((i) => i.isPrimary);
  return primary?.imageURL || imgs[0]?.imageURL || "/assets/images/products/img-8.png";
}

export default function ProductDetailsModule({
  productId,
  title = "Product Details",
  pageTitle = "Ecommerce",
  routes,
}: {
  productId: string;
  title?: string;
  pageTitle?: string;
  routes: ProductDetailsRoutes;
}) {
  const dispatch = useAppDispatch();
  const didFetchKey = useRef<string>("");

  const product = useAppSelector((s) => s.product.selectedProduct);
  const isLoading = useAppSelector((s) => s.product.loading);
  const error = useAppSelector((s) => s.product.error);

  const [activeImageUrl, setActiveImageUrl] = useState<string>("/assets/images/products/img-8.png");

  useEffect(() => {
    const key = `product:${productId}`;
    if (didFetchKey.current === key) return;
    didFetchKey.current = key;

    (async () => {
      try {
        const p = await dispatch(fetchProductByIdAsync(productId)).unwrap();
        setActiveImageUrl(pickPrimaryImage(p as Product));
      } catch {
      }
    })();
  }, [dispatch, productId]);

  const images = useMemo(() => (product?.productImages || []).slice().sort((a, b) => a.displayOrder - b.displayOrder), [product]);
  const variants = product?.productVariants || [];
  const tags = product?.tags || [];

  const sizeVariants = useMemo(() => variants.filter((v) => v.variantAttribute.toLowerCase().includes("size")), [variants]);
  const colorVariants = useMemo(() => variants.filter((v) => v.variantAttribute.toLowerCase().includes("color")), [variants]);

  return (
    <div className="container-fluid">
      <BreadCrumb
        items={[
          { label: pageTitle, icon: "ri-store-line" },
          { label: title, icon: "ri-file-list-line" },
        ]}
      />

      {error ? (
        <div className="alert alert-danger" role="alert">
          {error}
        </div>
      ) : null}

      {isLoading ? (
        <div className="card">
          <div className="card-body text-muted">Loading...</div>
        </div>
      ) : null}

      {!isLoading && product && product.id === productId ? (
        <div className="row">
          <div className="col-lg-12">
            <div className="card">
              <div className="card-body">
                <div className="row gx-lg-5">
                  <div className="col-xl-4 col-md-8 mx-auto">
                    <div className="sticky-side-div">
                      <div className="p-2 rounded bg-light">
                        <img src={activeImageUrl} alt="" className="img-fluid d-block" />
                      </div>
                      {images.length > 1 ? (
                        <div className="d-flex gap-2 mt-2 flex-wrap">
                          {images.map((img) => (
                            <button
                              key={img.id}
                              type="button"
                              className={`btn p-0 border rounded ${activeImageUrl === img.imageURL ? "border-primary" : ""}`}
                              style={{ width: 64, height: 64, overflow: "hidden" }}
                              onClick={() => setActiveImageUrl(img.imageURL)}
                            >
                              <img src={img.imageURL} alt="" className="img-fluid d-block" />
                            </button>
                          ))}
                        </div>
                      ) : null}
                    </div>
                  </div>

                  <div className="col-xl-8">
                    <div className="mt-xl-0 mt-5">
                      <div className="d-flex">
                        <div className="flex-grow-1">
                          <h4>{product.name}</h4>
                          <div className="hstack gap-3 flex-wrap">
                            <div>
                              <span className="text-primary d-block">{product.brandName || product.manufacturer || "-"}</span>
                            </div>
                            <div className="vr"></div>
                            <div className="text-muted">
                              Category : <span className="text-body fw-medium">{product.categoryName || "-"}</span>
                            </div>
                            <div className="vr"></div>
                            <div className="text-muted">
                              Published : <span className="text-body fw-medium">{formatDate(product.createdAt)}</span>
                            </div>
                          </div>
                        </div>
                        <div className="flex-shrink-0">
                          <div>
                            <Link href={routes.edit(product.id)} className="btn btn-light" title="Edit">
                              <i className="ri-pencil-fill align-bottom"></i>
                            </Link>
                          </div>
                        </div>
                      </div>

                      <div className="row mt-4">
                        <div className="col-lg-3 col-sm-6">
                          <div className="p-2 border border-dashed rounded">
                            <div className="d-flex align-items-center">
                              <div className="avatar-sm me-2">
                                <div className="avatar-title rounded bg-transparent text-success fs-24">
                                  <i className="ri-money-dollar-circle-fill"></i>
                                </div>
                              </div>
                              <div className="flex-grow-1">
                                <p className="text-muted mb-1">Price :</p>
                                <h5 className="mb-0">{formatMoney(product.price)}</h5>
                              </div>
                            </div>
                          </div>
                        </div>
                        <div className="col-lg-3 col-sm-6">
                          <div className="p-2 border border-dashed rounded">
                            <div className="d-flex align-items-center">
                              <div className="avatar-sm me-2">
                                <div className="avatar-title rounded bg-transparent text-success fs-24">
                                  <i className="ri-file-copy-2-fill"></i>
                                </div>
                              </div>
                              <div className="flex-grow-1">
                                <p className="text-muted mb-1">SKU :</p>
                                <h5 className="mb-0">{product.sku}</h5>
                              </div>
                            </div>
                          </div>
                        </div>
                        <div className="col-lg-3 col-sm-6">
                          <div className="p-2 border border-dashed rounded">
                            <div className="d-flex align-items-center">
                              <div className="avatar-sm me-2">
                                <div className="avatar-title rounded bg-transparent text-success fs-24">
                                  <i className="ri-stack-fill"></i>
                                </div>
                              </div>
                              <div className="flex-grow-1">
                                <p className="text-muted mb-1">Available Stocks :</p>
                                <h5 className="mb-0">{product.stockQuantity}</h5>
                              </div>
                            </div>
                          </div>
                        </div>
                        <div className="col-lg-3 col-sm-6">
                          <div className="p-2 border border-dashed rounded">
                            <div className="d-flex align-items-center">
                              <div className="avatar-sm me-2">
                                <div className="avatar-title rounded bg-transparent text-success fs-24">
                                  <i className="ri-inbox-archive-fill"></i>
                                </div>
                              </div>
                              <div className="flex-grow-1">
                                <p className="text-muted mb-1">Status :</p>
                                <h5 className="mb-0">{publishStatusLabel(product)}</h5>
                              </div>
                            </div>
                          </div>
                        </div>
                      </div>

                      {sizeVariants.length > 0 ? (
                        <div className="row mt-4">
                          <div className="col-xl-6">
                            <div>
                              <h5 className="fs-15">Sizes :</h5>
                              <div className="d-flex flex-wrap gap-2">
                                {sizeVariants.map((v) => (
                                  <label
                                    key={v.id}
                                    className={`btn btn-soft-primary avatar-xs rounded-circle p-0 d-flex justify-content-center align-items-center ${v.isActive ? "" : "disabled"}`}
                                    title={v.isActive ? `${v.quantity} Items Available` : "Out of Stock"}
                                  >
                                    {v.variantValue}
                                  </label>
                                ))}
                              </div>
                            </div>
                          </div>
                        </div>
                      ) : null}

                      {colorVariants.length > 0 ? (
                        <div className="row mt-2">
                          <div className="col-xl-6">
                            <div>
                              <h5 className="fs-15">Colors :</h5>
                              <div className="d-flex flex-wrap gap-2">
                                {colorVariants.map((v) => (
                                  <span
                                    key={v.id}
                                    className={`badge bg-light text-muted border ${v.isActive ? "" : "opacity-50"}`}
                                    title={v.isActive ? `${v.quantity} Items Available` : "Out of Stock"}
                                  >
                                    {v.variantValue}
                                  </span>
                                ))}
                              </div>
                            </div>
                          </div>
                        </div>
                      ) : null}

                      <div className="mt-4 text-muted">
                        <h5 className="fs-14">Description :</h5>
                        <p>{product.description || "-"}</p>
                      </div>

                      <div className="mt-4 text-muted">
                        <h5 className="fs-14">Short Description :</h5>
                        <p>{product.shortDescription || "-"}</p>
                      </div>

                      <div className="mt-4">
                        <h5 className="fs-14">Visibility :</h5>
                        <span className="badge bg-light text-muted border">{visibilityLabel(product)}</span>
                        {product.publishStatus === ProductPublishStatus.Scheduled && product.publishDate ? (
                          <span className="ms-2 text-muted">
                            Publish at: <span className="text-body fw-medium">{formatDate(product.publishDate)}</span>
                          </span>
                        ) : null}
                      </div>

                      {tags.length > 0 ? (
                        <div className="mt-4">
                          <h5 className="fs-14">Tags :</h5>
                          <div className="d-flex flex-wrap gap-2">
                            {tags.map((t) => (
                              <span key={t} className="badge bg-light text-muted border">
                                {t}
                              </span>
                            ))}
                          </div>
                        </div>
                      ) : null}

                      <div className="mt-4">
                        <Link href={routes.backToList} className="btn btn-soft-primary">
                          Back to Products
                        </Link>
                      </div>
                    </div>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>
      ) : null}
    </div>
  );
}

