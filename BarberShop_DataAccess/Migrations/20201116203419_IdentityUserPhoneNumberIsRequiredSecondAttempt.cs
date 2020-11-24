using Microsoft.EntityFrameworkCore.Migrations;

namespace BarberShop_DataAccess.Migrations
{
    public partial class IdentityUserPhoneNumberIsRequiredSecondAttempt : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
               name: "PhoneNumber",
               table: "AspNetUsers",
               type: "nvarchar(max)",
               nullable: true,
               defaultValue: "",
               oldClrType: typeof(string),
               oldType: "nvarchar(max)",
               oldNullable: false);
        }
    }
}
