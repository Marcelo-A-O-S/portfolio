import { Badge } from "@/components/ui/badge";
import { buttonVariants } from "@/components/ui/button";
import { SelectTrigger, SelectValue, SelectContent, SelectGroup, SelectLabel, SelectItem, Select } from "@/components/ui/select";
import { LanguageSchema } from "@/domain/schemas/LanguageSchema"
import { PostSchema } from "@/domain/schemas/PostSchema"
import Link from "next/link";
import { useEffect, useState } from "react";
import ProjectActions from "./project-actions";
import { Heart, MessageCircle } from "lucide-react";
import { useAddLikePost } from "@/hooks/useAddLikePost";
import { useRemoveLikePost } from "@/hooks/useRemoveLikePost";
type CardProjectProps = {
    languages?: LanguageSchema[],
    item: PostSchema
}
export default function CardProject({ languages, item }: CardProjectProps) {
    const { mutateAsync: addLike, isPending: isAdding } = useAddLikePost();
    const { mutateAsync: removeLike, isPending: isRemoving } = useRemoveLikePost();
    const [lang, setLang] = useState(languages?.[0]?.code);
    const loading = isAdding || isRemoving;
    const content = item.postContents.find(
        tc => tc.language?.code === lang
    );
    const tools = item.tools
        .map(t => {
            const content = t.toolContents.find(
                tc => tc.language?.code == lang
            )
            return content;
        }
        )
    const categories = item.categories
        .map(c => {
            const content = c.categoryContents.find(
                cc => cc.language?.code === lang
            );
            return content;
        })
    const handleLike = async () => {
        if (item.liked) {
            await removeLike({
                postId: item.id!
            });
        } else {
            await addLike({
                postId: item.id!
            });
        }
    }
    return (
        <>
            <div className="bg-background border border-primary max-w-sm w-full max-h-[580px] h-full rounded-lg overflow-hidden shadow-sm
                hover:shadow-md hover:-translate-y-1 transition-all duration-300 flex flex-col">
                <article className="p-4 flex flex-col space-x-3">
                    <div className="flex flex-col flex-1 min-w-0">
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
                                <SelectTrigger className="w-[90px] max-w-48 text-xs">
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
                        <p className="text-primary text-sm line-clamp-4 mb-2 ">
                            {content?.description}
                        </p>
                        <div className="flex flex-col">

                        </div>
                        <div className="flex flex-col">
                            <div className="flex flex-nowrap overflow-x-auto scrollbar-hide gap-2 py-1">
                                {categories.slice(0, 3).map((cc, index) => (
                                    <Badge key={index} variant="secondary" className="text-xs whitespace-nowrap">{cc?.name}</Badge>
                                ))}
                                {categories.length > 3 && (
                                    <Badge>
                                        +{categories.length - 3}
                                    </Badge>
                                )}
                            </div>
                        </div>
                        <div className="relative mt-3 rounded-xl  border tweet-border overflow-hidden">
                            <img src={`${process.env.NEXT_PUBLIC_FILES_URL}/${item.media?.url}`}
                                alt="" className="w-full object-cover aspect-video relative" />
                            <div className="absolute bottom-2 left-2 flex gap-2 overflow-hidden line-clamp-1 h-5">
                                {tools.slice(0, 3).map((tc, index) => (
                                    <Badge key={index} variant="secondary" className="text-xs whitespace-nowrap">{tc?.name}</Badge>
                                ))}
                                {tools.length > 3 && (
                                    <Badge>
                                        +{tools.length - 3}
                                    </Badge>
                                )}
                            </div>
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
                                    <span>15</span>
                                </button>
                            </div>
                            <div>
                                <ProjectActions project={item} />
                            </div>

                        </div>
                        <Link className={buttonVariants({ variant: "default" }) + ` w-full mt-3`} href={`/admin/projects/${item.id}/${lang}`}>View Project</Link>
                    </div>
                </article>
            </div>
        </>
    )
}