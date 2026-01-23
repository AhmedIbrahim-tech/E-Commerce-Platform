"use client";

import Link from "next/link";
import { useRouter } from "next/navigation";
import { useEffect, useMemo, useState } from "react";

import { AppRoutes } from "@/constants/routes";
import { useAppDispatch, useAppSelector } from "@/store/hooks";
import { loginAsync } from "@/store/slices/authSlice";
import { UserRole } from "@/types";

export default function LoginPage() {
  const router = useRouter();
  const dispatch = useAppDispatch();

  const { isLoading, isAuthenticated, error, user } = useAppSelector((s) => s.auth);

  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [showPassword, setShowPassword] = useState(false);
  const [rememberMe, setRememberMe] = useState(true);

  const canSubmit = useMemo(() => email.trim() !== "" && password.trim() !== "", [email, password]);

  useEffect(() => {
    /**
     * What caused the redirect loop:
     * - This page redirected ANY authenticated user to `/` (Home) immediately.
     * - So when an already-authenticated Admin/Merchant clicked "Login", they were bounced back to `/`
     *   (instead of their dashboard), making the auth flow look broken.
     *
     * What changed:
     * - Redirect authenticated users to the correct post-login destination based on role.
     * - `/login` stays public for guests (no redirect when unauthenticated).
     */
    if (!isAuthenticated) return;

    const role = user?.role;
    const target =
      role === UserRole.Admin || role === UserRole.SuperAdmin
        ? AppRoutes.Dashboard.Admin
        : role === UserRole.Merchant || role === UserRole.Vendor
          ? AppRoutes.Dashboard.Merchant
          : AppRoutes.Home;

    router.replace(target);
  }, [isAuthenticated, router, user?.role]);

  const onSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!canSubmit || isLoading) return;
    await dispatch(loginAsync({ email: email.trim(), password }));
  };

  return (
    <div className="card overflow-hidden">
      <div className="row g-0">
        <div className="col-lg-6">
          <div className="p-lg-5 p-4 auth-one-bg h-100">
            <div className="bg-overlay"></div>
            <div className="position-relative h-100 d-flex flex-column">
              <div className="mb-4">
                <Link href={AppRoutes.Home} className="d-block">
                  <img src="/assets/images/logo-light.png" alt="Logo" height={18} />
                </Link>
              </div>

              <div className="mt-auto">
                <div className="mb-3">
                  <i className="ri-double-quotes-l display-4 text-success"></i>
                </div>

                <div id="qoutescarouselIndicators" className="carousel slide" data-bs-ride="carousel">
                  <div className="carousel-indicators">
                    <button
                      type="button"
                      data-bs-target="#qoutescarouselIndicators"
                      data-bs-slide-to={0}
                      className="active"
                      aria-current="true"
                      aria-label="Slide 1"
                    ></button>
                    <button
                      type="button"
                      data-bs-target="#qoutescarouselIndicators"
                      data-bs-slide-to={1}
                      aria-label="Slide 2"
                    ></button>
                    <button
                      type="button"
                      data-bs-target="#qoutescarouselIndicators"
                      data-bs-slide-to={2}
                      aria-label="Slide 3"
                    ></button>
                  </div>

                  <div className="carousel-inner text-center text-white pb-5">
                    <div className="carousel-item active">
                      <p className="fs-15 fst-italic">
                        &quot; Great! Clean code, clean design, easy for customization. Thanks very much! &quot;
                      </p>
                    </div>
                    <div className="carousel-item">
                      <p className="fs-15 fst-italic">
                        &quot; The theme is really great with an amazing customer support. &quot;
                      </p>
                    </div>
                    <div className="carousel-item">
                      <p className="fs-15 fst-italic">
                        &quot; Great! Clean code, clean design, easy for customization. Thanks very much! &quot;
                      </p>
                    </div>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>

        <div className="col-lg-6">
          <div className="p-lg-5 p-4">
            <div>
              <h5 className="text-primary">Welcome Back !</h5>
              <p className="text-muted">Sign in to continue.</p>
            </div>

            <div className="mt-4">
              {error ? (
                <div className="alert alert-danger" role="alert">
                  {error}
                </div>
              ) : null}

              <form onSubmit={onSubmit}>
                <div className="mb-3">
                  <label htmlFor="email" className="form-label">
                    Email
                  </label>
                  <input
                    id="email"
                    type="email"
                    className="form-control"
                    placeholder="Enter email"
                    value={email}
                    onChange={(e) => setEmail(e.target.value)}
                    autoComplete="email"
                    disabled={isLoading}
                    required
                  />
                </div>

                <div className="mb-3">
                  <div className="float-end">
                    <a href="#" className="text-muted">
                      Forgot password?
                    </a>
                  </div>
                  <label className="form-label" htmlFor="password-input">
                    Password
                  </label>
                  <div className="position-relative auth-pass-inputgroup mb-3">
                    <input
                      type={showPassword ? "text" : "password"}
                      className="form-control pe-5"
                      placeholder="Enter password"
                      id="password-input"
                      value={password}
                      onChange={(e) => setPassword(e.target.value)}
                      autoComplete="current-password"
                      disabled={isLoading}
                      required
                    />
                    <button
                      className="btn btn-link position-absolute end-0 top-0 text-decoration-none text-muted"
                      type="button"
                      onClick={() => setShowPassword((v) => !v)}
                      aria-label={showPassword ? "Hide password" : "Show password"}
                    >
                      <i className={showPassword ? "ri-eye-off-fill align-middle" : "ri-eye-fill align-middle"}></i>
                    </button>
                  </div>
                </div>

                <div className="form-check">
                  <input
                    className="form-check-input"
                    type="checkbox"
                    id="auth-remember-check"
                    checked={rememberMe}
                    onChange={(e) => setRememberMe(e.target.checked)}
                    disabled={isLoading}
                  />
                  <label className="form-check-label" htmlFor="auth-remember-check">
                    Remember me
                  </label>
                </div>

                <div className="mt-4">
                  <button className="btn btn-success w-100" type="submit" disabled={!canSubmit || isLoading}>
                    {isLoading ? "Signing In..." : "Sign In"}
                  </button>
                </div>
              </form>
            </div>

            <div className="mt-5 text-center">
              <p className="mb-0">
                Don&apos;t have an account ?{" "}
                <Link href={AppRoutes.Auth.Register} className="fw-bold text-primary text-decoration-underline">
                  Signup
                </Link>
              </p>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
}

