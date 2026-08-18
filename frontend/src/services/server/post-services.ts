import { PostSchema } from "@/domain/schemas/PostSchema";
import { apiServer } from "./api-server";
import { PostsFilters } from "@/domain/schemas/PostsFilters";
import { LikePostSchema } from "@/domain/schemas/LikePostSchema";
import { LikeSchema } from "@/domain/schemas/LikeSchema";
import { LinkSchema } from "@/domain/schemas/LinkSchema";
import { CommentSchema } from "@/domain/schemas/CommentSchema";
import { CommentFilters } from "@/domain/schemas/CommentFilters";

export const addPostService = async (post: PostSchema) => {
    const api = await apiServer();
    const response = await api.post("/api/Post", post);
    return response;
}
export const updatePostService = async (id: string, post: PostSchema) => {
    const api = await apiServer();
    const response = await api.put(`/api/Post/${id}`, post);
    return response;
}
export const deletePostByRouteService = async (id: string) => {
    const api = await apiServer();
    const response = await api.delete(`/api/Post/${id}`);
    return response;
}
export const getPostsByPagination = async (filters: PostsFilters) => {
    const api = await apiServer();
    const params = new URLSearchParams();
    params.append(`page`, filters.page.toString());
    if (filters.search) {
        params.append("search", filters.search)
    }
    const response = await api.get(`/api/Post/GetByPagination?${params}`);
    return response;
}
export const getPostByIdService = async (id: string) => {
    const api = await apiServer();
    const response = await api.get(`/api/Post/GetPostById/${id}`);
    return response;
}
export const getPosts = async () => {
    const api = await apiServer();
    const response = await api.get(`/api/Post/GetPosts`);
    return response;
}
export const addLikePost = async (data: LikeSchema) => {
    const api = await apiServer();
    if (data.type != "Post")
        throw new Error("Só é possivel dar curtidas em postagens de projetos.");
    const response = await api.post(`/api/Like`, data);
    return response;
}
export const removeLikePost = async (data: LikeSchema) => {
    const api = await apiServer();
    if (data.type != "Post")
        throw new Error("Só é possivel dar curtidas em postagens de projetos.");
    const response = await api.delete(`/api/Like`, {
        data: data
    })
    return response;
}
export const addPostComment = async (data: CommentSchema) => {
    const api = await apiServer();
    if (data.type != "Post")
        throw new Error("Só é possivel comentar em uma postagem de um projeto.")
    const response = await api.post(`/api/Comment`, data);
    return response;
}
export const updatePostComment = async (id: string, data: CommentSchema) => {
    const api = await apiServer();
    if (data.type != "Post")
        throw new Error("Só é possivel comentar em uma postagem de um projeto.")
    console.log("Enviando para o backend: ", data);
    const response = await api.put(`/api/Comment/${id}`, data);
    return response;
}
export const removePostCommentById = async (id: string, data: CommentSchema) => {
    const api = await apiServer();
    if (data.type != "Post")
        throw new Error("Só é possivel comentar em uma postagem de um projeto.")
    const response = await api.delete(`/api/Comment/${id}`);
    return response;
}
export const removePostComment = async (data: CommentSchema) => {
    const api = await apiServer();
    if (data.type != "Post")
        throw new Error("Só é possivel deletar o comentário em uma postagem de um projeto.")
    const response = await api.delete(`/api/Comment`, {
        data: data
    });
    return response;
}
export const hardRemovePostComment = async(id:string, data: CommentSchema) =>{
    const api = await apiServer();
    if(data.type != "Post")
        throw new Error("Só é possivel deletar o comentário de uma postagem de um projeto.")
    const response = await api.delete(`/api/Comment/Hard/${id}`,{
        data: data
    });
    return response;
}
export const addPostReply = async (ownerId: string, data: CommentSchema) => {
    const api = await apiServer();
    if (data.type != "Post")
        throw new Error("Só é possivel responder em uma postagem de um projeto.")
    const response = await api.post(`/api/Comment/${ownerId}/Reply`, data);
    return response;
}
export const updatePostReply = async (ownerId: string, id:string, data: CommentSchema) => {
    const api = await apiServer();
    if (data.type != "Post")
        throw new Error("Só é possivel responder em uma postagem de um projeto.")
    const response = await api.put(`/api/Comment/${ownerId}/Reply/${id}`, data);
    return response;
}
export const deletePostReply = async (ownerId: string, id:string, data: CommentSchema) => {
    const api = await apiServer();
    if (data.type != "Post")
        throw new Error("Só é possivel responder em uma postagem de um projeto.");
    const response = await api.delete(`/api/Comment/${ownerId}/Reply/${id}`, {
        data: data
    });
    return response;
}
export const hardRemovePostReply = async (ownerId: string, id:string, data:CommentSchema) =>{
    const api = await apiServer();
    if(data.type != "Post")
        throw new Error("Só é possivel deletar uma publicação de um projeto.");
    const response = await api.delete(`/api/Comment/Hard/${ownerId}/Reply/${id}`, {
        data: data
    });
    return response;
}
export const addPostCommentLike = async(data: LikeSchema) => {
    const api = await apiServer();
    if (data.type != "Comment")
        throw new Error("Só é possivel dar curtidas nos comentários de um projeto.");
    const response = await api.post(`/api/Like`, data);
    return response;
}
export const removePostCommentLike = async(data: LikeSchema) => {
    const api = await apiServer();
    if (data.type != "Comment")
        throw new Error("Só é possivel dar curtidas nos comentários de um projeto.");
    const response = await api.delete(`/api/Like`, {
        data: data
    });
    return response;
}
export const getPostCommentsByPagination = async (filters: CommentFilters) => {
    const api = await apiServer();
    const params = new URLSearchParams();
    params.append("page", filters.page.toString());
    params.append("targetId", filters.targetId.toString());
    if (filters.type != "Post")
        throw new Error("Só é possivel buscar comentários de uma postagem de um projeto.");
    params.append("type", filters.type.toString());
    const response = await api.get(`/api/Comment/GetByPagination?${params}`);
    return response;
}
export const addLinkPost = async(data: LinkSchema ) => {
    const api = await apiServer();
    const response = await api.post("/api/Post/Link",data);
    return response;
}
export const updateLinkPost = async(id:string, data:LinkSchema) => {
    const api = await apiServer();
    const response = await api.put(`/api/Post/Link/${id}`,data);
    return response;
}
export const deleteLinkPost = async(id:string, data: LinkSchema) => {
    const api = await apiServer();
    const response = await api.delete(`/api/Post/Link/${id}`,{
        data: data
    })
    return response;
}