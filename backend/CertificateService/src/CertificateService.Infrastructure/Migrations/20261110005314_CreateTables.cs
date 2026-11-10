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
            migrationBuilder.EnsureSchema(
                name: "certificate_service");

            migrationBuilder.CreateTable(
                name: "LanguageProjections",
                schema: "certificate_service",
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

            migrationBuilder.CreateTable(
                name: "MediaProjections",
                schema: "certificate_service",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    MediaId = table.Column<Guid>(type: "uuid", nullable: false),
                    Url = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MediaProjections", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OutboxMessages",
                schema: "certificate_service",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    EventType = table.Column<string>(type: "text", nullable: false),
                    Payload = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ProcessedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ProcessingStartedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Error = table.Column<string>(type: "text", nullable: true),
                    Attempts = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OutboxMessages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Certificates",
                schema: "certificate_service",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    MediaProjectionId = table.Column<Guid>(type: "uuid", nullable: false),
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
                        principalSchema: "certificate_service",
                        principalTable: "MediaProjections",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PostProjections",
                schema: "certificate_service",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PostId = table.Column<Guid>(type: "uuid", nullable: false),
                    MediaProjectionId = table.Column<Guid>(type: "uuid", nullable: false),
                    LikeCount = table.Column<int>(type: "integer", nullable: false),
                    CommentCount = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostProjections", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PostProjections_MediaProjections_MediaProjectionId",
                        column: x => x.MediaProjectionId,
                        principalSchema: "certificate_service",
                        principalTable: "MediaProjections",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CertificateContents",
                schema: "certificate_service",
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
                        principalSchema: "certificate_service",
                        principalTable: "Certificates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CertificateContents_LanguageProjections_LanguageProjectionId",
                        column: x => x.LanguageProjectionId,
                        principalSchema: "certificate_service",
                        principalTable: "LanguageProjections",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CertificatePostProjection",
                schema: "certificate_service",
                columns: table => new
                {
                    CertificatesId = table.Column<Guid>(type: "uuid", nullable: false),
                    PostProjectionsId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CertificatePostProjection", x => new { x.CertificatesId, x.PostProjectionsId });
                    table.ForeignKey(
                        name: "FK_CertificatePostProjection_Certificates_CertificatesId",
                        column: x => x.CertificatesId,
                        principalSchema: "certificate_service",
                        principalTable: "Certificates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CertificatePostProjection_PostProjections_PostProjectionsId",
                        column: x => x.PostProjectionsId,
                        principalSchema: "certificate_service",
                        principalTable: "PostProjections",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PostContentProjections",
                schema: "certificate_service",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PostContentId = table.Column<Guid>(type: "uuid", nullable: false),
                    PostProjectionId = table.Column<Guid>(type: "uuid", nullable: false),
                    LanguageProjectionId = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostContentProjections", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PostContentProjections_LanguageProjections_LanguageProjecti~",
                        column: x => x.LanguageProjectionId,
                        principalSchema: "certificate_service",
                        principalTable: "LanguageProjections",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PostContentProjections_PostProjections_PostProjectionId",
                        column: x => x.PostProjectionId,
                        principalSchema: "certificate_service",
                        principalTable: "PostProjections",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CertificateContents_CertificateId",
                schema: "certificate_service",
                table: "CertificateContents",
                column: "CertificateId");

            migrationBuilder.CreateIndex(
                name: "IX_CertificateContents_LanguageProjectionId",
                schema: "certificate_service",
                table: "CertificateContents",
                column: "LanguageProjectionId");

            migrationBuilder.CreateIndex(
                name: "IX_CertificatePostProjection_PostProjectionsId",
                schema: "certificate_service",
                table: "CertificatePostProjection",
                column: "PostProjectionsId");

            migrationBuilder.CreateIndex(
                name: "IX_Certificates_MediaProjectionId",
                schema: "certificate_service",
                table: "Certificates",
                column: "MediaProjectionId");

            migrationBuilder.CreateIndex(
                name: "IX_LanguageProjections_Code",
                schema: "certificate_service",
                table: "LanguageProjections",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MediaProjections_Url_MediaId",
                schema: "certificate_service",
                table: "MediaProjections",
                columns: new[] { "Url", "MediaId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PostContentProjections_LanguageProjectionId",
                schema: "certificate_service",
                table: "PostContentProjections",
                column: "LanguageProjectionId");

            migrationBuilder.CreateIndex(
                name: "IX_PostContentProjections_PostContentId",
                schema: "certificate_service",
                table: "PostContentProjections",
                column: "PostContentId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PostContentProjections_PostProjectionId",
                schema: "certificate_service",
                table: "PostContentProjections",
                column: "PostProjectionId");

            migrationBuilder.CreateIndex(
                name: "IX_PostProjections_MediaProjectionId",
                schema: "certificate_service",
                table: "PostProjections",
                column: "MediaProjectionId");

            migrationBuilder.CreateIndex(
                name: "IX_PostProjections_PostId",
                schema: "certificate_service",
                table: "PostProjections",
                column: "PostId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CertificateContents",
                schema: "certificate_service");

            migrationBuilder.DropTable(
                name: "CertificatePostProjection",
                schema: "certificate_service");

            migrationBuilder.DropTable(
                name: "OutboxMessages",
                schema: "certificate_service");

            migrationBuilder.DropTable(
                name: "PostContentProjections",
                schema: "certificate_service");

            migrationBuilder.DropTable(
                name: "Certificates",
                schema: "certificate_service");

            migrationBuilder.DropTable(
                name: "LanguageProjections",
                schema: "certificate_service");

            migrationBuilder.DropTable(
                name: "PostProjections",
                schema: "certificate_service");

            migrationBuilder.DropTable(
                name: "MediaProjections",
                schema: "certificate_service");
        }
    }
}
