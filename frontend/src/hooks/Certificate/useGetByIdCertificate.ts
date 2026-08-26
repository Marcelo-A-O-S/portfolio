import { getCertificateByIdService } from "@/services/client/certificate-services";
import { useQuery } from "@tanstack/react-query";
import { toast } from "sonner";

export function useGetByIdCertificate(certificateId?: string){
    return useQuery({
        queryKey: ["certificate-object", certificateId],
        enabled: !!certificateId,
        queryFn: async() => {
            const response = await getCertificateByIdService(certificateId!);
            if(response.status != 200){
                return toast.error(response.data.message)
            }
            return response.data;
        }
    })
}