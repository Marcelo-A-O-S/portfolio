import { Avatar, AvatarFallback, AvatarImage } from "@/components/ui/avatar";
import { Button } from "@/components/ui/button";
import { Dialog, DialogClose, DialogContent, DialogDescription, DialogFooter, DialogHeader, DialogTitle, DialogTrigger } from "@/components/ui/dialog";
import { DropdownMenuItem } from "@/components/ui/dropdown-menu";
import { Field, FieldGroup } from "@/components/ui/field";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import { Select, SelectContent, SelectGroup, SelectItem, SelectLabel, SelectTrigger, SelectValue } from "@/components/ui/select";
import { contributorSchema, ContributorSchema } from "@/domain/schemas/ContributorSchema";
import { useAddContributorPost } from "@/hooks/Post/Contributor/useAddContributorPost";
import { useUpdateContributorPost } from "@/hooks/Post/Contributor/useUpdateContributorPost";
import { useDebounce } from "@/hooks/useDebounce";
import { useGetUsers } from "@/hooks/useGetUsers";
import { zodResolver } from "@hookform/resolvers/zod";
import { useEffect, useState } from "react";
import { Controller, useForm } from "react-hook-form";
import { toast } from "sonner";
type FormContributorProps = {
    contributor?: ContributorSchema,
    postId: string
}
export default function FormContributorPost({ postId, contributor }: FormContributorProps) {
    const [searchInput, setSearchInput] = useState("");
    const debouncedSearch = useDebounce(searchInput, 500);
    const { data: users, isLoading, error } = useGetUsers(searchInput);
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
                <DialogContent >
                    <form onSubmit={handleSubmit(onSubmit, (errors) => {
                        console.log("Erros RHF:");
                        console.dir(errors, { depth: null });
                    })}
                        className=" "
                    >
                        <DialogHeader>
                            <DialogTitle>{contributor ? "Update Contributor" : "Create Contributor"}</DialogTitle>
                            <DialogDescription>
                                {contributor ? `Make changes to your contributor here. Click save when you're
                                done.`: `Add your contributor here. Click save when you're done.`}
                            </DialogDescription>
                        </DialogHeader>
                        <div className="flex min-h-0 flex-1 flex-col gap-1 px-6 py-4">
                            <FieldGroup>
                                <Controller
                                    name="name"
                                    control={control}
                                    render={({ field }) => (
                                        <Field className="">
                                            <Label htmlFor="Name">Name</Label>
                                            <Input {...field}
                                                placeholder="Informe o nome" />
                                            {errors.name && <span className="text-sm text-red-600 text-wrap text-justify">{errors.name?.message}</span>}
                                        </Field>
                                    )}
                                />
                                <Controller
                                    name={`userId`}
                                    control={control}
                                    render={({ field }) => (
                                        <Field className="">
                                            <Label htmlFor="language">Language</Label>
                                            <Select
                                                onValueChange={(value) => field.onChange(value)}
                                                value={field.value}
                                            >
                                                <SelectTrigger className="">
                                                    <SelectValue placeholder="Selecione o idioma" />
                                                </SelectTrigger>
                                                <SelectContent>
                                                    <SelectGroup>
                                                        <SelectLabel>User</SelectLabel>
                                                        {users?.map((user, index) => (
                                                            <SelectItem
                                                                key={index}
                                                                value={`${user.id}`}
                                                                className="flex"
                                                            >
                                                                <div className="rounded-full">
                                                                    <Avatar>
                                                                        <AvatarImage src={user.profileUrl!} alt={user.name!} />
                                                                        <AvatarFallback>LR</AvatarFallback>
                                                                    </Avatar>
                                                                </div>
                                                                <div>
                                                                    {user.name}
                                                                </div>
                                                            </SelectItem>
                                                        ))}
                                                    </SelectGroup>
                                                </SelectContent>
                                            </Select>
                                        </Field>
                                    )}
                                />
                                <Controller
                                    name="description"
                                    control={control}
                                    render={({ field }) => (
                                        <Field className="">
                                            <Label htmlFor="Description">Description</Label>
                                            <Input {...field}
                                                placeholder="Informe a descrição" />
                                            {errors.name && <span className="text-sm text-red-600 text-wrap text-justify">{errors.description?.message}</span>}
                                        </Field>
                                    )}
                                />
                                <Controller
                                    name="profileUrl"
                                    control={control}
                                    render={({ field }) => (
                                        <Field className="">
                                            <Label htmlFor="profileUrl">Profile Url:</Label>
                                            <Input {...field}
                                                placeholder="Informe a descrição" />
                                            {errors.name && <span className="text-sm text-red-600 text-wrap text-justify">{errors.profileUrl?.message}</span>}
                                        </Field>
                                    )}
                                />
                            </FieldGroup>
                        </div>
                        <DialogFooter className="shrink-0 border-t px-6 py-4">
                            <DialogClose asChild>
                                <Button className="cursor-pointer" variant="outline">Cancel</Button>
                            </DialogClose>
                            <Button
                                className="cursor-pointer"
                                type="submit"
                                disabled={isSubmitting}
                            >
                                {contributor ? `Update` : `Save`}
                            </Button>
                        </DialogFooter>
                    </form>
                </DialogContent>
            </Dialog>
        </>
    )
}