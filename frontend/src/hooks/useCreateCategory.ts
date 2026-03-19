import { CategorySchema } from "@/domain/schemas/CategorySchema";
import { ApiResponse } from "@/domain/types/ApiResponse";
import { addCategoryService } from "@/services/client/category-services";
import { useMutation, useQueryClient } from "@tanstack/react-query";
import { AxiosError, AxiosResponse } from "axios";
import { toast } from "sonner";

export function useCreateCategory() {
    const queryClient = useQueryClient();
    return useMutation<AxiosResponse<ApiResponse>, AxiosError<ApiResponse>, CategorySchema>({
        mutationFn: addCategoryService,
        onSuccess: (response) => {
            queryClient.invalidateQueries({
                queryKey: ["categories-pagination"]
            })
            toast.success(response.data.message)
        },
        onError: (error) =>{
            toast.error(error.response?.data?.message ?? "Erro ao criar categoria")
        }
    })
}