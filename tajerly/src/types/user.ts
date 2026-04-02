/** id is number from API; may be string from route/display. */
export interface User {
    id: number | string;
    fullName: string;
    email: string;
    role: string;
    status: string;
    profilePicture?: string;
    phone?: string;
    createdAt: string;
}

export interface Address {
    id: number | string;
    label: string;
    fullName: string;
    phone: string;
    addressLine1: string;
    addressLine2?: string;
    city: string;
    state?: string;
    postalCode?: string;
    country: string;
    isDefault: boolean;
}

export interface CreateUserDto {
    fullName: string;
    email: string;
    password?: string;
    role: string;
    profilePicture?: string;
}

export interface UpdateUserDto {
    fullName: string;
    email: string;
    role: string;
    profilePicture?: string;
}

export interface UpdateProfileDto {
    fullName: string;
    email: string;
    phone?: string;
    profilePicture?: string;
}

export interface ChangePasswordDto {
    currentPassword: string;
    newPassword: string;
}

export interface CreateAddressDto {
    label: string;
    fullName: string;
    phone: string;
    addressLine1: string;
    addressLine2?: string;
    city: string;
    state?: string;
    postalCode?: string;
    country: string;
    isDefault: boolean;
}

export interface UpdateAddressDto extends CreateAddressDto {
    /* same shape */
}
