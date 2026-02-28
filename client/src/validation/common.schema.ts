import * as yup from 'yup';

export const commonStringSchema = yup.string().required('This field is required');
export const commonNumberSchema = yup.number().required('This field is required');
