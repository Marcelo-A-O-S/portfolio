import { Button } from "@/components/ui/button";
import { Dialog, DialogTrigger } from "@/components/ui/dialog";
import { DropdownMenuItem } from "@/components/ui/dropdown-menu";
import { contributorSchema, ContributorSchema } from "@/domain/schemas/ContributorSchema";
import { useAddContributorPost } from "@/hooks/Post/Contributor/useAddContributorPost";
import { useUpdateContributorPost } from "@/hooks/Post/Contributor/useUpdateContributorPost";
import { zodResolver } from "@hookform/resolvers/zod";
import { useEffect } from "react";
import { useForm } from "react-hook-form";
import { toast } from "sonner";
type FormContributorProps = {
    contributor?: ContributorSchema,
    postId: string
}
export default function FormContributorPost({ postId, contributor }: FormContributorProps) {
    const { mutateAsync: createContributor, isPending: isAdding } = useAddContributorPost();
    const { mutateAsync: updateContributor, isPending: isUpdating } = useUpdateContributorPost();
    const { control, handleSubmit, reset, formState: { errors } } = useForm<ContributorSchema>({
        resolver: zodResolver(contributorSchema),
        defaultValues: {
            postId: postId,
            name: ""
        }
    })
    useEffect(() => {
        if (contributor) {
            reset(contributor);
        }
    }, [contributor, reset]);
    const onSubmit = async (data: ContributorSchema) => {
        if (contributor) {
            if (!contributor.id)
                return toast.error("O identificador não pode ser nulo.");
            updateContributor({ id: contributor.id, data: data })
        } else {
            createContributor(data)
        }
    }
    const isSubmitting = isAdding || isUpdating;
    return (
        <>
            <Dialog
                onOpenChange={(open) => {
                    if (!open) reset()
                }}
            >
                <DialogTrigger asChild>
                    {contributor ?
                        <DropdownMenuItem
                            onSelect={(e) => e.preventDefault()}
                            className="cursor-pointer">Update Contributor</DropdownMenuItem>
                        :
                        <Button className="cursor-pointer">Add Contributor</Button>}
                </DialogTrigger>
            </Dialog>
        </>
    )
}