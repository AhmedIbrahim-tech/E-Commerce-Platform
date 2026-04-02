import * as Yup from 'yup';

export const registerSchema = Yup.object({
    firstName: Yup.string()
        .required('First name is required')
        .min(2, 'Must be at least 2 characters'),
    lastName: Yup.string()
        .required('Last name is required')
        .min(2, 'Must be at least 2 characters'),
    userName: Yup.string()
        .required('Username is required')
        .min(3, 'Must be at least 3 characters')
        .matches(/^[a-zA-Z0-9._]+$/, 'Username can only contain letters, numbers, dots and underscores'),
    email: Yup.string()
        .required('Email is required')
        .email('Please enter a valid email address'),
    password: Yup.string()
        .required('Password is required')
        .min(6, 'Password must be at least 6 characters')
        .matches(/[A-Z]/, 'Password must contain at least one uppercase letter')
        .matches(/[0-9]/, 'Password must contain at least one number'),
    confirmPassword: Yup.string()
        .required('Please confirm your password')
        .oneOf([Yup.ref('password')], 'Passwords must match'),
    phone: Yup.string()
        .optional()
        .matches(/^[+]?[\d\s-]+$/, 'Please enter a valid phone number'),
});

export type RegisterFormValues = Yup.InferType<typeof registerSchema>;

export const registerInitialValues: RegisterFormValues = {
    firstName: '',
    lastName: '',
    userName: '',
    email: '',
    password: '',
    confirmPassword: '',
    phone: '',
};
