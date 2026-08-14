import { LinkTypeSchema } from "@/domain/schemas/LinkTypeSchema";
import { ApiResponse } from "@/domain/types/ApiResponse";
import { UpdateProps } from "@/domain/types/UpdateProps";
import { updateLinkType } from "@/services/client/link-type-services";
import { useMutation, useQueryClient } from "@tanstack/react-query";
import { AxiosError, AxiosResponse } from "axios";
import { toast } from "sonner";

export function useUpdateLinkType(){
    const queryClient = useQueryClient();
    return useMutation<AxiosResponse<ApiResponse>,AxiosError<ApiResponse>,UpdateProps<LinkTypeSchema>>({
        mutationFn:({id, data}) => updateLinkType(id, data),
        onSuccess:(response)=>{
            queryClient.invalidateQueries({
                queryKey: ["link-type-pagination"]
            });
            toast.success(response.data.message);
        },
        onError:(error)=>{
            toast.error(error.response?.data?.message ?? "Erro ao criar tipo de link");
        }
    })
}