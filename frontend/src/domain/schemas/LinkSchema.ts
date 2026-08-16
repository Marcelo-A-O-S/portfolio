import z from "zod";
import { linkTypeSchema } from "./LinkTypeSchema";
import { postBaseSchema } from "./PostBaseSchema";
export const linkSchema = z.object({
    id: z.uuid().optional(),
    url: z.string(),
    title: z.string(),
    linkTypeId: z.uuid().optional(),
    linkType: linkTypeSchema.optional(),
    toolId: z.uuid().optional(),
    postId: z.uuid().optional()
})
export type LinkSchema = z.infer<typeof linkSchema>;