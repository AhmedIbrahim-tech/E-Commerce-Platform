"use client";

import Link from "next/link";
import { useRouter } from "next/navigation";
import { useEffect, useMemo, useState } from "react";

import { AppRoutes } from "@/constants/routes";
import { useAppDispatch, useAppSelector } from "@/store/hooks";
import { registerAsync } from "@/store/slices/authSlice";
import { UserRole } from "@/types";

export default function RegisterPage() {
  const router = useRouter();
  const dispatch = useAppDispatch();

  const { isLoading, isAuthenticated, error, user } = useAppSelector((s) => s.auth);

  const [firstName, setFirstName] = useState("");
  const [lastName, setLastName] = useState("");
  const [userName, setUserName] = useState("");
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [confirmPassword, setConfirmPassword] = useState("");
  const [showPassword, setShowPassword] = useState(false);

  const passwordChecks = useMemo(() => {
    return {
      length: password.length >= 8,
      lower: /[a-z]/.test(password),
      upper: /[A-Z]/.test(password),
      number: /\d/.test(password),
    };
  }, [password]);

  const passwordsMatch = useMemo(
    () => confirmPassword.trim() !== "" && password === confirmPassword,
    [password, confirmPassword]
  );

  const canSubmit = useMemo(() => {
    return (
      firstName.trim() !== "" &&
      lastName.trim() !== "" &&
      userName.trim() !== "" &&
      email.trim() !== "" &&
      password.trim() !== "" &&
      confirmPassword.trim() !== "" &&
      Object.values(passwordChecks).every(Boolean) &&
      passwordsMatch
    );
  }, [
    confirmPassword,
    email,
    firstName,
    lastName,
    password,
    passwordChecks,
    passwordsMatch,
    userName,
  ]);

  useEffect(() => {
    /**
     * What caused the redirect loop:
     * - After registration completes (it logs in to obtain tokens), this page redirected to `/` unconditionally.
     * - That prevented Admin/Merchant accounts from reaching their dashboards after successful auth.
     *
     * What changed:
     * - Redirect authenticated users to the correct destination based on role.
     * - Guests can still access `/register` without being redirected away.
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
    await dispatch(
      registerAsync({
        firstName: firstName.trim(),
        lastName: lastName.trim(),
        userName: userName.trim(),
        email: email.trim(),
        password,
        confirmPassword,
      })
    );
  };

  return (
    <div className="card overflow-hidden m-0">
      <div className="row justify-content-center g-0">
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
              <h5 className="text-primary">Register Account</h5>
              <p className="text-muted">Create your account now.</p>
            </div>

            <div className="mt-4">
              {error ? (
                <div className="alert alert-danger" role="alert">
                  {error}
                </div>
              ) : null}

              <form className="needs-validation" noValidate onSubmit={onSubmit}>
                <div className="row">
                  <div className="col-md-6">
                    <div className="mb-3">
                      <label htmlFor="firstName" className="form-label">
                        First name <span className="text-danger">*</span>
                      </label>
                      <input
                        type="text"
                        className="form-control"
                        id="firstName"
                        placeholder="Enter first name"
                        value={firstName}
                        onChange={(e) => setFirstName(e.target.value)}
                        disabled={isLoading}
                        required
                      />
                    </div>
                  </div>
                  <div className="col-md-6">
                    <div className="mb-3">
                      <label htmlFor="lastName" className="form-label">
                        Last name <span className="text-danger">*</span>
                      </label>
                      <input
                        type="text"
                        className="form-control"
                        id="lastName"
                        placeholder="Enter last name"
                        value={lastName}
                        onChange={(e) => setLastName(e.target.value)}
                        disabled={isLoading}
                        required
                      />
                    </div>
                  </div>
                </div>

                <div className="mb-3">
                  <label htmlFor="useremail" className="form-label">
                    Email <span className="text-danger">*</span>
                  </label>
                  <input
                    type="email"
                    className="form-control"
                    id="useremail"
                    placeholder="Enter email address"
                    value={email}
                    onChange={(e) => setEmail(e.target.value)}
                    autoComplete="email"
                    disabled={isLoading}
                    required
                  />
                </div>

                <div className="mb-3">
                  <label htmlFor="username" className="form-label">
                    Username <span className="text-danger">*</span>
                  </label>
                  <input
                    type="text"
                    className="form-control"
                    id="username"
                    placeholder="Enter username"
                    value={userName}
                    onChange={(e) => setUserName(e.target.value)}
                    autoComplete="username"
                    disabled={isLoading}
                    required
                  />
                </div>

                <div className="mb-3">
                  <label className="form-label" htmlFor="password-input">
                    Password <span className="text-danger">*</span>
                  </label>
                  <div className="position-relative auth-pass-inputgroup">
                    <input
                      type={showPassword ? "text" : "password"}
                      className="form-control pe-5 password-input"
                      placeholder="Enter password"
                      id="password-input"
                      value={password}
                      onChange={(e) => setPassword(e.target.value)}
                      autoComplete="new-password"
                      disabled={isLoading}
                      required
                    />
                    <button
                      className="btn btn-link position-absolute end-0 top-0 text-decoration-none text-muted password-addon"
                      type="button"
                      onClick={() => setShowPassword((v) => !v)}
                      aria-label={showPassword ? "Hide password" : "Show password"}
                    >
                      <i className={showPassword ? "ri-eye-off-fill align-middle" : "ri-eye-fill align-middle"}></i>
                    </button>
                  </div>
                </div>

                <div className="mb-3">
                  <label className="form-label" htmlFor="confirm-password-input">
                    Confirm password <span className="text-danger">*</span>
                  </label>
                  <input
                    type={showPassword ? "text" : "password"}
                    className={`form-control ${confirmPassword && !passwordsMatch ? "is-invalid" : ""}`}
                    id="confirm-password-input"
                    placeholder="Confirm password"
                    value={confirmPassword}
                    onChange={(e) => setConfirmPassword(e.target.value)}
                    autoComplete="new-password"
                    disabled={isLoading}
                    required
                  />
                  {confirmPassword && !passwordsMatch ? (
                    <div className="invalid-feedback">Passwords do not match.</div>
                  ) : null}
                </div>

                <div className="mb-4">
                  <p className="mb-0 fs-13 text-muted fst-italic">
                    By registering you agree to the Velzon{" "}
                    <a href="#" className="text-primary text-decoration-underline fst-normal fw-semibold">
                      Terms of Use
                    </a>
                  </p>
                </div>

                <div id="password-contain" className="p-3 bg-light mb-2 rounded">
                  <h5 className="fs-13">Password must contain:</h5>
                  <p id="pass-length" className={`${passwordChecks.length ? "valid" : "invalid"} fs-13 mb-2`}>
                    Minimum <b>8 characters</b>
                  </p>
                  <p id="pass-lower" className={`${passwordChecks.lower ? "valid" : "invalid"} fs-13 mb-2`}>
                    At least <b>lowercase</b> letter (a-z)
                  </p>
                  <p id="pass-upper" className={`${passwordChecks.upper ? "valid" : "invalid"} fs-13 mb-2`}>
                    At least <b>uppercase</b> letter (A-Z)
                  </p>
                  <p id="pass-number" className={`${passwordChecks.number ? "valid" : "invalid"} fs-13 mb-0`}>
                    At least <b>number</b> (0-9)
                  </p>
                </div>

                <div className="mt-4">
                  <button className="btn btn-success w-100" type="submit" disabled={!canSubmit || isLoading}>
                    {isLoading ? "Signing Up..." : "Sign Up"}
                  </button>
                </div>
              </form>
            </div>

            <div className="mt-5 text-center">
              <p className="mb-0">
                Already have an account ?{" "}
                <Link href={AppRoutes.Auth.Login} className="fw-bold text-primary text-decoration-underline">
                  Signin
                </Link>
              </p>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
}

