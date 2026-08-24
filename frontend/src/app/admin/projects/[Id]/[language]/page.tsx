import OnThisPage from "@/app/admin/tools/components/on-this-page";
import AuthorSection from "@/components/author-section";
import LinkSection from "@/components/link-section";
import MaxWidthWrapper from "@/components/max-width-wrapper";
import { Badge } from "@/components/ui/badge";
import { postSchema } from "@/domain/schemas/PostSchema";
import { rehypePrefixImageHost } from "@/lib/utils";
import { getPostByIdService, getPostCommentsByPagination } from "@/services/server/post-services";
import { transformerCopyButton } from "@rehype-pretty/transformers";
import { Heart, MessageCircle } from "lucide-react";
import { Metadata } from "next";
import { getServerSession } from "next-auth";
import Link from "next/link";
import { notFound } from "next/navigation";
import rehypeAutolinkHeadings from "rehype-autolink-headings";
import rehypePrettyCode from "rehype-pretty-code";
import rehypeSlug from "rehype-slug";
import rehypeStringify from "rehype-stringify";
import remarkGfm from "remark-gfm";
import remarkParse from "remark-parse";
import remarkRehype from "remark-rehype";
import { unified } from "unified";
import PostComments from "../../components/post-comments";
import ContributorsSection from "@/components/contributors-section";
const hostBackend = process.env.BACKEND_SERVER!;
type Props = {
    params: Promise<{ Id: string, language: string }>
}
async function getPostOrThrow(id: string) {
    const response = await getPostByIdService(id);
    console.log(response.data);
    if (response.status !== 200) {
        notFound();
    }
    const result = await postSchema.safeParseAsync(response.data);
    if (result.error) {
        console.log(`Error ao validar: ${result.error.message}`);
        notFound();
    }
    return result.data;
}
export async function generateMetadata({ params }: Props): Promise<Metadata> {
    const { Id, language } = await params;
    const post = await getPostOrThrow(Id);
    const content = post.postContents.find(
        c => c.language?.code === language
    )
    if (!content) {
        return notFound();
    }
    const tools = post.tools
        .map(t => {
            const toolContent = t.toolContents.find(
                tc => tc.language?.code === language
            )
            return toolContent?.name;
        })
        .filter((c): c is string => Boolean(c));
    const categories = post.categories
        .map(c => {
            const categoryContent = c.categoryContents.find(
                cc => cc.language?.code === language
            );
            return categoryContent?.name;
        })
        .filter((c): c is string => Boolean(c));
    const keywords = [...categories, ...tools, content.title, content.slug]
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
            siteName: content.title,
            title: content.title,
            description: content.description,
            images: [{
                url: `${hostBackend}/${post.media?.url}`,
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
    const post = await getPostOrThrow(Id);
    const postContent = post.postContents.find(
        c => c.language?.code === language
    )
    if (!postContent) {
        return notFound();
    }
    const tools = post.tools
        .map(t => {
            const toolContent = t.toolContents.find(
                tc => tc.language?.code === language
            )
            return {
                name: toolContent?.name,
                url: `/admin/tools/${t?.id}/${language}`
            }
        })
    const categories = post.categories
        .map(c => {
            const categoryContent = c.categoryContents.find(
                cc => cc.language?.code === language
            );
            return categoryContent?.name;
        })
        .filter((c): c is string => Boolean(c));
    const [renderedContent, commentsResponse] = await Promise.all([
        unified()
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
            .process(postContent.content)
            .then((r) => r.toString()),
        getPostCommentsByPagination({ targetId: Id, type: "Post", page: 1 })
    ])
    return (
        <>
            <main className="mx-auto flex min-h-screen w-full max-w-[1440px] justify-center ">
                <MaxWidthWrapper className="prose prose-neutral dark:prose-invert px-4 md:px-10 py-20">
                    <div className="flex md:px-12 gap-8">
                        <article className="min-w-0 prose prose-neutral dark:prose-invert md:max-w-none">
                            <h1 className="text-5xl font-bold">{postContent.title}</h1>
                            <div className="flex flex-col">
                                <p>Ferramentas</p>
                                <div>
                                    {tools.map((tool, index) => (
                                        <Badge key={index} variant="secondary">
                                            <Link href={tool.url} >{tool.name}</Link>
                                        </Badge>
                                    ))}
                                </div>
                                <p>Categorias:</p>
                                <div className="">
                                    {categories.map((name, index) => (
                                        <Badge key={index} variant="secondary">{name}</Badge>
                                    ))}
                                </div>

                            </div>
                            <div>
                                <img src={`${hostBackend}/${post.media?.url}`}
                                    alt={postContent.title}
                                    loading="eager"
                                    className="max-w-full w-full rounded-lg object-cover" />
                            </div>
                            <div dangerouslySetInnerHTML={{ __html: renderedContent }} />
                            <div className="flex items-center justify-between mt-4 text-primary text-xs sm:text-sm">
                                <div className="flex">
                                    <button
                                        className="flex items-center space-x-1 p-2 rounded-full cursor-pointer">
                                        <Heart
                                            className={post.liked ? "fill-current" : ""}
                                        />
                                        <span>{post.likes}</span>
                                    </button>
                                    <button className="flex items-center space-x-1 p-2 rounded-full cursor-pointer">
                                        <MessageCircle />
                                        <span>{post.comments}</span>
                                    </button>
                                </div>
                            </div>
                            <div className="">
                                {post.links?.map((link, index) => (
                                    <LinkSection key={index} link={link} language={language} />
                                ))}
                            </div>
                            <div className="max-w-full">
                                {post.author && (
                                    <AuthorSection
                                        author={post.author}
                                    />
                                )}
                            </div>
                            <div className="max-w-full">
                                {post.contributors && (
                                    <ContributorsSection
                                        contributors={post.contributors}
                                    />
                                )}
                            </div>
                            <div className="max-w-full">
                                <PostComments
                                    postId={Id}
                                    initialItems={commentsResponse.data.items}
                                />
                            </div>
                        </article>
                        <div className="hidden md:block md:w-full lg:w-[50%]">
                            <OnThisPage htmlContent={renderedContent} />
                        </div>
                    </div>
                </MaxWidthWrapper>
            </main>
        </>
    )
}