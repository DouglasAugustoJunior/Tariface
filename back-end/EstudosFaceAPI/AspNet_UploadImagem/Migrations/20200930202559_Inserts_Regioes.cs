using Microsoft.EntityFrameworkCore.Migrations;

namespace AspNet_UploadImagem.Migrations
{
    public partial class Inserts_Regioes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                SET IDENTITY_INSERT Regiao ON
                INSERT INTO Regiao (Id, Nome) VALUES (1, 'Norte');
                INSERT INTO Regiao (Id, Nome) VALUES (2, 'Nordeste');
                INSERT INTO Regiao (Id, Nome) VALUES (3, 'Sudeste');
                INSERT INTO Regiao (Id, Nome) VALUES (4, 'Sul');
                INSERT INTO Regiao (Id, Nome) VALUES (5, 'Centro-Oeste');
                SET IDENTITY_INSERT Regiao OFF
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                DELETE FROM Regiao;
            ");
        }
    }
}
