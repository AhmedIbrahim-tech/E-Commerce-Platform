import Link from "next/link";

import Navbar from "@/components/layout/navbar";
import { AppRoutes } from "@/constants/routes";

export default function HomePage() {
  return (
    <div className="min-vh-100 d-flex flex-column">
      <Navbar />

      <main className="flex-grow-1">
        <section className="py-5 bg-light">
          <div className="container">
            <div className="row align-items-center g-4">
              <div className="col-lg-7">
                <h1 className="display-6 fw-semibold mb-3">Welcome to Tajerly</h1>
                <p className="text-muted fs-5 mb-4">
                  A modern ecommerce starter: browse products, manage orders, and track everything from one place.
                </p>

                <div className="d-flex flex-wrap gap-2">
                  <Link className="btn btn-primary" href={AppRoutes.Auth.Register}>
                    Create account
                  </Link>
                  <Link className="btn btn-outline-primary" href={AppRoutes.Auth.Login}>
                    Sign in
                  </Link>
                  <Link className="btn btn-link" href={AppRoutes.Dashboard.Home}>
                    Go to dashboard
                  </Link>
                </div>
              </div>

              <div className="col-lg-5">
                <div className="card shadow-sm border-0">
                  <div className="card-body p-4">
                    <h5 className="mb-3">Getting started</h5>
                    <ul className="mb-0">
                      <li>Register a new account</li>
                      <li>Login to access protected pages</li>
                      <li>Open the dashboard to manage the store</li>
                    </ul>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </section>

        <section className="py-5">
          <div className="container">
            <div className="row g-4">
              <div className="col-md-4">
                <div className="card h-100">
                  <div className="card-body">
                    <h5 className="card-title">Products</h5>
                    <p className="card-text text-muted mb-0">
                      Add and manage products, pricing, images, and stock.
                    </p>
                  </div>
                </div>
              </div>
              <div className="col-md-4">
                <div className="card h-100">
                  <div className="card-body">
                    <h5 className="card-title">Orders</h5>
                    <p className="card-text text-muted mb-0">
                      Track orders, payments, shipping, and customer activity.
                    </p>
                  </div>
                </div>
              </div>
              <div className="col-md-4">
                <div className="card h-100">
                  <div className="card-body">
                    <h5 className="card-title">Customers</h5>
                    <p className="card-text text-muted mb-0">
                      Manage customer profiles, addresses, and order history.
                    </p>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </section>
      </main>

      <footer className="py-4 border-top bg-white">
        <div className="container">
          <div className="d-flex flex-wrap justify-content-between align-items-center gap-2">
            <span className="text-muted">© {new Date().getFullYear()} Tajerly</span>
            <span className="text-muted">Ecommerce starter</span>
          </div>
        </div>
      </footer>
    </div>
  );
}

