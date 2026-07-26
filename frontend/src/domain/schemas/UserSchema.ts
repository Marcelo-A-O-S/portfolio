import z from "zod";

export const userSchema = z.object({
    id: z.uuid(),
    username: z.string(),
    profileUrl: z.string()
})