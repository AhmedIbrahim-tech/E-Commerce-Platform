import * as Yup from 'yup';

export const addressSchema = Yup.object({
    label: Yup.string()
        .required('Label is required (e.g. Home, Work)')
        .max(50, 'Label must not exceed 50 characters'),
    fullName: Yup.string()
        .required('Full name is required')
        .min(2, 'Name must be at least 2 characters'),
    phone: Yup.string()
        .required('Phone number is required')
        .matches(/^[+]?[\d\s-]+$/, 'Please enter a valid phone number'),
    addressLine1: Yup.string()
        .required('Address line is required')
        .min(5, 'Address must be at least 5 characters'),
    addressLine2: Yup.string()
        .optional(),
    city: Yup.string()
        .required('City is required'),
    state: Yup.string()
        .optional(),
    postalCode: Yup.string()
        .optional(),
    country: Yup.string()
        .required('Country is required'),
    isDefault: Yup.boolean()
        .default(false),
});

export type AddressFormValues = Yup.InferType<typeof addressSchema>;

export const addressInitialValues: AddressFormValues = {
    label: '',
    fullName: '',
    phone: '',
    addressLine1: '',
    addressLine2: '',
    city: '',
    state: '',
    postalCode: '',
    country: 'Egypt',
    isDefault: false,
};
