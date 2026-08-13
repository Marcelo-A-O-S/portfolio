import { AuthorSchema } from "@/domain/schemas/AuthorSchema"
import { Avatar, AvatarFallback, AvatarImage } from "./ui/avatar"

type AuthorProps = {
    author: AuthorSchema
}
export default function AuthorSection({ author }: AuthorProps) {
    return (
        <>
            <div className="flex items-center mb-4">
                <div className="flex items-center justify-center gap-2 flex-col md:flex-row">
                    <div className="">
                        <img className="object-cover w-15 h-15 mt-3 mr-3 rounded-full" src={author.profileUrl}/>
                    </div>
                    <div className="flex flex-col">
                        <div className="text-2xl font-medium">
                            {author.username}
                        </div>
                        <div className="text-sm text-muted-foreground">
                            <p>Author</p>
                        </div>
                    </div>
                </div>
            </div>
        </>
    )
}