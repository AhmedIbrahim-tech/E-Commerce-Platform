import type { AuthTokens } from "@/types";

const AUTH_TOKENS_KEY = "auth_tokens";

function isValidTokens(value: unknown): value is AuthTokens {
  if (!value || typeof value !== "object") return false;
  const v = value as Record<string, unknown>;
  return (
    typeof v.accessToken === "string" &&
    v.accessToken.trim() !== "" &&
    typeof v.refreshToken === "string" &&
    v.refreshToken.trim() !== ""
  );
}

export function getStoredTokens(): AuthTokens | null {
  if (typeof window === "undefined") return null;
  const raw = localStorage.getItem(AUTH_TOKENS_KEY);
  if (!raw || raw === "null" || raw === "undefined" || raw.trim() === "") return null;

  try {
    const parsed = JSON.parse(raw) as unknown;
    if (isValidTokens(parsed)) return parsed;
    localStorage.removeItem(AUTH_TOKENS_KEY);
    return null;
  } catch {
    localStorage.removeItem(AUTH_TOKENS_KEY);
    return null;
  }
}

export function setStoredTokens(tokens: AuthTokens): void {
  if (typeof window === "undefined") return;
  localStorage.setItem(AUTH_TOKENS_KEY, JSON.stringify(tokens));
}

export function clearStoredTokens(): void {
  if (typeof window === "undefined") return;
  localStorage.removeItem(AUTH_TOKENS_KEY);
}

