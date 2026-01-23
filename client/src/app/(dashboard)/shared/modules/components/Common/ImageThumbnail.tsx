"use client";

interface ImageThumbnailProps {
  src: string;
  alt?: string;
  width?: number;
  height?: number;
  rounded?: boolean;
  roundedCircle?: boolean;
  className?: string;
}

export default function ImageThumbnail({
  src,
  alt = "Thumbnail",
  width,
  height,
  rounded = false,
  roundedCircle = false,
  className = "",
}: ImageThumbnailProps) {
  const imageClasses = [
    "img-thumbnail",
    rounded && "rounded",
    roundedCircle && "rounded-circle",
    className,
  ]
    .filter(Boolean)
    .join(" ");

  return (
    <img
      src={src}
      alt={alt}
      className={imageClasses}
      width={width}
      height={height}
    />
  );
}
