import { CategoriesFilters } from "@/domain/interfaces/CategoriesFilters";
import { getCategoriesByPaginationService } from "@/services/client/category-services";
import { useQuery } from "@tanstack/react-query";

export function useCategories(categoriesFilters: CategoriesFilters){
    return useQuery({
        queryKey: ["categories", categoriesFilters],
        queryFn: async () =>{
            const response = await getCategoriesByPaginationService(categoriesFilters);
            if (response.status != 200) {
                throw new Error(response.data.message)
            }
            return response.data
        }
    })
}