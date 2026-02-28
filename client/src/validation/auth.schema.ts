import * as yup from 'yup';

export const registerSchema = yup.object({
    FirstName: yup.string().required('First name is required'),
    LastName: yup.string().required('Last name is required'),
    UserName: yup.string().required('Username is required'),
    Email: yup.string().email('Enter a valid email').required('Email is required'),
    Password: yup.string().min(6, 'Password should be of minimum 6 characters length').required('Password is required'),
    ConfirmPassword: yup.string()
        .oneOf([yup.ref('Password')], 'Passwords must match')
        .required('Confirm password is required'),
});

export const forgotPasswordSchema = yup.object({
    email: yup.string().email('Enter a valid email').required('Email is required'),
});

export const confirmResetPasswordSchema = yup.object({
    code: yup.string().required('Code is required').length(6, 'Code must be 6 digits'),
    email: yup.string().email('Enter a valid email').required('Email is required'),
});

export const resetPasswordSchema = yup.object({
    newPassword: yup.string().min(6, 'Password should be at least 6 characters').required('Password is required'),
    confirmPassword: yup.string()
        .oneOf([yup.ref('newPassword')], 'Passwords must match')
        .required('Confirm password is required'),
});

export const twoStepSchema = yup.object({
    code: yup.string().required('Code is required').min(6, 'Code must be at least 6 characters'),
});
