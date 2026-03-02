import { OrderType } from "@/types/table/order";

export const orderData: OrderType[] = [
  {
    id: "ORD-001",
    customer: {
      name: "John Doe",
      avatar: "/images/profile/user-10.jpg",
    },
    products: [
      {
        image: "/images/products/s1.jpg",
        name: "Wireless Headphones",
        quantity: 1,
        price: 120,
        sku: 58988,
      },
      {
        image: "/images/products/s2.jpg",
        name: "Bluetooth Speaker",
        quantity: 2,
        price: 90,
        sku: 52988,
      },
    ],
    status: "Pending",
    date: "06-10-2025",
    time: "10:31AM",
    amount: 120,
    address: "123 Main St, New York, NY",
  },
  {
    id: "ORD-002",
    customer: {
      name: "Jane Smith",
      avatar: "/images/profile/user-6.jpg",
    },
    products: [
      {
        image: "/images/products/s3.jpg",
        name: "Smartwatch Series 9",
        quantity: 1,
        price: 120,
        sku: 52982,
      },
    ],
    status: "Completed",
    date: "06-09-2025",
    time: "11:31AM",
    amount: 150,
    address: "456 Elm St, Los Angeles, CA",
  },
  {
    id: "ORD-003",
    customer: {
      name: "Bob Johnson",
      avatar: "/images/profile/user-7.jpg",
    },
    products: [
      {
        image: "/images/products/s5.jpg",
        name: "Canvas Backpack",
        quantity: 1,
        price: 120,
        sku: 12988,
      },
      {
        image: "/images/products/s6.jpg",
        name: "Wireless Charging Pad",
        quantity: 2,
        price: 90,
        sku: 52988,
      },
    ],
    status: "Shipped",
    date: "06-08-2025",
    time: "12:01AM",
    amount: 210,
    address: "789 Pine St, Chicago, IL",
  },
  {
    id: "ORD-004",
    customer: {
      name: "Georgeanna Ramero",
      avatar: "/images/profile/user-2.jpg",
    },
    products: [
      {
        image: "/images/products/s8.jpg",
        name: "Fitness Resistance Bands",
        quantity: 1,
        price: 120,
        sku: 32988,
      },
    ],
    status: "Cancelled",
    date: "06-08-2025",
    time: "01:06AM",
    amount: 310,
    address: "19214 110th Rd, Saint Albans, NY, 1141",
  },
  {
    id: "ORD-005",
    customer: {
      name: "Dalton Paden",
      avatar: "/images/profile/user-6.jpg",
    },
    products: [
      {
        image: "/images/products/s9.jpg",
        name: "Ceramic Coffee Mug",
        quantity: 1,
        price: 120,
        sku: 52988,
      },
      {
        image: "/images/products/s10.jpg",
        name: "Essential Oil Diffuser",
        quantity: 2,
        price: 90,
        sku: 52988,
      },
    ],
    status: "Processing",
    date: "2025-06-08",
    time: "02:34AM",
    amount: 600,
    address: "19103 Stefani Ave, Cerritos, CA, 90703",
  },
  {
    id: "ORD-006",
    customer: {
      name: "Cami Macha",
      avatar: "/images/profile/user-3.jpg",
    },
    products: [
      {
        image: "/images/products/s12.jpg",
        name: "Stainless Steel Watch",
        quantity: 1,
        price: 120,
        sku: 72988,
      },
      {
        image: "/images/products/s11.jpg",
        name: "Wireless Charging Pad",
        quantity: 2,
        price: 90,
        sku: 62988,
      },
    ],
    status: "Pending",
    date: "06-08-2025",
    time: "02:34AM",
    amount: 500,
    address: "930 Fruit Ave, Farrell, PA, 16121",
  },
];
