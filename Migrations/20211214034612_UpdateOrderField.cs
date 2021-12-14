using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace thekitchen_aspnetcore.Migrations
{
    public partial class UpdateOrderField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "OrderReceiverName",
                table: "Orders",
                newName: "OrderReceiverLastname");

            migrationBuilder.AddColumn<string>(
                name: "OrderReceiverFirstname",
                table: "Orders",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OrderReceiverFirstname",
                table: "Orders");

            migrationBuilder.RenameColumn(
                name: "OrderReceiverLastname",
                table: "Orders",
                newName: "OrderReceiverName");
        }
    }
}
