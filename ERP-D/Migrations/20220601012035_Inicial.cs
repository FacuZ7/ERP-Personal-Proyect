using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ERP_D.Migrations
{
    public partial class Inicial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Imagenes",
                columns: table => new
                {
                    ImagenId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(maxLength: 65, nullable: false),
                    Path = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Imagenes", x => x.ImagenId);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    Discriminator = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Empresas",
                columns: table => new
                {
                    EmpresaId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(maxLength: 80, nullable: false),
                    Rubro = table.Column<string>(maxLength: 50, nullable: false),
                    Direccion = table.Column<string>(maxLength: 500, nullable: false),
                    EmailContacto = table.Column<string>(maxLength: 320, nullable: false),
                    Telefono = table.Column<string>(maxLength: 21, nullable: false),
                    ImagenId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Empresas", x => x.EmpresaId);
                    table.ForeignKey(
                        name: "FK_Empresas_Imagenes_ImagenId",
                        column: x => x.ImagenId,
                        principalTable: "Imagenes",
                        principalColumn: "ImagenId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<int>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Gastos",
                columns: table => new
                {
                    GastoId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Descripcion = table.Column<string>(maxLength: 120, nullable: false),
                    Monto = table.Column<double>(nullable: false),
                    FechaGasto = table.Column<DateTime>(nullable: false),
                    EmpleadoID = table.Column<int>(nullable: false),
                    CentroDeCostoId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Gastos", x => x.GastoId);

                });

            migrationBuilder.CreateTable(
                name: "Gerencias",
                columns: table => new
                {
                    GerenciaId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(maxLength: 100, nullable: false),
                    EsGerenciaGeneral = table.Column<bool>(nullable: false),
                    EmpresaId = table.Column<int>(nullable: false),
                    PosicionResponsableId = table.Column<int>(nullable: true),
                    GerenciaSuperiorId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Gerencias", x => x.GerenciaId);
                    table.ForeignKey(
                        name: "FK_Gerencias_Empresas_EmpresaId",
                        column: x => x.EmpresaId,
                        principalTable: "Empresas",
                        principalColumn: "EmpresaId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Gerencias_Gerencias_GerenciaSuperiorId",
                        column: x => x.GerenciaSuperiorId,
                        principalTable: "Gerencias",
                        principalColumn: "GerenciaId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CentrosDeCosto",
                columns: table => new
                {
                    CentroDeCostoId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(maxLength: 120, nullable: false),
                    MontoMaximo = table.Column<double>(nullable: false),
                    GerenciaId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CentrosDeCosto", x => x.CentroDeCostoId);
                    table.ForeignKey(
                        name: "FK_CentrosDeCosto_Gerencias_GerenciaId",
                        column: x => x.GerenciaId,
                        principalTable: "Gerencias",
                        principalColumn: "GerenciaId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Posiciones",
                columns: table => new
                {
                    PosicionId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(maxLength: 100, nullable: false),
                    Descripcion = table.Column<string>(maxLength: 8000, nullable: true),
                    Sueldo = table.Column<double>(nullable: false),
                    GerenciaId = table.Column<int>(nullable: false),
                    ResponsableId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Posiciones", x => x.PosicionId);
                    table.ForeignKey(
                        name: "FK_Posiciones_Gerencias_GerenciaId",
                        column: x => x.GerenciaId,
                        principalTable: "Gerencias",
                        principalColumn: "GerenciaId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Posiciones_Posiciones_ResponsableId",
                        column: x => x.ResponsableId,
                        principalTable: "Posiciones",
                        principalColumn: "PosicionId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Personas",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(maxLength: 256, nullable: true),
                    UserName = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    PasswordHash = table.Column<string>(nullable: true),
                    SecurityStamp = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    TwoFactorEnabled = table.Column<bool>(nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
                    LockoutEnabled = table.Column<bool>(nullable: false),
                    AccessFailedCount = table.Column<int>(nullable: false),
                    Discriminator = table.Column<string>(nullable: false),
                    Nombre = table.Column<string>(maxLength: 30, nullable: true),
                    Apellido = table.Column<string>(maxLength: 30, nullable: true),
                    DNI = table.Column<string>(maxLength: 10, nullable: true),
                    Direccion = table.Column<string>(nullable: true),
                    FechaAlta = table.Column<DateTime>(nullable: true),
                    ImagenId = table.Column<int>(nullable: true),
                    PosicionId = table.Column<int>(nullable: true),
                    ObraSocial = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Personas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Personas_Posiciones_PosicionId",
                        column: x => x.PosicionId,
                        principalTable: "Posiciones",
                        principalColumn: "PosicionId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Personas_Imagenes_ImagenId",
                        column: x => x.ImagenId,
                        principalTable: "Imagenes",
                        principalColumn: "ImagenId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_Personas_UserId",
                        column: x => x.UserId,
                        principalTable: "Personas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(nullable: false),
                    ProviderKey = table.Column<string>(nullable: false),
                    ProviderDisplayName = table.Column<string>(nullable: true),
                    UserId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_Personas_UserId",
                        column: x => x.UserId,
                        principalTable: "Personas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<int>(nullable: false),
                    LoginProvider = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_Personas_UserId",
                        column: x => x.UserId,
                        principalTable: "Personas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PersonasRoles",
                columns: table => new
                {
                    UserId = table.Column<int>(nullable: false),
                    RoleId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonasRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_PersonasRoles_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PersonasRoles_Personas_UserId",
                        column: x => x.UserId,
                        principalTable: "Personas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Telefonos",
                columns: table => new
                {
                    TelefonoId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Numero = table.Column<string>(maxLength: 21, nullable: false),
                    TipoTelefono = table.Column<int>(nullable: false),
                    EmpleadoId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Telefonos", x => x.TelefonoId);
                    table.ForeignKey(
                        name: "FK_Telefonos_Personas_EmpleadoId",
                        column: x => x.EmpleadoId,
                        principalTable: "Personas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_CentrosDeCosto_GerenciaId",
                table: "CentrosDeCosto",
                column: "GerenciaId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Empresas_ImagenId",
                table: "Empresas",
                column: "ImagenId");

            migrationBuilder.CreateIndex(
                name: "IX_Gastos_CentroDeCostoId",
                table: "Gastos",
                column: "CentroDeCostoId");

            migrationBuilder.CreateIndex(
                name: "IX_Gastos_EmpleadoID",
                table: "Gastos",
                column: "EmpleadoID");

            migrationBuilder.CreateIndex(
                name: "IX_Gerencias_EmpresaId",
                table: "Gerencias",
                column: "EmpresaId");

            migrationBuilder.CreateIndex(
                name: "IX_Gerencias_GerenciaSuperiorId",
                table: "Gerencias",
                column: "GerenciaSuperiorId");

            migrationBuilder.CreateIndex(
                name: "IX_Gerencias_PosicionResponsableId",
                table: "Gerencias",
                column: "PosicionResponsableId");

            migrationBuilder.CreateIndex(
                name: "IX_Personas_PosicionId",
                table: "Personas",
                column: "PosicionId",
                unique: true,
                filter: "[PosicionId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Personas_ImagenId",
                table: "Personas",
                column: "ImagenId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "Personas",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "Personas",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_PersonasRoles_RoleId",
                table: "PersonasRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Posiciones_GerenciaId",
                table: "Posiciones",
                column: "GerenciaId");

            migrationBuilder.CreateIndex(
                name: "IX_Posiciones_ResponsableId",
                table: "Posiciones",
                column: "ResponsableId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "Roles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Telefonos_EmpleadoId",
                table: "Telefonos",
                column: "EmpleadoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Gastos_Personas_EmpleadoID",
                table: "Gastos",
                column: "EmpleadoID",
                principalTable: "Personas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Gastos_CentrosDeCosto_CentroDeCostoId",
                table: "Gastos",
                column: "CentroDeCostoId",
                principalTable: "CentrosDeCosto",
                principalColumn: "CentroDeCostoId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Gerencias_Posiciones_PosicionResponsableId",
                table: "Gerencias",
                column: "PosicionResponsableId",
                principalTable: "Posiciones",
                principalColumn: "PosicionId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Posiciones_Gerencias_GerenciaId",
                table: "Posiciones");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "Gastos");

            migrationBuilder.DropTable(
                name: "PersonasRoles");

            migrationBuilder.DropTable(
                name: "Telefonos");

            migrationBuilder.DropTable(
                name: "CentrosDeCosto");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Personas");

            migrationBuilder.DropTable(
                name: "Gerencias");

            migrationBuilder.DropTable(
                name: "Empresas");

            migrationBuilder.DropTable(
                name: "Posiciones");

            migrationBuilder.DropTable(
                name: "Imagenes");
        }
    }
}
