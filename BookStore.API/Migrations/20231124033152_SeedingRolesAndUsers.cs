using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BookStore.API.Migrations
{
    /// <inheritdoc />
    public partial class SeedingRolesAndUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "223f9400-50fc-4e38-9195-1ef87b0fb751", null, "Administrator", "ADMINISTRATOR" },
                    { "ce392ebd-fdf9-4d12-a950-75db07abc220", null, "User", "USER" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "1c1d9951-3588-4a41-a061-b6b75491f740", 0, "03d6ba47-3488-49aa-be63-6967a0561b02", "user@bookstore.com", false, "System", "User", false, null, "USER@BOOKSTORE.COM", "USER@BOOKSTORE.COM", "AQAAAAIAAYagAAAAEAudRjxMqoUDe0mtcTooJnf66OetpFYM4fN9ZuvfJ/D240CGypM/weKOKfuSVrxjpw==", null, false, "174681aa-6501-4553-b70b-e5c99f26a47c", false, "user@bookstore.com" },
                    { "41be71ef-cc46-4a39-9e3e-4efd48aa052e", 0, "0e8dc20a-845d-4eca-bd11-cf815893f231", "admin@bookstore.com", false, "System", "Admin", false, null, "ADMIN@BOOKSTORE.COM", "ADMIN@BOOKSTORE.COM", "AQAAAAIAAYagAAAAEDkJtMleLMjT01CiWNZqOVws6kfvcZOTyGHjRmIMZs45NKqcuoIvdIQMqn9l2V9DOQ==", null, false, "6f55d845-d7a1-40dc-90c1-c64e5fa2fa73", false, "admin@bookstore.com" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { "ce392ebd-fdf9-4d12-a950-75db07abc220", "1c1d9951-3588-4a41-a061-b6b75491f740" },
                    { "223f9400-50fc-4e38-9195-1ef87b0fb751", "41be71ef-cc46-4a39-9e3e-4efd48aa052e" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "ce392ebd-fdf9-4d12-a950-75db07abc220", "1c1d9951-3588-4a41-a061-b6b75491f740" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "223f9400-50fc-4e38-9195-1ef87b0fb751", "41be71ef-cc46-4a39-9e3e-4efd48aa052e" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "223f9400-50fc-4e38-9195-1ef87b0fb751");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ce392ebd-fdf9-4d12-a950-75db07abc220");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1c1d9951-3588-4a41-a061-b6b75491f740");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "41be71ef-cc46-4a39-9e3e-4efd48aa052e");
        }
    }
}
