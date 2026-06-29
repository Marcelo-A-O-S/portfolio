import { PostSchema } from "@/domain/schemas/PostSchema";
import { ApiResponse } from "@/domain/types/ApiResponse";
import { UpdateProps } from "@/domain/types/UpdateProps";
import { updatePostService } from "@/services/client/post-services";
import { useMutation, useQueryClient } from "@tanstack/react-query";
import { AxiosError, AxiosResponse } from "axios";
import { toast } from "sonner";

export function useUpdateProject(){
    const queryClient = useQueryClient();
    return useMutation<AxiosResponse<ApiResponse>,AxiosError<ApiResponse>,UpdateProps<PostSchema>>({
        mutationFn: ({id, data}) => updatePostService(id, data),
        onSuccess:(response)=>{
            queryClient.invalidateQueries({
                queryKey: ["project-pagination"]
            });
            toast.success(response.data.message);
        },
        onError:(error)=>{
            toast.error(error.response?.data?.message);
        }
    })
}