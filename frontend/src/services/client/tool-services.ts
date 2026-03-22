import { ToolSchema } from "@/domain/schemas/ToolSchema";
import { apiClient } from "./api-client";
import { ToolFilters } from "@/domain/schemas/ToolFilters";

export const addToolService = async(tool: ToolSchema) =>{
    const api = await apiClient();
    const response = await api.post("/api/admin/tools",tool);
    return response;
}
export const updateToolService = async(id:string, tool: ToolSchema) =>{
    const api = await apiClient();
    const response = await api.put(`/api/admin/tools/${id}`,tool);
    return response;
}
export const deleteToolByRouteService = async(id:string) =>{
    const api = await apiClient();
    const response = await api.delete(`/api/admin/tools/${id}`);
    return response;
}
export const getToolsByPagination = async(filters: ToolFilters) =>{
    const api = await apiClient();
    const params = new URLSearchParams();
    params.append("page", filters.page.toString());
    if(filters.search){
        params.append("search", filters.search);
    }
    const response = await api.get(`/api/admin/tools/pagination?${params}`);
    return response;
}
export const getToolByIdService = async(id:string) =>{
    const api = await apiClient();
    const response = await api.get(`/api/admin/tools/${id}`);
    return response;
}