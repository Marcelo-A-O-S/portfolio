import { PostSchema } from "@/domain/schemas/PostSchema";
import { apiServer } from "./api-server";

export const addPostService = async(post: PostSchema) =>{
    const api = await apiServer();
    const response = await api.post("/api/post", post);
    return response;
}