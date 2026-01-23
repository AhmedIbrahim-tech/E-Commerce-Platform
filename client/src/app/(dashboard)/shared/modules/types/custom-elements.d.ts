import { CSSProperties } from "react";

declare global {
  namespace JSX {
    interface IntrinsicElements {
      "lord-icon": React.DetailedHTMLProps<
        React.HTMLAttributes<HTMLElement> & {
          src?: string;
          trigger?: string;
          colors?: string;
          style?: CSSProperties;
        },
        HTMLElement
      >;
    }
  }

  interface Window {
    jsVectorMap?: new (options: {
      map: string;
      selector: string;
      zoomOnScroll?: boolean;
      zoomButtons?: boolean;
      markers?: Array<{ name: string; coords: [number, number] }>;
      regionStyle?: {
        initial?: {
          fill?: string;
          fillOpacity?: number;
          stroke?: string;
          strokeWidth?: number;
        };
        hover?: {
          fill?: string;
          fillOpacity?: number;
        };
      };
      [key: string]: unknown;
    }) => void;
    SimpleBar?: new (el: HTMLElement) => void;
    feather?: {
      replace: () => void;
    };
  }
}

export {};
