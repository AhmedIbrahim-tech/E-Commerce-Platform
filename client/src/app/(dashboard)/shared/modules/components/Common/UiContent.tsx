import { ReactNode } from "react";

interface UIContentProps {
  children?: ReactNode;
}

export default function UIContent({ children }: UIContentProps) {
  return <div className="page-content">{children}</div>;
}
