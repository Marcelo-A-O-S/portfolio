import z from "zod";

export const authorSchema = z.object({
    username: z.string(),
    profileUrl: z.string(),
    profile: z.string().optional(),
    description: z.string().optional()
});
export type AuthorSchema = z.infer<typeof authorSchema>;