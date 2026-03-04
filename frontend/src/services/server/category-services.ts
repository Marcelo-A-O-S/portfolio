import { CategorySchema } from "@/domain/schemas/CategorySchema";
import { apiServer } from "./api-server";

export const addCategoryService = async(data: CategorySchema) =>{
    const api = await apiServer();
    const response = await api.post("/api/admin/categories",data);
    return response;
}
export const updateCategoryService = async(id:string, data: CategorySchema) =>{
    const api = await apiServer();
    const response = await api.post(`/api/admin/categories/${id}`,data);
    return response;
}
export const deleteCategoryByRouteService = async(id:string) => {
    const api = await apiServer();
    const response = await api.delete(`/api/categories/${id}`);
    return response;
}