import { CertificateSchema } from "@/domain/schemas/CertificateSchema";
import { ApiResponse } from "@/domain/types/ApiResponse";
import { addCertificateService } from "@/services/client/certificate-services";
import { useMutation, useQueryClient } from "@tanstack/react-query";
import { AxiosError, AxiosResponse } from "axios";
import { toast } from "sonner";

export function useCreateCertificate() {
    const queryClient = useQueryClient();
    return useMutation<AxiosResponse<ApiResponse>, AxiosError<ApiResponse>, CertificateSchema>({
        mutationFn: (data: CertificateSchema) => addCertificateService(data),
        onSuccess: (response) => {
            queryClient.invalidateQueries({
                queryKey: ["certificate-pagination"]
            })
            toast.success(response.data.message);
        },
        onError: (error) => {
            console.log(error.response?.data);
            toast.error(error.response?.data?.message ?? "Erro ao criar certificado");
        }
    })
}