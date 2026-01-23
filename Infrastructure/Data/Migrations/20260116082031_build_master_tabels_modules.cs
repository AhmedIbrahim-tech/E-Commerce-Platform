using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class build_master_tabels_modules : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "BrandId",
                table: "products",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "SubCategoryId",
                table: "products",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "UnitOfMeasureId",
                table: "products",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "accounts",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    account_name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    account_number = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    bank_name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    branch_name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    iban = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    swift_code = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    initial_balance = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    current_balance = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    description = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    is_active = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    created_time = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "SYSDATETIMEOFFSET()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_accounts", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "brands",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    description = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    image_url = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    is_active = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    created_time = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "SYSDATETIMEOFFSET()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_brands", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "coupons",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    description = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    discount_amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    discount_percentage = table.Column<decimal>(type: "decimal(5,2)", nullable: true),
                    minimum_purchase_amount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    maximum_discount_amount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    start_date = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    end_date = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    usage_limit = table.Column<int>(type: "int", nullable: true),
                    used_count = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    is_active = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    created_time = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "SYSDATETIMEOFFSET()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_coupons", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "discount_plans",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    description = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    is_active = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    created_time = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "SYSDATETIMEOFFSET()")
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
                    code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    recipient_name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    recipient_email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    remaining_amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    expiry_date = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    is_active = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    is_redeemed = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    redeemed_date = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    created_time = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "SYSDATETIMEOFFSET()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_gift_cards", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "sub_categories",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    description = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    image_url = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    is_active = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    created_time = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "SYSDATETIMEOFFSET()"),
                    category_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
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
                    name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    short_name = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    description = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    is_active = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    created_time = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "SYSDATETIMEOFFSET()")
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
                    name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    description = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    is_active = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    created_time = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "SYSDATETIMEOFFSET()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_variant_attributes", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "discounts",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    description = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    discount_amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    discount_percentage = table.Column<decimal>(type: "decimal(5,2)", nullable: true),
                    start_date = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    end_date = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    is_active = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    created_time = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "SYSDATETIMEOFFSET()"),
                    discount_plan_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_discounts", x => x.id);
                    table.ForeignKey(
                        name: "FK_discounts_discount_plans_discount_plan_id",
                        column: x => x.discount_plan_id,
                        principalTable: "discount_plans",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "variant_attribute_values",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    value = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    display_name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    is_active = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    created_time = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "SYSDATETIMEOFFSET()"),
                    variant_attribute_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
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
                name: "IX_products_BrandId",
                table: "products",
                column: "BrandId");

            migrationBuilder.CreateIndex(
                name: "IX_products_SubCategoryId",
                table: "products",
                column: "SubCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_products_UnitOfMeasureId",
                table: "products",
                column: "UnitOfMeasureId");

            migrationBuilder.CreateIndex(
                name: "ix_accounts_account_number",
                table: "accounts",
                column: "account_number");

            migrationBuilder.CreateIndex(
                name: "ix_brands_name",
                table: "brands",
                column: "name");

            migrationBuilder.CreateIndex(
                name: "ix_coupons_code",
                table: "coupons",
                column: "code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_discount_plans_name",
                table: "discount_plans",
                column: "name");

            migrationBuilder.CreateIndex(
                name: "IX_discounts_discount_plan_id",
                table: "discounts",
                column: "discount_plan_id");

            migrationBuilder.CreateIndex(
                name: "ix_discounts_name",
                table: "discounts",
                column: "name");

            migrationBuilder.CreateIndex(
                name: "ix_gift_cards_code",
                table: "gift_cards",
                column: "code",
                unique: true);

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
                name: "FK_products_brands_BrandId",
                table: "products",
                column: "BrandId",
                principalTable: "brands",
                principalColumn: "id");

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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_products_brands_BrandId",
                table: "products");

            migrationBuilder.DropForeignKey(
                name: "FK_products_sub_categories_SubCategoryId",
                table: "products");

            migrationBuilder.DropForeignKey(
                name: "FK_products_units_UnitOfMeasureId",
                table: "products");

            migrationBuilder.DropTable(
                name: "accounts");

            migrationBuilder.DropTable(
                name: "brands");

            migrationBuilder.DropTable(
                name: "coupons");

            migrationBuilder.DropTable(
                name: "discounts");

            migrationBuilder.DropTable(
                name: "gift_cards");

            migrationBuilder.DropTable(
                name: "sub_categories");

            migrationBuilder.DropTable(
                name: "units");

            migrationBuilder.DropTable(
                name: "variant_attribute_values");

            migrationBuilder.DropTable(
                name: "discount_plans");

            migrationBuilder.DropTable(
                name: "variant_attributes");

            migrationBuilder.DropIndex(
                name: "IX_products_BrandId",
                table: "products");

            migrationBuilder.DropIndex(
                name: "IX_products_SubCategoryId",
                table: "products");

            migrationBuilder.DropIndex(
                name: "IX_products_UnitOfMeasureId",
                table: "products");

            migrationBuilder.DropColumn(
                name: "BrandId",
                table: "products");

            migrationBuilder.DropColumn(
                name: "SubCategoryId",
                table: "products");

            migrationBuilder.DropColumn(
                name: "UnitOfMeasureId",
                table: "products");
        }
    }
}
