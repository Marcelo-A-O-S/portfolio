"use client"
import { useSound } from "@/hooks/useSound";
import { createContext, ReactNode, useContext, useEffect, useState } from "react"
import Cookies from "js-cookie";
import { stringToBoolean } from "@/lib/utils";

type MusicContextProps = {
    enableSounds: boolean | null,
    toggleSounds: (data: boolean) => void
}
const MusicContext = createContext<MusicContextProps>({} as MusicContextProps);
type MusicProviderProps = {
    children: ReactNode
}
export const MusicProvider = ({ children }: MusicProviderProps) => {
    const { playMusicAmbient, stopMusicAmbient } = useSound();
    const [enableSounds, setEnableSounds] = useState<boolean | null>(()=>{
        if (typeof window === "undefined") return null;
        const session = sessionStorage.getItem("music");
        if(session == null) 
            return null;
        if(stringToBoolean(session)){
            return true;
        }
        return false;
    });
    useEffect(() => {
        if (enableSounds) {
            playMusicAmbient();
        } else {
            stopMusicAmbient();
        }
    }, [enableSounds])
    function toggleSounds(data: boolean) {
        setEnableSounds(data);
        sessionStorage.setItem("music", String(data));
    }
    return (
        <MusicContext.Provider value={{ enableSounds, toggleSounds }}>
            {children}
        </MusicContext.Provider>
    )
}

export const useMusic = () => {
    const context = useContext(MusicContext);
    if (!context) {
        throw new Error("useMusic só pode ser usado atraves do uso do MusicProvider");
    }
    return context;
}