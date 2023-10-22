using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace challenge.ibge.infra.data.Migrations
{
    /// <inheritdoc />
    public partial class Removed_UniqueKey_City : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_tb_ibge_City",
                table: "tb_ibge");

            migrationBuilder.AlterColumn<string>(
                name: "City",
                table: "tb_ibge",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(255)")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "City",
                table: "tb_ibge",
                type: "varchar(255)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_tb_ibge_City",
                table: "tb_ibge",
                column: "City",
                unique: true);
        }
    }
}
