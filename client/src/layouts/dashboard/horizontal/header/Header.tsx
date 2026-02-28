import { useCustomizer } from '@/hooks/useCustomizer';
import * as React from 'react';
import AppBar from '@mui/material/AppBar';
import Box from '@mui/material/Box';
import IconButton from '@mui/material/IconButton';
import Stack from '@mui/material/Stack';
import { Theme } from '@mui/material/styles';
import Toolbar from '@mui/material/Toolbar';
import useMediaQuery from '@mui/material/useMediaQuery';
import { styled } from '@mui/material/styles';
import { useContext } from "react";
import { IconMenu2, IconMoon, IconSun } from '@tabler/icons-react';
import Notifications from '../../vertical/header/Notification';
import config from "@/config/config";

import Profile from '../../vertical/header/Profile';
import Search from '../../vertical/header/Search';
import Language from '../../vertical/header/Language';
import Navigation from '../../vertical/header/Navigation';
import Logo from '../../shared/logo/Logo';


const Header = () => {
  const lgDown = useMediaQuery((theme: Theme) => theme.breakpoints.down('lg'));
  const lgUp = useMediaQuery((theme: Theme) => theme.breakpoints.up('lg'));

  const { isLayout, setIsMobileSidebar, isMobileSidebar, activeMode, setActiveMode } = useCustomizer();

  const TopbarHeight = config.topbarHeight;


  const AppBarStyled = styled(AppBar)(({ theme }) => ({
    background: theme.palette.background.paper,
    justifyContent: 'center',
    backdropFilter: 'blur(4px)',

    [theme.breakpoints.up('lg')]: {
      minHeight: TopbarHeight,
    },
  }));
  const ToolbarStyled = styled(Toolbar)(({ theme }) => ({ margin: '0 auto', width: '100%', color: `${theme.palette.text.secondary} !important`, }));

  return (
    <AppBarStyled position="sticky" color="default" elevation={8}>
      <ToolbarStyled
        sx={{
          maxWidth: isLayout === 'boxed' ? 'lg' : '100%!important',
        }}
      >
        <Box sx={{ width: lgDown ? '45px' : 'auto', overflow: 'hidden' }}>
          <Logo />
        </Box>
        {/* ------------------------------------------- */}
        {/* Toggle Button Sidebar */}
        {/* ------------------------------------------- */}
        {lgDown ? (
          <IconButton
            color="inherit"
            aria-label="menu"
            onClick={() => setIsMobileSidebar(!isMobileSidebar)}
          >
            <IconMenu2 />
          </IconButton>
        ) : (
          ''
        )}
        {/* ------------------------------------------- */}
        {/* Search Dropdown */}
        {/* ------------------------------------------- */}
        <Search />
        {lgUp ? (
          <>
            <Navigation />
          </>
        ) : null}
        <Box flexGrow={1} />
        <Stack spacing={1} direction="row" alignItems="center">
          <Language />
          {/* ------------------------------------------- */}
          {/* Ecommerce Dropdown */}
          {/* ------------------------------------------- */}

          {/* ------------------------------------------- */}
          {/* End Ecommerce Dropdown */}
          {/* ------------------------------------------- */}

          <IconButton size="large" color="inherit">
            {activeMode === "light" ? (
              <IconMoon
                size="21"
                stroke="1.5"
                onClick={() => setActiveMode("dark")}
              />
            ) : (
              <IconSun
                size="21"
                stroke="1.5"
                onClick={() => setActiveMode("light")}
              />
            )}
          </IconButton>
          <Notifications />
          <Profile />
        </Stack>
      </ToolbarStyled>
    </AppBarStyled>
  );
};

export default Header;
