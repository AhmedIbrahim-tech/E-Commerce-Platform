interface ProductsGlobalFilterProps {
  searchValue?: string;
  setSearchValue?: (value: string) => void;
}

export function ProductsGlobalFilter({
  searchValue = "",
  setSearchValue,
}: ProductsGlobalFilterProps) {
  return (
    <div className="search-box ms-2">
      <input
        type="text"
        className="form-control search"
        placeholder="Search..."
        value={searchValue}
        onChange={(e) => setSearchValue?.(e.target.value)}
      />
      <i className="ri-search-line search-icon"></i>
    </div>
  );
}

interface OrdersGlobalFilterProps {
  searchValue?: string;
  setSearchValue?: (value: string) => void;
}

export function OrdersGlobalFilter({
  searchValue = "",
  setSearchValue,
}: OrdersGlobalFilterProps) {
  return (
    <div className="col-sm">
      <div className="d-flex justify-content-sm-end">
        <div className="search-box ms-2">
          <input
            type="text"
            className="form-control search"
            placeholder="Search orders..."
            value={searchValue}
            onChange={(e) => setSearchValue?.(e.target.value)}
          />
          <i className="ri-search-line search-icon"></i>
        </div>
      </div>
    </div>
  );
}

interface ContactsGlobalFilterProps {
  searchValue?: string;
  setSearchValue?: (value: string) => void;
}

export function ContactsGlobalFilter({
  searchValue = "",
  setSearchValue,
}: ContactsGlobalFilterProps) {
  return (
    <div className="search-box">
      <input
        type="text"
        className="form-control search"
        placeholder="Search contacts..."
        value={searchValue}
        onChange={(e) => setSearchValue?.(e.target.value)}
      />
      <i className="ri-search-line search-icon"></i>
    </div>
  );
}

interface CompaniesGlobalFilterProps {
  searchValue?: string;
  setSearchValue?: (value: string) => void;
}

export function CompaniesGlobalFilter({
  searchValue = "",
  setSearchValue,
}: CompaniesGlobalFilterProps) {
  return (
    <div className="search-box">
      <input
        type="text"
        className="form-control search"
        placeholder="Search companies..."
        value={searchValue}
        onChange={(e) => setSearchValue?.(e.target.value)}
      />
      <i className="ri-search-line search-icon"></i>
    </div>
  );
}

interface LeadsGlobalFilterProps {
  searchValue?: string;
  setSearchValue?: (value: string) => void;
}

export function LeadsGlobalFilter({
  searchValue = "",
  setSearchValue,
}: LeadsGlobalFilterProps) {
  return (
    <div className="search-box">
      <input
        type="text"
        className="form-control search"
        placeholder="Search leads..."
        value={searchValue}
        onChange={(e) => setSearchValue?.(e.target.value)}
      />
      <i className="ri-search-line search-icon"></i>
    </div>
  );
}

interface CryptoOrdersGlobalFilterProps {
  searchValue?: string;
  setSearchValue?: (value: string) => void;
}

export function CryptoOrdersGlobalFilter({
  searchValue = "",
  setSearchValue,
}: CryptoOrdersGlobalFilterProps) {
  return (
    <div className="search-box">
      <input
        type="text"
        className="form-control search"
        placeholder="Search orders..."
        value={searchValue}
        onChange={(e) => setSearchValue?.(e.target.value)}
      />
      <i className="ri-search-line search-icon"></i>
    </div>
  );
}

interface InvoiceListGlobalFilterProps {
  searchValue?: string;
  setSearchValue?: (value: string) => void;
}

export function InvoiceListGlobalFilter({
  searchValue = "",
  setSearchValue,
}: InvoiceListGlobalFilterProps) {
  return (
    <div className="search-box">
      <input
        type="text"
        className="form-control search"
        placeholder="Search invoices..."
        value={searchValue}
        onChange={(e) => setSearchValue?.(e.target.value)}
      />
      <i className="ri-search-line search-icon"></i>
    </div>
  );
}

interface TicketsListGlobalFilterProps {
  searchValue?: string;
  setSearchValue?: (value: string) => void;
}

export function TicketsListGlobalFilter({
  searchValue = "",
  setSearchValue,
}: TicketsListGlobalFilterProps) {
  return (
    <div className="search-box">
      <input
        type="text"
        className="form-control search"
        placeholder="Search tickets..."
        value={searchValue}
        onChange={(e) => setSearchValue?.(e.target.value)}
      />
      <i className="ri-search-line search-icon"></i>
    </div>
  );
}

interface NFTRankingGlobalFilterProps {
  searchValue?: string;
  setSearchValue?: (value: string) => void;
}

export function NFTRankingGlobalFilter({
  searchValue = "",
  setSearchValue,
}: NFTRankingGlobalFilterProps) {
  return (
    <div className="search-box">
      <input
        type="text"
        className="form-control search"
        placeholder="Search ranking..."
        value={searchValue}
        onChange={(e) => setSearchValue?.(e.target.value)}
      />
      <i className="ri-search-line search-icon"></i>
    </div>
  );
}

interface TaskListGlobalFilterProps {
  searchValue?: string;
  setSearchValue?: (value: string) => void;
}

export function TaskListGlobalFilter({
  searchValue = "",
  setSearchValue,
}: TaskListGlobalFilterProps) {
  return (
    <div className="search-box">
      <input
        type="text"
        className="form-control search"
        placeholder="Search tasks..."
        value={searchValue}
        onChange={(e) => setSearchValue?.(e.target.value)}
      />
      <i className="ri-search-line search-icon"></i>
    </div>
  );
}
