import { ToolSchema } from "@/domain/schemas/ToolSchema";
import { ApiResponse } from "@/domain/types/ApiResponse";
import { addToolService } from "@/services/client/tool-services";
import { useMutation, useQueryClient } from "@tanstack/react-query";
import { AxiosError, AxiosResponse } from "axios";
import { toast } from "sonner";
export function useCreateTool() {
    const queryClient = useQueryClient();
    return useMutation<AxiosResponse<ApiResponse>, AxiosError<ApiResponse>, ToolSchema>({
        mutationFn: (data: ToolSchema) => addToolService(data),
        onSuccess: (response) => {
            queryClient.invalidateQueries({
                queryKey: ["tool-pagination"]
            })
            toast.success(response.data.message);
        },
        onError:(error)=>{
            console.log(error.response?.data);
            toast.error(error.response?.data?.message ?? "Erro ao criar linguagem");
        }
    })
}