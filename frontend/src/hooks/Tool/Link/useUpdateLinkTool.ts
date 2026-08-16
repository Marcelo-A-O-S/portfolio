import { LinkSchema } from "@/domain/schemas/LinkSchema";
import { ApiResponse } from "@/domain/types/ApiResponse";
import { UpdateProps } from "@/domain/types/UpdateProps";
import { updateLinkTool } from "@/services/client/tool-services";
import { useMutation, useQueryClient } from "@tanstack/react-query"
import { AxiosError, AxiosResponse } from "axios";
import { toast } from "sonner";

export function useUpdateLinkTool(){
    const queryClient = useQueryClient();
    return useMutation<AxiosResponse<ApiResponse>, AxiosError<ApiResponse>, UpdateProps<LinkSchema>>({
        mutationFn: ({ id, data}) => updateLinkTool(id, data),
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