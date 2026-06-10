import { LikePostSchema } from "@/domain/schemas/LikePostSchema";
import { ApiResponse } from "@/domain/types/ApiResponse";
import { removeLikePost } from "@/services/client/post-services";
import { useMutation, useQueryClient } from "@tanstack/react-query";
import { AxiosError, AxiosResponse } from "axios";
export function useRemoveLikePost() {
    const queryClient = useQueryClient();
    return useMutation<AxiosResponse<ApiResponse>, AxiosError<ApiResponse>, LikePostSchema>({
        mutationFn: removeLikePost,
        onSuccess: (response) => {
            console.log("INVALIDANDO QUERY - Removendo curtida");
            queryClient.invalidateQueries({
                queryKey: ["project-pagination"]
            })
        }
    })
}