import { CertificateSchema } from "@/domain/schemas/CertificateSchema";
import { ApiResponse } from "@/domain/types/ApiResponse";
import { DeleteProps } from "@/domain/types/DeleteProps";
import { deleteCertificateService } from "@/services/client/certificate-services";
import { useMutation, useQueryClient } from "@tanstack/react-query";
import { AxiosError, AxiosResponse } from "axios";
import { toast } from "sonner";

export function useDeleteCertificate() {
    const queryClient = useQueryClient()
    return useMutation<AxiosResponse<ApiResponse>, AxiosError<ApiResponse>, DeleteProps<CertificateSchema>>({
        mutationFn: ({ id, data }) => deleteCertificateService(id, data),
        onSuccess: (response) => {
            queryClient.invalidateQueries({
                queryKey: ["certificate-pagination"]
            })
            toast.success(response.data.message);
        },
        onError: (error) => {
            toast.error(error.response?.data?.message);
        }
    })
}