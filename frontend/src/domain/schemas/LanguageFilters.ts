import z from "zod";

export const languageFilters = z.object({
    page : z.number(),
    search: z.string().optional(),
    code: z.string().optional()
})
export type LanguageFilters = z.infer<typeof languageFilters>