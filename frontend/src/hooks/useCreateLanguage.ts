import { LanguageSchema } from "@/domain/schemas/LanguageSchema";
import { ApiResponse } from "@/domain/types/ApiResponse";
import { addLanguageService } from "@/services/client/language-services";
import { useMutation, useQueryClient } from "@tanstack/react-query";
import { AxiosError, AxiosResponse } from "axios";
import { toast } from "sonner";
type CreateLanguageProps = {
    language: LanguageSchema
}
export function useCreateLanguage(){
    const queryClient = useQueryClient();
    return useMutation<AxiosResponse<ApiResponse>,AxiosError<ApiResponse>, LanguageSchema>({
        mutationFn:(data:LanguageSchema)=>addLanguageService(data),
        onSuccess: (response)=>{
            queryClient.invalidateQueries({
                queryKey: ["languages-pagination"]
            })
            toast.success(response.data.message);
        },
        onError: (error) =>{
            toast.error(error.response?.data?.message ?? "Erro ao criar linguagem");
        }
    })
}