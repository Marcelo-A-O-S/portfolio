import { PostSchema } from "@/domain/schemas/PostSchema";
import { apiClient } from "./api-client";
import { PostsFilters } from "@/domain/schemas/PostsFilters";
import { LikePostSchema } from "@/domain/schemas/LikePostSchema";
import { LikeSchema } from "@/domain/schemas/LikeSchema";
import { LinkSchema } from "@/domain/schemas/LinkSchema";
import { CommentSchema } from "@/domain/schemas/CommentSchema";
import { CommentFilters } from "@/domain/schemas/CommentFilters";
import { ContributorSchema } from "@/domain/schemas/ContributorSchema";
import { contributorFilters } from "@/domain/schemas/ContributorFilters";
export const addPostService = async (post: PostSchema) => {
    const api = await apiClient();
    const response = await api.post("/api/admin/posts", post);
    return response;
}
export const updatePostService = async (id: string, post: PostSchema) => {
    const api = await apiClient();
    const response = await api.put(`/api/admin/posts/${id}`, post);
    return response;
}
export const deletePostByRouteService = async (id: string) => {
    const api = await apiClient();
    const response = await api.delete(`/api/admin/posts/${id}`);
    return response;
}

export const getPostsByPagination = async (filters: PostsFilters) => {
    const api = await apiClient();
    const params = new URLSearchParams();
    params.append(`page`, filters.page.toString());
    if (filters.search) {
        params.append("search", filters.search)
    }
    const response = await api.get(`/api/admin/posts/pagination?${params}`);
    return response;
}
export const getPostByIdService = async (id: string) => {
    const api = await apiClient();
    const response = await api.get(`/api/admin/posts/${id}`);
    return response;
}
export const getPostBySlugService = async (slug: string) => {
    const api = await apiClient();
    const response = await api.get(`/api/admin/posts/${slug}`);
    return response;
}
export const getPosts = async () => {
    const api = await apiClient();
    const response = await api.get(`/api/admin/posts`);
    return response;
}
export const addLikePost = async (data: LikeSchema) => {
    const api = await apiClient();
    if (data.type != "Post")
        throw new Error("Só é possivel dar curtidas em postagens de projetos.");
    const response = await api.post(`/api/admin/posts/likes`, data);
    return response;
}
export const removeLikePost = async (data: LikeSchema) => {
    const api = await apiClient();
    if (data.type != "Post")
        throw new Error("Só é possivel dar curtidas em postagens de projetos.");
    const response = await api.delete(`/api/admin/posts/likes`, {
        data: data
    })
    return response;
}
export const addPostComment = async (data: CommentSchema) => {
    const api = await apiClient();
    if (data.type != "Post")
        throw new Error("Só é possivel comentar em uma postagem de um projeto.")
    const response = await api.post(`/api/posts/comments`, data);
    return response;
}
export const updatePostComment = async (id: string, data: CommentSchema) => {
    const api = await apiClient();
    if (data.type != "Post")
        throw new Error("Só é possivel comentar em uma postagem de um projeto.")
    const response = await api.put(`/api/posts/comments/${id}`, data);
    return response;
}
export const removePostCommentById = async (id: string, data: CommentSchema) => {
    const api = await apiClient();
    if (data.type != "Post")
        throw new Error("Só é possivel comentar em uma postagem de um projeto.")
    const response = await api.delete(`/api/posts/comments/${id}`, {
        data: data
    });
    return response;
}
export const removePostComment = async (data: CommentSchema) => {
    const api = await apiClient();
    if (data.type != "Post")
        throw new Error("Só é possivel remover comentários de um projeto.")
    const response = await api.delete(`/api/admin/posts/comments`, {
        data: data
    });
    return response;
}
export const hardRemovePostComment = async(id:string, data: CommentSchema) => {
    const api = await apiClient();
    if(data.type != "Post")
        throw new Error("Só é possivel remover comentários de uma postagem de um projeto.")
    const response = await api.delete(`/api/admin/posts/comments/hard/${id}`,{
        data: data
    });
    return response;
}
export const addPostReply = async (ownerId: string, data: CommentSchema) => {
    const api = await apiClient();
    if (data.type != "Post")
        throw new Error("Só é possivel responder em uma postagem de um projeto.")
    const response = await api.post(`/api/posts/comments/replies/${ownerId}`, data);
    return response;
}
export const updatePostReply = async (ownerId: string, id:string, data: CommentSchema) => {
    const api = await apiClient();
    if (data.type != "Post")
        throw new Error("Só é possivel responder em uma postagem de um projeto.")
    const response = await api.put(`/api/posts/comments/replies/${ownerId}/${id}`, data);
    return response;
}
export const deletePostReply = async (ownerId: string, id:string, data: CommentSchema) => {
    const api = await apiClient();
    if (data.type != "Post")
        throw new Error("Só é possivel responder em uma postagem de um projeto.")
    const response = await api.delete(`/api/posts/comments/replies/${ownerId}/${id}`, {
        data: data
    });
    return response;
}
export const hardRemovePostReply = async(ownerId: string,id:string, data: CommentSchema) => {
    const api = await apiClient();
    if(data.type != "Post")
        throw new Error("Só é possivel remover comentários de uma postagem de um projeto.")
    const response = await api.delete(`/api/admin/posts/comments/hard/replies/${ownerId}/${id}`,{
        data: data
    });
    return response;
}
export const addPostCommentLike = async(data: LikeSchema) => {
    const api = await apiClient();
    if (data.type != "Comment")
        throw new Error("Só é possivel dar curtidas nos comentários de um projeto.");
    const response = await api.post(`/api/admin/posts/comments/likes`, data);
    return response;
}
export const removePostCommentLike = async(data: LikeSchema) => {
    const api = await apiClient();
    if (data.type != "Comment")
        throw new Error("Só é possivel dar curtidas nos comentários de um projeto.");
    const response = await api.delete(`/api/admin/posts/comments/likes`, {
        data: data
    });
    return response;
}
export const getPostCommentsByPagination = async (filters: CommentFilters) => {
    const api = await apiClient();
    const params = new URLSearchParams();
    params.append("page", filters.page.toString());
    params.append("targetId", filters.targetId.toString());
    if (filters.type != "Post")
        throw new Error("Só é possivel buscar comentários de uma postagem de um projeto.");
    params.append("type", filters.type.toString());
    const response = await api.get(`/api/admin/posts/comments/pagination?${params}`);
    return response;
}
export const addLinkPost = async (data: LinkSchema) => {
    const api = await apiClient();
    const response = await api.post("/api/admin/posts/links", data);
    return response;
}
export const updateLinkPost = async (id: string, data: LinkSchema) => {
    const api = await apiClient();
    const response = await api.put(`/api/admin/posts/links/${id}`, data);
    return response;
}
export const deleteLinkPost = async (id: string, data: LinkSchema) => {
    const api = await apiClient();
    const response = await api.delete(`/api/admin/posts/links/${id}`, {
        data: data
    })
    return response;
}
export const addContributorPost = async(data: ContributorSchema) =>{
    const api = await apiClient();
    const response = await api.post(`/api/admin/posts/contributors`,data);
    return response;
}
export const updateContributorPost = async(id: string, data: ContributorSchema) =>{
    const api = await apiClient();
    const response = await api.put(`/api/admin/posts/contributors/${id}`,data);
    return response;
}
export const deleteContributorPost = async(id:string, data: ContributorSchema) => {
    const api = await apiClient();
    const response = await api.delete(`/api/admin/posts/contributors/${id}`,{
        data: data
    })
    return response;
}
export const getContributorsPagination = async(filters: contributorFilters) => {
    const api = await apiClient();
    const params = new URLSearchParams();
    params.append(`page`, filters.page.toString());
    params.append(`postId`, filters.postId.toString())
    if (filters.search) {
        params.append("search", filters.search)
    }
    const response = await api.get(`/api/admin/contributors/pagination?${params}`);
    return response;
}