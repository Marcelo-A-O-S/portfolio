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
            migrationBuilder.CreateTable(
                name: "Categories",
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
                name: "Languages",
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
                name: "CategoriesContents",
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
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CategoriesContents_Languages_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "Languages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CategoryPost",
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
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CategoryTool",
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
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MediaProjections",
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
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    MediaProjectionId = table.Column<Guid>(type: "uuid", nullable: false),
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
                        name: "FK_Posts_MediaProjections_MediaProjectionId",
                        column: x => x.MediaProjectionId,
                        principalTable: "MediaProjections",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Tools",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    MediaProjectionId = table.Column<Guid>(type: "uuid", nullable: false),
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
                        name: "FK_Tools_MediaProjections_MediaProjectionId",
                        column: x => x.MediaProjectionId,
                        principalTable: "MediaProjections",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PostContents",
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
                        principalTable: "Languages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PostContents_Posts_PostId",
                        column: x => x.PostId,
                        principalTable: "Posts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PostTool",
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
                        principalTable: "Posts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PostTool_Tools_ToolsId",
                        column: x => x.ToolsId,
                        principalTable: "Tools",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ToolContents",
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
                        principalTable: "Languages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ToolContents_Tools_ToolId",
                        column: x => x.ToolId,
                        principalTable: "Tools",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CategoriesContents_CategoryId",
                table: "CategoriesContents",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_CategoriesContents_LanguageId",
                table: "CategoriesContents",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_CategoriesContents_Slug",
                table: "CategoriesContents",
                column: "Slug");

            migrationBuilder.CreateIndex(
                name: "IX_CategoryPost_PostsId",
                table: "CategoryPost",
                column: "PostsId");

            migrationBuilder.CreateIndex(
                name: "IX_CategoryTool_ToolsId",
                table: "CategoryTool",
                column: "ToolsId");

            migrationBuilder.CreateIndex(
                name: "IX_Languages_Code",
                table: "Languages",
                column: "Code");

            migrationBuilder.CreateIndex(
                name: "IX_LikeProjections_TargetId_UserId",
                table: "LikeProjections",
                columns: new[] { "TargetId", "UserId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MediaProjections_PostContentId",
                table: "MediaProjections",
                column: "PostContentId");

            migrationBuilder.CreateIndex(
                name: "IX_MediaProjections_ToolContentId",
                table: "MediaProjections",
                column: "ToolContentId");

            migrationBuilder.CreateIndex(
                name: "IX_MediaProjections_Url_MediaId",
                table: "MediaProjections",
                columns: new[] { "Url", "MediaId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PostContents_LanguageId",
                table: "PostContents",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_PostContents_PostId",
                table: "PostContents",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_PostContents_Slug",
                table: "PostContents",
                column: "Slug");

            migrationBuilder.CreateIndex(
                name: "IX_Posts_MediaProjectionId",
                table: "Posts",
                column: "MediaProjectionId");

            migrationBuilder.CreateIndex(
                name: "IX_PostTool_ToolsId",
                table: "PostTool",
                column: "ToolsId");

            migrationBuilder.CreateIndex(
                name: "IX_ToolContents_LanguageId",
                table: "ToolContents",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_ToolContents_Slug",
                table: "ToolContents",
                column: "Slug");

            migrationBuilder.CreateIndex(
                name: "IX_ToolContents_ToolId",
                table: "ToolContents",
                column: "ToolId");

            migrationBuilder.CreateIndex(
                name: "IX_Tools_MediaProjectionId",
                table: "Tools",
                column: "MediaProjectionId");

            migrationBuilder.AddForeignKey(
                name: "FK_CategoryPost_Posts_PostsId",
                table: "CategoryPost",
                column: "PostsId",
                principalTable: "Posts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CategoryTool_Tools_ToolsId",
                table: "CategoryTool",
                column: "ToolsId",
                principalTable: "Tools",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MediaProjections_PostContents_PostContentId",
                table: "MediaProjections",
                column: "PostContentId",
                principalTable: "PostContents",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MediaProjections_ToolContents_ToolContentId",
                table: "MediaProjections",
                column: "ToolContentId",
                principalTable: "ToolContents",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PostContents_Languages_LanguageId",
                table: "PostContents");

            migrationBuilder.DropForeignKey(
                name: "FK_ToolContents_Languages_LanguageId",
                table: "ToolContents");

            migrationBuilder.DropForeignKey(
                name: "FK_PostContents_Posts_PostId",
                table: "PostContents");

            migrationBuilder.DropForeignKey(
                name: "FK_ToolContents_Tools_ToolId",
                table: "ToolContents");

            migrationBuilder.DropTable(
                name: "CategoriesContents");

            migrationBuilder.DropTable(
                name: "CategoryPost");

            migrationBuilder.DropTable(
                name: "CategoryTool");

            migrationBuilder.DropTable(
                name: "LikeProjections");

            migrationBuilder.DropTable(
                name: "PostTool");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Languages");

            migrationBuilder.DropTable(
                name: "Posts");

            migrationBuilder.DropTable(
                name: "Tools");

            migrationBuilder.DropTable(
                name: "MediaProjections");

            migrationBuilder.DropTable(
                name: "PostContents");

            migrationBuilder.DropTable(
                name: "ToolContents");
        }
    }
}
