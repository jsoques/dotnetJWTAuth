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

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "ActivateKey", "DateCreated", "Name", "PasswordHash", "Status" },
                values: new object[] { 1, "", "12/13/2020 2:19:29 PM", "admin@admin.com", "AQAAAAEAACcQAAAAEEuZMQSPyBWSA+9sPwLsJvEeL3wMoqj2XFuPs8dfappQ0AXbs9cRzN9/+Cb76U+j4g==", 1 });

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
