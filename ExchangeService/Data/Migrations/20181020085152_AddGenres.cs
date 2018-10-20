using Microsoft.EntityFrameworkCore.Migrations;

namespace ExchangeService.Data.Migrations
{
    public partial class AddGenres : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("INSERT INTO dbo.Genres(Name) values('Strategiczna')");
            migrationBuilder.Sql("INSERT INTO dbo.Genres(Name) values('Familijna')");
            migrationBuilder.Sql("INSERT INTO dbo.Genres(Name) values('Karciana')");
            migrationBuilder.Sql("INSERT INTO dbo.Genres(Name) values('Logiczna')");
            migrationBuilder.Sql("INSERT INTO dbo.Genres(Name) values('Kooperacyjna')");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM dbo.Genres WHERE Name IN ('Strategiczna',Familijna','Karciana','Logiczna','Kooperacyjna')");
        }
    }
}
