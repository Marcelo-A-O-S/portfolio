import { ApiResponse } from "@/domain/types/ApiResponse";
import { DeleteProps } from "@/domain/types/DeleteProps";
import { useMutation, useQueryClient } from "@tanstack/react-query";
import { AxiosError, AxiosResponse } from "axios";
import { ReplyPostSchema } from "./useAddReplyPost";
import { hardRemovePostReply } from "@/services/client/post-services";
import { toast } from "sonner";

export function useHardDeleteReplyPost() {
    const queryClient = useQueryClient();
    return useMutation<AxiosResponse<ApiResponse>, AxiosError<ApiResponse>, DeleteProps<ReplyPostSchema>>({
        mutationFn: ({ id, data }) => hardRemovePostReply(data.ownerId, id, data.reply),
        onSuccess: (response) => {
            queryClient.invalidateQueries({
                queryKey: ["post-comment-pagination"]
            })
            toast.success(response.data.message);
        },
        onError: (error) =>{
            toast.error(error.response?.data?.message ?? "Erro ao deletar comentário");
        }
    })
}