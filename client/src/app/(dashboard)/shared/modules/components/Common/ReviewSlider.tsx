export default function ReviewSlider() {
  return (
    <div className="swiper-container">
      <div className="swiper-wrapper">
        <div className="swiper-slide">
          <div className="card border card-border-success">
            <div className="card-body">
              <div className="d-flex align-items-center">
                <div className="flex-shrink-0">
                  <img
                    src="/assets/images/users/avatar-1.jpg"
                    alt=""
                    className="avatar-sm rounded-circle"
                  />
                </div>
                <div className="flex-grow-1 ms-3">
                  <h6 className="fs-14 mb-1">Henry</h6>
                  <p className="text-muted mb-0">12 Jan, 2022</p>
                </div>
                <div className="flex-shrink-0">
                  <span className="badge badge-soft-success">
                    <i className="ri-star-fill align-bottom me-1"></i> 4.0
                  </span>
                </div>
              </div>
              <div className="mt-3 pt-1">
                <p className="text-muted">
                  &quot;Awesome product! I really enjoyed using this theme. The interface
                  is clean and the features are amazing.&quot;
                </p>
              </div>
            </div>
          </div>
        </div>

        <div className="swiper-slide">
          <div className="card border card-border-success">
            <div className="card-body">
              <div className="d-flex align-items-center">
                <div className="flex-shrink-0">
                  <img
                    src="/assets/images/users/avatar-2.jpg"
                    alt=""
                    className="avatar-sm rounded-circle"
                  />
                </div>
                <div className="flex-grow-1 ms-3">
                  <h6 className="fs-14 mb-1">Angela Bernier</h6>
                  <p className="text-muted mb-0">24 Nov, 2022</p>
                </div>
                <div className="flex-shrink-0">
                  <span className="badge badge-soft-success">
                    <i className="ri-star-fill align-bottom me-1"></i> 4.5
                  </span>
                </div>
              </div>
              <div className="mt-3 pt-1">
                <p className="text-muted">
                  &quot;Great support team! They helped me resolve all my issues very
                  quickly. Highly recommended!&quot;
                </p>
              </div>
            </div>
          </div>
        </div>

        <div className="swiper-slide">
          <div className="card border card-border-success">
            <div className="card-body">
              <div className="d-flex align-items-center">
                <div className="flex-shrink-0">
                  <img
                    src="/assets/images/users/avatar-3.jpg"
                    alt=""
                    className="avatar-sm rounded-circle"
                  />
                </div>
                <div className="flex-grow-1 ms-3">
                  <h6 className="fs-14 mb-1">David Grasso</h6>
                  <p className="text-muted mb-0">15 Dec, 2022</p>
                </div>
                <div className="flex-shrink-0">
                  <span className="badge badge-soft-success">
                    <i className="ri-star-fill align-bottom me-1"></i> 5.0
                  </span>
                </div>
              </div>
              <div className="mt-3 pt-1">
                <p className="text-muted">
                  &quot;Best dashboard template I have ever used. Clean code and easy to
                  customize. Worth every penny!&quot;
                </p>
              </div>
            </div>
          </div>
        </div>
      </div>
      <div className="swiper-pagination swiper-pagination-dark"></div>
    </div>
  );
}
