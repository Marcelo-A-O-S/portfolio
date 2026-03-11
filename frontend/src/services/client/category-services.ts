import { CategorySchema } from "@/domain/schemas/CategorySchema";
import { apiClient } from "./api-client";
import { CategoriesFilters } from "@/domain/interfaces/CategoriesFilters";

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
export const getCategoriesByPaginationService = async(categoriesFilters: CategoriesFilters) =>{
    const api = await apiClient();
    const params = new URLSearchParams();
    params.append("page",categoriesFilters.page.toString());
    params.append("language",categoriesFilters.language);
    const response = await api.get(`/api/admin/categories/pagination?${params}`);
    return response;
}