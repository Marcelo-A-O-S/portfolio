import { LinkTypeSchema } from "@/domain/schemas/LinkTypeSchema";
import { ApiResponse } from "@/domain/types/ApiResponse";
import { DeleteProps } from "@/domain/types/DeleteProps";
import { deleteLinkType } from "@/services/client/link-type-services";
import { useMutation, useQueryClient } from "@tanstack/react-query";
import { AxiosError, AxiosResponse } from "axios";
import { toast } from "sonner";

export function useDeleteLinkType() {
    const queryClient = useQueryClient();
    return useMutation<AxiosResponse<ApiResponse>, AxiosError<ApiResponse>, DeleteProps<LinkTypeSchema>>({
        mutationFn: ({ id, data }) => deleteLinkType(id, data),
        onSuccess: (response) => {
            queryClient.invalidateQueries({
                queryKey: ["link-type-pagination"]
            });
            toast.success(response.data.message);
        },
        onError: (error) => {
            toast.error(error.response?.data?.message ?? "Erro ao deletar tipo de link");
        }
    })
}