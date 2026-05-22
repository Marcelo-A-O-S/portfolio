import { PostSchema } from "@/domain/schemas/PostSchema";
import { ApiResponse } from "@/domain/types/ApiResponse";
import { addPostService } from "@/services/client/post-services";
import { useMutation, useQueryClient } from "@tanstack/react-query";
import { AxiosError, AxiosResponse } from "axios";
import { toast } from "sonner";

export function useCreateProject(){
    const queryClient = useQueryClient();
    return useMutation<AxiosResponse<ApiResponse>,AxiosError<ApiResponse>,PostSchema>({
        mutationFn: (data: PostSchema) => addPostService(data),
        onSuccess:(response)=>{
            queryClient.invalidateQueries({
                queryKey: ["project-pagination"]
            })
            toast.success(response.data.message);
        },
        onError:(error)=>{
            console.log(error.response?.data);
            toast.error(error.response?.data?.message ?? "Erro ao criar linguagem");
        }
    })
}