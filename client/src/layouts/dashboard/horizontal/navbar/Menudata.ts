import { IconPoint, IconAperture, IconBoxMultiple, IconApps, IconShoppingCart, IconUserCircle } from "@tabler/icons-react";
import { uniqueId } from "lodash";

interface MenuitemsType {
  [x: string]: any;
  id?: string;
  navlabel?: boolean;
  subheader?: string;
  title?: string;
  icon?: any;
  href?: string;
  children?: MenuitemsType[];
  chip?: string;
  chipColor?: string;
  variant?: string;
  external?: boolean;
}

const Menuitems: MenuitemsType[] = [
  {
    navlabel: true,
    subheader: "Admin",
  },
  {
    id: uniqueId(),
    title: "Dashboard",
    icon: IconAperture,
    href: "/admin",
  },
  {
    navlabel: true,
    subheader: "Modules",
  },
  {
    id: uniqueId(),
    title: "Users Management",
    icon: IconUserCircle,
    href: "/users",
    children: [
      {
        id: uniqueId(),
        title: "Users",
        icon: IconUserCircle,
        href: "/admin/users",
      },
      {
        id: uniqueId(),
        title: "Roles",
        icon: IconUserCircle,
        href: "/admin/roles",
      },
      {
        id: uniqueId(),
        title: "Permissions",
        icon: IconUserCircle,
        href: "/admin/permissions",
      }
    ]
  },
];
export default Menuitems;
