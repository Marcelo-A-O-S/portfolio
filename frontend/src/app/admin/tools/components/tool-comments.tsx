"use client"
import { Button } from "@/components/ui/button"
import { Field } from "@/components/ui/field"
import { InputGroup, InputGroupTextarea } from "@/components/ui/input-group"
import { commentSchema, CommentSchema } from "@/domain/schemas/CommentSchema"
import { useAddCommentTool } from "@/hooks/Tool/useAddCommentTool"
import { useGetToolCommentPagination } from "@/hooks/Tool/useGetToolCommentPagination"
import { zodResolver } from "@hookform/resolvers/zod"
import { Flag, MessageSquare, Share2, ThumbsUp } from "lucide-react"
import { useSession } from "next-auth/react"
import { useState } from "react"
import { useForm, Controller } from "react-hook-form"
type ToolCommentProps = {
    toolId: string,
    initialItems: CommentSchema[]
}
export default function ToolComments({ toolId, initialItems }: ToolCommentProps) {
    const { mutateAsync : addCommentTool } = useAddCommentTool();
    const { data: currentUser } = useSession();
    const [page, setPage] = useState(1);
    const { data: paginatedResult, isLoading } = useGetToolCommentPagination({
        targetId: toolId,
        type: "Tool",
        page: page
    })
    const items = paginatedResult?.items ?? initialItems;
    const { control, handleSubmit, formState: { errors: errorComment, isSubmitting } } = useForm<CommentSchema>({
        resolver: zodResolver(commentSchema),
        defaultValues: {
            content: "",
            targetId: toolId,
            type: "Tool"
        }
    })
    const onSubmit = async (data: CommentSchema) => {
        console.log("Dados do comentário: ",data);
        await addCommentTool(data);
    }
    return (
        <>
            <section aria-labelledby="comments-heading" className="mt-8">
                {currentUser ? (
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
                ) : (
                    <p className="text-sm text-muted-foreground mb-6">
                        Faça login para deixar um comentário.
                    </p>
                )}
                {isLoading && items.length === 0 ? (
                    <p className="text-sm text-muted-foreground">Carregando comentários...</p>
                ) : items.length === 0 ? (
                    <p className="text-sm text-muted-foreground">
                        Nenhum comentário ainda. Seja o primeiro a comentar!
                    </p>
                ) : (
                    <ul className="flex flex-col gap-4 list-none">
                        {items.map((comment) => (
                            <li
                                key={comment.id}
                                className=" border px-6 py-4 rounded-lg w-full"
                            >
                                <div className="flex items-center mb-4">
                                    <div>
                                        {/* <div className="text-lg font-medium">{comment.author.name}</div>
                                        <time
                                            dateTime={comment.createdAt}
                                            className="text-xs text-muted-foreground"
                                        >
                                            {formatDistanceToNow(new Date(comment.createdAt), {
                                                addSuffix: true,
                                                locale: ptBR,
                                            })}
                                        </time> */}
                                    </div>
                                </div>
                                <p className="text-base leading-relaxed mb-4">{comment.content}</p>
                                <div className="flex justify-between items-center text-sm">
                                    <div className="flex gap-4">
                                        <button
                                            type="button"
                                            className="flex items-center gap-1 hover:opacity-70"
                                        >
                                            <ThumbsUp size={16} /> Curtir
                                        </button>
                                        <button
                                            type="button"
                                            className="flex items-center gap-1 hover:opacity-70"
                                        >
                                            <MessageSquare size={16} /> Responder
                                        </button>
                                    </div>
                                    <div className="flex gap-4">
                                        <button
                                            type="button"
                                            className="flex items-center gap-1 hover:opacity-70"
                                        >
                                            <Flag size={16} /> Denunciar
                                        </button>
                                        <button
                                            type="button"
                                            className="flex items-center gap-1 hover:opacity-70"
                                        >
                                            <Share2 size={16} /> Compartilhar
                                        </button>
                                    </div>
                                </div>
                            </li>
                        ))}
                    </ul>
                )}
                {paginatedResult?.currentPage != paginatedResult?.totalPages && (
                    <div className="flex justify-center mt-4">
                        <Button
                            variant="secondary"
                            onClick={() => setPage((p) => p + 1)}
                            className="cursor-pointer"
                        >
                            Carregar mais
                        </Button>
                    </div>
                )}
            </section>
            
        </>
    )
}