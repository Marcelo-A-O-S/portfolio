import { PostSchema } from "@/domain/schemas/PostSchema";
import { apiClient } from "./api-client";
import { PostsFilters } from "@/domain/schemas/PostsFilters";
export const addPostService = async (post: PostSchema) =>{
    const api = await apiClient();
    const formData = new FormData();
    formData.append("imgUrl",post.imgUrl);
    if(post.imgFile)
        formData.append("imgFile",post.imgFile);
    formData.append("status", post.status);
    formData.append("categories", JSON.stringify(post.categories));
    formData.append("postContents", JSON.stringify(post.postContents));
    formData.append("tools", JSON.stringify(post.tools));
    const response = await api.post("/api/admin/post",formData, {
        headers: {
            "Content-Type":"multipart/form-data"
        }
    });
    return response;
}
export const updatePostService = async(id:string, post: PostSchema) =>{
    const api = await apiClient();
    const formData = new FormData();
    formData.append("imgUrl",post.imgUrl);
    if(post.imgFile)
        formData.append("imgFile",post.imgFile);
    formData.append("status", post.status);
    formData.append("categories", JSON.stringify(post.categories));
    formData.append("postContents", JSON.stringify(post.postContents));
    formData.append("tools", JSON.stringify(post.tools));
    const response = await api.put(`/api/admin/post/${id}`,formData,{
        headers: {
            "Content-Type":"multipart/form-data"
        }
    });
    return response;
}
export const deletePostByRouteService = async(id:string) =>{
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