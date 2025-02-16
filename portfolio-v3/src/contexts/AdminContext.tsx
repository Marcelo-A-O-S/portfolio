import {createContext, ReactNode, useEffect, useState } from "react";
type PropsAdminProvider = {
    children: ReactNode
}
type PropsAdminContext = {
    registered: boolean,
    onchange: (registered: boolean) => void
}
export const AdminContext = createContext({} as PropsAdminContext)
export default function AdminProvider({children}: PropsAdminProvider){
    const [AdminRegistered, SetAdminRegistered] = useState(false)
    useEffect(()=>{
        const checkAdminStatus = async () => {
            const response = await fetch('/check-admin'); 
            const data = await response.json();
            SetAdminRegistered(data); 
        };
        checkAdminStatus();
    }, [])
    return(
        <>
          <AdminContext.Provider value={{registered: AdminRegistered, onchange: SetAdminRegistered}}>
            {children}
          </AdminContext.Provider>
        </>
    );
}