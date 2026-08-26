import { CertificateSchema } from "@/domain/schemas/CertificateSchema";
import { ApiResponse } from "@/domain/types/ApiResponse";
import { UpdateProps } from "@/domain/types/UpdateProps";
import { updateCertificateService } from "@/services/client/certificate-services";
import { useMutation, useQueryClient } from "@tanstack/react-query";
import { AxiosError, AxiosResponse } from "axios";
import { toast } from "sonner";

export function useUpdateCertificate() {
    const queryClient = useQueryClient();
    return useMutation<AxiosResponse<ApiResponse>, AxiosError<ApiResponse>, UpdateProps<CertificateSchema>>({
        mutationFn: ({ id, data }) => updateCertificateService(id, data),
        onSuccess: (response) => {
            queryClient.invalidateQueries({
                queryKey: ["certificate-pagination"]
            });
            toast.success(response.data.message);
        },
        onError: (error) => {
            toast.error(error.response?.data?.message);
        }
    })
}