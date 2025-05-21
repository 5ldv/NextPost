using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NextPost.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedDefaultRolesAndAdminUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                schema: "security",
                table: "Roles",
                columns: new[] { "Name", "NormalizedName", "ConcurrencyStamp" },
                values: new object[,]
                {
                {
                    "Admin",
                    "ADMIN",
                    Guid.NewGuid().ToString()
                },
                {
                    "Author",
                    "AUTHOR",
                    Guid.NewGuid().ToString()
                }
                });

            migrationBuilder.InsertData(
                schema: "security",
                table: "Users",
                columns: new[] {
                    "UserName",
                    "NormalizedUserName",
                    "Email",
                    "NormalizedEmail",
                    "EmailConfirmed",
                    "PhoneNumberConfirmed",
                    "TwoFactorEnabled",
                    "LockoutEnabled",
                    "PasswordHash",
                    "SecurityStamp",
                    "ConcurrencyStamp",
                    "AccessFailedCount",
                },
                values: new object[,]
                {
                    {
                        "Admin",
                        "ADMIN",
                        "admin@NextPost.local",
                        "ADMIN@NEXTPOST.LOCAL",
                        false,
                        false,
                        false,
                        false,
                        "AQAAAAIAAYagAAAAEE4rExzgJHG+YFRvZ8xYdiQ64O9xoSgxd2eQSUP0LmHszA//khyPyr6NHMDF4v8pFg==",
                        Guid.NewGuid().ToString(),
                        Guid.NewGuid().ToString(),
                        0
                    }
                });
            migrationBuilder.InsertData(
                schema: "security",
                table: "UserRoles",
                columns: new[] { "UserId", "RoleId" },
                values: new object[,]
                {
                    {
                "(SELECT Id FROM [security].Users WHERE UserName = 'Admin')",
                "(SELECT Id FROM [security].Roles WHERE Name = 'Admin')"
                    }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM [security].UserRoles WHERE UserId = (SELECT Id FROM [security].Users WHERE UserName = 'Admin')");
            migrationBuilder.Sql("DELETE FROM [security].Users WHERE UserName = 'Admin'");
            migrationBuilder.Sql("DELETE FROM [security].Roles WHERE Name IN ('Admin', 'Author')");
        }
    }
}
