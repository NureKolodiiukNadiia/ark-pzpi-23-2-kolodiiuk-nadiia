using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SpotRent.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddOwnerToSubscriptionPlan : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "transaction_id",
                table: "subscriptions",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddColumn<int>(
                name: "owner_id",
                table: "subscription_plans",
                type: "integer",
                nullable: false);

            migrationBuilder.CreateIndex(
                name: "ix_subscription_plans_owner_id",
                table: "subscription_plans",
                column: "owner_id");

            migrationBuilder.AddForeignKey(
                name: "fk_subscription_plans_asp_net_users_owner_id",
                table: "subscription_plans",
                column: "owner_id",
                principalTable: "asp_net_users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_subscription_plans_asp_net_users_owner_id",
                table: "subscription_plans");

            migrationBuilder.DropIndex(
                name: "ix_subscription_plans_owner_id",
                table: "subscription_plans");

            migrationBuilder.DropColumn(
                name: "owner_id",
                table: "subscription_plans");

            migrationBuilder.AlterColumn<long>(
                name: "transaction_id",
                table: "subscriptions",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);
        }
    }
}
