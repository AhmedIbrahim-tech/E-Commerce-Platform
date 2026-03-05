'use client';

import { useCustomizer } from '@/hooks/useCustomizer';
import { Box, Avatar, Typography, IconButton, Tooltip, useMediaQuery } from '@mui/material';
import { IconPower } from '@tabler/icons-react';
import { useAppDispatch } from '@/hooks/useRedux';
import { useRouter } from 'next/navigation';
import { logoutAsync } from '@/store/slices/authSlice';

export const Profile = () => {
  const lgUp = useMediaQuery((theme: any) => theme.breakpoints.up('lg'));
  const dispatch = useAppDispatch();
  const router = useRouter();
  const { isSidebarHover, isCollapse } = useCustomizer();
  const hideMenu = lgUp ? isCollapse == 'mini-sidebar' && !isSidebarHover : '';

  const handleLogout = () => {
    dispatch(logoutAsync()).then(() => router.push('/login'));
  };

  return (
    <Box
      display={'flex'}
      alignItems="center"
      gap={2}
      sx={{ m: 3, p: 2, bgcolor: `${'secondary.light'}` }}
    >
      {!hideMenu ? (
        <>
          <Avatar alt="Remy Sharp" src={"/images/profile/user-1.jpg"} sx={{ height: 40, width: 40 }} />

          <Box>
            <Typography variant="h6">Mathew</Typography>
            <Typography variant="caption">Designer</Typography>
          </Box>
          <Box sx={{ ml: 'auto' }}>
            <Tooltip title="Logout" placement="top">
              <IconButton
                color="primary"
                onClick={handleLogout}
                aria-label="logout"
                size="small"
              >
                <IconPower size="20" />
              </IconButton>
            </Tooltip>
          </Box>
        </>
      ) : (
        ''
      )}
    </Box>
  );
};
