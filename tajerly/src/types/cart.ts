export interface CartItem {
    id: number | string;
    productId: number;
    productName: string;
    productSlug: string;
    thumbnailUrl?: string;
    price: number;
    quantity: number;
    /** price × quantity */
    subtotal: number;
    stock: number;
}

export interface Cart {
    items: CartItem[];
    /** Sum of all item subtotals */
    totalAmount: number;
    /** Number of distinct items */
    itemCount: number;
}

export interface AddToCartDto {
    productId: number;
    quantity: number;
}

export interface UpdateCartItemDto {
    quantity: number;
}
