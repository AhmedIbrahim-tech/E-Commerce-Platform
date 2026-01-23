"use client";

import { useState, useCallback } from "react";

const DEFAULT_AVATAR = "/assets/images/users/user-dummy-img.jpg";

export function getProfileImageSrc(profileImageUrl: string | null | undefined): string {
  if (!profileImageUrl || profileImageUrl.trim() === "") {
    return DEFAULT_AVATAR;
  }
  return profileImageUrl;
}

interface ProfileImageProps {
  profileImageUrl?: string | null;
  previewImageUrl?: string | null;
  alt?: string;
  className?: string;
  showLoadingSpinner?: boolean;
  size?: number;
}

export default function ProfileImage({
  profileImageUrl,
  previewImageUrl,
  alt = "Profile",
  className = "",
  showLoadingSpinner = false,
  size,
}: ProfileImageProps) {
  const [imageError, setImageError] = useState(false);
  const [isLoading, setIsLoading] = useState(false);

  const effectiveImageUrl = previewImageUrl || profileImageUrl;
  const imageSrc = previewImageUrl ? previewImageUrl : getProfileImageSrc(profileImageUrl);
  const shouldShowDefault = imageError || (!effectiveImageUrl || effectiveImageUrl.trim() === "");

  const handleLoad = useCallback(() => {
    setIsLoading(false);
    setImageError(false);
  }, []);

  const handleError = useCallback((e: React.SyntheticEvent<HTMLImageElement>) => {
    const target = e.target as HTMLImageElement;
    setIsLoading(false);
    if (target.src !== DEFAULT_AVATAR && !target.src.includes(DEFAULT_AVATAR)) {
      setImageError(true);
      // Force reload with default avatar
      setTimeout(() => {
        if (target.src !== DEFAULT_AVATAR) {
          target.src = DEFAULT_AVATAR;
        }
      }, 0);
    }
  }, []);

  const finalSrc = shouldShowDefault ? DEFAULT_AVATAR : imageSrc;

  if (showLoadingSpinner && isLoading && !shouldShowDefault) {
    return (
      <div
        className={`img-thumbnail rounded-circle bg-light d-flex align-items-center justify-content-center ${className}`}
        style={size ? { width: `${size}px`, height: `${size}px`, objectFit: "cover" } : undefined}
      >
        <div className="spinner-border spinner-border-sm text-primary" role="status">
          <span className="visually-hidden">Loading...</span>
        </div>
      </div>
    );
  }

  return (
    <img
      src={finalSrc}
      alt={alt}
      className={className}
      onLoad={handleLoad}
      onError={handleError}
      style={
        size
          ? {
              width: `${size}px`,
              height: `${size}px`,
              objectFit: "cover",
              display: "block",
            }
          : { objectFit: "cover", display: "block" }
      }
    />
  );
}
