import { toolSchema } from "@/domain/schemas/ToolSchema";
import { getToolByIdService } from "@/services/server/tool-services"
import { Metadata } from "next";
import { notFound } from "next/navigation";
import ToolView from "../../components/tool-view";
import { getServerSession } from "next-auth";
import remarkGfm from "remark-gfm";
import rehypeHighlight from "rehype-highlight";
import rehypeRaw from "rehype-raw";
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
type Props = {
    params: Promise<{ Id: string, language: string }>
}
async function getToolOrThrow(id: string) {
    const response = await getToolByIdService(id);
    if (response.status !== 200) {
        notFound();
    }
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
    const keywords = [...categories, content.name, content.slug]
    return {
        title: content.name,
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
            title: content.name,
            description: content.description,
            images: [{
                url: tool.imgUrl,
                alt: content.name
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
        .use(rehypeStringify)
        .use(rehypeSlug)
        .use(rehypeAutolinkHeadings)
        .process(toolContent.content)).toString()
    return (
        <>
            <main className="relative mx-auto flex min-h-screen inset-0 w-screen max-w-[1440px] justify-center bg-white dark:bg-black ">
                <MaxWidthWrapper className="prose prose-neutral dark:prose-invert px-10 py-20">
                    <div className="flex px-12 gap-8">
                        <div className="">
                            <h1 className="text-5xl font-bold">{toolContent.name}</h1>
                            <div className="flex">
                                {categories.map((name, index) => (
                                    <Badge key={index} variant="secondary">{name}</Badge>
                                ))}
                            </div>
                            <div>
                                <img src={tool.imgUrl} alt={toolContent.name} className="object-cover w-full"/>
                            </div>
                            <div dangerouslySetInnerHTML={{ __html: result }} />
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