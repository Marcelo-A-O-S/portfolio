import { CommentSchema } from "@/domain/schemas/CommentSchema";
import { useCommentPermissions } from "@/hooks/Comments/useCommentPermissions";
import { useAddLikeCommentPost } from "@/hooks/Post/Comment/useAddLikeCommentPost";
import { useAddReplyPost } from "@/hooks/Post/Comment/useAddReplyPost";
import { useDeleteReplyPost } from "@/hooks/Post/Comment/useDeleteReplyPost";
import { useHardDeleteReplyPost } from "@/hooks/Post/Comment/useHardDeleteReplyPost";
import { useRemoveLikeCommentPost } from "@/hooks/Post/Comment/useRemoveLikeCommentPost";
import { useUpdateReplyPost } from "@/hooks/Post/Comment/useUpdateReplyPost";
import { useSession } from "next-auth/react";
import { useState } from "react";
import PostCommentHeader from "./post-comment-header";
import { ChevronDown, ChevronUp, Heart, MessageSquare, Pencil, RotateCcw, Trash2 } from "lucide-react";
import PostEditCommentForm from "./post-comment-edit";
import PostDeleteConfirm from "./post-comment-delete";
import PostHardDeleteConfirm from "./post-comment-hard-delete";
import PostReplyForm from "./post-reply-form";

export default function PostReplyItem({ reply, postId, commentId }: { reply: CommentSchema; postId: string, commentId: string }) {
    const { data: currentUser } = useSession();
    const permissions = useCommentPermissions(reply);
    const { mutateAsync: addReplyPost, isPending: isReplying } = useAddReplyPost();
    const { mutateAsync: updateReplyPost, isPending: isUpdating } = useUpdateReplyPost();
    const { mutateAsync: deleteReplyPost, isPending: isDeleting } = useDeleteReplyPost();
    const { mutateAsync: hardDeleteReplyPost, isPending: isHardDeleting } = useHardDeleteReplyPost();
    const { mutateAsync: addLike, isPending: isAdding } = useAddLikeCommentPost();
    const { mutateAsync: removeLike, isPending: isRemoving } = useRemoveLikeCommentPost();

    const [isEditing, setIsEditing] = useState(false)
    const [isDeletingConfirm, setIsDeletingConfirm] = useState(false)
    const [isHardDeletingConfirm, setIsHardDeletingConfirm] = useState(false)
    const [isReplyFormOpen, setIsReplyFormOpen] = useState(false)
    const [showReplies, setShowReplies] = useState(true)

    const loading = isAdding || isRemoving;
    const replies = reply.replies ?? []
    const handleLike = async () => {
        if (reply.liked) {
            await removeLike({
                targetId: reply.id!,
                type: "Comment"
            });
        } else {
            await addLike({
                targetId: reply.id!,
                type: "Comment"
            });
        }
    }
    return (
        <>
            <li className="pt-3">
                <div className="flex justify-between items-center">
                    <PostCommentHeader comment={reply} compact={true} isReply={true} />
                    <div className="flex gap-4 flex-col md:flex-row">
                        {permissions.canEdit && (
                            <button
                                type="button"
                                className="flex items-center gap-1 hover:opacity-70 text-sm"
                                onClick={() => setIsEditing(true)}
                            >
                                <Pencil size={14} /> Editar
                            </button>
                        )}
                        {permissions.canDelete && (
                            <button
                                type="button"
                                className="flex items-center gap-1 text-red-600 hover:opacity-70 text-sm"
                                onClick={() => setIsDeletingConfirm(true)}
                            >
                                <Trash2 size={14} /> Excluir
                            </button>
                        )}
                        {permissions.canRestore && (
                            <button
                                type="button"
                                className="flex items-center gap-1 hover:opacity-70 text-sm"
                            >
                                <RotateCcw />
                                Restaurar
                            </button>
                        )}
                        {permissions.canHardDelete && (
                            <button
                                type="button"
                                className="flex items-center gap-1 text-red-600 hover:opacity-70 text-sm"
                                onClick={() => setIsHardDeletingConfirm(true)}
                            >
                                <Trash2 size={14} /> Excluir
                            </button>
                        )}
                    </div>
                </div>
                {isEditing ? (
                    <PostEditCommentForm
                        comment={reply}
                        postId={postId}
                        isSaving={isUpdating}
                        onCancel={() => setIsEditing(false)}
                        onSave={async (content) => {
                            await updateReplyPost({
                                id: reply.id!,
                                data: {
                                    ownerId: commentId,
                                    reply: {
                                        id: reply.id!,
                                        content: content,
                                        targetId: postId,
                                        type: "Post",
                                        parentCommentId: commentId,
                                    }
                                }
                            })
                            setIsEditing(false)
                        }}
                    />
                ) : (
                    <p className={
                        permissions.isDeleted
                            ? "italic text-muted-foreground"
                            : "text-base leading-relaxed"
                    }>{reply.content}</p>
                )}
                {isDeletingConfirm && (
                    <PostDeleteConfirm
                        isDeleting={isDeleting}
                        onCancel={() => setIsDeletingConfirm(false)}
                        onConfirm={async () => {
                            await deleteReplyPost({
                                id: reply.id!,
                                data: {
                                    ownerId: commentId,
                                    reply: reply
                                }
                            })
                            setIsDeletingConfirm(false)
                        }}
                    />
                )}
                {isHardDeletingConfirm && (
                    <PostHardDeleteConfirm
                        isDeleting={isHardDeleting}
                        onCancel={() => setIsHardDeletingConfirm(false)}
                        onConfirm={async () => {
                            await hardDeleteReplyPost({
                                id: reply.id!,
                                data: {
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
                        {permissions.canLike && (
                            <button
                                disabled={loading}
                                onClick={handleLike}
                                className="flex items-center space-x-1 p-2 rounded-full cursor-pointer">
                                <Heart
                                    size={16} className={reply.liked ? "fill-current" : ""}
                                />
                                <span>{reply.likes}</span>
                            </button>
                        )}
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
                    <PostReplyForm
                        postId={postId}
                        parentCommentId={reply.id!}
                        isSaving={isReplying}
                        onCancel={() => setIsReplyFormOpen(false)}
                        onSave={async (content) => {
                            await addReplyPost({
                                ownerId: reply.id!,
                                reply: {
                                    content,
                                    targetId: postId,
                                    type: "Post",
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
                            <PostReplyItem key={replyitem.id} reply={replyitem} postId={postId} commentId={reply.id!} />
                        ))}
                    </ul>
                )}
            </li>
        </>
    )
}