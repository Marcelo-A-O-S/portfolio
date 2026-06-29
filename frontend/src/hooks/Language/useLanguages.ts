import { LanguageSchema } from "@/domain/schemas/LanguageSchema";
import { getLanguages } from "@/services/client/language-services";
import { useQuery } from "@tanstack/react-query";
import { toast } from "sonner";
export function useLanguages(){
    return useQuery<LanguageSchema[]>({
        queryKey: ["languages"],
        queryFn: async ()=>{
            const response = await getLanguages();
            if(response.status !== 200)
                return toast.error(response.data.message);
            return response.data;
        }
    })
}