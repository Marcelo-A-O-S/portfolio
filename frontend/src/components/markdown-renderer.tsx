import ReactMarkdown from 'react-markdown';
import remarkGfm from "remark-gfm";
interface MarkdownRendererProps {
    content: string;
}
export default function MarkdownRenderer({content}: MarkdownRendererProps){
    console.log("CONTENT:", content)
    return(
        <>
        <div className="prose max-w-none dark:prose-invert">
            <ReactMarkdown remarkPlugins={[remarkGfm]} >
                {content}
            </ReactMarkdown>
        </div>
        </>
    )
}