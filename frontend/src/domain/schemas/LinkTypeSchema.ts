import z from "zod";

export const linkTypeSchema = z.object({
    id: z.uuid().optional(),
    name: z.string("O nome do tipo é obrigatório.").min(3,"O nome do tipo é obrigatório."),
    backgroundColor: z.string("A cor de fundo é obrigatória.").min(3,"A cor de fundo é obrigatória."),
    textColor: z.string("A cor de texto é obrigatória.").min(3,"A cor de texto é obrigatória."),
    borderColor: z.string("A cor da borda é obrigatória.").min(3,"A cor da borda é obrigatória."),
    icon: z.string("O Icone é obrigatório.").min(3,"O Icone é obrigatório.")
})
export type LinkTypeSchema = z.infer<typeof linkTypeSchema>;