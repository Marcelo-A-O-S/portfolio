import { LikeSchema } from "@/domain/schemas/LikeSchema";
import { ApiResponse } from "@/domain/types/ApiResponse";
import { removePostCommentLike } from "@/services/client/post-services";
import { useMutation, useQueryClient } from "@tanstack/react-query";
import { AxiosError, AxiosResponse } from "axios";
import { toast } from "sonner";

export function useRemoveLikeCommentPost() {
    const queryClient = useQueryClient();
    return useMutation<AxiosResponse<ApiResponse>, AxiosError<ApiResponse>, LikeSchema>({
        mutationFn: removePostCommentLike,
        onSuccess: (response) => {
            queryClient.invalidateQueries({
                queryKey: ["post-comment-pagination"]
            })
            toast.success(response.data.message);
        },
        onError: (error) => {
            toast.error(error.response?.data?.message ?? "Erro ao adicionar curtida");
        }
    })
}