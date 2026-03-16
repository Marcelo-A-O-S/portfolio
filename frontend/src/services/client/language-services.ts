import { LanguageFilters } from "@/domain/schemas/LanguageFilters";
import { apiClient } from "./api-client"

export const getLanguagesByPagination = async(filters: LanguageFilters) =>{
    const api = await apiClient();
    const params = new URLSearchParams();
    params.append("page", filters.page.toString());
    if(filters.search){
        params.append("search",filters.search);
    }
    if(filters.code){
        params.append("code", filters.code);
    }
    const response = await api.get(`/api/admin/languages/pagination?${params}`);
    return response;
}