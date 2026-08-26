import { CertificateFilters } from "@/domain/schemas/CertificateFilters";
import { apiClient } from "./api-client";
import { CertificateSchema } from "@/domain/schemas/CertificateSchema";

export const getCertificatesByPagination = async(filters: CertificateFilters) =>{
    const api = await apiClient();
    const params = new URLSearchParams();
    params.append(`page`, filters.page.toString());
    if (filters.search) {
        params.append("search", filters.search)
    }
    const response = await api.get(`/api/admin/certificates/pagination?${params}`);
    return response;
}
export const addCertificateService = async (certificate: CertificateSchema) => {
    const api = await apiClient();
    const response = await api.post("/api/admin/certificates", certificate);
    return response;
}
export const updateCertificateService = async (id: string, data: CertificateSchema) => {
    const api = await apiClient();
    const response = await api.put(`/api/admin/certificates/${id}`, data);
    return response;
}
export const deleteCertificateByRouteService = async (id: string) => {
    const api = await apiClient();
    const response = await api.delete(`/api/admin/certificates/${id}`);
    return response;
}
export const deleteCertificateService = async (id: string, data: CertificateSchema) => {
    const api = await apiClient();
    const response = await api.delete(`/api/admin/certificates/${id}`,{
        data: data
    });
    return response;
}
export const getCertificateByIdService = async(id: string) => {
    const api = await apiClient();
    const response = await api.get(`/api/admin/certificates/${id}`);
    return response;
}