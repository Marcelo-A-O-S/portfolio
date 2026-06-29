import { ToolSchema } from "@/domain/schemas/ToolSchema";
import { apiServer } from "./api-server";
import { ToolFilters } from "@/domain/schemas/ToolFilters";
import { LikeSchema, likeTypeSchema, } from "@/domain/schemas/LikeSchema";

export const addToolService = async (tool: ToolSchema) => {
    const api = await apiServer();
    const response = await api.post("/api/Tool", tool);
    return response;
}
export const updateToolService = async (id: string, tool: ToolSchema) => {
    const api = await apiServer();
    const response = await api.put(`/api/Tool/${id}`, tool);
    return response;
}
export const deleteToolByRouteService = async (id: string) => {
    const api = await apiServer();
    const response = await api.delete(`/api/Tool/${id}`);
    return response;
}
export const getToolsByPagination = async (filters: ToolFilters) => {
    const api = await apiServer();
    const params = new URLSearchParams();
    params.append("page", filters.page.toString());
    if (filters.search) {
        params.append("search", filters.search);
    }
    const response = await api.get(`/api/Tool/GetByPagination?${params}`);
    return response;
}
export const getToolByIdService = async (id: string) => {
    const api = await apiServer();
    const response = await api.get(`/api/Tool/GetToolById/${id}`);
    return response;
}
export const getTools = async () => {
    const api = await apiServer();
    const response = await api.get(`/api/Tool/GetTools`);
    return response;
}
export const addToolLike = async (data: LikeSchema) => {
    const api = await apiServer();
    if (data.type != "Tool")
        throw new Error("Só é possivel dar curtidas em postagens de ferramentas.");
    const response = await api.post(`/api/Like`, data);
    return response;
}
export const removeToolLike = async (data: LikeSchema) => {
    const api = await apiServer();
    if (data.type != "Tool")
        throw new Error("Só é possivel dar curtidas em postagens de ferramentas.");
    const response = await api.post(`/api/Like`, data);
    return response;
}