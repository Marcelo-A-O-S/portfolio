import { CommentSchema } from "@/domain/schemas/CommentSchema";
import { apiServer } from "./api-server";

export const addCommentByUser = async(data: CommentSchema) =>{
    const api = await apiServer();
    const response = await api.post(`/api/user/comments`,data);
    return response;
}
export const addCommentByModerator = async(data: CommentSchema) =>{
    const api = await apiServer();
    const response = await api.post(`/api/moderator/comments`,data);
    return response;
}
export const addCommentByAdmin = async(data: CommentSchema) =>{
    const api = await apiServer();
    const response = await api.post(`/api/admin/comments`,data);
    return response;
}