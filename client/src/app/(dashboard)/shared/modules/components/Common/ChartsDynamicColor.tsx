export function getChartColorsArray(colors: string): string[] {
  try {
    const parsedColors = JSON.parse(colors);
    return parsedColors.map((color: string) => {
      const trimmedColor = color.replace(/\s/g, "");
      if (trimmedColor.includes("--")) {
        const cssVariable = getComputedStyle(document.documentElement)
          .getPropertyValue(trimmedColor)
          .trim();
        return cssVariable || trimmedColor;
      }
      return trimmedColor;
    });
  } catch {
    return [];
  }
}

export default function ChartsDynamicColor() {
  return null;
}
