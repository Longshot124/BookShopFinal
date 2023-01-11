using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookShop.Migrations
{
    public partial class SliderUpdated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OldPrice",
                table: "Sliders");

            migrationBuilder.DropColumn(
                name: "SubTitle",
                table: "Sliders");

            migrationBuilder.RenameColumn(
                name: "Title",
                table: "Sliders",
                newName: "ButtonText");

            migrationBuilder.RenameColumn(
                name: "Price",
                table: "Sliders",
                newName: "BookId");

            migrationBuilder.CreateIndex(
                name: "IX_Sliders_BookId",
                table: "Sliders",
                column: "BookId");

            migrationBuilder.AddForeignKey(
                name: "FK_Sliders_Books_BookId",
                table: "Sliders",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sliders_Books_BookId",
                table: "Sliders");

            migrationBuilder.DropIndex(
                name: "IX_Sliders_BookId",
                table: "Sliders");

            migrationBuilder.RenameColumn(
                name: "ButtonText",
                table: "Sliders",
                newName: "Title");

            migrationBuilder.RenameColumn(
                name: "BookId",
                table: "Sliders",
                newName: "Price");

            migrationBuilder.AddColumn<int>(
                name: "OldPrice",
                table: "Sliders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "SubTitle",
                table: "Sliders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
