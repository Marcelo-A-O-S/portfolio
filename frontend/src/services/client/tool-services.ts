import { ToolSchema } from "@/domain/schemas/ToolSchema";
import { apiClient } from "./api-client";
import { ToolFilters } from "@/domain/schemas/ToolFilters";
import { LikeSchema } from "@/domain/schemas/LikeSchema";
import { CommentSchema } from "@/domain/schemas/CommentSchema";
import { CommentFilters } from "@/domain/schemas/CommentFilters";

export const addToolService = async (tool: ToolSchema) => {
    const api = await apiClient();
    const response = await api.post("/api/admin/tools", tool);
    return response;
}
export const updateToolService = async (id: string, tool: ToolSchema) => {
    const api = await apiClient();
    const response = await api.put(`/api/admin/tools/${id}`, tool);
    return response;
}
export const deleteToolByRouteService = async (id: string) => {
    const api = await apiClient();
    const response = await api.delete(`/api/admin/tools/${id}`);
    return response;
}
export const getToolsByPagination = async (filters: ToolFilters) => {
    const api = await apiClient();
    const params = new URLSearchParams();
    params.append("page", filters.page.toString());
    if (filters.search) {
        params.append("search", filters.search);
    }
    const response = await api.get(`/api/admin/tools/pagination?${params}`);
    return response;
}
export const getToolByIdService = async (id: string) => {
    const api = await apiClient();
    const response = await api.get(`/api/admin/tools/${id}`);
    return response;
}
export const getTools = async () => {
    const api = await apiClient();
    const response = await api.get(`/api/admin/tools`);
    return response;
}
export const addToolLike = async (data: LikeSchema) => {
    const api = await apiClient();
    if (data.type != "Tool")
        throw new Error("Só é possivel dar curtidas em postagens de ferramentas.");
    const response = await api.post(`/api/tools/likes`, data);
    return response;
}
export const removeToolLike = async (data: LikeSchema) => {
    const api = await apiClient();
    if (data.type != "Tool")
        throw new Error("Só é possivel dar curtidas em postagens de ferramentas.");
    const response = await api.delete(`/api/tools/likes`, {
        data: data
    });
    return response;
}
export const addToolComment = async (data: CommentSchema) => {
    const api = await apiClient();
    if (data.type != "Tool")
        throw new Error("Só é possivel comentar em uma postagem de ferramenta")
    const response = await api.post(`/api/tools/comments`, data);
    return response;
}
export const updateToolComment = async (id: string, data: CommentSchema) => {
    const api = await apiClient();
    if (data.type != "Tool")
        throw new Error("Só é possivel comentar em uma postagem de ferramenta")
    const response = await api.put(`/api/tools/comments/${id}`, data);
    return response;
}
export const removeToolCommentById = async (id: string, data: CommentSchema) => {
    const api = await apiClient();
    if (data.type != "Tool")
        throw new Error("Só é possivel comentar em uma postagem de ferramenta")
    const response = await api.delete(`/api/tools/comments/${id}`, {
        data: data
    });
    return response;
}
export const removeToolComment = async (data: CommentSchema) => {
    const api = await apiClient();
    if (data.type != "Tool")
        throw new Error("Só é possivel comentar em uma postagem de ferramenta")
    const response = await api.delete(`/api/admin/tools/comments`, {
        data: data
    });
    return response;
}
export const addToolReply = async (ownerId: string, data: CommentSchema) => {
    const api = await apiClient();
    if (data.type != "Tool")
        throw new Error("Só é possivel responder em uma postagem de ferramenta")
    const response = await api.post(`/api/tools/comments/replies/${ownerId}`, data);
    return response;
}
export const updateToolReply = async (ownerId: string, id:string, data: CommentSchema) => {
    const api = await apiClient();
    if (data.type != "Tool")
        throw new Error("Só é possivel responder em uma postagem de ferramenta")
    const response = await api.put(`/api/tools/comments/replies/${ownerId}/${id}`, data);
    return response;
}
export const deleteToolReply = async (ownerId: string, id:string, data: CommentSchema) => {
    const api = await apiClient();
    if (data.type != "Tool")
        throw new Error("Só é possivel responder em uma postagem de ferramenta")
    const response = await api.delete(`/api/tools/comments/replies/${ownerId}/${id}`, {
        data: data
    });
    return response;
}
export const addToolCommentLike = async(data: LikeSchema) => {
    const api = await apiClient();
    if (data.type != "Comment")
        throw new Error("Só é possivel dar curtidas nos comentários da ferramenta.");
    const response = await api.post(`/api/admin/tools/comments/likes`, data);
    return response;
}
export const removeToolCommentLike = async(data: LikeSchema) => {
    const api = await apiClient();
    if (data.type != "Comment")
        throw new Error("Só é possivel dar curtidas nos comentários da ferramenta.");
    const response = await api.delete(`/api/admin/tools/comments/likes`, {
        data: data
    });
    return response;
}
export const getToolCommentsByPagination = async (filters: CommentFilters) => {
    const api = await apiClient();
    const params = new URLSearchParams();
    params.append("page", filters.page.toString());
    params.append("targetId", filters.targetId.toString());
    if (filters.type != "Tool")
        throw new Error("Só é possivel buscar comentários de uma postagem de ferramenta");
    params.append("type", filters.type.toString());
    const response = await api.get(`/api/admin/tools/comments/pagination?${params}`);
    return response;
}