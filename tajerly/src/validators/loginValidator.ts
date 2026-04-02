import * as Yup from 'yup';

export const loginSchema = Yup.object({
    email: Yup.string()
        .required('Email is required')
        .email('Please enter a valid email address'),
    password: Yup.string()
        .required('Password is required')
        .min(6, 'Password must be at least 6 characters'),
});

export type LoginFormValues = Yup.InferType<typeof loginSchema>;

export const loginInitialValues: LoginFormValues = {
    email: '',
    password: '',
};
