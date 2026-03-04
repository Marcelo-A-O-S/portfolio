import { CategorySchema } from "@/domain/schemas/CategorySchema";
import { apiClient } from "./api-client";
import { id } from "zod/v4/locales";

export const addCategoryService = async(data: CategorySchema) => {
    const api = await apiClient();
    const response = await api.post("/api/categories", data);
    return response;
}
export const updateCategoryService = async(id:string, data: CategorySchema) => {
    const api = await apiClient();
    const response = await api.put(`/api/categories/${id}`,data);
    return response;
}
export const deleteCategoryByRouteService = async(id:string) => {
    const api = await apiClient();
    const response = await api.delete(`/api/categories/${id}`);
    return response;
}