using Microsoft.EntityFrameworkCore.Migrations;

namespace AspNet_UploadImagem.Migrations
{
    public partial class Inserts_StatusTransacao : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                SET IDENTITY_INSERT StatusTransacao ON
                INSERT INTO StatusTransacao (ID,Nome) VALUES (1,'Concluída');
                INSERT INTO StatusTransacao (ID,Nome) VALUES (2,'Cancelada');
                SET IDENTITY_INSERT StatusTransacao OFF
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                DELETE FROM StatusTransacao;
            ");
        }
    }
}
