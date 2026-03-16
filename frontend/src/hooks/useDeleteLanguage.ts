import { ApiResponse } from "@/domain/types/ApiResponse";
import { deleteLanguageService } from "@/services/client/language-services";
import { useMutation, useQueryClient } from "@tanstack/react-query";
import { AxiosError, AxiosResponse } from "axios";
import { toast } from "sonner";

export function useDeleteLanguage(){
    const queryClient = useQueryClient();
    return useMutation<AxiosResponse<ApiResponse>,AxiosError<ApiResponse>,string>({
        mutationFn: (id:string) => deleteLanguageService(id),
        onSuccess: (response)=>{
            queryClient.invalidateQueries({
                queryKey: ["languages-pagination"]
            })
            toast.success(response.data.message);
        },
        onError: (error)=>{
            toast.error(error.response?.data?.message);
        }
    })
}