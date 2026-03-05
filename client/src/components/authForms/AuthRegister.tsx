'use client'
import React, { useState } from 'react';
import { useRouter } from 'next/navigation';
import { useForm } from 'react-hook-form';
import { yupResolver } from '@hookform/resolvers/yup';
import { registerSchema } from '@/validation/auth.schema';
import { useAppDispatch, useAppSelector } from '@/hooks/useRedux';
import { registerUser, clearError } from '@/store/slices/authSlice';
import Box from '@mui/material/Box';
import Button from '@mui/material/Button';
import Divider from '@mui/material/Divider';
import Typography from '@mui/material/Typography';
import Link from 'next/link';
import CustomTextField from '@/components/forms/theme-elements/CustomTextField';
import { useNotify } from '@/context/NotifyContext';
import CustomFormLabel from '@/components/forms/theme-elements/CustomFormLabel';
import { Stack } from '@mui/system';
import AuthSocialButtons from './AuthSocialButtons';

interface registerType {
  title?: string;
  subtitle?: React.ReactNode;
  subtext?: React.ReactNode;
}

const AuthRegister = ({ title, subtitle, subtext }: registerType) => {
  const dispatch = useAppDispatch();
  const router = useRouter();
  const { registerError, registerLoading } = useAppSelector((state) => state.auth);
  const { notify } = useNotify();

  const {
    register,
    handleSubmit,
    formState: { errors },
  } = useForm({
    resolver: yupResolver(registerSchema),
  });

  const onSubmit = async (data: any) => {
    dispatch(clearError());
    const resultAction = await dispatch(registerUser(data));
    if (registerUser.fulfilled.match(resultAction)) {
      notify('Registration successful! Check your email to confirm your account.', 'success');
    } else if (registerUser.rejected.match(resultAction)) {
      notify(typeof resultAction.payload === 'string' ? resultAction.payload : (registerError || 'Registration failed'), 'error');
    }
  };

  return (
    <>
      {title ? (
        <Typography fontWeight="700" variant="h3" mb={1}>
          {title}
        </Typography>
      ) : null}

      {subtext}
      <AuthSocialButtons title="Sign up with" />

      <Box mt={3}>
        <Divider>
          <Typography
            component="span"
            color="textSecondary"
            variant="h6"
            fontWeight="400"
            position="relative"
            px={2}
          >
            or sign up with
          </Typography>
        </Divider>
      </Box>

      <Box component="form" onSubmit={handleSubmit(onSubmit)}>
        <Stack mb={3}>
          <Box display="flex" gap={2}>
            <Box flex={1}>
              <CustomFormLabel htmlFor="FirstName">First Name</CustomFormLabel>
              <CustomTextField id="FirstName" variant="outlined" fullWidth {...register('FirstName')} error={!!errors.FirstName} helperText={errors.FirstName?.message} />
            </Box>
            <Box flex={1}>
              <CustomFormLabel htmlFor="LastName">Last Name</CustomFormLabel>
              <CustomTextField id="LastName" variant="outlined" fullWidth {...register('LastName')} error={!!errors.LastName} helperText={errors.LastName?.message} />
            </Box>
          </Box>

          <CustomFormLabel htmlFor="UserName">Username</CustomFormLabel>
          <CustomTextField id="UserName" variant="outlined" fullWidth {...register('UserName')} error={!!errors.UserName} helperText={errors.UserName?.message} />

          <CustomFormLabel htmlFor="Email">Email Address</CustomFormLabel>
          <CustomTextField id="Email" variant="outlined" fullWidth {...register('Email')} error={!!errors.Email} helperText={errors.Email?.message} />

          <CustomFormLabel htmlFor="Password">Password</CustomFormLabel>
          <CustomTextField id="Password" type="password" variant="outlined" fullWidth {...register('Password')} error={!!errors.Password} helperText={errors.Password?.message} />

          <CustomFormLabel htmlFor="ConfirmPassword">Confirm Password</CustomFormLabel>
          <CustomTextField id="ConfirmPassword" type="password" variant="outlined" fullWidth {...register('ConfirmPassword')} error={!!errors.ConfirmPassword} helperText={errors.ConfirmPassword?.message} />
        </Stack>
        <Button
          color="primary"
          variant="contained"
          size="large"
          fullWidth
          type="submit"
          disabled={registerLoading}
        >
          {registerLoading ? 'Signing Up...' : 'Sign Up'}
        </Button>
      </Box>
      {subtitle}
    </>
  );
};

export default AuthRegister;
