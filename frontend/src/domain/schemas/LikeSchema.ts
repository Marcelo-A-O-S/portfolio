import z from "zod";
const LikeType = z.enum(["Post", "Comment", "Tool", "Blog"])
const likeSchema = z.object({
    id: z.uuid().optional(),
    userId: z.uuid(),
    targetId: z.uuid(),
    type: LikeType
})
export type LikeSchema = z.infer<typeof likeSchema>;