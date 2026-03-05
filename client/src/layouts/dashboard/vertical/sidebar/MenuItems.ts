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
import {
    IconAperture,
    IconList,
    IconUserCircle,
    IconShieldLock,
} from "@tabler/icons-react";

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
        id: uniqueId(),
        title: "Order Table",
        icon: IconList,
        href: "/order-table",
    },
    {
        navlabel: true,
        subheader: "Modules",
    },
    {
        id: uniqueId(),
        title: "Users Management",
        icon: IconUserCircle,
        children: [
            {
                id: uniqueId(),
                title: "Users",
                icon: IconUserCircle,
                href: "/admin/users",
            },
            {
                id: uniqueId(),
                title: "Roles & Permissions",
                icon: IconShieldLock,
                href: "/admin/roles-permissions",
            },
        ],
    },
];

export default Menuitems;
