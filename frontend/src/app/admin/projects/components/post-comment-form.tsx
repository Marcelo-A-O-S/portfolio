import { Button } from "@/components/ui/button";
import { Field } from "@/components/ui/field";
import { InputGroup, InputGroupTextarea } from "@/components/ui/input-group";
import { commentSchema, CommentSchema } from "@/domain/schemas/CommentSchema"
import { useAddCommentPost } from "@/hooks/Post/Comment/useAddCommentPost";
import { zodResolver } from "@hookform/resolvers/zod"
import { useSession } from "next-auth/react";
import { Controller, useForm } from "react-hook-form"

export default function PostCommentForm({
    postId,
}: {
    postId: string
}) {
    const { data: currentUser } = useSession();
    const { mutateAsync: addCommentPost } = useAddCommentPost();
    const { control, handleSubmit, formState: { errors: errorComment, isSubmitting }, reset } = useForm<CommentSchema>({
        resolver: zodResolver(commentSchema),
        defaultValues: {
            content: "",
            targetId: postId,
            type: "Post"
        }
    })
    const onSubmit = async (data: CommentSchema) => {
        await addCommentPost(data);
        reset({ content: "", targetId: postId, type: "Post", parentCommentId: null })
    }
    return (
        <form onSubmit={handleSubmit(onSubmit,
            (errors) => {
                console.log("Erros RHF:");
                console.dir(errors, { depth: null });
            })} className="flex-1 flex flex-col gap-2 mb-6">
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
                    disabled={isSubmitting}
                    className="cursor-pointer">
                    {isSubmitting ? "Enviando..." : "Comentar"}
                </Button>
            </div>
        </form>
    )
}