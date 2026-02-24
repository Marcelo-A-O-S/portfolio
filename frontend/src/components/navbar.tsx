"use client"
import Link from "next/link"
import { MouseEvent, useEffect, useRef, useState } from "react";
import Image from "next/image";
import ItemLink, { LinkProps } from "./item-link";
import { ArrowUpRight, Github, Instagram, Linkedin, Volume2, VolumeX } from "lucide-react";
import gsap from "gsap";
import { useMusic } from "@/contexts/MusicContext";
import { useSound } from "@/hooks/useSound";
import LoginForm from "./login-form";
import { FaXTwitter } from "react-icons/fa6";
const links: LinkProps[] = [{
    imgString: "/assets/image_1.jpg",
    linkName: "Home",
    pathName: "/"
}, {
    imgString: "/assets/image_1.jpg",
    linkName: "About",
    pathName: "/about"
}, {
    imgString: "/assets/image_1.jpg",
    linkName: "Projects",
    pathName: "/projects"
}, {
    imgString: "/assets/image_1.jpg",
    linkName: "Certificates",
    pathName: "/certificates"
}]
export default function Navbar() {
    const { enableSounds, toggleSounds } = useMusic();
    const { playMenuClose, playMenuOpen, playLinkHover } = useSound();
    const imgPrevRef = useRef<HTMLImageElement | null>(null);
    const menuOverlayRef = useRef<HTMLDivElement | null>(null);
    const menuContentRef = useRef<HTMLDivElement | null>(null);
    const [imagePreview, setImagePreview] = useState("/assets/image_1.jpg");
    const [isOpen, setIsOpen] = useState(false);

    useEffect(() => {
        if (!menuOverlayRef.current || !menuContentRef.current) return;
        if (isOpen) {
            gsap.to(menuContentRef.current, {
                x: 0,
                y: 0,
                scale: 1,
                rotate: 0,
                opacity: 1,
                duration: 1.5,
                ease: "power4.inOut"
            })
            gsap.to(menuOverlayRef.current, {
                clipPath: "polygon(0% 0%, 100% 0%, 100% 175%, 0% 100%)",
                duration: 1.25,
                ease: "power4.inOut"
            })
        } else {
            gsap.to(menuContentRef.current, {
                x: -50,
                y: -50,
                scale: 1.5,
                rotate: -15,
                opacity: 0.25,
                duration: 1.5,
                ease: "power4.inOut"
            })
            gsap.to(menuOverlayRef.current, {
                clipPath: "polygon(0% 0%, 100% 0%, 100% 0%, 0% 0%)",
                duration: 1.25,
                ease: "power4.inOut"
            })
        }
    }, [isOpen])
    const closeMenu = () => {
        setIsOpen(false);
    }
    const UpdateImage = (e: MouseEvent<HTMLAnchorElement>) => {
        if (!isOpen) return;
        const imgSrc = e.currentTarget.getAttribute("data-image");
        if (!imgSrc) return;
        playLinkHover();
        setImagePreview(imgSrc);
        if (!imgPrevRef.current) return;
        gsap.fromTo(imgPrevRef.current, {
            rotate: 15,
            scale: 1.5,
            duration: 1,
            ease: "power4.inOut"
        }, {
            rotation: 0,
            scale: 1
        })
    }
    return (
        <>
            <nav className="fixed inset-x-0 top-0 z-20 flex justify-center">
                <div className="w-full max-w-[1440px] flex justify-between items-center p-10">
                    <div id="logo">
                        <Link href={"/"} className="relative text-lg font-semibold uppercase">Marcelo.Dev</Link>
                    </div>
                    <div className="relative flex gap-3 items-center justify-center">
                        <div className="flex items-center justify-center cursor-pointer" onClick={() =>{
                            if(enableSounds == null) return;
                            toggleSounds(!enableSounds);
                        }}>
                            {enableSounds? <Volume2/> :<VolumeX/>}
                        </div>
                        <div className="flex items-center justify-center cursor-pointer">
                            <LoginForm/>
                        </div>
                        <div id="menu-toggle" className="flex items-center justify-center cursor-pointer" onClick={() => {
                            setIsOpen(!isOpen);
                            if (!enableSounds) return;
                            if (isOpen) {
                                playMenuOpen();
                            } else {
                                playMenuClose()
                            }
                        }}>
                            <p className="text-lg font-semibold">
                                {isOpen? "Close": "Menu"}
                            </p>
                            
                        </div>
                    </div>
                </div>
            </nav>
            <div ref={menuOverlayRef} id="menu-overlay"
                style={{
                    clipPath: "polygon(0% 0%, 100% 0%, 100% 0%, 0% 0%)"
                }}
                className={`fixed mx-auto inset-0 w-screen max-w-[1440px] h-svh z-10 bg-white dark:bg-black`}>
                <div ref={menuContentRef} id="menu-content" className="relative w-full h-full flex justify-center items-center origin-bottom-left will-change-transform">
                    <div id="menu-items" className="w-full p-10 flex gap-10">
                        <div id="col-preview" className="flex-3 flex justify-center items-center">
                            <div id="menu-preview-img" className="relative h-[45%] w-[45%] overflow-hidden">
                                <Image ref={imgPrevRef} src={imagePreview} alt="" fill priority className="absolute will-change-transform w-full h-full object-cover" />
                            </div>
                        </div>
                        <div id="col-links" className="flex-2 flex flex-col gap-10 p-10 px-0">
                            <div id="menu-links" className="flex flex-col items-start">
                                {links.map((link, index) => (
                                    <ItemLink
                                        key={index}
                                        pathName={link.pathName}
                                        imgString={link.imgString}
                                        linkName={link.linkName}
                                        isOpen={isOpen}
                                        index={index}
                                        updateImage={UpdateImage}
                                        onPush={closeMenu} />
                                ))}
                            </div>
                            <div id="menu-socials" className="flex flex-col items-start">
                                <div className="social pb-1.5 relative inline-block overflow-hidden">
                                    <Link className="relative flex items-center text-lg font-normal tracking-tighter gap-1" href={""}>
                                        <Github />Github<ArrowUpRight />
                                    </Link>
                                </div>
                                <div className="social pb-1.5 relative inline-block overflow-hidden">
                                    <Link className="relative flex items-center text-lg font-normal tracking-tighter gap-1" href={""}>
                                        <Linkedin />Linkedln<ArrowUpRight />
                                    </Link>
                                </div>
                                <div className="social pb-1.5 relative inline-block overflow-hidden">
                                    <Link className="relative flex items-center text-lg font-normal tracking-tighter gap-1" href={""}>
                                        <Instagram />Instagram<ArrowUpRight />
                                    </Link>
                                </div>
                                <div className="social pb-1.5 relative inline-block overflow-hidden">
                                    <Link className="relative flex items-center text-lg font-normal tracking-tighter gap-1" href={""}>
                                        <FaXTwitter />Twitter<ArrowUpRight />
                                    </Link>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div id="menu-footer" className="absolute bottom-0 w-full p-10 flex justify-between ">
                        <div className="flex-3">
                            <Link href={""}>Brazil</Link>
                        </div>
                        <div className="flex-2 flex justify-between">
                            <Link className="flex justify-center items-center" href={"mailto:marceloaugustooliveirasoares@gmail.com"}>
                                My Email<ArrowUpRight />
                            </Link>
                            <Link href={""}>
                                © 2026
                            </Link>
                        </div>
                    </div>
                </div>
            </div>
        </>
    )
}