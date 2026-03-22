import z from "zod";

export const toolFilters = z.object({
    page: z.number(),
    search: z.string().optional()
})
export type ToolFilters = z.infer<typeof toolFilters>