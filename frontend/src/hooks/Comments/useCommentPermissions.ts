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
            !!user &&
            isOwner &&
            comment.commentStatus === "Active",
        canDelete:
            !!user &&
            isOwner &&
            comment.commentStatus === "Active",
        canRestore:
            !!user &&
            comment.commentStatus === "Deleted" &&
            (
                user?.role === "Moderator" ||
                user?.role === "Administrador"//"Administrador"
            ),
        canHardDelete:
            !!user &&
            comment.commentStatus === "Deleted" &&
            user?.role === "Administrador",
        canLike:
            !!user &&
            comment.commentStatus === "Active",
        canReply: true
    };
}