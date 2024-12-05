using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DesafioFSBR.Migrations
{
    /// <inheritdoc />
    public partial class RemoveCodigoMunicipio : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CodigoMunicipio",
                table: "Processos");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CodigoMunicipio",
                table: "Processos",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }
    }
}
