"use client";

import ProfileModule from "@/app/(dashboard)/shared/modules/account/ProfileModule";

export default function AdminProfilePage() {
  return <ProfileModule title="Profile" pageTitle="Admin" accountLabel="Account" profileSettingsHref="/admin/modules/account/profile-settings" />;
}

