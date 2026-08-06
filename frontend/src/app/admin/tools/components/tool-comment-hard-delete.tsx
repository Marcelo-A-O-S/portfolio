import { Button } from "@/components/ui/button"
export default function HardDeleteConfirm({
    isDeleting,
    onCancel,
    onConfirm,
}: {
    isDeleting: boolean
    onCancel: () => void
    onConfirm: () => Promise<void>
}){
    return (
        <div className="flex items-center gap-3 mb-4 text-sm bg-red-50 dark:bg-red-950/30 px-3 py-2 rounded-md">
            <span>Excluir este comentário permanentemente?</span>
            <Button type="button" size="sm" variant="destructive" disabled={isDeleting} onClick={onConfirm}>
                {isDeleting ? "Excluindo..." : "Confirmar"}
            </Button>
            <Button type="button" size="sm" variant="outline" onClick={onCancel}>
                Cancelar
            </Button>
        </div>
    )
}