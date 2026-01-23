"use client";

import ProfileModule from "@/app/(dashboard)/shared/modules/account/ProfileModule";

export default function MerchantProfilePage() {
  return <ProfileModule title="Profile" pageTitle="Merchant" profileSettingsHref="/merchant/modules/account/profile-settings" />;
}

