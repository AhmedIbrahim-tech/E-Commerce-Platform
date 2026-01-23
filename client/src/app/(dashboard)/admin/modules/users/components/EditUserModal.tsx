"use client";

import { useState, useEffect } from "react";
import { adminService } from "@/app/(dashboard)/shared/modules/api/users/adminService";
import { vendorService } from "@/app/(dashboard)/shared/modules/api/users/vendorService";
import { customerService } from "@/app/(dashboard)/shared/modules/api/users/customerService";
import type { UpdateAdminRequest, UpdateVendorRequest, UpdateCustomerRequest, Gender, Admin, Vendor, Customer } from "@/types";
import { useToast } from "@/app/(dashboard)/shared/modules/hooks/useToast";
import type { UserTab, UserItem } from "../types";

interface EditUserModalProps {
  show: boolean;
  user: UserItem | null;
  activeTab: UserTab;
  onClose: () => void;
  onSuccess: () => void;
}

export default function EditUserModal({ show, user, activeTab, onClose, onSuccess }: EditUserModalProps) {
  const toast = useToast();
  const [loading, setLoading] = useState(false);
  const [fetching, setFetching] = useState(false);
  const [formData, setFormData] = useState<UpdateAdminRequest | UpdateVendorRequest | UpdateCustomerRequest>({
    id: "",
    firstName: "",
    lastName: "",
    userName: "",
    email: "",
    gender: undefined,
    phoneNumber: "",
    secondPhoneNumber: "",
    ...(activeTab === "merchants" && { storeName: "", commissionRate: 0 }),
    ...(activeTab === "admins" && { address: "" }),
  });
  const [profileImage, setProfileImage] = useState<File | null>(null);
  const [previewImage, setPreviewImage] = useState<string | null>(null);

  useEffect(() => {
    if (show && user) {
      setFetching(true);
      // Load full user data
      const loadUser = async () => {
        try {
          let fullUser: Admin | Vendor | Customer;
          if (activeTab === "admins") {
            fullUser = await adminService.getAdminById(user.id);
          } else if (activeTab === "merchants") {
            fullUser = await vendorService.getVendorById(user.id);
          } else {
            fullUser = await customerService.getCustomerById(user.id);
          }

          // Parse fullName to firstName and lastName
          const nameParts = fullUser.fullName?.split(" ") || [];
          const firstName = nameParts[0] || "";
          const lastName = nameParts.slice(1).join(" ") || "";

          setFormData({
            id: fullUser.id,
            firstName,
            lastName,
            userName: fullUser.userName || "",
            email: fullUser.email || "",
            gender: fullUser.gender,
            phoneNumber: fullUser.phoneNumber || "",
            secondPhoneNumber: fullUser.secondPhoneNumber || "",
            ...(activeTab === "merchants" && {
              storeName: (fullUser as Vendor).storeName || "",
              commissionRate: (fullUser as Vendor).commissionRate || 0,
            }),
            ...(activeTab === "admins" && {
              address: (fullUser as Admin).address || "",
            }),
          });

          if (fullUser.profileImage) {
            setPreviewImage(fullUser.profileImage);
          }
        } catch (error) {
          toast.error(error instanceof Error ? error.message : "Failed to load user data");
          onClose();
        } finally {
          setFetching(false);
        }
      };
      loadUser();
    }
  }, [show, user, activeTab, toast, onClose]);

  const handleInputChange = (e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>) => {
    const { name, value } = e.target;
    setFormData((prev) => ({
      ...prev,
      [name]: name === "commissionRate" ? parseFloat(value) || 0 : value,
    }));
  };

  const handleImageChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const file = e.target.files?.[0];
    if (file) {
      setProfileImage(file);
      const reader = new FileReader();
      reader.onloadend = () => {
        setPreviewImage(reader.result as string);
      };
      reader.readAsDataURL(file);
    }
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!user) return;

    setLoading(true);

    try {
      const payload = {
        ...formData,
        profileImage: profileImage || undefined,
      };

      let result: string;
      if (activeTab === "admins") {
        result = await adminService.updateAdmin(payload as UpdateAdminRequest);
      } else if (activeTab === "merchants") {
        result = await vendorService.updateVendor(payload as UpdateVendorRequest);
      } else {
        result = await customerService.updateCustomer(payload as UpdateCustomerRequest);
      }

      toast.success(result || "User updated successfully");
      onSuccess();
      onClose();
    } catch (error) {
      toast.error(error instanceof Error ? error.message : "Failed to update user");
    } finally {
      setLoading(false);
    }
  };

  if (!show || !user) return null;

  const title = activeTab === "admins" ? "Edit Admin" : activeTab === "merchants" ? "Edit Merchant" : "Edit Customer";

  return (
    <>
      <div className={`modal fade ${show ? "show" : ""}`} tabIndex={-1} aria-hidden={!show} style={{ display: show ? "block" : "none" }}>
        <div className="modal-dialog modal-dialog-centered modal-lg">
          <div className="modal-content">
            <div className="modal-header">
              <h5 className="modal-title">{title}</h5>
              <button type="button" className="btn-close" aria-label="Close" onClick={onClose} />
            </div>
            {fetching ? (
              <div className="modal-body text-center py-5">
                <div className="spinner-border text-primary" role="status">
                  <span className="visually-hidden">Loading...</span>
                </div>
              </div>
            ) : (
              <form onSubmit={handleSubmit}>
                <div className="modal-body">
                  <div className="row g-3">
                    <div className="col-md-6">
                      <label className="form-label">
                        First Name <span className="text-danger">*</span>
                      </label>
                      <input
                        type="text"
                        className="form-control"
                        name="firstName"
                        value={formData.firstName || ""}
                        onChange={handleInputChange}
                        required
                      />
                    </div>
                    <div className="col-md-6">
                      <label className="form-label">
                        Last Name <span className="text-danger">*</span>
                      </label>
                      <input
                        type="text"
                        className="form-control"
                        name="lastName"
                        value={formData.lastName || ""}
                        onChange={handleInputChange}
                        required
                      />
                    </div>
                    <div className="col-md-6">
                      <label className="form-label">
                        Username <span className="text-danger">*</span>
                      </label>
                      <input
                        type="text"
                        className="form-control"
                        name="userName"
                        value={formData.userName || ""}
                        onChange={handleInputChange}
                        required
                      />
                    </div>
                    <div className="col-md-6">
                      <label className="form-label">
                        Email <span className="text-danger">*</span>
                      </label>
                      <input
                        type="email"
                        className="form-control"
                        name="email"
                        value={formData.email || ""}
                        onChange={handleInputChange}
                        required
                      />
                    </div>
                    <div className="col-md-6">
                      <label className="form-label">Gender</label>
                      <select className="form-select" name="gender" value={formData.gender || ""} onChange={handleInputChange}>
                        <option value="">Select Gender</option>
                        <option value="Male">Male</option>
                        <option value="Female">Female</option>
                      </select>
                    </div>
                    <div className="col-md-6">
                      <label className="form-label">Phone Number</label>
                      <input
                        type="tel"
                        className="form-control"
                        name="phoneNumber"
                        value={formData.phoneNumber || ""}
                        onChange={handleInputChange}
                      />
                    </div>
                    {activeTab === "merchants" && (
                      <>
                        <div className="col-md-6">
                          <label className="form-label">
                            Store Name <span className="text-danger">*</span>
                          </label>
                          <input
                            type="text"
                            className="form-control"
                            name="storeName"
                            value={(formData as UpdateVendorRequest).storeName || ""}
                            onChange={handleInputChange}
                            required
                          />
                        </div>
                        <div className="col-md-6">
                          <label className="form-label">
                            Commission Rate <span className="text-danger">*</span>
                          </label>
                          <input
                            type="number"
                            step="0.01"
                            min="0"
                            max="100"
                            className="form-control"
                            name="commissionRate"
                            value={(formData as UpdateVendorRequest).commissionRate || 0}
                            onChange={handleInputChange}
                            required
                          />
                        </div>
                      </>
                    )}
                    {activeTab === "admins" && (
                      <div className="col-md-12">
                        <label className="form-label">Address</label>
                        <input
                          type="text"
                          className="form-control"
                          name="address"
                          value={(formData as UpdateAdminRequest).address || ""}
                          onChange={handleInputChange}
                        />
                      </div>
                    )}
                    <div className="col-md-12">
                      <label className="form-label">Profile Image</label>
                      <input type="file" className="form-control" accept="image/*" onChange={handleImageChange} />
                      {previewImage && (
                        <div className="mt-2">
                          <img src={previewImage} alt="Preview" className="img-thumbnail" style={{ maxWidth: "150px", maxHeight: "150px" }} />
                        </div>
                      )}
                    </div>
                  </div>
                </div>
                <div className="modal-footer">
                  <button type="button" className="btn btn-light" onClick={onClose} disabled={loading}>
                    Cancel
                  </button>
                  <button type="submit" className="btn btn-primary" disabled={loading}>
                    {loading ? "Updating..." : "Update"}
                  </button>
                </div>
              </form>
            )}
          </div>
        </div>
      </div>
      {show ? <div className="modal-backdrop fade show" onClick={onClose} /> : null}
    </>
  );
}
