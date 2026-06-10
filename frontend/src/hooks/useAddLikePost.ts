import { LikePostSchema } from "@/domain/schemas/LikePostSchema";
import { ApiResponse } from "@/domain/types/ApiResponse";
import { addLikePost } from "@/services/client/post-services";
import { useMutation, useQueryClient } from "@tanstack/react-query";
import { AxiosError, AxiosResponse } from "axios";
export function useAddLikePost() {
    const queryClient = useQueryClient();
    return useMutation<AxiosResponse<ApiResponse>, AxiosError<ApiResponse>, LikePostSchema>({
        mutationFn: addLikePost,
        onSuccess: (response) =>{
            console.log("INVALIDANDO QUERY - Curtindo postagem");
            queryClient.invalidateQueries({
                queryKey: ["project-pagination"]
            })
        }
    })
}