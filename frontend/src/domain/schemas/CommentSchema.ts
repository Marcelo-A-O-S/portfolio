import z from "zod";
import { userSchema } from "./UserSchema";
export const commentTypeSchema = z.enum(["Post","Tool","Blog","Certificate"])
const commentBaseSchema = z.object({
    id: z.uuid().optional(),
    targetId: z.uuid(),
    type: commentTypeSchema,
    content: z.string({}),
    user: userSchema.optional(),
    createdAt: z.string().optional()
})
export const commentSchema = commentBaseSchema.extend({
    parentCommentId: z.uuid().optional().nullable(),
    comment: commentBaseSchema.optional(),
    replies: z.array(commentBaseSchema).optional()
})
export type CommentSchema = z.infer<typeof commentSchema>;