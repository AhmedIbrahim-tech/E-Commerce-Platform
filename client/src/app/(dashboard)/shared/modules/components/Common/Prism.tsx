import { ReactNode } from "react";

interface PrismProps {
  code: string;
  language?: string;
  children?: ReactNode;
}

export default function Prism({ code, language = "javascript" }: PrismProps) {
  return (
    <div className="live-preview">
      <pre>
        <code className={`language-${language}`}>{code}</code>
      </pre>
    </div>
  );
}
