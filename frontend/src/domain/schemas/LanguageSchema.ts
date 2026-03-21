import z from "zod";
export const languageSchema = z.object({
    id: z.uuid().optional(),
    code: z.string(),
    name: z.string(),
    createdAt: z.string().optional(),
    updatedAt: z.string().optional()
});
export type LanguageSchema = z.infer<typeof languageSchema>;