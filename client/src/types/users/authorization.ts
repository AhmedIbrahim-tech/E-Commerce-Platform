// ── Role ─────────────────────────────────────────────────────────────

export interface Role {
    id: string;
    name: string;
}

// ── User Role management ────────────────────────────────────────────

export interface UserRoleItem {
    id: string;
    name: string;
    hasRole: boolean;
}

export interface ManageUserRolesResponse {
    userId: string;
    roles: UserRoleItem[];
}

export interface UpdateUserRolesPayload {
    userId: string;
    roles: UserRoleItem[];
}

// ── User Claims / Permissions ───────────────────────────────────────

export interface UserClaimItem {
    type: string;
    value: boolean;
}

export interface ManageUserClaimsResponse {
    userId: string;
    claims: UserClaimItem[];
}

export interface UpdateUserClaimsPayload {
    userId: string;
    claims: UserClaimItem[];
}

// ── Authorization state ─────────────────────────────────────────────

export interface AuthorizationState {
    roles: Role[];
    rolesLoading: boolean;
    rolesError: string | null;

    selectedRole: Role | null;

    userRoles: ManageUserRolesResponse | null;
    userRolesLoading: boolean;

    userClaims: ManageUserClaimsResponse | null;
    userClaimsLoading: boolean;

    actionLoading: boolean;
    actionError: string | null;
}

export const authorizationInitialState: AuthorizationState = {
    roles: [],
    rolesLoading: false,
    rolesError: null,

    selectedRole: null,

    userRoles: null,
    userRolesLoading: false,

    userClaims: null,
    userClaimsLoading: false,

    actionLoading: false,
    actionError: null,
};
