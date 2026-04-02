import type { OrderStatus } from '@/constants/orderStatuses';

export interface OrderItem {
    id: number | string;
    productId: number;
    productName: string;
    thumbnailUrl?: string;
    price: number;
    quantity: number;
    subtotal: number;
}

export interface Order {
    id: number | string;
    orderNumber: string;
    status: OrderStatus;
    items: OrderItem[];
    totalAmount: number;
    shippingAddressId: number;
    shippingAddress?: ShippingAddressSummary;
    paymentMethod?: string;
    notes?: string;
    createdAt: string;
    updatedAt?: string;
}

export interface ShippingAddressSummary {
    fullName: string;
    addressLine: string;
    city: string;
    country: string;
    phone: string;
}

export interface CreateOrderDto {
    shippingAddressId: number;
    paymentMethod: string;
    notes?: string;
}

export interface UpdateOrderStatusDto {
    status: OrderStatus;
}
