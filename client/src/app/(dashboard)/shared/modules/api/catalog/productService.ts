import apiClient from "@/services/core/apiClient";
import { Routes } from "@/constants/apiroutes";
import type { CreateProductRequest, Product, ProductListResponse, UpdateProductRequest } from "@/types";
import { extractApiData } from "@/types/api";
import { handleApiError } from "@/services/core/apiErrorHandler";

class ProductService {
  async getProductPaginatedList(
    pageNumber: number = 1,
    pageSize: number = 10,
    search?: string,
    sortBy?: number,
    filters?: {
      categoryId?: string;
      brandIds?: string[];
      isActive?: boolean;
      minPrice?: number;
      maxPrice?: number;
      minDiscountPercentage?: number;
      minRating?: number;
    }
  ): Promise<ProductListResponse> {
    try {
      const requestBody = {
        pageNumber,
        pageSize,
        search: search || null,
        sortBy: sortBy || 0,
        categoryId: filters?.categoryId || null,
        brandIds: filters?.brandIds && filters.brandIds.length > 0 ? filters.brandIds : null,
        isActive: typeof filters?.isActive === "boolean" ? filters.isActive : null,
        minPrice: typeof filters?.minPrice === "number" ? filters.minPrice : null,
        maxPrice: typeof filters?.maxPrice === "number" ? filters.maxPrice : null,
        minDiscountPercentage:
          typeof filters?.minDiscountPercentage === "number" ? filters.minDiscountPercentage : null,
        minRating: typeof filters?.minRating === "number" ? filters.minRating : null,
      };

      const response = await apiClient.post(Routes.Product.Paginated, requestBody);
      const data = extractApiData<ProductListResponse>(response);

      if (data.data) {
        data.data = data.data.map((product) => {
          const p = product as Product & { createdTime?: string; modifiedTime?: string };
          return {
            ...product,
            createdAt: p.createdTime || product.createdAt,
            updatedAt: p.modifiedTime || product.updatedAt,
          };
        });
      }

      return data;
    } catch (error) {
      handleApiError(error);
    }
  }

  async getProductById(
    id: string,
    reviewPageNumber: number = 1,
    reviewPageSize: number = 10,
    reviewSortBy: number = 0,
    reviewSearch?: string
  ): Promise<Product> {
    try {
      const params = new URLSearchParams({
        productId: id,
        reviewPageNumber: reviewPageNumber.toString(),
        reviewPageSize: reviewPageSize.toString(),
        sortBy: reviewSortBy.toString(),
        ...(reviewSearch && { search: reviewSearch }),
      });
      const response = await apiClient.get(`${Routes.Product.GetSingle}?${params.toString()}`);
      const data = extractApiData<Product>(response);

      const createdTime = (data as unknown as { createdTime?: string }).createdTime;
      const modifiedTime = (data as unknown as { modifiedTime?: string }).modifiedTime;
      return {
        ...data,
        createdAt: createdTime || data.createdAt,
        updatedAt: modifiedTime || data.updatedAt,
      } as Product;
    } catch (error) {
      handleApiError(error);
    }
  }

  async createProduct(data: CreateProductRequest): Promise<string> {
    try {
      const formData = new FormData();

      formData.append("name", data.name);
      formData.append("slug", data.slug);
      formData.append("sku", data.sku);
      if (data.description) formData.append("description", data.description);
      if (data.shortDescription) formData.append("shortDescription", data.shortDescription);
      formData.append("price", data.price.toString());
      formData.append("stockQuantity", data.stockQuantity.toString());
      formData.append("quantityAlert", data.quantityAlert.toString());
      if (data.barcode) formData.append("barcode", data.barcode);
      if (data.barcodeSymbology) formData.append("barcodeSymbology", data.barcodeSymbology);
      if (data.publishStatus) formData.append("publishStatus", data.publishStatus.toString());
      if (data.visibility) formData.append("visibility", data.visibility.toString());
      if (data.publishDate) formData.append("publishDate", data.publishDate);
      if (data.tags && data.tags.length > 0) {
        data.tags.forEach((t, index) => {
          formData.append(`tags[${index}]`, t);
        });
      }
      formData.append("productType", data.productType.toString());
      formData.append("sellingType", data.sellingType.toString());
      if (data.taxType) formData.append("taxType", data.taxType.toString());
      if (data.taxRate) formData.append("taxRate", data.taxRate.toString());
      if (data.discountType) formData.append("discountType", data.discountType.toString());
      if (data.discountValue) formData.append("discountValue", data.discountValue.toString());
      formData.append("categoryId", data.categoryId);
      if (data.subCategoryId) formData.append("subCategoryId", data.subCategoryId);
      if (data.brandId) formData.append("brandId", data.brandId);
      if (data.unitOfMeasureId) formData.append("unitOfMeasureId", data.unitOfMeasureId);
      if (data.warrantyId) formData.append("warrantyId", data.warrantyId);
      if (data.manufacturedDate) formData.append("manufacturedDate", data.manufacturedDate);
      if (data.expiryDate) formData.append("expiryDate", data.expiryDate);
      if (data.manufacturer) formData.append("manufacturer", data.manufacturer);

      if (data.productImages && data.productImages.length > 0) {
        data.productImages.forEach((img, index) => {
          formData.append(`productImages[${index}].imageFile`, img.imageFile);
          formData.append(`productImages[${index}].isPrimary`, (img.isPrimary || false).toString());
          formData.append(`productImages[${index}].displayOrder`, (img.displayOrder || index).toString());
        });
      }

      if (data.productVariants && data.productVariants.length > 0) {
        data.productVariants.forEach((variant, index) => {
          formData.append(`productVariants[${index}].variantAttribute`, variant.variantAttribute);
          formData.append(`productVariants[${index}].variantValue`, variant.variantValue);
          formData.append(`productVariants[${index}].sku`, variant.sku);
          formData.append(`productVariants[${index}].quantity`, variant.quantity.toString());
          formData.append(`productVariants[${index}].price`, variant.price.toString());
          if (variant.imageURL) formData.append(`productVariants[${index}].imageURL`, variant.imageURL);
        });
      }

      const response = await apiClient.post(Routes.Product.Create, formData);
      const result = extractApiData<string>(response);
      return result;
    } catch (error) {
      handleApiError(error);
    }
  }

  async updateProduct(data: UpdateProductRequest): Promise<string> {
    try {
      const formData = new FormData();

      formData.append("id", data.id);
      formData.append("name", data.name);
      formData.append("slug", data.slug);
      formData.append("sku", data.sku);
      if (data.description) formData.append("description", data.description);
      if (data.shortDescription) formData.append("shortDescription", data.shortDescription);
      formData.append("price", data.price.toString());
      formData.append("stockQuantity", data.stockQuantity.toString());
      formData.append("quantityAlert", data.quantityAlert.toString());
      if (data.barcode) formData.append("barcode", data.barcode);
      if (data.barcodeSymbology) formData.append("barcodeSymbology", data.barcodeSymbology);
      if (data.publishStatus) formData.append("publishStatus", data.publishStatus.toString());
      if (data.visibility) formData.append("visibility", data.visibility.toString());
      if (data.publishDate) formData.append("publishDate", data.publishDate);
      if (data.tags && data.tags.length > 0) {
        data.tags.forEach((t, index) => {
          formData.append(`tags[${index}]`, t);
        });
      }
      formData.append("replaceTags", (data.replaceTags ?? true).toString());
      formData.append("productType", data.productType.toString());
      formData.append("sellingType", data.sellingType.toString());
      if (data.taxType) formData.append("taxType", data.taxType.toString());
      if (data.taxRate) formData.append("taxRate", data.taxRate.toString());
      if (data.discountType) formData.append("discountType", data.discountType.toString());
      if (data.discountValue) formData.append("discountValue", data.discountValue.toString());
      formData.append("categoryId", data.categoryId);
      if (data.subCategoryId) formData.append("subCategoryId", data.subCategoryId);
      if (data.brandId) formData.append("brandId", data.brandId);
      if (data.unitOfMeasureId) formData.append("unitOfMeasureId", data.unitOfMeasureId);
      if (data.warrantyId) formData.append("warrantyId", data.warrantyId);
      if (data.manufacturedDate) formData.append("manufacturedDate", data.manufacturedDate);
      if (data.expiryDate) formData.append("expiryDate", data.expiryDate);
      if (data.manufacturer) formData.append("manufacturer", data.manufacturer);
      formData.append("isActive", data.isActive.toString());

      if (data.productImages && data.productImages.length > 0) {
        data.productImages.forEach((img, index) => {
          formData.append(`productImages[${index}].imageFile`, img.imageFile);
          formData.append(`productImages[${index}].isPrimary`, (img.isPrimary || false).toString());
          formData.append(`productImages[${index}].displayOrder`, (img.displayOrder || index).toString());
        });
      }

      if (data.productVariants && data.productVariants.length > 0) {
        data.productVariants.forEach((variant, index) => {
          formData.append(`productVariants[${index}].variantAttribute`, variant.variantAttribute);
          formData.append(`productVariants[${index}].variantValue`, variant.variantValue);
          formData.append(`productVariants[${index}].sku`, variant.sku);
          formData.append(`productVariants[${index}].quantity`, variant.quantity.toString());
          formData.append(`productVariants[${index}].price`, variant.price.toString());
          if (variant.imageURL) formData.append(`productVariants[${index}].imageURL`, variant.imageURL);
        });
      }

      const response = await apiClient.put(Routes.Product.Edit, formData);
      const result = extractApiData<string>(response);
      return result;
    } catch (error) {
      handleApiError(error);
    }
  }

  async deleteProduct(id: string): Promise<void> {
    try {
      await apiClient.delete(Routes.Product.Delete(id));
    } catch (error) {
      handleApiError(error);
    }
  }
}

export const productService = new ProductService();

