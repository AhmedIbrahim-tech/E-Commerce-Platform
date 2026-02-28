'use client'
import { useCustomizer } from '@/hooks/useCustomizer';
import React, { useContext } from 'react';
import Card from '@mui/material/Card'

type Props = {
  children: React.ReactNode;
};

const AppCard = ({ children }: Props) => {
  const { isCardShadow } = useCustomizer();

  return (
    <Card
      sx={{ display: 'flex', p: 0 }}
      elevation={isCardShadow ? 9 : 0}
      variant={!isCardShadow ? 'outlined' : undefined}
    >
      {children}
    </Card>
  );
};

export default AppCard;
