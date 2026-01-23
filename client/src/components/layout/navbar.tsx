import Link from "next/link";

import { AppRoutes } from "@/constants/routes";

export default function Navbar() {
  return (
    <nav className="navbar navbar-expand-lg navbar-dark bg-primary">
      <div className="container">
        <Link className="navbar-brand fw-semibold" href={AppRoutes.Home}>
          Tajerly
        </Link>

        <button
          className="navbar-toggler"
          type="button"
          data-bs-toggle="collapse"
          data-bs-target="#storefrontNavbar"
          aria-controls="storefrontNavbar"
          aria-expanded="false"
          aria-label="Toggle navigation"
        >
          <span className="navbar-toggler-icon"></span>
        </button>

        <div className="collapse navbar-collapse" id="storefrontNavbar">
          <ul className="navbar-nav me-auto mb-2 mb-lg-0">
            <li className="nav-item">
              <Link className="nav-link active" href={AppRoutes.Home}>
                Home
              </Link>
            </li>
            <li className="nav-item">
              <Link className="nav-link" href={AppRoutes.Dashboard.Home}>
                Dashboard
              </Link>
            </li>
          </ul>

          <div className="d-flex gap-2">
            <Link className="btn btn-outline-light" href={AppRoutes.Auth.Login}>
              Login
            </Link>
            <Link className="btn btn-light text-primary" href={AppRoutes.Auth.Register}>
              Register
            </Link>
          </div>
        </div>
      </div>
    </nav>
  );
}

