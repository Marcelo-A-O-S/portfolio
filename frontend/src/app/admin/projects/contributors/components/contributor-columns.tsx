import { ContributorSchema } from "@/domain/schemas/ContributorSchema"
import { ColumnDef } from "@tanstack/react-table"

export const getContributorsColumns = () =>{
    const columns: ColumnDef<ContributorSchema>[] =[
        {
            header: "Preview"
        }
    ]
    return columns;
}