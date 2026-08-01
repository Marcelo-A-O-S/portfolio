import { CommentSchema } from "@/domain/schemas/CommentSchema";
import { useSession } from "next-auth/react";

export function useCommentPermissions(comment: CommentSchema){
    const { data : session } = useSession();
    const user = session?.user;
    const isOwner = user?.id === comment.user?.id;
    return {
        isDeleted:
            comment.commentStatus === "Deleted",
        canEdit:
            isOwner &&
            comment.commentStatus === "Active",
        canDelete:
            isOwner &&
            comment.commentStatus === "Active",
        canRestore:
            comment.commentStatus === "Deleted" &&
            (
                user?.role === "Moderator" ||
                user?.role === "Administrador"//"Administrador"
            ),
        canHardDelete:
            comment.commentStatus === "Deleted" &&
            user?.role === "Administrador",
        canLike:
            comment.commentStatus === "Active",
        canReply: true
    };
}