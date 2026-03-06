import { apiClient } from "./api-client"

export const getUsersByPaginationService = async(page: number) =>{
    const api = await apiClient();
    const response = await api.get(`/api/admin/users?page=${page}`);
    return response;
}
export const getUsersService = async() =>{
    const api = await apiClient();
    const response = await api.get(`/api/admin/users`);
    return response;
}