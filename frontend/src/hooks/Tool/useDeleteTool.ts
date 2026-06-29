import { ApiResponse } from "@/domain/types/ApiResponse";
import { deleteToolByRouteService } from "@/services/client/tool-services";
import { useMutation, useQueryClient } from "@tanstack/react-query";
import { AxiosError, AxiosResponse } from "axios";
import { toast } from "sonner";

export function useDeleteTool(){
    const queryClient = useQueryClient();
    return useMutation<AxiosResponse<ApiResponse>,AxiosError<ApiResponse>,string>({
        mutationFn: (id:string) => deleteToolByRouteService(id),
        onSuccess: (response)=>{
            queryClient.invalidateQueries({
                queryKey: ["tool-pagination"]
            })
            toast.success(response.data.message);
        },
        onError: (error)=>{
            toast.error(error.response?.data?.message);
        }
    })
}