'use client';

import { Sun, Moon } from 'lucide-react';
import { useTheme } from './ThemeProvider';
import { Button } from 'react-bootstrap';

export function ThemeToggle() {
  const { theme, toggleTheme } = useTheme();

  return (
    <Button
      variant="link"
      onClick={toggleTheme}
      className="p-2 rounded-circle d-flex align-items-center justify-content-center"
      style={{
        width: 40, height: 40,
        color: 'var(--tj-text-muted)',
        transition: 'all 0.2s',
      }}
      aria-label={`Switch to ${theme === 'light' ? 'dark' : 'light'} mode`}
    >
      {theme === 'light' ? <Moon size={18} /> : <Sun size={18} />}
    </Button>
  );
}
