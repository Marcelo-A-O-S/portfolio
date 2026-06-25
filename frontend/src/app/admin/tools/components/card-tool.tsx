import { Badge } from "@/components/ui/badge"
import { Button, buttonVariants } from "@/components/ui/button"
import { SelectTrigger, SelectValue, SelectContent, SelectGroup, SelectLabel, SelectItem, Select } from "@/components/ui/select"
import { LanguageSchema } from "@/domain/schemas/LanguageSchema"
import { ToolSchema } from "@/domain/schemas/ToolSchema"
import { Heart, MessageCircle } from "lucide-react"
import { useState } from "react"
import ToolActions from "./tool-actions"
import Link from "next/link"
import { useAddLikePost } from "@/hooks/useAddLikePost"
import { useRemoveLikePost } from "@/hooks/useRemoveLikePost"
type CardToolProps = {
    languages?: LanguageSchema[]
    item: ToolSchema
}
export default function CardTool({ languages, item }: CardToolProps) {
    const { mutateAsync: addLike, isPending: isAdding } = useAddLikePost();
    const { mutateAsync: removeLike, isPending: isRemoving } = useRemoveLikePost();
    const [lang, setLang] = useState(languages?.[0]?.code);
    const loading = isAdding || isRemoving;
    const content = item.toolContents.find(
        tc => tc.language?.code === lang
    );
    const categories = item.categories
        .map(c => {
            const content = c.categoryContents.find(
                cc => cc.language?.code === lang
            );
            return content;
        })
    const handleLike = async () => {

    }
    console.log("Objeto: ", item);
    return (
        <div className="bg-background border border-primary max-w-sm w-full max-h-[450px] h-full rounded-lg overflow-hidden shadow-sm
            hover:shadow-md hover:-translate-y-1 transition-all duration-300 flex flex-col">
            <article className="p-4 flex flex-col space-x-3">
                <div className="flex flex-col justify-between flex-1 min-w-0">
                    <div className="flex justify-between items-center mb-1">
                        <div className="flex items-baseline space-x-1 text-sm min-w-0">
                            <span className="font-bold text-primary truncate hover:underline cursor-pointer">
                                {content?.title}
                            </span>
                            <span className="text-primary hover:underline cursor-pointer whitespace-nowrap text-xs">
                                3h
                            </span>
                        </div>
                        <Select
                            value={lang}
                            onValueChange={(value) => setLang(value)}
                        >
                            <SelectTrigger className="w-[90px] max-w-48">
                                <SelectValue placeholder="Selecione o idioma" />
                            </SelectTrigger>
                            <SelectContent>
                                <SelectGroup>
                                    <SelectLabel>Idiomas</SelectLabel>
                                    {languages?.map((item, index) => (
                                        <SelectItem key={index} value={item.code}>{item.code}</SelectItem>
                                    ))}
                                </SelectGroup>
                            </SelectContent>
                        </Select>
                    </div>
                    <p className="text-primary text-sm line-clamp-4 mb-2">
                        {content?.description}
                    </p>
                    <div className="flex flex-wrap gap-2 mt-2 line-clamp-1">
                        {categories.map((cc, index) => (
                            <Badge key={index} variant="secondary">{cc?.name}</Badge>
                        ))}

                    </div>
                    <div className="mt-3 rounded-xl border tweet-border overflow-hidden">
                        <img src={`${process.env.NEXT_PUBLIC_FILES_URL}/${item.media?.url}`}
                            alt="" className="w-full object-cover aspect-video border border-primary" />
                    </div>
                    <div className="flex items-center justify-between mt-4 text-primary text-xs sm:text-sm">
                        <div className="flex">
                            <button
                                disabled={loading}
                                onClick={handleLike}
                                className="flex items-center space-x-1 p-2 rounded-full cursor-pointer">
                                <Heart
                                    className={item.liked ? "fill-current" : ""}
                                />
                                <span>{item.likes}</span>
                            </button>
                            <button className="flex items-center space-x-1 p-2 rounded-full cursor-pointer">
                                <MessageCircle />
                                <span>{item.comments}</span>
                            </button>
                        </div>
                        <div>
                            <ToolActions tool={item} />
                        </div>

                    </div>
                    <Link className={buttonVariants({ variant: "default" }) + ` w-full`} href={`/admin/tools/${item.id}/${lang}`}>View Tool</Link>
                </div>
            </article>
        </div>
    )
}