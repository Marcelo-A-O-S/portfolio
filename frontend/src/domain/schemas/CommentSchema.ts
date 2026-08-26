import z from "zod";
import { userSchema } from "./UserSchema";
export const commentTypeSchema = z.enum(["Post", "Tool", "Blog", "Certificate"])
export const commentStatus = z.enum(["Active","Deleted"]);
export const commentDeletionType = z.enum(["None","User","Moderator","Administrador"])
const commentBaseSchema = z.object({
    id: z.uuid().optional(),
    targetId: z.uuid(),
    type: commentTypeSchema,
    content: z.string().nonempty(),
    user: userSchema.optional(),
    likes: z.number().optional(),
    liked: z.boolean().optional(),
    createdAt: z.string().optional(),
    deletedAt: z.string().optional(),
    commentStatus: commentStatus.optional(),
    commentDeletion: commentDeletionType.optional()
})
export const commentSchema = commentBaseSchema.extend({
    parentCommentId: z.uuid().optional().nullable(),
    comment: commentBaseSchema.optional(),
    replies: z.array(commentBaseSchema).optional()
})
export type CommentSchema = z.infer<typeof commentSchema>;