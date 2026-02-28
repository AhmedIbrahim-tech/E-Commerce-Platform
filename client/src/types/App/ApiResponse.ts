export interface ApiResponse<T> {
    statusCode: number;
    meta: any;
    succeeded: boolean;
    message: string;
    errors: any;
    data: T;
}