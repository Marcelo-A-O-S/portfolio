import { ToolSchema } from "@/domain/schemas/ToolSchema";
import { apiServer } from "./api-server";
import { ToolFilters } from "@/domain/schemas/ToolFilters";

export const addToolService = async(tool: ToolSchema) =>{
    const api = await apiServer();
    const formData = new FormData();
    formData.append("imgUrl", tool.imgUrl);
    formData.append("status", tool.status);
    formData.append("categories", JSON.stringify(tool.categories));
    formData.append("toolContents", JSON.stringify(tool.toolContents));
    const response = await api.post("/api/Tool",formData,{
        headers: {
            "Content-Type": "multipart/form-data"
        }
    });
    return response;
}
export const updateToolService = async(id:string, tool: ToolSchema) =>{
    const api = await apiServer();
    const response = await api.put(`/api/Tool/${id}`,tool);
    return response;
}
export const deleteToolByRouteService = async(id:string) =>{
    const api = await apiServer();
    const response = await api.delete(`/api/Tool/${id}`);
    return response;
}
export const getToolsByPagination = async(filters: ToolFilters) =>{
    const api = await apiServer();
    const params = new URLSearchParams();
    params.append("page", filters.page.toString());
    if(filters.search){
        params.append("search", filters.search);
    }
    const response = await api.get(`/api/Tool/GetByPagination?${params}`);
    return response;
}
export const getToolByIdService = async(id:string) =>{
    const api = await apiServer();
    const response = await api.get(`/api/Tool/GetToolById/${id}`);
    return response;
}
export const getTools = async()=>{
    const api = await apiServer();
    const response = await api.get(`/api/Tool/GetTools`);
    return response;
}