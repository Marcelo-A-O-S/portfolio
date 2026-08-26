import z from "zod";

export const linkTypeSchema = z.object({
    id: z.uuid().optional(),
    name: z.string().min(3,"O nome do tipo é obrigatório.").nonempty("O nome do tipo é obrigatório."),
    backgroundColor: z.string().min(3,"A cor de fundo é obrigatória.").nonempty("A cor de fundo é obrigatória."),
    textColor: z.string().min(3,"A cor de texto é obrigatória.").nonempty("A cor de texto é obrigatória."),
    borderColor: z.string().min(3,"A cor da borda é obrigatória.").nonempty("A cor da borda é obrigatória."),
    icon: z.string().min(3,"O Icone é obrigatório.").nonempty("O Icone é obrigatório.")
})
export type LinkTypeSchema = z.infer<typeof linkTypeSchema>;