import { ApiResponse } from "@/domain/types/ApiResponse";
import { useMutation, useQueryClient } from "@tanstack/react-query";
import { AxiosError, AxiosResponse } from "axios";
import { ReplyToolSchema } from "./useAddReplyTool";
import { UpdateProps } from "@/domain/types/UpdateProps";
import { updateToolReply } from "@/services/client/tool-services";
import { toast } from "sonner";
export function useUpdateReplyTool() {
    const queryClient = useQueryClient();
    return useMutation<AxiosResponse<ApiResponse>, AxiosError<ApiResponse>, UpdateProps<ReplyToolSchema>>({
        mutationFn: ({ id, data }) => updateToolReply(data.ownerId, id, data.reply),
        onSuccess: (response) => {
            queryClient.invalidateQueries({
                queryKey: ["tool-comment-pagination"]
            })
            toast.success(response.data.message);
        },
        onError: (error) => {
            toast.error(error.response?.data?.message ?? "Erro ao atualizar resposta");
        }
    })
}