import { getUsersByPaginationService } from "@/services/client/user-services";
import { useQuery } from "@tanstack/react-query";

export function useUsers(page: number, search?: string, role?: string, status?: string) {
    return useQuery({
        queryKey: ["users", page, search, role, status],
        queryFn: async () => {
            const response = await getUsersByPaginationService(page, search, role, status);
            if (response.status != 200) {
                throw new Error(response.data.message)
            }
            return response.data
        }
    })
}