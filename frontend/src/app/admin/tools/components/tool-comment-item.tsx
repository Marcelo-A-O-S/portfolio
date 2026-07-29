import { CommentSchema } from "@/domain/schemas/CommentSchema";
import { useAddCommentTool } from "@/hooks/Tool/useAddCommentTool";
import { useDeleteCommentTool } from "@/hooks/Tool/useDeleteCommentTool";
import { useUpdateCommentTool } from "@/hooks/Tool/useUpdateCommentTool";
import { useSession } from "next-auth/react";
import { useState } from "react";
import ReplyItem from "./tool-reply-item";
import { ChevronDown, ChevronUp, Heart, MessageSquare, Pencil, ThumbsUp, Trash2 } from "lucide-react";
import DeleteConfirm from "./tool-comment-delete";
import EditCommentForm from "./tool-comment-edit";
import CommentHeader from "./tool-comment-header";
import ReplyForm from "./tool-reply-form";
import { useAddReplyTool } from "@/hooks/Tool/useAddReplyTool";
import { useAddLikeCommentTool } from "@/hooks/Tool/useAddLikeCommentTool";
import { useRemoveLikeCommentTool } from "@/hooks/Tool/useRemoveLikeCommentTool";

export default function CommentItem({ comment, toolId }: { comment: CommentSchema; toolId: string }) {
    const { data: currentUser } = useSession()
    const { mutateAsync: updateCommentTool, isPending: isUpdating } = useUpdateCommentTool()
    const { mutateAsync: deleteCommentTool, isPending: isDeleting } = useDeleteCommentTool()
    const { mutateAsync: addCommentTool, isPending: isCommenting } = useAddCommentTool()
    const { mutateAsync: addReplyTool, isPending: isReplying } = useAddReplyTool();
    const { mutateAsync: addLike, isPending: isAdding } = useAddLikeCommentTool();
    const { mutateAsync: removeLike, isPending: isRemoving } = useRemoveLikeCommentTool();

    const [isEditing, setIsEditing] = useState(false)
    const [isDeletingConfirm, setIsDeletingConfirm] = useState(false)
    const [isReplyFormOpen, setIsReplyFormOpen] = useState(false)
    const [showReplies, setShowReplies] = useState(true)

    const loading = isAdding || isRemoving;
    const isOwner = currentUser?.user?.id === comment.user?.id
    const replies = comment.replies ?? []
    const handleLike = async () => {
        if (comment.liked) {
            await removeLike({
                targetId: comment.id!,
                type: "Comment"
            });
        } else {
            await addLike({
                targetId: comment.id!,
                type: "Comment"
            });
        }
    }
    return (
        <li className="border px-6 py-4 rounded-lg w-full">
            <div className="flex justify-between items-center">
                <CommentHeader comment={comment} />
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
                    <button
                        disabled={loading}
                        onClick={handleLike}
                        className="flex items-center space-x-1 p-2 rounded-full cursor-pointer">
                        <Heart
                            size={16} className={comment.liked ? "fill-current" : ""}
                        />
                        <span>{comment.likes}</span>
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
                    parentCommentId={comment.id!}
                    isSaving={isReplying}
                    onCancel={() => setIsReplyFormOpen(false)}
                    onSave={async (content) => {
                        await addReplyTool({
                            ownerId: comment.id!,
                            reply: {
                                content,
                                targetId: toolId,
                                type: "Tool",
                                parentCommentId: comment.id!,
                            }
                        })
                        setIsReplyFormOpen(false)
                        setShowReplies(true)
                    }}
                />
            )}
            {showReplies && replies.length > 0 && (
                <ul className="flex flex-col gap-3 mt-3 pl-4 border-l border-muted-foreground/20 list-none">
                    {replies.map((reply) => (
                        <ReplyItem key={reply.id} reply={reply} toolId={toolId} commentId={comment.id!} />
                    ))}
                </ul>
            )}
        </li>
    )
}