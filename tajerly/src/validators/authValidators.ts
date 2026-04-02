import * as Yup from 'yup';

export const forgotPasswordSchema = Yup.object({
    email: Yup.string()
        .required('Email is required')
        .email('Please enter a valid email address'),
});

export const verifyCodeSchema = Yup.object({
    code: Yup.string()
        .required('Verification code is required')
        .length(6, 'Code must be exactly 6 digits'),
});

export const resetPasswordSchema = Yup.object({
    password: Yup.string()
        .required('Password is required')
        .min(6, 'Password must be at least 6 characters')
        .matches(/[A-Z]/, 'Password must contain at least one uppercase letter')
        .matches(/[0-9]/, 'Password must contain at least one number'),
    confirmPassword: Yup.string()
        .required('Please confirm your password')
        .oneOf([Yup.ref('password')], 'Passwords must match'),
});

export const twoStepSchema = Yup.object({
    code: Yup.string()
        .required('Security code is required')
        .min(6, 'Code must be at least 6 digits'),
});

export const changePasswordSchema = Yup.object({
    currentPassword: Yup.string().required('Current password is required'),
    newPassword: Yup.string()
        .required('New password is required')
        .min(6, 'New password must be at least 6 characters'),
    confirmPassword: Yup.string()
        .required('Please confirm your new password')
        .oneOf([Yup.ref('newPassword')], 'Passwords must match'),
});
