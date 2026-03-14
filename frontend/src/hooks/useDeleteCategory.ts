import { deleteCategoryByRouteService } from "@/services/client/category-services";
import { useMutation, useQueryClient } from "@tanstack/react-query";
import { AxiosError, AxiosResponse } from "axios";
import { toast } from "sonner";
type ApiResponse = {
  message: string
  statusCode?: number
}
export function useDeleteCategory(){
    const queryClient = useQueryClient();
    return useMutation<AxiosResponse<ApiResponse>, AxiosError<ApiResponse>, string>({
        mutationFn: deleteCategoryByRouteService,
        onSuccess: (response)=>{
            queryClient.invalidateQueries({
                queryKey: ["categories"]
            })
            toast.success(response.data.message);
        },
        onError:(error)=>{
            toast.error(error.response?.data?.message ?? "Erro ao deletar categoria")
        }
    })
}