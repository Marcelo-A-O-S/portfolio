"use client"
import { CommentSchema } from "@/domain/schemas/CommentSchema"
import { useGetToolCommentPagination } from "@/hooks/Tool/Comment/useGetToolCommentPagination"
import { useSession } from "next-auth/react"
import { useState } from "react"
import CommentItem from "./tool-comment-item"
import CommentForm from "./tool-comment-form"
type ToolCommentProps = {
    toolId: string,
    initialItems: CommentSchema[]
}
export default function ToolComments({ toolId, initialItems }: ToolCommentProps) {
    const { data: currentUser } = useSession();
    const [page, setPage] = useState(1);
    const { data: paginatedResult, isLoading } = useGetToolCommentPagination({
        targetId: toolId,
        type: "Tool",
        page: page
    })
    const items = paginatedResult?.items ?? initialItems;
    console.log("Comentários: ", items)
    return (
        <>
            <section aria-labelledby="comments-heading" className="mt-8">
                {currentUser ? (
                    <CommentForm toolId={toolId}/>
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