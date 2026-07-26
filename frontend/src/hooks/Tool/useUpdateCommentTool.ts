import { CommentSchema } from "@/domain/schemas/CommentSchema";
import { ApiResponse } from "@/domain/types/ApiResponse";
import { UpdateProps } from "@/domain/types/UpdateProps";
import { updateToolComment } from "@/services/client/tool-services";
import { useMutation, useQueryClient } from "@tanstack/react-query";
import { AxiosError, AxiosResponse } from "axios";
import { toast } from "sonner";

export function useUpdateCommentTool(){
    const queryClient = useQueryClient();
    return useMutation<AxiosResponse<ApiResponse>, AxiosError<ApiResponse>, UpdateProps<CommentSchema>>({
        mutationFn: ({id, data}) => updateToolComment(id, data),
        onSuccess: (response) =>{
            queryClient.invalidateQueries({
                queryKey: ["tool-comment-pagination"]
            })
            toast.success(response.data.message);
        },
        onError: (error) =>{
            toast.error(error.response?.data?.message ?? "Erro ao atualizar comentário");
        }
    })
}