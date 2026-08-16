import { LinkSchema } from "@/domain/schemas/LinkSchema";
import { apiClient } from "./api-client";
import { LinkFilters } from "@/domain/schemas/LinkFilters";
export const getLinksByPagination = async(filters: LinkFilters) =>{
    const api = await apiClient();
    const params = new URLSearchParams();
    params.append("page", filters.page.toString());
    params.append("postBaseId", filters.postBaseId.toString())
    if(filters.search){
        params.append("page", filters.search);
    }
    const response = await api.get(`/api/admin/links/pagination?${params}`);
    return response;
}
export const addLink = async(data: LinkSchema) =>{
    const api = await apiClient();
    const response = await api.post(`/api/admin/links`, data);
    return response;
}
export const updateLink = async(id: string, data: LinkSchema) =>{
    const api = await apiClient();
    const response = await api.put(`/api/admin/links/${id}`,data);
    return response;
}
export const deleteLink = async(id: string, data: LinkSchema) => {
    const api = await apiClient();
    const response = await api.delete(`/api/admin/links/${id}`,{
        data: data
    })
    return response;
}