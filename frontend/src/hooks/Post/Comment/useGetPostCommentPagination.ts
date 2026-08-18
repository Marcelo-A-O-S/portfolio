import { CommentFilters } from "@/domain/schemas/CommentFilters";
import { CommentSchema } from "@/domain/schemas/CommentSchema";
import { PaginatedResult } from "@/domain/types/PaginatedResult";
import { getPostCommentsByPagination } from "@/services/client/post-services";
import { useQuery } from "@tanstack/react-query";

export function useGetPostCommentPagination(filters: CommentFilters){
    return useQuery<PaginatedResult<CommentSchema>>({
        queryKey: ["tool-comment-pagination", filters],
        queryFn: async () => {
            const response = await getPostCommentsByPagination(filters);
            return response.data 
        },
    })
}