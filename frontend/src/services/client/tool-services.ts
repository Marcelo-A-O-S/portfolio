import { ToolSchema } from "@/domain/schemas/ToolSchema";
import { apiClient } from "./api-client";
import { ToolFilters } from "@/domain/schemas/ToolFilters";
import { LikeSchema } from "@/domain/schemas/LikeSchema";
import { CommentSchema } from "@/domain/schemas/CommentSchema";

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
export const getTools = async()=>{
    const api = await apiClient();
    const response = await api.get(`/api/admin/tools`);
    return response;
}
export const addToolLike = async (data: LikeSchema) => {
    const api = await apiClient();
    if (data.type != "Tool")
        throw new Error("Só é possivel dar curtidas em postagens de ferramentas.");
    const response = await api.post(`/api/admin/tools/likes`, data);
    return response;
}
export const removeToolLike = async (data: LikeSchema) => {
    const api = await apiClient();
    if (data.type != "Tool")
        throw new Error("Só é possivel dar curtidas em postagens de ferramentas.");
    const response = await api.delete(`/api/admin/tools/likes`, {
        data: data
    });
    return response;
}
export const addToolComment = async(data: CommentSchema) =>{
    const api = await apiClient();
    if(data.type != "Tool")
        throw new Error("Só é possivel comentar em uma postagem de ferramenta")
    const response = await api.post(`/api/admin/tools/comments`,data);
    return response;
}
export const removeToolComment = async(data: CommentSchema) =>{
    const api = await apiClient();
    if(data.type != "Tool")
        throw new Error("Só é possivel comentar em uma postagem de ferramenta")
    const response = await api.delete(`/api/admin/tools/comments`,{
        data: data
    });
    return response;
}