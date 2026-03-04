import { ToolSchema } from "@/domain/schemas/ToolSchema";
import { apiServer } from "./api-server";

export const addToolService = async(tool: ToolSchema) =>{
    const api = await apiServer();
    const response = await api.post("/api/tools",tool);
    return response;
}
export const updateToolService = async(id:string, tool: ToolSchema) =>{
    const api = await apiServer();
    const response = await api.put(`/api/tools/${id}`,tool);
    return response;
}
export const deleteToolByRouteService = async(id:string) =>{
    const api = await apiServer();
    const response = await api.delete(`/api/tools/${id}`);
    return response;
}