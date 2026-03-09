import Navbar from "./navbar"
import { getServerSession } from "next-auth"
export default async function Header(){
    const session = await getServerSession();
    return(
        <>
        <Navbar session={session} />
        </>
    )
}