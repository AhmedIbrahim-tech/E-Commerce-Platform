import { useCallback, useEffect, useRef, useState } from "react";

export interface UseListTableOptions<T> {
  /** Function to fetch data from API */
  fetcher: (page: number, pageSize: number, searchTerm?: string) => Promise<{ data: T[]; totalCount: number }>;
  /** Initial page size */
  initialPageSize?: number;
  /** Initial search term */
  initialSearchTerm?: string;
  /** Auto-fetch on mount */
  autoFetch?: boolean;
}

export interface UseListTableReturn<T> {
  /** Current page data */
  data: T[];
  /** Loading state */
  loading: boolean;
  /** Error state */
  error: string | null;
  /** Current page number */
  currentPage: number;
  /** Page size */
  pageSize: number;
  /** Total count of items */
  totalCount: number;
  /** Search term */
  searchTerm: string;
  /** Set current page */
  setCurrentPage: (page: number) => void;
  /** Set page size */
  setPageSize: (size: number) => void;
  /** Set search term */
  setSearchTerm: (term: string) => void;
  /** Reload data */
  reload: () => void;
  /** Refetch data manually */
  refetch: () => Promise<void>;
}

/**
 * Centralized hook for managing list table state and data fetching
 * Handles pagination, search, loading, and error states
 */
export function useListTable<T>({
  fetcher,
  initialPageSize = 10,
  initialSearchTerm = "",
  autoFetch = true,
}: UseListTableOptions<T>): UseListTableReturn<T> {
  const [data, setData] = useState<T[]>([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const [currentPage, setCurrentPage] = useState(1);
  const [pageSize, setPageSize] = useState(initialPageSize);
  const [totalCount, setTotalCount] = useState(0);
  const [searchTerm, setSearchTerm] = useState(initialSearchTerm);
  const [reloadToken, setReloadToken] = useState(0);
  const fetcherRef = useRef(fetcher);

  // Update fetcher ref when it changes
  useEffect(() => {
    fetcherRef.current = fetcher;
  }, [fetcher]);

  const fetchData = useCallback(async () => {
    setLoading(true);
    setError(null);
    try {
      const response = await fetcherRef.current(currentPage, pageSize, searchTerm || undefined);
      setData(response.data || []);
      setTotalCount(response.totalCount || 0);
    } catch (err) {
      setError(err instanceof Error ? err.message : "Failed to load data");
      setData([]);
      setTotalCount(0);
    } finally {
      setLoading(false);
    }
  }, [currentPage, pageSize, searchTerm]);

  useEffect(() => {
    if (autoFetch) {
      void fetchData();
    }
  }, [fetchData, autoFetch, reloadToken]);

  const handleSetCurrentPage = useCallback((page: number) => {
    setCurrentPage(page);
  }, []);

  const handleSetPageSize = useCallback((size: number) => {
    setPageSize(size);
    setCurrentPage(1);
  }, []);

  const handleSetSearchTerm = useCallback((term: string) => {
    setSearchTerm(term);
    setCurrentPage(1);
  }, []);

  const reload = useCallback(() => {
    setReloadToken((v) => v + 1);
  }, []);

  const refetch = useCallback(async () => {
    await fetchData();
  }, [fetchData]);

  return {
    data,
    loading,
    error,
    currentPage,
    pageSize,
    totalCount,
    searchTerm,
    setCurrentPage: handleSetCurrentPage,
    setPageSize: handleSetPageSize,
    setSearchTerm: handleSetSearchTerm,
    reload,
    refetch,
  };
}
