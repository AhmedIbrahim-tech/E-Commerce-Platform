import * as Yup from 'yup';

export const productSchema = Yup.object({
    name: Yup.string()
        .required('Product name is required')
        .min(2, 'Name must be at least 2 characters')
        .max(200, 'Name must not exceed 200 characters'),
    description: Yup.string()
        .required('Description is required')
        .min(10, 'Description must be at least 10 characters'),
    shortDescription: Yup.string()
        .optional()
        .max(300, 'Short description must not exceed 300 characters'),
    price: Yup.number()
        .required('Price is required')
        .min(0.01, 'Price must be greater than zero'),
    compareAtPrice: Yup.number()
        .nullable()
        .optional()
        .min(0, 'Compare price cannot be negative'),
    sku: Yup.string()
        .optional()
        .max(50, 'SKU must not exceed 50 characters'),
    stock: Yup.number()
        .required('Stock quantity is required')
        .integer('Stock must be a whole number')
        .min(0, 'Stock cannot be negative'),
    categoryId: Yup.number()
        .required('Category is required')
        .min(1, 'Please select a category'),
    subCategoryId: Yup.number()
        .nullable()
        .optional(),
    brand: Yup.string()
        .optional()
        .max(100, 'Brand must not exceed 100 characters'),
    isActive: Yup.boolean()
        .default(true),
    isFeatured: Yup.boolean()
        .default(false),
});

export type ProductFormValues = Yup.InferType<typeof productSchema>;

export const productInitialValues: ProductFormValues = {
    name: '',
    description: '',
    shortDescription: '',
    price: 0,
    compareAtPrice: null,
    sku: '',
    stock: 0,
    categoryId: 0,
    subCategoryId: null,
    brand: '',
    isActive: true,
    isFeatured: false,
};
