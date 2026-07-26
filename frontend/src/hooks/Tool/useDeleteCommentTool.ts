import { CommentSchema } from "@/domain/schemas/CommentSchema";
import { ApiResponse } from "@/domain/types/ApiResponse";
import { DeleteProps } from "@/domain/types/DeleteProps";
import { removeToolCommentById } from "@/services/client/tool-services";
import { useMutation, useQueryClient } from "@tanstack/react-query";
import { AxiosError, AxiosResponse } from "axios";
import { toast } from "sonner";

export function useDeleteCommentTool(){
    const queryClient = useQueryClient();
    return useMutation<AxiosResponse<ApiResponse>,AxiosError<ApiResponse>,DeleteProps<CommentSchema>>({
        mutationFn: ({id, data}) => removeToolCommentById(id,data),
        onSuccess: (response)=>{
            queryClient.invalidateQueries({
                queryKey: ["tool-comment-pagination"]
            })
            toast.success(response.data.message);
        },
        onError: (error)=>{
            toast.error(error.response?.data?.message ?? "Erro ao deletar comentário");
        }
    })
}