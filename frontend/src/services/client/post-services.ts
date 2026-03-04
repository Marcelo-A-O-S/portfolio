import { PostSchema } from "@/domain/schemas/PostSchema";
import { apiClient } from "./api-client";
export const addPostService = async (post: PostSchema) =>{
    const api = await apiClient();
    const response = await api.post("/api/admin/post",post);
    return response;
}
export const updatePostService = async(id:string, post: PostSchema) =>{
    const api = await apiClient();
    const response = await api.put(`/api/admin/post/${id}`,post);
    return response;
}
export const getPostByIdService = async(id:string) => {
    const api = await apiClient();
    const response = await api.get(`/api/post/${id}`);
    return response;
}
export const getPostBySlugService = async(slug:string) => {
    const api = await apiClient();
    const response = await api.get(`/api/post/${slug}`);
    return response;
} 