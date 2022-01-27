using Microsoft.EntityFrameworkCore.Migrations;

namespace pujcovna.Data.Migrations
{
    public partial class Add_testing_categories : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "CategoryId", "Hidden", "OrderNo", "ParentCategoryId", "Title", "Url" },
                values: new object[] { 1, false, 1, null, "Vrtací a bourací kladiva", "vrtaci-a-bouraci-kladiva" });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "CategoryId", "Hidden", "OrderNo", "ParentCategoryId", "Title", "Url" },
                values: new object[] { 2, false, 2, null, "Řezání a ohýbání", "rezani-a-ohybani" });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "CategoryId", "Hidden", "OrderNo", "ParentCategoryId", "Title", "Url" },
                values: new object[] { 3, false, 7, null, "Lešení a bednění", "leseni-a-bedneni" });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "CategoryId", "Hidden", "OrderNo", "ParentCategoryId", "Title", "Url" },
                values: new object[,]
                {
                    { 4, false, 3, 2, "Pily", "pily" },
                    { 5, false, 4, 2, "Profilovačky, falcovačky", "profilovacky-falcovacky" },
                    { 6, false, 5, 2, "Řezačky", "rezacky" },
                    { 7, false, 6, 2, "Úhlové brusky", "uhlove-brusky" },
                    { 8, false, 8, 3, "Bednění a stavební podpěry", "bedneni-a-stavebni-podpery" },
                    { 9, false, 9, 3, "Lešení a lešenové věže", "leseni-a-lesenove-veze" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 3);
        }
    }
}
