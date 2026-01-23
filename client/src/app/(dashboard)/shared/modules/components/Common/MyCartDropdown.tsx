"use client";

import { useState } from "react";

const cartItems = [
  {
    id: 1,
    name: "Branded T-Shirts",
    image: "/assets/images/products/img-1.png",
    quantity: 10,
    price: 32,
    total: 320,
  },
  {
    id: 2,
    name: "Bentwood Chair",
    image: "/assets/images/products/img-2.png",
    quantity: 5,
    price: 18,
    total: 89,
  },
  {
    id: 3,
    name: "Borosil Paper Cup",
    image: "/assets/images/products/img-3.png",
    quantity: 3,
    price: 250,
    total: 750,
  },
  {
    id: 4,
    name: "Gray Styled T-Shirt",
    image: "/assets/images/products/img-6.png",
    quantity: 1,
    price: 1250,
    total: 1250,
  },
  {
    id: 5,
    name: "Stillbird Helmet",
    image: "/assets/images/products/img-5.png",
    quantity: 2,
    price: 495,
    total: 990,
  },
];

export default function MyCartDropdown() {
  const [isOpen, setIsOpen] = useState(false);
  const [items, setItems] = useState(cartItems);

  const handleRemoveItem = (id: number) => {
    setItems(items.filter((item) => item.id !== id));
  };

  const totalAmount = items.reduce((acc, item) => acc + item.total, 0);

  return (
    <div className={`dropdown topbar-head-dropdown ms-1 header-item ${isOpen ? "show" : ""}`}>
      <button
        type="button"
        className="btn btn-icon btn-topbar btn-ghost-secondary rounded-circle"
        id="page-header-cart-dropdown"
        onClick={() => setIsOpen(!isOpen)}
        data-bs-toggle="dropdown"
        aria-haspopup="true"
        aria-expanded={isOpen}
      >
        <i className="bx bx-shopping-bag fs-22"></i>
        <span className="position-absolute topbar-badge cartitem-badge fs-10 translate-middle badge rounded-pill bg-info">
          {items.length}
        </span>
      </button>
      <div
        className={`dropdown-menu dropdown-menu-xl dropdown-menu-end p-0 dropdown-menu-cart ${isOpen ? "show" : ""}`}
        aria-labelledby="page-header-cart-dropdown"
      >
        <div className="p-3 border-top-0 border-start-0 border-end-0 border-dashed border">
          <div className="row align-items-center">
            <div className="col">
              <h6 className="m-0 fs-16 fw-semibold">My Cart</h6>
            </div>
            <div className="col-auto">
              <span className="badge bg-warning-subtle text-warning fs-13">
                <span className="cartitem-badge">{items.length}</span> items
              </span>
            </div>
          </div>
        </div>
        <div style={{ maxHeight: "300px", overflowY: "auto" }}>
          <div className="p-2">
            {items.length === 0 ? (
              <div className="text-center empty-cart" id="empty-cart">
                <div className="avatar-md mx-auto my-3">
                  <div className="avatar-title bg-info-subtle text-info fs-36 rounded-circle">
                    <i className="bx bx-cart"></i>
                  </div>
                </div>
                <h5 className="mb-3">Your Cart is Empty!</h5>
                <a href="#" className="btn btn-success w-md mb-3">
                  Shop Now
                </a>
              </div>
            ) : (
              items.map((item) => (
                <div
                  key={item.id}
                  className="d-block dropdown-item dropdown-item-cart text-wrap px-3 py-2"
                >
                  <div className="d-flex align-items-center">
                    <img
                      src={item.image}
                      className="me-3 rounded-circle avatar-sm p-2 bg-light flex-shrink-0"
                      alt={item.name}
                    />
                    <div className="flex-grow-1">
                      <h6 className="mt-0 mb-1 fs-14">
                        <a href="#" className="text-reset">
                          {item.name}
                        </a>
                      </h6>
                      <p className="mb-0 fs-12 text-muted">
                        Quantity: <span>{item.quantity} x ${item.price}</span>
                      </p>
                    </div>
                    <div className="px-2">
                      <h5 className="m-0 fw-normal">
                        $<span className="cart-item-price">{item.total}</span>
                      </h5>
                    </div>
                    <div className="ps-2">
                      <button
                        type="button"
                        className="btn btn-icon btn-sm btn-ghost-secondary remove-item-btn"
                        onClick={() => handleRemoveItem(item.id)}
                      >
                        <i className="ri-close-fill fs-16"></i>
                      </button>
                    </div>
                  </div>
                </div>
              ))
            )}
          </div>
        </div>
        {items.length > 0 && (
          <div
            className="p-3 border-bottom-0 border-start-0 border-end-0 border-dashed border"
            id="checkout-elem"
          >
            <div className="d-flex justify-content-between align-items-center pb-3">
              <h5 className="m-0 text-muted">Total:</h5>
              <div className="px-2">
                <h5 className="m-0" id="cart-item-total">
                  ${totalAmount.toFixed(2)}
                </h5>
              </div>
            </div>

            <a href="#" className="btn btn-success text-center w-100">
              Checkout
            </a>
          </div>
        )}
      </div>
    </div>
  );
}
