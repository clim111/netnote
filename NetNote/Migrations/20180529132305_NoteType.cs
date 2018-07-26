using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace NetNote.Migrations
{
    public partial class NoteType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TypeId",
                table: "Notes",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "NoteModel",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Content = table.Column<string>(nullable: false),
                    Create = table.Column<DateTime>(nullable: false),
                    Title = table.Column<string>(maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NoteModel", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NoteTypes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NoteTypes", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Notes_TypeId",
                table: "Notes",
                column: "TypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Notes_NoteTypes_TypeId",
                table: "Notes",
                column: "TypeId",
                principalTable: "NoteTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notes_NoteTypes_TypeId",
                table: "Notes");

            migrationBuilder.DropTable(
                name: "NoteModel");

            migrationBuilder.DropTable(
                name: "NoteTypes");

            migrationBuilder.DropIndex(
                name: "IX_Notes_TypeId",
                table: "Notes");

            migrationBuilder.DropColumn(
                name: "TypeId",
                table: "Notes");
        }
    }
}
