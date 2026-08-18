import { LinkSchema } from "@/domain/schemas/LinkSchema";
import { ApiResponse } from "@/domain/types/ApiResponse";
import { UpdateProps } from "@/domain/types/UpdateProps";
import { updateLinkPost } from "@/services/client/post-services";
import { useMutation, useQueryClient } from "@tanstack/react-query";
import { AxiosError, AxiosResponse } from "axios";
import { toast } from "sonner";

export function useUpdateLinkPost(){
    const queryClient = useQueryClient();
    return useMutation<AxiosResponse<ApiResponse>, AxiosError<ApiResponse>, UpdateProps<LinkSchema>>({
        mutationFn: ({ id, data}) => updateLinkPost(id, data),
        onSuccess: (response) =>{
            queryClient.invalidateQueries({
                queryKey: ["link-pagination"]
            })
            toast.success(response.data.message);
        },
        onError: (error) =>{
            toast.error(error.response?.data?.message ?? "Erro ao atualizar link");
        }
    })
}