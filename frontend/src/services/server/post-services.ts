import { PostSchema } from "@/domain/schemas/PostSchema";
import { apiServer } from "./api-server";
import { PostsFilters } from "@/domain/schemas/PostsFilters";

export const addPostService = async(post: PostSchema) =>{
    const api = await apiServer();
    const response = await api.post("/api/Post", post);
    return response;
}
export const updatePostService = async(id: string, data: PostSchema)=>{
    const api = await apiServer();
    const response = await api.put(`/api/Post/${id}`,data);
    return response;
}
export const deletePostService = async(id:string) =>{
    const api = await apiServer();
    const response = await api.delete(`/api/Post/${id}`);
    return response;
}
export const getPostsByPagination = async(filters: PostsFilters)=>{
    const api = await apiServer();
    const params = new URLSearchParams();
    params.append(`page`, filters.page.toString());
    if(filters.search){
        params.append("search",filters.search)
    }
    const response = await api.get(`/api/Post/GetByPagination?${params}`);
    return response;
}
export const getPostByIdService = async(id:string) =>{
    const api = await apiServer();
    const response = await api.get(`/api/Post/GetPostById/${id}`);
    return response;
}
export const getPosts = async() =>{
    const api = await apiServer();
    const response = await api.get(`/api/Post/GetPosts`);
    return response;
}