import { ToolContentSchema } from "@/domain/schemas/ToolContentSchema"
import { ToolSchema } from "@/domain/schemas/ToolSchema"

type ToolViewProps = {
    toolData: ToolSchema,
    content: ToolContentSchema,
    categoriesData: string[]
}
export default function ToolView({ toolData, content, categoriesData }: ToolViewProps) {
    return (
        <>
        <main className="">

        </main>
        </>
    )
}