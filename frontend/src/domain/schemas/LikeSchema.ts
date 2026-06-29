import z from "zod";
export const likeTypeSchema = z.enum(["Post", "Comment", "Tool", "Blog"])
export const likeSchema = z.object({
    id: z.uuid().optional(),
    targetId: z.uuid(),
    type: likeTypeSchema
})
export type LikeSchema = z.infer<typeof likeSchema>;
export type LikeTypeSchema = z.infer<typeof likeTypeSchema>;
