import { LinkTypeSchema } from "@/domain/schemas/LinkTypeSchema";
import { ApiResponse } from "@/domain/types/ApiResponse";
import { addLinkType } from "@/services/client/link-type-services";
import { useMutation, useQueryClient } from "@tanstack/react-query";
import { AxiosError, AxiosResponse } from "axios";
import { toast } from "sonner";

export function useCreateLinkType(){
    const queryClient = useQueryClient();
    return useMutation<AxiosResponse<ApiResponse>,AxiosError<ApiResponse>, LinkTypeSchema>({
        mutationFn: (data: LinkTypeSchema)=> addLinkType(data),
        onSuccess:(response)=>{
            queryClient.invalidateQueries({
                queryKey: ["link-type-pagination"]
            })
            toast.success(response.data.message);
        },
        onError:(error)=>{
            toast.error(error.response?.data?.message ?? "Erro ao criar tipo de link");
        }
    })
}