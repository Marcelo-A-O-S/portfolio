using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PostService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CreateTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "post_service");

            migrationBuilder.CreateTable(
                name: "Authors",
                schema: "post_service",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    Username = table.Column<string>(type: "text", nullable: false),
                    ProfileUrl = table.Column<string>(type: "text", nullable: false),
                    ProviderId = table.Column<string>(type: "text", nullable: false),
                    Provider = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Authors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Categories",
                schema: "post_service",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CertificateProjections",
                schema: "post_service",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CertificateId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CertificateProjections", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Languages",
                schema: "post_service",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Code = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Languages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LikeProjections",
                schema: "post_service",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TargetId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LikeProjections", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LinkTypes",
                schema: "post_service",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    BackgroundColor = table.Column<string>(type: "text", nullable: false),
                    TextColor = table.Column<string>(type: "text", nullable: false),
                    BorderColor = table.Column<string>(type: "text", nullable: false),
                    Icon = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LinkTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OutboxMessages",
                schema: "post_service",
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
                name: "CategoriesContents",
                schema: "post_service",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CategoryId = table.Column<Guid>(type: "uuid", nullable: false),
                    LanguageId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Slug = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdateAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoriesContents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CategoriesContents_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalSchema: "post_service",
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CategoriesContents_Languages_LanguageId",
                        column: x => x.LanguageId,
                        principalSchema: "post_service",
                        principalTable: "Languages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CertificateContentProjections",
                schema: "post_service",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CertificateContentId = table.Column<Guid>(type: "uuid", nullable: false),
                    CertificateProjectionId = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false),
                    LanguageId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CertificateContentProjections", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CertificateContentProjections_CertificateProjections_Certif~",
                        column: x => x.CertificateProjectionId,
                        principalSchema: "post_service",
                        principalTable: "CertificateProjections",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CertificateContentProjections_Languages_LanguageId",
                        column: x => x.LanguageId,
                        principalSchema: "post_service",
                        principalTable: "Languages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CategoryPost",
                schema: "post_service",
                columns: table => new
                {
                    CategoriesId = table.Column<Guid>(type: "uuid", nullable: false),
                    PostsId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoryPost", x => new { x.CategoriesId, x.PostsId });
                    table.ForeignKey(
                        name: "FK_CategoryPost_Categories_CategoriesId",
                        column: x => x.CategoriesId,
                        principalSchema: "post_service",
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CategoryTool",
                schema: "post_service",
                columns: table => new
                {
                    CategoriesId = table.Column<Guid>(type: "uuid", nullable: false),
                    ToolsId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoryTool", x => new { x.CategoriesId, x.ToolsId });
                    table.ForeignKey(
                        name: "FK_CategoryTool_Categories_CategoriesId",
                        column: x => x.CategoriesId,
                        principalSchema: "post_service",
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CertificateProjectionPost",
                schema: "post_service",
                columns: table => new
                {
                    CertificateProjectionsId = table.Column<Guid>(type: "uuid", nullable: false),
                    PostsId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CertificateProjectionPost", x => new { x.CertificateProjectionsId, x.PostsId });
                    table.ForeignKey(
                        name: "FK_CertificateProjectionPost_CertificateProjections_Certificat~",
                        column: x => x.CertificateProjectionsId,
                        principalSchema: "post_service",
                        principalTable: "CertificateProjections",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Contributors",
                schema: "post_service",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: true),
                    PostId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    ProfileUrl = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contributors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LinkDescriptions",
                schema: "post_service",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false),
                    LinkId = table.Column<Guid>(type: "uuid", nullable: false),
                    LanguageId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LinkDescriptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LinkDescriptions_Languages_LanguageId",
                        column: x => x.LanguageId,
                        principalSchema: "post_service",
                        principalTable: "Languages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Links",
                schema: "post_service",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Url = table.Column<string>(type: "text", nullable: false),
                    LinkTypeId = table.Column<Guid>(type: "uuid", nullable: false),
                    PostId = table.Column<Guid>(type: "uuid", nullable: true),
                    ToolId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Links", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Links_LinkTypes_LinkTypeId",
                        column: x => x.LinkTypeId,
                        principalSchema: "post_service",
                        principalTable: "LinkTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MediaProjections",
                schema: "post_service",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    MediaId = table.Column<Guid>(type: "uuid", nullable: false),
                    Url = table.Column<string>(type: "text", nullable: false),
                    PostContentId = table.Column<Guid>(type: "uuid", nullable: true),
                    ToolContentId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MediaProjections", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Posts",
                schema: "post_service",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    MediaProjectionId = table.Column<Guid>(type: "uuid", nullable: false),
                    AuthorId = table.Column<Guid>(type: "uuid", nullable: false),
                    LikeCount = table.Column<int>(type: "integer", nullable: false),
                    CommentCount = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Status = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Posts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Posts_Authors_AuthorId",
                        column: x => x.AuthorId,
                        principalSchema: "post_service",
                        principalTable: "Authors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Posts_MediaProjections_MediaProjectionId",
                        column: x => x.MediaProjectionId,
                        principalSchema: "post_service",
                        principalTable: "MediaProjections",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Tools",
                schema: "post_service",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    MediaProjectionId = table.Column<Guid>(type: "uuid", nullable: false),
                    AuthorId = table.Column<Guid>(type: "uuid", nullable: false),
                    LikeCount = table.Column<int>(type: "integer", nullable: false),
                    CommentCount = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Status = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tools", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tools_Authors_AuthorId",
                        column: x => x.AuthorId,
                        principalSchema: "post_service",
                        principalTable: "Authors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Tools_MediaProjections_MediaProjectionId",
                        column: x => x.MediaProjectionId,
                        principalSchema: "post_service",
                        principalTable: "MediaProjections",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PostContents",
                schema: "post_service",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PostId = table.Column<Guid>(type: "uuid", nullable: false),
                    LanguageId = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Content = table.Column<string>(type: "text", nullable: false),
                    Slug = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostContents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PostContents_Languages_LanguageId",
                        column: x => x.LanguageId,
                        principalSchema: "post_service",
                        principalTable: "Languages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PostContents_Posts_PostId",
                        column: x => x.PostId,
                        principalSchema: "post_service",
                        principalTable: "Posts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PostTool",
                schema: "post_service",
                columns: table => new
                {
                    PostsId = table.Column<Guid>(type: "uuid", nullable: false),
                    ToolsId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostTool", x => new { x.PostsId, x.ToolsId });
                    table.ForeignKey(
                        name: "FK_PostTool_Posts_PostsId",
                        column: x => x.PostsId,
                        principalSchema: "post_service",
                        principalTable: "Posts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PostTool_Tools_ToolsId",
                        column: x => x.ToolsId,
                        principalSchema: "post_service",
                        principalTable: "Tools",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ToolContents",
                schema: "post_service",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ToolId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    LanguageId = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Content = table.Column<string>(type: "text", nullable: false),
                    Slug = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ToolContents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ToolContents_Languages_LanguageId",
                        column: x => x.LanguageId,
                        principalSchema: "post_service",
                        principalTable: "Languages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ToolContents_Tools_ToolId",
                        column: x => x.ToolId,
                        principalSchema: "post_service",
                        principalTable: "Tools",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CategoriesContents_CategoryId",
                schema: "post_service",
                table: "CategoriesContents",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_CategoriesContents_LanguageId",
                schema: "post_service",
                table: "CategoriesContents",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_CategoriesContents_Slug",
                schema: "post_service",
                table: "CategoriesContents",
                column: "Slug");

            migrationBuilder.CreateIndex(
                name: "IX_CategoryPost_PostsId",
                schema: "post_service",
                table: "CategoryPost",
                column: "PostsId");

            migrationBuilder.CreateIndex(
                name: "IX_CategoryTool_ToolsId",
                schema: "post_service",
                table: "CategoryTool",
                column: "ToolsId");

            migrationBuilder.CreateIndex(
                name: "IX_CertificateContentProjections_CertificateProjectionId",
                schema: "post_service",
                table: "CertificateContentProjections",
                column: "CertificateProjectionId");

            migrationBuilder.CreateIndex(
                name: "IX_CertificateContentProjections_LanguageId",
                schema: "post_service",
                table: "CertificateContentProjections",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_CertificateProjectionPost_PostsId",
                schema: "post_service",
                table: "CertificateProjectionPost",
                column: "PostsId");

            migrationBuilder.CreateIndex(
                name: "IX_CertificateProjections_CertificateId",
                schema: "post_service",
                table: "CertificateProjections",
                column: "CertificateId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Contributors_PostId",
                schema: "post_service",
                table: "Contributors",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_Languages_Code",
                schema: "post_service",
                table: "Languages",
                column: "Code");

            migrationBuilder.CreateIndex(
                name: "IX_LikeProjections_TargetId_UserId",
                schema: "post_service",
                table: "LikeProjections",
                columns: new[] { "TargetId", "UserId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LinkDescriptions_LanguageId",
                schema: "post_service",
                table: "LinkDescriptions",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_LinkDescriptions_LinkId",
                schema: "post_service",
                table: "LinkDescriptions",
                column: "LinkId");

            migrationBuilder.CreateIndex(
                name: "IX_Links_LinkTypeId",
                schema: "post_service",
                table: "Links",
                column: "LinkTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Links_PostId",
                schema: "post_service",
                table: "Links",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_Links_ToolId",
                schema: "post_service",
                table: "Links",
                column: "ToolId");

            migrationBuilder.CreateIndex(
                name: "IX_MediaProjections_PostContentId",
                schema: "post_service",
                table: "MediaProjections",
                column: "PostContentId");

            migrationBuilder.CreateIndex(
                name: "IX_MediaProjections_ToolContentId",
                schema: "post_service",
                table: "MediaProjections",
                column: "ToolContentId");

            migrationBuilder.CreateIndex(
                name: "IX_MediaProjections_Url_MediaId",
                schema: "post_service",
                table: "MediaProjections",
                columns: new[] { "Url", "MediaId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PostContents_LanguageId",
                schema: "post_service",
                table: "PostContents",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_PostContents_PostId",
                schema: "post_service",
                table: "PostContents",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_PostContents_Slug",
                schema: "post_service",
                table: "PostContents",
                column: "Slug");

            migrationBuilder.CreateIndex(
                name: "IX_Posts_AuthorId",
                schema: "post_service",
                table: "Posts",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_Posts_MediaProjectionId",
                schema: "post_service",
                table: "Posts",
                column: "MediaProjectionId");

            migrationBuilder.CreateIndex(
                name: "IX_PostTool_ToolsId",
                schema: "post_service",
                table: "PostTool",
                column: "ToolsId");

            migrationBuilder.CreateIndex(
                name: "IX_ToolContents_LanguageId",
                schema: "post_service",
                table: "ToolContents",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_ToolContents_Slug",
                schema: "post_service",
                table: "ToolContents",
                column: "Slug");

            migrationBuilder.CreateIndex(
                name: "IX_ToolContents_ToolId",
                schema: "post_service",
                table: "ToolContents",
                column: "ToolId");

            migrationBuilder.CreateIndex(
                name: "IX_Tools_AuthorId",
                schema: "post_service",
                table: "Tools",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_Tools_MediaProjectionId",
                schema: "post_service",
                table: "Tools",
                column: "MediaProjectionId");

            migrationBuilder.AddForeignKey(
                name: "FK_CategoryPost_Posts_PostsId",
                schema: "post_service",
                table: "CategoryPost",
                column: "PostsId",
                principalSchema: "post_service",
                principalTable: "Posts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CategoryTool_Tools_ToolsId",
                schema: "post_service",
                table: "CategoryTool",
                column: "ToolsId",
                principalSchema: "post_service",
                principalTable: "Tools",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CertificateProjectionPost_Posts_PostsId",
                schema: "post_service",
                table: "CertificateProjectionPost",
                column: "PostsId",
                principalSchema: "post_service",
                principalTable: "Posts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Contributors_Posts_PostId",
                schema: "post_service",
                table: "Contributors",
                column: "PostId",
                principalSchema: "post_service",
                principalTable: "Posts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LinkDescriptions_Links_LinkId",
                schema: "post_service",
                table: "LinkDescriptions",
                column: "LinkId",
                principalSchema: "post_service",
                principalTable: "Links",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Links_Posts_PostId",
                schema: "post_service",
                table: "Links",
                column: "PostId",
                principalSchema: "post_service",
                principalTable: "Posts",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Links_Tools_ToolId",
                schema: "post_service",
                table: "Links",
                column: "ToolId",
                principalSchema: "post_service",
                principalTable: "Tools",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MediaProjections_PostContents_PostContentId",
                schema: "post_service",
                table: "MediaProjections",
                column: "PostContentId",
                principalSchema: "post_service",
                principalTable: "PostContents",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MediaProjections_ToolContents_ToolContentId",
                schema: "post_service",
                table: "MediaProjections",
                column: "ToolContentId",
                principalSchema: "post_service",
                principalTable: "ToolContents",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PostContents_Languages_LanguageId",
                schema: "post_service",
                table: "PostContents");

            migrationBuilder.DropForeignKey(
                name: "FK_ToolContents_Languages_LanguageId",
                schema: "post_service",
                table: "ToolContents");

            migrationBuilder.DropForeignKey(
                name: "FK_PostContents_Posts_PostId",
                schema: "post_service",
                table: "PostContents");

            migrationBuilder.DropForeignKey(
                name: "FK_ToolContents_Tools_ToolId",
                schema: "post_service",
                table: "ToolContents");

            migrationBuilder.DropTable(
                name: "CategoriesContents",
                schema: "post_service");

            migrationBuilder.DropTable(
                name: "CategoryPost",
                schema: "post_service");

            migrationBuilder.DropTable(
                name: "CategoryTool",
                schema: "post_service");

            migrationBuilder.DropTable(
                name: "CertificateContentProjections",
                schema: "post_service");

            migrationBuilder.DropTable(
                name: "CertificateProjectionPost",
                schema: "post_service");

            migrationBuilder.DropTable(
                name: "Contributors",
                schema: "post_service");

            migrationBuilder.DropTable(
                name: "LikeProjections",
                schema: "post_service");

            migrationBuilder.DropTable(
                name: "LinkDescriptions",
                schema: "post_service");

            migrationBuilder.DropTable(
                name: "OutboxMessages",
                schema: "post_service");

            migrationBuilder.DropTable(
                name: "PostTool",
                schema: "post_service");

            migrationBuilder.DropTable(
                name: "Categories",
                schema: "post_service");

            migrationBuilder.DropTable(
                name: "CertificateProjections",
                schema: "post_service");

            migrationBuilder.DropTable(
                name: "Links",
                schema: "post_service");

            migrationBuilder.DropTable(
                name: "LinkTypes",
                schema: "post_service");

            migrationBuilder.DropTable(
                name: "Languages",
                schema: "post_service");

            migrationBuilder.DropTable(
                name: "Posts",
                schema: "post_service");

            migrationBuilder.DropTable(
                name: "Tools",
                schema: "post_service");

            migrationBuilder.DropTable(
                name: "Authors",
                schema: "post_service");

            migrationBuilder.DropTable(
                name: "MediaProjections",
                schema: "post_service");

            migrationBuilder.DropTable(
                name: "PostContents",
                schema: "post_service");

            migrationBuilder.DropTable(
                name: "ToolContents",
                schema: "post_service");
        }
    }
}
