import { LanguageFilters } from "@/domain/schemas/LanguageFilters";
import { apiServer } from "./api-server";
import { LanguageSchema } from "@/domain/schemas/LanguageSchema";

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
    const response = await api.get(`/api/Language/GetByPagination?${params}`);
    return response;
}
export const addLanguageService = async(data: LanguageSchema) =>{
    const api = await apiServer();
    const response = await api.post(`/api/Language`, data);
    return response;
}
export const updateLanguageService = async(id: string, data: LanguageSchema) =>{
    const api = await apiServer();
    const response = await api.put(`/api/Language/${id}`, data);
    return response;
}
export const deleteLanguageService = async(id: string) => {
    const api = await apiServer();
    const response = await api.delete(`/api/Language/${id}`);
    return response;
}
export const getLanguages = async()=>{
    const api = await apiServer();
    const response = await api.get(`/api/Language`);
    return response;
}