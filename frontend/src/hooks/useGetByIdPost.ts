import { getPostByIdService } from "@/services/client/post-services";
import { useQuery } from "@tanstack/react-query";
import { toast } from "sonner";

export function useGetByIdPost(postId?: string){
    return useQuery({
        queryKey: ["post-object", postId],
        enabled: !!postId,
        queryFn: async() => {
            const response = await getPostByIdService(postId!);
            if(response.status != 200){
                return toast(response.data.message)
            }
            return response.data;
        }
    })
}