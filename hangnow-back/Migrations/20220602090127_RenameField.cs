using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace hangnow_back.Migrations
{
    public partial class RenameField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EventUser_AspNetUsers_ParticipantsId",
                table: "EventUser");

            migrationBuilder.DropForeignKey(
                name: "FK_EventUser_Events_ParticipationsId",
                table: "EventUser");

            migrationBuilder.RenameColumn(
                name: "ParticipationsId",
                table: "EventUser",
                newName: "UsersId");

            migrationBuilder.RenameColumn(
                name: "ParticipantsId",
                table: "EventUser",
                newName: "EventsId");

            migrationBuilder.RenameIndex(
                name: "IX_EventUser_ParticipationsId",
                table: "EventUser",
                newName: "IX_EventUser_UsersId");

            migrationBuilder.AddForeignKey(
                name: "FK_EventUser_AspNetUsers_UsersId",
                table: "EventUser",
                column: "UsersId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EventUser_Events_EventsId",
                table: "EventUser",
                column: "EventsId",
                principalTable: "Events",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EventUser_AspNetUsers_UsersId",
                table: "EventUser");

            migrationBuilder.DropForeignKey(
                name: "FK_EventUser_Events_EventsId",
                table: "EventUser");

            migrationBuilder.RenameColumn(
                name: "UsersId",
                table: "EventUser",
                newName: "ParticipationsId");

            migrationBuilder.RenameColumn(
                name: "EventsId",
                table: "EventUser",
                newName: "ParticipantsId");

            migrationBuilder.RenameIndex(
                name: "IX_EventUser_UsersId",
                table: "EventUser",
                newName: "IX_EventUser_ParticipationsId");

            migrationBuilder.AddForeignKey(
                name: "FK_EventUser_AspNetUsers_ParticipantsId",
                table: "EventUser",
                column: "ParticipantsId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EventUser_Events_ParticipationsId",
                table: "EventUser",
                column: "ParticipationsId",
                principalTable: "Events",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
