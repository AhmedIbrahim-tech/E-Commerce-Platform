/**
 * Utility to ensure jsVectorMap is available globally
 * This prevents "jsVectorMap is not defined" errors
 */

if (typeof window !== "undefined") {
  // Create a no-op fallback if jsVectorMap is not yet loaded
  if (typeof (window as any).jsVectorMap === "undefined") {
    (window as any).jsVectorMap = class {
      constructor(options: any) {
        console.warn("jsVectorMap is not yet loaded. Please wait for scripts to load.");
        // Return a mock object to prevent errors
        return {
          destroy: () => {},
          update: () => {},
        };
      }
    };
  }
}

export const ensureJsVectorMap = (): boolean => {
  if (typeof window === "undefined") return false;
  
  // Check if jsVectorMap is available
  if (typeof (window as any).jsVectorMap !== "undefined") {
    return true;
  }
  
  // Try to find it under different names
  if (typeof (window as any).jsvectormap !== "undefined") {
    (window as any).jsVectorMap = (window as any).jsvectormap;
    return true;
  }
  
  if (typeof (window as any).VectorMap !== "undefined") {
    (window as any).jsVectorMap = (window as any).VectorMap;
    return true;
  }
  
  return false;
};
