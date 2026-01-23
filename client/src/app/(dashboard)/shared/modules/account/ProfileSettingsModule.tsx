"use client";

import { useEffect, useMemo, useRef, useState } from "react";

import BreadCrumb from "@/components/Common/BreadCrumb";
import ToastContainer from "@/components/Common/ToastContainer";
import { useToast } from "@/app/(dashboard)/shared/modules/hooks/useToast";
import { profileService } from "@/services/users/profileService";
import { shippingAddressService } from "@/services/shipping/shippingAddressService";
import { useAppDispatch, useAppSelector } from "@/store/hooks";
import { setUser } from "@/store/slices/authSlice";
import apiClient from "@/services/core/apiClient";
import { Routes } from "@/constants/apiroutes";
import type { MyProfile, ShippingAddress } from "@/types";
import ProfileImage from "../components/Common/ProfileImage";

type TabId = "personalDetails" | "changePassword" | "addresses" | "privacy";

export default function ProfileSettingsModule({
  title = "Profile Settings",
  pageTitle = "Pages",
}: {
  title?: string;
  pageTitle?: string;
}) {
  const didFetch = useRef(false);
  const dispatch = useAppDispatch();
  const authUser = useAppSelector((s) => s.auth.user);

  const fallbackProfile = useMemo<MyProfile>(
    () => ({
      id: authUser?.id || "",
      userName: authUser?.userName || "",
      displayName: authUser?.displayName || authUser?.userName || "User",
      email: authUser?.email || "",
      phoneNumber: authUser?.phoneNumber || "",
      profileImageUrl: authUser?.profileImageUrl,
    }),
    [authUser]
  );

  const [activeTab, setActiveTab] = useState<TabId>("personalDetails");
  const [profile, setProfile] = useState<MyProfile>(fallbackProfile);
  const [isLoading, setIsLoading] = useState(true);
  const toast = useToast();

  const initialName = useMemo(() => {
    const parts = (profile.displayName || "").trim().split(/\s+/).filter(Boolean);
    if (!parts.length) return { firstName: "", lastName: "" };
    return { firstName: parts[0], lastName: parts.slice(1).join(" ") };
  }, [profile.displayName]);

  const [firstName, setFirstName] = useState("");
  const [lastName, setLastName] = useState("");
  const [phoneNumber, setPhoneNumber] = useState("");
  const [profileImage, setProfileImage] = useState<File | null>(null);

  const [currentPassword, setCurrentPassword] = useState("");
  const [newPassword, setNewPassword] = useState("");
  const [confirmPassword, setConfirmPassword] = useState("");
  const [isSavingProfile, setIsSavingProfile] = useState(false);
  const [isChangingPassword, setIsChangingPassword] = useState(false);

  // Addresses state
  const [addresses, setAddresses] = useState<ShippingAddress[]>([]);
  const [isAddressesLoading, setIsAddressesLoading] = useState(false);
  const [isEditingAddress, setIsEditingAddress] = useState(false);
  const [editingAddressId, setEditingAddressId] = useState<string | null>(null);
  const [isSavingAddress, setIsSavingAddress] = useState(false);
  const [addressForm, setAddressForm] = useState({
    firstName: "",
    lastName: "",
    street: "",
    city: "",
    state: "",
  });

  useEffect(() => {
    setFirstName(initialName.firstName);
    setLastName(initialName.lastName);
    setPhoneNumber(profile.phoneNumber || "");
  }, [initialName.firstName, initialName.lastName, profile.phoneNumber]);

  const didFetchAddresses = useRef(false);

  useEffect(() => {
    if (didFetch.current) return;
    didFetch.current = true;

    (async () => {
      try {
        const p = await profileService.getMyProfile();
        setProfile((prev) => ({ ...prev, ...p }));
      } catch (e) {
        toast.error(e instanceof Error ? e.message : "Failed to load profile");
      } finally {
        setIsLoading(false);
      }
    })();
  }, []);

  useEffect(() => {
    if (activeTab !== "addresses") {
      didFetchAddresses.current = false;
      return;
    }
    if (didFetchAddresses.current) return;
    didFetchAddresses.current = true;

    (async () => {
      setIsAddressesLoading(true);
      try {
        const list = await shippingAddressService.getShippingAddressList();
        setAddresses(list);
      } catch (e) {
        toast.error(e instanceof Error ? e.message : "Failed to load addresses");
      } finally {
        setIsAddressesLoading(false);
      }
    })();
  }, [activeTab]);

  const [previewImageUrl, setPreviewImageUrl] = useState<string | null>(null);

  useEffect(() => {
    if (profileImage) {
      const url = URL.createObjectURL(profileImage);
      setPreviewImageUrl(url);
      return () => {
        URL.revokeObjectURL(url);
      };
    } else {
      setPreviewImageUrl(null);
    }
  }, [profileImage]);

  const onSaveProfile = async (e: React.FormEvent) => {
    e.preventDefault();
    if (isSavingProfile) return;

    const displayName = `${firstName} ${lastName}`.trim().replace(/\s+/g, " ");
    if (!displayName) {
      toast.error("Display name is required");
      return;
    }

    setIsSavingProfile(true);
    try {
      const updated = await profileService.updateMyProfile({
        displayName,
        phoneNumber: phoneNumber.trim() || undefined,
        profileImage: profileImage || undefined,
      });

      setProfile((prev) => ({ ...prev, ...updated }));
      setProfileImage(null);

      // Update global user state IMMEDIATELY so navbar updates instantly
      // This ensures navbar image updates without page refresh
      if (authUser) {
        dispatch(
          setUser({
            ...authUser,
            displayName: updated.displayName || displayName,
            phoneNumber: updated.phoneNumber || phoneNumber,
            // Use updated image URL (can be null if user removed image)
            profileImageUrl: updated.profileImageUrl ?? authUser.profileImageUrl,
          })
        );
      }

      toast.success("Profile updated successfully");
    } catch (e) {
      toast.error(e instanceof Error ? e.message : "Failed to update profile");
    } finally {
      setIsSavingProfile(false);
    }
  };

  const onChangePassword = async (e: React.FormEvent) => {
    e.preventDefault();
    if (isChangingPassword) return;

    if (!authUser?.id) {
      toast.error("Missing user id");
      return;
    }

    if (!currentPassword || !newPassword || !confirmPassword) {
      toast.error("All password fields are required");
      return;
    }

    if (newPassword !== confirmPassword) {
      toast.error("Passwords do not match");
      return;
    }

    setIsChangingPassword(true);
    try {
      await apiClient.put(Routes.User.ChangePassword, {
        id: authUser.id,
        currentPassword,
        newPassword,
        confirmPassword,
      });
      toast.success("Password changed successfully");
      setCurrentPassword("");
      setNewPassword("");
      setConfirmPassword("");
    } catch (e) {
      toast.error(e instanceof Error ? e.message : "Failed to change password");
    } finally {
      setIsChangingPassword(false);
    }
  };

  const onStartAddAddress = () => {
    setIsEditingAddress(true);
    setEditingAddressId(null);
    setAddressForm({
      firstName: "",
      lastName: "",
      street: "",
      city: "",
      state: "",
    });
  };

  const onStartEditAddress = (addr: ShippingAddress) => {
    setIsEditingAddress(true);
    setEditingAddressId(addr.id);
    setAddressForm({
      firstName: addr.firstName || "",
      lastName: addr.lastName || "",
      street: addr.street || "",
      city: addr.city || "",
      state: addr.state || "",
    });
  };

  const onCancelAddress = () => {
    setIsEditingAddress(false);
    setEditingAddressId(null);
    setAddressForm({
      firstName: "",
      lastName: "",
      street: "",
      city: "",
      state: "",
    });
  };

  const onSaveAddress = async (e: React.FormEvent) => {
    e.preventDefault();
    if (isSavingAddress) return;

    if (!addressForm.firstName || !addressForm.lastName || !addressForm.street || !addressForm.city || !addressForm.state) {
      toast.error("All fields are required");
      return;
    }

    setIsSavingAddress(true);
    try {
      if (editingAddressId) {
        await shippingAddressService.updateShippingAddress({
          id: editingAddressId,
          firstName: addressForm.firstName,
          lastName: addressForm.lastName,
          street: addressForm.street,
          city: addressForm.city,
          state: addressForm.state,
        });
        toast.success("Address updated successfully");
      } else {
        await shippingAddressService.createShippingAddress({
          firstName: addressForm.firstName,
          lastName: addressForm.lastName,
          street: addressForm.street,
          city: addressForm.city,
          state: addressForm.state,
        });
        toast.success("Address added successfully");
      }

      // Refresh addresses list
      const list = await shippingAddressService.getShippingAddressList();
      setAddresses(list);
      onCancelAddress();
    } catch (e) {
      toast.error(e instanceof Error ? e.message : "Failed to save address");
    } finally {
      setIsSavingAddress(false);
    }
  };

  const onDeleteAddress = async (id: string) => {
    if (!confirm("Are you sure you want to delete this address?")) return;

    try {
      await shippingAddressService.deleteShippingAddress(id);
      toast.success("Address deleted successfully");
      const list = await shippingAddressService.getShippingAddressList();
      setAddresses(list);
    } catch (e) {
      toast.error(e instanceof Error ? e.message : "Failed to delete address");
    }
  };

  return (
    <div className="container-fluid">
      <BreadCrumb
        items={[
          { label: pageTitle, icon: "ri-account-circle-line" },
          { label: title, icon: "ri-settings-3-line" },
        ]}
      />
      <ToastContainer toasts={toast.toasts} onRemove={toast.removeToast} />

      <div className="position-relative mx-n4 mt-n4">
        <div className="profile-wid-bg profile-setting-img">
          <img src="/assets/images/profile-bg.jpg" className="profile-wid-img" alt="" />
          <div className="overlay-content">
            <div className="text-end p-3">
              <div className="p-0 ms-auto rounded-circle profile-photo-edit">
                <input id="profile-foreground-img-file-input" type="file" className="profile-foreground-img-file-input" />
                <label htmlFor="profile-foreground-img-file-input" className="profile-photo-edit btn btn-light">
                  <i className="ri-image-edit-line align-bottom me-1"></i> Change Cover
                </label>
              </div>
            </div>
          </div>
        </div>
      </div>

      <div className="row">
        <div className="col-xxl-3">
          <div className="card mt-n5">
            <div className="card-body p-4">
              <div className="text-center">
                <div className="profile-user position-relative d-inline-block mx-auto mb-4">
                  <ProfileImage
                    profileImageUrl={profile.profileImageUrl}
                    previewImageUrl={previewImageUrl}
                    alt="user-profile-image"
                    className="rounded-circle avatar-xl img-thumbnail user-profile-image"
                    showLoadingSpinner={isLoading}
                    size={120}
                  />
                  <div className="avatar-xs p-0 rounded-circle profile-photo-edit">
                    <input
                      id="profile-img-file-input"
                      type="file"
                      className="profile-img-file-input"
                      accept="image/*"
                      onChange={(e) => setProfileImage(e.target.files?.[0] || null)}
                    />
                    <label htmlFor="profile-img-file-input" className="profile-photo-edit avatar-xs">
                      <span className="avatar-title rounded-circle bg-light text-body">
                        <i className="ri-camera-fill"></i>
                      </span>
                    </label>
                  </div>
                </div>
                <h5 className="mb-1">{profile.displayName || "User"}</h5>
                <p className="text-muted mb-0">{authUser?.role || ""}</p>
              </div>
            </div>
          </div>

          <div className="card">
            <div className="card-body">
              <div className="d-flex align-items-center mb-5">
                <div className="flex-grow-1">
                  <h5 className="card-title mb-0">Complete Your Profile</h5>
                </div>
                <div className="flex-shrink-0">
                  <span className="badge bg-light text-primary fs-12">
                    <i className="ri-edit-box-line align-bottom me-1"></i> Edit
                  </span>
                </div>
              </div>
              <div className="progress animated-progress custom-progress progress-label">
                <div className="progress-bar bg-danger" role="progressbar" style={{ width: "30%" }}>
                  <div className="label">30%</div>
                </div>
              </div>
            </div>
          </div>

        </div>

        <div className="col-xxl-9">
          <div className="card mt-xxl-n5">
            <div className="card-header">
              <ul className="nav nav-tabs-custom rounded card-header-tabs border-bottom-0" role="tablist">
                <li className="nav-item">
                  <button type="button" className={`nav-link ${activeTab === "personalDetails" ? "active" : ""}`} onClick={() => setActiveTab("personalDetails")}>
                    <i className="fas fa-home"></i> Personal Details
                  </button>
                </li>
                <li className="nav-item">
                  <button type="button" className={`nav-link ${activeTab === "changePassword" ? "active" : ""}`} onClick={() => setActiveTab("changePassword")}>
                    <i className="far fa-user"></i> Change Password
                  </button>
                </li>
                <li className="nav-item">
                  <button type="button" className={`nav-link ${activeTab === "addresses" ? "active" : ""}`} onClick={() => setActiveTab("addresses")}>
                    <i className="ri-map-pin-line"></i> Addresses
                  </button>
                </li>
                <li className="nav-item">
                  <button type="button" className={`nav-link ${activeTab === "privacy" ? "active" : ""}`} onClick={() => setActiveTab("privacy")}>
                    <i className="ri-shield-keyhole-line"></i> Privacy Policy
                  </button>
                </li>
              </ul>
            </div>

            <div className="card-body p-4">
              {isLoading ? <div className="text-muted">Loading...</div> : null}

              {activeTab === "personalDetails" ? (
                <form onSubmit={onSaveProfile}>
                  <div className="row">
                    <div className="col-lg-6">
                      <div className="mb-3">
                        <label htmlFor="firstnameInput" className="form-label">
                          First Name
                        </label>
                        <input type="text" className="form-control" id="firstnameInput" placeholder="Enter your firstname" value={firstName} onChange={(e) => setFirstName(e.target.value)} disabled={isSavingProfile} />
                      </div>
                    </div>

                    <div className="col-lg-6">
                      <div className="mb-3">
                        <label htmlFor="lastnameInput" className="form-label">
                          Last Name
                        </label>
                        <input type="text" className="form-control" id="lastnameInput" placeholder="Enter your lastname" value={lastName} onChange={(e) => setLastName(e.target.value)} disabled={isSavingProfile} />
                      </div>
                    </div>

                    <div className="col-lg-6">
                      <div className="mb-3">
                        <label htmlFor="phonenumberInput" className="form-label">
                          Phone Number
                        </label>
                        <input type="text" className="form-control" id="phonenumberInput" placeholder="Enter your phone number" value={phoneNumber} onChange={(e) => setPhoneNumber(e.target.value)} disabled={isSavingProfile} />
                      </div>
                    </div>

                    <div className="col-lg-6">
                      <div className="mb-3">
                        <label htmlFor="emailInput" className="form-label">
                          Email Address
                        </label>
                        <input type="email" className="form-control" id="emailInput" placeholder="Enter your email" value={profile.email || ""} disabled />
                      </div>
                    </div>

                    <div className="col-lg-12">
                      <div className="hstack gap-2 justify-content-end">
                        <button type="submit" className="btn btn-primary" disabled={isSavingProfile}>
                          {isSavingProfile ? "Updating..." : "Updates"}
                        </button>
                        <button
                          type="button"
                          className="btn btn-soft-success"
                          disabled={isSavingProfile}
                          onClick={() => {
                            setFirstName(initialName.firstName);
                            setLastName(initialName.lastName);
                            setPhoneNumber(profile.phoneNumber || "");
                            setProfileImage(null);
                          }}
                        >
                          Cancel
                        </button>
                      </div>
                    </div>
                  </div>
                </form>
              ) : null}

              {activeTab === "changePassword" ? (
                <form onSubmit={onChangePassword}>
                    <div className="row g-2">
                      <div className="col-lg-4">
                        <div>
                          <label htmlFor="oldpasswordInput" className="form-label">
                            Old Password*
                          </label>
                          <input type="password" className="form-control" id="oldpasswordInput" placeholder="Enter current password" value={currentPassword} onChange={(e) => setCurrentPassword(e.target.value)} disabled={isChangingPassword} />
                        </div>
                      </div>

                      <div className="col-lg-4">
                        <div>
                          <label htmlFor="newpasswordInput" className="form-label">
                            New Password*
                          </label>
                          <input type="password" className="form-control" id="newpasswordInput" placeholder="Enter new password" value={newPassword} onChange={(e) => setNewPassword(e.target.value)} disabled={isChangingPassword} />
                        </div>
                      </div>

                      <div className="col-lg-4">
                        <div>
                          <label htmlFor="confirmpasswordInput" className="form-label">
                            Confirm Password*
                          </label>
                          <input type="password" className="form-control" id="confirmpasswordInput" placeholder="Confirm password" value={confirmPassword} onChange={(e) => setConfirmPassword(e.target.value)} disabled={isChangingPassword} />
                        </div>
                      </div>

                      <div className="col-lg-12">
                        <div className="text-end">
                          <button type="submit" className="btn btn-success" disabled={isChangingPassword}>
                            {isChangingPassword ? "Changing..." : "Change Password"}
                          </button>
                        </div>
                      </div>
                    </div>
                  </form>
              ) : null}

              {activeTab === "addresses" ? (
                <>
                  <div className="d-flex justify-content-between align-items-center mb-3">
                    <h5 className="mb-0">My Addresses</h5>
                    {!isEditingAddress ? (
                      <button type="button" className="btn btn-primary" onClick={onStartAddAddress}>
                        <i className="ri-add-line me-1"></i> Add Address
                      </button>
                    ) : null}
                  </div>

                  {isAddressesLoading ? <div className="text-muted">Loading...</div> : null}

                  {isEditingAddress || (!isEditingAddress && addresses.length === 0 && !isAddressesLoading) ? (
                    <form onSubmit={onSaveAddress} className="mb-4">
                      <div className="row g-2">
                        <div className="col-md-6">
                          <label className="form-label">First Name*</label>
                          <input
                            type="text"
                            className="form-control"
                            value={addressForm.firstName}
                            onChange={(e) => setAddressForm((prev) => ({ ...prev, firstName: e.target.value }))}
                            disabled={isSavingAddress}
                            required
                          />
                        </div>
                        <div className="col-md-6">
                          <label className="form-label">Last Name*</label>
                          <input
                            type="text"
                            className="form-control"
                            value={addressForm.lastName}
                            onChange={(e) => setAddressForm((prev) => ({ ...prev, lastName: e.target.value }))}
                            disabled={isSavingAddress}
                            required
                          />
                        </div>
                        <div className="col-md-12">
                          <label className="form-label">Street*</label>
                          <input
                            type="text"
                            className="form-control"
                            value={addressForm.street}
                            onChange={(e) => setAddressForm((prev) => ({ ...prev, street: e.target.value }))}
                            disabled={isSavingAddress}
                            required
                          />
                        </div>
                        <div className="col-md-6">
                          <label className="form-label">City*</label>
                          <input
                            type="text"
                            className="form-control"
                            value={addressForm.city}
                            onChange={(e) => setAddressForm((prev) => ({ ...prev, city: e.target.value }))}
                            disabled={isSavingAddress}
                            required
                          />
                        </div>
                        <div className="col-md-6">
                          <label className="form-label">State*</label>
                          <input
                            type="text"
                            className="form-control"
                            value={addressForm.state}
                            onChange={(e) => setAddressForm((prev) => ({ ...prev, state: e.target.value }))}
                            disabled={isSavingAddress}
                            required
                          />
                        </div>
                        <div className="col-md-12">
                          <div className="hstack gap-2">
                            <button type="submit" className="btn btn-success" disabled={isSavingAddress}>
                              {isSavingAddress ? "Saving..." : isEditingAddress ? "Update Address" : "Add Address"}
                            </button>
                            {isEditingAddress ? (
                              <button type="button" className="btn btn-secondary" onClick={onCancelAddress} disabled={isSavingAddress}>
                                Cancel
                              </button>
                            ) : null}
                          </div>
                        </div>
                      </div>
                    </form>
                  ) : null}

                  {!isEditingAddress && addresses.length > 0 ? (
                    <div className="row g-3">
                      {addresses.map((addr) => (
                        <div key={addr.id} className="col-md-6">
                          <div className="card border">
                            <div className="card-body">
                              <div className="d-flex justify-content-between align-items-start mb-2">
                                <h6 className="mb-0">{addr.firstName || addr.lastName ? `${addr.firstName} ${addr.lastName}`.trim() : "Address"}</h6>
                                <div className="dropdown">
                                  <a href="javascript:void(0);" className="btn btn-sm btn-light" data-bs-toggle="dropdown">
                                    <i className="ri-more-2-fill"></i>
                                  </a>
                                  <ul className="dropdown-menu dropdown-menu-end">
                                    <li>
                                      <button type="button" className="dropdown-item" onClick={() => onStartEditAddress(addr)}>
                                        <i className="ri-edit-line me-2"></i>Edit
                                      </button>
                                    </li>
                                    <li>
                                      <button type="button" className="dropdown-item text-danger" onClick={() => onDeleteAddress(addr.id)}>
                                        <i className="ri-delete-bin-line me-2"></i>Delete
                                      </button>
                                    </li>
                                  </ul>
                                </div>
                              </div>
                              <p className="text-muted mb-0 small">
                                {addr.street}
                                <br />
                                {addr.city}, {addr.state}
                              </p>
                            </div>
                          </div>
                        </div>
                      ))}
                    </div>
                  ) : null}
                </>
              ) : null}
              {activeTab === "privacy" ? (
                <div className="text-muted">
                  <p>Privacy Policy will be available soon</p>
                </div>
              ) : null}
            </div>
          </div>
        </div>
      </div>
    </div>
  );
}

