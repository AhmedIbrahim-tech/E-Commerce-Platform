export interface ValidationError {
    PropertyName?: string;
    ErrorMessage?: string;
    propertyName?: string;
    errorMessage?: string;
}

export interface ApiError {
    title?: string;
    detail?: string;
    status?: number;
    instance?: string;
    traceId?: string;
    errors?: ValidationError[] | Record<string, string[]> | string[];

    // Fallback for custom wrapped ApiResponse payload
    statusCode?: number;
    message?: string;
    succeeded?: boolean;
}
