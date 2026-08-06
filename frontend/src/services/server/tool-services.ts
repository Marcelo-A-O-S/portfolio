import { ToolSchema } from "@/domain/schemas/ToolSchema";
import { apiServer } from "./api-server";
import { ToolFilters } from "@/domain/schemas/ToolFilters";
import { LikeSchema, likeTypeSchema, } from "@/domain/schemas/LikeSchema";
import { CommentSchema } from "@/domain/schemas/CommentSchema";
import { CommentFilters } from "@/domain/schemas/CommentFilters";

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
    const response = await api.delete(`/api/Like`, {
        data: data
    });
    return response;
}
export const addToolComment = async (data: CommentSchema) => {
    const api = await apiServer();
    if (data.type != "Tool")
        throw new Error("Só é possivel comentar em uma postagem de ferramenta")
    const response = await api.post(`/api/Comment`, data);
    return response;
}
export const updateToolComment = async (id: string, data: CommentSchema) => {
    const api = await apiServer();
    if (data.type != "Tool")
        throw new Error("Só é possivel comentar em uma postagem de ferramenta")
    console.log("Enviando para o backend: ", data);
    const response = await api.put(`/api/Comment/${id}`, data);
    return response;
}
export const removeToolCommentById = async (id: string, data: CommentSchema) => {
    const api = await apiServer();
    if (data.type != "Tool")
        throw new Error("Só é possivel comentar em uma postagem de ferramenta")
    const response = await api.delete(`/api/Comment/${id}`);
    return response;
}
export const removeToolComment = async (data: CommentSchema) => {
    const api = await apiServer();
    if (data.type != "Tool")
        throw new Error("Só é possivel deletar o comentário em uma postagem de uma ferramenta.")
    const response = await api.delete(`/api/Comment`, {
        data: data
    });
    return response;
}
export const hardRemoveToolComment = async(id:string, data: CommentSchema) =>{
    const api = await apiServer();
    if(data.type != "Tool")
        throw new Error("Só é possivel deletar o comentário de uma postagem de uma ferramenta.")
    const response = await api.delete(`/api/Comment/Hard/${id}`,{
        data: data
    });
    return response;
}
export const addToolReply = async (ownerId: string, data: CommentSchema) => {
    const api = await apiServer();
    if (data.type != "Tool")
        throw new Error("Só é possivel responder em uma postagem de ferramenta")
    const response = await api.post(`/api/Comment/${ownerId}/Reply`, data);
    return response;
}
export const updateToolReply = async (ownerId: string, id:string, data: CommentSchema) => {
    const api = await apiServer();
    if (data.type != "Tool")
        throw new Error("Só é possivel responder em uma postagem de ferramenta")
    const response = await api.put(`/api/Comment/${ownerId}/Reply/${id}`, data);
    return response;
}
export const deleteToolReply = async (ownerId: string, id:string, data: CommentSchema) => {
    const api = await apiServer();
    if (data.type != "Tool")
        throw new Error("Só é possivel responder em uma postagem de ferramenta");
    const response = await api.delete(`/api/Comment/${ownerId}/Reply/${id}`, {
        data: data
    });
    return response;
}
export const hardRemoveToolReply = async (ownerId: string, id:string, data:CommentSchema) =>{
    const api = await apiServer();
    if(data.type != "Tool")
        throw new Error("Só é possivel deletar uma publicação de uma ferramenta.");
    const response = await api.delete(`/api/Comment/Hard/${ownerId}/Reply/${id}`, {
        data: data
    });
    return response;
}
export const addToolCommentLike = async(data: LikeSchema) => {
    const api = await apiServer();
    if (data.type != "Comment")
        throw new Error("Só é possivel dar curtidas nos comentários da ferramenta.");
    const response = await api.post(`/api/Like`, data);
    return response;
}
export const removeToolCommentLike = async(data: LikeSchema) => {
    const api = await apiServer();
    if (data.type != "Comment")
        throw new Error("Só é possivel dar curtidas nos comentários da ferramenta.");
    const response = await api.delete(`/api/Like`, {
        data: data
    });
    return response;
}
export const getToolCommentsByPagination = async (filters: CommentFilters) => {
    const api = await apiServer();
    const params = new URLSearchParams();
    params.append("page", filters.page.toString());
    params.append("targetId", filters.targetId.toString());
    if (filters.type != "Tool")
        throw new Error("Só é possivel buscar comentários de uma postagem de ferramenta");
    params.append("type", filters.type.toString());
    const response = await api.get(`/api/Comment/GetByPagination?${params}`);
    return response;
}