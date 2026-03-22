import { getToolByIdService } from "@/services/client/tool-services";
import { useQuery } from "@tanstack/react-query";
export function useGetByIdTool(toolId?: string) {
    return useQuery({
        queryKey: ["tool-object", toolId],
        enabled: !!toolId,
        queryFn: async () => {
            const response = await getToolByIdService(toolId!);
            if (response.status != 200) {
                throw new Error(response.data.message)
            }
            return response.data
        }
    })
}