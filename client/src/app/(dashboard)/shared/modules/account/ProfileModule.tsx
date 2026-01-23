"use client";

import Link from "next/link";
import { useEffect, useMemo, useRef, useState } from "react";

import BreadCrumb from "@/components/Common/BreadCrumb";
import { profileService } from "@/services/users/profileService";
import { productService } from "@/services/catalog/productService";
import { useAppSelector } from "@/store/hooks";
import type { ActivityItem, MyProfile, PaginatedResponse, Product, UserDocument } from "@/types";
import ProfileImage from "../components/Common/ProfileImage";

export default function ProfileModule({
  title = "Profile",
  pageTitle = "Pages",
  accountLabel,
  profileSettingsHref,
}: {
  title?: string;
  pageTitle?: string;
  accountLabel?: string;
  profileSettingsHref: string;
}) {
  const didFetchProfile = useRef(false);
  const didFetchRecommended = useRef(false);
  const didFetchDocuments = useRef(false);
  const lastActivitiesKey = useRef<string>("");

  const authUser = useAppSelector((s) => s.auth.user);
  const fallbackProfile = useMemo<MyProfile>(
    () => ({
      id: authUser?.id || "",
      userName: authUser?.userName || "",
      displayName: authUser?.displayName || authUser?.userName || "User",
      email: authUser?.email || "",
      phoneNumber: authUser?.phoneNumber || "",
      profileImageUrl: authUser?.profileImageUrl,
      roles: authUser?.role ? [authUser.role] : [],
      accountType: authUser?.role || "",
      accountStatus: authUser?.isLocked ? "Locked" : "Active",
    }),
    [authUser]
  );

  const [profile, setProfile] = useState<MyProfile>(fallbackProfile);
  const [isLoading, setIsLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [activeTab, setActiveTab] = useState<"overview" | "activities" | "documents">("overview");

  const [recommendedProducts, setRecommendedProducts] = useState<Product[]>([]);
  const [recommendedError, setRecommendedError] = useState<string | null>(null);

  const [documents, setDocuments] = useState<UserDocument[]>([]);
  const [documentsError, setDocumentsError] = useState<string | null>(null);
  const [isDocumentsLoading, setIsDocumentsLoading] = useState(false);
  const [documentType, setDocumentType] = useState<string>("");
  const [documentFile, setDocumentFile] = useState<File | null>(null);
  const [documentsSuccess, setDocumentsSuccess] = useState<string | null>(null);
  const [isUploadingDocument, setIsUploadingDocument] = useState(false);

  type ActivityCategory = "All" | "Orders" | "Profile" | "Documents" | "Security";
  const [activityCategory, setActivityCategory] = useState<ActivityCategory>("All");
  const [activitySearch, setActivitySearch] = useState<string>("");
  const [activitiesPage, setActivitiesPage] = useState<number>(1);
  const [activities, setActivities] = useState<PaginatedResponse<ActivityItem> | null>(null);
  const [isActivitiesLoading, setIsActivitiesLoading] = useState(false);
  const [activitiesError, setActivitiesError] = useState<string | null>(null);

  useEffect(() => {
    if (didFetchProfile.current) return;
    didFetchProfile.current = true;

    (async () => {
      try {
        const p = await profileService.getMyProfile();
        setProfile((prev) => ({ ...prev, ...p }));
      } catch (e) {
        setError(e instanceof Error ? e.message : "Failed to load profile");
      } finally {
        setIsLoading(false);
      }
    })();
  }, []);

  useEffect(() => {
    if (didFetchRecommended.current) return;
    didFetchRecommended.current = true;

    (async () => {
      try {
        const r = await productService.getProductPaginatedList(1, 4, undefined, 0, { isActive: true });
        setRecommendedProducts(r.data || []);
      } catch (e) {
        setRecommendedError(e instanceof Error ? e.message : "Failed to load recommended products");
      }
    })();
  }, []);

  useEffect(() => {
    if (activeTab !== "documents") return;
    if (didFetchDocuments.current) return;
    didFetchDocuments.current = true;

    (async () => {
      setDocumentsError(null);
      setDocumentsSuccess(null);
      setIsDocumentsLoading(true);
      try {
        const docs = await profileService.getMyDocuments();
        setDocuments(docs);
      } catch (e) {
        setDocumentsError(e instanceof Error ? e.message : "Failed to load documents");
      } finally {
        setIsDocumentsLoading(false);
      }
    })();
  }, [activeTab]);

  useEffect(() => {
    if (activeTab !== "activities") return;

    const key = JSON.stringify({
      page: activitiesPage,
      category: activityCategory,
      search: activitySearch.trim(),
    });
    if (lastActivitiesKey.current === key) return;
    lastActivitiesKey.current = key;

    (async () => {
      setActivitiesError(null);
      setIsActivitiesLoading(true);
      try {
        const result = await profileService.getMyActivitiesPaginatedList({
          pageNumber: activitiesPage,
          pageSize: 10,
          search: activitySearch.trim() || undefined,
          category: activityCategory === "All" ? undefined : activityCategory,
          sortBy: 1, // CreatedAtDesc
        });
        setActivities(result);
      } catch (e) {
        setActivitiesError(e instanceof Error ? e.message : "Failed to load activities");
      } finally {
        setIsActivitiesLoading(false);
      }
    })();
  }, [activeTab, activitiesPage, activityCategory, activitySearch]);

  const roleLabel = authUser?.role || profile.roles?.[0] || "";
  const accountType = profile.accountType || roleLabel || "-";
  const accountStatus = profile.accountStatus || "-";

  const stats = profile.ecommerceStats || {
    totalOrders: 0,
    completedOrders: 0,
    pendingOrders: 0,
    totalSpent: 0,
  };

  const recentActivities = profile.recentActivities || [];

  const formatDateTime = (value?: string) => {
    if (!value) return "-";
    const d = new Date(value);
    if (Number.isNaN(d.getTime())) return value;
    return d.toLocaleString();
  };

  const formatMoney = (value: number) => {
    try {
      return new Intl.NumberFormat(undefined, { style: "currency", currency: "EGP" }).format(value);
    } catch {
      return value.toFixed(2);
    }
  };

  const formatBytes = (bytes: number) => {
    if (!bytes && bytes !== 0) return "-";
    const units = ["B", "KB", "MB", "GB"];
    let size = bytes;
    let i = 0;
    while (size >= 1024 && i < units.length - 1) {
      size /= 1024;
      i++;
    }
    return `${size.toFixed(i === 0 ? 0 : 2)} ${units[i]}`;
  };

  const getProductThumb = (p: Product) => {
    const primary = p.productImages?.find((img) => img.isPrimary)?.imageURL;
    return primary || p.productImages?.[0]?.imageURL || "/assets/images/small/img-2.jpg";
  };

  const isPreviewable = (fileName: string) => {
    const lower = (fileName || "").toLowerCase();
    return lower.endsWith(".pdf") || lower.endsWith(".png") || lower.endsWith(".jpg") || lower.endsWith(".jpeg");
  };

  const onPickDocumentFile = async (file: File | null) => {
    if (!file) return;

    setDocumentsError(null);
    setDocumentsSuccess(null);

    const type = documentType.trim();
    if (!type) {
      setDocumentsError("Document type is required");
      return;
    }

    setIsUploadingDocument(true);
    try {
      const created = await profileService.uploadMyDocument({ type, file });
      setDocuments((prev) => [created, ...prev]);
      setDocumentType("");
      setDocumentsSuccess("Document uploaded successfully");
    } catch (e) {
      setDocumentsError(e instanceof Error ? e.message : "Failed to upload document");
    } finally {
      setIsUploadingDocument(false);
    }
  };

  const onDeleteDocument = async (id: string) => {
    if (!id) return;
    const ok = typeof window === "undefined" ? true : window.confirm("Delete this document?");
    if (!ok) return;

    setDocumentsError(null);
    setDocumentsSuccess(null);
    try {
      await profileService.deleteMyDocument(id);
      setDocuments((prev) => prev.filter((d) => d.id !== id));
      setDocumentsSuccess("Document deleted");
    } catch (e) {
      setDocumentsError(e instanceof Error ? e.message : "Failed to delete document");
    }
  };

  const onDownloadDocument = async (doc: UserDocument, mode: "download" | "view") => {
    setDocumentsError(null);
    try {
      const blob = await profileService.downloadMyDocument(doc.id);
      const url = URL.createObjectURL(blob);
      if (mode === "view" && isPreviewable(doc.fileName)) {
        window.open(url, "_blank", "noopener,noreferrer");
        return;
      }
      const a = document.createElement("a");
      a.href = url;
      a.download = doc.fileName || "document";
      document.body.appendChild(a);
      a.click();
      a.remove();
      URL.revokeObjectURL(url);
    } catch (e) {
      setDocumentsError(e instanceof Error ? e.message : "Failed to download document");
    }
  };

  const activityIcon = (category?: string) => {
    const c = (category || "").toLowerCase();
    if (c === "orders") return "ri-shopping-bag-line";
    if (c === "profile") return "ri-user-settings-line";
    if (c === "documents") return "ri-folder-line";
    if (c === "security") return "ri-shield-keyhole-line";
    return "ri-notification-3-line";
  };

  const breadcrumbItems = accountLabel
    ? [
        { label: pageTitle, icon: "ri-account-circle-line" },
        { label: accountLabel, icon: "ri-folder-line" },
        { label: title, icon: "ri-user-settings-line" },
      ]
    : [
        { label: pageTitle, icon: "ri-account-circle-line" },
        { label: title, icon: "ri-user-settings-line" },
      ];

  return (
    <div className="container-fluid">
      <BreadCrumb items={breadcrumbItems} />

      {error ? (
        <div className="alert alert-danger" role="alert">
          {error}
        </div>
      ) : null}

      <div className="profile-foreground position-relative mx-n4 mt-n4">
        <div className="profile-wid-bg">
          <img src="/assets/images/profile-bg.jpg" alt="" className="profile-wid-img" />
        </div>
      </div>

      <div className="pt-4 mb-4 mb-lg-3 pb-lg-4">
        <div className="row g-4">
          <div className="col-auto">
            <div className="avatar-lg">
              <ProfileImage
                profileImageUrl={profile.profileImageUrl}
                alt="user-img"
                className="img-thumbnail rounded-circle"
                showLoadingSpinner={isLoading}
                size={120}
              />
            </div>
          </div>

          <div className="col">
            <div className="p-2">
              <h3 className="text-white mb-1">{profile.displayName || "User"}</h3>
              {roleLabel ? <p className="text-white-75">{roleLabel}</p> : <p className="text-white-75">&nbsp;</p>}
              <div className="hstack text-white-50 gap-1">
                <div className="me-2">
                  <i className="ri-mail-line me-1 text-white-75 fs-16 align-middle"></i>
                  {profile.email || "-"}
                </div>
              </div>
            </div>
          </div>

          <div className="col-12 col-lg-auto order-last order-lg-0">
            <div className="row text text-white-50 text-center">
              <div className="col-lg-6 col-4">
                <div className="p-2">
                  <h4 className="text-white mb-1">{stats.totalOrders}</h4>
                  <p className="fs-15 mb-0">Total Orders</p>
                </div>
              </div>
              <div className="col-lg-6 col-4">
                <div className="p-2">
                  <h4 className="text-white mb-1">{formatMoney(stats.totalSpent)}</h4>
                  <p className="fs-15 mb-0">Total Spent</p>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>

      <div className="row">
        <div className="col-lg-12">
          <div>
            <div className="d-flex">
              <ul className="nav nav-pills animation-nav profile-nav gap-2 gap-lg-3 flex-grow-1" role="tablist">
                <li className="nav-item">
                  <button type="button" className={`nav-link fs-14 ${activeTab === "overview" ? "active" : ""}`} onClick={() => setActiveTab("overview")}>
                    <i className="ri-airplay-fill d-inline-block d-md-none"></i> <span className="d-none d-md-inline-block">Overview</span>
                  </button>
                </li>
                <li className="nav-item">
                  <button type="button" className={`nav-link fs-14 ${activeTab === "activities" ? "active" : ""}`} onClick={() => setActiveTab("activities")}>
                    <i className="ri-list-unordered d-inline-block d-md-none"></i> <span className="d-none d-md-inline-block">Activities</span>
                  </button>
                </li>
                <li className="nav-item">
                  <button type="button" className={`nav-link fs-14 ${activeTab === "documents" ? "active" : ""}`} onClick={() => setActiveTab("documents")}>
                    <i className="ri-folder-4-line d-inline-block d-md-none"></i> <span className="d-none d-md-inline-block">Documents</span>
                  </button>
                </li>
              </ul>
              <div className="flex-shrink-0">
                <Link href={profileSettingsHref} className="btn btn-success">
                  <i className="ri-edit-box-line align-bottom"></i> Edit Profile
                </Link>
              </div>
            </div>

            <div className="tab-content pt-4 text-muted">
              <div className={`tab-pane ${activeTab === "overview" ? "active show" : "fade"}`} id="overview-tab" role="tabpanel">
                <div className="row">
                  <div className="col-xxl-3">
                    <div className="card">
                      <div className="card-body">
                        <h5 className="card-title mb-3">Info</h5>
                        <div className="table-responsive">
                          <table className="table table-borderless mb-0">
                            <tbody>
                              <tr>
                                <th className="ps-0" scope="row">
                                  Full Name :
                                </th>
                                <td className="text-muted">{profile.displayName || "-"}</td>
                              </tr>
                              <tr>
                                <th className="ps-0" scope="row">
                                  Mobile :
                                </th>
                                <td className="text-muted">{profile.phoneNumber || "-"}</td>
                              </tr>
                              <tr>
                                <th className="ps-0" scope="row">
                                  E-mail :
                                </th>
                                <td className="text-muted">{profile.email || "-"}</td>
                              </tr>
                              <tr>
                                <th className="ps-0" scope="row">
                                  Account Type :
                                </th>
                                <td className="text-muted">{accountType}</td>
                              </tr>
                              <tr>
                                <th className="ps-0" scope="row">
                                  Account Status :
                                </th>
                                <td className="text-muted">{accountStatus}</td>
                              </tr>
                            </tbody>
                          </table>
                        </div>
                      </div>
                    </div>

                    <div className="card">
                      <div className="card-body">
                        <div className="d-flex align-items-center mb-4">
                          <div className="flex-grow-1">
                            <h5 className="card-title mb-0">Recommended Products</h5>
                          </div>
                        </div>

                        {recommendedError ? <div className="text-danger small">{recommendedError}</div> : null}

                        {recommendedProducts.length === 0 ? (
                          <div className="text-muted">No recommendations yet</div>
                        ) : (
                          <div>
                            {recommendedProducts.map((p) => (
                              <div key={p.id} className="d-flex align-items-center py-3">
                                <div className="avatar-xs flex-shrink-0 me-3">
                                  <img src={getProductThumb(p)} alt="" className="img-fluid rounded-circle" />
                                </div>
                                <div className="flex-grow-1">
                                  <div>
                                    <h5 className="fs-15 mb-1">{p.name}</h5>
                                    <p className="text-muted mb-0">{formatMoney(p.price)}</p>
                                  </div>
                                </div>
                              </div>
                            ))}
                          </div>
                        )}
                      </div>
                    </div>
                  </div>

                  <div className="col-xxl-9">
                    <div className="card">
                      <div className="card-body">
                        <h5 className="card-title mb-4">E-commerce Stats</h5>
                        <div className="row g-3">
                          <div className="col-md-3 col-6">
                            <div className="p-2 border rounded">
                              <div className="text-muted">Total Orders</div>
                              <div className="fs-16 fw-semibold">{stats.totalOrders}</div>
                            </div>
                          </div>
                          <div className="col-md-3 col-6">
                            <div className="p-2 border rounded">
                              <div className="text-muted">Completed</div>
                              <div className="fs-16 fw-semibold">{stats.completedOrders}</div>
                            </div>
                          </div>
                          <div className="col-md-3 col-6">
                            <div className="p-2 border rounded">
                              <div className="text-muted">Pending</div>
                              <div className="fs-16 fw-semibold">{stats.pendingOrders}</div>
                            </div>
                          </div>
                          <div className="col-md-3 col-6">
                            <div className="p-2 border rounded">
                              <div className="text-muted">Total Spent</div>
                              <div className="fs-16 fw-semibold">{formatMoney(stats.totalSpent)}</div>
                            </div>
                          </div>
                        </div>
                      </div>
                    </div>

                    <div className="card">
                      <div className="card-body">
                        <h5 className="card-title mb-3">Recent Activity</h5>

                        {isLoading ? <div className="text-muted">Loading...</div> : null}

                        {!isLoading && recentActivities.length === 0 ? (
                          <div className="text-muted">No recent activity yet</div>
                        ) : (
                          <div className="acitivity-timeline">
                            {recentActivities.map((a) => (
                              <div key={a.id} className="acitivity-item py-3 d-flex">
                                <div className="flex-shrink-0">
                                  <div className="avatar-xs acitivity-avatar">
                                    <div className="avatar-title rounded-circle bg-soft-info text-info">
                                      <i className={activityIcon(a.relatedEntity)}></i>
                                    </div>
                                  </div>
                                </div>
                                <div className="flex-grow-1 ms-3">
                                  <h6 className="mb-1">{a.actionType}</h6>
                                  {a.description ? <p className="text-muted mb-2">{a.description}</p> : null}
                                  <small className="mb-0 text-muted">{formatDateTime(a.createdAt)}</small>
                                </div>
                              </div>
                            ))}
                          </div>
                        )}
                      </div>
                    </div>
                  </div>
                </div>
              </div>

              <div className={`tab-pane ${activeTab === "activities" ? "active show" : "fade"}`} id="activities" role="tabpanel">
                <div className="card">
                  <div className="card-body">
                    <div className="d-flex align-items-center mb-3">
                      <h5 className="card-title flex-grow-1 mb-0">Activities</h5>
                    </div>

                    <div className="row g-2 align-items-end mb-3">
                      <div className="col-md-4">
                        <label className="form-label">Search</label>
                        <input
                          type="text"
                          className="form-control"
                          value={activitySearch}
                          onChange={(e) => {
                            setActivitiesPage(1);
                            setActivitySearch(e.target.value);
                          }}
                          placeholder="Search activities"
                        />
                      </div>
                      <div className="col-md-4">
                        <label className="form-label">Category</label>
                        <select
                          className="form-select"
                          value={activityCategory}
                          onChange={(e) => {
                            setActivitiesPage(1);
                            setActivityCategory(e.target.value as ActivityCategory);
                          }}
                        >
                          <option value="All">All</option>
                          <option value="Orders">Orders</option>
                          <option value="Profile">Profile</option>
                          <option value="Documents">Documents</option>
                          <option value="Security">Security</option>
                        </select>
                      </div>
                    </div>

                    {activitiesError ? <div className="alert alert-danger">{activitiesError}</div> : null}
                    {isActivitiesLoading ? <div className="text-muted">Loading...</div> : null}

                    {!isActivitiesLoading && activities && activities.data.length === 0 ? (
                      <div className="text-muted">No activities found</div>
                    ) : null}

                    {!isActivitiesLoading && activities && activities.data.length > 0 ? (
                      <>
                        <div className="acitivity-timeline">
                          {activities.data.map((a) => (
                            <div key={a.id} className="acitivity-item py-3 d-flex">
                              <div className="flex-shrink-0">
                                <div className="avatar-xs acitivity-avatar">
                                  <div className="avatar-title rounded-circle bg-soft-info text-info">
                                    <i className={activityIcon(a.category || a.relatedEntity)}></i>
                                  </div>
                                </div>
                              </div>
                              <div className="flex-grow-1 ms-3">
                                <h6 className="mb-1">{a.actionType}</h6>
                                {a.description ? <p className="text-muted mb-2">{a.description}</p> : null}
                                <small className="mb-0 text-muted">{formatDateTime(a.createdAt)}</small>
                              </div>
                            </div>
                          ))}
                        </div>

                        <div className="mt-4">
                          <ul className="pagination pagination-separated justify-content-center mb-0">
                            <li className={`page-item ${activities.hasPreviousPage ? "" : "disabled"}`}>
                              <button type="button" className="page-link" onClick={() => setActivitiesPage((p) => Math.max(1, p - 1))} disabled={!activities.hasPreviousPage}>
                                <i className="mdi mdi-chevron-left"></i>
                              </button>
                            </li>
                            <li className="page-item active">
                              <span className="page-link">{activities.currentPage}</span>
                            </li>
                            <li className={`page-item ${activities.hasNextPage ? "" : "disabled"}`}>
                              <button type="button" className="page-link" onClick={() => setActivitiesPage((p) => p + 1)} disabled={!activities.hasNextPage}>
                                <i className="mdi mdi-chevron-right"></i>
                              </button>
                            </li>
                          </ul>
                        </div>
                      </>
                    ) : null}
                  </div>
                </div>
              </div>

              <div className={`tab-pane ${activeTab === "documents" ? "active show" : "fade"}`} id="documents" role="tabpanel">
                <div className="card">
                  <div className="card-body">
                    <div className="d-flex align-items-center mb-4">
                      <h5 className="card-title flex-grow-1 mb-0">Documents</h5>
                      <div className="flex-shrink-0 d-flex align-items-center gap-2">
                        <input className="form-control" type="text" placeholder="Type (e.g. ID, Tax Card)" value={documentType} onChange={(e) => setDocumentType(e.target.value)} disabled={isUploadingDocument} />

                        <input
                          className="d-none"
                          type="file"
                          id="documentFileInput"
                          accept="*/*"
                          onChange={async (e) => {
                            const file = e.target.files?.[0] || null;
                            if (file) {
                              await onPickDocumentFile(file);
                            }
                            e.target.value = "";
                          }}
                          disabled={isUploadingDocument}
                        />
                        <label htmlFor="documentFileInput" className="btn btn-success mb-0" style={{ cursor: isUploadingDocument ? "not-allowed" : "pointer" }}>
                          <i className="ri-upload-2-fill me-1 align-bottom"></i>
                          {isUploadingDocument ? "Uploading..." : "Upload Document"}
                        </label>
                      </div>
                    </div>

                    {documentsError ? <div className="alert alert-danger">{documentsError}</div> : null}
                    {documentsSuccess ? <div className="alert alert-success">{documentsSuccess}</div> : null}
                    {isDocumentsLoading ? <div className="text-muted">Loading...</div> : null}

                    <div className="row">
                      <div className="col-lg-12">
                        <div className="table-responsive">
                          <table className="table table-borderless align-middle mb-0">
                            <thead className="table-light">
                              <tr>
                                <th scope="col">File Name</th>
                                <th scope="col">Type</th>
                                <th scope="col">Size</th>
                                <th scope="col">Upload Date</th>
                                <th scope="col">Action</th>
                              </tr>
                            </thead>
                            <tbody>
                              {documents.length === 0 ? (
                                <tr>
                                  <td colSpan={5} className="text-center text-muted py-4">
                                    No documents uploaded yet
                                  </td>
                                </tr>
                              ) : (
                                documents.map((d) => (
                                  <tr key={d.id}>
                                    <td>
                                      <div className="d-flex align-items-center">
                                        <div className="avatar-sm">
                                          <div className="avatar-title bg-soft-primary text-primary rounded fs-20">
                                            <i className="ri-file-2-line"></i>
                                          </div>
                                        </div>
                                        <div className="ms-3 flex-grow-1">
                                          <h6 className="fs-16 mb-0">
                                            <a href="javascript:void(0)">{d.fileName}</a>
                                          </h6>
                                        </div>
                                      </div>
                                    </td>
                                    <td>
                                      <div className="d-flex flex-column gap-1">
                                        <span>{d.type}</span>
                                        <span className="badge bg-soft-warning text-warning text-uppercase">{String(d.status)}</span>
                                      </div>
                                    </td>
                                    <td>{formatBytes(d.size)}</td>
                                    <td>{formatDateTime(d.createdAt)}</td>
                                    <td>
                                      <div className="dropdown">
                                        <a href="javascript:void(0);" className="btn btn-light btn-icon" data-bs-toggle="dropdown" aria-expanded="true">
                                          <i className="ri-equalizer-fill"></i>
                                        </a>
                                        <ul className="dropdown-menu dropdown-menu-end">
                                          <li>
                                            <button type="button" className="dropdown-item" onClick={() => onDownloadDocument(d, "view")}>
                                              <i className="ri-eye-fill me-2 align-middle text-muted"></i>View
                                            </button>
                                          </li>
                                          <li>
                                            <button type="button" className="dropdown-item" onClick={() => onDownloadDocument(d, "download")}>
                                              <i className="ri-download-2-fill me-2 align-middle text-muted"></i>Download
                                            </button>
                                          </li>
                                          <li className="dropdown-divider"></li>
                                          <li>
                                            <button type="button" className="dropdown-item" onClick={() => onDeleteDocument(d.id)}>
                                              <i className="ri-delete-bin-5-line me-2 align-middle text-muted"></i>Delete
                                            </button>
                                          </li>
                                        </ul>
                                      </div>
                                    </td>
                                  </tr>
                                ))
                              )}
                            </tbody>
                          </table>
                        </div>
                      </div>
                    </div>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
}

