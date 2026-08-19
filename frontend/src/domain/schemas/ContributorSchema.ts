import z from "zod";

export const contributorSchema = z.object({
    id: z.uuid().optional(),
    userId: z.uuid().optional(),
    postId: z.uuid(),
    name: z.string().nonempty(),
    description: z.string().optional(),
    profileUrl: z.string().optional()
})
export type ContributorSchema = z.infer<typeof contributorSchema>;