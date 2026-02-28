'use client';

import React, { createContext, useCallback, useContext, useState } from 'react';
import { Alert, AlertColor, Snackbar, useTheme } from '@mui/material';

type NotifySeverity = AlertColor;

interface NotifyContextValue {
  notify: (message: string, severity?: NotifySeverity) => void;
}

const NotifyContext = createContext<NotifyContextValue | null>(null);

export function useNotify(): NotifyContextValue {
  const ctx = useContext(NotifyContext);
  if (!ctx) throw new Error('useNotify must be used within NotifyProvider');
  return ctx;
}

const severityStyles: Record<NotifySeverity, { bg: string; color: string }> = {
  success: { bg: '#13DEB9', color: '#fff' },
  error: { bg: '#FA896B', color: '#fff' },
  warning: { bg: '#FFAE1F', color: '#fff' },
  info: { bg: '#539BFF', color: '#fff' },
};

interface NotifyProviderProps {
  children: React.ReactNode;
  autoHideDuration?: number;
  anchorOrigin?: { vertical: 'top' | 'bottom'; horizontal: 'left' | 'center' | 'right' };
}

export function NotifyProvider({
  children,
  autoHideDuration = 5000,
  anchorOrigin = { vertical: 'top', horizontal: 'right' },
}: NotifyProviderProps) {
  const theme = useTheme();
  const [open, setOpen] = useState(false);
  const [message, setMessage] = useState('');
  const [severity, setSeverity] = useState<NotifySeverity>('info');

  const notify = useCallback((msg: string, sev: NotifySeverity = 'info') => {
    setMessage(msg);
    setSeverity(sev);
    setOpen(true);
  }, []);

  const handleClose = useCallback((_: React.SyntheticEvent | Event, reason?: string) => {
    if (reason === 'clickaway') return;
    setOpen(false);
  }, []);

  const style = severityStyles[severity] ?? severityStyles.info;
  const bg = theme.palette[severity]?.main ?? style.bg;

  return (
    <NotifyContext.Provider value={{ notify }}>
      {children}
      <Snackbar
        open={open}
        autoHideDuration={autoHideDuration}
        onClose={handleClose}
        anchorOrigin={anchorOrigin}
      >
        <Alert
          onClose={handleClose}
          severity={severity}
          variant="filled"
          sx={{
            width: '100%',
            backgroundColor: bg,
            color: style.color,
            '& .MuiAlert-icon': { color: style.color },
          }}
        >
          {message}
        </Alert>
      </Snackbar>
    </NotifyContext.Provider>
  );
}
