"use client";

import { ReactNode, useState } from "react";
import Button from "./Button";

export interface WizardStep {
  id: string;
  label: string;
  icon?: string;
  content: ReactNode;
}

interface WizardProps {
  steps: WizardStep[];
  onComplete?: () => void;
  arrowNav?: boolean;
  className?: string;
}

export default function Wizard({ steps, onComplete, arrowNav = false, className = "" }: WizardProps) {
  const [currentStep, setCurrentStep] = useState(0);

  const nextStep = () => {
    if (currentStep < steps.length - 1) {
      setCurrentStep(currentStep + 1);
    } else {
      onComplete?.();
    }
  };

  const prevStep = () => {
    if (currentStep > 0) {
      setCurrentStep(currentStep - 1);
    }
  };

  const goToStep = (index: number) => {
    if (index >= 0 && index < steps.length) {
      setCurrentStep(index);
    }
  };

  return (
    <div className={className}>
      {arrowNav && (
        <div className="step-arrow-nav mb-4">
          <ul className="nav nav-pills custom-nav nav-justified" role="tablist">
            {steps.map((step, index) => (
              <li key={step.id} className="nav-item" role="presentation">
                <button
                  className={`nav-link ${index === currentStep ? "active" : index < currentStep ? "done" : ""}`}
                  onClick={() => goToStep(index)}
                  type="button"
                  role="tab"
                >
                  {step.icon && <i className={step.icon}></i>}
                  {step.label}
                </button>
              </li>
            ))}
          </ul>
        </div>
      )}

      <div className="tab-content">
        {steps.map((step, index) => (
          <div
            key={step.id}
            className={`tab-pane fade ${index === currentStep ? "show active" : ""}`}
            role="tabpanel"
          >
            {step.content}
          </div>
        ))}
      </div>

      <div className="d-flex align-items-start gap-3 mt-4">
        {currentStep > 0 && (
          <Button variant="light" onClick={prevStep}>
            Previous
          </Button>
        )}
        <Button
          variant="success"
          label
          labelIcon="ri-arrow-right-line"
          labelPosition="right"
          onClick={nextStep}
          className="ms-auto"
        >
          {currentStep === steps.length - 1 ? "Complete" : "Next"}
        </Button>
      </div>
    </div>
  );
}
