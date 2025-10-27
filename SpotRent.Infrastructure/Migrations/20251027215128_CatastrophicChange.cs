using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace SpotRent.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CatastrophicChange : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_access_log_booking_BookingId",
                table: "access_log");

            migrationBuilder.DropForeignKey(
                name: "FK_access_log_device_device_id",
                table: "access_log");

            migrationBuilder.DropForeignKey(
                name: "FK_access_log_space_space_id",
                table: "access_log");

            migrationBuilder.DropForeignKey(
                name: "FK_access_log_user_user_id",
                table: "access_log");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                table: "AspNetRoleClaims");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserClaims_user_UserId",
                table: "AspNetUserClaims");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserLogins_user_UserId",
                table: "AspNetUserLogins");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                table: "AspNetUserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserRoles_user_UserId",
                table: "AspNetUserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserTokens_user_UserId",
                table: "AspNetUserTokens");

            migrationBuilder.DropForeignKey(
                name: "FK_booking_space_space_id",
                table: "booking");

            migrationBuilder.DropForeignKey(
                name: "FK_booking_user_user_id",
                table: "booking");

            migrationBuilder.DropForeignKey(
                name: "FK_device_space_space_id",
                table: "device");

            migrationBuilder.DropForeignKey(
                name: "FK_subscription_subscription_plan_subscription_plan_id",
                table: "subscription");

            migrationBuilder.DropForeignKey(
                name: "FK_subscription_user_user_id",
                table: "subscription");

            migrationBuilder.DropForeignKey(
                name: "FK_user_refresh_token_user_user_id",
                table: "user_refresh_token");

            migrationBuilder.DropTable(
                name: "payment");

            migrationBuilder.DropPrimaryKey(
                name: "PK_user_refresh_token",
                table: "user_refresh_token");

            migrationBuilder.DropPrimaryKey(
                name: "PK_user",
                table: "user");

            migrationBuilder.DropPrimaryKey(
                name: "PK_subscription_plan",
                table: "subscription_plan");

            migrationBuilder.DropPrimaryKey(
                name: "PK_subscription",
                table: "subscription");

            migrationBuilder.DropPrimaryKey(
                name: "PK_space",
                table: "space");

            migrationBuilder.DropPrimaryKey(
                name: "PK_device",
                table: "device");

            migrationBuilder.DropPrimaryKey(
                name: "PK_booking",
                table: "booking");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AspNetUserTokens",
                table: "AspNetUserTokens");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AspNetUserRoles",
                table: "AspNetUserRoles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AspNetUserLogins",
                table: "AspNetUserLogins");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AspNetUserClaims",
                table: "AspNetUserClaims");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AspNetRoles",
                table: "AspNetRoles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AspNetRoleClaims",
                table: "AspNetRoleClaims");

            migrationBuilder.DropPrimaryKey(
                name: "PK_access_log",
                table: "access_log");

            migrationBuilder.DropIndex(
                name: "IX_access_log_BookingId",
                table: "access_log");

            migrationBuilder.DropIndex(
                name: "IX_access_log_space_id",
                table: "access_log");

            migrationBuilder.DropColumn(
                name: "PictureUrl",
                table: "user");

            migrationBuilder.DropColumn(
                name: "equipment",
                table: "space");

            migrationBuilder.DropColumn(
                name: "has_air_conditioning",
                table: "space");

            migrationBuilder.DropColumn(
                name: "has_projector",
                table: "space");

            migrationBuilder.DropColumn(
                name: "has_whiteboard",
                table: "space");

            migrationBuilder.DropColumn(
                name: "has_wifi",
                table: "space");

            migrationBuilder.DropColumn(
                name: "image_url",
                table: "space");

            migrationBuilder.DropColumn(
                name: "room_number",
                table: "space");

            migrationBuilder.DropColumn(
                name: "device_identifier",
                table: "device");

            migrationBuilder.DropColumn(
                name: "last_heartbeat",
                table: "device");

            migrationBuilder.DropColumn(
                name: "BookingId",
                table: "access_log");

            migrationBuilder.DropColumn(
                name: "space_id",
                table: "access_log");

            migrationBuilder.RenameTable(
                name: "user",
                newName: "asp_net_users");

            migrationBuilder.RenameTable(
                name: "subscription_plan",
                newName: "subscription_plans");

            migrationBuilder.RenameTable(
                name: "subscription",
                newName: "subscriptions");

            migrationBuilder.RenameTable(
                name: "space",
                newName: "spaces");

            migrationBuilder.RenameTable(
                name: "device",
                newName: "devices");

            migrationBuilder.RenameTable(
                name: "booking",
                newName: "bookings");

            migrationBuilder.RenameTable(
                name: "AspNetUserTokens",
                newName: "asp_net_user_tokens");

            migrationBuilder.RenameTable(
                name: "AspNetUserRoles",
                newName: "asp_net_user_roles");

            migrationBuilder.RenameTable(
                name: "AspNetUserLogins",
                newName: "asp_net_user_logins");

            migrationBuilder.RenameTable(
                name: "AspNetUserClaims",
                newName: "asp_net_user_claims");

            migrationBuilder.RenameTable(
                name: "AspNetRoles",
                newName: "asp_net_roles");

            migrationBuilder.RenameTable(
                name: "AspNetRoleClaims",
                newName: "asp_net_role_claims");

            migrationBuilder.RenameTable(
                name: "access_log",
                newName: "access_logs");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "user_refresh_token",
                newName: "id");

            migrationBuilder.RenameIndex(
                name: "IX_user_refresh_token_user_id",
                table: "user_refresh_token",
                newName: "ix_user_refresh_tokens_user_id");

            migrationBuilder.RenameColumn(
                name: "UserName",
                table: "asp_net_users",
                newName: "user_name");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "asp_net_users",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "TwoFactorEnabled",
                table: "asp_net_users",
                newName: "two_factor_enabled");

            migrationBuilder.RenameColumn(
                name: "SecurityStamp",
                table: "asp_net_users",
                newName: "security_stamp");

            migrationBuilder.RenameColumn(
                name: "PhoneNumberConfirmed",
                table: "asp_net_users",
                newName: "phone_number_confirmed");

            migrationBuilder.RenameColumn(
                name: "PasswordHash",
                table: "asp_net_users",
                newName: "password_hash");

            migrationBuilder.RenameColumn(
                name: "NormalizedUserName",
                table: "asp_net_users",
                newName: "normalized_user_name");

            migrationBuilder.RenameColumn(
                name: "NormalizedEmail",
                table: "asp_net_users",
                newName: "normalized_email");

            migrationBuilder.RenameColumn(
                name: "LockoutEnd",
                table: "asp_net_users",
                newName: "lockout_end");

            migrationBuilder.RenameColumn(
                name: "LockoutEnabled",
                table: "asp_net_users",
                newName: "lockout_enabled");

            migrationBuilder.RenameColumn(
                name: "GoogleId",
                table: "asp_net_users",
                newName: "google_id");

            migrationBuilder.RenameColumn(
                name: "EmailConfirmed",
                table: "asp_net_users",
                newName: "email_confirmed");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "asp_net_users",
                newName: "created_at");

            migrationBuilder.RenameColumn(
                name: "ConcurrencyStamp",
                table: "asp_net_users",
                newName: "concurrency_stamp");

            migrationBuilder.RenameColumn(
                name: "AccessFailedCount",
                table: "asp_net_users",
                newName: "access_failed_count");

            migrationBuilder.RenameColumn(
                name: "user_id",
                table: "asp_net_users",
                newName: "id");

            migrationBuilder.RenameIndex(
                name: "UserNameIndex",
                table: "asp_net_users",
                newName: "user_name_index");

            migrationBuilder.RenameIndex(
                name: "EmailIndex",
                table: "asp_net_users",
                newName: "email_index");

            migrationBuilder.RenameColumn(
                name: "subscription_plan_id",
                table: "subscription_plans",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "subscription_id",
                table: "subscriptions",
                newName: "id");

            migrationBuilder.RenameIndex(
                name: "IX_subscription_user_id",
                table: "subscriptions",
                newName: "ix_subscriptions_user_id");

            migrationBuilder.RenameIndex(
                name: "IX_subscription_subscription_plan_id",
                table: "subscriptions",
                newName: "ix_subscriptions_subscription_plan_id");

            migrationBuilder.RenameColumn(
                name: "space_id",
                table: "spaces",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "type",
                table: "spaces",
                newName: "space_type");

            migrationBuilder.RenameColumn(
                name: "device_id",
                table: "devices",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "status",
                table: "devices",
                newName: "lock_status");

            migrationBuilder.RenameIndex(
                name: "IX_device_space_id",
                table: "devices",
                newName: "ix_devices_space_id");

            migrationBuilder.RenameColumn(
                name: "booking_id",
                table: "bookings",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "booking_type",
                table: "bookings",
                newName: "payment_status");

            migrationBuilder.RenameIndex(
                name: "IX_booking_user_id",
                table: "bookings",
                newName: "ix_bookings_user_id");

            migrationBuilder.RenameIndex(
                name: "IX_booking_space_id",
                table: "bookings",
                newName: "ix_bookings_space_id");

            migrationBuilder.RenameColumn(
                name: "Value",
                table: "asp_net_user_tokens",
                newName: "value");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "asp_net_user_tokens",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "LoginProvider",
                table: "asp_net_user_tokens",
                newName: "login_provider");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "asp_net_user_tokens",
                newName: "user_id");

            migrationBuilder.RenameColumn(
                name: "RoleId",
                table: "asp_net_user_roles",
                newName: "role_id");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "asp_net_user_roles",
                newName: "user_id");

            migrationBuilder.RenameIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "asp_net_user_roles",
                newName: "ix_asp_net_user_roles_role_id");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "asp_net_user_logins",
                newName: "user_id");

            migrationBuilder.RenameColumn(
                name: "ProviderDisplayName",
                table: "asp_net_user_logins",
                newName: "provider_display_name");

            migrationBuilder.RenameColumn(
                name: "ProviderKey",
                table: "asp_net_user_logins",
                newName: "provider_key");

            migrationBuilder.RenameColumn(
                name: "LoginProvider",
                table: "asp_net_user_logins",
                newName: "login_provider");

            migrationBuilder.RenameIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "asp_net_user_logins",
                newName: "ix_asp_net_user_logins_user_id");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "asp_net_user_claims",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "asp_net_user_claims",
                newName: "user_id");

            migrationBuilder.RenameColumn(
                name: "ClaimValue",
                table: "asp_net_user_claims",
                newName: "claim_value");

            migrationBuilder.RenameColumn(
                name: "ClaimType",
                table: "asp_net_user_claims",
                newName: "claim_type");

            migrationBuilder.RenameIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "asp_net_user_claims",
                newName: "ix_asp_net_user_claims_user_id");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "asp_net_roles",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "asp_net_roles",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "NormalizedName",
                table: "asp_net_roles",
                newName: "normalized_name");

            migrationBuilder.RenameColumn(
                name: "ConcurrencyStamp",
                table: "asp_net_roles",
                newName: "concurrency_stamp");

            migrationBuilder.RenameIndex(
                name: "RoleNameIndex",
                table: "asp_net_roles",
                newName: "role_name_index");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "asp_net_role_claims",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "RoleId",
                table: "asp_net_role_claims",
                newName: "role_id");

            migrationBuilder.RenameColumn(
                name: "ClaimValue",
                table: "asp_net_role_claims",
                newName: "claim_value");

            migrationBuilder.RenameColumn(
                name: "ClaimType",
                table: "asp_net_role_claims",
                newName: "claim_type");

            migrationBuilder.RenameIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "asp_net_role_claims",
                newName: "ix_asp_net_role_claims_role_id");

            migrationBuilder.RenameColumn(
                name: "access_log_id",
                table: "access_logs",
                newName: "id");

            migrationBuilder.RenameIndex(
                name: "IX_access_log_user_id",
                table: "access_logs",
                newName: "ix_access_logs_user_id");

            migrationBuilder.RenameIndex(
                name: "IX_access_log_device_id",
                table: "access_logs",
                newName: "ix_access_logs_device_id");

            migrationBuilder.AlterColumn<string>(
                name: "phone_number",
                table: "asp_net_users",
                type: "character varying(30)",
                maxLength: 30,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(30)",
                oldMaxLength: 30);

            migrationBuilder.AlterColumn<DateTime>(
                name: "updated_at",
                table: "asp_net_users",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "CURRENT_TIMESTAMP",
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<string>(
                name: "google_id",
                table: "asp_net_users",
                type: "character varying(255)",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_at",
                table: "asp_net_users",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "CURRENT_TIMESTAMP",
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AddColumn<DateTime>(
                name: "cancelled_at",
                table: "subscriptions",
                type: "timestamp with time zone",
                nullable: true,
                defaultValueSql: "CURRENT_TIMESTAMP");

            migrationBuilder.AddColumn<DateTime>(
                name: "payment_created_at",
                table: "subscriptions",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "CURRENT_TIMESTAMP");

            migrationBuilder.AddColumn<string>(
                name: "payment_failure_reason",
                table: "subscriptions",
                type: "character varying(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "payment_processed_at",
                table: "subscriptions",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "payment_status",
                table: "subscriptions",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "total_amount",
                table: "subscriptions",
                type: "numeric(10,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<long>(
                name: "transaction_id",
                table: "subscriptions",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "address_line",
                table: "spaces",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "area_sqm",
                table: "spaces",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "city",
                table: "spaces",
                type: "character varying(250)",
                maxLength: 250,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "house",
                table: "spaces",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "oblast",
                table: "spaces",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "owner_id",
                table: "spaces",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "street",
                table: "spaces",
                type: "character varying(250)",
                maxLength: 250,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "updated_at",
                table: "spaces",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<string>(
                name: "device_name",
                table: "devices",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "user_id",
                table: "bookings",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<DateTime>(
                name: "payment_created_at",
                table: "bookings",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "CURRENT_TIMESTAMP");

            migrationBuilder.AddColumn<string>(
                name: "payment_failure_reason",
                table: "bookings",
                type: "character varying(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "payment_processed_at",
                table: "bookings",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "transaction_id",
                table: "bookings",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AlterColumn<int>(
                name: "user_id",
                table: "access_logs",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "device_id",
                table: "access_logs",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddPrimaryKey(
                name: "pk_user_refresh_tokens",
                table: "user_refresh_token",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_asp_net_users",
                table: "asp_net_users",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_subscription_plans",
                table: "subscription_plans",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_subscriptions",
                table: "subscriptions",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_spaces",
                table: "spaces",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_devices",
                table: "devices",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_bookings",
                table: "bookings",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_asp_net_user_tokens",
                table: "asp_net_user_tokens",
                columns: new[] { "user_id", "login_provider", "name" });

            migrationBuilder.AddPrimaryKey(
                name: "pk_asp_net_user_roles",
                table: "asp_net_user_roles",
                columns: new[] { "user_id", "role_id" });

            migrationBuilder.AddPrimaryKey(
                name: "pk_asp_net_user_logins",
                table: "asp_net_user_logins",
                columns: new[] { "login_provider", "provider_key" });

            migrationBuilder.AddPrimaryKey(
                name: "pk_asp_net_user_claims",
                table: "asp_net_user_claims",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_asp_net_roles",
                table: "asp_net_roles",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_asp_net_role_claims",
                table: "asp_net_role_claims",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_access_logs",
                table: "access_logs",
                column: "id");

            migrationBuilder.CreateTable(
                name: "attribute",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    data_type = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_attribute", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "image",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    image_url = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    space_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_image", x => x.id);
                    table.ForeignKey(
                        name: "fk_image_spaces_space_id",
                        column: x => x.space_id,
                        principalTable: "spaces",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "space_attribute",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    attribute_id = table.Column<int>(type: "integer", nullable: false),
                    space_id = table.Column<int>(type: "integer", nullable: false),
                    value = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_space_attribute", x => x.id);
                    table.ForeignKey(
                        name: "fk_space_attribute_attribute_attribute_id",
                        column: x => x.attribute_id,
                        principalTable: "attribute",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_space_attribute_spaces_space_id",
                        column: x => x.space_id,
                        principalTable: "spaces",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_spaces_owner_id",
                table: "spaces",
                column: "owner_id");

            migrationBuilder.CreateIndex(
                name: "ix_image_space_id",
                table: "image",
                column: "space_id");

            migrationBuilder.CreateIndex(
                name: "ix_space_attribute_attribute_id",
                table: "space_attribute",
                column: "attribute_id");

            migrationBuilder.CreateIndex(
                name: "ix_space_attribute_space_id",
                table: "space_attribute",
                column: "space_id");

            migrationBuilder.AddForeignKey(
                name: "fk_access_logs_asp_net_users_user_id",
                table: "access_logs",
                column: "user_id",
                principalTable: "asp_net_users",
                principalColumn: "id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "fk_access_logs_devices_device_id",
                table: "access_logs",
                column: "device_id",
                principalTable: "devices",
                principalColumn: "id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "fk_asp_net_role_claims_asp_net_roles_role_id",
                table: "asp_net_role_claims",
                column: "role_id",
                principalTable: "asp_net_roles",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_asp_net_user_claims_asp_net_users_user_id",
                table: "asp_net_user_claims",
                column: "user_id",
                principalTable: "asp_net_users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_asp_net_user_logins_asp_net_users_user_id",
                table: "asp_net_user_logins",
                column: "user_id",
                principalTable: "asp_net_users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_asp_net_user_roles_asp_net_roles_role_id",
                table: "asp_net_user_roles",
                column: "role_id",
                principalTable: "asp_net_roles",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_asp_net_user_roles_asp_net_users_user_id",
                table: "asp_net_user_roles",
                column: "user_id",
                principalTable: "asp_net_users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_asp_net_user_tokens_asp_net_users_user_id",
                table: "asp_net_user_tokens",
                column: "user_id",
                principalTable: "asp_net_users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_bookings_asp_net_users_user_id",
                table: "bookings",
                column: "user_id",
                principalTable: "asp_net_users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_bookings_spaces_space_id",
                table: "bookings",
                column: "space_id",
                principalTable: "spaces",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_devices_spaces_space_id",
                table: "devices",
                column: "space_id",
                principalTable: "spaces",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_spaces_asp_net_users_owner_id",
                table: "spaces",
                column: "owner_id",
                principalTable: "asp_net_users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_subscriptions_asp_net_users_user_id",
                table: "subscriptions",
                column: "user_id",
                principalTable: "asp_net_users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_subscriptions_subscription_plans_subscription_plan_id",
                table: "subscriptions",
                column: "subscription_plan_id",
                principalTable: "subscription_plans",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_user_refresh_tokens_asp_net_users_user_id",
                table: "user_refresh_token",
                column: "user_id",
                principalTable: "asp_net_users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_access_logs_asp_net_users_user_id",
                table: "access_logs");

            migrationBuilder.DropForeignKey(
                name: "fk_access_logs_devices_device_id",
                table: "access_logs");

            migrationBuilder.DropForeignKey(
                name: "fk_asp_net_role_claims_asp_net_roles_role_id",
                table: "asp_net_role_claims");

            migrationBuilder.DropForeignKey(
                name: "fk_asp_net_user_claims_asp_net_users_user_id",
                table: "asp_net_user_claims");

            migrationBuilder.DropForeignKey(
                name: "fk_asp_net_user_logins_asp_net_users_user_id",
                table: "asp_net_user_logins");

            migrationBuilder.DropForeignKey(
                name: "fk_asp_net_user_roles_asp_net_roles_role_id",
                table: "asp_net_user_roles");

            migrationBuilder.DropForeignKey(
                name: "fk_asp_net_user_roles_asp_net_users_user_id",
                table: "asp_net_user_roles");

            migrationBuilder.DropForeignKey(
                name: "fk_asp_net_user_tokens_asp_net_users_user_id",
                table: "asp_net_user_tokens");

            migrationBuilder.DropForeignKey(
                name: "fk_bookings_asp_net_users_user_id",
                table: "bookings");

            migrationBuilder.DropForeignKey(
                name: "fk_bookings_spaces_space_id",
                table: "bookings");

            migrationBuilder.DropForeignKey(
                name: "fk_devices_spaces_space_id",
                table: "devices");

            migrationBuilder.DropForeignKey(
                name: "fk_spaces_asp_net_users_owner_id",
                table: "spaces");

            migrationBuilder.DropForeignKey(
                name: "fk_subscriptions_asp_net_users_user_id",
                table: "subscriptions");

            migrationBuilder.DropForeignKey(
                name: "fk_subscriptions_subscription_plans_subscription_plan_id",
                table: "subscriptions");

            migrationBuilder.DropForeignKey(
                name: "fk_user_refresh_tokens_asp_net_users_user_id",
                table: "user_refresh_token");

            migrationBuilder.DropTable(
                name: "image");

            migrationBuilder.DropTable(
                name: "space_attribute");

            migrationBuilder.DropTable(
                name: "attribute");

            migrationBuilder.DropPrimaryKey(
                name: "pk_user_refresh_tokens",
                table: "user_refresh_token");

            migrationBuilder.DropPrimaryKey(
                name: "pk_subscriptions",
                table: "subscriptions");

            migrationBuilder.DropPrimaryKey(
                name: "pk_subscription_plans",
                table: "subscription_plans");

            migrationBuilder.DropPrimaryKey(
                name: "pk_spaces",
                table: "spaces");

            migrationBuilder.DropIndex(
                name: "ix_spaces_owner_id",
                table: "spaces");

            migrationBuilder.DropPrimaryKey(
                name: "pk_devices",
                table: "devices");

            migrationBuilder.DropPrimaryKey(
                name: "pk_bookings",
                table: "bookings");

            migrationBuilder.DropPrimaryKey(
                name: "pk_asp_net_users",
                table: "asp_net_users");

            migrationBuilder.DropPrimaryKey(
                name: "pk_asp_net_user_tokens",
                table: "asp_net_user_tokens");

            migrationBuilder.DropPrimaryKey(
                name: "pk_asp_net_user_roles",
                table: "asp_net_user_roles");

            migrationBuilder.DropPrimaryKey(
                name: "pk_asp_net_user_logins",
                table: "asp_net_user_logins");

            migrationBuilder.DropPrimaryKey(
                name: "pk_asp_net_user_claims",
                table: "asp_net_user_claims");

            migrationBuilder.DropPrimaryKey(
                name: "pk_asp_net_roles",
                table: "asp_net_roles");

            migrationBuilder.DropPrimaryKey(
                name: "pk_asp_net_role_claims",
                table: "asp_net_role_claims");

            migrationBuilder.DropPrimaryKey(
                name: "pk_access_logs",
                table: "access_logs");

            migrationBuilder.DropColumn(
                name: "cancelled_at",
                table: "subscriptions");

            migrationBuilder.DropColumn(
                name: "payment_created_at",
                table: "subscriptions");

            migrationBuilder.DropColumn(
                name: "payment_failure_reason",
                table: "subscriptions");

            migrationBuilder.DropColumn(
                name: "payment_processed_at",
                table: "subscriptions");

            migrationBuilder.DropColumn(
                name: "payment_status",
                table: "subscriptions");

            migrationBuilder.DropColumn(
                name: "total_amount",
                table: "subscriptions");

            migrationBuilder.DropColumn(
                name: "transaction_id",
                table: "subscriptions");

            migrationBuilder.DropColumn(
                name: "address_line",
                table: "spaces");

            migrationBuilder.DropColumn(
                name: "area_sqm",
                table: "spaces");

            migrationBuilder.DropColumn(
                name: "city",
                table: "spaces");

            migrationBuilder.DropColumn(
                name: "house",
                table: "spaces");

            migrationBuilder.DropColumn(
                name: "oblast",
                table: "spaces");

            migrationBuilder.DropColumn(
                name: "owner_id",
                table: "spaces");

            migrationBuilder.DropColumn(
                name: "street",
                table: "spaces");

            migrationBuilder.DropColumn(
                name: "updated_at",
                table: "spaces");

            migrationBuilder.DropColumn(
                name: "payment_created_at",
                table: "bookings");

            migrationBuilder.DropColumn(
                name: "payment_failure_reason",
                table: "bookings");

            migrationBuilder.DropColumn(
                name: "payment_processed_at",
                table: "bookings");

            migrationBuilder.DropColumn(
                name: "transaction_id",
                table: "bookings");

            migrationBuilder.RenameTable(
                name: "subscriptions",
                newName: "subscription");

            migrationBuilder.RenameTable(
                name: "subscription_plans",
                newName: "subscription_plan");

            migrationBuilder.RenameTable(
                name: "spaces",
                newName: "space");

            migrationBuilder.RenameTable(
                name: "devices",
                newName: "device");

            migrationBuilder.RenameTable(
                name: "bookings",
                newName: "booking");

            migrationBuilder.RenameTable(
                name: "asp_net_users",
                newName: "user");

            migrationBuilder.RenameTable(
                name: "asp_net_user_tokens",
                newName: "AspNetUserTokens");

            migrationBuilder.RenameTable(
                name: "asp_net_user_roles",
                newName: "AspNetUserRoles");

            migrationBuilder.RenameTable(
                name: "asp_net_user_logins",
                newName: "AspNetUserLogins");

            migrationBuilder.RenameTable(
                name: "asp_net_user_claims",
                newName: "AspNetUserClaims");

            migrationBuilder.RenameTable(
                name: "asp_net_roles",
                newName: "AspNetRoles");

            migrationBuilder.RenameTable(
                name: "asp_net_role_claims",
                newName: "AspNetRoleClaims");

            migrationBuilder.RenameTable(
                name: "access_logs",
                newName: "access_log");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "user_refresh_token",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "ix_user_refresh_tokens_user_id",
                table: "user_refresh_token",
                newName: "IX_user_refresh_token_user_id");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "subscription",
                newName: "subscription_id");

            migrationBuilder.RenameIndex(
                name: "ix_subscriptions_user_id",
                table: "subscription",
                newName: "IX_subscription_user_id");

            migrationBuilder.RenameIndex(
                name: "ix_subscriptions_subscription_plan_id",
                table: "subscription",
                newName: "IX_subscription_subscription_plan_id");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "subscription_plan",
                newName: "subscription_plan_id");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "space",
                newName: "space_id");

            migrationBuilder.RenameColumn(
                name: "space_type",
                table: "space",
                newName: "type");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "device",
                newName: "device_id");

            migrationBuilder.RenameColumn(
                name: "lock_status",
                table: "device",
                newName: "status");

            migrationBuilder.RenameIndex(
                name: "ix_devices_space_id",
                table: "device",
                newName: "IX_device_space_id");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "booking",
                newName: "booking_id");

            migrationBuilder.RenameColumn(
                name: "payment_status",
                table: "booking",
                newName: "booking_type");

            migrationBuilder.RenameIndex(
                name: "ix_bookings_user_id",
                table: "booking",
                newName: "IX_booking_user_id");

            migrationBuilder.RenameIndex(
                name: "ix_bookings_space_id",
                table: "booking",
                newName: "IX_booking_space_id");

            migrationBuilder.RenameColumn(
                name: "user_name",
                table: "user",
                newName: "UserName");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "user",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "two_factor_enabled",
                table: "user",
                newName: "TwoFactorEnabled");

            migrationBuilder.RenameColumn(
                name: "security_stamp",
                table: "user",
                newName: "SecurityStamp");

            migrationBuilder.RenameColumn(
                name: "phone_number_confirmed",
                table: "user",
                newName: "PhoneNumberConfirmed");

            migrationBuilder.RenameColumn(
                name: "password_hash",
                table: "user",
                newName: "PasswordHash");

            migrationBuilder.RenameColumn(
                name: "normalized_user_name",
                table: "user",
                newName: "NormalizedUserName");

            migrationBuilder.RenameColumn(
                name: "normalized_email",
                table: "user",
                newName: "NormalizedEmail");

            migrationBuilder.RenameColumn(
                name: "lockout_end",
                table: "user",
                newName: "LockoutEnd");

            migrationBuilder.RenameColumn(
                name: "lockout_enabled",
                table: "user",
                newName: "LockoutEnabled");

            migrationBuilder.RenameColumn(
                name: "google_id",
                table: "user",
                newName: "GoogleId");

            migrationBuilder.RenameColumn(
                name: "email_confirmed",
                table: "user",
                newName: "EmailConfirmed");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "user",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "concurrency_stamp",
                table: "user",
                newName: "ConcurrencyStamp");

            migrationBuilder.RenameColumn(
                name: "access_failed_count",
                table: "user",
                newName: "AccessFailedCount");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "user",
                newName: "user_id");

            migrationBuilder.RenameIndex(
                name: "user_name_index",
                table: "user",
                newName: "UserNameIndex");

            migrationBuilder.RenameIndex(
                name: "email_index",
                table: "user",
                newName: "EmailIndex");

            migrationBuilder.RenameColumn(
                name: "value",
                table: "AspNetUserTokens",
                newName: "Value");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "AspNetUserTokens",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "login_provider",
                table: "AspNetUserTokens",
                newName: "LoginProvider");

            migrationBuilder.RenameColumn(
                name: "user_id",
                table: "AspNetUserTokens",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "role_id",
                table: "AspNetUserRoles",
                newName: "RoleId");

            migrationBuilder.RenameColumn(
                name: "user_id",
                table: "AspNetUserRoles",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "ix_asp_net_user_roles_role_id",
                table: "AspNetUserRoles",
                newName: "IX_AspNetUserRoles_RoleId");

            migrationBuilder.RenameColumn(
                name: "user_id",
                table: "AspNetUserLogins",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "provider_display_name",
                table: "AspNetUserLogins",
                newName: "ProviderDisplayName");

            migrationBuilder.RenameColumn(
                name: "provider_key",
                table: "AspNetUserLogins",
                newName: "ProviderKey");

            migrationBuilder.RenameColumn(
                name: "login_provider",
                table: "AspNetUserLogins",
                newName: "LoginProvider");

            migrationBuilder.RenameIndex(
                name: "ix_asp_net_user_logins_user_id",
                table: "AspNetUserLogins",
                newName: "IX_AspNetUserLogins_UserId");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "AspNetUserClaims",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "user_id",
                table: "AspNetUserClaims",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "claim_value",
                table: "AspNetUserClaims",
                newName: "ClaimValue");

            migrationBuilder.RenameColumn(
                name: "claim_type",
                table: "AspNetUserClaims",
                newName: "ClaimType");

            migrationBuilder.RenameIndex(
                name: "ix_asp_net_user_claims_user_id",
                table: "AspNetUserClaims",
                newName: "IX_AspNetUserClaims_UserId");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "AspNetRoles",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "AspNetRoles",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "normalized_name",
                table: "AspNetRoles",
                newName: "NormalizedName");

            migrationBuilder.RenameColumn(
                name: "concurrency_stamp",
                table: "AspNetRoles",
                newName: "ConcurrencyStamp");

            migrationBuilder.RenameIndex(
                name: "role_name_index",
                table: "AspNetRoles",
                newName: "RoleNameIndex");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "AspNetRoleClaims",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "role_id",
                table: "AspNetRoleClaims",
                newName: "RoleId");

            migrationBuilder.RenameColumn(
                name: "claim_value",
                table: "AspNetRoleClaims",
                newName: "ClaimValue");

            migrationBuilder.RenameColumn(
                name: "claim_type",
                table: "AspNetRoleClaims",
                newName: "ClaimType");

            migrationBuilder.RenameIndex(
                name: "ix_asp_net_role_claims_role_id",
                table: "AspNetRoleClaims",
                newName: "IX_AspNetRoleClaims_RoleId");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "access_log",
                newName: "access_log_id");

            migrationBuilder.RenameIndex(
                name: "ix_access_logs_user_id",
                table: "access_log",
                newName: "IX_access_log_user_id");

            migrationBuilder.RenameIndex(
                name: "ix_access_logs_device_id",
                table: "access_log",
                newName: "IX_access_log_device_id");

            migrationBuilder.AddColumn<string>(
                name: "equipment",
                table: "space",
                type: "character varying(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "has_air_conditioning",
                table: "space",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "has_projector",
                table: "space",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "has_whiteboard",
                table: "space",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "has_wifi",
                table: "space",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "image_url",
                table: "space",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "room_number",
                table: "space",
                type: "character varying(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "device_name",
                table: "device",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.AddColumn<string>(
                name: "device_identifier",
                table: "device",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "last_heartbeat",
                table: "device",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "user_id",
                table: "booking",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "phone_number",
                table: "user",
                type: "character varying(30)",
                maxLength: 30,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(30)",
                oldMaxLength: 30,
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "user",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValueSql: "CURRENT_TIMESTAMP");

            migrationBuilder.AlterColumn<string>(
                name: "GoogleId",
                table: "user",
                type: "character varying(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(255)",
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "user",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValueSql: "CURRENT_TIMESTAMP");

            migrationBuilder.AddColumn<string>(
                name: "PictureUrl",
                table: "user",
                type: "character varying(400)",
                maxLength: 400,
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "user_id",
                table: "access_log",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "device_id",
                table: "access_log",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BookingId",
                table: "access_log",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "space_id",
                table: "access_log",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_user_refresh_token",
                table: "user_refresh_token",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_subscription",
                table: "subscription",
                column: "subscription_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_subscription_plan",
                table: "subscription_plan",
                column: "subscription_plan_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_space",
                table: "space",
                column: "space_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_device",
                table: "device",
                column: "device_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_booking",
                table: "booking",
                column: "booking_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_user",
                table: "user",
                column: "user_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AspNetUserTokens",
                table: "AspNetUserTokens",
                columns: new[] { "UserId", "LoginProvider", "Name" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_AspNetUserRoles",
                table: "AspNetUserRoles",
                columns: new[] { "UserId", "RoleId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_AspNetUserLogins",
                table: "AspNetUserLogins",
                columns: new[] { "LoginProvider", "ProviderKey" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_AspNetUserClaims",
                table: "AspNetUserClaims",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AspNetRoles",
                table: "AspNetRoles",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AspNetRoleClaims",
                table: "AspNetRoleClaims",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_access_log",
                table: "access_log",
                column: "access_log_id");

            migrationBuilder.CreateTable(
                name: "payment",
                columns: table => new
                {
                    payment_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    booking_id = table.Column<int>(type: "integer", nullable: true),
                    subscription_id = table.Column<int>(type: "integer", nullable: true),
                    user_id = table.Column<int>(type: "integer", nullable: false),
                    amount = table.Column<decimal>(type: "numeric(10,2)", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    failure_reason = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    processed_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    status = table.Column<int>(type: "integer", nullable: false),
                    transaction_id = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_payment", x => x.payment_id);
                    table.ForeignKey(
                        name: "FK_payment_booking_booking_id",
                        column: x => x.booking_id,
                        principalTable: "booking",
                        principalColumn: "booking_id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_payment_subscription_subscription_id",
                        column: x => x.subscription_id,
                        principalTable: "subscription",
                        principalColumn: "subscription_id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_payment_user_user_id",
                        column: x => x.user_id,
                        principalTable: "user",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_access_log_BookingId",
                table: "access_log",
                column: "BookingId");

            migrationBuilder.CreateIndex(
                name: "IX_access_log_space_id",
                table: "access_log",
                column: "space_id");

            migrationBuilder.CreateIndex(
                name: "IX_payment_booking_id",
                table: "payment",
                column: "booking_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_payment_subscription_id",
                table: "payment",
                column: "subscription_id");

            migrationBuilder.CreateIndex(
                name: "IX_payment_user_id",
                table: "payment",
                column: "user_id");

            migrationBuilder.AddForeignKey(
                name: "FK_access_log_booking_BookingId",
                table: "access_log",
                column: "BookingId",
                principalTable: "booking",
                principalColumn: "booking_id");

            migrationBuilder.AddForeignKey(
                name: "FK_access_log_device_device_id",
                table: "access_log",
                column: "device_id",
                principalTable: "device",
                principalColumn: "device_id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_access_log_space_space_id",
                table: "access_log",
                column: "space_id",
                principalTable: "space",
                principalColumn: "space_id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_access_log_user_user_id",
                table: "access_log",
                column: "user_id",
                principalTable: "user",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId",
                principalTable: "AspNetRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserClaims_user_UserId",
                table: "AspNetUserClaims",
                column: "UserId",
                principalTable: "user",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserLogins_user_UserId",
                table: "AspNetUserLogins",
                column: "UserId",
                principalTable: "user",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId",
                principalTable: "AspNetRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserRoles_user_UserId",
                table: "AspNetUserRoles",
                column: "UserId",
                principalTable: "user",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserTokens_user_UserId",
                table: "AspNetUserTokens",
                column: "UserId",
                principalTable: "user",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_booking_space_space_id",
                table: "booking",
                column: "space_id",
                principalTable: "space",
                principalColumn: "space_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_booking_user_user_id",
                table: "booking",
                column: "user_id",
                principalTable: "user",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_device_space_space_id",
                table: "device",
                column: "space_id",
                principalTable: "space",
                principalColumn: "space_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_subscription_subscription_plan_subscription_plan_id",
                table: "subscription",
                column: "subscription_plan_id",
                principalTable: "subscription_plan",
                principalColumn: "subscription_plan_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_subscription_user_user_id",
                table: "subscription",
                column: "user_id",
                principalTable: "user",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_user_refresh_token_user_user_id",
                table: "user_refresh_token",
                column: "user_id",
                principalTable: "user",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
