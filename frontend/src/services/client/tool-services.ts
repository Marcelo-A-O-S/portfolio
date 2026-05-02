import { ToolSchema } from "@/domain/schemas/ToolSchema";
import { apiClient } from "./api-client";
import { ToolFilters } from "@/domain/schemas/ToolFilters";

export const addToolService = async(tool: ToolSchema) =>{
    const api = await apiClient();
    const formData = new FormData();
    formData.append("imgUrl", tool.imgUrl);
    if(tool.imgFile)
        formData.append("imgFile", tool.imgFile);
    formData.append("status", tool.status);
    formData.append("categories", JSON.stringify(tool.categories));
    formData.append("toolContents", JSON.stringify(tool.toolContents));
    const response = await api.post("/api/admin/tools",formData,{
        headers: {
            "Content-Type":"multipart/form-data"
        }
    });
    return response;
}
export const updateToolService = async(id:string, tool: ToolSchema) =>{
    const api = await apiClient();
    const formData = new FormData();
    formData.append("imgUrl", tool.imgUrl);
    if(tool.imgFile)
        formData.append("imgFile", tool.imgFile);
    formData.append("status", tool.status);
    formData.append("categories", JSON.stringify(tool.categories));
    formData.append("toolContents", JSON.stringify(tool.toolContents));
    const response = await api.put(`/api/admin/tools/${id}`,formData,{
        headers: {
            "Content-Type":"multipart/form-data"
        }
    });
    return response;
}
export const deleteToolByRouteService = async(id:string) =>{
    const api = await apiClient();
    const response = await api.delete(`/api/admin/tools/${id}`);
    return response;
}
export const getToolsByPagination = async(filters: ToolFilters) =>{
    const api = await apiClient();
    const params = new URLSearchParams();
    params.append("page", filters.page.toString());
    if(filters.search){
        params.append("search", filters.search);
    }
    const response = await api.get(`/api/admin/tools/pagination?${params}`);
    return response;
}
export const getToolByIdService = async(id:string) =>{
    const api = await apiClient();
    const response = await api.get(`/api/admin/tools/${id}`);
    return response;
}
export const getTools = async()=>{
    const api = await apiClient();
    const response = await api.get(`/api/admin/tools`);
    return response;
}