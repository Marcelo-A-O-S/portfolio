import { apiServer } from "./api-server";

export const getUsersByPaginationService = async(page: number) =>{
    const api = await apiServer();
    const response = await api.get(`/api/User?page=${page}`);
    return response;
}
export const getUsersService = async() =>{
    const api = await apiServer();
    const response = await api.get(`/api/User`);
    return response;
}