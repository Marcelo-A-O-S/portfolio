import { ContributorSchema } from "@/domain/schemas/ContributorSchema";
import { ApiResponse } from "@/domain/types/ApiResponse";
import { DeleteProps } from "@/domain/types/DeleteProps";
import { deleteContributorPost } from "@/services/client/post-services";
import { useMutation, useQueryClient } from "@tanstack/react-query";
import { AxiosError, AxiosResponse } from "axios";
import { toast } from "sonner";

export function useDeleteContributorPost() {
    const queryClient = useQueryClient();
    return useMutation<AxiosResponse<ApiResponse>, AxiosError<ApiResponse>, DeleteProps<ContributorSchema>>({
        mutationFn: ({ id, data }) => deleteContributorPost(id, data),
        onSuccess: (response) => {
            queryClient.invalidateQueries({
                queryKey: ["contributor-pagination"]
            })
            toast.success(response.data.message);
        },
        onError: (error) => {
            toast.error(error.response?.data?.message ?? "Erro ao deletar contribuidor");
        }
    })
}