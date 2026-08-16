import { LinkTypeSchema } from "@/domain/schemas/LinkTypeSchema";
import { getLinkTypes } from "@/services/client/link-type-services";
import { useQuery } from "@tanstack/react-query";

export function useGetLinkTypes() {
    return useQuery<LinkTypeSchema[]>({
        queryKey: ["link-types"],
        queryFn: async () => {
            const response = await getLinkTypes();
            if (response.status != 200) {
                throw new Error(response.data.message)
            }
            return response.data
        },
    })
}