import { LikeSchema } from "@/domain/schemas/LikeSchema";
import { ApiResponse } from "@/domain/types/ApiResponse";
import { addToolCommentLike } from "@/services/client/tool-services";
import { useMutation, useQueryClient } from "@tanstack/react-query";
import { AxiosError, AxiosResponse } from "axios";
import { toast } from "sonner";

export function useAddLikeCommentTool(){
    const queryClient = useQueryClient();
    return useMutation<AxiosResponse<ApiResponse>, AxiosError<ApiResponse>, LikeSchema>({
        mutationFn: addToolCommentLike,
        onSuccess: (response) =>{
            queryClient.invalidateQueries({
                queryKey: ["tool-comment-pagination"]
            })
            toast.success(response.data.message);
        },
        onError: (error) =>{
            toast.error(error.response?.data?.message ?? "Erro ao adicionar curtida");
        }
    })
}