"use client";

import { useRef, useState } from "react";

interface FileUploadProps {
  multiple?: boolean;
  accept?: string;
  maxSize?: number;
  onFilesSelected?: (files: File[]) => void;
  className?: string;
}

export default function FileUpload({
  multiple = false,
  accept,
  maxSize,
  onFilesSelected,
  className = "",
}: FileUploadProps) {
  const [files, setFiles] = useState<File[]>([]);
  const [previews, setPreviews] = useState<string[]>([]);
  const fileInputRef = useRef<HTMLInputElement>(null);
  const dropzoneRef = useRef<HTMLDivElement>(null);

  const handleFileSelect = (selectedFiles: FileList | null) => {
    if (!selectedFiles) return;

    const fileArray = Array.from(selectedFiles);
    const validFiles = fileArray.filter((file) => {
      if (maxSize && file.size > maxSize) {
        alert(`File ${file.name} exceeds maximum size of ${maxSize / 1024 / 1024}MB`);
        return false;
      }
      return true;
    });

    setFiles((prev) => [...prev, ...validFiles]);
    onFilesSelected?.(validFiles);

    validFiles.forEach((file) => {
      if (file.type.startsWith("image/")) {
        const reader = new FileReader();
        reader.onload = (e) => {
          setPreviews((prev) => [...prev, e.target?.result as string]);
        };
        reader.readAsDataURL(file);
      }
    });
  };

  const handleDrop = (e: React.DragEvent) => {
    e.preventDefault();
    handleFileSelect(e.dataTransfer.files);
  };

  const handleDragOver = (e: React.DragEvent) => {
    e.preventDefault();
  };

  const removeFile = (index: number) => {
    setFiles((prev) => prev.filter((_, i) => i !== index));
    setPreviews((prev) => prev.filter((_, i) => i !== index));
  };

  return (
    <div className={className}>
      <div
        ref={dropzoneRef}
        className="dropzone"
        onDrop={handleDrop}
        onDragOver={handleDragOver}
      >
        <div className="fallback">
          <input
            ref={fileInputRef}
            type="file"
            multiple={multiple}
            accept={accept}
            onChange={(e) => handleFileSelect(e.target.files)}
          />
        </div>
        <div className="dz-message needsclick">
          <div className="mb-3">
            <i className="display-4 text-muted ri-upload-cloud-2-fill"></i>
          </div>
          <h4>Drop files here or click to upload.</h4>
        </div>
      </div>

      {files.length > 0 && (
        <ul className="list-unstyled mb-0 mt-3">
          {files.map((file, index) => (
            <li key={index} className="mt-2">
              <div className="border rounded">
                <div className="d-flex p-2">
                  {previews[index] && (
                    <div className="flex-shrink-0 me-3">
                      <div className="avatar-sm bg-light rounded">
                        <img
                          src={previews[index]}
                          className="img-fluid rounded d-block"
                          alt={file.name}
                        />
                      </div>
                    </div>
                  )}
                  <div className="flex-grow-1">
                    <div className="pt-1">
                      <h5 className="fs-14 mb-1">{file.name}</h5>
                      <p className="fs-13 text-muted mb-0">
                        {(file.size / 1024).toFixed(2)} KB
                      </p>
                    </div>
                  </div>
                  <div className="flex-shrink-0 ms-3">
                    <button
                      className="btn btn-sm btn-danger"
                      onClick={() => removeFile(index)}
                    >
                      Delete
                    </button>
                  </div>
                </div>
              </div>
            </li>
          ))}
        </ul>
      )}
    </div>
  );
}
