import { LikeSchema } from "@/domain/schemas/LikeSchema";
import { ApiResponse } from "@/domain/types/ApiResponse";
import { removeToolLike } from "@/services/client/tool-services";
import { useMutation, useQueryClient } from "@tanstack/react-query";
import { AxiosError, AxiosResponse } from "axios";

export function useRemoveLikeTool(){
    const queryClient = useQueryClient();
    return useMutation<AxiosResponse<ApiResponse>, AxiosError<ApiResponse>, LikeSchema>({
        mutationFn: removeToolLike,
        onSuccess: (response) =>{
            console.log("INVALIDANDO QUERY - Curtindo postagem");
            queryClient.invalidateQueries({
                queryKey: ["tool-pagination"]
            })
        },
        onError: (error) =>{
            
        }
    })
}