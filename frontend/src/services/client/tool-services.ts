import { ToolSchema } from "@/domain/schemas/ToolSchema";
import { apiClient } from "./api-client";

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
    const response = await api.delete(`/api/tools/${id}`);
    return response;
}