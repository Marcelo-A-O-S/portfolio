import { Avatar, AvatarFallback, AvatarImage } from "@/components/ui/avatar";
import { CommentSchema } from "@/domain/schemas/CommentSchema";
import { formatDistanceToNow } from "date-fns";
import { ptBR } from "date-fns/locale";
import { CornerDownRight } from "lucide-react";

export default function CommentHeader({ comment, compact = false , isReply }: { comment: CommentSchema; compact?: boolean, isReply?: boolean }){

    return(
        <div className={`flex items-center ${compact ? "mb-2" : "mb-4"}`}>
            <div className="flex items-center justify-center gap-2">
                {isReply && (
                    <CornerDownRight size={14} className="text-muted-foreground shrink-0" />
                )}
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