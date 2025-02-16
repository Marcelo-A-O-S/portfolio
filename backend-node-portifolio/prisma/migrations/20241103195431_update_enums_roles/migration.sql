-- CreateEnum
CREATE TYPE "ERoles" AS ENUM ('ADMIN', 'USER');

-- AlterTable
ALTER TABLE "User" ADD COLUMN     "Role" "ERoles" NOT NULL DEFAULT 'USER';
