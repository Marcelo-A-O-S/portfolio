import { Button } from "@/components/ui/button"
import { Dialog, DialogClose, DialogContent, DialogDescription, DialogFooter, DialogHeader, DialogTitle, DialogTrigger } from "@/components/ui/dialog"
import { DropdownMenuItem } from "@/components/ui/dropdown-menu"
import { Field, FieldGroup } from "@/components/ui/field"
import { Input } from "@/components/ui/input"
import { Label } from "@/components/ui/label"
import { languageSchema, LanguageSchema } from "@/domain/schemas/LanguageSchema"
import { useCreateLanguage } from "@/hooks/useCreateLanguage"
import { useUpdateLanguage } from "@/hooks/useUpdateLanguage"
import { zodResolver } from "@hookform/resolvers/zod"
import { useEffect } from "react"
import { useForm, Controller } from "react-hook-form"
import { toast } from "sonner"
type FormLanguageProps = {
    language?: LanguageSchema
}
export default function FormLanguage({ language }: FormLanguageProps) {
    const {mutateAsync: createLanguage } = useCreateLanguage();
    const {mutateAsync: updateLanguage } = useUpdateLanguage();
    const { control, handleSubmit, reset } = useForm<LanguageSchema>({
        resolver: zodResolver(languageSchema),
        defaultValues: {
            code: "",
            name: "",
        }
    })
    useEffect(()=>{
        if(language){
            reset(language)
        }
    },[language])
    const onSubmit = async(data: LanguageSchema) =>{
        if(language){
            if(language.id == null)
                return toast.error("O identificador não pode ser nulo.");
            await updateLanguage({id: language.id, language: data});
        }else{
            await createLanguage(data);
        } 
    }
    return (
        <>
            <Dialog>
                <DialogTrigger asChild>
                    {language ?
                        <DropdownMenuItem onSelect={(e) => e.preventDefault()} className="cursor-pointer">Update Language</DropdownMenuItem>
                        :
                        <Button className="cursor-pointer">Add Language</Button>}
                </DialogTrigger>
                <DialogContent className="sm:max-w-sm">
                    <form onSubmit={handleSubmit(onSubmit)} className="">
                        <DialogHeader>
                            <DialogTitle>{language ? "Update Language" : "Create Language"}</DialogTitle>
                        </DialogHeader>
                        <FieldGroup>
                            <div className="grid grid-cols-1 md:grid-cols-2 gap-2 py-3 ">
                                <Controller
                                    name={"code"}
                                    control={control}
                                    render={({ field }) => (
                                        <Field>
                                            <Label >Code</Label>
                                            <Input {...field} name="name" />
                                        </Field>
                                    )}
                                />
                                <Controller
                                    name="name"
                                    control={control}
                                    render={({ field }) => (
                                        <Field>
                                            <Label htmlFor="username-1">Name</Label>
                                            <Input {...field} name="username" />
                                        </Field>
                                    )}
                                />
                            </div>
                        </FieldGroup>
                        <DialogFooter>
                            <DialogClose asChild>
                                <Button variant="outline">Cancel</Button>
                            </DialogClose>
                            <Button type="submit">Save changes</Button>
                        </DialogFooter>
                    </form>
                </DialogContent>
            </Dialog>
        </>
    )
}