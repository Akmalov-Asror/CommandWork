using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TestProject.Migrations
{
    /// <inheritdoc />
    public partial class added : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9b73d691-67fa-4210-b0d3-6f07e4cc3499");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b7a22967-654a-44f6-881a-597abcdf0f62");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "44658504-727f-47c8-a3c3-70cdd2be663f", null, "ADMIN", "ADMIN" },
                    { "f8b1e839-62f9-47bb-9f9e-d5f6d8463df7", null, "USER", "USER" }
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "b74ddd14-6340-4840-95c2-db12554843e5",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "907ab009-f177-426a-8b9b-e94c36d9cb09", "AQAAAAIAAYagAAAAEEzN1yPvAdVyLwrayEtqel6MYJg5g3Uq6QLa8DZYSVYvjtpxk8FrUeAaZv/0SZ9PXA==", "3a05f8ce-0d1f-4161-9036-472ce83bc56d" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "44658504-727f-47c8-a3c3-70cdd2be663f");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f8b1e839-62f9-47bb-9f9e-d5f6d8463df7");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "9b73d691-67fa-4210-b0d3-6f07e4cc3499", null, "USER", "USER" },
                    { "b7a22967-654a-44f6-881a-597abcdf0f62", null, "ADMIN", "ADMIN" }
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "b74ddd14-6340-4840-95c2-db12554843e5",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "b3ccf4c6-43aa-4679-bc42-2a708a4cd857", "AQAAAAIAAYagAAAAEJmnrlgnAGfy/jmnspXVhMsNfCqQsANm3G+OWasttZ9XgxjXxNYuuFSUTBXzpPlEQA==", "03a9d742-81df-4f8e-92b8-486d71b04047" });
        }
    }
}
