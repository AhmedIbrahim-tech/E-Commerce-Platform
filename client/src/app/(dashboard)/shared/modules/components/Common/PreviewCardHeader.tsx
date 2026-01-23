interface PreviewCardHeaderProps {
  title: string;
}

export default function PreviewCardHeader({ title }: PreviewCardHeaderProps) {
  return (
    <div className="card-header align-items-center d-flex">
      <h4 className="card-title mb-0 flex-grow-1">{title}</h4>
      <div className="flex-shrink-0">
        <ul className="nav justify-content-end nav-tabs-custom rounded card-header-tabs border-bottom-0">
          <li className="nav-item">
            <a className="nav-link active" href="#preview-tab" role="tab">
              Preview
            </a>
          </li>
          <li className="nav-item">
            <a className="nav-link" href="#code-tab" role="tab">
              <i className="ri-code-s-slash-line"></i>
            </a>
          </li>
        </ul>
      </div>
    </div>
  );
}
