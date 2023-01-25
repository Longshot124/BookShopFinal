using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookShop.Migrations
{
    public partial class BookLanguageAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BookLanguageId",
                table: "Books",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "BookLanguages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookLanguages", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Books_BookLanguageId",
                table: "Books",
                column: "BookLanguageId");

            migrationBuilder.AddForeignKey(
                name: "FK_Books_BookLanguages_BookLanguageId",
                table: "Books",
                column: "BookLanguageId",
                principalTable: "BookLanguages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Books_BookLanguages_BookLanguageId",
                table: "Books");

            migrationBuilder.DropTable(
                name: "BookLanguages");

            migrationBuilder.DropIndex(
                name: "IX_Books_BookLanguageId",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "BookLanguageId",
                table: "Books");
        }
    }
}
