import { ToolSchema } from "@/domain/schemas/ToolSchema";
import { apiServer } from "./api-server";
import { ToolFilters } from "@/domain/schemas/ToolFilters";

export const addToolService = async(tool: ToolSchema) =>{
    const api = await apiServer();
    const response = await api.post("/api/Tool",tool);
    return response;
}
export const updateToolService = async(id:string, tool: ToolSchema) =>{
    const api = await apiServer();
    const response = await api.put(`/api/Tool/${id}`,tool);
    return response;
}
export const deleteToolByRouteService = async(id:string) =>{
    const api = await apiServer();
    const response = await api.delete(`/api/tools/${id}`);
    return response;
}
export const getToolsByPagination = async(filters: ToolFilters) =>{
    const api = await apiServer();
    const params = new URLSearchParams();
    params.append("page", filters.page.toString());
    if(filters.search){
        params.append("search", filters.search);
    }
    const response = await api.get(`/api/Tool/GetByPagination?${params}`);
    return response;
}
export const getToolByIdService = async(id:string) =>{
    const api = await apiServer();
    const response = await api.get(`/api/Tool/GetToolById/${id}`);
    return response;
}