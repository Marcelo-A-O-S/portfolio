import { CommentSchema } from "@/domain/schemas/CommentSchema";
import { useAddReplyTool } from "@/hooks/Tool/useAddReplyTool";
import { useDeleteReplyTool } from "@/hooks/Tool/useDeleteReplyTool";
import { useUpdateReplyTool } from "@/hooks/Tool/useUpdateReplyTool";
import { useSession } from "next-auth/react";
import { useState } from "react";
import CommentHeader from "./tool-comment-header";
import DeleteConfirm from "./tool-comment-delete";
import EditCommentForm from "./tool-comment-edit";
import { ChevronDown, ChevronUp, MessageSquare, Pencil, ThumbsUp, Trash2 } from "lucide-react";
import ReplyForm from "./tool-reply-form";

export default function ReplyItem({ reply, toolId, commentId }: { reply: CommentSchema; toolId: string, commentId: string }) {
    const { data: currentUser } = useSession()
    const { mutateAsync: addReplyTool, isPending: isReplying } = useAddReplyTool();
    const { mutateAsync: updateReplyTool, isPending: isUpdating } = useUpdateReplyTool();
    const { mutateAsync: deleteReplyTool, isPending: isDeleting } = useDeleteReplyTool();

    const [isEditing, setIsEditing] = useState(false)
    const [isDeletingConfirm, setIsDeletingConfirm] = useState(false)
    const [isReplyFormOpen, setIsReplyFormOpen] = useState(false)
    const [showReplies, setShowReplies] = useState(true)

    const isOwner = currentUser?.user?.id === reply.user?.id
    const replies = reply.replies ?? []
    return (
        <li className="pt-3">
            <div className="flex justify-between items-center">
                <CommentHeader comment={reply} compact={true} isReply={true} />
                <div className="flex gap-4 flex-col md:flex-row">
                    {isOwner && !isEditing && (
                        <>
                            <button
                                type="button"
                                className="flex items-center gap-1 hover:opacity-70 text-sm"
                                onClick={() => setIsEditing(true)}
                            >
                                <Pencil size={14} /> Editar
                            </button>
                            <button
                                type="button"
                                className="flex items-center gap-1 text-red-600 hover:opacity-70 text-sm"
                                onClick={() => setIsDeletingConfirm(true)}
                            >
                                <Trash2 size={14} /> Excluir
                            </button>
                        </>
                    )}
                </div>
            </div>

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
                        await deleteReplyTool({
                            id: reply.id!, data: {
                                ownerId: commentId,
                                reply: reply
                            }
                        })
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

            </div>
            {isReplyFormOpen && (
                <ReplyForm
                    toolId={toolId}
                    parentCommentId={reply.id!}
                    isSaving={isReplying}
                    onCancel={() => setIsReplyFormOpen(false)}
                    onSave={async (content) => {
                        await addReplyTool({
                            ownerId: reply.id!,
                            reply: {
                                content,
                                targetId: toolId,
                                type: "Tool",
                                parentCommentId: reply.id!,
                            }
                        })
                        setIsReplyFormOpen(false)
                        setShowReplies(true)
                    }}
                />
            )}
            {showReplies && replies.length > 0 && (
                <ul className="flex flex-col gap-3 mt-4 pl-6 border-l list-none">
                    {replies.map((replyitem) => (
                        <ReplyItem key={replyitem.id} reply={replyitem} toolId={toolId} commentId={reply.id!} />
                    ))}
                </ul>
            )}
        </li>
    )
}