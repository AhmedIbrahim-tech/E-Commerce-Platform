using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class Add_index : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "ix_vendors_created_time",
                table: "Vendors",
                column: "CreatedTime");

            migrationBuilder.CreateIndex(
                name: "ix_vendors_full_name",
                table: "Vendors",
                column: "FullName");

            migrationBuilder.CreateIndex(
                name: "ix_vendors_store_name",
                table: "Vendors",
                column: "StoreName");

            migrationBuilder.CreateIndex(
                name: "ix_customers_created_time",
                table: "Customers",
                column: "CreatedTime");

            migrationBuilder.CreateIndex(
                name: "ix_customers_full_name",
                table: "Customers",
                column: "FullName");

            migrationBuilder.CreateIndex(
                name: "ix_admins_created_time",
                table: "Admins",
                column: "CreatedTime");

            migrationBuilder.CreateIndex(
                name: "ix_admins_full_name",
                table: "Admins",
                column: "FullName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_vendors_created_time",
                table: "Vendors");

            migrationBuilder.DropIndex(
                name: "ix_vendors_full_name",
                table: "Vendors");

            migrationBuilder.DropIndex(
                name: "ix_vendors_store_name",
                table: "Vendors");

            migrationBuilder.DropIndex(
                name: "ix_customers_created_time",
                table: "Customers");

            migrationBuilder.DropIndex(
                name: "ix_customers_full_name",
                table: "Customers");

            migrationBuilder.DropIndex(
                name: "ix_admins_created_time",
                table: "Admins");

            migrationBuilder.DropIndex(
                name: "ix_admins_full_name",
                table: "Admins");
        }
    }
}
