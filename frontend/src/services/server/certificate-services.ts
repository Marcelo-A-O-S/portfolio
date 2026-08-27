import { CertificateFilters } from "@/domain/schemas/CertificateFilters";
import { apiServer } from "./api-server";
import { CertificateSchema } from "@/domain/schemas/CertificateSchema";

export const getCertificatesByPagination = async(filters: CertificateFilters) =>{
    const api = await apiServer();
    const params = new URLSearchParams();
    params.append(`page`, filters.page.toString());
    if (filters.search) {
        params.append("search", filters.search)
    }
    const response = await api.get(`/api/Certificate/GetByPagination?${params}`);
    return response;
}
export const addCertificateService = async (certificate: CertificateSchema) => {
    const api = await apiServer();
    const response = await api.post("/api/Certificate", certificate);
    return response;
}
export const updateCertificateService = async (id: string, data: CertificateSchema) => {
    const api = await apiServer();
    const response = await api.put(`/api/Certificate/${id}`, data);
    return response;
}
export const deleteCertificateByRouteService = async (id: string) => {
    const api = await apiServer();
    const response = await api.delete(`/api/Certificate/${id}`);
    return response;
}
export const deleteCertificateService = async (id: string, data: CertificateSchema) => {
    const api = await apiServer();
    const response = await api.delete(`/api/Certificate/${id}`,{
        data: data
    });
    return response;
}
export const getCertificateByIdService = async(id: string) => {
    const api = await apiServer();
    const response = await api.get(`/api/Certificate/GetCertificateById/${id}`);
    return response;
}