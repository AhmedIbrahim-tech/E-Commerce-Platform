'use client';

import { Toaster } from 'sonner';
import { useTheme } from './ThemeProvider';

export function NotifyToaster() {
  const { theme } = useTheme();
  return <Toaster position="top-right" theme={theme} richColors closeButton />;
}
