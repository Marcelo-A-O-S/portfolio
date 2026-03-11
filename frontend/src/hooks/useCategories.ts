import { CategoriesFilters } from "@/domain/interfaces/CategoriesFilters";
import { useQuery } from "@tanstack/react-query";

export function useCategories(categoriesFilters: CategoriesFilters){
    return useQuery({
        queryKey: ["categories", categoriesFilters],
        queryFn: async () =>{

        }
    })
}