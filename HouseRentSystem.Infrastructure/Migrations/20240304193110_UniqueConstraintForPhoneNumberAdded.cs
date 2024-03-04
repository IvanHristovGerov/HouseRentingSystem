using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HouseRentSystem.Infrastructure.Migrations
{
    public partial class UniqueConstraintForPhoneNumberAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterTable(
                name: "Agents",
                comment: "House Agent");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "6d5800ce-d726-4fc8-83d9-d6b3ac1f591e",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "8dada09b-8a93-4eaa-b6a3-c6a4b709cbad", "AQAAAAEAACcQAAAAEEFS24LtPWv9M96orx3ys2woBysot+GjrF13Wmc8Tl9O0PzxeuHMvIcKTjS249nZlg==", "10d6caf0-cdf0-42be-b599-973b3bf5842b" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "dea12856-c198-4129-b3f3-b893d8395082",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "6ff7bbbd-962f-4857-a062-f9ac534426f3", "AQAAAAEAACcQAAAAED97rihkIfxgzU+zmgDp/pL5A2fMShXXmIqdTA49RZX4jEaX5TNmxiAqp2oy9q8u2g==", "84e0494e-79d4-4e31-a199-358b7c55dfd7" });

            migrationBuilder.CreateIndex(
                name: "IX_Agents_PhoneNumber",
                table: "Agents",
                column: "PhoneNumber",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Agents_PhoneNumber",
                table: "Agents");

            migrationBuilder.AlterTable(
                name: "Agents",
                oldComment: "House Agent");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "6d5800ce-d726-4fc8-83d9-d6b3ac1f591e",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "9c85b2ed-6fa8-4d37-9f56-77a16b002cdf", "AQAAAAEAACcQAAAAEOWqlXLex83v45oBzAZ+q0emh74b3XIrDpyGNx8Nn17Ukj7SAzEMIsrbLDabJJv5hw==", "159c3d63-abde-4b2f-911c-f8cbe58aa9d2" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "dea12856-c198-4129-b3f3-b893d8395082",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "ebbfec73-0341-4a55-ac5d-b403197b43d7", "AQAAAAEAACcQAAAAEI6Xpu24xPfJeg+gN8VAWCJuO21nbgHSclE+q2v7wR3zFsGUGv3z/uXt5Z0cO35c/A==", "54004b3d-5e03-4bcd-aa2e-37ecea3ea6a1" });
        }
    }
}
