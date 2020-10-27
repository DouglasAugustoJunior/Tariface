using Microsoft.EntityFrameworkCore.Migrations;

namespace AspNet_UploadImagem.Migrations
{
    public partial class Inserts_TipoTransacao : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                SET IDENTITY_INSERT TipoTransacao ON
                INSERT INTO TipoTransacao (ID,Nome) VALUES (1,'Crédito');
                INSERT INTO TipoTransacao (ID,Nome) VALUES (2,'Débito');
                SET IDENTITY_INSERT TipoTransacao OFF
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                DELETE FROM TipoTransacao;
            ");
        }
    }
}
