using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Meetups.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateRsvp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PaymentId",
                table: "Rsvps",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PaymentStatus",
                table: "Rsvps",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RefundId",
                table: "Rsvps",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RefundStatus",
                table: "Rsvps",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PaymentId",
                table: "Rsvps");

            migrationBuilder.DropColumn(
                name: "PaymentStatus",
                table: "Rsvps");

            migrationBuilder.DropColumn(
                name: "RefundId",
                table: "Rsvps");

            migrationBuilder.DropColumn(
                name: "RefundStatus",
                table: "Rsvps");
        }
    }
}
