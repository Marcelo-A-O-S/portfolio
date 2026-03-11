import { UsersFilters } from "@/domain/interfaces/UsersFilters";
import { apiClient } from "./api-client"
export const getUsersByPaginationService = async(filters: UsersFilters) =>{
    const api = await apiClient();
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
    const response = await api.get(`/api/admin/users/pagination?${params}`);
    return response;
}
export const getUsersService = async(page:number) =>{
    const api = await apiClient();
    const response = await api.get(`/api/admin/users?page=${page}`);
    return response;
}
export const modifyRoleService = async(Id:string, role: string)=>{
    const api = await apiClient();
    const response = await api.patch(`/api/admin/users/${Id}/role`,role);
    return response;
}