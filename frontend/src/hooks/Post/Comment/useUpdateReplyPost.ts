import { ApiResponse } from "@/domain/types/ApiResponse";
import { UpdateProps } from "@/domain/types/UpdateProps";
import { useMutation, useQueryClient } from "@tanstack/react-query";
import { AxiosError, AxiosResponse } from "axios";
import { ReplyPostSchema } from "./useAddReplyPost";
import { updatePostReply } from "@/services/client/post-services";
import { toast } from "sonner";

export function useUpdateReplyPost() {
    const queryClient = useQueryClient();
    return useMutation<AxiosResponse<ApiResponse>, AxiosError<ApiResponse>, UpdateProps<ReplyPostSchema>>({
        mutationFn: ({ id, data }) => updatePostReply(data.ownerId, id, data.reply),
        onSuccess: (response) => {
            queryClient.invalidateQueries({
                queryKey: ["post-comment-pagination"]
            })
            toast.success(response.data.message);
        },
        onError: (error) => {
            toast.error(error.response?.data?.message ?? "Erro ao atualizar resposta");
        }
    })
}