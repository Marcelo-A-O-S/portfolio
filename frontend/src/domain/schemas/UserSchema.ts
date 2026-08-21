import z from "zod";

export const userSchema = z.object({
    id: z.uuid(),
    name: z.string().optional(),
    username: z.string(),
    profileUrl: z.string(),
    description: z.string().optional()
})
export type UserSchema = z.infer<typeof userSchema>;