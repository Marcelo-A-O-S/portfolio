import z from "zod";

export const toolContentSchema = z.object({
    id: z.uuid().optional(),
    toolId: z.uuid().optional(),
    
})