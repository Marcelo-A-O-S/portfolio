import { CertificateFilters } from "@/domain/schemas/CertificateFilters";
import { CertificateSchema } from "@/domain/schemas/CertificateSchema";
import { PaginatedResult } from "@/domain/types/PaginatedResult";
import { getCertificatesByPagination } from "@/services/client/certificate-services";
import { useQuery } from "@tanstack/react-query";

export function usePaginationCertificate(filters: CertificateFilters) {
    return useQuery<PaginatedResult<CertificateSchema>>({
        queryKey: ["certificate-pagination", filters],
        queryFn: async () => {
            const response = await getCertificatesByPagination(filters);
            if (response.status != 200) {
                throw new Error(response.data.message)
            }
            return response.data
        }
    })
}