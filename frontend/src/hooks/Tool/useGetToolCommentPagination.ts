import { CommentFilters } from "@/domain/schemas/CommentFilters";
import { CommentSchema } from "@/domain/schemas/CommentSchema";
import { PaginatedResult } from "@/domain/types/PaginatedResult";
import { getToolCommentsByPagination } from "@/services/client/tool-services";
import { useQuery } from "@tanstack/react-query";

export function useGetToolCommentPagination(filters: CommentFilters) {
    return useQuery<PaginatedResult<CommentSchema>>({
        queryKey: ["tool-comment-pagination", filters],
        queryFn: async () => {
            const response = await getToolCommentsByPagination(filters);
            return response.data 
        },
    })
}