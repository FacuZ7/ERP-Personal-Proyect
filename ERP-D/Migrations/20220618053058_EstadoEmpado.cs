using Microsoft.EntityFrameworkCore.Migrations;

namespace ERP_D.Migrations
{
    public partial class EstadoEmpado : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "EstadoEmpleado",
                table: "Personas",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EstadoEmpleado",
                table: "Personas");
        }
    }
}
