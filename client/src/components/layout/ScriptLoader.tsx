"use client";

import Script from "next/script";
import { useState, useEffect } from "react";
import { JS_ASSETS } from "@/constants/layout";
import { ensureJsVectorMap } from "@/utils/jsVectorMap";

/**
 * ScriptLoader - Loads external JS assets only after React components have mounted.
 * 
 * This component handles loading of external JavaScript libraries that are needed
 * for the template to function (Bootstrap, SimpleBar, etc.)
 * 
 * Note: app.js and plugins.js are NOT loaded here as their functionality
 * is implemented directly in React components to avoid DOM conflicts.
 */
export default function ScriptLoader() {
  const [isMounted, setIsMounted] = useState(false);
  const [loadedScripts, setLoadedScripts] = useState(0);

  useEffect(() => {
    // Delay mounting to ensure all React components have fully rendered
    const timer = setTimeout(() => {
      setIsMounted(true);
    }, 300);

    return () => clearTimeout(timer);
  }, []);

  // Initialize SimpleBar after scripts are loaded
  useEffect(() => {
    if (loadedScripts >= JS_ASSETS.length && typeof window !== "undefined") {
      // Initialize SimpleBar on scrollable elements
      const initSimpleBar = () => {
        const scrollbarElement = document.getElementById("scrollbar");
        if (scrollbarElement && window.SimpleBar) {
          try {
            new window.SimpleBar(scrollbarElement);
          } catch {
            // SimpleBar may already be initialized
          }
        }
      };

      // Initialize Feather icons
      const initFeather = () => {
        if (window.feather) {
          window.feather.replace();
        }
      };

      // Ensure jsVectorMap is available globally
      ensureJsVectorMap();

      // Small delay to ensure scripts are fully loaded
      setTimeout(() => {
        initSimpleBar();
        initFeather();
      }, 100);
    }
  }, [loadedScripts]);

  if (!isMounted) {
    return null;
  }

  const handleScriptLoad = (src: string) => {
    // Ensure jsVectorMap is available after loading the main library
    if (src.includes("jsvectormap.min.js")) {
      // The library should be available after this script loads
      // Small delay to ensure the script has fully initialized
      setTimeout(() => {
        ensureJsVectorMap();
        setLoadedScripts((prev) => prev + 1);
      }, 100);
    } else {
      setLoadedScripts((prev) => prev + 1);
    }
  };

  return (
    <>
      {JS_ASSETS.map(({ src }) => (
        <Script 
          key={src} 
          src={src} 
          strategy="lazyOnload"
          onLoad={() => handleScriptLoad(src)}
          onError={() => console.warn(`Failed to load script: ${src}`)}
        />
      ))}
    </>
  );
}
