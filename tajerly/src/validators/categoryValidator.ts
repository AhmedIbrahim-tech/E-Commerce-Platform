import * as Yup from 'yup';

export const categorySchema = Yup.object({
    name: Yup.string()
        .required('Category name is required')
        .min(2, 'Name must be at least 2 characters')
        .max(100, 'Name must not exceed 100 characters'),
    description: Yup.string()
        .nullable()
        .optional()
        .max(500, 'Description must not exceed 500 characters'),
    imageUrl: Yup.string()
        .nullable()
        .optional()
        .url('Please enter a valid URL'),
    isActive: Yup.boolean()
        .default(true),
    displayOrder: Yup.number()
        .optional()
        .integer('Display order must be a whole number')
        .min(0, 'Display order cannot be negative'),
});

export type CategoryFormValues = Yup.InferType<typeof categorySchema>;

export const categoryInitialValues: CategoryFormValues = {
    name: '',
    description: '',
    imageUrl: '',
    isActive: true,
    displayOrder: 0,
};
