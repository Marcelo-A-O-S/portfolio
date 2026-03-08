import Navbar from "./navbar"
import { getServerSession } from "next-auth"
export default async function Header(){
    const session = await getServerSession();
    console.log("Session: ",session);
    return(
        <>
        <Navbar session={session} />
        </>
    )
}