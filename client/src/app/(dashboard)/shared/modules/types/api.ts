export interface ApiResponse<T> {
  succeeded?: boolean;
  data?: T;
  meta?: unknown;
  message?: string;
  errors?: Record<string, string[]> | string[];
  detail?: string;
  title?: string;
  status?: number;
}

export interface ApiErrorResponse {
  message?: string;
  errors?: Record<string, string[]> | string[];
  detail?: string;
  title?: string;
  status?: number;
}

export function extractApiData<T>(response: { data: ApiResponse<T> | T }): T {
  const responseData = response.data;
  if (responseData && typeof responseData === 'object' && 'data' in responseData) {
    // Check if it's a PaginatedResponse (has currentPage, totalPages, etc.)
    // If so, return it as-is without extracting the data property
    if ('currentPage' in responseData || 'totalPages' in responseData || 'pageSize' in responseData) {
      // This is a PaginatedResponse, return it as-is
      if ('succeeded' in responseData && responseData.succeeded === false) {
        if ('messages' in responseData && Array.isArray(responseData.messages) && responseData.messages.length > 0) {
          throw new Error(responseData.messages[0]);
        }
        throw new Error('Request failed');
      }
      return responseData as T;
    }
    // Otherwise, treat it as an ApiResponse wrapper
    const apiResponse = responseData as ApiResponse<T>;
    if (apiResponse.succeeded === false) {
      if (apiResponse.message) {
        throw new Error(apiResponse.message);
      }
      throw new Error('Request failed');
    }
    return apiResponse.data as T;
  }
  return responseData as T;
}

export function getApiErrorMessage(error: unknown): string {
  if (error instanceof Error) {
    return error.message;
  }
  return 'An error occurred';
}
