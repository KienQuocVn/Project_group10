using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnDemandTutor.API.Migrations
{
    /// <inheritdoc />
    public partial class UpdateClass : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Classes_AspNetUsers_TutorId",
                table: "Classes");

            migrationBuilder.RenameColumn(
                name: "TutorId",
                table: "Classes",
                newName: "AccountId");

            migrationBuilder.RenameIndex(
                name: "IX_Classes_TutorId",
                table: "Classes",
                newName: "IX_Classes_AccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_Classes_AspNetUsers_AccountId",
                table: "Classes",
                column: "AccountId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Classes_AspNetUsers_AccountId",
                table: "Classes");

            migrationBuilder.RenameColumn(
                name: "AccountId",
                table: "Classes",
                newName: "TutorId");

            migrationBuilder.RenameIndex(
                name: "IX_Classes_AccountId",
                table: "Classes",
                newName: "IX_Classes_TutorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Classes_AspNetUsers_TutorId",
                table: "Classes",
                column: "TutorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
