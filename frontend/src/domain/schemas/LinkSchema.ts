import z from "zod";
import { linkTypeSchema } from "./LinkTypeSchema";

export const linkSchema = z.object({
    id: z.uuid().optional(),
    url: z.string(),
    title: z.string(),
    linkType: linkTypeSchema.optional()
})
export type LinkSchema = z.infer<typeof linkSchema>;