export type UserTab = "admins" | "merchants" | "customers";

export type UserItem = import("@/types").Admin | import("@/types").Vendor | import("@/types").Customer;
