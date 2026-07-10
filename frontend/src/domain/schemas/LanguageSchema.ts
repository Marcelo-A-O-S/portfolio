import z from "zod";
export const languageSchema = z.object({
    id: z.uuid().optional(),
    code: z.string("O código da linguagem é obrigatório."),
    name: z.string("O nome da linguagem é obrigatório."),
    createdAt: z.string().optional(),
    updatedAt: z.string().optional()
});
export type LanguageSchema = z.infer<typeof languageSchema>;