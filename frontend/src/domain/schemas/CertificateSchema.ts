import z from "zod";
export const certificateSchema = z.object({
    id: z.string().nonempty(),
    educationalInstitution: z.string().nonempty(),
    diploma: z.string().nonempty(),
    description: z.string().nonempty(),
    startDate: z.date(),
    endDate: z.date().optional()
})