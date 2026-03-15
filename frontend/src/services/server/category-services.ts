import { CategorySchema } from "@/domain/schemas/CategorySchema";
import { apiServer } from "./api-server";
import { CategoriesFilters } from "@/domain/schemas/CategoriesFilters";

export const addCategoryService = async(data: CategorySchema) =>{
    const api = await apiServer();
    const response = await api.post("/api/Category",data);
    return response;
}
export const updateCategoryService = async(id:string, data: CategorySchema) =>{
    const api = await apiServer();
    const response = await api.put(`/api/Category/${id}`,data);
    return response;
}
export const deleteCategoryByRouteService = async(id:string) => {
    const api = await apiServer();
    const response = await api.delete(`/api/Category/${id}`);
    return response;
}
export const getCategoriesByPaginationService = async(filters: CategoriesFilters) =>{
    const api = await apiServer();
    const params = new URLSearchParams();
    params.append("page",filters.page.toString());
    if(filters.language){
        params.append("language",filters.language);
    }
    if(filters.search){
        params.append("search",filters.search);
    }
    const response = await api.get(`/api/Category/GetByPagination?${params}`);
    return response;
}