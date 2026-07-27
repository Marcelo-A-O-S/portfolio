import { Button } from "@/components/ui/button"
import { Field } from "@/components/ui/field"
import { InputGroup, InputGroupTextarea } from "@/components/ui/input-group"
import { commentSchema, CommentSchema } from "@/domain/schemas/CommentSchema"
import { zodResolver } from "@hookform/resolvers/zod"
import { Check, X } from "lucide-react"
import { Controller, useForm } from "react-hook-form"

type EditCommentFormProps = {
    comment: CommentSchema
    toolId: string
    isSaving: boolean
    onCancel: () => void
    onSave: (content: string) => Promise<void>
}
export default function EditCommentForm({ comment, toolId, isSaving, onCancel, onSave }: EditCommentFormProps) {
    const { control, handleSubmit, formState: { errors } } = useForm<CommentSchema>({
        resolver: zodResolver(commentSchema),
        defaultValues: {
            content: comment.content,
            targetId: toolId,
            type: "Tool",
        }
    })
    return (
        <form
            onSubmit={handleSubmit((data) => onSave(data.content))}
            className="flex flex-col gap-2 mb-4"
        >
            <Controller
                name="content"
                control={control}
                render={({ field }) => (
                    <Field>
                        <InputGroup>
                            <InputGroupTextarea
                                {...field}
                                className="resize-none text-sm leading-relaxed"
                            />
                        </InputGroup>
                        {errors.content && <span className="text-red-600 text-sm">{errors.content.message}</span>}
                    </Field>
                )}
            />
            <div className="flex justify-end gap-2">
                <Button type="button" variant="outline" size="sm" onClick={onCancel}>
                    <X size={14} className="mr-1" /> Cancelar
                </Button>
                <Button type="submit" size="sm" disabled={isSaving}>
                    <Check size={14} className="mr-1" /> {isSaving ? "Salvando..." : "Salvar"}
                </Button>
            </div>
        </form>
    )
}