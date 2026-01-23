using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class build_Product_module : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_discounts_discount_plans_discount_plan_id",
                table: "discounts");

            migrationBuilder.DropForeignKey(
                name: "FK_orders_customers_customer_id",
                table: "orders");

            migrationBuilder.DropForeignKey(
                name: "FK_orders_deliveries_delivery_id",
                table: "orders");

            migrationBuilder.DropForeignKey(
                name: "FK_orders_payments_payment_id",
                table: "orders");

            migrationBuilder.DropForeignKey(
                name: "FK_orders_shipping_addresses_shipping_address_id",
                table: "orders");

            migrationBuilder.DropForeignKey(
                name: "FK_products_brands_BrandId",
                table: "products");

            migrationBuilder.DropForeignKey(
                name: "FK_products_categories_category_id",
                table: "products");

            migrationBuilder.DropForeignKey(
                name: "FK_products_sub_categories_SubCategoryId",
                table: "products");

            migrationBuilder.DropForeignKey(
                name: "FK_products_units_UnitOfMeasureId",
                table: "products");

            migrationBuilder.DropForeignKey(
                name: "FK_reviews_customers_customer_id",
                table: "reviews");

            migrationBuilder.DropForeignKey(
                name: "FK_reviews_products_product_id",
                table: "reviews");

            migrationBuilder.DropForeignKey(
                name: "FK_user_refresh_tokens_Users_app_user_id",
                table: "user_refresh_tokens");

            migrationBuilder.DropTable(
                name: "audit_logs");

            migrationBuilder.DropTable(
                name: "cart_items");

            migrationBuilder.DropTable(
                name: "discount_plans");

            migrationBuilder.DropTable(
                name: "gift_cards");

            migrationBuilder.DropTable(
                name: "order_items");

            migrationBuilder.DropTable(
                name: "shipping_addresses");

            migrationBuilder.DropTable(
                name: "sub_categories");

            migrationBuilder.DropTable(
                name: "units");

            migrationBuilder.DropTable(
                name: "variant_attribute_values");

            migrationBuilder.DropTable(
                name: "carts");

            migrationBuilder.DropTable(
                name: "variant_attributes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_vendors",
                table: "vendors");

            migrationBuilder.DropPrimaryKey(
                name: "PK_reviews",
                table: "reviews");

            migrationBuilder.DropPrimaryKey(
                name: "PK_products",
                table: "products");

            migrationBuilder.DropPrimaryKey(
                name: "PK_payments",
                table: "payments");

            migrationBuilder.DropIndex(
                name: "ix_payments_transaction_id",
                table: "payments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_orders",
                table: "orders");

            migrationBuilder.DropIndex(
                name: "ix_orders_delivery_id",
                table: "orders");

            migrationBuilder.DropIndex(
                name: "ix_orders_payment_id",
                table: "orders");

            migrationBuilder.DropIndex(
                name: "IX_orders_shipping_address_id",
                table: "orders");

            migrationBuilder.DropPrimaryKey(
                name: "PK_discounts",
                table: "discounts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_deliveries",
                table: "deliveries");

            migrationBuilder.DropPrimaryKey(
                name: "PK_customers",
                table: "customers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_coupons",
                table: "coupons");

            migrationBuilder.DropPrimaryKey(
                name: "PK_categories",
                table: "categories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_brands",
                table: "brands");

            migrationBuilder.DropPrimaryKey(
                name: "PK_admins",
                table: "admins");

            migrationBuilder.DropPrimaryKey(
                name: "PK_accounts",
                table: "accounts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_user_refresh_tokens",
                table: "user_refresh_tokens");

            migrationBuilder.DropColumn(
                name: "image_url",
                table: "products");

            migrationBuilder.RenameTable(
                name: "vendors",
                newName: "Vendors");

            migrationBuilder.RenameTable(
                name: "reviews",
                newName: "Reviews");

            migrationBuilder.RenameTable(
                name: "products",
                newName: "Products");

            migrationBuilder.RenameTable(
                name: "payments",
                newName: "Payments");

            migrationBuilder.RenameTable(
                name: "orders",
                newName: "Orders");

            migrationBuilder.RenameTable(
                name: "discounts",
                newName: "Discounts");

            migrationBuilder.RenameTable(
                name: "deliveries",
                newName: "Deliveries");

            migrationBuilder.RenameTable(
                name: "customers",
                newName: "Customers");

            migrationBuilder.RenameTable(
                name: "coupons",
                newName: "Coupons");

            migrationBuilder.RenameTable(
                name: "categories",
                newName: "Categories");

            migrationBuilder.RenameTable(
                name: "brands",
                newName: "Brands");

            migrationBuilder.RenameTable(
                name: "admins",
                newName: "Admins");

            migrationBuilder.RenameTable(
                name: "accounts",
                newName: "Accounts");

            migrationBuilder.RenameTable(
                name: "user_refresh_tokens",
                newName: "UserRefreshTokens");

            migrationBuilder.RenameColumn(
                name: "gender",
                table: "Vendors",
                newName: "Gender");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Vendors",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "store_name",
                table: "Vendors",
                newName: "StoreName");

            migrationBuilder.RenameColumn(
                name: "second_phone_number",
                table: "Vendors",
                newName: "SecondPhoneNumber");

            migrationBuilder.RenameColumn(
                name: "phone_number",
                table: "Vendors",
                newName: "PhoneNumber");

            migrationBuilder.RenameColumn(
                name: "modified_time",
                table: "Vendors",
                newName: "ModifiedTime");

            migrationBuilder.RenameColumn(
                name: "modified_by",
                table: "Vendors",
                newName: "ModifiedBy");

            migrationBuilder.RenameColumn(
                name: "is_deleted",
                table: "Vendors",
                newName: "IsDeleted");

            migrationBuilder.RenameColumn(
                name: "full_name",
                table: "Vendors",
                newName: "FullName");

            migrationBuilder.RenameColumn(
                name: "deleted_time",
                table: "Vendors",
                newName: "DeletedTime");

            migrationBuilder.RenameColumn(
                name: "deleted_by",
                table: "Vendors",
                newName: "DeletedBy");

            migrationBuilder.RenameColumn(
                name: "created_time",
                table: "Vendors",
                newName: "CreatedTime");

            migrationBuilder.RenameColumn(
                name: "created_by",
                table: "Vendors",
                newName: "CreatedBy");

            migrationBuilder.RenameColumn(
                name: "commission_rate",
                table: "Vendors",
                newName: "CommissionRate");

            migrationBuilder.RenameColumn(
                name: "app_user_id",
                table: "Vendors",
                newName: "AppUserId");

            migrationBuilder.RenameColumn(
                name: "profile_image",
                table: "Users",
                newName: "ProfileImage");

            migrationBuilder.RenameColumn(
                name: "rating",
                table: "Reviews",
                newName: "Rating");

            migrationBuilder.RenameColumn(
                name: "comment",
                table: "Reviews",
                newName: "Comment");

            migrationBuilder.RenameColumn(
                name: "product_id",
                table: "Reviews",
                newName: "ProductId");

            migrationBuilder.RenameColumn(
                name: "customer_id",
                table: "Reviews",
                newName: "CustomerId");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "Reviews",
                newName: "CreatedTime");

            migrationBuilder.RenameColumn(
                name: "price",
                table: "Products",
                newName: "Price");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "Products",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "description",
                table: "Products",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Products",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "stock_quantity",
                table: "Products",
                newName: "StockQuantity");

            migrationBuilder.RenameColumn(
                name: "category_id",
                table: "Products",
                newName: "CategoryId");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "Products",
                newName: "CreatedTime");

            migrationBuilder.RenameIndex(
                name: "IX_products_UnitOfMeasureId",
                table: "Products",
                newName: "IX_Products_UnitOfMeasureId");

            migrationBuilder.RenameIndex(
                name: "IX_products_SubCategoryId",
                table: "Products",
                newName: "ix_products_sub_category_id");

            migrationBuilder.RenameIndex(
                name: "ix_products_created_at",
                table: "Products",
                newName: "ix_products_created_time");

            migrationBuilder.RenameIndex(
                name: "IX_products_BrandId",
                table: "Products",
                newName: "ix_products_brand_id");

            migrationBuilder.RenameColumn(
                name: "status",
                table: "Payments",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Payments",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "transaction_id",
                table: "Payments",
                newName: "TransactionId");

            migrationBuilder.RenameColumn(
                name: "total_amount",
                table: "Payments",
                newName: "TotalAmount");

            migrationBuilder.RenameColumn(
                name: "payment_method",
                table: "Payments",
                newName: "PaymentMethod");

            migrationBuilder.RenameColumn(
                name: "payment_date",
                table: "Payments",
                newName: "PaymentDate");

            migrationBuilder.RenameColumn(
                name: "order_id",
                table: "Payments",
                newName: "OrderId");

            migrationBuilder.RenameColumn(
                name: "status",
                table: "Orders",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Orders",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "total_amount",
                table: "Orders",
                newName: "TotalAmount");

            migrationBuilder.RenameColumn(
                name: "shipping_address_id",
                table: "Orders",
                newName: "ShippingAddressId");

            migrationBuilder.RenameColumn(
                name: "payment_id",
                table: "Orders",
                newName: "PaymentId");

            migrationBuilder.RenameColumn(
                name: "order_date",
                table: "Orders",
                newName: "OrderDate");

            migrationBuilder.RenameColumn(
                name: "delivery_id",
                table: "Orders",
                newName: "DeliveryId");

            migrationBuilder.RenameColumn(
                name: "customer_id",
                table: "Orders",
                newName: "CustomerId");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "Discounts",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "description",
                table: "Discounts",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Discounts",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "start_date",
                table: "Discounts",
                newName: "StartDate");

            migrationBuilder.RenameColumn(
                name: "is_active",
                table: "Discounts",
                newName: "IsActive");

            migrationBuilder.RenameColumn(
                name: "end_date",
                table: "Discounts",
                newName: "EndDate");

            migrationBuilder.RenameColumn(
                name: "discount_plan_id",
                table: "Discounts",
                newName: "DiscountPlanId");

            migrationBuilder.RenameColumn(
                name: "discount_percentage",
                table: "Discounts",
                newName: "DiscountPercentage");

            migrationBuilder.RenameColumn(
                name: "discount_amount",
                table: "Discounts",
                newName: "DiscountAmount");

            migrationBuilder.RenameColumn(
                name: "created_time",
                table: "Discounts",
                newName: "CreatedTime");

            migrationBuilder.RenameIndex(
                name: "IX_discounts_discount_plan_id",
                table: "Discounts",
                newName: "IX_Discounts_DiscountPlanId");

            migrationBuilder.RenameColumn(
                name: "status",
                table: "Deliveries",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "description",
                table: "Deliveries",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "cost",
                table: "Deliveries",
                newName: "Cost");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Deliveries",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "delivery_time",
                table: "Deliveries",
                newName: "DeliveryTime");

            migrationBuilder.RenameColumn(
                name: "delivery_method",
                table: "Deliveries",
                newName: "DeliveryMethod");

            migrationBuilder.RenameColumn(
                name: "gender",
                table: "Customers",
                newName: "Gender");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Customers",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "second_phone_number",
                table: "Customers",
                newName: "SecondPhoneNumber");

            migrationBuilder.RenameColumn(
                name: "phone_number",
                table: "Customers",
                newName: "PhoneNumber");

            migrationBuilder.RenameColumn(
                name: "modified_time",
                table: "Customers",
                newName: "ModifiedTime");

            migrationBuilder.RenameColumn(
                name: "modified_by",
                table: "Customers",
                newName: "ModifiedBy");

            migrationBuilder.RenameColumn(
                name: "is_deleted",
                table: "Customers",
                newName: "IsDeleted");

            migrationBuilder.RenameColumn(
                name: "full_name",
                table: "Customers",
                newName: "FullName");

            migrationBuilder.RenameColumn(
                name: "deleted_time",
                table: "Customers",
                newName: "DeletedTime");

            migrationBuilder.RenameColumn(
                name: "deleted_by",
                table: "Customers",
                newName: "DeletedBy");

            migrationBuilder.RenameColumn(
                name: "created_time",
                table: "Customers",
                newName: "CreatedTime");

            migrationBuilder.RenameColumn(
                name: "created_by",
                table: "Customers",
                newName: "CreatedBy");

            migrationBuilder.RenameColumn(
                name: "app_user_id",
                table: "Customers",
                newName: "AppUserId");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "Coupons",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "description",
                table: "Coupons",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "code",
                table: "Coupons",
                newName: "Code");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Coupons",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "used_count",
                table: "Coupons",
                newName: "UsedCount");

            migrationBuilder.RenameColumn(
                name: "usage_limit",
                table: "Coupons",
                newName: "UsageLimit");

            migrationBuilder.RenameColumn(
                name: "start_date",
                table: "Coupons",
                newName: "StartDate");

            migrationBuilder.RenameColumn(
                name: "minimum_purchase_amount",
                table: "Coupons",
                newName: "MinimumPurchaseAmount");

            migrationBuilder.RenameColumn(
                name: "maximum_discount_amount",
                table: "Coupons",
                newName: "MaximumDiscountAmount");

            migrationBuilder.RenameColumn(
                name: "is_active",
                table: "Coupons",
                newName: "IsActive");

            migrationBuilder.RenameColumn(
                name: "end_date",
                table: "Coupons",
                newName: "EndDate");

            migrationBuilder.RenameColumn(
                name: "discount_percentage",
                table: "Coupons",
                newName: "DiscountPercentage");

            migrationBuilder.RenameColumn(
                name: "discount_amount",
                table: "Coupons",
                newName: "DiscountAmount");

            migrationBuilder.RenameColumn(
                name: "created_time",
                table: "Coupons",
                newName: "CreatedTime");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "Categories",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "description",
                table: "Categories",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Categories",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "Brands",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "description",
                table: "Brands",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Brands",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "is_active",
                table: "Brands",
                newName: "IsActive");

            migrationBuilder.RenameColumn(
                name: "image_url",
                table: "Brands",
                newName: "ImageUrl");

            migrationBuilder.RenameColumn(
                name: "created_time",
                table: "Brands",
                newName: "CreatedTime");

            migrationBuilder.RenameColumn(
                name: "gender",
                table: "Admins",
                newName: "Gender");

            migrationBuilder.RenameColumn(
                name: "address",
                table: "Admins",
                newName: "Address");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Admins",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "second_phone_number",
                table: "Admins",
                newName: "SecondPhoneNumber");

            migrationBuilder.RenameColumn(
                name: "phone_number",
                table: "Admins",
                newName: "PhoneNumber");

            migrationBuilder.RenameColumn(
                name: "modified_time",
                table: "Admins",
                newName: "ModifiedTime");

            migrationBuilder.RenameColumn(
                name: "modified_by",
                table: "Admins",
                newName: "ModifiedBy");

            migrationBuilder.RenameColumn(
                name: "is_deleted",
                table: "Admins",
                newName: "IsDeleted");

            migrationBuilder.RenameColumn(
                name: "full_name",
                table: "Admins",
                newName: "FullName");

            migrationBuilder.RenameColumn(
                name: "deleted_time",
                table: "Admins",
                newName: "DeletedTime");

            migrationBuilder.RenameColumn(
                name: "deleted_by",
                table: "Admins",
                newName: "DeletedBy");

            migrationBuilder.RenameColumn(
                name: "created_time",
                table: "Admins",
                newName: "CreatedTime");

            migrationBuilder.RenameColumn(
                name: "created_by",
                table: "Admins",
                newName: "CreatedBy");

            migrationBuilder.RenameColumn(
                name: "app_user_id",
                table: "Admins",
                newName: "AppUserId");

            migrationBuilder.RenameColumn(
                name: "iban",
                table: "Accounts",
                newName: "Iban");

            migrationBuilder.RenameColumn(
                name: "description",
                table: "Accounts",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Accounts",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "swift_code",
                table: "Accounts",
                newName: "SwiftCode");

            migrationBuilder.RenameColumn(
                name: "is_active",
                table: "Accounts",
                newName: "IsActive");

            migrationBuilder.RenameColumn(
                name: "initial_balance",
                table: "Accounts",
                newName: "InitialBalance");

            migrationBuilder.RenameColumn(
                name: "current_balance",
                table: "Accounts",
                newName: "CurrentBalance");

            migrationBuilder.RenameColumn(
                name: "created_time",
                table: "Accounts",
                newName: "CreatedTime");

            migrationBuilder.RenameColumn(
                name: "branch_name",
                table: "Accounts",
                newName: "BranchName");

            migrationBuilder.RenameColumn(
                name: "bank_name",
                table: "Accounts",
                newName: "BankName");

            migrationBuilder.RenameColumn(
                name: "account_number",
                table: "Accounts",
                newName: "AccountNumber");

            migrationBuilder.RenameColumn(
                name: "account_name",
                table: "Accounts",
                newName: "AccountName");

            migrationBuilder.RenameColumn(
                name: "token",
                table: "UserRefreshTokens",
                newName: "Token");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "UserRefreshTokens",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "revoked_reason",
                table: "UserRefreshTokens",
                newName: "RevokedReason");

            migrationBuilder.RenameColumn(
                name: "revoked_at",
                table: "UserRefreshTokens",
                newName: "RevokedAt");

            migrationBuilder.RenameColumn(
                name: "jwt_id",
                table: "UserRefreshTokens",
                newName: "JwtId");

            migrationBuilder.RenameColumn(
                name: "is_used",
                table: "UserRefreshTokens",
                newName: "IsUsed");

            migrationBuilder.RenameColumn(
                name: "is_revoked",
                table: "UserRefreshTokens",
                newName: "IsRevoked");

            migrationBuilder.RenameColumn(
                name: "expires_at",
                table: "UserRefreshTokens",
                newName: "ExpiresAt");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "UserRefreshTokens",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "app_user_id",
                table: "UserRefreshTokens",
                newName: "AppUserId");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Products",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Products",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(300)",
                oldMaxLength: 300,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Barcode",
                table: "Products",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BarcodeSymbology",
                table: "Products",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedBy",
                table: "Products",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "DeletedBy",
                table: "Products",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DeletedTime",
                table: "Products",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DiscountType",
                table: "Products",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "DiscountValue",
                table: "Products",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "ExpiryDate",
                table: "Products",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Products",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Products",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "ManufacturedDate",
                table: "Products",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Manufacturer",
                table: "Products",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ModifiedBy",
                table: "Products",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "ModifiedTime",
                table: "Products",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProductType",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "QuantityAlert",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "SKU",
                table: "Products",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "SellingType",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Slug",
                table: "Products",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "TaxRate",
                table: "Products",
                type: "decimal(5,2)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TaxType",
                table: "Products",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "WarrantyId",
                table: "Products",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedBy",
                table: "Orders",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CreatedTime",
                table: "Orders",
                type: "datetimeoffset",
                nullable: false,
                defaultValueSql: "SYSDATETIMEOFFSET()");

            migrationBuilder.AddColumn<Guid>(
                name: "ModifiedBy",
                table: "Orders",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "ModifiedTime",
                table: "Orders",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedBy",
                table: "Discounts",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "DeletedBy",
                table: "Discounts",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DeletedTime",
                table: "Discounts",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Discounts",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "ModifiedBy",
                table: "Discounts",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "ModifiedTime",
                table: "Discounts",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedBy",
                table: "Coupons",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "DeletedBy",
                table: "Coupons",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DeletedTime",
                table: "Coupons",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Coupons",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "ModifiedBy",
                table: "Coupons",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "ModifiedTime",
                table: "Coupons",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedBy",
                table: "Brands",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "DeletedBy",
                table: "Brands",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DeletedTime",
                table: "Brands",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Brands",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "ModifiedBy",
                table: "Brands",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "ModifiedTime",
                table: "Brands",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedBy",
                table: "Accounts",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "DeletedBy",
                table: "Accounts",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DeletedTime",
                table: "Accounts",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Accounts",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "ModifiedBy",
                table: "Accounts",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "ModifiedTime",
                table: "Accounts",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Vendors",
                table: "Vendors",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Reviews",
                table: "Reviews",
                columns: new[] { "CustomerId", "ProductId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Products",
                table: "Products",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Payments",
                table: "Payments",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Orders",
                table: "Orders",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Discounts",
                table: "Discounts",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Deliveries",
                table: "Deliveries",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Customers",
                table: "Customers",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Coupons",
                table: "Coupons",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Categories",
                table: "Categories",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Brands",
                table: "Brands",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Admins",
                table: "Admins",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Accounts",
                table: "Accounts",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserRefreshTokens",
                table: "UserRefreshTokens",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "AuditLogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EventType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    EventName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UserEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    AdditionalData = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    CreatedTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "SYSDATETIMEOFFSET()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditLogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Cart",
                columns: table => new
                {
                    CustomerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true, defaultValueSql: "SYSDATETIMEOFFSET()"),
                    PaymentToken = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    PaymentIntentId = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cart", x => x.CustomerId);
                });

            migrationBuilder.CreateTable(
                name: "DiscountPlans",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    CreatedTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "SYSDATETIMEOFFSET()"),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModifiedTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DeletedTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiscountPlans", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GiftCards",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    RecipientName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    RecipientEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    RemainingAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ExpiryDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    IsRedeemed = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    RedeemedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    CreatedTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "SYSDATETIMEOFFSET()"),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModifiedTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DeletedTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GiftCards", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OrderItems",
                columns: table => new
                {
                    ProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SubAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderItems", x => new { x.ProductId, x.OrderId });
                    table.ForeignKey(
                        name: "FK_OrderItems_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderItems_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProductImages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ImageURL = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    IsPrimary = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false),
                    CreatedTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "SYSDATETIMEOFFSET()"),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModifiedTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DeletedTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductImages_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductVariants",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VariantAttribute = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    VariantValue = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    SKU = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ImageURL = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "SYSDATETIMEOFFSET()"),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModifiedTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DeletedTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductVariants", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductVariants_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ShippingAddresses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Street = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    City = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    State = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    CustomerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShippingAddresses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ShippingAddresses_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SubCategories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    ImageUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    CreatedTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "SYSDATETIMEOFFSET()"),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModifiedTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DeletedTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubCategories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SubCategories_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UnitOfMeasures",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ShortName = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    CreatedTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "SYSDATETIMEOFFSET()"),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModifiedTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DeletedTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UnitOfMeasures", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VariantAttributes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    CreatedTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "SYSDATETIMEOFFSET()"),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModifiedTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DeletedTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VariantAttributes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CartItem",
                columns: table => new
                {
                    CartId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Quantity = table.Column<int>(type: "int", nullable: true),
                    SubAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CreatedTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "SYSDATETIMEOFFSET()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CartItem", x => new { x.CartId, x.ProductId });
                    table.ForeignKey(
                        name: "FK_CartItem_Cart_CartId",
                        column: x => x.CartId,
                        principalTable: "Cart",
                        principalColumn: "CustomerId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CartItem_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "VariantAttributeValues",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DisplayName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    CreatedTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "SYSDATETIMEOFFSET()"),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModifiedTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DeletedTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    VariantAttributeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VariantAttributeValues", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VariantAttributeValues_VariantAttributes_VariantAttributeId",
                        column: x => x.VariantAttributeId,
                        principalTable: "VariantAttributes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_products_sku",
                table: "Products",
                column: "SKU",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_products_slug",
                table: "Products",
                column: "Slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_payments_transaction_id",
                table: "Payments",
                column: "TransactionId",
                unique: true,
                filter: "[TransactionId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "ix_orders_delivery_id",
                table: "Orders",
                column: "DeliveryId",
                unique: true,
                filter: "[DeliveryId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "ix_orders_payment_id",
                table: "Orders",
                column: "PaymentId",
                unique: true,
                filter: "[PaymentId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_ShippingAddressId",
                table: "Orders",
                column: "ShippingAddressId",
                unique: true,
                filter: "[ShippingAddressId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "ix_audit_logs_created_time",
                table: "AuditLogs",
                column: "CreatedTime");

            migrationBuilder.CreateIndex(
                name: "ix_audit_logs_event_type",
                table: "AuditLogs",
                column: "EventType");

            migrationBuilder.CreateIndex(
                name: "ix_audit_logs_user_id",
                table: "AuditLogs",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "ix_cart_items_cart_id",
                table: "CartItem",
                column: "CartId");

            migrationBuilder.CreateIndex(
                name: "ix_cart_items_product_id",
                table: "CartItem",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "ix_discount_plans_name",
                table: "DiscountPlans",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "ix_gift_cards_code",
                table: "GiftCards",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_order_items_order_id",
                table: "OrderItems",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "ix_order_items_product_id",
                table: "OrderItems",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "ix_product_images_product_id",
                table: "ProductImages",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "ix_product_variants_product_id",
                table: "ProductVariants",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "ix_product_variants_sku",
                table: "ProductVariants",
                column: "SKU",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_shipping_addresses_customer_id",
                table: "ShippingAddresses",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "ix_sub_categories_category_id",
                table: "SubCategories",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "ix_sub_categories_name",
                table: "SubCategories",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "ix_units_name",
                table: "UnitOfMeasures",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "ix_units_short_name",
                table: "UnitOfMeasures",
                column: "ShortName");

            migrationBuilder.CreateIndex(
                name: "ix_variant_attributes_name",
                table: "VariantAttributes",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "ix_variant_attribute_values_variant_attribute_id",
                table: "VariantAttributeValues",
                column: "VariantAttributeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Discounts_DiscountPlans_DiscountPlanId",
                table: "Discounts",
                column: "DiscountPlanId",
                principalTable: "DiscountPlans",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Customers_CustomerId",
                table: "Orders",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Deliveries_DeliveryId",
                table: "Orders",
                column: "DeliveryId",
                principalTable: "Deliveries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Payments_PaymentId",
                table: "Orders",
                column: "PaymentId",
                principalTable: "Payments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_ShippingAddresses_ShippingAddressId",
                table: "Orders",
                column: "ShippingAddressId",
                principalTable: "ShippingAddresses",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Brands_BrandId",
                table: "Products",
                column: "BrandId",
                principalTable: "Brands",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Categories_CategoryId",
                table: "Products",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_SubCategories_SubCategoryId",
                table: "Products",
                column: "SubCategoryId",
                principalTable: "SubCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_UnitOfMeasures_UnitOfMeasureId",
                table: "Products",
                column: "UnitOfMeasureId",
                principalTable: "UnitOfMeasures",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Customers_CustomerId",
                table: "Reviews",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Products_ProductId",
                table: "Reviews",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserRefreshTokens_Users_AppUserId",
                table: "UserRefreshTokens",
                column: "AppUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Discounts_DiscountPlans_DiscountPlanId",
                table: "Discounts");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Customers_CustomerId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Deliveries_DeliveryId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Payments_PaymentId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_ShippingAddresses_ShippingAddressId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_Brands_BrandId",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_Categories_CategoryId",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_SubCategories_SubCategoryId",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_UnitOfMeasures_UnitOfMeasureId",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Customers_CustomerId",
                table: "Reviews");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Products_ProductId",
                table: "Reviews");

            migrationBuilder.DropForeignKey(
                name: "FK_UserRefreshTokens_Users_AppUserId",
                table: "UserRefreshTokens");

            migrationBuilder.DropTable(
                name: "AuditLogs");

            migrationBuilder.DropTable(
                name: "CartItem");

            migrationBuilder.DropTable(
                name: "DiscountPlans");

            migrationBuilder.DropTable(
                name: "GiftCards");

            migrationBuilder.DropTable(
                name: "OrderItems");

            migrationBuilder.DropTable(
                name: "ProductImages");

            migrationBuilder.DropTable(
                name: "ProductVariants");

            migrationBuilder.DropTable(
                name: "ShippingAddresses");

            migrationBuilder.DropTable(
                name: "SubCategories");

            migrationBuilder.DropTable(
                name: "UnitOfMeasures");

            migrationBuilder.DropTable(
                name: "VariantAttributeValues");

            migrationBuilder.DropTable(
                name: "Cart");

            migrationBuilder.DropTable(
                name: "VariantAttributes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Vendors",
                table: "Vendors");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Reviews",
                table: "Reviews");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Products",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "ix_products_sku",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "ix_products_slug",
                table: "Products");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Payments",
                table: "Payments");

            migrationBuilder.DropIndex(
                name: "ix_payments_transaction_id",
                table: "Payments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Orders",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "ix_orders_delivery_id",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "ix_orders_payment_id",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_ShippingAddressId",
                table: "Orders");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Discounts",
                table: "Discounts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Deliveries",
                table: "Deliveries");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Customers",
                table: "Customers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Coupons",
                table: "Coupons");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Categories",
                table: "Categories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Brands",
                table: "Brands");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Admins",
                table: "Admins");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Accounts",
                table: "Accounts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserRefreshTokens",
                table: "UserRefreshTokens");

            migrationBuilder.DropColumn(
                name: "Barcode",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "BarcodeSymbology",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "DeletedTime",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "DiscountType",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "DiscountValue",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ExpiryDate",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ManufacturedDate",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Manufacturer",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ModifiedTime",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ProductType",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "QuantityAlert",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "SKU",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "SellingType",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Slug",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "TaxRate",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "TaxType",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "WarrantyId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "CreatedTime",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "ModifiedTime",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Discounts");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "Discounts");

            migrationBuilder.DropColumn(
                name: "DeletedTime",
                table: "Discounts");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Discounts");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "Discounts");

            migrationBuilder.DropColumn(
                name: "ModifiedTime",
                table: "Discounts");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Coupons");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "Coupons");

            migrationBuilder.DropColumn(
                name: "DeletedTime",
                table: "Coupons");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Coupons");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "Coupons");

            migrationBuilder.DropColumn(
                name: "ModifiedTime",
                table: "Coupons");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Brands");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "Brands");

            migrationBuilder.DropColumn(
                name: "DeletedTime",
                table: "Brands");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Brands");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "Brands");

            migrationBuilder.DropColumn(
                name: "ModifiedTime",
                table: "Brands");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "DeletedTime",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "ModifiedTime",
                table: "Accounts");

            migrationBuilder.RenameTable(
                name: "Vendors",
                newName: "vendors");

            migrationBuilder.RenameTable(
                name: "Reviews",
                newName: "reviews");

            migrationBuilder.RenameTable(
                name: "Products",
                newName: "products");

            migrationBuilder.RenameTable(
                name: "Payments",
                newName: "payments");

            migrationBuilder.RenameTable(
                name: "Orders",
                newName: "orders");

            migrationBuilder.RenameTable(
                name: "Discounts",
                newName: "discounts");

            migrationBuilder.RenameTable(
                name: "Deliveries",
                newName: "deliveries");

            migrationBuilder.RenameTable(
                name: "Customers",
                newName: "customers");

            migrationBuilder.RenameTable(
                name: "Coupons",
                newName: "coupons");

            migrationBuilder.RenameTable(
                name: "Categories",
                newName: "categories");

            migrationBuilder.RenameTable(
                name: "Brands",
                newName: "brands");

            migrationBuilder.RenameTable(
                name: "Admins",
                newName: "admins");

            migrationBuilder.RenameTable(
                name: "Accounts",
                newName: "accounts");

            migrationBuilder.RenameTable(
                name: "UserRefreshTokens",
                newName: "user_refresh_tokens");

            migrationBuilder.RenameColumn(
                name: "Gender",
                table: "vendors",
                newName: "gender");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "vendors",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "StoreName",
                table: "vendors",
                newName: "store_name");

            migrationBuilder.RenameColumn(
                name: "SecondPhoneNumber",
                table: "vendors",
                newName: "second_phone_number");

            migrationBuilder.RenameColumn(
                name: "PhoneNumber",
                table: "vendors",
                newName: "phone_number");

            migrationBuilder.RenameColumn(
                name: "ModifiedTime",
                table: "vendors",
                newName: "modified_time");

            migrationBuilder.RenameColumn(
                name: "ModifiedBy",
                table: "vendors",
                newName: "modified_by");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "vendors",
                newName: "is_deleted");

            migrationBuilder.RenameColumn(
                name: "FullName",
                table: "vendors",
                newName: "full_name");

            migrationBuilder.RenameColumn(
                name: "DeletedTime",
                table: "vendors",
                newName: "deleted_time");

            migrationBuilder.RenameColumn(
                name: "DeletedBy",
                table: "vendors",
                newName: "deleted_by");

            migrationBuilder.RenameColumn(
                name: "CreatedTime",
                table: "vendors",
                newName: "created_time");

            migrationBuilder.RenameColumn(
                name: "CreatedBy",
                table: "vendors",
                newName: "created_by");

            migrationBuilder.RenameColumn(
                name: "CommissionRate",
                table: "vendors",
                newName: "commission_rate");

            migrationBuilder.RenameColumn(
                name: "AppUserId",
                table: "vendors",
                newName: "app_user_id");

            migrationBuilder.RenameColumn(
                name: "ProfileImage",
                table: "Users",
                newName: "profile_image");

            migrationBuilder.RenameColumn(
                name: "Rating",
                table: "reviews",
                newName: "rating");

            migrationBuilder.RenameColumn(
                name: "Comment",
                table: "reviews",
                newName: "comment");

            migrationBuilder.RenameColumn(
                name: "ProductId",
                table: "reviews",
                newName: "product_id");

            migrationBuilder.RenameColumn(
                name: "CustomerId",
                table: "reviews",
                newName: "customer_id");

            migrationBuilder.RenameColumn(
                name: "CreatedTime",
                table: "reviews",
                newName: "created_at");

            migrationBuilder.RenameColumn(
                name: "Price",
                table: "products",
                newName: "price");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "products",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "products",
                newName: "description");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "products",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "StockQuantity",
                table: "products",
                newName: "stock_quantity");

            migrationBuilder.RenameColumn(
                name: "CategoryId",
                table: "products",
                newName: "category_id");

            migrationBuilder.RenameColumn(
                name: "CreatedTime",
                table: "products",
                newName: "created_at");

            migrationBuilder.RenameIndex(
                name: "IX_Products_UnitOfMeasureId",
                table: "products",
                newName: "IX_products_UnitOfMeasureId");

            migrationBuilder.RenameIndex(
                name: "ix_products_sub_category_id",
                table: "products",
                newName: "IX_products_SubCategoryId");

            migrationBuilder.RenameIndex(
                name: "ix_products_created_time",
                table: "products",
                newName: "ix_products_created_at");

            migrationBuilder.RenameIndex(
                name: "ix_products_brand_id",
                table: "products",
                newName: "IX_products_BrandId");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "payments",
                newName: "status");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "payments",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "TransactionId",
                table: "payments",
                newName: "transaction_id");

            migrationBuilder.RenameColumn(
                name: "TotalAmount",
                table: "payments",
                newName: "total_amount");

            migrationBuilder.RenameColumn(
                name: "PaymentMethod",
                table: "payments",
                newName: "payment_method");

            migrationBuilder.RenameColumn(
                name: "PaymentDate",
                table: "payments",
                newName: "payment_date");

            migrationBuilder.RenameColumn(
                name: "OrderId",
                table: "payments",
                newName: "order_id");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "orders",
                newName: "status");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "orders",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "TotalAmount",
                table: "orders",
                newName: "total_amount");

            migrationBuilder.RenameColumn(
                name: "ShippingAddressId",
                table: "orders",
                newName: "shipping_address_id");

            migrationBuilder.RenameColumn(
                name: "PaymentId",
                table: "orders",
                newName: "payment_id");

            migrationBuilder.RenameColumn(
                name: "OrderDate",
                table: "orders",
                newName: "order_date");

            migrationBuilder.RenameColumn(
                name: "DeliveryId",
                table: "orders",
                newName: "delivery_id");

            migrationBuilder.RenameColumn(
                name: "CustomerId",
                table: "orders",
                newName: "customer_id");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "discounts",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "discounts",
                newName: "description");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "discounts",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "StartDate",
                table: "discounts",
                newName: "start_date");

            migrationBuilder.RenameColumn(
                name: "IsActive",
                table: "discounts",
                newName: "is_active");

            migrationBuilder.RenameColumn(
                name: "EndDate",
                table: "discounts",
                newName: "end_date");

            migrationBuilder.RenameColumn(
                name: "DiscountPlanId",
                table: "discounts",
                newName: "discount_plan_id");

            migrationBuilder.RenameColumn(
                name: "DiscountPercentage",
                table: "discounts",
                newName: "discount_percentage");

            migrationBuilder.RenameColumn(
                name: "DiscountAmount",
                table: "discounts",
                newName: "discount_amount");

            migrationBuilder.RenameColumn(
                name: "CreatedTime",
                table: "discounts",
                newName: "created_time");

            migrationBuilder.RenameIndex(
                name: "IX_Discounts_DiscountPlanId",
                table: "discounts",
                newName: "IX_discounts_discount_plan_id");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "deliveries",
                newName: "status");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "deliveries",
                newName: "description");

            migrationBuilder.RenameColumn(
                name: "Cost",
                table: "deliveries",
                newName: "cost");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "deliveries",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "DeliveryTime",
                table: "deliveries",
                newName: "delivery_time");

            migrationBuilder.RenameColumn(
                name: "DeliveryMethod",
                table: "deliveries",
                newName: "delivery_method");

            migrationBuilder.RenameColumn(
                name: "Gender",
                table: "customers",
                newName: "gender");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "customers",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "SecondPhoneNumber",
                table: "customers",
                newName: "second_phone_number");

            migrationBuilder.RenameColumn(
                name: "PhoneNumber",
                table: "customers",
                newName: "phone_number");

            migrationBuilder.RenameColumn(
                name: "ModifiedTime",
                table: "customers",
                newName: "modified_time");

            migrationBuilder.RenameColumn(
                name: "ModifiedBy",
                table: "customers",
                newName: "modified_by");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "customers",
                newName: "is_deleted");

            migrationBuilder.RenameColumn(
                name: "FullName",
                table: "customers",
                newName: "full_name");

            migrationBuilder.RenameColumn(
                name: "DeletedTime",
                table: "customers",
                newName: "deleted_time");

            migrationBuilder.RenameColumn(
                name: "DeletedBy",
                table: "customers",
                newName: "deleted_by");

            migrationBuilder.RenameColumn(
                name: "CreatedTime",
                table: "customers",
                newName: "created_time");

            migrationBuilder.RenameColumn(
                name: "CreatedBy",
                table: "customers",
                newName: "created_by");

            migrationBuilder.RenameColumn(
                name: "AppUserId",
                table: "customers",
                newName: "app_user_id");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "coupons",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "coupons",
                newName: "description");

            migrationBuilder.RenameColumn(
                name: "Code",
                table: "coupons",
                newName: "code");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "coupons",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UsedCount",
                table: "coupons",
                newName: "used_count");

            migrationBuilder.RenameColumn(
                name: "UsageLimit",
                table: "coupons",
                newName: "usage_limit");

            migrationBuilder.RenameColumn(
                name: "StartDate",
                table: "coupons",
                newName: "start_date");

            migrationBuilder.RenameColumn(
                name: "MinimumPurchaseAmount",
                table: "coupons",
                newName: "minimum_purchase_amount");

            migrationBuilder.RenameColumn(
                name: "MaximumDiscountAmount",
                table: "coupons",
                newName: "maximum_discount_amount");

            migrationBuilder.RenameColumn(
                name: "IsActive",
                table: "coupons",
                newName: "is_active");

            migrationBuilder.RenameColumn(
                name: "EndDate",
                table: "coupons",
                newName: "end_date");

            migrationBuilder.RenameColumn(
                name: "DiscountPercentage",
                table: "coupons",
                newName: "discount_percentage");

            migrationBuilder.RenameColumn(
                name: "DiscountAmount",
                table: "coupons",
                newName: "discount_amount");

            migrationBuilder.RenameColumn(
                name: "CreatedTime",
                table: "coupons",
                newName: "created_time");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "categories",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "categories",
                newName: "description");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "categories",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "brands",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "brands",
                newName: "description");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "brands",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "IsActive",
                table: "brands",
                newName: "is_active");

            migrationBuilder.RenameColumn(
                name: "ImageUrl",
                table: "brands",
                newName: "image_url");

            migrationBuilder.RenameColumn(
                name: "CreatedTime",
                table: "brands",
                newName: "created_time");

            migrationBuilder.RenameColumn(
                name: "Gender",
                table: "admins",
                newName: "gender");

            migrationBuilder.RenameColumn(
                name: "Address",
                table: "admins",
                newName: "address");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "admins",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "SecondPhoneNumber",
                table: "admins",
                newName: "second_phone_number");

            migrationBuilder.RenameColumn(
                name: "PhoneNumber",
                table: "admins",
                newName: "phone_number");

            migrationBuilder.RenameColumn(
                name: "ModifiedTime",
                table: "admins",
                newName: "modified_time");

            migrationBuilder.RenameColumn(
                name: "ModifiedBy",
                table: "admins",
                newName: "modified_by");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "admins",
                newName: "is_deleted");

            migrationBuilder.RenameColumn(
                name: "FullName",
                table: "admins",
                newName: "full_name");

            migrationBuilder.RenameColumn(
                name: "DeletedTime",
                table: "admins",
                newName: "deleted_time");

            migrationBuilder.RenameColumn(
                name: "DeletedBy",
                table: "admins",
                newName: "deleted_by");

            migrationBuilder.RenameColumn(
                name: "CreatedTime",
                table: "admins",
                newName: "created_time");

            migrationBuilder.RenameColumn(
                name: "CreatedBy",
                table: "admins",
                newName: "created_by");

            migrationBuilder.RenameColumn(
                name: "AppUserId",
                table: "admins",
                newName: "app_user_id");

            migrationBuilder.RenameColumn(
                name: "Iban",
                table: "accounts",
                newName: "iban");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "accounts",
                newName: "description");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "accounts",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "SwiftCode",
                table: "accounts",
                newName: "swift_code");

            migrationBuilder.RenameColumn(
                name: "IsActive",
                table: "accounts",
                newName: "is_active");

            migrationBuilder.RenameColumn(
                name: "InitialBalance",
                table: "accounts",
                newName: "initial_balance");

            migrationBuilder.RenameColumn(
                name: "CurrentBalance",
                table: "accounts",
                newName: "current_balance");

            migrationBuilder.RenameColumn(
                name: "CreatedTime",
                table: "accounts",
                newName: "created_time");

            migrationBuilder.RenameColumn(
                name: "BranchName",
                table: "accounts",
                newName: "branch_name");

            migrationBuilder.RenameColumn(
                name: "BankName",
                table: "accounts",
                newName: "bank_name");

            migrationBuilder.RenameColumn(
                name: "AccountNumber",
                table: "accounts",
                newName: "account_number");

            migrationBuilder.RenameColumn(
                name: "AccountName",
                table: "accounts",
                newName: "account_name");

            migrationBuilder.RenameColumn(
                name: "Token",
                table: "user_refresh_tokens",
                newName: "token");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "user_refresh_tokens",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "RevokedReason",
                table: "user_refresh_tokens",
                newName: "revoked_reason");

            migrationBuilder.RenameColumn(
                name: "RevokedAt",
                table: "user_refresh_tokens",
                newName: "revoked_at");

            migrationBuilder.RenameColumn(
                name: "JwtId",
                table: "user_refresh_tokens",
                newName: "jwt_id");

            migrationBuilder.RenameColumn(
                name: "IsUsed",
                table: "user_refresh_tokens",
                newName: "is_used");

            migrationBuilder.RenameColumn(
                name: "IsRevoked",
                table: "user_refresh_tokens",
                newName: "is_revoked");

            migrationBuilder.RenameColumn(
                name: "ExpiresAt",
                table: "user_refresh_tokens",
                newName: "expires_at");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "user_refresh_tokens",
                newName: "created_at");

            migrationBuilder.RenameColumn(
                name: "AppUserId",
                table: "user_refresh_tokens",
                newName: "app_user_id");

            migrationBuilder.AlterColumn<string>(
                name: "name",
                table: "products",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "description",
                table: "products",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(2000)",
                oldMaxLength: 2000,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "image_url",
                table: "products",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_vendors",
                table: "vendors",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_reviews",
                table: "reviews",
                columns: new[] { "customer_id", "product_id" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_products",
                table: "products",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_payments",
                table: "payments",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_orders",
                table: "orders",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_discounts",
                table: "discounts",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_deliveries",
                table: "deliveries",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_customers",
                table: "customers",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_coupons",
                table: "coupons",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_categories",
                table: "categories",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_brands",
                table: "brands",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_admins",
                table: "admins",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_accounts",
                table: "accounts",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_user_refresh_tokens",
                table: "user_refresh_tokens",
                column: "id");

            migrationBuilder.CreateTable(
                name: "audit_logs",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    additional_data = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    created_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "SYSDATETIMEOFFSET()"),
                    description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    event_name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    event_type = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    user_email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    user_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_audit_logs", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "carts",
                columns: table => new
                {
                    customer_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true, defaultValueSql: "SYSDATETIMEOFFSET()"),
                    payment_intent_id = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    payment_token = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    total_amount = table.Column<decimal>(type: "decimal(18,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_carts", x => x.customer_id);
                });

            migrationBuilder.CreateTable(
                name: "discount_plans",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    created_time = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "SYSDATETIMEOFFSET()"),
                    description = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    is_active = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_discount_plans", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "gift_cards",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    created_time = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "SYSDATETIMEOFFSET()"),
                    expiry_date = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    is_active = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    is_redeemed = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    recipient_email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    recipient_name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    redeemed_date = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    remaining_amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_gift_cards", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "order_items",
                columns: table => new
                {
                    product_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    order_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    quantity = table.Column<int>(type: "int", nullable: false),
                    sub_amount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    unit_price = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_order_items", x => new { x.product_id, x.order_id });
                    table.ForeignKey(
                        name: "FK_order_items_orders_order_id",
                        column: x => x.order_id,
                        principalTable: "orders",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_order_items_products_product_id",
                        column: x => x.product_id,
                        principalTable: "products",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "shipping_addresses",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    customer_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    city = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    first_name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    last_name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    state = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    street = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_shipping_addresses", x => x.id);
                    table.ForeignKey(
                        name: "FK_shipping_addresses_customers_customer_id",
                        column: x => x.customer_id,
                        principalTable: "customers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "sub_categories",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    category_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    created_time = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "SYSDATETIMEOFFSET()"),
                    description = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    image_url = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    is_active = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sub_categories", x => x.id);
                    table.ForeignKey(
                        name: "FK_sub_categories_categories_category_id",
                        column: x => x.category_id,
                        principalTable: "categories",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "units",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    created_time = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "SYSDATETIMEOFFSET()"),
                    description = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    is_active = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    short_name = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_units", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "variant_attributes",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    created_time = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "SYSDATETIMEOFFSET()"),
                    description = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    is_active = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_variant_attributes", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "cart_items",
                columns: table => new
                {
                    cart_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    product_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "SYSDATETIMEOFFSET()"),
                    price = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    quantity = table.Column<int>(type: "int", nullable: true),
                    sub_amount = table.Column<decimal>(type: "decimal(18,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cart_items", x => new { x.cart_id, x.product_id });
                    table.ForeignKey(
                        name: "FK_cart_items_carts_cart_id",
                        column: x => x.cart_id,
                        principalTable: "carts",
                        principalColumn: "customer_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_cart_items_products_product_id",
                        column: x => x.product_id,
                        principalTable: "products",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "variant_attribute_values",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    variant_attribute_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    created_time = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "SYSDATETIMEOFFSET()"),
                    display_name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    is_active = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    value = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_variant_attribute_values", x => x.id);
                    table.ForeignKey(
                        name: "FK_variant_attribute_values_variant_attributes_variant_attribute_id",
                        column: x => x.variant_attribute_id,
                        principalTable: "variant_attributes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_payments_transaction_id",
                table: "payments",
                column: "transaction_id",
                unique: true,
                filter: "[transaction_id] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "ix_orders_delivery_id",
                table: "orders",
                column: "delivery_id",
                unique: true,
                filter: "[delivery_id] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "ix_orders_payment_id",
                table: "orders",
                column: "payment_id",
                unique: true,
                filter: "[payment_id] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_orders_shipping_address_id",
                table: "orders",
                column: "shipping_address_id",
                unique: true,
                filter: "[shipping_address_id] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "ix_audit_logs_created_at",
                table: "audit_logs",
                column: "created_at");

            migrationBuilder.CreateIndex(
                name: "ix_audit_logs_event_type",
                table: "audit_logs",
                column: "event_type");

            migrationBuilder.CreateIndex(
                name: "ix_audit_logs_user_id",
                table: "audit_logs",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_cart_items_cart_id",
                table: "cart_items",
                column: "cart_id");

            migrationBuilder.CreateIndex(
                name: "ix_cart_items_product_id",
                table: "cart_items",
                column: "product_id");

            migrationBuilder.CreateIndex(
                name: "ix_discount_plans_name",
                table: "discount_plans",
                column: "name");

            migrationBuilder.CreateIndex(
                name: "ix_gift_cards_code",
                table: "gift_cards",
                column: "code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_order_items_order_id",
                table: "order_items",
                column: "order_id");

            migrationBuilder.CreateIndex(
                name: "ix_order_items_product_id",
                table: "order_items",
                column: "product_id");

            migrationBuilder.CreateIndex(
                name: "ix_shipping_addresses_customer_id",
                table: "shipping_addresses",
                column: "customer_id");

            migrationBuilder.CreateIndex(
                name: "ix_sub_categories_category_id",
                table: "sub_categories",
                column: "category_id");

            migrationBuilder.CreateIndex(
                name: "ix_sub_categories_name",
                table: "sub_categories",
                column: "name");

            migrationBuilder.CreateIndex(
                name: "ix_units_name",
                table: "units",
                column: "name");

            migrationBuilder.CreateIndex(
                name: "ix_units_short_name",
                table: "units",
                column: "short_name");

            migrationBuilder.CreateIndex(
                name: "ix_variant_attribute_values_variant_attribute_id",
                table: "variant_attribute_values",
                column: "variant_attribute_id");

            migrationBuilder.CreateIndex(
                name: "ix_variant_attributes_name",
                table: "variant_attributes",
                column: "name");

            migrationBuilder.AddForeignKey(
                name: "FK_discounts_discount_plans_discount_plan_id",
                table: "discounts",
                column: "discount_plan_id",
                principalTable: "discount_plans",
                principalColumn: "id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_orders_customers_customer_id",
                table: "orders",
                column: "customer_id",
                principalTable: "customers",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_orders_deliveries_delivery_id",
                table: "orders",
                column: "delivery_id",
                principalTable: "deliveries",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_orders_payments_payment_id",
                table: "orders",
                column: "payment_id",
                principalTable: "payments",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_orders_shipping_addresses_shipping_address_id",
                table: "orders",
                column: "shipping_address_id",
                principalTable: "shipping_addresses",
                principalColumn: "id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_products_brands_BrandId",
                table: "products",
                column: "BrandId",
                principalTable: "brands",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_products_categories_category_id",
                table: "products",
                column: "category_id",
                principalTable: "categories",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_products_sub_categories_SubCategoryId",
                table: "products",
                column: "SubCategoryId",
                principalTable: "sub_categories",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_products_units_UnitOfMeasureId",
                table: "products",
                column: "UnitOfMeasureId",
                principalTable: "units",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_reviews_customers_customer_id",
                table: "reviews",
                column: "customer_id",
                principalTable: "customers",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_reviews_products_product_id",
                table: "reviews",
                column: "product_id",
                principalTable: "products",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_user_refresh_tokens_Users_app_user_id",
                table: "user_refresh_tokens",
                column: "app_user_id",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
