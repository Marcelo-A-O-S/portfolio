import { PostSchema } from "@/domain/schemas/PostSchema";
import { apiClient } from "./api-client";
import { PostsFilters } from "@/domain/schemas/PostsFilters";
import { LikePostSchema } from "@/domain/schemas/LikePostSchema";
import { LikeSchema } from "@/domain/schemas/LikeSchema";
import { LinkSchema } from "@/domain/schemas/LinkSchema";
import { CommentSchema } from "@/domain/schemas/CommentSchema";
import { CommentFilters } from "@/domain/schemas/CommentFilters";
export const addPostService = async (post: PostSchema) => {
    const api = await apiClient();
    const response = await api.post("/api/admin/post", post);
    return response;
}
export const updatePostService = async (id: string, post: PostSchema) => {
    const api = await apiClient();
    const response = await api.put(`/api/admin/post/${id}`, post);
    return response;
}
export const deletePostByRouteService = async (id: string) => {
    const api = await apiClient();
    const response = await api.delete(`/api/admin/post/${id}`);
    return response;
}

export const getPostsByPagination = async (filters: PostsFilters) => {
    const api = await apiClient();
    const params = new URLSearchParams();
    params.append(`page`, filters.page.toString());
    if (filters.search) {
        params.append("search", filters.search)
    }
    const response = await api.get(`/api/admin/post/pagination?${params}`);
    return response;
}
export const getPostByIdService = async (id: string) => {
    const api = await apiClient();
    const response = await api.get(`/api/admin/post/${id}`);
    return response;
}
export const getPostBySlugService = async (slug: string) => {
    const api = await apiClient();
    const response = await api.get(`/api/admin/post/${slug}`);
    return response;
}
export const getPosts = async () => {
    const api = await apiClient();
    const response = await api.get(`/api/admin/post`);
    return response;
}
export const addLikePost = async (data: LikeSchema) => {
    const api = await apiClient();
    if (data.type != "Post")
        throw new Error("Só é possivel dar curtidas em postagens de projetos.");
    const response = await api.post(`/api/admin/post/likes`, data);
    return response;
}
export const removeLikePost = async (data: LikeSchema) => {
    const api = await apiClient();
    if (data.type != "Post")
        throw new Error("Só é possivel dar curtidas em postagens de projetos.");
    const response = await api.delete(`/api/admin/post/likes`, {
        data: data
    })
    return response;
}
export const addPostComment = async (data: CommentSchema) => {
    const api = await apiClient();
    if (data.type != "Post")
        throw new Error("Só é possivel comentar em uma postagem de um projeto.")
    const response = await api.post(`/api/post/comments`, data);
    return response;
}
export const updatePostComment = async (id: string, data: CommentSchema) => {
    const api = await apiClient();
    if (data.type != "Post")
        throw new Error("Só é possivel comentar em uma postagem de um projeto.")
    const response = await api.put(`/api/post/comments/${id}`, data);
    return response;
}
export const removePostCommentById = async (id: string, data: CommentSchema) => {
    const api = await apiClient();
    if (data.type != "Post")
        throw new Error("Só é possivel comentar em uma postagem de um projeto.")
    const response = await api.delete(`/api/post/comments/${id}`, {
        data: data
    });
    return response;
}
export const removePostComment = async (data: CommentSchema) => {
    const api = await apiClient();
    if (data.type != "Post")
        throw new Error("Só é possivel remover comentários de um projeto.")
    const response = await api.delete(`/api/admin/post/comments`, {
        data: data
    });
    return response;
}
export const hardRemovePostComment = async(id:string, data: CommentSchema) => {
    const api = await apiClient();
    if(data.type != "Post")
        throw new Error("Só é possivel remover comentários de uma postagem de um projeto.")
    const response = await api.delete(`/api/admin/post/comments/hard/${id}`,{
        data: data
    });
    return response;
}
export const addPostReply = async (ownerId: string, data: CommentSchema) => {
    const api = await apiClient();
    if (data.type != "Post")
        throw new Error("Só é possivel responder em uma postagem de um projeto.")
    const response = await api.post(`/api/post/comments/replies/${ownerId}`, data);
    return response;
}
export const updatePostReply = async (ownerId: string, id:string, data: CommentSchema) => {
    const api = await apiClient();
    if (data.type != "Post")
        throw new Error("Só é possivel responder em uma postagem de um projeto.")
    const response = await api.put(`/api/post/comments/replies/${ownerId}/${id}`, data);
    return response;
}
export const deletePostReply = async (ownerId: string, id:string, data: CommentSchema) => {
    const api = await apiClient();
    if (data.type != "Post")
        throw new Error("Só é possivel responder em uma postagem de um projeto.")
    const response = await api.delete(`/api/post/comments/replies/${ownerId}/${id}`, {
        data: data
    });
    return response;
}
export const hardRemovePostReply = async(ownerId: string,id:string, data: CommentSchema) => {
    const api = await apiClient();
    if(data.type != "Post")
        throw new Error("Só é possivel remover comentários de uma postagem de um projeto.")
    const response = await api.delete(`/api/admin/post/comments/hard/replies/${ownerId}/${id}`,{
        data: data
    });
    return response;
}
export const addPostCommentLike = async(data: LikeSchema) => {
    const api = await apiClient();
    if (data.type != "Comment")
        throw new Error("Só é possivel dar curtidas nos comentários de um projeto.");
    const response = await api.post(`/api/admin/post/comments/likes`, data);
    return response;
}
export const removePostCommentLike = async(data: LikeSchema) => {
    const api = await apiClient();
    if (data.type != "Comment")
        throw new Error("Só é possivel dar curtidas nos comentários de um projeto.");
    const response = await api.delete(`/api/admin/post/comments/likes`, {
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
    const response = await api.get(`/api/admin/post/comments/pagination?${params}`);
    return response;
}
export const addLinkPost = async (data: LinkSchema) => {
    const api = await apiClient();
    const response = await api.post("/api/admin/post/links", data);
    return response;
}
export const updateLinkPost = async (id: string, data: LinkSchema) => {
    const api = await apiClient();
    const response = await api.put(`/api/admin/post/links/${id}`, data);
    return response;
}
export const deleteLinkPost = async (id: string, data: LinkSchema) => {
    const api = await apiClient();
    const response = await api.delete(`/api/admin/post/links/${id}`, {
        data: data
    })
    return response;
}