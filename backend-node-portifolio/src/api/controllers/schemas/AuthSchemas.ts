import {z} from "zod"
export const SocialMediaAccountSchema = z.object({
    Id: z.number(),
    Provider: z.string().min(1,"Informe o provedor e tente novamente!"),
    SocialId: z.string().min(1,"Informe o ID relacionado ao provedor e tente novamente!"),
    Email: z.string().email("Email inválido, corrija e tente novamente!"),
    Username: z.string().min(1,"Informe o username e tente novamente!")
})
export const loginSchema = z.object({
    Id: z.number(),
    Guid: z.string(),
    Name: z.string().min(1,"Informe o nome do usuário e tente novamente!"),
    Accounts: z.array(SocialMediaAccountSchema).optional()
})