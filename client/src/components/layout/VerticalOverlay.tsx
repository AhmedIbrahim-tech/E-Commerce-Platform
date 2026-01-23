"use client";

export default function VerticalOverlay() {
  const handleClick = () => {
    document.body.classList.remove("sidebar-enable");
  };

  return (
    <div 
      className="vertical-overlay" 
      onClick={handleClick}
    />
  );
}
