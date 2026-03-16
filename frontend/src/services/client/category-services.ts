import { CategorySchema } from "@/domain/schemas/CategorySchema";
import { apiClient } from "./api-client";
import { CategoriesFilters } from "@/domain/schemas/CategoriesFilters";

export const addCategoryService = async(data: CategorySchema) => {
    const api = await apiClient();
    const response = await api.post("/api/admin/categories", data);
    return response;
}
export const updateCategoryService = async(id:string, data: CategorySchema) => {
    const api = await apiClient();
    const response = await api.put(`/api/admin/categories/${id}`,data);
    return response;
}
export const deleteCategoryByRouteService = async(id:string) => {
    const api = await apiClient();
    const response = await api.delete(`/api/admin/categories/${id}`);
    return response;
}
export const getCategoriesByPaginationService = async(categoriesFilters: CategoriesFilters) =>{
    const api = await apiClient();
    const params = new URLSearchParams();
    params.append("page",categoriesFilters.page.toString());
    if(categoriesFilters.language){
        params.append("language",categoriesFilters.language);
    }
    if(categoriesFilters.search){
        params.append("search",categoriesFilters.search);
    }
    const response = await api.get(`/api/admin/categories/pagination?${params}`);
    return response;
}
export const getCategories = async() =>{
    const api = await apiClient();
    const response = await api.get(`/api/admin/categories`);
    return response;
}