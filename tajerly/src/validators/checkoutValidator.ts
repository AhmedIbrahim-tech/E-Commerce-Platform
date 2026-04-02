import * as Yup from 'yup';

export const checkoutSchema = Yup.object({
    shippingAddressId: Yup.number()
        .required('Please select a shipping address')
        .min(1, 'Please select a shipping address'),
    paymentMethod: Yup.string()
        .required('Please select a payment method')
        .oneOf(['cod', 'card', 'wallet'], 'Invalid payment method'),
    notes: Yup.string()
        .optional()
        .max(500, 'Notes must not exceed 500 characters'),
});

export type CheckoutFormValues = Yup.InferType<typeof checkoutSchema>;

export const checkoutInitialValues: CheckoutFormValues = {
    shippingAddressId: 0,
    paymentMethod: 'cod',
    notes: '',
};
