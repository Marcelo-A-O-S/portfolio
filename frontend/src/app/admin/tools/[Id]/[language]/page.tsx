import { toolSchema } from "@/domain/schemas/ToolSchema";
import { getToolByIdService } from "@/services/server/tool-services"
import { Metadata } from "next";
import { notFound } from "next/navigation";
import ToolView from "../../components/tool-view";
type Props = {
    params: Promise<{ Id: string, language:string }>
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
export async function generateMetadata({ params }:Props): Promise<Metadata>{
    const { Id, language } = await params;
    const tool = await getToolOrThrow(Id);
    const content = tool.toolContents.find(
        c => c.language?.code === language
    )
    if(!content){
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
            images:[{
                url: tool.imgUrl,
                alt: content.name
            }],
            type:"article"
        },
        keywords: keywords,
    }
}

export default async function PageById({ params }: Props) {
    const { Id, language } = await params;
    const tool = await getToolOrThrow(Id);
    const toolContent = tool.toolContents.find(
        c => c.language?.code === language
    )
    if(!toolContent){
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
    return <ToolView toolData={tool} toolContent={toolContent} categoriesData={categories} />
}