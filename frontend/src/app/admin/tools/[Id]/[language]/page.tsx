import { toolSchema } from "@/domain/schemas/ToolSchema";
import { getToolByIdService } from "@/services/server/tool-services"
import { Metadata } from "next";
import { notFound } from "next/navigation";
import { getServerSession } from "next-auth";
import remarkGfm from "remark-gfm";
import rehypeStringify from "rehype-stringify";
import remarkParse from "remark-parse";
import remarkRehype from "remark-rehype";
import { unified } from "unified";
import MaxWidthWrapper from "@/components/max-width-wrapper";
import { Badge } from "@/components/ui/badge";
import rehypeAutolinkHeadings from 'rehype-autolink-headings'
import rehypeSlug from 'rehype-slug'
import OnThisPage from "../../components/on-this-page";
import rehypePrettyCode from "rehype-pretty-code";
import { transformerCopyButton } from '@rehype-pretty/transformers'
import { rehypePrefixImageHost } from "@/lib/utils";
import { Heart, MessageCircle } from "lucide-react";
import { Button } from "@/components/ui/button";
import { Field } from "@/components/ui/field";
import { InputGroup, InputGroupTextarea } from "@/components/ui/input-group";
const hostBackend = process.env.BACKEND_SERVER!;
type Props = {
    params: Promise<{ Id: string, language: string }>
}
async function getToolOrThrow(id: string) {
    const response = await getToolByIdService(id);
    if (response.status !== 200) {
        notFound();
    }
    console.log("Retorno: ", response.data);
    const result = await toolSchema.safeParseAsync(response.data);
    if (result.error) {
        console.log(`Error ao validar: ${result.error.message}`);
        notFound();
    }
    return result.data;
}
export async function generateMetadata({ params }: Props): Promise<Metadata> {
    const { Id, language } = await params;
    const tool = await getToolOrThrow(Id);
    const content = tool.toolContents.find(
        c => c.language?.code === language
    )
    if (!content) {
        return notFound();
    }
    const categories = tool.categories
        .map(c => {
            const categoryContent = c.categoryContents.find(
                cc => cc.language?.code === language
            );
            return categoryContent?.name;
        })
        .filter((c): c is string => Boolean(c));
    const keywords = [...categories, content.name, content.title, content.slug]
    return {
        title: content.title,
        description: content.description,
        authors: [{
            name: 'Marcelo Augusto de Oliveira Soares', url: 'https://github.com/Marcelo-A-O-S'
        }],
        generator: "Next.js",
        robots: 'index, follow',
        creator: "Marcelo Augusto de Oliveira Soares",
        category: "education",
        openGraph: {
            siteName: content.name,
            title: content.title,
            description: content.description,
            images: [{
                url: `${hostBackend}/${tool.media?.url}`,
                alt: content.title
            }],
            type: "article"
        },
        keywords: keywords,
    }
}
export default async function PageById({ params }: Props) {
    const session = await getServerSession();
    const { Id, language } = await params;
    const tool = await getToolOrThrow(Id);
    const toolContent = tool.toolContents.find(
        c => c.language?.code === language
    )
    if (!toolContent) {
        return notFound();
    }
    const categories = tool.categories
        .map(c => {
            const categoryContent = c.categoryContents.find(
                cc => cc.language?.code === language
            );
            return categoryContent?.name;
        })
        .filter((c): c is string => Boolean(c));
    const result = (await unified()
        .use(remarkParse)
        .use(remarkGfm)
        .use(remarkRehype, { allowDangerousHtml: true })
        .use(rehypePrettyCode, {
            theme: "tokyo-night",
            transformers: [
                transformerCopyButton({
                    visibility: 'always',
                    feedbackDuration: 3_000,
                }),
            ],
        })
        .use(rehypePrefixImageHost(hostBackend))
        .use(rehypeSlug)
        .use(rehypeAutolinkHeadings)
        .use(rehypeStringify)
        .process(toolContent.content)).toString()
    return (
        <>
            <main className="relative mx-auto flex min-h-screen inset-0 w-screen max-w-[1440px] justify-center bg-white dark:bg-black ">
                <MaxWidthWrapper className="prose prose-neutral dark:prose-invert px-10 py-20">
                    <div className="flex px-12 gap-8">
                        <div className="">
                            <h1 className="text-5xl font-bold">{toolContent.title}</h1>
                            <div className="flex">
                                {categories.map((name, index) => (
                                    <Badge key={index} variant="secondary">{name}</Badge>
                                ))}
                            </div>
                            <div>
                                <img src={`${hostBackend}/${tool.media?.url}`} alt={toolContent.name} className="object-cover w-full" />
                            </div>
                            <div dangerouslySetInnerHTML={{ __html: result }} />
                            <div className="flex items-center justify-between mt-4 text-primary text-xs sm:text-sm">
                                <div className="flex">
                                    <button
                                        className="flex items-center space-x-1 p-2 rounded-full cursor-pointer">
                                        <Heart
                                            className={tool.liked ? "fill-current" : ""}
                                        />
                                        <span>{tool.likes}</span>
                                    </button>
                                    <button className="flex items-center space-x-1 p-2 rounded-full cursor-pointer">
                                        <MessageCircle />
                                        <span>{tool.comments}</span>
                                    </button>
                                </div>
                            </div>
                            <div>
                                <form className="flex-1 flex flex-col gap-2">
                                    <Field className="flex flex-col flex-1 min-h-0">
                                        <InputGroup className="flex-1 min-h-0 items-stretch">
                                            <InputGroupTextarea
                                                placeholder="Escreva um comentário..."
                                                className="flex-1 resize-none overflow-y-auto text-sm leading-relaxed"
                                            />
                                        </InputGroup>
                                    </Field>
                                    <div className="flex justify-end">
                                        <Button type="submit"
                                            className="cursor-pointer">
                                            Post comment
                                        </Button>
                                    </div>
                                </form>
                            </div>
                        </div>
                        <div className="hidden md:block md:w-full lg:w-[50%]">
                            <OnThisPage htmlContent={result} />
                        </div>
                    </div>
                </MaxWidthWrapper>
            </main>
        </>
    )
}