using Microsoft.EntityFrameworkCore.Migrations;

namespace AspNet_UploadImagem.Migrations
{
    public partial class Muda_Numero_Cartao : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                ALTER TABLE CARTAO ALTER COLUMN Numero VARCHAR(16);
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                ALTER TABLE CARTAO ALTER COLUMN Numero Int;
            ");
        }
    }
}
