import { MediaRequestSchema } from "@/domain/schemas/MediaSchema";
import { apiServer } from "./api-server";

export const addMediaService = async(data: MediaRequestSchema) =>{
    const api = await apiServer();
    const formData = new FormData();
    if(data.ownerId)
        formData.append("ownerId", data.ownerId);
    formData.append("file", data.file);
    formData.append("ownerType", data.ownerType);
    const response = await api.post("/api/Media", formData, {
        headers: {
            "Content-Type": "multipart/form-data"
        }
    });
    return response;
}