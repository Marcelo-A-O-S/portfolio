import z from "zod";
export const languageSchema = z.object({
    id: z.uuid().optional(),
    code: z.string(),
    name: z.string(),
    createdAt: z.date().optional(),
    updatedAt: z.date().optional()
});
export type LanguageSchema = z.infer<typeof languageSchema>;