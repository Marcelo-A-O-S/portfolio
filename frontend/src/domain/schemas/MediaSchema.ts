import z from "zod";
export const mediaSchema = z.object({
    id: z.uuid().optional(),
    mediaId: z.uuid(),
    url: z.string(),
    ownerType: z.string(),
    file: z.instanceof(File)
        .refine((file) => file.size <= 2_000_000, "Max 2 MB")
        .refine((file) => ["image/jpeg", "image/png", "image/webp"].includes(file.type), "Formato inválido")
        .optional(),
})
export const mediaRequestSchema = z.object({
    ownerId: z.uuid().optional(),
    ownerType: z.string(),
    file: z.instanceof(File)
        .refine((file) => file.size <= 2_000_000, "Max 2 MB")
        .refine((file) => ["image/jpeg", "image/png", "image/webp"].includes(file.type), "Formato inválido"),
})
export type MediaRequestSchema = z.infer<typeof mediaRequestSchema>;
export type MediaSchema = z.infer<typeof mediaSchema>