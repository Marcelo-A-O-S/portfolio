import { CategorySchema } from "@/domain/schemas/CategorySchema";
import { ApiResponse } from "@/domain/types/ApiResponse";
import { updateCategoryService } from "@/services/client/category-services";
import { useMutation, useQueryClient } from "@tanstack/react-query";
import { AxiosError, AxiosResponse } from "axios";
import { toast } from "sonner";

type UpdateCategoryProps = {
    id: string,
    category: CategorySchema
}
export function useUpdateCategory(){
    const queryClient = useQueryClient();
    return useMutation<AxiosResponse<ApiResponse>, AxiosError<ApiResponse>, UpdateCategoryProps>({
        mutationFn:({id,category})=> 
            updateCategoryService(id,category),
        onSuccess: (response)=>{
            queryClient.invalidateQueries({
                queryKey: ["categories"]
            })
            toast.success(response.data.message);
        },
        onError:(error)=>{
            toast.error(error.response?.data?.message ?? "Erro ao atualizar categoria")
        }
    })
}