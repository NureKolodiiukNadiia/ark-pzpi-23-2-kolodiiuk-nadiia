using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace SpotRent.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AllChangesMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "payments");

            migrationBuilder.RenameColumn(
                name: "booking_type",
                table: "bookings",
                newName: "payment_status");

            migrationBuilder.AddColumn<DateTime>(
                name: "cancelled_at",
                table: "subscriptions",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "payment_created_at",
                table: "subscriptions",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "payment_failure_reason",
                table: "subscriptions",
                type: "text",
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
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<long>(
                name: "transaction_id",
                table: "subscriptions",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<bool>(
                name: "is_available",
                table: "spaces",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "updated_at",
                table: "spaces",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "payment_created_at",
                table: "bookings",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "payment_failure_reason",
                table: "bookings",
                type: "text",
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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
                name: "is_available",
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

            migrationBuilder.RenameColumn(
                name: "payment_status",
                table: "bookings",
                newName: "booking_type");

            migrationBuilder.CreateTable(
                name: "payments",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    booking_id = table.Column<int>(type: "integer", nullable: true),
                    subscription_id = table.Column<int>(type: "integer", nullable: true),
                    amount = table.Column<decimal>(type: "numeric(10,2)", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    failure_reason = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    processed_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    status = table.Column<int>(type: "integer", nullable: false),
                    transaction_id = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_payments", x => x.id);
                    table.ForeignKey(
                        name: "fk_payments_bookings_booking_id",
                        column: x => x.booking_id,
                        principalTable: "bookings",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "fk_payments_subscriptions_subscription_id",
                        column: x => x.subscription_id,
                        principalTable: "subscriptions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "ix_payments_booking_id",
                table: "payments",
                column: "booking_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_payments_subscription_id",
                table: "payments",
                column: "subscription_id");
        }
    }
}
