import { CommentSchema } from "@/domain/schemas/CommentSchema";
import { apiClient } from "./api-client"


export const addCommentByUser = async(data: CommentSchema) =>{
    const api = await apiClient();
    const response = await api.post(`/api/user/comments`,data);
    return response;
}
export const addCommentByModerator = async(data: CommentSchema) =>{
    const api = await apiClient();
    const response = await api.post(`/api/moderator/comments`,data);
    return response;
}
export const addCommentByAdmin = async(data: CommentSchema) =>{
    const api = await apiClient();
    const response = await api.post(`/api/admin/comments`,data);
    return response;
}