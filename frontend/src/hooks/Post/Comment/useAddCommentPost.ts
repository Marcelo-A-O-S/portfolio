import { CommentSchema } from "@/domain/schemas/CommentSchema";
import { ApiResponse } from "@/domain/types/ApiResponse";
import { addPostComment } from "@/services/client/post-services";
import { useMutation, useQueryClient } from "@tanstack/react-query";
import { AxiosError, AxiosResponse } from "axios";
import { toast } from "sonner";

export function useAddCommentPost(){
    const queryClient = useQueryClient();
    return useMutation<AxiosResponse<ApiResponse>, AxiosError<ApiResponse>, CommentSchema>({
        mutationFn: addPostComment,
        onSuccess: (response) =>{
            queryClient.invalidateQueries({
                queryKey: ["post-comment-pagination"]
            })
            toast.success(response.data.message);
        },
        onError: (error) =>{
            toast.error(error.response?.data?.message ?? "Erro ao criar comentário");
        }
    })
}