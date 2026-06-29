import { ToolSchema } from "@/domain/schemas/ToolSchema";
import { ApiResponse } from "@/domain/types/ApiResponse";
import { UpdateProps } from "@/domain/types/UpdateProps";
import { updateToolService } from "@/services/client/tool-services";
import { useMutation, useQueryClient } from "@tanstack/react-query";
import { AxiosError, AxiosResponse } from "axios";
import { toast } from "sonner";

export function useUpdateTool(){
    const queryClient = useQueryClient()
    return useMutation<AxiosResponse<ApiResponse>,AxiosError<ApiResponse>,UpdateProps<ToolSchema>>({
        mutationFn:({ id, data }) => updateToolService(id,data),
        onSuccess: (response)=>{
            queryClient.invalidateQueries({
                queryKey: ["tool-pagination"]
            });
            toast.success(response.data.message);
        },
        onError:(error)=>{
            toast.error(error.response?.data?.message);
        }
    })
}