import { unified } from "unified"
import remarkParse from "remark-parse"
import remarkRehype from "remark-rehype"
import rehypeRaw from "rehype-raw"
import rehypeStringify from "rehype-stringify"
import rehypeHighlight from "rehype-highlight"
import { ToolContentSchema } from "@/domain/schemas/ToolContentSchema"
import { ToolSchema } from "@/domain/schemas/ToolSchema"
import MaxWidthWrapper from "@/components/max-width-wrapper"
import remarkGfm from "remark-gfm"
type ToolViewProps = {
    toolData: ToolSchema,
    toolContent: ToolContentSchema,
    categoriesData: string[]
}
export default function ToolView({ toolData, toolContent, categoriesData }: ToolViewProps) {
    const result = unified()
        .use(remarkParse)
        .use(remarkGfm)  
        .use(remarkRehype, { allowDangerousHtml: true })
        .use(rehypeRaw)
        .use(rehypeHighlight)
        .use(rehypeStringify)
        .processSync(toolContent.content).toString()

    return (
        <>
            <main className="">
                <MaxWidthWrapper className="prose dark:prose-invert">
                    <h1 className="text-2xl font-bold">{toolContent.name}</h1>
                    <div dangerouslySetInnerHTML={{__html:result}}/>
                </MaxWidthWrapper>
            </main>
        </>
    )
}