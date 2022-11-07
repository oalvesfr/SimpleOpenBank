using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SimpleOpenBank.Persistence.Migrations
{
    public partial class renameUserId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IdUser",
                table: "TokenRefreshs",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "IdUser",
                table: "Accounts",
                newName: "UserId");

            migrationBuilder.AlterColumn<string>(
                name: "Created_At",
                table: "Movims",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Currency",
                table: "Accounts",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Created_At",
                table: "Accounts",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "TokenRefreshs",
                newName: "IdUser");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Accounts",
                newName: "IdUser");

            migrationBuilder.AlterColumn<string>(
                name: "Created_At",
                table: "Movims",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Currency",
                table: "Accounts",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Created_At",
                table: "Accounts",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");
        }
    }
}
