import { apiClient } from "./api-client"

export const addImageMarkDown = async(file: File)=>{
    const api = await apiClient();
    const formData = new FormData();
    formData.append("file", file);
    const response = await api.post("/api/admin/images/markdown",formData,{
        headers:{
            "Content-Type":"multipart/form-data"
        }
    });
    return response;
}