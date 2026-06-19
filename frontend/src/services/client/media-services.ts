import { MediaRequestSchema } from "@/domain/schemas/MediaSchema";
import { apiClient } from "./api-client"

export const addMediaService = async(data: MediaRequestSchema) =>{
    const api = await apiClient();
    const formData = new FormData();
    if(data.ownerId)
        formData.append("ownerId", data.ownerId);
    formData.append("file", data.file);
    formData.append("ownerType", data.ownerType);
    const response = await api.post("/api/admin/media", formData, {
        headers: {
            "Content-Type": "multipart/form-data"
        }
    });
    return response;
}