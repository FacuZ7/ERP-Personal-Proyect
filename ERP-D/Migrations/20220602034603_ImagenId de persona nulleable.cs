using Microsoft.EntityFrameworkCore.Migrations;

namespace ERP_D.Migrations
{
    public partial class ImagenIddepersonanulleable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Personas_Imagenes_ImagenId",
                table: "Personas");

            migrationBuilder.AddForeignKey(
                name: "FK_Personas_Imagenes_ImagenId",
                table: "Personas",
                column: "ImagenId",
                principalTable: "Imagenes",
                principalColumn: "ImagenId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Personas_Imagenes_ImagenId",
                table: "Personas");

            migrationBuilder.AddForeignKey(
                name: "FK_Personas_Imagenes_ImagenId",
                table: "Personas",
                column: "ImagenId",
                principalTable: "Imagenes",
                principalColumn: "ImagenId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
