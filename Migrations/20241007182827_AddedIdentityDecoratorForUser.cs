using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TeaPost.Migrations
{
    /// <inheritdoc />
    public partial class AddedIdentityDecoratorForUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_User",
                table: "User");

            migrationBuilder.RenameTable(
                name: "User",
                newName: "Users");

            migrationBuilder.RenameColumn(
                name: "followedAt",
                table: "Followers",
                newName: "FollowedAt");

            migrationBuilder.RenameColumn(
                name: "followed",
                table: "Followers",
                newName: "Followed");

            migrationBuilder.RenameColumn(
                name: "follower",
                table: "Followers",
                newName: "Follower");

            migrationBuilder.RenameColumn(
                name: "userName",
                table: "Users",
                newName: "UserName");

            migrationBuilder.RenameColumn(
                name: "phoneNumber",
                table: "Users",
                newName: "PhoneNumber");

            migrationBuilder.RenameColumn(
                name: "lastName",
                table: "Users",
                newName: "LastName");

            migrationBuilder.RenameColumn(
                name: "firstName",
                table: "Users",
                newName: "FirstName");

            migrationBuilder.RenameColumn(
                name: "email",
                table: "Users",
                newName: "Email");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Users",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "password",
                table: "Users",
                newName: "Pass");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Users",
                table: "Users",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Users",
                table: "Users");

            migrationBuilder.RenameTable(
                name: "Users",
                newName: "User");

            migrationBuilder.RenameColumn(
                name: "FollowedAt",
                table: "Followers",
                newName: "followedAt");

            migrationBuilder.RenameColumn(
                name: "Followed",
                table: "Followers",
                newName: "followed");

            migrationBuilder.RenameColumn(
                name: "Follower",
                table: "Followers",
                newName: "follower");

            migrationBuilder.RenameColumn(
                name: "UserName",
                table: "User",
                newName: "userName");

            migrationBuilder.RenameColumn(
                name: "PhoneNumber",
                table: "User",
                newName: "phoneNumber");

            migrationBuilder.RenameColumn(
                name: "LastName",
                table: "User",
                newName: "lastName");

            migrationBuilder.RenameColumn(
                name: "FirstName",
                table: "User",
                newName: "firstName");

            migrationBuilder.RenameColumn(
                name: "Email",
                table: "User",
                newName: "email");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "User",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "Pass",
                table: "User",
                newName: "password");

            migrationBuilder.AddPrimaryKey(
                name: "PK_User",
                table: "User",
                column: "id");
        }
    }
}
