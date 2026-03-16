import { LanguageFilters } from "@/domain/schemas/LanguageFilters";
import { getLanguagesByPagination } from "@/services/client/language-services";
import { useQuery } from "@tanstack/react-query";
export default function useGetLanguages(languageFilters: LanguageFilters){
    return useQuery({
        queryKey: ["languages", languageFilters],
        queryFn: async ()=> {
            const response = await getLanguagesByPagination(languageFilters);
            if (response.status != 200) {
                throw new Error(response.data.message)
            }
            return response.data
        }
    })
}