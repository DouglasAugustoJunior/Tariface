using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AspNet_UploadImagem.Migrations
{
    public partial class Creates_Iniciais : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.CreateTable(
                name: "GrupoPessoa",
                schema: "dbo",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(nullable: true),
                    IDAzure = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GrupoPessoa", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Regiao",
                schema: "dbo",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Regiao", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "StatusTransacao",
                schema: "dbo",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StatusTransacao", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "TipoTransacao",
                schema: "dbo",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TipoTransacao", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "UF",
                schema: "dbo",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(nullable: true),
                    Sigla = table.Column<string>(nullable: true),
                    RegiaoID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UF", x => x.ID);
                    table.ForeignKey(
                        name: "FK_UF_Regiao_RegiaoID",
                        column: x => x.RegiaoID,
                        principalSchema: "dbo",
                        principalTable: "Regiao",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Municipio",
                schema: "dbo",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(nullable: true),
                    UfID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Municipio", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Municipio_UF_UfID",
                        column: x => x.UfID,
                        principalSchema: "dbo",
                        principalTable: "UF",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Endereco",
                schema: "dbo",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Logradouro = table.Column<string>(nullable: true),
                    Numero = table.Column<int>(nullable: false),
                    CEP = table.Column<int>(nullable: false),
                    MunicipioID = table.Column<int>(nullable: false),
                    Complemento = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Endereco", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Endereco_Municipio_MunicipioID",
                        column: x => x.MunicipioID,
                        principalSchema: "dbo",
                        principalTable: "Municipio",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Usuario",
                schema: "dbo",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(nullable: true),
                    CPF = table.Column<string>(nullable: false),
                    Saldo = table.Column<decimal>(type: "decimal(8,2)", nullable: false),
                    GrupoPessoaID = table.Column<int>(nullable: false),
                    PersonId = table.Column<Guid>(nullable: false),
                    EnderecoID = table.Column<int>(nullable: false),
                    Email = table.Column<string>(nullable: true),
                    Senha = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuario", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Usuario_Endereco_EnderecoID",
                        column: x => x.EnderecoID,
                        principalSchema: "dbo",
                        principalTable: "Endereco",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Usuario_GrupoPessoa_GrupoPessoaID",
                        column: x => x.GrupoPessoaID,
                        principalSchema: "dbo",
                        principalTable: "GrupoPessoa",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Cartao",
                schema: "dbo",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Numero = table.Column<int>(nullable: false),
                    Titular = table.Column<string>(nullable: true),
                    Validade = table.Column<DateTime>(nullable: false),
                    CSV = table.Column<int>(nullable: false),
                    IDUsuario = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cartao", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Cartao_Usuario_IDUsuario",
                        column: x => x.IDUsuario,
                        principalSchema: "dbo",
                        principalTable: "Usuario",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Historico",
                schema: "dbo",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IDUsuario = table.Column<int>(nullable: false),
                    Valor = table.Column<decimal>(type: "decimal(8,2)", nullable: false),
                    DataCriacao = table.Column<DateTime>(nullable: false),
                    TipoID = table.Column<int>(nullable: false),
                    StatusID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Historico", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Historico_Usuario_IDUsuario",
                        column: x => x.IDUsuario,
                        principalSchema: "dbo",
                        principalTable: "Usuario",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Historico_StatusTransacao_StatusID",
                        column: x => x.StatusID,
                        principalSchema: "dbo",
                        principalTable: "StatusTransacao",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Historico_TipoTransacao_TipoID",
                        column: x => x.TipoID,
                        principalSchema: "dbo",
                        principalTable: "TipoTransacao",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Imagem",
                schema: "dbo",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(nullable: true),
                    IDUsuario = table.Column<int>(nullable: false),
                    URL = table.Column<string>(nullable: true),
                    PersistedFaceID = table.Column<int>(nullable: true),
                    Perfil = table.Column<bool>(nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Imagem", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Imagem_Usuario_IDUsuario",
                        column: x => x.IDUsuario,
                        principalSchema: "dbo",
                        principalTable: "Usuario",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cartao_IDUsuario",
                schema: "dbo",
                table: "Cartao",
                column: "IDUsuario");

            migrationBuilder.CreateIndex(
                name: "IX_Endereco_MunicipioID",
                schema: "dbo",
                table: "Endereco",
                column: "MunicipioID");

            migrationBuilder.CreateIndex(
                name: "IX_Historico_IDUsuario",
                schema: "dbo",
                table: "Historico",
                column: "IDUsuario");

            migrationBuilder.CreateIndex(
                name: "IX_Historico_StatusID",
                schema: "dbo",
                table: "Historico",
                column: "StatusID");

            migrationBuilder.CreateIndex(
                name: "IX_Historico_TipoID",
                schema: "dbo",
                table: "Historico",
                column: "TipoID");

            migrationBuilder.CreateIndex(
                name: "IX_Imagem_IDUsuario",
                schema: "dbo",
                table: "Imagem",
                column: "IDUsuario");

            migrationBuilder.CreateIndex(
                name: "IX_Municipio_UfID",
                schema: "dbo",
                table: "Municipio",
                column: "UfID");

            migrationBuilder.CreateIndex(
                name: "IX_UF_RegiaoID",
                schema: "dbo",
                table: "UF",
                column: "RegiaoID");

            migrationBuilder.CreateIndex(
                name: "IX_Usuario_EnderecoID",
                schema: "dbo",
                table: "Usuario",
                column: "EnderecoID");

            migrationBuilder.CreateIndex(
                name: "IX_Usuario_GrupoPessoaID",
                schema: "dbo",
                table: "Usuario",
                column: "GrupoPessoaID");

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.DropTable(
                name: "Cartao",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Historico",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "StatusTransacao",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "TipoTransacao",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Usuario",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Endereco",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "GrupoPessoa",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Imagem",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Municipio",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "UF",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Regiao",
                schema: "dbo");
        }
    }
}
