"use client";

import { useState, useEffect } from "react";
import { applicationUserService } from "@/app/(dashboard)/shared/modules/api/users/applicationUserService";
import { customerService } from "@/app/(dashboard)/shared/modules/api/users/customerService";
import type { CreateAdminRequest, CreateVendorRequest, CreateCustomerRequest, Gender } from "@/types";
import { useToast } from "@/app/(dashboard)/shared/modules/hooks/useToast";
import type { UserTab } from "../types";

interface CreateUserModalProps {
  show: boolean;
  activeTab: UserTab;
  onClose: () => void;
  onSuccess: () => void;
}

export default function CreateUserModal({ show, activeTab, onClose, onSuccess }: CreateUserModalProps) {
  const toast = useToast();
  const [loading, setLoading] = useState(false);
  const [formData, setFormData] = useState<CreateAdminRequest | CreateVendorRequest | CreateCustomerRequest>({
    firstName: "",
    lastName: "",
    userName: "",
    email: "",
    gender: undefined,
    phoneNumber: "",
    secondPhoneNumber: "",
    password: "",
    confirmPassword: "",
    ...(activeTab === "merchants" && { storeName: "", commissionRate: 0 }),
    ...(activeTab === "admins" && { address: "" }),
  });
  const [profileImage, setProfileImage] = useState<File | null>(null);
  const [previewImage, setPreviewImage] = useState<string | null>(null);

  useEffect(() => {
    if (!show) {
      // Reset form when modal closes
      setFormData({
        firstName: "",
        lastName: "",
        userName: "",
        email: "",
        gender: undefined,
        phoneNumber: "",
        secondPhoneNumber: "",
        password: "",
        confirmPassword: "",
        ...(activeTab === "merchants" && { storeName: "", commissionRate: 0 }),
        ...(activeTab === "admins" && { address: "" }),
      });
      setProfileImage(null);
      setPreviewImage(null);
    }
  }, [show, activeTab]);

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
    setLoading(true);

    try {
      const payload = {
        ...formData,
        profileImage: profileImage || undefined,
      };

      let result: string;
      if (activeTab === "admins") {
        result = await applicationUserService.createAdmin(payload as CreateAdminRequest);
      } else if (activeTab === "merchants") {
        result = await applicationUserService.createVendor(payload as CreateVendorRequest);
      } else {
        result = await customerService.createCustomer(payload as CreateCustomerRequest);
      }

      toast.success(result || `${activeTab === "admins" ? "Admin" : activeTab === "merchants" ? "Merchant" : "Customer"} created successfully`);
      onSuccess();
      onClose();
    } catch (error) {
      toast.error(error instanceof Error ? error.message : "Failed to create user");
    } finally {
      setLoading(false);
    }
  };

  if (!show) return null;

  const title = activeTab === "admins" ? "Create Admin" : activeTab === "merchants" ? "Create Merchant" : "Create Customer";

  return (
    <>
      <div className={`modal fade ${show ? "show" : ""}`} tabIndex={-1} aria-hidden={!show} style={{ display: show ? "block" : "none" }}>
        <div className="modal-dialog modal-dialog-centered modal-lg">
          <div className="modal-content">
            <div className="modal-header">
              <h5 className="modal-title">{title}</h5>
              <button type="button" className="btn-close" aria-label="Close" onClick={onClose} />
            </div>
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
                          value={(formData as CreateVendorRequest).storeName || ""}
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
                          value={(formData as CreateVendorRequest).commissionRate || 0}
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
                        value={(formData as CreateAdminRequest).address || ""}
                        onChange={handleInputChange}
                      />
                    </div>
                  )}
                  <div className="col-md-6">
                    <label className="form-label">
                      Password <span className="text-danger">*</span>
                    </label>
                    <input
                      type="password"
                      className="form-control"
                      name="password"
                      value={formData.password || ""}
                      onChange={handleInputChange}
                      required
                    />
                  </div>
                  <div className="col-md-6">
                    <label className="form-label">
                      Confirm Password <span className="text-danger">*</span>
                    </label>
                    <input
                      type="password"
                      className="form-control"
                      name="confirmPassword"
                      value={formData.confirmPassword || ""}
                      onChange={handleInputChange}
                      required
                    />
                  </div>
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
                  {loading ? "Creating..." : "Create"}
                </button>
              </div>
            </form>
          </div>
        </div>
      </div>
      {show ? <div className="modal-backdrop fade show" onClick={onClose} /> : null}
    </>
  );
}
