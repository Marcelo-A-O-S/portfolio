import { ToolFilters } from "@/domain/schemas/ToolFilters";
import { ToolSchema } from "@/domain/schemas/ToolSchema";
import { PaginatedResult } from "@/domain/types/PaginatedResult";
import { getToolsByPagination } from "@/services/client/tool-services";
import { useQuery } from "@tanstack/react-query";

export function usePaginationTool(filters: ToolFilters){
    return useQuery<PaginatedResult<ToolSchema>>({
        queryKey: ["tool-pagination", filters],
        queryFn: async() =>{
            const response = await getToolsByPagination(filters);
            if (response.status != 200) {
                throw new Error(response.data.message)
            }
            return response.data
        }
    })
}