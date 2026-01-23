"use client";

import { ReactNode } from "react";

export type ProgressVariant = "primary" | "secondary" | "success" | "danger" | "warning" | "info" | "light" | "dark";

interface ProgressBar {
  value: number;
  variant?: ProgressVariant;
  label?: string;
  animated?: boolean;
}

interface ProgressProps {
  bars: ProgressBar[];
  height?: string;
  animated?: boolean;
  stepArrow?: boolean;
  withSteps?: boolean;
  customLabel?: boolean;
  className?: string;
}

export default function Progress({
  bars,
  height,
  animated = false,
  stepArrow = false,
  withSteps = false,
  customLabel = false,
  className = "",
}: ProgressProps) {
  const progressClasses = [
    "progress",
    animated && "animated-progress",
    stepArrow && "progress-step-arrow",
    customLabel && "custom-progress progress-label",
    className,
  ]
    .filter(Boolean)
    .join(" ");

  if (withSteps) {
    return (
      <div className="position-relative m-4">
        <div className="progress" style={{ height: height || "1px" }}>
          {bars.map((bar, index) => (
            <div
              key={index}
              className={`progress-bar ${bar.variant ? `bg-${bar.variant}` : ""}`}
              role="progressbar"
              style={{ width: `${bar.value}%` }}
              aria-valuenow={bar.value}
              aria-valuemin={0}
              aria-valuemax={100}
            ></div>
          ))}
        </div>
        {bars.map((bar, index) => (
          <button
            key={index}
            type="button"
            className={`position-absolute top-0 start-${index === 0 ? "0" : index === 1 ? "50" : "100"} translate-middle btn btn-sm ${index < bars.length - 1 ? `btn-${bar.variant || "primary"}` : "btn-light"} rounded-pill`}
            style={{ width: "2rem", height: "2rem" }}
          >
            {index + 1}
          </button>
        ))}
      </div>
    );
  }

  if (stepArrow) {
    return (
      <div className={progressClasses}>
        {bars.map((bar, index) => (
          <div
            key={index}
            className={`progress-bar ${bar.variant ? `bg-${bar.variant}` : ""}`}
            role="progressbar"
            style={{ width: "100%" }}
            aria-valuenow={bar.value}
            aria-valuemin={0}
            aria-valuemax={100}
          >
            {bar.label || `Step ${index + 1}`}
          </div>
        ))}
      </div>
    );
  }

  return (
    <div className={progressClasses} style={{ height }}>
      {bars.map((bar, index) => (
        <div
          key={index}
          className={`progress-bar ${bar.variant ? `bg-${bar.variant}` : ""} ${animated ? "animated-progress" : ""}`}
          role="progressbar"
          style={{ width: `${bar.value}%` }}
          aria-valuenow={bar.value}
          aria-valuemin={0}
          aria-valuemax={100}
        >
          {customLabel && bar.label && <div className="label">{bar.label}</div>}
        </div>
      ))}
    </div>
  );
}

interface ContentProgressProps {
  percentage: number;
  label: string;
  timeLeft?: string;
  variant?: ProgressVariant;
  className?: string;
}

export function ContentProgress({
  percentage,
  label,
  timeLeft,
  variant = "secondary",
  className = "",
}: ContentProgressProps) {
  return (
    <div className={`card bg-light overflow-hidden shadow-none ${className}`}>
      <div className="card-body">
        <div className="d-flex">
          <div className="flex-grow-1">
            <h6 className="mb-0">
              <b className={`text-${variant}`}>{percentage}%</b> {label}
            </h6>
          </div>
          {timeLeft && (
            <div className="flex-shrink-0">
              <h6 className="mb-0">{timeLeft}</h6>
            </div>
          )}
        </div>
      </div>
      <div className={`progress bg-soft-${variant} rounded-0`}>
        <div
          className={`progress-bar bg-${variant}`}
          role="progressbar"
          style={{ width: `${percentage}%` }}
          aria-valuenow={percentage}
          aria-valuemin={0}
          aria-valuemax={100}
        ></div>
      </div>
    </div>
  );
}
