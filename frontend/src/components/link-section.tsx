import { LinkSchema } from "@/domain/schemas/LinkSchema"
import Link from "next/link";
import { buttonVariants } from "./ui/button";
import Image from "next/image";

type LinksProps = {
    link: LinkSchema,
    language?: string
}
export default function LinkSection({ link, language }: LinksProps) {
    return (
        <div className="flex flex-col">
            {link.descriptions.map((item, index) => {
                if(language){
                    if(item.language?.code !== language)
                        return;
                }
                return (
                    <Link
                        key={item.id}
                        href={link.url}
                        target="_blank"
                        style={{
                            backgroundColor: link.linkType?.backgroundColor,
                            color: link.linkType?.textColor,
                            borderColor: link.linkType?.borderColor,
                            border: "1px",
                            borderStyle: "solid"
                        }}
                        className={`${buttonVariants({ variant: "default" })} cursor-pointer`}
                    >
                        <Image src={link.linkType!.icon} alt={link.linkType!.name} width={20} height={20} />
                        {link.linkType?.name} | {item.title}
                    </Link>
                )
            })}

        </div>

    )
}