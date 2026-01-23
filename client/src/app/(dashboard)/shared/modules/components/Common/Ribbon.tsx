"use client";

import { ReactNode } from "react";

export type RibbonVariant = "primary" | "secondary" | "success" | "danger" | "warning" | "info" | "dark" | "light";
export type RibbonPosition = "left" | "right" | "top" | "bottom";

interface RibbonProps {
  variant: RibbonVariant;
  children: ReactNode;
  position?: RibbonPosition;
  rounded?: boolean;
  className?: string;
}

export default function Ribbon({
  variant,
  children,
  position = "left",
  rounded = false,
  className = "",
}: RibbonProps) {
  const ribbonClasses = [
    "ribbon",
    `ribbon-${variant}`,
    rounded && "round-shape",
    className,
  ]
    .filter(Boolean)
    .join(" ");

  return <div className={ribbonClasses}>{children}</div>;
}

interface RibbonBoxProps {
  children: ReactNode;
  ribbon?: ReactNode;
  position?: RibbonPosition;
  className?: string;
}

export function RibbonBox({ children, ribbon, position = "left", className = "" }: RibbonBoxProps) {
  const boxClasses = [
    "card",
    "ribbon-box",
    "border",
    "shadow-none",
    position === "right" && "right",
    className,
  ]
    .filter(Boolean)
    .join(" ");

  return (
    <div className={boxClasses}>
      {ribbon}
      <div className="card-body">
        <div className="ribbon-content">{children}</div>
      </div>
    </div>
  );
}
