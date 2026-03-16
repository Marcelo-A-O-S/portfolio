import { Button } from "@/components/ui/button"
import { Dialog, DialogClose, DialogContent, DialogDescription, DialogFooter, DialogHeader, DialogTitle, DialogTrigger } from "@/components/ui/dialog"
import { DropdownMenuItem } from "@/components/ui/dropdown-menu"
import { Field, FieldGroup } from "@/components/ui/field"
import { Input } from "@/components/ui/input"
import { Label } from "@/components/ui/label"
import { languageSchema, LanguageSchema } from "@/domain/schemas/LanguageSchema"
import { zodResolver } from "@hookform/resolvers/zod"
import { useForm } from "react-hook-form"
type FormLanguageProps = {
    language?: LanguageSchema
}
export default function FormLanguage({ language }: FormLanguageProps) {
    const { control, handleSubmit } = useForm<LanguageSchema>({
        resolver: zodResolver(languageSchema),
        defaultValues: {
            code: "",
            name: "",
        }
    })
    return (
        <>

            <Dialog>
                <form>
                    <DialogTrigger asChild>
                        {language ?
                            <DropdownMenuItem onSelect={(e) => e.preventDefault()} className="cursor-pointer">Update Language</DropdownMenuItem>
                            :
                            <Button className="cursor-pointer">Add Language</Button>}
                    </DialogTrigger>
                    <DialogContent className="sm:max-w-sm">
                        <DialogHeader>
                            <DialogTitle>{language ? "Update Language" : "Create Language"}</DialogTitle>
                            
                        </DialogHeader>
                        <FieldGroup>
                            <Field>
                                <Label htmlFor="name-1">Name</Label>
                                <Input id="name-1" name="name" defaultValue="Pedro Duarte" />
                            </Field>
                            <Field>
                                <Label htmlFor="username-1">Username</Label>
                                <Input id="username-1" name="username" defaultValue="@peduarte" />
                            </Field>
                        </FieldGroup>
                        <DialogFooter>
                            <DialogClose asChild>
                                <Button variant="outline">Cancel</Button>
                            </DialogClose>
                            <Button type="submit">Save changes</Button>
                        </DialogFooter>
                    </DialogContent>
                </form>
            </Dialog>
        </>
    )
}