import { ContributorSchema } from "@/domain/schemas/ContributorSchema";
import { ApiResponse } from "@/domain/types/ApiResponse";
import { UpdateProps } from "@/domain/types/UpdateProps";
import { updateContributorPost } from "@/services/client/post-services";
import { useMutation, useQueryClient } from "@tanstack/react-query";
import { AxiosError, AxiosResponse } from "axios";
import { toast } from "sonner";

export function useUpdateContributorPost() {
    const queryClient = useQueryClient();
    return useMutation<AxiosResponse<ApiResponse>, AxiosError<ApiResponse>, UpdateProps<ContributorSchema>>({
        mutationFn: ({ id, data }) => updateContributorPost(id, data),
        onSuccess: (response) => {
            queryClient.invalidateQueries({
                queryKey: ["contributor-pagination"]
            });
            toast.success(response.data.message);
        },
        onError: (error) => {
            toast.error(error.response?.data?.message?? "Erro ao atualizar contribuidor");
        }
    })
}