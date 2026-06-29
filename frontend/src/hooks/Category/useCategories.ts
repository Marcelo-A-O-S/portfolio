import { CategorySchema } from "@/domain/schemas/CategorySchema";
import { getCategories } from "@/services/client/category-services";
import { useQuery } from "@tanstack/react-query";
import { toast } from "sonner";

export function useCategories(){
    return useQuery<CategorySchema[]>({
        queryKey: ["categories"],
        queryFn: async()=>{
            const response = await getCategories();
            if(response.status != 200){
                return toast.error(response.data.message);
            }
            return response.data
        }
    })
}