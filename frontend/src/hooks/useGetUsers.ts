import { UserSchema } from "@/domain/schemas/UserSchema";
import { getUsersService } from "@/services/client/user-services";
import { useQuery } from "@tanstack/react-query";

export function useGetUsers(search?: string){
    return useQuery<UserSchema[]>({
        queryKey: ["users", search],
        queryFn: async() =>{
            const response = await getUsersService(search);
            if (response.status != 200) {
                throw new Error(response.data.message);
            }
            return response.data;
        }
    })
}