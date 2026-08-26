import z from "zod";

export const certificateFilters = z.object({
    page: z.number(),
    search: z.string().optional()
})
export type CertificateFilters = z.infer<typeof certificateFilters>;