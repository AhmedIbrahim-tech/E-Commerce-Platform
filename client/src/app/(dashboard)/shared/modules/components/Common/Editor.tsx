"use client";

import { useEffect, useRef } from "react";

export type EditorType = "classic" | "snow" | "bubble";

interface EditorProps {
  type?: EditorType;
  value?: string;
  onChange?: (content: string) => void;
  placeholder?: string;
  height?: string;
  className?: string;
}

export default function Editor({
  type = "classic",
  value,
  onChange,
  placeholder,
  height = "300px",
  className = "",
}: EditorProps) {
  const editorRef = useRef<HTMLDivElement>(null);
  const editorInstanceRef = useRef<any>(null);

  useEffect(() => {
    if (!editorRef.current) return;

    const initEditor = async () => {
      if (typeof window !== "undefined") {
        try {
          if (type === "classic") {
            // @ts-ignore - Dynamic import for optional dependency
            const ClassicEditor = (await import("@ckeditor/ckeditor5-build-classic" as any)).default;
            if (editorInstanceRef.current) {
              editorInstanceRef.current.destroy();
            }
            editorInstanceRef.current = await ClassicEditor.create(editorRef.current, {
              placeholder,
            });
            if (value) {
              editorInstanceRef.current.setData(value);
            }
            editorInstanceRef.current.model.document.on("change:data", () => {
              const data = editorInstanceRef.current.getData();
              onChange?.(data);
            });
          } else if (type === "snow" || type === "bubble") {
            // @ts-ignore - Dynamic import for optional dependency
            const Quill = (await import("quill" as any)).default;
            if (!editorRef.current) return;
            if (editorInstanceRef.current) {
              editorInstanceRef.current = null;
            }
            const editorClass = type === "snow" ? "snow-editor" : "bubble-editor";
            editorRef.current.className = `${editorClass} ${className}`;
            editorRef.current.style.height = height;
            editorInstanceRef.current = new Quill(editorRef.current, {
              theme: type,
              placeholder,
            });
            if (value) {
              editorInstanceRef.current.root.innerHTML = value;
            }
            editorInstanceRef.current.on("text-change", () => {
              const html = editorInstanceRef.current.root.innerHTML;
              onChange?.(html);
            });
          }
        } catch (error) {
          console.error("Failed to initialize editor:", error);
        }
      }
    };

    initEditor();

    return () => {
      if (editorInstanceRef.current && typeof editorInstanceRef.current.destroy === "function") {
        editorInstanceRef.current.destroy();
      }
    };
  }, [type, placeholder, height, className]);

  useEffect(() => {
    if (editorInstanceRef.current && value !== undefined) {
      if (type === "classic") {
        const currentData = editorInstanceRef.current.getData();
        if (currentData !== value) {
          editorInstanceRef.current.setData(value);
        }
      } else {
        const currentHtml = editorInstanceRef.current.root.innerHTML;
        if (currentHtml !== value) {
          editorInstanceRef.current.root.innerHTML = value;
        }
      }
    }
  }, [value, type]);

  if (type === "classic") {
    return <div ref={editorRef} className={`ckeditor-classic ${className}`}></div>;
  }

  return <div ref={editorRef} className={`${type}-editor ${className}`} style={{ height }}></div>;
}
