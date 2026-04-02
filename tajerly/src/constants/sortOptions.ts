export type SortOption = 'newest' | 'price_asc' | 'price_desc' | 'rating' | 'popular' | 'name_asc' | 'name_desc';

export const SORT_OPTIONS: { value: SortOption; label: string }[] = [
    { value: 'newest', label: 'Newest First' },
    { value: 'popular', label: 'Most Popular' },
    { value: 'rating', label: 'Highest Rated' },
    { value: 'price_asc', label: 'Price: Low to High' },
    { value: 'price_desc', label: 'Price: High to Low' },
    { value: 'name_asc', label: 'Name: A to Z' },
    { value: 'name_desc', label: 'Name: Z to A' },
];

/** Map frontend sort option to backend sortBy + sortDescending params */
export function getSortParams(option: SortOption): { sortBy: string; sortDescending: boolean } {
    switch (option) {
        case 'newest': return { sortBy: 'CreatedAt', sortDescending: true };
        case 'popular': return { sortBy: 'SalesCount', sortDescending: true };
        case 'rating': return { sortBy: 'AverageRating', sortDescending: true };
        case 'price_asc': return { sortBy: 'Price', sortDescending: false };
        case 'price_desc': return { sortBy: 'Price', sortDescending: true };
        case 'name_asc': return { sortBy: 'Name', sortDescending: false };
        case 'name_desc': return { sortBy: 'Name', sortDescending: true };
        default: return { sortBy: 'CreatedAt', sortDescending: true };
    }
}
