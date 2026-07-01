import { Button } from "@/components/ui/button"
import { Field } from "@/components/ui/field"
import { InputGroup, InputGroupTextarea } from "@/components/ui/input-group"
import { commentSchema, CommentSchema } from "@/domain/schemas/CommentSchema"
import { zodResolver } from "@hookform/resolvers/zod"
import { useForm, Controller } from "react-hook-form"

type ToolCommentProps = {
    items: CommentSchema[]
}
export default function ToolComments({ items }: ToolCommentProps) {
    const { control, handleSubmit, formState: { errors: errorComment } } = useForm<CommentSchema>({
        resolver: zodResolver(commentSchema)
    })
    const onSubmit = async (data: CommentSchema) => {

    }
    return (
        <>
            <div>
                <form onSubmit={handleSubmit(onSubmit,
                    (errors) => {
                        console.log("Erros RHF:");
                        console.dir(errors, { depth: null });
                    })} className="flex-1 flex flex-col gap-2">
                    <Controller
                        name="content"
                        control={control}
                        render={({ field }) => (
                            <Field className="flex flex-col flex-1 min-h-0">
                                <InputGroup className="flex-1 min-h-0 items-stretch">
                                    <InputGroupTextarea
                                        {...field}
                                        placeholder="Escreva um comentário..."
                                        className="flex-1 resize-none overflow-y-auto text-sm leading-relaxed"
                                    />
                                </InputGroup>
                                {errorComment.content && <span className="text-wrap text-red-600 text-sm">{errorComment.content?.message}</span>}
                            </Field>
                        )}
                    />
                    <div className="flex justify-end">
                        <Button type="submit"
                            className="cursor-pointer">
                            Post comment
                        </Button>
                    </div>
                </form>
            </div>
        </>
    )
}