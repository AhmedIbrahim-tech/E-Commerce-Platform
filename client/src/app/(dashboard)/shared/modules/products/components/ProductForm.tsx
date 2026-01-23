"use client";

import Link from "next/link";
import { useRouter } from "next/navigation";
import { useEffect, useMemo, useRef, useState } from "react";

import { lookupsService, type BaseLookup, type EnumLookup } from "@/services/lookups/lookupsService";
import { productService } from "@/services/catalog/productService";
import {
  CreateProductRequest,
  DiscountType,
  Product,
  ProductImageDto,
  ProductPublishStatus,
  ProductType,
  ProductVisibility,
  SellingType,
  TaxType,
  UpdateProductRequest,
} from "@/types";

function safeNumber(value: string, fallback: number): number {
  const n = Number(value);
  return Number.isFinite(n) ? n : fallback;
}

function toLocalDateTimeInputValue(iso?: string): string {
  if (!iso) return "";
  const d = new Date(iso);
  if (Number.isNaN(d.getTime())) return "";
  const pad = (n: number) => String(n).padStart(2, "0");
  const yyyy = d.getFullYear();
  const mm = pad(d.getMonth() + 1);
  const dd = pad(d.getDate());
  const hh = pad(d.getHours());
  const min = pad(d.getMinutes());
  return `${yyyy}-${mm}-${dd}T${hh}:${min}`;
}

function fromLocalDateTimeInputValue(value: string): string | undefined {
  if (!value) return undefined;
  const d = new Date(value);
  if (Number.isNaN(d.getTime())) return undefined;
  return d.toISOString();
}

function slugify(value: string): string {
  return value
    .trim()
    .toLowerCase()
    .replace(/[^\w\s-]/g, "")
    .replace(/\s+/g, "-")
    .replace(/-+/g, "-");
}

function firstPrimaryImageUrl(p?: Product | null): string | null {
  const images = p?.productImages || [];
  const primary = images.find((i) => i.isPrimary);
  return (primary?.imageURL || images[0]?.imageURL) ?? null;
}

export default function ProductForm({ productId, afterSaveHref }: { productId?: string; afterSaveHref?: string }) {
  const router = useRouter();
  const isEdit = Boolean(productId);

  const subCategoriesFetchKeyRef = useRef<string>("");
  const productFetchKeyRef = useRef<string>("");
  const didLoadLookupsRef = useRef(false);

  const [isLoading, setIsLoading] = useState(true);
  const [isSaving, setIsSaving] = useState(false);
  const [error, setError] = useState<string | null>(null);

  const [categories, setCategories] = useState<BaseLookup[]>([]);
  const [subCategories, setSubCategories] = useState<BaseLookup[]>([]);
  const [brands, setBrands] = useState<BaseLookup[]>([]);
  const [units, setUnits] = useState<BaseLookup[]>([]);
  const [warranties, setWarranties] = useState<BaseLookup[]>([]);
  const [tagsLookup, setTagsLookup] = useState<BaseLookup[]>([]);

  const [publishStatuses, setPublishStatuses] = useState<EnumLookup[]>([]);
  const [visibilities, setVisibilities] = useState<EnumLookup[]>([]);
  const [productTypes, setProductTypes] = useState<EnumLookup[]>([]);
  const [sellingTypes, setSellingTypes] = useState<EnumLookup[]>([]);
  const [taxTypes, setTaxTypes] = useState<EnumLookup[]>([]);
  const [discountTypes, setDiscountTypes] = useState<EnumLookup[]>([]);

  const [name, setName] = useState("");
  const [slug, setSlug] = useState("");
  const [sku, setSku] = useState("");
  const [description, setDescription] = useState<string>("");
  const [shortDescription, setShortDescription] = useState<string>("");

  const [stockQuantity, setStockQuantity] = useState("0");
  const [price, setPrice] = useState("0");
  const [quantityAlert, setQuantityAlert] = useState("0");

  const [manufacturer, setManufacturer] = useState<string>("");
  const [barcode, setBarcode] = useState<string>("");
  const [barcodeSymbology, setBarcodeSymbology] = useState<string>("");

  const [selectedCategoryId, setSelectedCategoryId] = useState<string>("");
  const [selectedSubCategoryId, setSelectedSubCategoryId] = useState<string>("");
  const [selectedBrandId, setSelectedBrandId] = useState<string>("");
  const [selectedUnitId, setSelectedUnitId] = useState<string>("");
  const [selectedWarrantyId, setSelectedWarrantyId] = useState<string>("");

  const [tags, setTags] = useState<string[]>([]);
  const [tagInput, setTagInput] = useState<string>("");

  const [selectedPublishStatus, setSelectedPublishStatus] = useState<number | "">("");
  const [selectedVisibility, setSelectedVisibility] = useState<number | "">("");
  const [publishDateLocal, setPublishDateLocal] = useState<string>("");

  const [selectedProductType, setSelectedProductType] = useState<number>(1);
  const [selectedSellingType, setSelectedSellingType] = useState<number>(1);
  const [selectedTaxType, setSelectedTaxType] = useState<number | "">("");
  const [taxRate, setTaxRate] = useState<string>("");
  const [selectedDiscountType, setSelectedDiscountType] = useState<number | "">("");
  const [discountValue, setDiscountValue] = useState<string>("");

  const [existingProduct, setExistingProduct] = useState<Product | null>(null);
  const existingPrimaryImage = useMemo(() => firstPrimaryImageUrl(existingProduct), [existingProduct]);

  const [primaryImageFile, setPrimaryImageFile] = useState<File | null>(null);
  const [galleryFiles, setGalleryFiles] = useState<File[]>([]);

  const primaryImagePreviewUrl = useMemo(
    () => (primaryImageFile ? URL.createObjectURL(primaryImageFile) : null),
    [primaryImageFile]
  );

  useEffect(() => {
    return () => {
      if (primaryImagePreviewUrl) URL.revokeObjectURL(primaryImagePreviewUrl);
    };
  }, [primaryImagePreviewUrl]);

  useEffect(() => {
    if (didLoadLookupsRef.current) return;
    didLoadLookupsRef.current = true;

    (async () => {
      const [
        cats,
        brs,
        uoms,
        wars,
        tagsList,
        statuses,
        vis,
        pTypes,
        sTypes,
        tTypes,
        dTypes,
      ] = await Promise.all([
        lookupsService.getCategories(),
        lookupsService.getBrands(),
        lookupsService.getUnitOfMeasures(),
        lookupsService.getWarranties(),
        lookupsService.getTags(),
        lookupsService.getProductPublishStatuses(),
        lookupsService.getProductVisibilities(),
        lookupsService.getProductTypes(),
        lookupsService.getSellingTypes(),
        lookupsService.getTaxTypes(),
        lookupsService.getDiscountTypes(),
      ]);

      setCategories(cats);
      setBrands(brs);
      setUnits(uoms);
      setWarranties(wars);
      setTagsLookup(tagsList);
      setPublishStatuses(statuses);
      setVisibilities(vis);
      setProductTypes(pTypes);
      setSellingTypes(sTypes);
      setTaxTypes(tTypes);
      setDiscountTypes(dTypes);

      const defaultStatusId = statuses.find((s) => s.name === "Published")?.id ?? statuses[0]?.id;
      const defaultVisibilityId = vis.find((v) => v.name === "Public")?.id ?? vis[0]?.id;
      if (defaultStatusId !== undefined) setSelectedPublishStatus(defaultStatusId);
      if (defaultVisibilityId !== undefined) setSelectedVisibility(defaultVisibilityId);
    })().catch(() => {
      setError("Failed to load lookups");
    });
  }, []);

  useEffect(() => {
    if (!selectedCategoryId) {
      setSubCategories([]);
      setSelectedSubCategoryId("");
      return;
    }

    const key = `subcats:${selectedCategoryId}`;
    if (subCategoriesFetchKeyRef.current === key) return;
    subCategoriesFetchKeyRef.current = key;

    (async () => {
      try {
        const subs = await lookupsService.getSubCategoriesByCategory(selectedCategoryId);
        setSubCategories(subs);
      } catch {
        setSubCategories([]);
      }
    })();
  }, [selectedCategoryId]);

  useEffect(() => {
    const key = `product:${productId || "new"}`;
    if (productFetchKeyRef.current === key) return;
    productFetchKeyRef.current = key;

    (async () => {
      if (!productId) {
        setIsLoading(false);
        return;
      }

      try {
        const p = await productService.getProductById(productId);
        setExistingProduct(p);

        setName(p.name || "");
        setSlug(p.slug || "");
        setSku(p.sku || "");
        setDescription(p.description || "");
        setShortDescription(p.shortDescription || "");

        setPrice(String(p.price ?? 0));
        setStockQuantity(String(p.stockQuantity ?? 0));
        setQuantityAlert(String(p.quantityAlert ?? 0));

        setManufacturer(p.manufacturer || "");
        setBarcode(p.barcode || "");
        setBarcodeSymbology(p.barcodeSymbology || "");

        setSelectedCategoryId(p.categoryId || "");
        setSelectedSubCategoryId(p.subCategoryId || "");
        setSelectedBrandId(p.brandId || "");
        setSelectedUnitId(p.unitOfMeasureId || "");
        setSelectedWarrantyId(p.warrantyId || "");

        setSelectedPublishStatus(p.publishStatus ? Number(p.publishStatus) : p.isActive ? 1 : 3);
        setSelectedVisibility(p.visibility ? Number(p.visibility) : 1);
        setPublishDateLocal(toLocalDateTimeInputValue(p.publishDate));
        setTags(p.tags || []);

        setSelectedProductType(Number(p.productType || 1));
        setSelectedSellingType(Number(p.sellingType || 1));
        setSelectedTaxType(p.taxType ? Number(p.taxType) : "");
        setTaxRate(p.taxRate !== undefined && p.taxRate !== null ? String(p.taxRate) : "");
        setSelectedDiscountType(p.discountType ? Number(p.discountType) : "");
        setDiscountValue(p.discountValue !== undefined && p.discountValue !== null ? String(p.discountValue) : "");
      } catch (e) {
        setError(e instanceof Error ? e.message : "Failed to load product");
      } finally {
        setIsLoading(false);
      }
    })();
  }, [productId]);

  useEffect(() => {
    setSlug((prev) => (prev ? prev : slugify(name)));
  }, [name]);

  const primaryImageDisplay = primaryImagePreviewUrl || existingPrimaryImage || "/assets/images/products/img-8.png";

  const onSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setError(null);

    if (!name.trim() || !slug.trim() || !sku.trim()) {
      setError("Please fill Product Title, Slug, and SKU.");
      return;
    }
    if (!selectedCategoryId) {
      setError("Please select a product category.");
      return;
    }

    if (selectedPublishStatus === "") {
      setError("Please select a publish status.");
      return;
    }
    if (selectedVisibility === "") {
      setError("Please select a visibility.");
      return;
    }

    const publishDate = fromLocalDateTimeInputValue(publishDateLocal);
    if (selectedPublishStatus === ProductPublishStatus.Scheduled && !publishDate) {
      setError("Please select Publish Date & Time when status is Scheduled.");
      return;
    }

    const isActive =
      selectedPublishStatus === ProductPublishStatus.Published ||
      (selectedPublishStatus === ProductPublishStatus.Scheduled &&
        publishDate !== undefined &&
        new Date(publishDate).getTime() <= Date.now());

    const normalizedTags = tags
      .map((t) => t.trim())
      .filter(Boolean)
      .filter((t, idx, arr) => arr.findIndex((x) => x.toLowerCase() === t.toLowerCase()) === idx);

    const images: ProductImageDto[] = [];
    if (primaryImageFile) {
      images.push({ imageFile: primaryImageFile, isPrimary: true, displayOrder: 0 });
    }
    if (galleryFiles.length > 0) {
      galleryFiles.forEach((f, idx) => images.push({ imageFile: f, isPrimary: false, displayOrder: idx + 1 }));
    }

    try {
      setIsSaving(true);

      if (!isEdit) {
        const payload: CreateProductRequest = {
          name: name.trim(),
          slug: slug.trim(),
          sku: sku.trim(),
          description: description || undefined,
          shortDescription: shortDescription || undefined,
          price: safeNumber(price, 0),
          stockQuantity: safeNumber(stockQuantity, 0),
          quantityAlert: safeNumber(quantityAlert, 0),
          barcode: barcode || undefined,
          barcodeSymbology: barcodeSymbology || undefined,
          publishStatus: selectedPublishStatus as ProductPublishStatus,
          visibility: selectedVisibility as ProductVisibility,
          publishDate,
          tags: normalizedTags.length > 0 ? normalizedTags : undefined,
          productType: selectedProductType as unknown as ProductType,
          sellingType: selectedSellingType as unknown as SellingType,
          taxType: selectedTaxType === "" ? undefined : (selectedTaxType as unknown as TaxType),
          taxRate: taxRate ? safeNumber(taxRate, 0) : undefined,
          discountType:
            selectedDiscountType === "" ? undefined : (selectedDiscountType as unknown as DiscountType),
          discountValue: discountValue ? safeNumber(discountValue, 0) : undefined,
          categoryId: selectedCategoryId,
          subCategoryId: selectedSubCategoryId || undefined,
          brandId: selectedBrandId || undefined,
          unitOfMeasureId: selectedUnitId || undefined,
          warrantyId: selectedWarrantyId || undefined,
          manufacturer: manufacturer || undefined,
          productImages: images.length > 0 ? images : undefined,
        };

        await productService.createProduct(payload);
      } else {
        const payload: UpdateProductRequest = {
          id: productId!,
          name: name.trim(),
          slug: slug.trim(),
          sku: sku.trim(),
          description: description || undefined,
          shortDescription: shortDescription || undefined,
          price: safeNumber(price, 0),
          stockQuantity: safeNumber(stockQuantity, 0),
          quantityAlert: safeNumber(quantityAlert, 0),
          barcode: barcode || undefined,
          barcodeSymbology: barcodeSymbology || undefined,
          publishStatus: selectedPublishStatus as ProductPublishStatus,
          visibility: selectedVisibility as ProductVisibility,
          publishDate,
          tags: normalizedTags.length > 0 ? normalizedTags : undefined,
          replaceTags: true,
          productType: selectedProductType as unknown as ProductType,
          sellingType: selectedSellingType as unknown as SellingType,
          taxType: selectedTaxType === "" ? undefined : (selectedTaxType as unknown as TaxType),
          taxRate: taxRate ? safeNumber(taxRate, 0) : undefined,
          discountType:
            selectedDiscountType === "" ? undefined : (selectedDiscountType as unknown as DiscountType),
          discountValue: discountValue ? safeNumber(discountValue, 0) : undefined,
          categoryId: selectedCategoryId,
          subCategoryId: selectedSubCategoryId || undefined,
          brandId: selectedBrandId || undefined,
          unitOfMeasureId: selectedUnitId || undefined,
          warrantyId: selectedWarrantyId || undefined,
          manufacturer: manufacturer || undefined,
          isActive,
          productImages: images.length > 0 ? images : undefined,
        };

        await productService.updateProduct(payload);
      }

      router.replace(afterSaveHref || "/merchant/modules/products");
    } catch (err) {
      setError(err instanceof Error ? err.message : "Failed to save product");
    } finally {
      setIsSaving(false);
    }
  };

  if (isLoading) {
    return (
      <div className="container-fluid">
        <div className="card">
          <div className="card-body text-muted">Loading...</div>
        </div>
      </div>
    );
  }

  return (
    <div className="container-fluid">
      {error ? (
        <div className="alert alert-danger" role="alert">
          {error}
        </div>
      ) : null}

      <form id="createproduct-form" autoComplete="off" className="needs-validation" onSubmit={onSubmit}>
        <div className="row">
          <div className="col-lg-8">
            <div className="card">
              <div className="card-body">
                <div className="mb-3">
                  <label className="form-label" htmlFor="product-title-input">
                    Product Title
                  </label>
                  <input type="hidden" className="form-control" id="formAction" name="formAction" value={isEdit ? "edit" : "add"} />
                  <input type="text" className="form-control" id="product-title-input" value={name} onChange={(e) => setName(e.target.value)} placeholder="Enter product title" required />
                </div>

                <div className="row">
                  <div className="col-lg-6">
                    <div className="mb-3">
                      <label className="form-label" htmlFor="product-slug-input">
                        Slug
                      </label>
                      <input type="text" className="form-control" id="product-slug-input" value={slug} onChange={(e) => setSlug(e.target.value)} placeholder="Enter slug" required />
                    </div>
                  </div>
                  <div className="col-lg-6">
                    <div className="mb-3">
                      <label className="form-label" htmlFor="product-sku-input">
                        SKU
                      </label>
                      <input type="text" className="form-control" id="product-sku-input" value={sku} onChange={(e) => setSku(e.target.value)} placeholder="Enter SKU" required />
                    </div>
                  </div>
                </div>

                <div>
                  <label className="form-label" htmlFor="product-description-input">
                    Product Description
                  </label>
                  <textarea
                    id="product-description-input"
                    className="form-control"
                    rows={6}
                    value={description}
                    onChange={(e) => setDescription(e.target.value)}
                    placeholder="Enter product description"
                  />
                </div>
              </div>
            </div>

            <div className="card">
              <div className="card-header">
                <h5 className="card-title mb-0">Product Gallery</h5>
              </div>
              <div className="card-body">
                <div className="mb-4">
                  <h5 className="fs-15 mb-1">Product Image</h5>
                  <p className="text-muted">Add Product main Image.</p>
                  <div className="text-center">
                    <div className="position-relative d-inline-block">
                      <div className="position-absolute top-100 start-100 translate-middle">
                        <label htmlFor="product-image-input" className="mb-0" title="Select Image">
                          <div className="avatar-xs">
                            <div className="avatar-title bg-light border rounded-circle text-muted cursor-pointer">
                              <i className="ri-image-fill"></i>
                            </div>
                          </div>
                        </label>
                        <input
                          className="form-control d-none"
                          id="product-image-input"
                          type="file"
                          accept="image/png, image/gif, image/jpeg"
                          onChange={(e) => setPrimaryImageFile(e.target.files?.[0] || null)}
                        />
                      </div>
                      <div className="avatar-lg">
                        <div className="avatar-title bg-light rounded">
                          <img src={primaryImageDisplay} alt="" id="product-img" className="avatar-md h-auto" />
                        </div>
                      </div>
                    </div>
                  </div>
                </div>

                <div>
                  <h5 className="fs-15 mb-1">Product Gallery</h5>
                  <p className="text-muted">Add Product Gallery Images.</p>
                  <input
                    className="form-control"
                    type="file"
                    accept="image/png, image/gif, image/jpeg"
                    multiple
                    onChange={(e) => setGalleryFiles(Array.from(e.target.files || []))}
                  />
                  {galleryFiles.length > 0 ? (
                    <ul className="list-unstyled mb-0 mt-3">
                      {galleryFiles.map((f, idx) => (
                        <li key={`${f.name}-${idx}`} className="mt-2">
                          <div className="border rounded">
                            <div className="d-flex p-2">
                              <div className="flex-grow-1">
                                <div className="pt-1">
                                  <h5 className="fs-14 mb-1">{f.name}</h5>
                                  <p className="fs-13 text-muted mb-0">{Math.round(f.size / 1024)} KB</p>
                                </div>
                              </div>
                              <div className="flex-shrink-0 ms-3">
                                <button
                                  type="button"
                                  className="btn btn-sm btn-danger"
                                  onClick={() => setGalleryFiles((prev) => prev.filter((_, i) => i !== idx))}
                                >
                                  Delete
                                </button>
                              </div>
                            </div>
                          </div>
                        </li>
                      ))}
                    </ul>
                  ) : null}
                </div>
              </div>
            </div>

            <div className="card">
              <div className="card-header">
                <ul className="nav nav-tabs-custom card-header-tabs border-bottom-0" role="tablist">
                  <li className="nav-item">
                    <a className="nav-link active" data-bs-toggle="tab" href="#addproduct-general-info" role="tab">
                      General Info
                    </a>
                  </li>
                  <li className="nav-item">
                    <a className="nav-link" data-bs-toggle="tab" href="#addproduct-metadata" role="tab">
                      Meta Data
                    </a>
                  </li>
                </ul>
              </div>
              <div className="card-body">
                <div className="tab-content">
                  <div className="tab-pane active" id="addproduct-general-info" role="tabpanel">
                    <div className="row">
                      <div className="col-lg-6">
                        <div className="mb-3">
                          <label className="form-label" htmlFor="manufacturer-name-input">
                            Manufacturer Name
                          </label>
                          <input
                            type="text"
                            className="form-control"
                            id="manufacturer-name-input"
                            placeholder="Enter manufacturer name"
                            value={manufacturer}
                            onChange={(e) => setManufacturer(e.target.value)}
                          />
                        </div>
                      </div>
                      <div className="col-lg-6">
                        <div className="mb-3">
                          <label className="form-label" htmlFor="manufacturer-brand-input">
                            Barcode Symbology
                          </label>
                          <input
                            type="text"
                            className="form-control"
                            id="manufacturer-brand-input"
                            placeholder="Enter barcode symbology"
                            value={barcodeSymbology}
                            onChange={(e) => setBarcodeSymbology(e.target.value)}
                          />
                        </div>
                      </div>
                    </div>

                    <div className="row">
                      <div className="col-lg-3 col-sm-6">
                        <div className="mb-3">
                          <label className="form-label" htmlFor="stocks-input">
                            Stocks
                          </label>
                          <input
                            type="number"
                            className="form-control"
                            id="stocks-input"
                            placeholder="Stocks"
                            value={stockQuantity}
                            onChange={(e) => setStockQuantity(e.target.value)}
                            required
                          />
                        </div>
                      </div>
                      <div className="col-lg-3 col-sm-6">
                        <div className="mb-3">
                          <label className="form-label" htmlFor="product-price-input">
                            Price
                          </label>
                          <div className="input-group has-validation mb-3">
                            <span className="input-group-text" id="product-price-addon">
                              $
                            </span>
                            <input
                              type="number"
                              className="form-control"
                              id="product-price-input"
                              placeholder="Enter price"
                              aria-label="Price"
                              aria-describedby="product-price-addon"
                              value={price}
                              onChange={(e) => setPrice(e.target.value)}
                              required
                            />
                          </div>
                        </div>
                      </div>
                      <div className="col-lg-3 col-sm-6">
                        <div className="mb-3">
                          <label className="form-label" htmlFor="product-discount-input">
                            Discount
                          </label>
                          <div className="input-group mb-3">
                            <span className="input-group-text" id="product-discount-addon">
                              {selectedDiscountType === 1 ? "%" : "$"}
                            </span>
                            <input
                              type="number"
                              className="form-control"
                              id="product-discount-input"
                              placeholder="Enter discount"
                              aria-label="discount"
                              aria-describedby="product-discount-addon"
                              value={discountValue}
                              onChange={(e) => setDiscountValue(e.target.value)}
                            />
                          </div>
                        </div>
                      </div>
                      <div className="col-lg-3 col-sm-6">
                        <div className="mb-3">
                          <label className="form-label" htmlFor="orders-input">
                            Quantity Alert
                          </label>
                          <input
                            type="number"
                            className="form-control"
                            id="orders-input"
                            placeholder="Quantity alert"
                            value={quantityAlert}
                            onChange={(e) => setQuantityAlert(e.target.value)}
                            required
                          />
                        </div>
                      </div>
                    </div>

                    <div className="row">
                      <div className="col-lg-4">
                        <div className="mb-3">
                          <label className="form-label" htmlFor="product-type-input">
                            Product Type
                          </label>
                          <select
                            id="product-type-input"
                            className="form-select"
                            value={selectedProductType}
                            onChange={(e) => setSelectedProductType(Number(e.target.value))}
                          >
                            {productTypes.map((t) => (
                              <option key={t.id} value={t.id}>
                                {t.name}
                              </option>
                            ))}
                          </select>
                        </div>
                      </div>
                      <div className="col-lg-4">
                        <div className="mb-3">
                          <label className="form-label" htmlFor="selling-type-input">
                            Selling Type
                          </label>
                          <select
                            id="selling-type-input"
                            className="form-select"
                            value={selectedSellingType}
                            onChange={(e) => setSelectedSellingType(Number(e.target.value))}
                          >
                            {sellingTypes.map((t) => (
                              <option key={t.id} value={t.id}>
                                {t.name}
                              </option>
                            ))}
                          </select>
                        </div>
                      </div>
                      <div className="col-lg-4">
                        <div className="mb-3">
                          <label className="form-label" htmlFor="barcode-input">
                            Barcode
                          </label>
                          <input
                            type="text"
                            className="form-control"
                            id="barcode-input"
                            placeholder="Enter barcode"
                            value={barcode}
                            onChange={(e) => setBarcode(e.target.value)}
                          />
                        </div>
                      </div>
                    </div>
                  </div>

                  <div className="tab-pane" id="addproduct-metadata" role="tabpanel">
                    <div className="row">
                      <div className="col-lg-6">
                        <div className="mb-3">
                          <label className="form-label" htmlFor="tax-type-input">
                            Tax Type
                          </label>
                          <select
                            id="tax-type-input"
                            className="form-select"
                            value={selectedTaxType}
                            onChange={(e) => setSelectedTaxType(e.target.value ? Number(e.target.value) : "")}
                          >
                            <option value="">None</option>
                            {taxTypes.map((t) => (
                              <option key={t.id} value={t.id}>
                                {t.name}
                              </option>
                            ))}
                          </select>
                        </div>
                      </div>
                      <div className="col-lg-6">
                        <div className="mb-3">
                          <label className="form-label" htmlFor="tax-rate-input">
                            Tax Rate
                          </label>
                          <input
                            type="number"
                            className="form-control"
                            id="tax-rate-input"
                            placeholder="Enter tax rate"
                            value={taxRate}
                            onChange={(e) => setTaxRate(e.target.value)}
                          />
                        </div>
                      </div>
                    </div>

                    <div className="row">
                      <div className="col-lg-6">
                        <div className="mb-3">
                          <label className="form-label" htmlFor="discount-type-input">
                            Discount Type
                          </label>
                          <select
                            id="discount-type-input"
                            className="form-select"
                            value={selectedDiscountType}
                            onChange={(e) => setSelectedDiscountType(e.target.value ? Number(e.target.value) : "")}
                          >
                            <option value="">None</option>
                            {discountTypes.map((t) => (
                              <option key={t.id} value={t.id}>
                                {t.name}
                              </option>
                            ))}
                          </select>
                        </div>
                      </div>
                      <div className="col-lg-6">
                        <div className="mb-3">
                          <label className="form-label" htmlFor="discount-value-input">
                            Discount Value
                          </label>
                          <input
                            type="number"
                            className="form-control"
                            id="discount-value-input"
                            placeholder="Enter discount value"
                            value={discountValue}
                            onChange={(e) => setDiscountValue(e.target.value)}
                          />
                        </div>
                      </div>
                    </div>
                  </div>
                </div>
              </div>
            </div>

            <div className="text-end mb-3">
              <button type="submit" className="btn btn-success w-sm" disabled={isSaving}>
                {isSaving ? "Saving..." : "Submit"}
              </button>
            </div>
          </div>

          <div className="col-lg-4">
            <div className="card">
              <div className="card-header">
                <h5 className="card-title mb-0">Publish</h5>
              </div>
              <div className="card-body">
                <div className="mb-3">
                  <label htmlFor="choices-publish-status-input" className="form-label">
                    Status
                  </label>
                  <select
                    className="form-select"
                    id="choices-publish-status-input"
                    value={selectedPublishStatus}
                    onChange={(e) => setSelectedPublishStatus(e.target.value ? Number(e.target.value) : "")}
                    disabled={publishStatuses.length === 0}
                  >
                    <option value="">Select status</option>
                    {publishStatuses.map((s) => (
                      <option key={s.id} value={s.id}>
                        {s.name}
                      </option>
                    ))}
                  </select>
                </div>

                <div>
                  <label htmlFor="choices-publish-visibility-input" className="form-label">
                    Visibility
                  </label>
                  <select
                    className="form-select"
                    id="choices-publish-visibility-input"
                    value={selectedVisibility}
                    onChange={(e) => setSelectedVisibility(e.target.value ? Number(e.target.value) : "")}
                    disabled={visibilities.length === 0}
                  >
                    <option value="">Select visibility</option>
                    {visibilities.map((v) => (
                      <option key={v.id} value={v.id}>
                        {v.name}
                      </option>
                    ))}
                  </select>
                </div>
              </div>
            </div>

            <div className="card">
              <div className="card-header">
                <h5 className="card-title mb-0">Publish Schedule</h5>
              </div>
              <div className="card-body">
                <div>
                  <label htmlFor="datepicker-publish-input" className="form-label">
                    Publish Date & Time
                  </label>
                  <input
                    type="datetime-local"
                    id="datepicker-publish-input"
                    className="form-control"
                    value={publishDateLocal}
                    onChange={(e) => setPublishDateLocal(e.target.value)}
                    disabled={selectedPublishStatus !== ProductPublishStatus.Scheduled}
                  />
                  <div className="text-muted mt-2 small">
                    Enable by selecting status &quot;Scheduled&quot;.
                  </div>
                </div>
              </div>
            </div>

            <div className="card">
              <div className="card-header">
                <h5 className="card-title mb-0">Product Categories</h5>
              </div>
              <div className="card-body">
                <p className="text-muted mb-2">
                  Select product category
                  <Link href="#" className="float-end text-decoration-underline" aria-disabled="true" tabIndex={-1}>
                    Add New
                  </Link>
                </p>
                <select
                  className="form-select"
                  id="choices-category-input"
                  value={selectedCategoryId}
                  onChange={(e) => setSelectedCategoryId(e.target.value)}
                  required
                >
                  <option value="">Select category</option>
                  {categories.map((c) => (
                    <option key={c.id} value={c.id}>
                      {c.name}
                    </option>
                  ))}
                </select>

                <div className="mt-3">
                  <label className="form-label" htmlFor="choices-subcategory-input">
                    Sub Category
                  </label>
                  <select
                    className="form-select"
                    id="choices-subcategory-input"
                    value={selectedSubCategoryId}
                    onChange={(e) => setSelectedSubCategoryId(e.target.value)}
                    disabled={!selectedCategoryId}
                  >
                    <option value="">Select sub category</option>
                    {subCategories.map((sc) => (
                      <option key={sc.id} value={sc.id}>
                        {sc.name}
                      </option>
                    ))}
                  </select>
                </div>
              </div>
            </div>

            <div className="card">
              <div className="card-header">
                <h5 className="card-title mb-0">Brand & More</h5>
              </div>
              <div className="card-body">
                <div className="mb-3">
                  <label className="form-label" htmlFor="choices-brand-input">
                    Brand
                  </label>
                  <select
                    className="form-select"
                    id="choices-brand-input"
                    value={selectedBrandId}
                    onChange={(e) => setSelectedBrandId(e.target.value)}
                  >
                    <option value="">Select brand</option>
                    {brands.map((b) => (
                      <option key={b.id} value={b.id}>
                        {b.name}
                      </option>
                    ))}
                  </select>
                </div>

                <div className="mb-3">
                  <label className="form-label" htmlFor="choices-unit-input">
                    Unit of Measure
                  </label>
                  <select
                    className="form-select"
                    id="choices-unit-input"
                    value={selectedUnitId}
                    onChange={(e) => setSelectedUnitId(e.target.value)}
                  >
                    <option value="">Select unit</option>
                    {units.map((u) => (
                      <option key={u.id} value={u.id}>
                        {u.name}
                      </option>
                    ))}
                  </select>
                </div>

                <div>
                  <label className="form-label" htmlFor="choices-warranty-input">
                    Warranty
                  </label>
                  <select
                    className="form-select"
                    id="choices-warranty-input"
                    value={selectedWarrantyId}
                    onChange={(e) => setSelectedWarrantyId(e.target.value)}
                  >
                    <option value="">Select warranty</option>
                    {warranties.map((w) => (
                      <option key={w.id} value={w.id}>
                        {w.name}
                      </option>
                    ))}
                  </select>
                </div>
              </div>
            </div>

            <div className="card">
              <div className="card-header">
                <h5 className="card-title mb-0">Product Tags</h5>
              </div>
              <div className="card-body">
                <div className="hstack gap-3 align-items-start">
                  <div className="flex-grow-1">
                    <input
                      className="form-control"
                      list="product-tags-suggestions"
                      placeholder="Enter tags"
                      value={tagInput}
                      onChange={(e) => setTagInput(e.target.value)}
                      onKeyDown={(e) => {
                        if (e.key === "Enter" || e.key === ",") {
                          e.preventDefault();
                          const next = tagInput.trim().replace(/,$/, "");
                          if (!next) return;
                          setTags((prev) => {
                            const exists = prev.some((t) => t.toLowerCase() === next.toLowerCase());
                            return exists ? prev : [...prev, next];
                          });
                          setTagInput("");
                        }
                      }}
                    />
                    <datalist id="product-tags-suggestions">
                      {tagsLookup.map((t) => (
                        <option key={t.id} value={t.name} />
                      ))}
                    </datalist>
                    {tags.length > 0 ? (
                      <div className="d-flex flex-wrap gap-2 mt-2">
                        {tags.map((t) => (
                          <span key={t} className="badge bg-light text-muted border">
                            {t}{" "}
                            <button
                              type="button"
                              className="btn btn-sm p-0 ms-1"
                              onClick={() => setTags((prev) => prev.filter((x) => x !== t))}
                              aria-label={`Remove ${t}`}
                            >
                              <i className="ri-close-line"></i>
                            </button>
                          </span>
                        ))}
                      </div>
                    ) : null}
                  </div>
                </div>
              </div>
            </div>

            <div className="card">
              <div className="card-header">
                <h5 className="card-title mb-0">Product Short Description</h5>
              </div>
              <div className="card-body">
                <p className="text-muted mb-2">Add short description for product</p>
                <textarea
                  className="form-control"
                  placeholder="Must enter minimum of a 100 characters"
                  rows={3}
                  value={shortDescription}
                  onChange={(e) => setShortDescription(e.target.value)}
                />
              </div>
            </div>
          </div>
        </div>
      </form>
    </div>
  );
}

