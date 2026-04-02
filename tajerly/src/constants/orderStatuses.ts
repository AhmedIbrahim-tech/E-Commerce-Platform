export type OrderStatus = 'Pending' | 'Confirmed' | 'Processing' | 'Shipped' | 'Delivered' | 'Cancelled' | 'Returned';

export const ORDER_STATUS_VALUES: Record<OrderStatus, number> = {
    Pending: 0,
    Confirmed: 1,
    Processing: 2,
    Shipped: 3,
    Delivered: 4,
    Cancelled: 5,
    Returned: 6,
};

export const ORDER_STATUSES: { value: OrderStatus; label: string }[] = [
    { value: 'Pending', label: 'Pending' },
    { value: 'Confirmed', label: 'Confirmed' },
    { value: 'Processing', label: 'Processing' },
    { value: 'Shipped', label: 'Shipped' },
    { value: 'Delivered', label: 'Delivered' },
    { value: 'Cancelled', label: 'Cancelled' },
    { value: 'Returned', label: 'Returned' },
];

export function getOrderStatusLabel(status: OrderStatus | string | null | undefined): string {
    if (!status) return '';
    const found = ORDER_STATUSES.find((s) => s.value === status);
    return found?.label ?? String(status);
}

export type OrderStatusColor = 'warning' | 'info' | 'primary' | 'success' | 'danger' | 'secondary';

export function getOrderStatusColor(status: OrderStatus | string): OrderStatusColor {
    switch (status) {
        case 'Pending': return 'warning';
        case 'Confirmed': return 'info';
        case 'Processing': return 'primary';
        case 'Shipped': return 'info';
        case 'Delivered': return 'success';
        case 'Cancelled': return 'danger';
        case 'Returned': return 'secondary';
        default: return 'secondary';
    }
}
