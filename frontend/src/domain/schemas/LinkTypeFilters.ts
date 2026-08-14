import z from "zod";

export const linkTypeFilters = z.object({
    page: z.number(),
    search: z.string().optional(),
})
export type LinkTypeFilters = z.infer<typeof linkTypeFilters>