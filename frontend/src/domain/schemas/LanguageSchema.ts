import z from "zod";
export const languageSchema = z.object({
    id: z.uuid().optional(),
    code: z.string().nonempty("O código da linguagem é obrigatório."),
    name: z.string().nonempty("O nome da linguagem é obrigatório."),
    createdAt: z.date().optional(),
    updatedAt: z.date().optional()
});
export type LanguageSchema = z.infer<typeof languageSchema>;