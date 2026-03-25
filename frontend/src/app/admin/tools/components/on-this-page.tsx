"use client";
import { useMemo } from "react";
type OnThisPageProps = {
    htmlContent: string
}
type LinkType = {
    id: string,
    text: string
}
export default function OnThisPage({ htmlContent }: OnThisPageProps) {
    const links = useMemo(() => {
        const temp = document.createElement("div");
        temp.innerHTML = htmlContent;

        const headings = temp.querySelectorAll("h2, h3");

        const generatedLinks: LinkType[] = [];

        headings.forEach((heading, index) => {
            const id = heading.id || `heading-${index}`;
            heading.id = id;

            generatedLinks.push({
                id,
                text: heading.textContent
            });
        });

        return generatedLinks;

    }, [htmlContent]);
    return (
        <>
        <div className="hidden md:block text-sm ">
            <div className="sticky top-16">
                <h2>On this page</h2>
                <ul className="not-prose">
                    {links && links.map((link)=>(
                        <li key={link.id} className="pt-2">
                            <a href={`#${link.id}`}>{link.text.slice(0,50)}</a>
                        </li>
                    ))}
                </ul>
            </div>
        </div>
        </>
    )
}