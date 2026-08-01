using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LojaPedidos.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCpfComprador : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Cpf",
                table: "Compradores",
                type: "nvarchar(11)",
                maxLength: 11,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Compradores_Cpf",
                table: "Compradores",
                column: "Cpf",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Compradores_Cpf",
                table: "Compradores");

            migrationBuilder.DropColumn(
                name: "Cpf",
                table: "Compradores");
        }
    }
}
