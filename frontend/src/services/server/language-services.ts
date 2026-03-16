import { LanguageFilters } from "@/domain/schemas/LanguageFilters";
import { apiServer } from "./api-server";

export const getLanguagesByPagination = async(filters: LanguageFilters) =>{
    const api = await apiServer();
    const params = new URLSearchParams();
    params.append("page", filters.page.toString());
    if(filters.search){
        params.append("search",filters.search);
    }
    if(filters.code){
        params.append("code", filters.code);
    }
    console.log("Chamando backend .NET...");
    console.log(`/api/Language/GetByPagination?${params}`);
    const response = await api.get(`/api/Language/GetByPagination?${params}`);
    return response;
}