import { CommentSchema } from "@/domain/schemas/CommentSchema";
import { ApiResponse } from "@/domain/types/ApiResponse";
import { addToolReply } from "@/services/client/tool-services";
import { useMutation, useQueryClient } from "@tanstack/react-query";
import { AxiosError, AxiosResponse } from "axios";
import { toast } from "sonner";
export type ReplyToolSchema = {
    ownerId: string,
    reply: CommentSchema
}
export function useAddReplyTool() {
    const queryClient = useQueryClient();
    return useMutation<AxiosResponse<ApiResponse>, AxiosError<ApiResponse>, ReplyToolSchema>({
        mutationFn: ({ ownerId, reply }) => addToolReply(ownerId, reply),
        onSuccess: (response) => {
            queryClient.invalidateQueries({
                queryKey: ["tool-comment-pagination"]
            })
            toast.success(response.data.message);
        },
        onError: (error) => {
            toast.error(error.response?.data?.message ?? "Erro ao criar resposta");
        }
    })
}