import { LikePostSchema } from "@/domain/schemas/LikePostSchema";
import { LikeSchema } from "@/domain/schemas/LikeSchema";
import { ApiResponse } from "@/domain/types/ApiResponse";
import { removeLikePost } from "@/services/client/post-services";
import { useMutation, useQueryClient } from "@tanstack/react-query";
import { AxiosError, AxiosResponse } from "axios";
export function useRemoveLikePost() {
    const queryClient = useQueryClient();
    return useMutation<AxiosResponse<ApiResponse>, AxiosError<ApiResponse>, LikeSchema>({
        mutationFn: removeLikePost,
        onSuccess: (response) => {
            console.log("INVALIDANDO QUERY - Removendo curtida");
            queryClient.invalidateQueries({
                queryKey: ["project-pagination"]
            })
        }
    })
}