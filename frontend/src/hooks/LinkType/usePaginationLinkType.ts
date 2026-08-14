import { LinkTypeFilters } from "@/domain/schemas/LinkTypeFilters";
import { LinkTypeSchema } from "@/domain/schemas/LinkTypeSchema";
import { PaginatedResult } from "@/domain/types/PaginatedResult";
import { getLinkTypeByPagination } from "@/services/client/link-type-services";
import { useQuery } from "@tanstack/react-query";

export function usePaginationLinkType(filters: LinkTypeFilters) {
    return useQuery<PaginatedResult<LinkTypeSchema>>({
        queryKey: ["link-type-pagination", filters],
        queryFn: async () => {
            const response = await getLinkTypeByPagination(filters);
            if (response.status != 200) {
                throw new Error(response.data.message)
            }
            return response.data
        }
    })
}