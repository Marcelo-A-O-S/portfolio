"use client"
import Image from "next/image";
import { useEffect, useState } from "react";
import Link from "next/link";
import ImgPerfil from "../assets/imagemperfil.jpg"
import IconInstagram from "../assets/instagram.svg"
import IconFacebook from "../assets/mdi_facebook.svg"
import IconTwitter from "../assets/mdi_twitter.svg"
import IconGithub from "../assets/mdi_github.svg"
import IconLinkedin from "../assets/mdi_linkedin.svg"
export default function Home() {
  const [ closeIntro, setCloseIntro] = useState(false)
  const [ showIntro, setShowIntro] = useState(false)
  const [ showWelcome, setShowWelcome] = useState(false)
  const [isFirstVisit, setIsFirstVisit] = useState(false);
/*   useEffect(()=>{
    setTimeout(()=>{
      setShowIntro(true)
      setTimeout(()=>{
        setShowIntro(false)
        setShowWelcome(true)
        setTimeout(()=>{
          setShowWelcome(false)
          setCloseIntro(true)
        },4000)
      },2600)
    },400)
  },[]) */
  
  return (
    <div className="container mx-auto">
{/*         <div className={`intro ${closeIntro?"intro-close":""} `}>
          <h1 className="logo-header">
            <span className={`${showIntro?"logo-title":"d-none"}`}>Hello, my name is Marcelo Augusto!</span>
            <span className={`${showWelcome?"logo-welcome":"d-none"}`}>Welcome!</span>
          </h1>
        </div> */}
        <section className="flex flex-col content-between p-5 gap-7 my-20 lg:flex-row lg:gap-0 ">
          <div className="flex flex-col">
              <div className="flex flex-col gap-3 items-start w-full p-2">
                  <p className="flex break-words w-full text-2xl" >Hello, I am Marcelo Augusto and I am</p>
                  <h1 className="font-extrabold flex flex-col w-full text-6xl sm:text-8xl" >DEVELOPER <span style={{
                    color:"rgba(20, 12, 108, 1)", 
                  }}>FULL STACK</span></h1>
                  <p className="flex w-10/12 text-2xl" >I am a student and enthusiast with a focus on full stack development in my spare time.</p>
                  <div className="flex gap-3">
                    <Link href={"https://github.com/Marcelo-A-O-S"} target="_blank"><Image src={IconGithub} alt="Github"/></Link>
                    <Link href={"https://www.facebook.com/marcelo.augusto.dev/"} target="_blank"><Image src={IconFacebook} alt="Facebook"/></Link>
                    <Link href={"https://www.instagram.com/marcelo.augusto1234/"} target="_blank"><Image src={IconInstagram} alt="Instagram"/></Link>
                    <Link href={"https://x.com/Telo_Augusto_O"} target="_blank"><Image src={IconTwitter} alt="Twitter"/></Link>
                    <Link href={"https://www.linkedin.com/in/marcelo-augusto-dev/"} target="_blank"><Image src={IconLinkedin} alt="Linkedin"/></Link>
                  </div>
                  <button className="rounded-3xl text-2xl" style={{
                    backgroundColor:"rgba(20, 12, 108, 1)",
                    height:"80px",
                    width:"216px",
                    }}>About me</button>
              </div>
          </div>
          <div className="flex justify-center w-full lg:justify-end">
                <img className="flex h-full" src={ImgPerfil.src}  style={{
                  borderRadius:"167px",
                  width:"479px",
                }} alt=""/>
          </div>
        </section>
        <section className="">
          <div>
          </div>
          <div>
          </div>
        </section>
    </div>
  );
}
