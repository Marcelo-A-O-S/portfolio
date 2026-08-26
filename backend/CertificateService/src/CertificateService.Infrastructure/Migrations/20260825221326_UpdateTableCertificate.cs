using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CertificateService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTableCertificate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Certificates_MediaProjections_MediaProjectionId",
                table: "Certificates");

            migrationBuilder.AlterColumn<Guid>(
                name: "MediaProjectionId",
                table: "Certificates",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddForeignKey(
                name: "FK_Certificates_MediaProjections_MediaProjectionId",
                table: "Certificates",
                column: "MediaProjectionId",
                principalTable: "MediaProjections",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Certificates_MediaProjections_MediaProjectionId",
                table: "Certificates");

            migrationBuilder.AlterColumn<Guid>(
                name: "MediaProjectionId",
                table: "Certificates",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Certificates_MediaProjections_MediaProjectionId",
                table: "Certificates",
                column: "MediaProjectionId",
                principalTable: "MediaProjections",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
