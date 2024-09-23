-- CreateTable
CREATE TABLE "User" (
    "Id" SERIAL NOT NULL,
    "Guid" TEXT NOT NULL,
    "Name" TEXT NOT NULL,

    CONSTRAINT "User_pkey" PRIMARY KEY ("Id")
);

-- CreateTable
CREATE TABLE "SocialMediaAccount" (
    "Id" SERIAL NOT NULL,
    "Provider" TEXT NOT NULL,
    "SocialId" TEXT NOT NULL,
    "Email" TEXT NOT NULL,
    "Username" TEXT NOT NULL,
    "UserId" INTEGER NOT NULL,

    CONSTRAINT "SocialMediaAccount_pkey" PRIMARY KEY ("Id")
);

-- CreateTable
CREATE TABLE "Category" (
    "Id" SERIAL NOT NULL,
    "Guid" TEXT NOT NULL,
    "CategoryName" TEXT NOT NULL,
    "PostId" INTEGER,

    CONSTRAINT "Category_pkey" PRIMARY KEY ("Id")
);

-- CreateTable
CREATE TABLE "Image" (
    "Id" SERIAL NOT NULL,
    "Guid" TEXT NOT NULL,
    "Sequence" INTEGER NOT NULL,
    "UrlImage" TEXT NOT NULL,
    "Caption" TEXT,
    "SectionId" INTEGER NOT NULL,

    CONSTRAINT "Image_pkey" PRIMARY KEY ("Id")
);

-- CreateTable
CREATE TABLE "Title" (
    "Id" SERIAL NOT NULL,
    "Guid" TEXT NOT NULL,
    "Sequence" INTEGER NOT NULL,
    "Content" TEXT NOT NULL,
    "SectionId" INTEGER NOT NULL,

    CONSTRAINT "Title_pkey" PRIMARY KEY ("Id")
);

-- CreateTable
CREATE TABLE "Text" (
    "Id" SERIAL NOT NULL,
    "Guid" TEXT NOT NULL,
    "Sequence" INTEGER NOT NULL,
    "Content" TEXT NOT NULL,
    "SectionId" INTEGER NOT NULL,

    CONSTRAINT "Text_pkey" PRIMARY KEY ("Id")
);

-- CreateTable
CREATE TABLE "Video" (
    "Id" SERIAL NOT NULL,
    "Guid" TEXT NOT NULL,
    "Sequence" INTEGER NOT NULL,
    "UrlVideo" TEXT NOT NULL,
    "Caption" TEXT NOT NULL,
    "SectionId" INTEGER NOT NULL,

    CONSTRAINT "Video_pkey" PRIMARY KEY ("Id")
);

-- CreateTable
CREATE TABLE "Section" (
    "Id" SERIAL NOT NULL,
    "Guid" TEXT NOT NULL,
    "Sequence" INTEGER NOT NULL,
    "PostId" INTEGER NOT NULL,

    CONSTRAINT "Section_pkey" PRIMARY KEY ("Id")
);

-- CreateTable
CREATE TABLE "Link" (
    "Id" SERIAL NOT NULL,
    "Guid" TEXT NOT NULL,
    "Plataform" TEXT NOT NULL,
    "UrlLink" TEXT NOT NULL,
    "PostId" INTEGER,

    CONSTRAINT "Link_pkey" PRIMARY KEY ("Id")
);

-- CreateTable
CREATE TABLE "DevelopmentTool" (
    "Id" SERIAL NOT NULL,
    "Guid" TEXT NOT NULL,
    "Tool" TEXT NOT NULL,
    "Description" TEXT NOT NULL,
    "PostId" INTEGER,

    CONSTRAINT "DevelopmentTool_pkey" PRIMARY KEY ("Id")
);

-- CreateTable
CREATE TABLE "Like" (
    "Id" SERIAL NOT NULL,
    "Guid" TEXT NOT NULL,
    "UserId" INTEGER NOT NULL,
    "PostId" INTEGER,
    "CommentId" INTEGER,

    CONSTRAINT "Like_pkey" PRIMARY KEY ("Id")
);

-- CreateTable
CREATE TABLE "Comment" (
    "Id" SERIAL NOT NULL,
    "Guid" TEXT NOT NULL,
    "Content" TEXT NOT NULL,
    "PostId" INTEGER,
    "DateCreate" TIMESTAMP(3) NOT NULL,
    "ParentId" INTEGER,

    CONSTRAINT "Comment_pkey" PRIMARY KEY ("Id")
);

-- CreateTable
CREATE TABLE "Post" (
    "Id" SERIAL NOT NULL,
    "Guid" TEXT NOT NULL,
    "DateCreate" TIMESTAMP(3) NOT NULL,

    CONSTRAINT "Post_pkey" PRIMARY KEY ("Id")
);

-- CreateIndex
CREATE UNIQUE INDEX "User_Guid_key" ON "User"("Guid");

-- CreateIndex
CREATE UNIQUE INDEX "SocialMediaAccount_SocialId_key" ON "SocialMediaAccount"("SocialId");

-- CreateIndex
CREATE UNIQUE INDEX "SocialMediaAccount_Email_key" ON "SocialMediaAccount"("Email");

-- CreateIndex
CREATE UNIQUE INDEX "Category_Guid_key" ON "Category"("Guid");

-- CreateIndex
CREATE UNIQUE INDEX "Image_Guid_key" ON "Image"("Guid");

-- CreateIndex
CREATE UNIQUE INDEX "Title_Guid_key" ON "Title"("Guid");

-- CreateIndex
CREATE UNIQUE INDEX "Text_Guid_key" ON "Text"("Guid");

-- CreateIndex
CREATE UNIQUE INDEX "Video_Guid_key" ON "Video"("Guid");

-- CreateIndex
CREATE UNIQUE INDEX "Section_Guid_key" ON "Section"("Guid");

-- CreateIndex
CREATE UNIQUE INDEX "Link_Guid_key" ON "Link"("Guid");

-- CreateIndex
CREATE UNIQUE INDEX "DevelopmentTool_Guid_key" ON "DevelopmentTool"("Guid");

-- CreateIndex
CREATE UNIQUE INDEX "Like_Guid_key" ON "Like"("Guid");

-- CreateIndex
CREATE UNIQUE INDEX "Comment_Guid_key" ON "Comment"("Guid");

-- CreateIndex
CREATE UNIQUE INDEX "Post_Guid_key" ON "Post"("Guid");

-- AddForeignKey
ALTER TABLE "SocialMediaAccount" ADD CONSTRAINT "SocialMediaAccount_UserId_fkey" FOREIGN KEY ("UserId") REFERENCES "User"("Id") ON DELETE RESTRICT ON UPDATE CASCADE;

-- AddForeignKey
ALTER TABLE "Category" ADD CONSTRAINT "Category_PostId_fkey" FOREIGN KEY ("PostId") REFERENCES "Post"("Id") ON DELETE SET NULL ON UPDATE CASCADE;

-- AddForeignKey
ALTER TABLE "Image" ADD CONSTRAINT "Image_SectionId_fkey" FOREIGN KEY ("SectionId") REFERENCES "Section"("Id") ON DELETE RESTRICT ON UPDATE CASCADE;

-- AddForeignKey
ALTER TABLE "Title" ADD CONSTRAINT "Title_SectionId_fkey" FOREIGN KEY ("SectionId") REFERENCES "Section"("Id") ON DELETE RESTRICT ON UPDATE CASCADE;

-- AddForeignKey
ALTER TABLE "Text" ADD CONSTRAINT "Text_SectionId_fkey" FOREIGN KEY ("SectionId") REFERENCES "Section"("Id") ON DELETE RESTRICT ON UPDATE CASCADE;

-- AddForeignKey
ALTER TABLE "Video" ADD CONSTRAINT "Video_SectionId_fkey" FOREIGN KEY ("SectionId") REFERENCES "Section"("Id") ON DELETE RESTRICT ON UPDATE CASCADE;

-- AddForeignKey
ALTER TABLE "Section" ADD CONSTRAINT "Section_PostId_fkey" FOREIGN KEY ("PostId") REFERENCES "Post"("Id") ON DELETE RESTRICT ON UPDATE CASCADE;

-- AddForeignKey
ALTER TABLE "Link" ADD CONSTRAINT "Link_PostId_fkey" FOREIGN KEY ("PostId") REFERENCES "Post"("Id") ON DELETE SET NULL ON UPDATE CASCADE;

-- AddForeignKey
ALTER TABLE "DevelopmentTool" ADD CONSTRAINT "DevelopmentTool_PostId_fkey" FOREIGN KEY ("PostId") REFERENCES "Post"("Id") ON DELETE SET NULL ON UPDATE CASCADE;

-- AddForeignKey
ALTER TABLE "Like" ADD CONSTRAINT "Like_UserId_fkey" FOREIGN KEY ("UserId") REFERENCES "User"("Id") ON DELETE RESTRICT ON UPDATE CASCADE;

-- AddForeignKey
ALTER TABLE "Like" ADD CONSTRAINT "Like_PostId_fkey" FOREIGN KEY ("PostId") REFERENCES "Post"("Id") ON DELETE SET NULL ON UPDATE CASCADE;

-- AddForeignKey
ALTER TABLE "Like" ADD CONSTRAINT "Like_CommentId_fkey" FOREIGN KEY ("CommentId") REFERENCES "Comment"("Id") ON DELETE SET NULL ON UPDATE CASCADE;

-- AddForeignKey
ALTER TABLE "Comment" ADD CONSTRAINT "Comment_PostId_fkey" FOREIGN KEY ("PostId") REFERENCES "Post"("Id") ON DELETE SET NULL ON UPDATE CASCADE;

-- AddForeignKey
ALTER TABLE "Comment" ADD CONSTRAINT "Comment_ParentId_fkey" FOREIGN KEY ("ParentId") REFERENCES "Comment"("Id") ON DELETE SET NULL ON UPDATE CASCADE;
