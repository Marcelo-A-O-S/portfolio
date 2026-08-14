import { LanguageSchema } from "@/domain/schemas/LanguageSchema";
import { ApiResponse } from "@/domain/types/ApiResponse";
import { UpdateProps } from "@/domain/types/UpdateProps";
import { updateLanguageService } from "@/services/client/language-services";
import { useMutation, useQueryClient } from "@tanstack/react-query";
import { AxiosError, AxiosResponse } from "axios";
import { toast } from "sonner";
export function useUpdateLanguage() {
    const queryClient = useQueryClient();
    return useMutation<AxiosResponse<ApiResponse>, AxiosError<ApiResponse>, UpdateProps<LanguageSchema>>({
        mutationFn: ({ id, data }) => updateLanguageService(id, data),
        onSuccess: (response) => {
            queryClient.invalidateQueries({
                queryKey: ["languages-pagination"]
            });
            toast.success(response.data.message);
        },
        onError: (error) => {
            toast.error(error.response?.data?.message);
        }
    })
}