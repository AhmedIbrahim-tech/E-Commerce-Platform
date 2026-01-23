"use client";

import { useEffect } from "react";
import { useRouter } from "next/navigation";

import { AppRoutes } from "@/constants/routes";
import { useAppSelector } from "@/store/hooks";

export default function AuthGuard({ children }: { children: React.ReactNode }) {
  const router = useRouter();
  const { isAuthenticated, isInitialized, isInitializing } = useAppSelector((s) => s.auth);

  useEffect(() => {
    if (isInitialized && !isAuthenticated) {
      router.replace(AppRoutes.Auth.Login);
    }
  }, [isAuthenticated, isInitialized, router]);

  if (!isInitialized || isInitializing) {
    return (
      <div className="d-flex align-items-center justify-content-center" style={{ minHeight: "60vh" }}>
        <div className="text-muted">Loading...</div>
      </div>
    );
  }

  if (!isAuthenticated) return null;

  return <>{children}</>;
}

