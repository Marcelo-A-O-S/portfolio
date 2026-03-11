import { useMutation, useQueryClient } from "@tanstack/react-query"
import { modifyRoleService } from "@/services/client/user-services"

export function useModifyRole() {
  const queryClient = useQueryClient()
  return useMutation({
    mutationFn: ({ userId, role }: { userId: string, role: string }) =>
      modifyRoleService(userId, role),

    onSuccess: () => {

      queryClient.invalidateQueries({
        queryKey: ["users"]
      })

    }
  })
}