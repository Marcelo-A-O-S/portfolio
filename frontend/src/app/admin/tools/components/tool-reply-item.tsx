import { CommentSchema } from "@/domain/schemas/CommentSchema";
import { useAddReplyTool } from "@/hooks/Tool/useAddReplyTool";
import { useDeleteReplyTool } from "@/hooks/Tool/useDeleteReplyTool";
import { useUpdateReplyTool } from "@/hooks/Tool/useUpdateReplyTool";
import { useSession } from "next-auth/react";
import { useState } from "react";
import CommentHeader from "./tool-comment-header";
import DeleteConfirm from "./tool-comment-delete";
import EditCommentForm from "./tool-comment-edit";

export default function ReplyItem({ reply, toolId, commentId }: { reply: CommentSchema; toolId: string, commentId: string }) {
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