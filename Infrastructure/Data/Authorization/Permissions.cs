namespace Infrastructure.Data.Authorization;

public static class Permissions
{
    public static class Customer
    {
        public const string ViewProfile = "Customer.ViewProfile";
        public const string EditProfile = "Customer.EditProfile";
        public const string ViewList = "Customer.ViewList";
        public const string Delete = "Customer.Delete";
    }

    public static class Vendor
    {
        public const string ViewProfile = "Vendor.ViewProfile";
        public const string EditProfile = "Vendor.EditProfile";
        public const string ViewDashboard = "Vendor.ViewDashboard";
        public const string ViewList = "Vendor.ViewList";
        public const string Create = "Vendor.Create";
        public const string Delete = "Vendor.Delete";
    }

    public static class Admin
    {
        public const string ViewProfile = "Admin.ViewProfile";
        public const string EditProfile = "Admin.EditProfile";
        public const string ViewList = "Admin.ViewList";
        public const string Create = "Admin.Create";
        public const string Delete = "Admin.Delete";
        public const string ManageUsers = "Admin.ManageUsers";
        public const string ManageRoles = "Admin.ManageRoles";
        public const string ManageClaims = "Admin.ManageClaims";
    }

    public static class Auth
    {
        public const string ChangePassword = "Auth.ChangePassword";
        public const string ViewOwnProfile = "Auth.ViewOwnProfile";
        public const string EditOwnProfile = "Auth.EditOwnProfile";
    }

    public static class Product
    {
        public const string ViewList = "Product.ViewList";
        public const string View = "Product.View";
        public const string Create = "Product.Create";
        public const string Edit = "Product.Edit";
        public const string Delete = "Product.Delete";
    }

    public static class Category
    {
        public const string ViewList = "Category.ViewList";
        public const string View = "Category.View";
        public const string Create = "Category.Create";
        public const string Edit = "Category.Edit";
        public const string Delete = "Category.Delete";
    }

    public static class SubCategory
    {
        public const string ViewList = "SubCategory.ViewList";
        public const string View = "SubCategory.View";
        public const string Create = "SubCategory.Create";
        public const string Edit = "SubCategory.Edit";
        public const string Delete = "SubCategory.Delete";
    }

    public static class Brand
    {
        public const string ViewList = "Brand.ViewList";
        public const string View = "Brand.View";
        public const string Create = "Brand.Create";
        public const string Edit = "Brand.Edit";
        public const string Delete = "Brand.Delete";
    }

    public static class UnitOfMeasure
    {
        public const string ViewList = "UnitOfMeasure.ViewList";
        public const string View = "UnitOfMeasure.View";
        public const string Create = "UnitOfMeasure.Create";
        public const string Edit = "UnitOfMeasure.Edit";
        public const string Delete = "UnitOfMeasure.Delete";
    }

    public static class Warranty
    {
        public const string ViewList = "Warranty.ViewList";
        public const string View = "Warranty.View";
        public const string Create = "Warranty.Create";
        public const string Edit = "Warranty.Edit";
        public const string Delete = "Warranty.Delete";
    }

    public static class VariantAttribute
    {
        public const string ViewList = "VariantAttribute.ViewList";
        public const string View = "VariantAttribute.View";
        public const string Create = "VariantAttribute.Create";
        public const string Edit = "VariantAttribute.Edit";
        public const string Delete = "VariantAttribute.Delete";
    }

    public static class Account
    {
        public const string ViewList = "Account.ViewList";
        public const string View = "Account.View";
        public const string Create = "Account.Create";
        public const string Edit = "Account.Edit";
        public const string Delete = "Account.Delete";
    }

    public static class Coupon
    {
        public const string ViewList = "Coupon.ViewList";
        public const string View = "Coupon.View";
        public const string Create = "Coupon.Create";
        public const string Edit = "Coupon.Edit";
        public const string Delete = "Coupon.Delete";
    }

    public static class Discount
    {
        public const string ViewList = "Discount.ViewList";
        public const string View = "Discount.View";
        public const string Create = "Discount.Create";
        public const string Edit = "Discount.Edit";
        public const string Delete = "Discount.Delete";
    }

    public static class GiftCard
    {
        public const string ViewList = "GiftCard.ViewList";
        public const string View = "GiftCard.View";
        public const string Create = "GiftCard.Create";
        public const string Edit = "GiftCard.Edit";
        public const string Delete = "GiftCard.Delete";
    }

    public static List<string> GetAll()
    {
        return new List<string>
        {
            Customer.ViewProfile,
            Customer.EditProfile,
            Customer.ViewList,
            Customer.Delete,
            Vendor.ViewProfile,
            Vendor.EditProfile,
            Vendor.ViewDashboard,
            Vendor.ViewList,
            Vendor.Create,
            Vendor.Delete,
            Admin.ViewProfile,
            Admin.EditProfile,
            Admin.ViewList,
            Admin.Create,
            Admin.Delete,
            Admin.ManageUsers,
            Admin.ManageRoles,
            Admin.ManageClaims,
            Auth.ChangePassword,
            Auth.ViewOwnProfile,
            Auth.EditOwnProfile,
            Product.ViewList,
            Product.View,
            Product.Create,
            Product.Edit,
            Product.Delete,
            Category.ViewList,
            Category.View,
            Category.Create,
            Category.Edit,
            Category.Delete,
            SubCategory.ViewList,
            SubCategory.View,
            SubCategory.Create,
            SubCategory.Edit,
            SubCategory.Delete,
            Brand.ViewList,
            Brand.View,
            Brand.Create,
            Brand.Edit,
            Brand.Delete,
            UnitOfMeasure.ViewList,
            UnitOfMeasure.View,
            UnitOfMeasure.Create,
            UnitOfMeasure.Edit,
            UnitOfMeasure.Delete,
            Warranty.ViewList,
            Warranty.View,
            Warranty.Create,
            Warranty.Edit,
            Warranty.Delete,
            VariantAttribute.ViewList,
            VariantAttribute.View,
            VariantAttribute.Create,
            VariantAttribute.Edit,
            VariantAttribute.Delete,
            Account.ViewList,
            Account.View,
            Account.Create,
            Account.Edit,
            Account.Delete,
            Coupon.ViewList,
            Coupon.View,
            Coupon.Create,
            Coupon.Edit,
            Coupon.Delete,
            Discount.ViewList,
            Discount.View,
            Discount.Create,
            Discount.Edit,
            Discount.Delete,
            GiftCard.ViewList,
            GiftCard.View,
            GiftCard.Create,
            GiftCard.Edit,
            GiftCard.Delete
        };
    }

    public static List<string> GetDefaultForRole(string roleName)
    {
        return roleName switch
        {
            Roles.Customer => new List<string>
            {
                Customer.ViewProfile,
                Customer.EditProfile,
                Auth.ChangePassword,
                Auth.ViewOwnProfile,
                Auth.EditOwnProfile,
                Product.ViewList,
                Product.View
            },
            Roles.Merchant => new List<string>
            {
                Vendor.ViewProfile,
                Vendor.EditProfile,
                Vendor.ViewDashboard,
                Auth.ChangePassword,
                Auth.ViewOwnProfile,
                Auth.EditOwnProfile,
                Product.ViewList,
                Product.View,
                Product.Create,
                Product.Edit
            },
            Roles.StaffMerchant => new List<string>
            {
                Vendor.ViewProfile,
                Vendor.EditProfile,
                Vendor.ViewDashboard,
                Auth.ChangePassword,
                Auth.ViewOwnProfile,
                Auth.EditOwnProfile,
                Product.ViewList,
                Product.View,
                Product.Create,
                Product.Edit
            },
            Roles.Admin => GetAll(),
            Roles.SuperAdmin => GetAll(),
            _ => new List<string>()
        };
    }
}

public static class Policies
{
    public static class Customer
    {
        public const string ViewProfile = "CustomerViewProfile";
        public const string EditProfile = "CustomerEditProfile";
        public const string ViewList = "CustomerViewList";
        public const string Delete = "CustomerDelete";
    }

    public static class Vendor
    {
        public const string ViewProfile = "VendorViewProfile";
        public const string EditProfile = "VendorEditProfile";
        public const string ViewDashboard = "VendorViewDashboard";
        public const string ViewList = "VendorViewList";
        public const string Create = "VendorCreate";
        public const string Delete = "VendorDelete";
    }

    public static class Admin
    {
        public const string ViewProfile = "AdminViewProfile";
        public const string EditProfile = "AdminEditProfile";
        public const string ViewList = "AdminViewList";
        public const string Create = "AdminCreate";
        public const string Delete = "AdminDelete";
        public const string ManageUsers = "AdminManageUsers";
        public const string ManageRoles = "AdminManageRoles";
        public const string ManageClaims = "AdminManageClaims";
    }

    public static class Auth
    {
        public const string ChangePassword = "AuthChangePassword";
        public const string ViewOwnProfile = "AuthViewOwnProfile";
        public const string EditOwnProfile = "AuthEditOwnProfile";
    }

    public static class Product
    {
        public const string ViewList = "ProductViewList";
        public const string View = "ProductView";
        public const string Create = "ProductCreate";
        public const string Edit = "ProductEdit";
        public const string Delete = "ProductDelete";
    }

    public static class Category
    {
        public const string ViewList = "CategoryViewList";
        public const string View = "CategoryView";
        public const string Create = "CategoryCreate";
        public const string Edit = "CategoryEdit";
        public const string Delete = "CategoryDelete";
    }

    public static class SubCategory
    {
        public const string ViewList = "SubCategoryViewList";
        public const string View = "SubCategoryView";
        public const string Create = "SubCategoryCreate";
        public const string Edit = "SubCategoryEdit";
        public const string Delete = "SubCategoryDelete";
    }

    public static class Brand
    {
        public const string ViewList = "BrandViewList";
        public const string View = "BrandView";
        public const string Create = "BrandCreate";
        public const string Edit = "BrandEdit";
        public const string Delete = "BrandDelete";
    }

    public static class UnitOfMeasure
    {
        public const string ViewList = "UnitOfMeasureViewList";
        public const string View = "UnitOfMeasureView";
        public const string Create = "UnitOfMeasureCreate";
        public const string Edit = "UnitOfMeasureEdit";
        public const string Delete = "UnitOfMeasureDelete";
    }

    public static class Warranty
    {
        public const string ViewList = "WarrantyViewList";
        public const string View = "WarrantyView";
        public const string Create = "WarrantyCreate";
        public const string Edit = "WarrantyEdit";
        public const string Delete = "WarrantyDelete";
    }

    public static class VariantAttribute
    {
        public const string ViewList = "VariantAttributeViewList";
        public const string View = "VariantAttributeView";
        public const string Create = "VariantAttributeCreate";
        public const string Edit = "VariantAttributeEdit";
        public const string Delete = "VariantAttributeDelete";
    }

    public static class Account
    {
        public const string ViewList = "AccountViewList";
        public const string View = "AccountView";
        public const string Create = "AccountCreate";
        public const string Edit = "AccountEdit";
        public const string Delete = "AccountDelete";
    }

    public static class Coupon
    {
        public const string ViewList = "CouponViewList";
        public const string View = "CouponView";
        public const string Create = "CouponCreate";
        public const string Edit = "CouponEdit";
        public const string Delete = "CouponDelete";
    }

    public static class Discount
    {
        public const string ViewList = "DiscountViewList";
        public const string View = "DiscountView";
        public const string Create = "DiscountCreate";
        public const string Edit = "DiscountEdit";
        public const string Delete = "DiscountDelete";
    }

    public static class GiftCard
    {
        public const string ViewList = "GiftCardViewList";
        public const string View = "GiftCardView";
        public const string Create = "GiftCardCreate";
        public const string Edit = "GiftCardEdit";
        public const string Delete = "GiftCardDelete";
    }
}
