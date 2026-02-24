"use client";
import { useMusic } from "@/contexts/MusicContext";
import gsap from "gsap";
import { LoaderCircle } from "lucide-react";
import { usePathname } from "next/navigation";
import { useEffect, useRef, useState } from "react";

export default function SplashScreen() {
    const pathName = usePathname();
    const isHome = pathName === "/";
    const { toggleSounds, enableSounds } = useMusic();
    const splashScreenRef = useRef<HTMLDivElement | null>(null);
    const authDialogRef = useRef<HTMLDialogElement | null>(null);
    const countRef = useRef<HTMLDivElement | null>(null);
    const spinnerRef = useRef<HTMLDivElement | null>(null);
    const [openDialog, setOpenDialog] = useState(false);

    useEffect(() => {
        if (sessionStorage.getItem("splashShown")) {
            gsap.set(splashScreenRef.current, { 
                bottom: "100%",
                onComplete:()=>{
                    
                } 
            });
            return;
        }
        sessionStorage.setItem("splashShown", "true");
        if (!splashScreenRef.current || !authDialogRef.current || !countRef.current || !spinnerRef.current) return;
        gsap.to(authDialogRef.current, {
            y: 50
        })
        gsap.fromTo(spinnerRef.current, {
            rotate: "0deg"
        }, {
            rotate: "360deg",
            repeat: -1
        })
        const counter = { value: 0 }
        gsap.to(counter, {
            value: 100,
            duration: 2.5,
            ease: "power3.out",
            onUpdate: () => {
                if (countRef.current) {
                    countRef.current.innerText = Math.floor(counter.value).toString();
                }
            },
            onComplete: () => {
                if (enableSounds === null) {
                    setOpenDialog(true);
                    gsap.to(authDialogRef.current, {
                        opacity: 1,
                        y: 0,
                        duration: 1,
                        delay: 0.3,
                        ease: "power4.inOut"
                    })
                } else {
                    toggleSounds(enableSounds)
                    gsap.to(splashScreenRef.current, {
                        bottom: "100%",
                        ease: "power4.inOut",
                    })
                }
            }
        })
    }, [])
    const handleAction = (data: boolean) => {
        if (!authDialogRef.current) return;
        toggleSounds(data);
        gsap.to(authDialogRef.current, {
            opacity: 0,
            y: 50,
            duration: 0.6,
            ease: "power4.inOut",
            onComplete: () => {
                setOpenDialog(false)
            }
        })
        gsap.to(splashScreenRef.current, {
            bottom: "100%",
            ease: "power4.inOut",
        })
    }
    return (
        <>
            <dialog ref={authDialogRef} className="hidden open:flex fixed left-1/2 opacity-0 -translate-x-1/2 -translate-y-1/2 z-30 items-center justify-center bg-transparent bottom-0" open={openDialog}>
                <div className="relative h-full w-full border-2 rounded-2xl 
             border-black dark:border-white bg-white dark:bg-black p-6">
                    <div className="flex flex-col">
                        <p className="text-sm text-black dark:text-white">Você permite a execução de efeitos sonoros?</p>
                        <p className="text-sm text-black dark:text-white">Essa execução tem o intuito de aumentar a imersão ao projeto.</p>
                    </div>
                    <div className="flex justify-center gap-4 mt-4">
                        <button className="border-2 border-black dark:border-white text-black dark:text-white px-4 py-2 rounded-sm cursor-pointer" onClick={() => handleAction(false)}>Recusar</button>
                        <button className="border-2 border-black dark:border-white text-black dark:text-white px-4 py-2 rounded-sm cursor-pointer" onClick={() => handleAction(true)}>Aceitar</button>
                    </div>
                </div>
            </dialog>
            <div ref={splashScreenRef} className="fixed mx-auto w-screen max-w-[1440px] h-svh z-20 flex flex-col justify-center bg-white dark:bg-black">
                <div className="relative flex justify-between w-full p-10">
                    <p className="leading-none text-sm md:text-lg font-semibold uppercase tracking-tighter">Marcelo.dev</p>
                </div>
                <div className="relative flex flex-col h-full justify-center items-center">
                    <div ref={countRef} className="text-9xl font-semibold">
                        0
                    </div>
                    <div className="flex gap-2 items-center justify-center">
                        <div className="leading-none text-lg font-semibold uppercase tracking-tighter text-black dark:text-white">
                            Loading
                        </div>
                        <div ref={spinnerRef} className="">
                            <LoaderCircle />
                        </div>
                    </div>
                </div>
                <div className="relative bottom-0 flex justify-end w-full p-10">
                    <p className="leading-none text-lg font-semibold uppercase tracking-tighter">Introduction</p>
                </div>
            </div>
        </>
    )
}