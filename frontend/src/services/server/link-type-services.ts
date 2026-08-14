import { LinkTypeSchema } from "@/domain/schemas/LinkTypeSchema";
import { apiServer } from "./api-server";
import { LinkTypeFilters } from "@/domain/schemas/LinkTypeFilters";
export const getLinkTypeByPagination = async(filters: LinkTypeFilters) =>{
    const api = await apiServer();
    const params = new URLSearchParams();
    params.append("page", filters.page.toString());
    if(filters.search){
        params.append("page", filters.search);
    }
    const response = await api.get(`/api/LinkType/GetByPagination?${params}`);
    return response;
}
export const addLinkType = async (data: LinkTypeSchema) => {
    const api = await apiServer();
    const response = await api.post("/api/LinkType", data);
    return response;
}
export const updateLinkType = async (id: string, data: LinkTypeSchema) => {
    const api = await apiServer();
    const response = await api.put(`/api/LinkType/${id}`, data);
    return response;
}
export const deleteLinkType = async (id: string, data: LinkTypeSchema) => {
    const api = await apiServer();
    const response = await api.delete(`/api/LinkType/${id}`,{
        data: data
    });
    return response;
}