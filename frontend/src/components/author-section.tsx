import { AuthorSchema } from "@/domain/schemas/AuthorSchema"
import { Avatar, AvatarFallback, AvatarImage } from "./ui/avatar"

type AuthorProps = {
    author: AuthorSchema
}
export default function AuthorSection({ author }: AuthorProps) {
    return (
        <>
            <div className="flex min-w-0 mb-4">
                <div className="flex gap-3 min-w-0 flex-col md:flex-row md:items-start">
                    <img
                        className="w-12 h-12 shrink-0 object-cover rounded-full"
                        src={author.profileUrl}
                        alt={author.username}
                    />
                    <div className="flex flex-col min-w-0 justify-start">
                        <p className="m-0 text-2xl font-medium leading-none">
                            {author.username}
                        </p>
                        <p className="m-0 text-sm text-muted-foreground leading-none">
                            Author
                        </p>
                        {author.description && (
                            <p className="m-0 text-sm leading-normal mt-2">
                                {author.description}
                            </p>
                        )}
                    </div>
                </div>
            </div>
        </>
    )
}