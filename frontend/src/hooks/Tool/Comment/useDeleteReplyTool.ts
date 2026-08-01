import { ApiResponse } from "@/domain/types/ApiResponse";
import { useMutation, useQueryClient } from "@tanstack/react-query";
import { AxiosError, AxiosResponse } from "axios";
import { ReplyToolSchema } from "./useAddReplyTool";
import { DeleteProps } from "@/domain/types/DeleteProps";
import { deleteToolReply } from "@/services/client/tool-services";
import { toast } from "sonner";

export function useDeleteReplyTool(){
    const queryClient = useQueryClient();
    return useMutation<AxiosResponse<ApiResponse>, AxiosError<ApiResponse>, DeleteProps<ReplyToolSchema>>({
        mutationFn: ({id, data}) => deleteToolReply(data.ownerId, id, data.reply),
        onSuccess: (response)=>{
            queryClient.invalidateQueries({
                queryKey: ["tool-comment-pagination"]
            })
            toast.success(response.data.message);
        },
        onError: (error)=>{
            toast.error(error.response?.data?.message ?? "Erro ao deletar resposta");
        }
    })
}