"use client";

import BreadCrumb from "@/components/Common/BreadCrumb";
import CommonTable, { type CommonTableColumn } from "@/components/Common/CommonTable";
import { useRouter } from "next/navigation";
import { useCallback, useEffect, useMemo, useRef, useState } from "react";

import { lookupsService, type BaseLookup } from "@/services/lookups/lookupsService";
import { productService } from "@/services/catalog/productService";
import type { Product } from "@/types";

export type ProductsRoutes = {
  list: string;
  create: string;
  view: (id: string) => string;
  edit: (id: string) => string;
};

export default function ProductsListModule({
  title = "Products",
  pageTitle = "Ecommerce",
  routes,
}: {
  title?: string;
  pageTitle?: string;
  routes: ProductsRoutes;
}) {
  const router = useRouter();
  const lookupsOnceRef = useRef(false);
  const [reloadToken, setReloadToken] = useState(0);
  const [isLoading, setIsLoading] = useState(false);
  const [productsError, setProductsError] = useState<string | null>(null);
  const [data, setData] = useState<Product[]>([]);
  const [currentPage, setCurrentPage] = useState(1);
  const [pageSize, setPageSize] = useState(10);
  const [totalCount, setTotalCount] = useState(0);
  const [searchTerm, setSearchTerm] = useState("");

  const [allCount, setAllCount] = useState(0);
  const [publishedCount, setPublishedCount] = useState(0);
  const [draftCount, setDraftCount] = useState(0);

  const [categories, setCategories] = useState<BaseLookup[]>([]);
  const [brands, setBrands] = useState<BaseLookup[]>([]);
  const [brandsSearch, setBrandsSearch] = useState("");

  const [activeTab, setActiveTab] = useState<"all" | "published" | "draft">("all");

  const [categoryId, setCategoryId] = useState<string | undefined>(undefined);
  const [brandIds, setBrandIds] = useState<string[]>([]);
  const [minPrice, setMinPrice] = useState<string>("");
  const [maxPrice, setMaxPrice] = useState<string>("");
  const [discountThresholds, setDiscountThresholds] = useState<number[]>([]);
  const [ratingThresholds, setRatingThresholds] = useState<number[]>([]);

  const [lookupsError, setLookupsError] = useState<string | null>(null);

  useEffect(() => {
    if (lookupsOnceRef.current) return;
    lookupsOnceRef.current = true;

    (async () => {
      const [cats, brs] = await Promise.all([lookupsService.getCategories(), lookupsService.getBrands()]);
      setCategories(cats);
      setBrands(brs);
    })().catch(() => {
      setLookupsError("Failed to load lookups");
    });
  }, []);

  const isActiveFilter = useMemo(() => {
    if (activeTab === "published") return true;
    if (activeTab === "draft") return false;
    return undefined;
  }, [activeTab]);

  const minDiscountPercentage = useMemo(() => {
    if (discountThresholds.length === 0) return undefined;
    return Math.max(...discountThresholds);
  }, [discountThresholds]);

  const minRating = useMemo(() => {
    if (ratingThresholds.length === 0) return undefined;
    return Math.max(...ratingThresholds);
  }, [ratingThresholds]);

  const filtersRef = useRef({
    isActiveFilter: undefined as boolean | undefined,
    categoryId: undefined as string | undefined,
    brandIds: [] as string[],
    minPrice: undefined as number | undefined,
    maxPrice: undefined as number | undefined,
    minDiscountPercentage: undefined as number | undefined,
    minRating: undefined as number | undefined,
  });

  useEffect(() => {
    filtersRef.current = {
      isActiveFilter,
      categoryId,
      brandIds,
      minPrice: minPrice ? Number(minPrice) : undefined,
      maxPrice: maxPrice ? Number(maxPrice) : undefined,
      minDiscountPercentage,
      minRating,
    };
  }, [isActiveFilter, categoryId, brandIds, minPrice, maxPrice, minDiscountPercentage, minRating]);

  const loadData = useCallback(async () => {
    setIsLoading(true);
    setProductsError(null);

    const f = filtersRef.current;

    try {
      const response = await productService.getProductPaginatedList(currentPage, pageSize, searchTerm || undefined, 0, {
        categoryId: f.categoryId,
        brandIds: f.brandIds.length > 0 ? f.brandIds : undefined,
        isActive: f.isActiveFilter,
        minPrice: Number.isFinite(f.minPrice as number) ? f.minPrice : undefined,
        maxPrice: Number.isFinite(f.maxPrice as number) ? f.maxPrice : undefined,
        minDiscountPercentage: f.minDiscountPercentage,
        minRating: f.minRating,
      });

      setAllCount(response.meta?.allCount ?? response.totalCount ?? 0);
      setPublishedCount(response.meta?.publishedCount ?? 0);
      setDraftCount(response.meta?.draftCount ?? 0);
      setData(response.data || []);
      setTotalCount(response.totalCount || 0);
    } catch {
      setProductsError("Failed to load products");
      setData([]);
      setTotalCount(0);
    } finally {
      setIsLoading(false);
    }
  }, [currentPage, pageSize, searchTerm, isActiveFilter, categoryId, brandIds, minPrice, maxPrice, minDiscountPercentage, minRating]);

  useEffect(() => {
    loadData();
  }, [loadData, reloadToken]);

  const handleSearchChange = useCallback((term: string) => {
    setSearchTerm(term);
    setCurrentPage(1);
  }, []);

  const columns = useMemo<Array<CommonTableColumn<Product>>>(() => {
    return [
      {
        key: "name",
        title: "Product",
        render: (_v, row) => {
          const imgs = (row as unknown as { productImages?: Array<{ imageURL?: string; isPrimary?: boolean }> }).productImages || [];
          const primary =
            imgs.find((i) => i.isPrimary)?.imageURL ||
            imgs[0]?.imageURL ||
            "/assets/images/products/img-8.png";

          return (
            <div className="d-flex align-items-center">
              <div className="flex-shrink-0 me-3">
                <img src={primary} className="avatar-sm rounded bg-light object-fit-cover" alt="" />
              </div>
              <div className="flex-grow-1">
                <h6 className="fs-14 mb-1">{row.name}</h6>
                <div className="text-muted fs-12">SKU: {row.sku}</div>
              </div>
            </div>
          );
        },
      },
      { key: "categoryName", title: "Category", render: (v) => (v ? String(v) : "-") },
      {
        key: "stockQuantity",
        title: "Stock",
        className: "text-center",
        render: (v, row) => {
          const stock = Number(v ?? 0);
          const alert = Number(row.quantityAlert ?? 0);
          const badge = stock <= alert ? "bg-soft-danger text-danger" : "bg-soft-success text-success";
          return <span className={`badge ${badge}`}>{stock}</span>;
        },
      },
      {
        key: "price",
        title: "Price",
        render: (v) => {
          const n = Number(v ?? 0);
          return `$${n.toFixed(2)}`;
        },
      },
      {
        key: "isActive",
        title: "Status",
        render: (v) => {
          const active = Boolean(v);
          const badge = active ? "bg-soft-success text-success" : "bg-soft-warning text-warning";
          const label = active ? "Published" : "Draft";
          return <span className={`badge ${badge}`}>{label}</span>;
        },
      },
    ];
  }, []);

  const filteredBrands = useMemo(() => {
    const q = brandsSearch.trim().toLowerCase();
    if (!q) return brands;
    return brands.filter((b) => b.name.toLowerCase().includes(q));
  }, [brands, brandsSearch]);

  const countAll = allCount;
  const countPublished = publishedCount;
  const countDraft = draftCount;

  const clearAll = () => {
    setCategoryId(undefined);
    setBrandIds([]);
    setMinPrice("");
    setMaxPrice("");
    setDiscountThresholds([]);
    setRatingThresholds([]);
    setActiveTab("all");
    setCurrentPage(1);
    setReloadToken((v) => v + 1);
  };

  const applyFilters = () => {
    setCurrentPage(1);
    setReloadToken((v) => v + 1);
  };

  const handleView = useCallback(
    (row: Product) => {
      router.push(routes.view(row.id));
    },
    [router, routes]
  );

  const handleEdit = useCallback(
    (row: Product) => {
      router.push(routes.edit(row.id));
    },
    [router, routes]
  );

  const handleDelete = useCallback(
    async (row: Product) => {
      const ok = window.confirm("Are you sure you want to delete this product?");
      if (!ok) return;
      try {
        await productService.deleteProduct(row.id);
        setReloadToken((v) => v + 1);
        setCurrentPage(1);
      } catch {
        setProductsError("Failed to delete product");
      }
    },
    []
  );

  return (
    <div className="container-fluid">
      <BreadCrumb
        items={[
          { label: pageTitle, icon: "ri-store-line" },
          { label: title, icon: "ri-shopping-bag-line" },
        ]}
      />

      {lookupsError ? (
        <div className="alert alert-danger" role="alert">
          {lookupsError}
        </div>
      ) : null}

      {productsError ? (
        <div className="alert alert-danger" role="alert">
          {productsError}
        </div>
      ) : null}

      <div className="row">
        <div className="col-xl-3 col-lg-4">
          <div className="card">
            <div className="card-header">
              <div className="d-flex mb-3">
                <div className="flex-grow-1">
                  <h5 className="fs-16">Filters</h5>
                </div>
                <div className="flex-shrink-0">
                  <button type="button" className="btn btn-link text-decoration-underline p-0" onClick={clearAll}>
                    Clear All
                  </button>
                </div>
              </div>
            </div>

            <div className="accordion accordion-flush filter-accordion">
              <div className="card-body border-bottom">
                <div>
                  <p className="text-muted text-uppercase fs-13 fw-medium mb-2">Categories</p>
                  <ul className="list-unstyled mb-0 filter-list">
                    <li>
                      <button
                        type="button"
                        className="btn btn-link text-decoration-none d-flex py-1 align-items-center p-0 w-100"
                        onClick={() => {
                          setCategoryId(undefined);
                        }}
                      >
                        <div className="flex-grow-1">
                          <h5 className="fs-14 mb-0 listname">All</h5>
                        </div>
                      </button>
                    </li>
                    {categories.map((c) => (
                      <li key={c.id}>
                        <button
                          type="button"
                          className="btn btn-link text-decoration-none d-flex py-1 align-items-center p-0 w-100"
                          onClick={() => {
                            setCategoryId(c.id);
                          }}
                        >
                          <div className="flex-grow-1">
                            <h5 className="fs-14 mb-0 listname">{c.name}</h5>
                          </div>
                        </button>
                      </li>
                    ))}
                  </ul>
                </div>
              </div>

              <div className="card-body border-bottom">
                <p className="text-muted text-uppercase fs-13 fw-medium mb-4">Price</p>
                <div className="formCost d-flex gap-2 align-items-center mt-3">
                  <input
                    className="form-control form-control-sm"
                    type="number"
                    value={minPrice}
                    onChange={(e) => setMinPrice(e.target.value)}
                    placeholder="Min"
                  />
                  <span className="fw-semibold text-muted">to</span>
                  <input
                    className="form-control form-control-sm"
                    type="number"
                    value={maxPrice}
                    onChange={(e) => setMaxPrice(e.target.value)}
                    placeholder="Max"
                  />
                </div>
                <div className="mt-3">
                  <button type="button" className="btn btn-soft-primary w-100" onClick={applyFilters}>
                    Apply
                  </button>
                </div>
              </div>

              <div className="accordion-item">
                <h2 className="accordion-header" id="flush-headingBrands">
                  <button
                    className="accordion-button bg-transparent shadow-none"
                    type="button"
                    data-bs-toggle="collapse"
                    data-bs-target="#flush-collapseBrands"
                    aria-expanded="true"
                    aria-controls="flush-collapseBrands"
                  >
                    <span className="text-muted text-uppercase fs-13 fw-medium">Brands</span>
                  </button>
                </h2>

                <div id="flush-collapseBrands" className="accordion-collapse collapse show" aria-labelledby="flush-headingBrands">
                  <div className="accordion-body text-body pt-0">
                    <div className="search-box search-box-sm">
                      <input
                        type="text"
                        className="form-control bg-light border-0"
                        value={brandsSearch}
                        onChange={(e) => setBrandsSearch(e.target.value)}
                        placeholder="Search Brands..."
                      />
                      <i className="ri-search-line search-icon"></i>
                    </div>
                    <div className="d-flex flex-column gap-2 mt-3 filter-check" style={{ maxHeight: 240, overflow: "auto" }}>
                      {filteredBrands.map((b) => {
                        const checked = brandIds.includes(b.id);
                        return (
                          <div className="form-check" key={b.id}>
                            <input
                              className="form-check-input"
                              type="checkbox"
                              value={b.id}
                              id={`productBrand-${b.id}`}
                              checked={checked}
                              onChange={() => {
                                setBrandIds((prev) => (prev.includes(b.id) ? prev.filter((x) => x !== b.id) : [...prev, b.id]));
                              }}
                            />
                            <label className="form-check-label" htmlFor={`productBrand-${b.id}`}>
                              {b.name}
                            </label>
                          </div>
                        );
                      })}
                    </div>
                  </div>
                </div>
              </div>

              <div className="accordion-item">
                <h2 className="accordion-header" id="flush-headingDiscount">
                  <button
                    className="accordion-button bg-transparent shadow-none collapsed"
                    type="button"
                    data-bs-toggle="collapse"
                    data-bs-target="#flush-collapseDiscount"
                    aria-expanded="false"
                    aria-controls="flush-collapseDiscount"
                  >
                    <span className="text-muted text-uppercase fs-13 fw-medium">Discount</span>
                  </button>
                </h2>
                <div id="flush-collapseDiscount" className="accordion-collapse collapse" aria-labelledby="flush-headingDiscount">
                  <div className="accordion-body text-body pt-1">
                    <div className="d-flex flex-column gap-2 filter-check">
                      {[50, 40, 30, 20, 10].map((v) => (
                        <div className="form-check" key={v}>
                          <input
                            className="form-check-input"
                            type="checkbox"
                            value={v}
                            id={`productdiscount-${v}`}
                            checked={discountThresholds.includes(v)}
                            onChange={() => {
                              setDiscountThresholds((prev) => (prev.includes(v) ? prev.filter((x) => x !== v) : [...prev, v]));
                            }}
                          />
                          <label className="form-check-label" htmlFor={`productdiscount-${v}`}>
                            {v}% or more
                          </label>
                        </div>
                      ))}
                    </div>
                  </div>
                </div>
              </div>

              <div className="accordion-item">
                <h2 className="accordion-header" id="flush-headingRating">
                  <button
                    className="accordion-button bg-transparent shadow-none collapsed"
                    type="button"
                    data-bs-toggle="collapse"
                    data-bs-target="#flush-collapseRating"
                    aria-expanded="false"
                    aria-controls="flush-collapseRating"
                  >
                    <span className="text-muted text-uppercase fs-13 fw-medium">Rating</span>
                  </button>
                </h2>

                <div id="flush-collapseRating" className="accordion-collapse collapse" aria-labelledby="flush-headingRating">
                  <div className="accordion-body text-body">
                    <div className="d-flex flex-column gap-2 filter-check">
                      {[4, 3, 2, 1].map((v) => (
                        <div className="form-check" key={v}>
                          <input
                            className="form-check-input"
                            type="checkbox"
                            value={v}
                            id={`productrating-${v}`}
                            checked={ratingThresholds.includes(v)}
                            onChange={() => {
                              setRatingThresholds((prev) => (prev.includes(v) ? prev.filter((x) => x !== v) : [...prev, v]));
                            }}
                          />
                          <label className="form-check-label" htmlFor={`productrating-${v}`}>
                            {v} & Above
                          </label>
                        </div>
                      ))}
                    </div>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>

        <div className="col-xl-9 col-lg-8">
          <div className="card">
            <div className="card-header border-0">
              <div className="row g-4">
                <div className="col-sm-auto">
                  <div>
                    <button
                      type="button"
                      className="btn btn-success"
                      id="addproduct-btn"
                      onClick={() => router.push(routes.create)}
                    >
                      <i className="ri-add-line align-bottom me-1"></i> Add Product
                    </button>
                  </div>
                </div>
              </div>
            </div>

            <div className="card-header">
              <div className="row align-items-center">
                <div className="col">
                  <ul className="nav nav-tabs-custom card-header-tabs border-bottom-0" role="tablist">
                    <li className="nav-item">
                      <button
                        type="button"
                        className={`nav-link fw-semibold ${activeTab === "all" ? "active" : ""}`}
                        onClick={() => {
                          setActiveTab("all");
                          setCurrentPage(1);
                          setReloadToken((v) => v + 1);
                        }}
                      >
                        All{" "}
                        <span className="badge badge-soft-danger align-middle rounded-pill ms-1">{countAll}</span>
                      </button>
                    </li>
                    <li className="nav-item">
                      <button
                        type="button"
                        className={`nav-link fw-semibold ${activeTab === "published" ? "active" : ""}`}
                        onClick={() => {
                          setActiveTab("published");
                          setCurrentPage(1);
                          setReloadToken((v) => v + 1);
                        }}
                      >
                        Published{" "}
                        <span className="badge badge-soft-danger align-middle rounded-pill ms-1">{countPublished}</span>
                      </button>
                    </li>
                    <li className="nav-item">
                      <button
                        type="button"
                        className={`nav-link fw-semibold ${activeTab === "draft" ? "active" : ""}`}
                        onClick={() => {
                          setActiveTab("draft");
                          setCurrentPage(1);
                          setReloadToken((v) => v + 1);
                        }}
                      >
                        Draft{" "}
                        <span className="badge badge-soft-danger align-middle rounded-pill ms-1">{countDraft}</span>
                      </button>
                    </li>
                  </ul>
                </div>
              </div>
            </div>

            <div className="card-body">
              <CommonTable<Product>
                columns={columns}
                data={data}
                loading={isLoading}
                searchable
                searchTerm={searchTerm}
                onSearchChange={handleSearchChange}
                searchPlaceholder="Search products..."
                pagination={{
                  currentPage,
                  pageSize,
                  total: totalCount,
                  onPageChange: setCurrentPage,
                  onPageSizeChange: (size) => {
                    setPageSize(size);
                    setCurrentPage(1);
                  },
                }}
                renderActions={(row) => (
                  <div className="d-flex gap-2">
                    <button
                      type="button"
                      className="btn btn-sm btn-soft-info"
                      onClick={() => handleView(row)}
                      title="View"
                    >
                      <i className="ri-eye-line"></i>
                    </button>
                    <button
                      type="button"
                      className="btn btn-sm btn-soft-success"
                      onClick={() => handleEdit(row)}
                      title="Edit"
                    >
                      <i className="ri-pencil-line"></i>
                    </button>
                    <button
                      type="button"
                      className="btn btn-sm btn-soft-danger"
                      onClick={() => handleDelete(row)}
                      title="Delete"
                    >
                      <i className="ri-delete-bin-line"></i>
                    </button>
                  </div>
                )}
                emptyMessage="No products found"
              />
            </div>
          </div>
        </div>
      </div>
    </div>
  );
}

