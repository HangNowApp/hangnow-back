using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace hangnow_back.Migrations
{
    public partial class FixTypo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EventUser_AspNetUsers_ParticipansId",
                table: "EventUser");

            migrationBuilder.DropTable(
                name: "Participants");

            migrationBuilder.RenameColumn(
                name: "ParticipansId",
                table: "EventUser",
                newName: "ParticipantsId");

            migrationBuilder.AddForeignKey(
                name: "FK_EventUser_AspNetUsers_ParticipantsId",
                table: "EventUser",
                column: "ParticipantsId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EventUser_AspNetUsers_ParticipantsId",
                table: "EventUser");

            migrationBuilder.RenameColumn(
                name: "ParticipantsId",
                table: "EventUser",
                newName: "ParticipansId");

            migrationBuilder.CreateTable(
                name: "Participants",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    EventId = table.Column<Guid>(type: "TEXT", nullable: false),
                    UserId = table.Column<Guid>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Participants", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Participants_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Participants_Events_EventId",
                        column: x => x.EventId,
                        principalTable: "Events",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Participants_EventId",
                table: "Participants",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_Participants_UserId",
                table: "Participants",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_EventUser_AspNetUsers_ParticipansId",
                table: "EventUser",
                column: "ParticipansId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
