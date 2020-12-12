using Microsoft.EntityFrameworkCore.Migrations;

namespace JWTAuth.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "text(256)", nullable: false),
                    PasswordHash = table.Column<string>(type: "text(124)", nullable: false),
                    Status = table.Column<int>(type: "INTEGER", nullable: false),
                    DateCreated = table.Column<string>(type: "text(25)", nullable: false),
                    ActivateKey = table.Column<string>(type: "text(32)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_Name",
                table: "Users",
                column: "Name",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
