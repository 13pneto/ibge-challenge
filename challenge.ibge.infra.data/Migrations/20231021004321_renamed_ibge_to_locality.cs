using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace challenge.ibge.infra.data.Migrations
{
    /// <inheritdoc />
    public partial class renamed_ibge_to_locality : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tb_ibge");

            migrationBuilder.CreateTable(
                name: "tb_locality",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IbgeCode = table.Column<int>(type: "int", nullable: false),
                    City = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UF = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_locality", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_tb_locality_IbgeCode",
                table: "tb_locality",
                column: "IbgeCode",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tb_locality");

            migrationBuilder.CreateTable(
                name: "tb_ibge",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    City = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Code = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: false),
                    UF = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_ibge", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_tb_ibge_Code",
                table: "tb_ibge",
                column: "Code",
                unique: true);
        }
    }
}
