import { apiServer } from "./api-server";

export const addImageMarkDown = async(file: File)=>{
    const api = await apiServer();
    const formData = new FormData();
    formData.append("file", file);
    const response = await api.post("/api/File/Upload/Markdown",formData,{
        headers:{
            "Content-Type":"multipart/form-data"
        }
    });
    return response;
}