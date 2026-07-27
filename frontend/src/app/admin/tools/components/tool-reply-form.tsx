import { Button } from "@/components/ui/button"
import { Field } from "@/components/ui/field"
import { InputGroup, InputGroupTextarea } from "@/components/ui/input-group"
import { commentSchema, CommentSchema } from "@/domain/schemas/CommentSchema"
import { useAddReplyTool } from "@/hooks/Tool/useAddReplyTool"
import { zodResolver } from "@hookform/resolvers/zod"
import { useSession } from "next-auth/react"
import { Controller, useForm } from "react-hook-form"

export default function ReplyForm({
    toolId,
    parentCommentId,
    isSaving,
    onCancel,
    onSave,
}: {
    toolId: string
    parentCommentId: string
    isSaving: boolean
    onCancel: () => void
    onSave: (content: string) => Promise<void>
}) {
    const { data: currentUser } = useSession();
    const { mutateAsync: addReplyTool, isPending: isReplying } = useAddReplyTool();
    const { control, handleSubmit, formState: { errors }, reset } = useForm<CommentSchema>({
        resolver: zodResolver(commentSchema),
        defaultValues: {
            content: "",
            targetId: toolId,
            type: "Tool",
            parentCommentId,
        }
    })
    const onSubmit = async (data: CommentSchema) => {
        await addReplyTool({ ownerId: toolId, reply: data });
        reset({ content: "", targetId: toolId, type: "Tool", parentCommentId: null })
    }
    return (
        <form onSubmit={handleSubmit((data) => onSave(data.content))} className="flex flex-col gap-2 mb-4 pl-2">
            <Controller
                name="content"
                control={control}
                render={({ field }) => (
                    <Field>
                        <InputGroup>
                            <InputGroupTextarea
                                {...field}
                                placeholder="Escreva uma resposta..."
                                className="resize-none text-sm leading-relaxed"
                            />
                        </InputGroup>
                        {errors.content && <span className="text-red-600 text-sm">{errors.content.message}</span>}
                    </Field>
                )}
            />
            <div className="flex justify-end gap-2">
                <Button type="button" variant="outline" size="sm" onClick={onCancel}>
                    Cancelar
                </Button>
                <Button type="submit" size="sm" disabled={isSaving}>
                    {isSaving ? "Enviando..." : "Responder"}
                </Button>
            </div>
        </form>
    )
}