"use client"
import { Button } from "@/components/ui/button"
import { Field } from "@/components/ui/field"
import { InputGroup, InputGroupTextarea } from "@/components/ui/input-group"
import { commentSchema, CommentSchema } from "@/domain/schemas/CommentSchema"
import { useAddCommentTool } from "@/hooks/Tool/useAddCommentTool"
import { useGetToolCommentPagination } from "@/hooks/Tool/useGetToolCommentPagination"
import { zodResolver } from "@hookform/resolvers/zod"
import { Check, ChevronDown, ChevronUp, Flag, MessageSquare, Pencil, Share2, ThumbsUp, Trash2, X } from "lucide-react"
import { useSession } from "next-auth/react"
import { useState } from "react"
import { useForm, Controller } from "react-hook-form"
import { formatDistanceToNow } from "date-fns"
import { ptBR } from "date-fns/locale";
import { Avatar, AvatarFallback, AvatarImage } from "@/components/ui/avatar"
import { useUpdateCommentTool } from "@/hooks/Tool/useUpdateCommentTool"
import { useDeleteCommentTool } from "@/hooks/Tool/useDeleteCommentTool"
import { useAddReplyTool } from "@/hooks/Tool/useAddReplyTool"
import { useUpdateReplyTool } from "@/hooks/Tool/useUpdateReplyTool"
import { useDeleteReplyTool } from "@/hooks/Tool/useDeleteReplyTool"
type ToolCommentProps = {
    toolId: string,
    initialItems: CommentSchema[]
}
export default function ToolComments({ toolId, initialItems }: ToolCommentProps) {
    const { mutateAsync: addCommentTool } = useAddCommentTool();
    const { mutateAsync: updateCommentTool, isPending: isUpdating } = useUpdateCommentTool()
    const { mutateAsync: deleteCommentTool, isPending: isDeleting } = useDeleteCommentTool()
    const { data: currentUser } = useSession();
    const [page, setPage] = useState(1);
    const [editingId, setEditingId] = useState<string | null>(null)
    const [deletingId, setDeletingId] = useState<string | null>(null)
    const { data: paginatedResult, isLoading } = useGetToolCommentPagination({
        targetId: toolId,
        type: "Tool",
        page: page
    })
    const items = paginatedResult?.items ?? initialItems;
    const { control, handleSubmit, formState: { errors: errorComment, isSubmitting }, reset } = useForm<CommentSchema>({
        resolver: zodResolver(commentSchema),
        defaultValues: {
            content: "",
            targetId: toolId,
            type: "Tool"
        }
    })
    const onSubmit = async (data: CommentSchema) => {
        await addCommentTool(data);
        reset({ content: "", targetId: toolId, type: "Tool", parentCommentId: null })
    }
    console.log("Comentários: ", items)
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
                            <CommentItem key={comment.id} comment={comment} toolId={toolId} />
                        ))}
                    </ul>
                )}
            </section>

        </>
    )
}
type EditCommentFormProps = {
    comment: CommentSchema
    toolId: string
    isSaving: boolean
    onCancel: () => void
    onSave: (content: string) => Promise<void>
}
function EditCommentForm({ comment, toolId, isSaving, onCancel, onSave }: EditCommentFormProps) {
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
function CommentHeader({ comment, compact = false }: { comment: CommentSchema; compact?: boolean }) {
    return (
        <div className={`flex items-center ${compact ? "mb-2" : "mb-4"}`}>
            <div className="flex items-center justify-center gap-2">
                <Avatar className={` ${compact ? "size-7" : ""}`}>
                    <AvatarImage style={{ margin: 0 }} src={comment.user?.profileUrl} alt={comment.user?.username} />
                    <AvatarFallback>LR</AvatarFallback>
                </Avatar>
                <div>
                    <div className={compact ? "text-sm font-medium" : "text-lg font-medium"}>
                        {comment.user?.username}
                    </div>
                    {comment.createdAt && (
                        <time dateTime={comment.createdAt} className="text-xs text-muted-foreground">
                            {formatDistanceToNow(new Date(comment.createdAt), {
                                addSuffix: true,
                                locale: ptBR,
                            })}
                        </time>
                    )}
                </div>
            </div>
        </div>
    )
}
function CommentItem({ comment, toolId }: { comment: CommentSchema; toolId: string }) {
    const { data: currentUser } = useSession()
    const { mutateAsync: updateCommentTool, isPending: isUpdating } = useUpdateCommentTool()
    const { mutateAsync: deleteCommentTool, isPending: isDeleting } = useDeleteCommentTool()
    const { mutateAsync: addCommentTool, isPending: isReplying } = useAddCommentTool()

    const [isEditing, setIsEditing] = useState(false)
    const [isDeletingConfirm, setIsDeletingConfirm] = useState(false)
    const [isReplyFormOpen, setIsReplyFormOpen] = useState(false)
    const [showReplies, setShowReplies] = useState(true)

    const isOwner = currentUser?.user?.id === comment.user?.id
    const replies = comment.replies ?? []
    return (
        <li className="border px-6 py-4 rounded-lg w-full">
            <CommentHeader comment={comment} />
            {isEditing ? (
                <EditCommentForm
                    comment={comment}
                    toolId={toolId}
                    isSaving={isUpdating}
                    onCancel={() => setIsEditing(false)}
                    onSave={async (content) => {
                        await updateCommentTool({
                            id: comment.id!,
                            data: {
                                content,
                                targetId: toolId,
                                type: "Tool"
                            }
                        })
                        setIsEditing(false)
                    }}
                />
            ) : (
                <p className="text-base leading-relaxed mb-4">{comment.content}</p>
            )}
            {isDeletingConfirm && (
                <DeleteConfirm
                    isDeleting={isDeleting}
                    onCancel={() => setIsDeletingConfirm(false)}
                    onConfirm={async () => {
                        await deleteCommentTool({ id: comment.id!, data: comment })
                        setIsDeletingConfirm(false)
                    }}
                />
            )}
            <div className="flex justify-between items-center text-sm mb-2">
                <div className="flex gap-4">
                    <button type="button" className="flex items-center gap-1 hover:opacity-70">
                        <ThumbsUp size={16} /> Curtir
                    </button>
                    <button
                        type="button"
                        className="flex items-center gap-1 hover:opacity-70"
                        onClick={() => setIsReplyFormOpen((v) => !v)}
                    >
                        <MessageSquare size={16} /> Responder
                    </button>
                    {replies.length > 0 && (
                        <button
                            type="button"
                            className="flex items-center gap-1 hover:opacity-70 text-muted-foreground"
                            onClick={() => setShowReplies((v) => !v)}
                        >
                            {showReplies ? <ChevronUp size={16} /> : <ChevronDown size={16} />}
                            {replies.length} {replies.length === 1 ? "resposta" : "respostas"}
                        </button>
                    )}
                </div>
                <div className="flex gap-4">
                    {isOwner && !isEditing && (
                        <>
                            <button
                                type="button"
                                className="flex items-center gap-1 hover:opacity-70"
                                onClick={() => setIsEditing(true)}
                            >
                                <Pencil size={14} /> Editar
                            </button>
                            <button
                                type="button"
                                className="flex items-center gap-1 text-red-600 hover:opacity-70"
                                onClick={() => setIsDeletingConfirm(true)}
                            >
                                <Trash2 size={14} /> Excluir
                            </button>
                        </>
                    )}
                </div>
            </div>
            {isReplyFormOpen && (
                <ReplyForm
                    toolId={toolId}
                    parentCommentId={comment.id!}
                    isSaving={isReplying}
                    onCancel={() => setIsReplyFormOpen(false)}
                    onSave={async (content) => {
                        await addCommentTool({
                            content,
                            targetId: toolId,
                            type: "Tool",
                            parentCommentId: comment.id!,
                        })
                        setIsReplyFormOpen(false)
                        setShowReplies(true)
                    }}
                />
            )}
            {showReplies && replies.length > 0 && (
                <ul className="flex flex-col gap-3 mt-4 pl-6 border-l list-none">
                    {replies.map((reply) => (
                        <ReplyItem key={reply.id} reply={reply} toolId={toolId} commentId={comment.id!} />
                    ))}
                </ul>
            )}
        </li>
    )
}
function DeleteConfirm({
    isDeleting,
    onCancel,
    onConfirm,
}: {
    isDeleting: boolean
    onCancel: () => void
    onConfirm: () => Promise<void>
}) {
    return (
        <div className="flex items-center gap-3 mb-4 text-sm bg-red-50 dark:bg-red-950/30 px-3 py-2 rounded-md">
            <span>Excluir este comentário?</span>
            <Button type="button" size="sm" variant="destructive" disabled={isDeleting} onClick={onConfirm}>
                {isDeleting ? "Excluindo..." : "Confirmar"}
            </Button>
            <Button type="button" size="sm" variant="outline" onClick={onCancel}>
                Cancelar
            </Button>
        </div>
    )
}
function ReplyForm({
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
    const { control, handleSubmit, formState: { errors } } = useForm<CommentSchema>({
        resolver: zodResolver(commentSchema),
        defaultValues: {
            content: "",
            targetId: toolId,
            type: "Tool",
            parentCommentId,
        }
    })
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
function ReplyItem({ reply, toolId, commentId }: { reply: CommentSchema; toolId: string, commentId: string }) {
    const { data: currentUser } = useSession()
    const { mutateAsync: addReplyTool, isPending: isReplying } = useAddReplyTool();
    const { mutateAsync: updateReplyTool, isPending: isUpdating } = useUpdateReplyTool();
    const { mutateAsync: deleteReplyTool, isPending: isDeleting } = useDeleteReplyTool(); 

    const [isEditing, setIsEditing] = useState(false)
    const [isDeletingConfirm, setIsDeletingConfirm] = useState(false)

    const isOwner = currentUser?.user?.id === reply.user?.id

    return (
        <li className="pt-3">
            <CommentHeader comment={reply} compact />
            {isEditing ? (
                <EditCommentForm
                    comment={reply}
                    toolId={toolId}
                    isSaving={isUpdating}
                    onCancel={() => setIsEditing(false)}
                    onSave={async (content) => {
                        await updateReplyTool({
                            id: reply.id!,
                            data: {
                                ownerId: commentId,
                                reply: {
                                    content: content,
                                    targetId: toolId,
                                    type: "Tool",
                                }
                            }
                        })
                        setIsEditing(false)
                    }}
                />
            ) : (
                <p className="text-base leading-relaxed mb-4">{reply.content}</p>
            )}
            {isDeletingConfirm && (
                <DeleteConfirm
                    isDeleting={isDeleting}
                    onCancel={() => setIsDeletingConfirm(false)}
                    onConfirm={async () => {
                        await deleteReplyTool({ id: reply.id!, data: {
                            ownerId: commentId,
                            reply: reply
                        } })
                        setIsDeletingConfirm(false)
                    }}
                />
            )}
        </li>
    )
}