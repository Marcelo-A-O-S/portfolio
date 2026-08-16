import { LinkSchema } from "@/domain/schemas/LinkSchema";
import { ApiResponse } from "@/domain/types/ApiResponse";
import { addLink } from "@/services/client/link-services";
import { useMutation, useQueryClient } from "@tanstack/react-query";
import { AxiosError, AxiosResponse } from "axios";
import { toast } from "sonner";

export function useCreateLink(){
    const queryClient = useQueryClient();
    return useMutation<AxiosResponse<ApiResponse>, AxiosError<ApiResponse>, LinkSchema>({
        mutationFn: addLink,
        onSuccess: (response) =>{
            queryClient.invalidateQueries({
                queryKey: ["link-pagination"]
            })
            toast.success(response.data.message);
        },
        onError: (error) =>{
            toast.error(error.response?.data?.message ?? "Erro ao adicionar link");
        }
    })
}