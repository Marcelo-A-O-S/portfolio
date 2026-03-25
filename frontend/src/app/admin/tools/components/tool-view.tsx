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
    return (
        <>
            <main className="">
            </main>
        </>
    )
}