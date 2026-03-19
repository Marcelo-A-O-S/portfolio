import { UsersFilters } from "@/domain/interfaces/UsersFilters";
import { getUsersByPaginationService } from "@/services/client/user-services";
import { useQuery } from "@tanstack/react-query";

export function useGetUsers(filters: UsersFilters) {
    return useQuery({
        queryKey: ["users-pagination", filters],
        queryFn: async () => {
            const response = await getUsersByPaginationService(filters);
            if (response.status != 200) {
                throw new Error(response.data.message)
            }
            return response.data
        }
    })
}