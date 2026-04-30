import z from "zod";

export const addImageMarkDownSchema = z.object({
    file: z.instanceof(File)
        .refine((file) => file.size <= 2_000_000, "Max 2 MB")
        .refine((file) => ["image/jpeg", "image/png", "image/webp"].includes(file.type), "Formato inválido"),
})