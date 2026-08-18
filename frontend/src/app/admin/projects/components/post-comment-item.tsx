import { CommentSchema } from "@/domain/schemas/CommentSchema";
import { useCommentPermissions } from "@/hooks/Comments/useCommentPermissions";
import { useAddLikeCommentPost } from "@/hooks/Post/Comment/useAddLikeCommentPost";
import { useAddReplyPost } from "@/hooks/Post/Comment/useAddReplyPost";
import { useDeleteCommentPost } from "@/hooks/Post/Comment/useDeleteCommentPost";
import { useHardDeleteCommentPost } from "@/hooks/Post/Comment/useHardDeleteCommentPost";
import { useRemoveLikeCommentPost } from "@/hooks/Post/Comment/useRemoveLikeCommentPost";
import { useUpdateCommentPost } from "@/hooks/Post/Comment/useUpdateCommentPost";
import { useSession } from "next-auth/react";
import { useState } from "react";
import PostCommentHeader from "./post-comment-header";
import { ChevronDown, ChevronUp, Heart, MessageSquare, Pencil, RotateCcw, Trash2 } from "lucide-react";
import PostEditCommentForm from "./post-comment-edit";
import PostDeleteConfirm from "./post-comment-delete";
import PostHardDeleteConfirm from "./post-comment-hard-delete";
import PostReplyForm from "./post-reply-form";
import PostReplyItem from "./post-reply-item";

export default function PostCommentItem({ comment, postId }: { comment: CommentSchema; postId: string }) {
    const { data: currentUser } = useSession();
    const permissions = useCommentPermissions(comment);
    const { mutateAsync: updateCommentPost, isPending: isUpdating } = useUpdateCommentPost();
    const { mutateAsync: deleteCommentPost, isPending: isDeleting } = useDeleteCommentPost();
    const { mutateAsync: hardDeleteCommentPost, isPending: isHardDeleting } = useHardDeleteCommentPost();
    const { mutateAsync: addReplyPost, isPending: isReplying } = useAddReplyPost();
    const { mutateAsync: addLike, isPending: isAdding } = useAddLikeCommentPost();
    const { mutateAsync: removeLike, isPending: isRemoving } = useRemoveLikeCommentPost();

    const [isEditing, setIsEditing] = useState(false)
    const [isDeletingConfirm, setIsDeletingConfirm] = useState(false)
    const [isHardDeletingConfirm, setIsHardDeletingConfirm] = useState(false)
    const [isReplyFormOpen, setIsReplyFormOpen] = useState(false)
    const [showReplies, setShowReplies] = useState(true)

    const loading = isAdding || isRemoving;
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
                <PostCommentHeader comment={comment} />
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
                    comment={comment}
                    postId={postId}
                    isSaving={isUpdating}
                    onCancel={() => setIsEditing(false)}
                    onSave={async (content) => {
                        await updateCommentPost({
                            id: comment.id!,
                            data: {
                                content,
                                targetId: postId,
                                type: "Post"
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
                }>{comment.content}</p>
            )}
            {isDeletingConfirm && (
                <PostDeleteConfirm
                    isDeleting={isDeleting}
                    onCancel={() => setIsDeletingConfirm(false)}
                    onConfirm={async () => {
                        await deleteCommentPost({ id: comment.id!, data: comment })
                        setIsDeletingConfirm(false)
                    }}
                />
            )}
            {isHardDeletingConfirm && (
                <PostHardDeleteConfirm
                    isDeleting={isHardDeleting}
                    onCancel={() => setIsHardDeletingConfirm(false)}
                    onConfirm={async () => {
                        await hardDeleteCommentPost({
                            id: comment.id!,
                            data: comment
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
                                size={16} className={comment.liked ? "fill-current" : ""}
                            />
                            <span>{comment.likes}</span>
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
                    parentCommentId={comment.id!}
                    isSaving={isReplying}
                    onCancel={() => setIsReplyFormOpen(false)}
                    onSave={async (content) => {
                        await addReplyPost({
                            ownerId: comment.id!,
                            reply: {
                                content,
                                targetId: postId,
                                type: "Post",
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
                        <PostReplyItem key={reply.id} reply={reply} postId={postId} commentId={comment.id!} />
                    ))}
                </ul>
            )}
        </li>
    )
}