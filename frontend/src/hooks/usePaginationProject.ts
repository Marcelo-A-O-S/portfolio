import { PostSchema } from "@/domain/schemas/PostSchema";
import { PostsFilters } from "@/domain/schemas/PostsFilters";
import { PaginatedResult } from "@/domain/types/PaginatedResult";
import { getPostsByPagination } from "@/services/client/post-services";
import { useQuery } from "@tanstack/react-query";
export function usePaginationProject(filters: PostsFilters){
    return useQuery<PaginatedResult<PostSchema>>({
        queryKey: ["project-pagination", filters],
        queryFn: async()=>{
            const response = await getPostsByPagination(filters);
            if (response.status != 200) {
                throw new Error(response.data.message)
            }
            return response.data
        }
    })
}