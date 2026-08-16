import { LinkSchema } from "@/domain/schemas/LinkSchema";
import { apiServer } from "./api-server";
import { LinkFilters } from "@/domain/schemas/LinkFilters";
export const getLinksByPagination = async(filters: LinkFilters) =>{
    const api = await apiServer();
    const params = new URLSearchParams();
    params.append("page", filters.page.toString());
    if(filters.tooId){
        params.append("toolId", filters.tooId.toString())
    }
    if(filters.search){
        params.append("page", filters.search);
    }
    const response = await api.get(`/api/Links/GetByPagination?${params}`);
    return response;
}
export const addLink = async(data: LinkSchema) =>{
    const api = await apiServer();
    const response = await api.post(`/api/Links`, data);
    return response;
}
export const updateLink = async(id: string, data: LinkSchema) =>{
    const api = await apiServer();
    const response = await api.put(`/api/Links/${id}`,data);
    return response;
}
export const deleteLink = async(id: string, data: LinkSchema) => {
    const api = await apiServer();
    const response = await api.delete(`/api/Links/${id}`,{
        data: data
    })
    return response;
}