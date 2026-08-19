import { ContributorSchema } from "@/domain/schemas/ContributorSchema";
import { ApiResponse } from "@/domain/types/ApiResponse";
import { addContributorPost } from "@/services/client/post-services";
import { useMutation, useQueryClient } from "@tanstack/react-query";
import { AxiosError, AxiosResponse } from "axios";

export function useAddContributorPost(){
    const queryClient = useQueryClient();
    return useMutation<AxiosResponse<ApiResponse>, AxiosError<ApiResponse>, ContributorSchema>({
        mutationFn: addContributorPost,
        onSuccess: (response) =>{
            queryClient.invalidateQueries({
                queryKey: ["contributor-pagination"]
            })
        }
    })
}