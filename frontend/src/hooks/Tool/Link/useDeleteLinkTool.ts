import { LinkSchema } from "@/domain/schemas/LinkSchema";
import { ApiResponse } from "@/domain/types/ApiResponse";
import { DeleteProps } from "@/domain/types/DeleteProps";
import { deleteLinkTool } from "@/services/client/tool-services";
import { useMutation, useQueryClient } from "@tanstack/react-query";
import { AxiosError, AxiosResponse } from "axios";
import { toast } from "sonner";

export function useDeleteLinkTool(){
    const queryClient = useQueryClient();
    return useMutation<AxiosResponse<ApiResponse>, AxiosError<ApiResponse>, DeleteProps<LinkSchema>>({
        mutationFn:({id, data}) => deleteLinkTool(id,data),
        onSuccess: (response) =>{
            queryClient.invalidateQueries({
                queryKey: ["link-pagination"]
            })
            toast.success(response.data.message);
        },
        onError: (error) =>{
            toast.error(error.response?.data?.message ?? "Erro ao deletar link");
        }
    })
}