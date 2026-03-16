import { LanguageSchema } from "@/domain/schemas/LanguageSchema";
import { ApiResponse } from "@/domain/types/ApiResponse";
import { updateLanguageService } from "@/services/client/language-services";
import { useMutation, useQueryClient } from "@tanstack/react-query";
import { AxiosError, AxiosResponse } from "axios";
import { toast } from "sonner";
type MutationProps = {
    id: string,
    language: LanguageSchema
}
export function useUpdateLanguage(){
    const queryClient = useQueryClient();
    return useMutation<AxiosResponse<ApiResponse>,AxiosError<ApiResponse>,MutationProps>({
        mutationFn: ({ id, language})=> updateLanguageService(id, language),
        onSuccess: (response)=>{
            queryClient.invalidateQueries({
                queryKey: ["languages"]
            });
            toast.success(response.data.message);
        },
        onError: (error)=>{
            toast.error(error.response?.data?.message);
        }
    })
}