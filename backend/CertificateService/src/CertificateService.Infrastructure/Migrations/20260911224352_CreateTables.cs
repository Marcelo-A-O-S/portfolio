using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CertificateService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CreateTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LanguageProjections",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    LanguageId = table.Column<Guid>(type: "uuid", nullable: false),
                    Code = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LanguageProjections", x => x.Id);
                });

            // migrationBuilder.CreateTable(
            //     name: "MediaProjections",
            //     columns: table => new
            //     {
            //         Id = table.Column<Guid>(type: "uuid", nullable: false),
            //         MediaId = table.Column<Guid>(type: "uuid", nullable: false),
            //         Url = table.Column<string>(type: "text", nullable: false)
            //     },
            //     constraints: table =>
            //     {
            //         table.PrimaryKey("PK_MediaProjections", x => x.Id);
            //     });

            migrationBuilder.CreateTable(
                name: "Certificates",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    MediaProjectionId = table.Column<Guid>(type: "uuid", nullable: true),
                    CredentialId = table.Column<string>(type: "text", nullable: true),
                    VerificationUrl = table.Column<string>(type: "text", nullable: true),
                    Institution = table.Column<string>(type: "text", nullable: false),
                    WorkLoadHours = table.Column<int>(type: "integer", nullable: true),
                    Status = table.Column<string>(type: "text", nullable: false),
                    CertificateType = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IssuerDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Certificates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Certificates_MediaProjections_MediaProjectionId",
                        column: x => x.MediaProjectionId,
                        principalTable: "MediaProjections",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CertificateContents",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CertificateId = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    LanguageProjectionId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CertificateContents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CertificateContents_Certificates_CertificateId",
                        column: x => x.CertificateId,
                        principalTable: "Certificates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CertificateContents_LanguageProjections_LanguageProjectionId",
                        column: x => x.LanguageProjectionId,
                        principalTable: "LanguageProjections",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PostProjections",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PostId = table.Column<Guid>(type: "uuid", nullable: false),
                    CertificateId = table.Column<Guid>(type: "uuid", nullable: false),
                    MediaProjectionId = table.Column<Guid>(type: "uuid", nullable: false),
                    LikeCount = table.Column<int>(type: "integer", nullable: false),
                    CommentCount = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostProjections", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PostProjections_Certificates_CertificateId",
                        column: x => x.CertificateId,
                        principalTable: "Certificates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PostProjections_MediaProjections_MediaProjectionId",
                        column: x => x.MediaProjectionId,
                        principalTable: "MediaProjections",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PostContentProjections",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PostContentId = table.Column<Guid>(type: "uuid", nullable: false),
                    LanguageProjectionId = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    PostProjectionId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostContentProjections", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PostContentProjections_LanguageProjections_LanguageProjecti~",
                        column: x => x.LanguageProjectionId,
                        principalTable: "LanguageProjections",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PostContentProjections_PostProjections_PostProjectionId",
                        column: x => x.PostProjectionId,
                        principalTable: "PostProjections",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_CertificateContents_CertificateId",
                table: "CertificateContents",
                column: "CertificateId");

            migrationBuilder.CreateIndex(
                name: "IX_CertificateContents_LanguageProjectionId",
                table: "CertificateContents",
                column: "LanguageProjectionId");

            migrationBuilder.CreateIndex(
                name: "IX_Certificates_MediaProjectionId",
                table: "Certificates",
                column: "MediaProjectionId");

            migrationBuilder.CreateIndex(
                name: "IX_LanguageProjections_Code",
                table: "LanguageProjections",
                column: "Code",
                unique: true);

            // migrationBuilder.CreateIndex(
            //     name: "IX_MediaProjections_Url_MediaId",
            //     table: "MediaProjections",
            //     columns: new[] { "Url", "MediaId" },
            //     unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PostContentProjections_LanguageProjectionId",
                table: "PostContentProjections",
                column: "LanguageProjectionId");

            migrationBuilder.CreateIndex(
                name: "IX_PostContentProjections_PostProjectionId",
                table: "PostContentProjections",
                column: "PostProjectionId");

            migrationBuilder.CreateIndex(
                name: "IX_PostProjections_CertificateId",
                table: "PostProjections",
                column: "CertificateId");

            migrationBuilder.CreateIndex(
                name: "IX_PostProjections_MediaProjectionId",
                table: "PostProjections",
                column: "MediaProjectionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CertificateContents");

            migrationBuilder.DropTable(
                name: "PostContentProjections");

            migrationBuilder.DropTable(
                name: "LanguageProjections");

            migrationBuilder.DropTable(
                name: "PostProjections");

            migrationBuilder.DropTable(
                name: "Certificates");

            migrationBuilder.DropTable(
                name: "MediaProjections");
        }
    }
}
