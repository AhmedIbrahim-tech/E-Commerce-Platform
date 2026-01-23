using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class Alter_Auth_Module : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_orders_customers_customer_id",
                table: "orders");

            migrationBuilder.DropForeignKey(
                name: "FK_user_refresh_tokens_Users_user_id",
                table: "user_refresh_tokens");

            migrationBuilder.DropTable(
                name: "employees");

            migrationBuilder.DropIndex(
                name: "ix_user_refresh_tokens_refresh_token",
                table: "user_refresh_tokens");

            migrationBuilder.DropColumn(
                name: "FullName",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "user_id",
                table: "user_refresh_tokens",
                newName: "app_user_id");

            migrationBuilder.RenameColumn(
                name: "refresh_token",
                table: "user_refresh_tokens",
                newName: "revoked_reason");

            migrationBuilder.RenameColumn(
                name: "expiry_date",
                table: "user_refresh_tokens",
                newName: "expires_at");

            migrationBuilder.RenameColumn(
                name: "added_time",
                table: "user_refresh_tokens",
                newName: "created_at");

            migrationBuilder.RenameIndex(
                name: "ix_user_refresh_tokens_user_id",
                table: "user_refresh_tokens",
                newName: "ix_user_refresh_tokens_app_user_id");

            migrationBuilder.RenameIndex(
                name: "ix_user_refresh_tokens_expiry_date",
                table: "user_refresh_tokens",
                newName: "ix_user_refresh_tokens_expires_at");

            migrationBuilder.AddColumn<string>(
                name: "DisplayName",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "token",
                table: "user_refresh_tokens",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "jwt_id",
                table: "user_refresh_tokens",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "revoked_at",
                table: "user_refresh_tokens",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "gender",
                table: "customers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "full_name",
                table: "customers",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "created_by",
                table: "customers",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "created_time",
                table: "customers",
                type: "datetimeoffset",
                nullable: false,
                defaultValueSql: "SYSDATETIMEOFFSET()");

            migrationBuilder.AddColumn<Guid>(
                name: "deleted_by",
                table: "customers",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "deleted_time",
                table: "customers",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "is_deleted",
                table: "customers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "modified_by",
                table: "customers",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "modified_time",
                table: "customers",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "gender",
                table: "admins",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "full_name",
                table: "admins",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "address",
                table: "admins",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(250)",
                oldMaxLength: 250,
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "created_by",
                table: "admins",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "created_time",
                table: "admins",
                type: "datetimeoffset",
                nullable: false,
                defaultValueSql: "SYSDATETIMEOFFSET()");

            migrationBuilder.AddColumn<Guid>(
                name: "deleted_by",
                table: "admins",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "deleted_time",
                table: "admins",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "is_deleted",
                table: "admins",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "modified_by",
                table: "admins",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "modified_time",
                table: "admins",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "audit_logs",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    event_type = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    event_name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    user_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    user_email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    additional_data = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    created_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "SYSDATETIMEOFFSET()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_audit_logs", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "vendors",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    store_name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    commission_rate = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    app_user_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    full_name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    gender = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    created_time = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "SYSDATETIMEOFFSET()"),
                    created_by = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    modified_time = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    modified_by = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    deleted_time = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    deleted_by = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_vendors", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "ix_user_refresh_tokens_token",
                table: "user_refresh_tokens",
                column: "token");

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
                name: "ix_vendors_app_user_id",
                table: "vendors",
                column: "app_user_id",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_orders_customers_customer_id",
                table: "orders",
                column: "customer_id",
                principalTable: "customers",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_user_refresh_tokens_Users_app_user_id",
                table: "user_refresh_tokens",
                column: "app_user_id",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_orders_customers_customer_id",
                table: "orders");

            migrationBuilder.DropForeignKey(
                name: "FK_user_refresh_tokens_Users_app_user_id",
                table: "user_refresh_tokens");

            migrationBuilder.DropTable(
                name: "audit_logs");

            migrationBuilder.DropTable(
                name: "vendors");

            migrationBuilder.DropIndex(
                name: "ix_user_refresh_tokens_token",
                table: "user_refresh_tokens");

            migrationBuilder.DropColumn(
                name: "DisplayName",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "revoked_at",
                table: "user_refresh_tokens");

            migrationBuilder.DropColumn(
                name: "created_by",
                table: "customers");

            migrationBuilder.DropColumn(
                name: "created_time",
                table: "customers");

            migrationBuilder.DropColumn(
                name: "deleted_by",
                table: "customers");

            migrationBuilder.DropColumn(
                name: "deleted_time",
                table: "customers");

            migrationBuilder.DropColumn(
                name: "is_deleted",
                table: "customers");

            migrationBuilder.DropColumn(
                name: "modified_by",
                table: "customers");

            migrationBuilder.DropColumn(
                name: "modified_time",
                table: "customers");

            migrationBuilder.DropColumn(
                name: "created_by",
                table: "admins");

            migrationBuilder.DropColumn(
                name: "created_time",
                table: "admins");

            migrationBuilder.DropColumn(
                name: "deleted_by",
                table: "admins");

            migrationBuilder.DropColumn(
                name: "deleted_time",
                table: "admins");

            migrationBuilder.DropColumn(
                name: "is_deleted",
                table: "admins");

            migrationBuilder.DropColumn(
                name: "modified_by",
                table: "admins");

            migrationBuilder.DropColumn(
                name: "modified_time",
                table: "admins");

            migrationBuilder.RenameColumn(
                name: "revoked_reason",
                table: "user_refresh_tokens",
                newName: "refresh_token");

            migrationBuilder.RenameColumn(
                name: "expires_at",
                table: "user_refresh_tokens",
                newName: "expiry_date");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "user_refresh_tokens",
                newName: "added_time");

            migrationBuilder.RenameColumn(
                name: "app_user_id",
                table: "user_refresh_tokens",
                newName: "user_id");

            migrationBuilder.RenameIndex(
                name: "ix_user_refresh_tokens_expires_at",
                table: "user_refresh_tokens",
                newName: "ix_user_refresh_tokens_expiry_date");

            migrationBuilder.RenameIndex(
                name: "ix_user_refresh_tokens_app_user_id",
                table: "user_refresh_tokens",
                newName: "ix_user_refresh_tokens_user_id");

            migrationBuilder.AddColumn<string>(
                name: "FullName",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "token",
                table: "user_refresh_tokens",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500);

            migrationBuilder.AlterColumn<string>(
                name: "jwt_id",
                table: "user_refresh_tokens",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "gender",
                table: "customers",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "full_name",
                table: "customers",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "gender",
                table: "admins",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "full_name",
                table: "admins",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "address",
                table: "admins",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(250)",
                oldMaxLength: 250);

            migrationBuilder.CreateTable(
                name: "employees",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    address = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    app_user_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    full_name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    gender = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    hire_date = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true, defaultValueSql: "SYSDATETIMEOFFSET()"),
                    position = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    salary = table.Column<decimal>(type: "decimal(18,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_employees", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "ix_user_refresh_tokens_refresh_token",
                table: "user_refresh_tokens",
                column: "refresh_token");

            migrationBuilder.CreateIndex(
                name: "ix_employees_app_user_id",
                table: "employees",
                column: "app_user_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_employees_position",
                table: "employees",
                column: "position");

            migrationBuilder.AddForeignKey(
                name: "FK_orders_customers_customer_id",
                table: "orders",
                column: "customer_id",
                principalTable: "customers",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_user_refresh_tokens_Users_user_id",
                table: "user_refresh_tokens",
                column: "user_id",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
