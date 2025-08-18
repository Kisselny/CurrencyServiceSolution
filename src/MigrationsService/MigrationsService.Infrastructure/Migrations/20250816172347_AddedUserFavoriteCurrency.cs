using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MigrationsService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddedUserFavoriteCurrency : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "user_favorite_currency",
                columns: table => new
                {
                    user_id = table.Column<int>(type: "integer", nullable: false),
                    currency_name = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_favorite_currency", x => new { x.user_id, x.currency_name });
                });

            migrationBuilder.CreateIndex(
                name: "IX_user_favorite_currency_user_id",
                table: "user_favorite_currency",
                column: "user_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "user_favorite_currency");
        }
    }
}
