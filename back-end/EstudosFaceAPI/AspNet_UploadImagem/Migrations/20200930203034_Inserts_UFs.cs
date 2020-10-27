using Microsoft.EntityFrameworkCore.Migrations;

namespace AspNet_UploadImagem.Migrations
{
    public partial class Inserts_UFs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                SET IDENTITY_INSERT UF ON
                Insert into UF (ID, Nome, Sigla, RegiaoID) values (12, 'Acre', 'AC',				1);
                Insert into UF (ID, Nome, Sigla, RegiaoID) values (27, 'Alagoas', 'AL',				2);
                Insert into UF (ID, Nome, Sigla, RegiaoID) values (16, 'Amapá', 'AP',				1);
                Insert into UF (ID, Nome, Sigla, RegiaoID) values (13, 'Amazonas', 'AM',			1);
                Insert into UF (ID, Nome, Sigla, RegiaoID) values (29, 'Bahia', 'BA',				2);
                Insert into UF (ID, Nome, Sigla, RegiaoID) values (23, 'Ceará', 'CE',				2);
                Insert into UF (ID, Nome, Sigla, RegiaoID) values (53, 'Distrito Federal', 'DF',	5);
                Insert into UF (ID, Nome, Sigla, RegiaoID) values (32, 'Espírito Santo', 'ES',		3);
                Insert into UF (ID, Nome, Sigla, RegiaoID) values (52, 'Goiás', 'GO',				5);
                Insert into UF (ID, Nome, Sigla, RegiaoID) values (21, 'Maranhão', 'MA',			2);
                Insert into UF (ID, Nome, Sigla, RegiaoID) values (51, 'Mato Grosso', 'MT',			5);
                Insert into UF (ID, Nome, Sigla, RegiaoID) values (50, 'Mato Grosso do Sul', 'MS',  5);
                Insert into UF (ID, Nome, Sigla, RegiaoID) values (31, 'Minas Gerais', 'MG',		3);
                Insert into UF (ID, Nome, Sigla, RegiaoID) values (15, 'Pará', 'PA',				1);
                Insert into UF (ID, Nome, Sigla, RegiaoID) values (25, 'Paraíba', 'PB',				2);
                Insert into UF (ID, Nome, Sigla, RegiaoID) values (41, 'Paraná', 'PR',			    4);
                Insert into UF (ID, Nome, Sigla, RegiaoID) values (26, 'Pernambuco', 'PE',          2);
                Insert into UF (ID, Nome, Sigla, RegiaoID) values (22, 'Piauí', 'PI',               2);
                Insert into UF (ID, Nome, Sigla, RegiaoID) values (33, 'Rio de Janeiro', 'RJ',      3);
                Insert into UF (ID, Nome, Sigla, RegiaoID) values (24, 'Rio Grande do Norte', 'RN', 2);
                Insert into UF (ID, Nome, Sigla, RegiaoID) values (43, 'Rio Grande do Sul', 'RS',   4);
                Insert into UF (ID, Nome, Sigla, RegiaoID) values (11, 'Rondônia', 'RO',            1);
                Insert into UF (ID, Nome, Sigla, RegiaoID) values (14, 'Roraima', 'RR',             1);
                Insert into UF (ID, Nome, Sigla, RegiaoID) values (42, 'Santa Catarina', 'SC',		4);
                Insert into UF (ID, Nome, Sigla, RegiaoID) values (35, 'São Paulo', 'SP',			3);
                Insert into UF (ID, Nome, Sigla, RegiaoID) values (28, 'Sergipe', 'SE',			    2);
                Insert into UF (ID, Nome, Sigla, RegiaoID) values (17, 'Tocantins', 'TO',			1);
                SET IDENTITY_INSERT UF OFF
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                DELETE FROM UF;
            ");
        }
    }
}
