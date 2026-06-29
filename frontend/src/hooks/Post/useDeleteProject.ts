import { ApiResponse } from "@/domain/types/ApiResponse";
import { deletePostByRouteService } from "@/services/client/post-services";
import { useMutation, useQueryClient } from "@tanstack/react-query";
import { AxiosError, AxiosResponse } from "axios";
import { toast } from "sonner";

export function useDeleteProject(){
    const queryClient = useQueryClient();
    return useMutation<AxiosResponse<ApiResponse>, AxiosError<ApiResponse>, string>({
        mutationFn: (id:string) => deletePostByRouteService(id),
        onSuccess:(response)=>{
            queryClient.invalidateQueries({
                queryKey: ["project-pagination"]
            })
            toast.success(response.data.message);
        },
        onError:(error)=>{
            toast.error(error.response?.data?.message);
        }
    })
}