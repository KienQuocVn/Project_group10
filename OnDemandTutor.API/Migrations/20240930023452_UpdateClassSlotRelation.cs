using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnDemandTutor.API.Migrations
{
    /// <inheritdoc />
    public partial class UpdateClassSlotRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Feedbacks_Slots_SlotId",
                table: "Feedbacks");

            migrationBuilder.DropForeignKey(
                name: "FK_Schedules_Slots_SlotId",
                table: "Schedules");

            migrationBuilder.AlterColumn<string>(
                name: "SlotId",
                table: "Schedules",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "SlotId",
                table: "Feedbacks",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Feedbacks_Slots_SlotId",
                table: "Feedbacks",
                column: "SlotId",
                principalTable: "Slots",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Schedules_Slots_SlotId",
                table: "Schedules",
                column: "SlotId",
                principalTable: "Slots",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Feedbacks_Slots_SlotId",
                table: "Feedbacks");

            migrationBuilder.DropForeignKey(
                name: "FK_Schedules_Slots_SlotId",
                table: "Schedules");

            migrationBuilder.AlterColumn<string>(
                name: "SlotId",
                table: "Schedules",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "SlotId",
                table: "Feedbacks",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddForeignKey(
                name: "FK_Feedbacks_Slots_SlotId",
                table: "Feedbacks",
                column: "SlotId",
                principalTable: "Slots",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Schedules_Slots_SlotId",
                table: "Schedules",
                column: "SlotId",
                principalTable: "Slots",
                principalColumn: "Id");
        }
    }
}
