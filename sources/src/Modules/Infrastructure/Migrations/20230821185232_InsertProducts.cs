using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InsertProducts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"insert into public.""AvailableProducts"" (""ProductId"", ""StockLevel"") 
values (1,20), (2,20), (3,20), (4,20), (5, 20), (6,20), (7,20), (8,20), (9,20), (10, 20)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
