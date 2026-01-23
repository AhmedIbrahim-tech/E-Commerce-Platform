"use client";

import { ReactNode } from "react";

interface PlaceholderProps {
  children?: ReactNode;
  className?: string;
  glow?: boolean;
}

export default function Placeholder({ children, className = "", glow = false }: PlaceholderProps) {
  const placeholderClasses = [
    "placeholder",
    glow && "placeholder-glow",
    className,
  ]
    .filter(Boolean)
    .join(" ");

  return <span className={placeholderClasses}>{children}</span>;
}
