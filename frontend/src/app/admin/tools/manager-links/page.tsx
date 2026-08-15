"use client"
import { linkSchema, LinkSchema } from "@/domain/schemas/LinkSchema";
import { zodResolver } from "@hookform/resolvers/zod";
import { useSearchParams } from "next/navigation";
import { useForm } from "react-hook-form";

export default function ManagerLinksPage() {
    const searchParams = useSearchParams();
    const toolId = searchParams.get("toolId") || undefined;
    const {} = useForm<LinkSchema>({
        resolver: zodResolver(linkSchema)
    })
}