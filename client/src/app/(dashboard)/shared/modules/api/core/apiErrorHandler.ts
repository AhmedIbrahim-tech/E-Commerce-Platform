import axios from "axios";

type ProblemDetails = {
  title?: string;
  detail?: string;
  status?: number;
  errors?: Record<string, string[]> | string[];
};

type ApiResponseError = {
  message?: string;
  errors?: Record<string, string[]> | string[];
  detail?: string;
  title?: string;
  status?: number;
  messages?: string[];
};

export type NormalizedApiError = {
  message: string;
  status?: number;
  errors?: string[];
  raw?: unknown;
};

function flattenErrors(errors: Record<string, string[]> | string[] | undefined): string[] | undefined {
  if (!errors) return undefined;
  if (Array.isArray(errors)) return errors.filter(Boolean);
  const flattened = Object.values(errors).flat().filter(Boolean);
  return flattened.length ? flattened : undefined;
}

export function normalizeApiError(error: unknown): NormalizedApiError {
  if (axios.isAxiosError(error)) {
    const status = error.response?.status;
    const data = error.response?.data as ApiResponseError | ProblemDetails | undefined;

    const errors = flattenErrors(data?.errors);
    const message =
      data?.detail ||
      (data as ApiResponseError | undefined)?.message ||
      (Array.isArray((data as ApiResponseError | undefined)?.messages)
        ? (data as ApiResponseError).messages?.[0]
        : undefined) ||
      data?.title ||
      (status === 401
        ? "Unauthorized. Please login again."
        : status === 403
          ? "Access denied. You do not have permission to perform this action."
          : status === 404
            ? "Resource not found."
            : status === 429
              ? "Too many requests. Please try again later."
              : status && status >= 500
                ? "Server error. Please try again later."
                : undefined) ||
      (error.response ? error.message : "Network error. Please check your connection.") ||
      "An error occurred";

    return { message, status, errors, raw: error };
  }

  if (error instanceof Error) {
    return { message: error.message || "An error occurred", raw: error };
  }

  return { message: "An error occurred", raw: error };
}

export function handleApiError(error: unknown): never {
  const normalized = normalizeApiError(error);
  const err = new Error(normalized.message);
  (err as unknown as { status?: number }).status = normalized.status;
  (err as unknown as { errors?: string[] }).errors = normalized.errors;
  throw err;
}

