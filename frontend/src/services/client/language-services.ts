import { LanguageFilters } from "@/domain/schemas/LanguageFilters";
import { apiClient } from "./api-client"
import { LanguageSchema } from "@/domain/schemas/LanguageSchema";

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
export const addLanguageService = async(data: LanguageSchema) =>{
    const api = await apiClient();
    const response = await api.post(`/api/admin/languages`, data);
    return response;
}
export const updateLanguageService = async(id: string, data: LanguageSchema) =>{
    const api = await apiClient();
    const response = await api.put(`/api/admin/languages/${id}`, data);
    return response;
}
export const deleteLanguageService = async(id: string) => {
    const api = await apiClient();
    const response = await api.delete(`/api/admin/languages/${id}`);
    return response;
}
export const getLanguages = async()=>{
    const api = await apiClient();
    const response = await api.get(`/api/admin/languages`);
    return response;
}