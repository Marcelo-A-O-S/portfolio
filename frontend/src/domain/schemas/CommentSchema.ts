import z from "zod";
export const commentTypeSchema = z.enum(["Post","Tool","Blog","Certificate"])
const commentBaseSchema = z.object({
    id: z.uuid().optional(),
    targetId: z.uuid(),
    type: commentTypeSchema,
    content: z.string(),
})
export const commentSchema = commentBaseSchema.extend({
    parentCommentId: z.uuid().optional(),
    comment: commentBaseSchema.optional(),
    replies: z.array(commentBaseSchema)
})
export type CommentSchema = z.infer<typeof commentSchema>;