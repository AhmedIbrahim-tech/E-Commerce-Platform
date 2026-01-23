"use client";

import { ReactNode, useState } from "react";

export interface AccordionItem {
  id: string;
  title: string;
  content: ReactNode;
  defaultOpen?: boolean;
}

interface AccordionProps {
  items: AccordionItem[];
  bordered?: boolean;
  nesting?: boolean;
  variant?: "primary" | "secondary" | "success" | "danger" | "warning" | "info";
  className?: string;
}

export default function Accordion({
  items,
  bordered = false,
  nesting = false,
  variant,
  className = "",
}: AccordionProps) {
  const [openItems, setOpenItems] = useState<Set<string>>(
    new Set(items.filter((item) => item.defaultOpen).map((item) => item.id))
  );

  const toggleItem = (id: string) => {
    const newOpenItems = new Set(openItems);
    if (newOpenItems.has(id)) {
      newOpenItems.delete(id);
    } else {
      newOpenItems.add(id);
    }
    setOpenItems(newOpenItems);
  };

  const accordionClasses = [
    "accordion",
    "custom-accordionwithicon",
    bordered && "custom-accordion-border accordion-border-box",
    variant && `accordion-${variant}`,
    nesting && "nesting-accordion",
    className,
  ]
    .filter(Boolean)
    .join(" ");

  return (
    <div className={accordionClasses}>
      {items.map((item, index) => (
        <div key={item.id} className="accordion-item">
          <h2 className="accordion-header">
            <button
              className={`accordion-button ${openItems.has(item.id) ? "" : "collapsed"}`}
              type="button"
              onClick={() => toggleItem(item.id)}
              aria-expanded={openItems.has(item.id)}
            >
              {item.title}
            </button>
          </h2>
          <div
            className={`accordion-collapse collapse ${openItems.has(item.id) ? "show" : ""}`}
          >
            <div className="accordion-body">{item.content}</div>
          </div>
        </div>
      ))}
    </div>
  );
}
