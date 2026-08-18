"use client"
import { CommentSchema } from "@/domain/schemas/CommentSchema"
import { useGetPostCommentPagination } from "@/hooks/Post/Comment/useGetPostCommentPagination";
import { useSession } from "next-auth/react";
import { useState } from "react";
import PostCommentForm from "./post-comment-form";

type PostCommentProps = {
    postId: string,
    initialItems: CommentSchema[]
}
export default function PostComments({ postId, initialItems }: PostCommentProps) {
    const { data: currentUser } = useSession();
    const [page, setPage] = useState(1);
    const { data: paginatedResult, isLoading } = useGetPostCommentPagination({
        targetId: postId,
        type: "Post",
        page: page
    })
    const items = paginatedResult?.items ?? initialItems;
    return (
        <>
            <section aria-labelledby="comments-heading" className="mt-8">
                {currentUser ? (
                    <PostCommentForm postId={postId} />
                ) : (
                    <p className="text-sm text-muted-foreground mb-6">
                        Faça login para deixar um comentário.
                    </p>
                )}
            </section>
        </>
    )
}