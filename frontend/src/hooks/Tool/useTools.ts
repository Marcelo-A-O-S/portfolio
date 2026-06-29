import { ToolSchema } from "@/domain/schemas/ToolSchema";
import { getTools } from "@/services/client/tool-services";
import { useQuery } from "@tanstack/react-query";
import { toast } from "sonner";

export function useTools(){
    return useQuery<ToolSchema[]>({
        queryKey:["tools"],
        queryFn: async()=>{
            const response = await getTools();
            if(response.status !== 200){
                return toast.error(response.data.message);
            }
            return response.data;
        }
    })
}