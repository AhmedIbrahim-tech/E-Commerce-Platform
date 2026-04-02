export interface Brand {
    id: number;
    name: string;
    description?: string;
    logoUrl?: string;
    isActive?: boolean;
    displayOrder?: number;
    productCount?: number;
}

export interface CreateBrandDto {
    name: string;
    description?: string;
    logoUrl?: string;
    isActive: boolean;
    displayOrder?: number;
}

export interface UpdateBrandDto {
    name: string;
    description?: string;
    logoUrl?: string;
    isActive: boolean;
    displayOrder: number;
}
