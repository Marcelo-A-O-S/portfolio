import { CategoriesFilters } from "@/domain/schemas/CategoriesFilters";
import { getCategoriesByPaginationService } from "@/services/client/category-services";
import { useQuery } from "@tanstack/react-query";

export function useGetCategories(categoriesFilters: CategoriesFilters){
    return useQuery({
        queryKey: ["categories-pagination", categoriesFilters],
        queryFn: async () =>{
            const response = await getCategoriesByPaginationService(categoriesFilters);
            if (response.status != 200) {
                throw new Error(response.data.message)
            }
            return response.data
        }
    })
}