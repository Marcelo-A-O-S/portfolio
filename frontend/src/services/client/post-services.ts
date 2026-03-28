import { PostSchema } from "@/domain/schemas/PostSchema";
import { apiClient } from "./api-client";
import { PostsFilters } from "@/domain/schemas/PostsFilters";
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
export const deletePostService = async(id:string) =>{
    const api = await apiClient();
    const response = await api.delete(`/api/admin/post/${id}`);
    return response;
}
export const getPostsByPagination = async(filters: PostsFilters)=>{
    const api = await apiClient();
    const params = new URLSearchParams();
    params.append(`page`, filters.page.toString());
    if(filters.search){
        params.append("search",filters.search)
    }
    const response = await api.get(`/api/admin/post/pagination?${params}`);
    return response;
}
export const getPostByIdService = async(id:string) => {
    const api = await apiClient();
    const response = await api.get(`/api/admin/post/${id}`);
    return response;
}
export const getPostBySlugService = async(slug:string) => {
    const api = await apiClient();
    const response = await api.get(`/api/admin/post/${slug}`);
    return response;
}
export const getPosts = async() =>{
    const api = await apiClient();
    const response = await api.get(`/api/admin/post`);
    return response;
} 