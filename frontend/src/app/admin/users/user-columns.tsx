import { User } from "@/domain/types/User"
import { ColumnDef } from "@tanstack/react-table"
import { Button } from "@/components/ui/button"
export const getUsersColumns = () => {
    const columns: ColumnDef<User>[] = [
        {
            accessorKey: "name",
            header: "Nome"
        },
        {
            accessorKey: "email",
            header: "Email"
        },
        {
            accessorKey: "role",
            header: "Função"
        },
        {
            accessorKey: "status",
            header: "Status"
        },
        {
            accessorKey: "createdAt",
            header: "Data de registro",
            cell:({getValue})=>{
                const createdAt = getValue() as string
                const formated = new Date(createdAt).toLocaleDateString("pt-br");
                return(
                    <>
                    <div>
                        {formated}
                    </div>
                    </>
                )
            }
        },{
            accessorKey:"actions",
            header:"Ações",
            cell:({getValue})=>{
                
                return(
                    <>
                    <div>
                        <Button>Banir</Button>
                        <Button>Opções</Button>
                    </div>
                    </>
                )
            }
        }
    ]
    return columns;
}