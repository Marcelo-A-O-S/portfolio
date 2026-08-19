import { contributorFilters } from "@/domain/schemas/ContributorFilters";
import { ContributorSchema } from "@/domain/schemas/ContributorSchema";
import { PaginatedResult } from "@/domain/types/PaginatedResult";
import { getContributorsPagination } from "@/services/client/post-services";
import { useQuery } from "@tanstack/react-query";

export function usePaginationContributor(filters: contributorFilters) {
    return useQuery<PaginatedResult<ContributorSchema>>({
        queryKey: ["contributor-pagination", filters],
        queryFn: async () => {
            const response = await getContributorsPagination(filters);
            if (response.status != 200) {
                throw new Error(response.data.message)
            }
            return response.data
        }
    })
}