import { UsersFilters } from "@/domain/interfaces/UsersFilters";
import { apiServer } from "./api-server";
export const getUsersByPaginationService = async(filters: UsersFilters) =>{
    const api = await apiServer();
    const params = new URLSearchParams();
    params.append("page",filters.page.toString());
    if(filters.search){
        params.append("search",filters.search.toString());
    }
    if(filters.role){
        params.append("role",filters.role.toString());
    }
    if(filters.status){
        params.append("status",filters.status.toString());
    }
    const response = await api.get(`/api/User/GetByPagination?${params}`);
    return response;
}
export const getUsersService = async(page:number) =>{
    const api = await apiServer();
    const response = await api.get(`/api/User?page=${page}`);
    return response;
}