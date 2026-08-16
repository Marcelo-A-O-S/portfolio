import { LinkFilters } from "@/domain/schemas/LinkFilters";
import { LinkSchema } from "@/domain/schemas/LinkSchema";
import { PaginatedResult } from "@/domain/types/PaginatedResult";
import { getLinksByPagination } from "@/services/client/link-services";
import { useQuery, useQueryClient } from "@tanstack/react-query";
export function usePaginationLink(filters: LinkFilters) {
    return useQuery<PaginatedResult<LinkSchema>>({
        queryKey: ["link-pagination", filters],
        queryFn: async () => {
            const response = await getLinksByPagination(filters);
            if (response.status != 200) {
                throw new Error(response.data.message);
            }
            return response.data;
        }
    })
}