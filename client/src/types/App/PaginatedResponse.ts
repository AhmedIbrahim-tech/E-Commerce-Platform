export interface PaginatedResponse<T> {
    currentPage: number;
    totalPages: number;
    totalCount: number;
    meta: any;
    pageSize: number;
    hasPreviousPage: boolean;
    hasNextPage: boolean;
    messages: string[];
    succeeded: boolean;
    data: T[];
}
