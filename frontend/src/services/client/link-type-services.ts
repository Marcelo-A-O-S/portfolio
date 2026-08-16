import { LinkTypeSchema } from "@/domain/schemas/LinkTypeSchema";
import { apiClient } from "./api-client";
import { LinkTypeFilters } from "@/domain/schemas/LinkTypeFilters";
export const getLinkTypeByPagination = async(filters: LinkTypeFilters) =>{
    const api = await apiClient();
    const params = new URLSearchParams();
    params.append("page", filters.page.toString());
    if(filters.search){
        params.append("page", filters.search);
    }
    const response = await api.get(`/api/admin/linkTypes/pagination?${params}`);
    return response;
}
export const getLinkTypes = async() => {
    const api = await apiClient();
    const response = await api.get(`/api/admin/linkTypes`);
    return response;
}
export const addLinkType = async (data: LinkTypeSchema) => {
    const api = await apiClient();
    const response = await api.post("/api/admin/linkTypes", data);
    return response;
}
export const updateLinkType = async (id: string, data: LinkTypeSchema) => {
    const api = await apiClient();
    const response = await api.put(`/api/admin/linkTypes/${id}`, data);
    return response;
}
export const deleteLinkType = async (id: string, data: LinkTypeSchema) => {
    const api = await apiClient();
    const response = await api.delete(`/api/admin/linkTypes/${id}`,{
        data: data
    });
    return response;
}
