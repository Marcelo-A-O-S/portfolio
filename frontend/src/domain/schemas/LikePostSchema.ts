import z from "zod";

export const likePostSchema = z.object({
    postId: z.uuid("O identificador da postagem ou projeto é obrigatório")
})
export type  LikePostSchema = z.infer<typeof likePostSchema>;