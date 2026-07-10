import z from "zod";
import { commentTypeSchema } from "./CommentSchema";

export const commentFilters = z.object({
    targetId: z.uuid(),
    type: commentTypeSchema,
    page: z.number()
})
export type CommentFilters = z.infer<typeof commentFilters>;