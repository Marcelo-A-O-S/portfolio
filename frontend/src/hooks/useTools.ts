import { useQuery } from "@tanstack/react-query";

export function useTools(){
    return useQuery({
        queryKey:["tools"]
    })
}